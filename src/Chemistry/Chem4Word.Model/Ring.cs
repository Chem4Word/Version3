// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Model.Enums;
using Chem4Word.Model.Geometry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace Chem4Word.Model
{
    /// <summary>
    /// Represents an undordered ring of atoms
    /// </summary>
    [DebuggerDisplay("Atoms: {Atoms.Count} Priority: {Priority}")]
    public class Ring : IComparer<Ring>
    {
        //private Point? _centroid;

        public int Priority
        {
            get
            {
                int result = -1;

                if (Atoms != null)
                {
                    switch (Atoms.Count)
                    {
                        case 6:
                            result = 1;
                            break;

                        case 5:
                            result = 2;
                            break;

                        case 7:
                            result = 3;
                            break;

                        case 4:
                            result = 4;
                            break;

                        case 3:
                            result = 5;
                            break;

                        case 8:
                            result = 6;
                            break;

                        case 9:
                            result = 7;
                            break;

                        default:
                            result = -1;
                            break;
                    }
                }

                return result;
            }
        }

        #region properties

        /// <summary>
        /// Collection of atoms that go to make up the ring
        /// </summary>
        public ObservableCollection<Atom> Atoms { get; }

        /// <summary>
        /// Returns a dynamically inferred enumerable of ring bonds
        /// </summary>
        public IEnumerable<Bond> Bonds
        {
            get
            {
                HashSet<Bond> sofar = new HashSet<Bond>();
                //get back a list of lists of all those bonds shared between a ring atom and its ring neighbours
                var ringbonds = from a in Atoms
                                select new
                                {
                                    Bondlist = (
                                        from n in a.Neighbours
                                        where Atoms.Contains(n)
                                        select new { Bond = a.BondBetween(n) }
                                        )
                                }
                      ;

                //and then flatten it
                //I HATE selectmany!
                return ringbonds.SelectMany(a => a.Bondlist).Select(b => b.Bond).Distinct();
            }
        }

        /// <summary>
        /// For the given Bond object determines how best to place its double bond line
        ///  </summary>
        /// <param name="b">Bond object (should be part of the ring)</param>
        /// <returns>BondDirection shoing how to place the bond</returns>
        public BondDirection InternalPlacement(Bond b)
        {
            Point? center = this.Centroid;
            if (center != null)
            {
                Vector toCenter = center.Value - b.StartAtom.Position;
                Vector bv = b.BondVector;
                if (Vector.AngleBetween(toCenter, bv) > 0)
                    return BondDirection.Clockwise;
                else
                {
                    return BondDirection.Anticlockwise;
                }
            }
            else
            {
                return BondDirection.None;
            }
        }

        /// <summary>
        /// Molecule that contains ring.
        ///  </summary>
        /// <remarks>Do NOT set explicitly.  Add or remove the ring from a Molecule</remarks>
        public Molecule Parent { get; set; }

        /// <summary>
        /// Obtains the centroid of the ring.
        ///  </summary>
        /*public System.Windows.Point? Centroid
        {
            get
            {
                if (_centroid == null)
                {
                    _centroid = Geometry<Atom>.GetCentroid(Traverse().ToArray(), atom => atom.Position);
                }
                return _centroid;
            }
        }
        */

        public System.Windows.Point? Centroid
        {
            get
            {
                return Geometry<Atom>.GetCentroid(Traverse().ToArray(), atom => atom.Position);
            }
        }

        public List<Atom> ConvexHull
        {
            get
            {
                var atomList = from Atom a in this.Atoms
                               orderby a.Position.X ascending, a.Position.Y descending
                               select a;

                return Geometry<Atom>.GetHull(atomList, atom => atom.Position);
            }
        }

        public List<Bond> DoubleBonds => Bonds.Where(b => (b.Order == Bond.OrderSingle | b.Order == Bond.OrderAromatic)).ToList();

        //generates a unique ID for each ring based on the atom hash codes()
        public string UniqueID
        {
            get
            {
                return Atoms.Select(a => a.GetHashCode().ToString()).OrderBy(hc => hc).Aggregate((s, hc) => s + "|" + hc);
            }
        }

        /// <summary>
        /// Circles a ring
        /// </summary>
        /// <param name="start">Atom to start at</param>
        /// <param name="anticlockwise">Which direction to go in</param>
        /// <returns>IEnumerable&lt;Atom&gt; that interates through the ring</returns>
        public IEnumerable<Atom> Traverse(Atom start = null,
            Enums.BondDirection direction = BondDirection.Anticlockwise)
        {
            HashSet<Atom> res = new HashSet<Atom>();
            res.Add(start);
            Atom next;
            if (start == null)
                start = Atoms[0];

            //start with the start atom, and find the other two adjacent atoms that are part of the ring
            var adj = from n in start.Neighbours
                      where n.Rings.Contains(this)
                      select n;
            var nextatoms = adj.ToArray();

            Debug.Assert(nextatoms.Count() == 2);

            Vector v1 = nextatoms[0].Position - start.Position;

            Vector v2 = nextatoms[1].Position - start.Position;

            //make sure a positive angle is the direction in which we want to travel
            //multiply the angle by the direction to choose the correct atom
            double angle = (int)direction * Vector.AngleBetween(v1, v2);

            if (angle > 0)
            {
                next = nextatoms[0];
            }
            else
            {
                next = nextatoms[1];
            }
            //circle the ring, making sure we ignore atoms we've visited already
            while (next != null)
            {
                yield return next;
                res.Add(next);
                var candidates = next.NeighbourSet; //get the set of atoms around the next atom
                //get rid of all the atoms NOT in the ring or already in the set
                candidates.RemoveWhere(a => res.Contains(a) || !this.Atoms.Contains(a));
                next = candidates.FirstOrDefault();
            }
        }

        #endregion properties

        #region Constructors

        public Ring()
        {
            Atoms = new ObservableCollection<Atom>();

            Atoms.CollectionChanged += Atoms_CollectionChanged;
        }

        public Ring(HashSet<Atom> ringAtoms) : this()
        {
            foreach (Atom atom in ringAtoms)
            {
                Atoms.Add(atom);
            }
        }

        private void Atoms_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Atom atom in e.NewItems)
                    {
                        if (!atom.Rings.Contains(this))
                            atom.Rings.Add(this);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (Atom atom in e.NewItems)
                    {
                        if (atom.Rings.Contains(this))
                            atom.Rings.Remove(this);
                    }
                    break;
            }
        }

        #endregion Constructors

        #region Operators

        /// <summary>
        /// Combines two rings to make a bigger ring
        /// doesn't include bridging atoms
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>New ring wich encompasses both original rings</returns>
        public static Ring operator +(Ring a, Ring b)
        {
            HashSet<Atom> setA = new HashSet<Atom>(a.Atoms);
            HashSet<Atom> setB = new HashSet<Atom>(b.Atoms);
            setA.SymmetricExceptWith(setB);
            return new Ring(setA);
        }

        public int Compare(Ring x, Ring y)
        {
            return String.Compare(x?.UniqueID, y?.UniqueID, StringComparison.Ordinal);
        }

        #endregion Operators
    }
}