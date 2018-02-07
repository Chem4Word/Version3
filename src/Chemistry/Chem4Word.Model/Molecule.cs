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
    public partial class Molecule : ChemistryContainer
    {
        #region Fields

        private Rect _boundingBox = Rect.Empty;

        public Rect BoundingBox
        {
            get
            {
                if (_boundingBox == Rect.Empty)
                {
                    CalculateBoundingBox();
                }
                return _boundingBox;
            }
        }

        private void CalculateBoundingBox()
        {
            var xMax = Atoms.Select(a => a.Position.X).Max();
            var xMin = Atoms.Select(a => a.Position.X).Min();

            var yMax = Atoms.Select(a => a.Position.Y).Max();
            var yMin = Atoms.Select(a => a.Position.Y).Min();

            const double padding = 50;

            double xDiff = Math.Abs(xMax - xMin);
            if (xDiff < padding)
            {
                xMin = xMin - padding / 2 - xDiff;
                xMax = xMax + padding / 2 + xDiff;
            }
            double yDiff = Math.Abs(yMax - yMin);
            if (yDiff < padding)
            {
                yMin = yMin - padding / 2 - yDiff;
                yMax = yMax + padding / 2 - yDiff;
            }

            _boundingBox = new Rect(new Point(xMin, yMin), new Point(xMax, yMax));
        }

        public string ConciseFormula { get; set; }

        public List<string> Warnings { get; set; }
        public List<string> Errors { get; set; }

        #endregion Fields

        #region Constructors

        /// <summary>
        ///
        /// </summary>
        public Molecule()
        {
            Atoms = new ObservableCollection<Atom>();
            Bonds = new ObservableCollection<Bond>();
            Rings = new ObservableCollection<Ring>();

            ChemicalNames = new ObservableCollection<ChemicalName>();
            Formulas = new ObservableCollection<Formula>();

            Atoms.CollectionChanged += Atoms_CollectionChanged;
            Bonds.CollectionChanged += BondsOnCollectionChanged;
            Rings.CollectionChanged += RingsOnCollectionChanged;

            Warnings = new List<string>();
            Errors = new List<string>();
        }

        /// <summary>
        /// Generates a molecule from a given atom
        /// </summary>
        /// <param name="seed"></param>
        public Molecule(Atom seed) : this()
        {
            Refresh(seed);
        }

        #endregion Constructors

        public string CalculatedFormula()
        {
            string result = "";

            Dictionary<string, int> f = new Dictionary<string, int>();
            SortedDictionary<string, int> r = new SortedDictionary<string, int>();

            f.Add("C", 0);
            f.Add("H", 0);

            foreach (Atom atom in Atoms)
            {
                // ToDo: Do we need to check if this is a functional group here?

                switch (atom.Element.Symbol)
                {
                    case "C":
                        f["C"]++;
                        break;

                    case "H":
                        f["H"]++;
                        break;

                    default:
                        if (!r.ContainsKey(atom.SymbolText))
                        {
                            r.Add(atom.SymbolText, 1);
                        }
                        else
                        {
                            r[atom.SymbolText]++;
                        }
                        break;
                }

                int hCount = atom.ImplicitHydrogenCount;
                if (hCount > 0)
                {
                    f["H"] += hCount;
                }
            }

            foreach (KeyValuePair<string, int> kvp in f)
            {
                if (kvp.Value > 0)
                {
                    result += $"{kvp.Key} {kvp.Value} ";
                }
            }
            foreach (KeyValuePair<string, int> kvp in r)
            {
                result += $"{kvp.Key} {kvp.Value} ";
            }

            return result.Trim();
        }

        /// <summary>
        /// Rebuilds the molecule without trashing it totally
        /// </summary>
        /// <param name="seed">start Atom for refresh operation</param>
        private void Refresh(Atom seed = null)
        {
            //keep a list of the atoms to refer to later when rebuilding
            List<Atom> checklist = new List<Atom>();
            //set the parent to null but keep a list of all atoms
            foreach (Atom atom in Atoms)
            {
                atom.Parent = null;
                checklist.Add(atom);
            }

            //clear the associated collections
            Atoms.RemoveAll();
            AllAtoms.RemoveAll();
            foreach (Bond bond in Bonds)
            {
                bond.Parent = null;
            }
            Bonds.RemoveAll();
            AllBonds.RemoveAll();
            Rings.RemoveAll();

            //if we've been provided with a seed atom, use that
            //else use the first atom in the checklist

            if (seed == null)
            {
                seed = checklist[0];
            }
            //now traverse the tree as far as it will go

            DepthFirstTraversal(seed,
                operation: atom =>
                    {
                        Atoms.Add(atom);

                        foreach (Bond b in atom.Bonds.Where(b => b.Parent == null))
                        {
                            Bonds.Add(b);
                        }
                        if (checklist.Contains(atom))
                        {
                            checklist.Remove(atom);
                        }
                    },
                isntProcessed: atom => atom.Parent == null);
            //only if the molecule has rings do we rebuild it
            RebuildRings();

            //now we check to see whether there are any more unconnected regions lurking around

            while (checklist.Count > 0)//means we haven't yet accounted for all the original atoms
            {
                seed = checklist[0];
                Molecule addnlMol = new Molecule();
                DepthFirstTraversal(seed,
                   operation: atom =>
                   {
                       addnlMol.Atoms.Add(atom);

                       foreach (Bond b in atom.Bonds.Where(b => b.Parent == null))
                       {
                           addnlMol.Bonds.Add(b);
                       }
                       if (checklist.Contains(atom))
                       {
                           checklist.Remove(atom);
                       }
                   },
                   isntProcessed: atom => atom.Parent == null);
                //only if the molecule has rings do we rebuild it
                addnlMol.RebuildRings();
                this.Parent.Molecules.Add(addnlMol);
            }
        }

        /// <summary>
        /// rebuilds the molecule without trashing it
        /// </summary>
        public void Refresh()
        {
            Atom start = this.Atoms[0];
            Refresh(start);
        }

        #region Properties

        public ObservableCollection<ChemicalName> ChemicalNames { get; }

        public ObservableCollection<Bond> Bonds { get; }

        public ObservableCollection<Ring> Rings { get; }

        public ObservableCollection<Atom> Atoms { get; }

        //aggregating collections:
        //metadata
        public ObservableCollection<Formula> Formulas { get; }

        /// <summary>
        /// Returns a snapshot of operations performed on each atom
        /// </summary>
        /// <param name="getproperty">Delegate or lambda function which should return a value to store against atom. </param>
        /// <returns></returns>
        public Dictionary<Atom, T> Projection<T>(Func<Atom, T> getproperty)
        {
            return Atoms.ToDictionary(a => a, a => getproperty(a));
        }

        public int FormalCharge { get; set; }
        public MoleculeRole Role { get; set; }

        /// <summary>
        /// Must be set if a molecule is a child of another.
        /// </summary>
        ///
        private int? _count;

        public int? Count
        {
            get { return _count; }

            set
            {
                if (value != null)
                {
                    if (Parent == null)
                    {
                        throw new ArgumentOutOfRangeException("Not allowed to set Count on a top-level molecule");
                    }
                }
                _count = value;
            }
        }

        #endregion Properties

        #region Methods

        #region Graph Stuff

        /// <summary>
        /// Traverses a molecular graph applying an operation to each and every atom.
        /// Does not require that the atoms be already part of a Molecule.
        /// </summary>
        /// <param name="startAtom">start atom</param>
        /// <param name="operation">delegate pointing to operation to perform</param>
        /// <param name="isntProcessed"> Predicate test to tell us whether or not to process an atom</param>
        private void DepthFirstTraversal(Atom startAtom, Action<Atom> operation, Predicate<Atom> isntProcessed)
        {
            operation(startAtom);

            while (startAtom.UnprocessedDegree(isntProcessed) > 0)
            {
                if (startAtom.UnprocessedDegree(isntProcessed) == 1)
                {
                    startAtom = NextUnprocessedAtom(startAtom, isntProcessed);
                    operation(startAtom);
                }
                else
                {
                    var unassignedAtom = from a in startAtom.Neighbours
                                         where isntProcessed(a)
                                         select a;
                    foreach (Atom atom in unassignedAtom)
                    {
                        DepthFirstTraversal(atom, operation, isntProcessed);
                    }
                }
            }
        }

        /// <summary>
        /// Cleaves off a degree 1 atom from the working set.
        /// Reduces the adjacent atoms' degree by one
        /// </summary>
        /// <param name="toPrune">Atom to prune</param>
        /// <param name="workingSet">Dictionary of atoms</param>
        private static void PruneAtom(Atom toPrune, Dictionary<Atom, int> workingSet)
        {
            foreach (var neighbour in toPrune.Neighbours)
            {
                if (workingSet.ContainsKey(neighbour))
                    workingSet[neighbour] -= 1;
            }
            workingSet.Remove(toPrune);
        }

        /// <summary>
        /// Removes side chain atoms from the working set
        /// DOES NOT MODIFY the original molecule!
        /// Assumes we don't have any degree zero atoms
        /// (i.e this isn't a single atom Molecule)
        /// </summary>
        /// <param name="projection">Molecule to prune</param>
        public static void PruneSideChains(Dictionary<Atom, int> projection)
        {
            bool hasPruned = true;

            while (hasPruned)
            {
                //clone the working set atoms first because otherwise LINQ will object

                Atom[] atomList = (from kvp in projection
                                   where kvp.Value < 2
                                   select kvp.Key).ToArray();
                hasPruned = atomList.Length > 0;

                foreach (Atom a in atomList)
                {
                    PruneAtom(a, projection);
                }
            }
        }

        #endregion Graph Stuff

        #region Ring stuff

        /// <summary>
        /// How many rings the molecule contains.  Non cyclic molecules ALWAYS have atoms = bonds +1
        /// </summary>
        public int TheoreticalRings => Bonds.Count - Atoms.Count + 1;

        /// <summary>
        /// If the molecule has more atoms than bonds, it doesn't have a ring. Test this before running ring perception.
        /// </summary>
        public bool HasRings => TheoreticalRings > 0;

        /// <summary>
        /// Quick check to see if the rings need recalculating
        /// </summary>
        public bool RingsCalculated
        {
            get
            {
                if (!HasRings)
                {
                    return true;  //don't bother recaculating the rings for a linear molecule
                }
                else
                {
                    return Rings.Count > 0; //we have rings present, so why haven't we calculated them?
                }
            }
        }//have we calculated the rings yet?

        public void RebuildRings()
        {
#if DEBUG
            Stopwatch sw = new Stopwatch();
            sw.Start();
#endif
            if (HasRings)
            {
                WipeMoleculeRings();

                //working set of atoms
                //it's a dictionary, because we initially store the degree of each atom against it
                //this will change as the pruning operation kicks in
                Dictionary<Atom, int> workingSet = Projection(a => a.Degree);
                //lop off any terminal branches
                PruneSideChains(workingSet);

                while (workingSet.Any()) //do we have any atoms left in the set
                {
                    var startAtom = workingSet.Keys.OrderByDescending(a => a.Degree).First(); // go for the highest degree atom
                    Ring nextRing = GetRing(startAtom); //identify a ring
                    if (nextRing != null) //bingo
                    {
                        //and add the ring to the atoms
                        Rings.Add(nextRing); //add the ring to the set
                        foreach (Atom a in nextRing.Atoms.ToList())
                        {
                            if (!a.Rings.Contains(nextRing))
                            {
                                a.Rings.Add(nextRing);
                            }

                            if (workingSet.ContainsKey(a))
                            {
                                workingSet.Remove(a);
                            }
                            //remove the atoms in the ring from the working set BUT NOT the graph!
                        }
                    }
                    else
                    {
                        workingSet.Remove(startAtom);
                    } //the atom doesn't belong in a ring, remove it from the set.
                }
            }
#if DEBUG
            Debug.WriteLine($"Molecule = {(ChemicalNames.Count > 0 ? this.ChemicalNames?[0].Name : this.ConciseFormula)},  Number of rings = {Rings.Count}");
            sw.Stop();
            Debug.WriteLine($"Elapsed {sw.ElapsedMilliseconds}");
#endif
        }

        /// <summary>
        /// Modified Figueras top-level algorithm:
        /// 1. choose the lowest degree atom
        /// 2. Work out which rings it belongs to
        /// 3. If it belongs to a ring and that ring hasn't been calculated before, then add it to the set
        /// 4. delete the atom from the projection, reduce the degree of neighbouring atoms and prune away the side chains
        /// </summary>
        public void RebuildRings2()
        {
#if DEBUG
            Stopwatch sw = new Stopwatch();
            sw.Start();
#endif
            HashSet<string> RingIDs = new HashSet<string>(); //list of rings processed so far
            if (HasRings)
            {
                WipeMoleculeRings();

                //working set of atoms
                //it's a dictionary, because we initially store the degree of each atom against it
                //this will change as the pruning operation kicks in
                Dictionary<Atom, int> workingSet = Projection(a => a.Degree);
                //lop off any terminal branches - removes all atoms of degree <=1
                PruneSideChains(workingSet);

                while (workingSet.Any()) //do we have any atoms left in the set
                {
                    var startAtom = workingSet.OrderBy(kvp => kvp.Value).First().Key; // go for the lowest degree atom (will be of degree >=2)
                    Ring nextRing = GetRing(startAtom); //identify a ring

                    if (nextRing != null && !RingIDs.Contains(nextRing.UniqueID)) //bingo
                    {
                        //and add the ring to the atoms
                        Rings.Add(nextRing); //add the ring to the set
                        RingIDs.Add(nextRing.UniqueID);

                        foreach (Atom a in nextRing.Atoms.ToList())
                        {
                            a.Rings.Add(nextRing);
                        }

                        //
                        if (workingSet.ContainsKey(startAtom))
                        {
                            foreach (Atom atom in startAtom.Neighbours.Where(a => workingSet.ContainsKey(a)))
                            {
                                //reduce the degree of its neighbours by one
                                workingSet[atom] -= 1;
                            }
                            //break the ring

                            workingSet.Remove(startAtom);
                            //and chop down the dangling chains
                            PruneSideChains(workingSet);
                        }
                        //remove the atoms in the ring from the working set BUT NOT the graph!
                    }
                    else
                    {
                        Debug.Assert(workingSet.ContainsKey(startAtom));
                        workingSet.Remove(startAtom);
                    } //the atom doesn't belong in a ring, remove it from the set.
                }
            }
#if DEBUG
            Debug.WriteLine($"Molecule = {(ChemicalNames.Count > 0 ? this.ChemicalNames?[0].Name : this.ConciseFormula)},  Number of rings = {Rings.Count}");
            sw.Stop();
            Debug.WriteLine($"Elapsed {sw.ElapsedMilliseconds}");
#endif
        }

        private List<Ring> _sortedRings = null;

        public List<Ring> SortedRings
        {
            get
            {
                if (_sortedRings == null)
                {
                    _sortedRings = SortRingsForDBPlacement();
                }
                return _sortedRings;
            }
        }

        private void WipeMoleculeRings()
        {
            Rings.RemoveAll();

            //first set all atoms to side chains
            foreach (var a in Atoms)
            {
                a.Rings.RemoveAll();
                _sortedRings = null;
            }
        }

        /// <summary>
        /// Sorts a series of small rings ready for determining double bond placement
        /// see DOI: 10.1002/minf.201200171
        /// Rendering molecular sketches for publication quality output
        /// Alex M Clark
        /// </summary>
        /// <returns>List of rings</returns>
        // ReSharper disable once InconsistentNaming
        public List<Ring> SortRingsForDBPlacement()
        {
            //
            Debug.Assert(HasRings); //no bloody point in running this unless it has rings
            Debug.Assert(RingsCalculated); //make sure that if the molecule contains rings that we have calculated them
            //1) All rings of sizes 6, 5, 7, 4 and 3 are discovered, in that order, and added to a list R.
            var R = Rings.Where(x => x.Priority > 0).OrderBy(x => x.Priority).ToList();

            //Define B as an array of size equal to the number of atoms, where each value is equal to the number of times the atom occurs in any of the rings R
            Dictionary<Atom, int> B = new Dictionary<Atom, int>();
            foreach (Atom atom in Atoms)
            {
                B[atom] = atom.Rings.Count;
            }

            //Define Q as an array of size equal to length of R, where each value is equal to sum of B[r], where r iterates over each of the atoms within the ring.
            Dictionary<Ring, int> Q = new Dictionary<Ring, int>();
            foreach (Ring ring in R)
            {
                int sumBr = 0;
                foreach (Atom atom in ring.Atoms)
                {
                    sumBr += B[atom];
                }

                Q[ring] = sumBr;
            }

            //Perform a stable sort of the list of rings, R, so that those with the lowest values of Q are listed first.
            var R2 = R.OrderBy(r => Q[r]);

            //Define D as an array of size equal to length of R, where each value is equal to the number of double bonds within the corresponding ring
            Dictionary<Ring, int> D = new Dictionary<Ring, int>();
            foreach (Ring ring in R2)
            {
                D[ring] = ring.Bonds.Count(b => b.OrderValue == 2);
            }

            //Perform a stable sort of the list of rings, R, so that those with highest values of D are listed first

            var R3 = R2.OrderByDescending(r => D[r]);

            return R3.ToList();
        }

        //noddy nested class for ring detection
        public class AtomData
        {
            public Atom CurrentAtom { get; set; }
            public Atom Source { get; set; }
        }

        /// <summary>
        /// Start with an atom and detect which ring it's part of
        /// </summary>
        /// <param name="startAtom">Atom of degree >= 2</param>
        ///
        private static Ring GetRing(Atom startAtom)
        {
            // Only returns the first ring.
            //
            // Uses the Figueras algorithm
            // Figueras, J, J. Chem. Inf. Comput. Sci., 1996,36, 96, 986-991
            // The algorithm goes as follows:
            //1. Remove node frontNode and its Source from the front of the queue.
            //2. For each node m attached to frontNode, and not equal to Source:
            //If path[m] is null, compute path[m] ) path[frontNode] +[m]
            //and put node m(with its Source, frontNode) on the back of the queue.
            //If path[m] is not null then
            //      1) Compute the intersection path[frontNode]*path[m].
            //      2) If the intersection is a singleton, compute the ring set  path[m]+path[frontNode] and exit.
            //3. Return to step 1.
            //set up the data structures
            Queue<AtomData> atomsSoFar; //needed for BFS
            Dictionary<Atom, HashSet<Atom>> path = new Dictionary<Atom, HashSet<Atom>>();

            //initialise all the paths to empty
            foreach (var atom in startAtom.Parent.Atoms)
            {
                path[atom] = new HashSet<Atom>();
            }
            //set up a new queue
            atomsSoFar = new Queue<AtomData>();

            //set up a front node and shove it onto the queue
            AtomData frontNode;
            //shove the neigbours onto the queue to prime it
            foreach (Atom initialAtom in startAtom.Neighbours)
            {
                var node = new AtomData() { Source = startAtom, CurrentAtom = initialAtom };
                path[initialAtom] = new HashSet<Atom>() { startAtom, initialAtom };
                atomsSoFar.Enqueue(node);
            }
            //now scan the Molecule and detect all rings
            while (atomsSoFar.Any())
            {
                frontNode = atomsSoFar.Dequeue();
                foreach (Atom m in frontNode.CurrentAtom.Neighbours)
                {
                    if (m != frontNode.Source) //ignore an atom that we've visited
                    {
                        if ((!path.ContainsKey(m)) || (path[m].Count == 0)) //null path
                        {
                            var temp = new HashSet<Atom>();
                            temp.Add(m);
                            temp.UnionWith(path[frontNode.CurrentAtom]);
                            path[m] = temp; //add on the path built up so far
                            AtomData newItem = new AtomData() { Source = frontNode.CurrentAtom, CurrentAtom = m };
                            atomsSoFar.Enqueue(newItem);
                        }
                        else //we've got a collision - is it a ring closure
                        {
                            HashSet<Atom> overlap = new HashSet<Atom>();
                            overlap.UnionWith(path[frontNode.CurrentAtom]); //clone this set
                            overlap.IntersectWith(path[m]);
                            if (overlap.Count == 1) //we've had a singleton overlap :  ring closure
                            {
                                var ringAtoms = new HashSet<Atom>();
                                ringAtoms.UnionWith(path[m]);
                                ringAtoms.UnionWith(path[frontNode.CurrentAtom]);

                                return new Ring(ringAtoms);
                            }
                        }
                    }
                }
            }
            //no collisions therefore no rings detected
            return null;
        }

        #endregion Ring stuff

        private static Atom NextUnprocessedAtom(Atom seed, Predicate<Atom> isntProcessed)
        {
            return seed.Neighbours.First(a => isntProcessed(a));
        }

        #endregion Methods

        private void RingsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Ring ring in e.NewItems)
                    {
                        ring.Parent = this;
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (Ring ring in e.OldItems)
                    {
                        ring.Parent = null;
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    foreach (Ring ring in e.NewItems)
                    {
                        ring.Parent = this;
                    }
                    foreach (Ring ring in e.OldItems)
                    {
                        ring.Parent = null;
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    break;
            }
        }

        private void BondsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Bond bond in e.NewItems)
                    {
                        bond.Parent = this;
                        AllBonds.Add(bond);
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (Bond bond in e.OldItems)
                    {
                        AllBonds.Remove(bond);
                        bond.Parent = null;
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    foreach (Bond bond in e.NewItems)
                    {
                        bond.Parent = this;
                        AllBonds.Add(bond);
                    }
                    foreach (Bond bond in e.OldItems)
                    {
                        AllBonds.Remove(bond);
                        bond.Parent = null;
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    break;
            }
        }

        private void Atoms_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Atom atom in e.NewItems)
                    {
                        atom.Parent = this;
                        AllAtoms.Add(atom);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (Atom atom in e.OldItems)
                    {
                        AllAtoms.Remove(atom);
                        atom.Parent = null;
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    foreach (Atom atom in e.NewItems)
                    {
                        atom.Parent = this;
                        AllAtoms.Add(atom);
                    }
                    foreach (Atom atom in e.OldItems)
                    {
                        AllAtoms.Remove(atom);
                        atom.Parent = null;
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    break;
            }
        }

        public System.Windows.Point Centroid
        {
            get
            {
                return new Point(0, 0);
            }
        }

        public List<Atom> ConvexHull
        {
            get
            {
                return Geometry<Atom>.GetHull(AtomsSortedForHull(), a => a.Position);
            }
        }

        public string Id { get; set; }

        /*makes extensive use of the Andrew montone algorithm for determining the convex
       hulls of molecules with or without side chains.
       Assumes all rings have been calculated  first*/

        public IEnumerable<Atom> AtomsSortedForHull()
        {
            Debug.Assert(RingsCalculated);
            var atomList = from Atom a in this.Atoms
                           orderby a.Position.X ascending, a.Position.Y descending
                           select a;

            return atomList;
        }

        #region helpers

        public double MeanBondLength
        {
            get { return Bonds.Average(b => b.BondVector.Length); }
        }

        public void ScaleToAverageBondLength(double newLength, Model model)
        {
            if (Bonds.Any())
            {
                double averageBondLength = MeanBondLength;

                if (averageBondLength != 0 && newLength > 0)
                {
                    double scale = newLength / averageBondLength;

                    foreach (Atom atom in Atoms)
                    {
                        atom.Position = new Point(atom.Position.X * scale, atom.Position.Y * scale);
                    }
                }
                foreach (Molecule child in Molecules)
                {
                    child.ScaleToAverageBondLength(newLength, model);
                }
            }
        }

        public void RepositionAll(double x, double y)
        {
            var offsetVector = new Vector(-x, -y);

            foreach (Atom a in Atoms)
            {
                a.Position += offsetVector;
            }

            foreach (Molecule child in Molecules)
            {
                child.RepositionAll(x, y);
            }
        }

        public void MoveAllAtoms(double x, double y)
        {
            var offsetVector = new Vector(x, y);

            foreach (Atom a in Atoms)
            {
                a.Position += offsetVector;
            }

            foreach (Molecule child in Molecules)
            {
                child.MoveAllAtoms(x, y);
            }

            _boundingBox = Rect.Empty;
        }

        #endregion helpers
    }
}