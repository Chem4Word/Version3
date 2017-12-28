// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace Chem4Word.Model
{
    /// <summary>
    /// DO NOT USE THIS MODULE.  THIS IS WORK IN PROGRESS!
    /// </summary>
    public partial class Molecule
    {
        #region Chirality

        /// <summary>
        /// carrier type for placing in queues
        /// </summary>
        public class CipData : AtomData
        {
            public Atom RootAtom { get; set; }

            public static bool operator <(CipData a, CipData b)
            {
                if ((a.CurrentAtom.Element as Element).AtomicNumber < (b.CurrentAtom.Element as Element).AtomicNumber)
                {
                    return true;
                }
                else if ((a.CurrentAtom.Element as Element).AtomicNumber > (b.CurrentAtom.Element as Element).AtomicNumber)
                {
                    return false;
                }
                else if ((a.CurrentAtom.Element as Element).AtomicNumber == (b.CurrentAtom.Element as Element).AtomicNumber)
                {
                    if ((a.CurrentAtom.Element.AtomicWeight < b.CurrentAtom.Element.AtomicWeight))
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }

            public static bool operator >(CipData a, CipData b)
            {
                if ((a.CurrentAtom.Element as Element).AtomicNumber > (b.CurrentAtom.Element as Element).AtomicNumber)
                {
                    return true;
                }
                else if ((a.CurrentAtom.Element as Element).AtomicNumber < (b.CurrentAtom.Element as Element).AtomicNumber)
                {
                    return false;
                }
                else if ((a.CurrentAtom.Element as Element).AtomicNumber == (b.CurrentAtom.Element as Element).AtomicNumber)
                {
                    if ((a.CurrentAtom.Element.AtomicWeight > b.CurrentAtom.Element.AtomicWeight))
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
        }

        /// <summary>
        /// Allows for interrogation of an atom's environment in bands
        /// </summary>
        public class CipBandQueue
        {
            private Queue<CipData> _queueA, _queueB;
            private Queue<CipData> _currentQueue, _nextQueue;

            public CipBandQueue()
            {
                _queueA = new Queue<CipData>();
                _queueB = new Queue<CipData>();

                _currentQueue = _queueA;
                _nextQueue = _queueB;
            }

            private void SwapQueues()
            {
                if (_currentQueue == _queueA)
                {
                    _currentQueue = _queueB;
                    _nextQueue = _queueA;
                }
                else
                {
                    _currentQueue = _queueA;
                    _nextQueue = _queueB;
                }
            }

            public void Enqueue(CipData data)
            {
                _nextQueue.Enqueue(data);
            }

            public CipData Dequeue()
            {
                return _currentQueue.Dequeue();
            }

            public int Count => _currentQueue.Count;

            public bool Any => Count > 0;

            public void NextBand()
            {
                SwapQueues();

                _nextQueue.Clear();
            }
        }

        public bool DeterminePriorities(out Dictionary<Atom, int> priorities, params Atom[] startAtoms)
        {
            //the band manager swaps queues from band to band
            CipBandQueue queueManager = new CipBandQueue();
            //this keeps a list of priorities as assigned to the root atoms.
            Dictionary<Atom, int> rootPriority = new Dictionary<Atom, int>();
            //keep track of atoms we've already visited so far:  this helps us to locate cycles
            HashSet<Atom> visitedAtoms = new HashSet<Atom>();

            //we scan outward from the root atom in concentric bands.
            //at the end of each band we check for distinctness

            //add the children of the start atoms to the first band,
            foreach (Atom startAtom in startAtoms)
            {
                foreach (Atom atom in startAtom.NeighboursExcept(startAtoms))
                {
                    CipData data = new CipData
                    {
                        RootAtom = atom,
                        CurrentAtom = atom,
                        Source = startAtom
                    };

                    //create a singleton list for each atom
                    List<CipData> newAtomList = new List<CipData> { data };
                    //set the p of each atom to zero
                    rootPriority[atom] = 0;

                    queueManager.Enqueue(data);
                }
            }

            //check there are four immediate first-generation children - if not, throw an exception

            if (queueManager.Count != 4)
            {
                throw new ArgumentException("Number of neigbouring atoms does not equal 4.");
            }

            //while we have no distinct set of CIP priorities - and there are plenty more atoms
            while (queueManager.Any)
            {
                Dictionary<Atom, List<CipData>> currentBandData = new Dictionary<Atom, List<CipData>>();
                //retrieve the current atom band
                while (queueManager.Any)
                {
                    var parentAtomdata = queueManager.Dequeue();
                    //add the child atoms to the next atom band
                    foreach (Atom childAtom in parentAtomdata.CurrentAtom.NeighboursExcept(parentAtomdata.Source))
                    {
                        //if we've visited the atom, add a ghost atom as it's a cycle
                        if (!visitedAtoms.Contains(childAtom))
                        {
                            //mark each atom as visited
                            visitedAtoms.Add(childAtom);

                            queueManager.Enqueue(new CipData
                            {
                                CurrentAtom = childAtom,
                                RootAtom = parentAtomdata.RootAtom,
                                Source = parentAtomdata.CurrentAtom
                            });
                            //if we've a multiple bond, add n-1 ghost atoms
                            for (double i = 2.0;
                                i <= (parentAtomdata.Source.BondBetween(childAtom).OrderValue ?? 1.0);
                                i++)
                            {
                                AddDummyAtom(childAtom, queueManager, parentAtomdata);
                            }
                        }
                        else
                        {
                            //create a dummy atom identical to the original atom
                            AddDummyAtom(childAtom, queueManager, parentAtomdata);
                        }
                        //and add it to thelookup
                        if (!currentBandData.ContainsKey(parentAtomdata.RootAtom))
                        {
                            currentBandData[parentAtomdata.RootAtom] = new List<CipData>();
                        }
                        currentBandData[parentAtomdata.RootAtom].Add(parentAtomdata);
                    }
                    //we've now run out of atoms from this band
                    //so rank them
                    Dictionary<Atom, int> nextPriorities = RankPriorities(currentBandData);
                    //now add them to the cumulative priorities

                    foreach (var root in nextPriorities.Keys)
                    {
                        rootPriority[root] += nextPriorities[root];
                    }

                    //if the cumulative priorities are all distinct

                    if (rootPriority.Values.Distinct().Count() == rootPriority.Values.Count)
                    {
                        //return the list of priorities by atoms
                        priorities = rootPriority;
                        return true;
                    }
                    else
                    //multiply all the cumulative priorities by four
                    {
                        foreach (Atom root in rootPriority.Keys)
                        {
                            rootPriority[root] *= 4;
                        }
                        //and swap the queues
                        queueManager.NextBand();
                    }
                }
                //end while
            }
            //if we've got here we've run out of atoms still don't have a set of distinct priorities.
            priorities = null;
            return false;
        }

        /// <summary>
        /// adds a dummy atom to the current queue manager which duplicates the atom in question
        /// </summary>
        /// <param name="childAtom">atom to duplicate</param>
        /// <param name="queueManager">what it says</param>
        /// <param name="parentAtomdata">data about the current atom on the queue</param>
        private static void AddDummyAtom(Atom childAtom, CipBandQueue queueManager, CipData parentAtomdata)
        {
            Atom dummy = new Atom
            {
                Element = childAtom.Element,
                IsotopeNumber = childAtom.IsotopeNumber
            };
            //and shove it on the queue

            queueManager.Enqueue(new CipData
            {
                CurrentAtom = dummy,
                RootAtom = parentAtomdata.RootAtom,
                Source = parentAtomdata.CurrentAtom
            });
        }

        ///
        ///
        ///
        private Dictionary<Atom, int> RankPriorities(Dictionary<Atom, List<CipData>> unrankedAtoms)
        {
            Dictionary<Atom, int> retval = new Dictionary<Atom, int>();

            //sort each list of atoms ascendingly
            foreach (KeyValuePair<Atom, List<CipData>> unrankedAtom in unrankedAtoms)
            {
                var sortedX = (from cipX in unrankedAtom.Value
                               orderby (cipX.CurrentAtom.Element as Element).AtomicNumber descending,
                               cipX.CurrentAtom.Element.AtomicWeight descending
                               select cipX).ToList();
                unrankedAtoms[unrankedAtom.Key] = sortedX;
            }

            //now sort the dictionary

            int rank = unrankedAtoms.Count - 1;
            int lastrank = -1;
            var lastAtomList = new List<CipData>();
            foreach (var rootList in unrankedAtoms.OrderByDescending(ua => ua.Value))
            {
                if (rootList.Value == lastAtomList)
                {
                    retval[rootList.Key] = lastrank;
                }
                else
                {
                    retval[rootList.Key] = rank;
                    lastrank = rank;
                }

                lastAtomList = rootList.Value;
                rank--;
            }

            return retval;
        }

        public class CipListComparer : Comparer<List<CipData>>
        {
            public override int Compare(List<CipData> x, List<CipData> y)
            {
                int maxSharedLength = Math.Min(x.Count, y.Count);
                int i = 0;
                while (i < maxSharedLength && x[i] == y[i])
                {
                    i++;
                }

                if (i >= maxSharedLength)
                {
                    //we ran off the end of one list
                    if (x.Count < y.Count) //the shorter lost is the lesser
                    {
                        return -1;
                    }
                    else if (x.Count == y.Count) //both lists are identical
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    //we broke out of the loop before we got to the end
                    if (x[i] < y[i])
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
        }

        #endregion Chirality
    }
}