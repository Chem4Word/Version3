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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Chem4Word.Model
{
    [DebuggerDisplay("Id: {Id} From: {StartAtom.Id} To: {EndAtom.Id}")]
    public class Bond : INotifyPropertyChanged
    {
        private Atom _startAtom, _endAtom;

        private Molecule _parent;

        /// <summary>
        /// Simple string label for the bond
        /// </summary>
        public string Id { get; set; }

        public bool Processed { get; set; }

        public Bond SelfRef
        {
            get { return this; }
        }

        /// <summary>
        /// Collection of rings to which the bond belongs
        /// calculated dynamically
        /// </summary>
        public List<Ring> Rings
        {
            get
            {
                var myRings = StartAtom.Rings.ToList().Intersect(EndAtom.Rings.ToList());
                return myRings.ToList();
            }
        }

        public Bond()
        {
        }

        private void Rings_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    break;

                case NotifyCollectionChangedAction.Remove:
                    break;
            }
        }

        public Point MidPoint
        {
            get
            {
                if (StartAtom == null | EndAtom == null)
                {
                    throw new InvalidOperationException("Both atoms must be assigned");
                }
                else
                {
                    return new Point((StartAtom.Position.X + EndAtom.Position.X) / 2,
                        (StartAtom.Position.Y + EndAtom.Position.Y) / 2);
                }
            }
        }

        /// <summary>
        /// Atom marking the start of the bond
        /// </summary>
        ///
        public Atom StartAtom
        {
            get
            {
                return _startAtom;
            }
            set
            {
                _startAtom?.Bonds.Remove(this);
                _startAtom = value;
                _startAtom.Bonds.Add(this);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Atom marking the end of the bond
        /// </summary>
        public Atom EndAtom
        {
            get
            {
                return _endAtom;
            }
            set
            {
                _endAtom?.Bonds.Remove(this);
                _endAtom = value;
                _endAtom.Bonds.Add(this);
                OnPropertyChanged();
            }
        }

        public List<Atom> GetAtoms()
        {
            return new List<Atom>() { StartAtom, EndAtom };
        }

        public const string OrderZero = "hbond";
        public const string OrderOther = "other";
        public const string OrderPartial01 = "partial01";
        public const string OrderSingle = "S";
        public const string OrderPartial12 = "partial12";
        public const string OrderAromatic = "A";
        public const string OrderDouble = "D";
        public const string OrderPartial23 = "partial23";
        public const string OrderTriple = "T";

        private string _order;

        public string Order
        {
            get { return _order; }
            set
            {
                if (value == "0.5")
                {
                    value = OrderPartial01;
                }
                if (value == "1")
                {
                    value = OrderSingle;
                }
                if (value == "1.5")
                {
                    value = OrderPartial12;
                }

                if (value == "2")
                {
                    value = OrderDouble;
                }
                if (value == "3")
                {
                    value = OrderTriple;
                }
                if (value == "0")
                {
                    value = OrderZero;
                }

                _order = value;
                OnPropertyChanged();
            }
        }

        public static string OrderValueToOrder(double val, bool isAromatic = false)
        {
            if (val == 0)
            {
                return OrderZero;
            }
            if (val == 0.5)
            {
                return OrderPartial01;
            }
            if (val == 1)
            {
                return OrderSingle;
            }
            if (val == 1.5)
            {
                if (isAromatic)
                {
                    return OrderAromatic;
                }
                else
                {
                    return OrderPartial12;
                }
            }
            if (val == 2)
            {
                return OrderDouble;
            }
            if (val == 2.5)
            {
                return OrderPartial23;
            }
            if (val == 3)
            {
                return OrderTriple;
            }
            return "";
        }

        public double? OrderValue
        {
            get
            {
                return OrderToOrderValue(Order);
            }
        }

        public static double? OrderToOrderValue(string order)
        {
            switch (order)
            {
                case OrderZero:
                case OrderOther:
                    return 0;

                case OrderPartial01:
                    return 0.5;

                case OrderSingle:
                    return 1;

                case OrderPartial12:
                    return 1.5;

                case OrderAromatic:
                    return 1.5;

                case OrderDouble:
                    return 2;

                case OrderPartial23:
                    return 2.5;

                case OrderTriple:
                    return 3;

                default:
                    return null;
            }
        }

        private BondStereo _stereo;

        public BondStereo Stereo
        {
            get { return _stereo; }
            set
            {
                _stereo = value;
                OnPropertyChanged();
            }
        }

        public double Angle
        {
            get { return Vector.AngleBetween(BasicGeometry.ScreenNorth(), BondVector); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Atom OtherAtom(Atom atom)
        {
            if (atom == StartAtom)
            {
                return EndAtom;
            }
            else if (atom == EndAtom)
            {
                return StartAtom;
            }
            else
            {
                // ReSharper disable once NotResolvedInText
                throw new ArgumentOutOfRangeException("Atom is not part of this bond.");
            }
        }

        /// <summary>
        /// Molecule the bond belongs to.
        /// DO NOT set this property explicitly:
        /// simply add the bond to the molecule's Bonds collection
        ///
        /// </summary>

        public Molecule Parent
        {
            get { return _parent; }

            internal set { _parent = value; }
        }

        public Vector BondVector => EndAtom.Position - StartAtom.Position;

        /// <summary>
        /// Are the atoms cis to the double bond?
        /// </summary>
        /// <param name="atomA"></param>
        /// <param name="atomB"></param>
        /// <returns></returns>
        public bool AtomsAreCis(Atom atomA, Atom atomB)
        {
            //do an assert to make sure that we're not calling this routine with atoms detached from the bond atoms
            Debug.Assert(StartAtom.Neighbours.Contains(atomA) & EndAtom.Neighbours.Contains(atomB)
                || StartAtom.Neighbours.Contains(atomB) & EndAtom.Neighbours.Contains(atomA));

            // Note: Add null checks as this has been found to be blowing up
            if (atomA != null && atomB != null
                && StartAtom.Neighbours != null & EndAtom.Neighbours != null
                && StartAtom.Neighbours.Count > 0 && EndAtom.Neighbours.Count > 0)
            {
                if (StartAtom.Neighbours.Contains(atomA))
                {
                    //draw two lines from the end atom to atom a and start atom to atom b and see if they intersect
                    return BasicGeometry.LineSegmentsIntersect(EndAtom.Position, atomA.Position,
                                                               StartAtom.Position, atomB.Position) != null;
                }
                else
                {
                    //draw the lines the other way around
                    return BasicGeometry.LineSegmentsIntersect(EndAtom.Position, atomB.Position,
                                                               StartAtom.Position, atomA.Position) != null;
                }
            }
            else
            {
                return false;
            }
        }

        public bool AtomsAreTrans(Atom atomA, Atom atomB)
        {
            return !AtomsAreCis(atomA, atomB);
        }

        public BondDirection? ExplicitPlacement { get; set; }

        ///
        ///  indicates which side of bond to draw subsidiary double bond
        /// if bond order is not 2, returns null
        /// if bond is in single ring, vector points to centre of ring
        /// if bond is in cisoid bond (excluding hydrogens) points to area
        /// including both bonds
        /// if bond is in 3 rings vector is null
        ///
        ///

        //private BondDirection? _dir;

        ///
        /// <summary>
        /// This stores the double bond placement as set by user
        /// Defaults to a computed value
        /// </summary>
        public BondDirection Placement
        {
            get
            {
                if (OrderValue == 2)
                {
                    //force a recalc of the rings if necessary
                    if (!Parent.RingsCalculated)
                    {
                        Parent.RebuildRings();
                    }
                    return ExplicitPlacement ?? ImplicitPlacement ?? BondDirection.None;
                }
                else
                {
                    return BondDirection.None;
                }
            }
            set
            {
                ExplicitPlacement = value;
                OnPropertyChanged();
            }
        }

        private BondDirection? _implicitPlacement = null; //caching variable, can be trashed

        /// <summary>
        /// What the model thinks the placement should be.
        /// This is a caching variable so we don't have
        /// to re-calulate every time we need to use it.
        /// </summary>
        public BondDirection? ImplicitPlacement
        {
            get
            {
                if (_implicitPlacement == null) //we haven't even touched this yet
                {
                    _implicitPlacement = GetPlacement(); //so guess what it should be
                }

                return _implicitPlacement;
            }
            internal set { _implicitPlacement = value; }
        }

        /// <summary>
        /// Returns the optimal placement for the double bond
        /// </summary>
        /// <returns>BondDirection value indicating how the bond should be placed.</returns>
        private BondDirection? GetPlacement()
        {
            BondDirection dir = BondDirection.None;

            var vec = GetPrettyDoubleBondVector();

            if (vec == null)
            {
                dir = BondDirection.None;
            }
            else
            {
                dir = (BondDirection)Math.Sign(Vector.CrossProduct(vec.Value, BondVector));
            }

            return dir;
        }

        public Vector? GetPrettyDoubleBondVector()
        {
            Vector? vector = null;

            if (IsCyclic())
            {
                return GetPrettyCyclicDoubleBondVector();
            }

            // We're acyclic.

            //GetAtomAndLigandIDs(0, out StartAtom, out a1LigandIDs);

            var startLigands = (from Atom a in StartAtom.Neighbours
                                where a != EndAtom
                                select a).ToList();

            if (!startLigands.Any())
            {
                return null;
            }

            var endLigands = (from Atom b in EndAtom.Neighbours
                              where b != StartAtom
                              select b).ToList();

            if (!endLigands.Any())
            {
                return null;
            }

            if (startLigands.Count() > 2 || endLigands.Count() > 2)
            {
                return null;
            }

            if (startLigands.Count() == 2 && endLigands.Count() == 2)
            {
                return null;
            }

            if (startLigands.AreAllH() && endLigands.AreAllH())
            {
                return null;
            }

            if (startLigands.ContainNoH() && endLigands.ContainNoH())
            {
                return null;
            }

            if (startLigands.GetHCount() == 1 && endLigands.GetNonHCount() == 1)
            {
                if (endLigands.Count() == 2)
                {
                    if (endLigands.GetHCount() == 2 || endLigands.ContainNoH())
                    {
                        //Double sided bond on the side of the non H atom from StartLigands
                        //Elbow bond :¬)
                        //DrawElbowBond(StartAtom);
                        return VectorOnSideOfNonHAtomFromStartLigands(StartAtom, EndAtom, startLigands);
                    }
                    // Now must have 1 H and 1 !H
                    if (AtomsAreCis(startLigands.GetFirstNonH(), endLigands.GetFirstNonH())
                        /*if a2a H on the same side as a1a H*/)
                    {
                        //double bond on the side of non H
                        return VectorOnSideOfNonHAtomFromStartLigands(StartAtom, EndAtom, startLigands);
                    }

                    //Now must be a trans bond.
                    return null;
                }
                else
                {
                    //Count now 1
                    if (endLigands.GetHCount() == 1)
                    {
                        //Double bond on the side of non H from StartLigands, bevel 1 end.
                        return VectorOnSideOfNonHAtomFromStartLigands(StartAtom, EndAtom, startLigands);
                    }

                    //Now !H
                    if (AtomsAreCis(startLigands.GetFirstNonH(), endLigands.GetFirstNonH())
                        /*EndAtomAtom's !H is on the same side as StartAtomAtom's !H*/)
                    {
                        //double bond on the side of !H from StartLigands, bevel both ends
                        return VectorOnSideOfNonHAtomFromStartLigands(StartAtom, EndAtom, startLigands);
                    }

                    //Now must be a trans bond.
                    return null;
                }
            }
            else if (startLigands.AreAllH())
            {
                if (endLigands.Count() == 2)
                {
                    if (endLigands.AreAllH())
                    {
                        //Already caught.
                        // ToDo: Check with Clyde
                        //throw new ApplicationException("This scenario should already have been caught");
                        return null;
                    }
                    if (endLigands.ContainNoH())
                    {
                        return null;
                    }

                    //Must now have 1 H and 1 !H
                    //double bond on the side of EndLigands' !H, bevel 1 end only
                    return VectorOnSideOfNonHAtomFromStartLigands(StartAtom, EndAtom, endLigands);
                }
                //Count must now be 1

                if (endLigands.AreAllH())
                {
                    //Already caught
                    // ToDo: Check with Clyde
                    //throw new ApplicationException("This scenario should already have been caught");
                    return null;
                }

                // Must now be 1 !H
                // Double bond on the side of EndLigands' !H, bevel 1 end only.
                return VectorOnSideOfNonHAtomFromStartLigands(StartAtom, EndAtom, endLigands);
            }
            else if (startLigands.GetHCount() == 0)
            {
                if (endLigands.Count() == 2)
                {
                    if (endLigands.AreAllH())
                    {
                        return null;
                    }
                    if (endLigands.ContainNoH())
                    {
                        return null;
                    }
                    // Now must have 1 H and 1 !H
                    //Double bond on the side of EndLigands' !H, bevel both ends.
                    return VectorOnSideOfNonHAtomFromStartLigands(StartAtom, EndAtom, endLigands);
                }
                //Count is 1
                if (endLigands.GetHCount() == 1)
                {
                    return null;
                }

                if (endLigands.GetHCount() == 0)
                {
                    //double bond on the side of EndLigands' !H, bevel both ends.
                    return VectorOnSideOfNonHAtomFromStartLigands(StartAtom, EndAtom, endLigands);
                }
            }
            // StartLigands' count = 1
            else if (startLigands.GetHCount() == 1)
            {
                if (endLigands.Count() == 2)
                {
                    if (endLigands.AreAllH())
                    {
                        // Already caught.
                        // ToDo: Check with Clyde
                        //throw new ApplicationException("This scenario should already have been caught");
                        return null;
                    }
                    if (endLigands.ContainNoH())
                    {
                        return null;
                    }

                    //Now EndLigands contains 1 H and 1 !H
                    //double bond on side of EndLigands' !H, bevel 1 end
                    return VectorOnSideOfNonHAtomFromStartLigands(StartAtom, EndAtom, endLigands);
                }

                if (endLigands.AreAllH())
                {
                    // Already caught.
                    // ToDo: Check with Clyde
                    //throw new ApplicationException("This scenario should already hve been caught");
                    return null;
                }
            }
            else if (startLigands.GetHCount() == 0)
            {
                if (endLigands.Count() == 2)
                {
                    if (endLigands.AreAllH())
                    {
                        //Double dbond on the side of StartLigands' !H, bevel one end.
                        return VectorOnSideOfNonHAtomFromStartLigands(StartAtom, EndAtom, startLigands);
                    }
                    if (endLigands.ContainNoH())
                    {
                        //double bond on the side of StartLigands' !H, bevel both end.
                        return VectorOnSideOfNonHAtomFromStartLigands(StartAtom, EndAtom, startLigands);
                    }

                    //Now EndLigands contains 1 H and 1 !H
                    if (AtomsAreCis(startLigands.GetFirstNonH(), endLigands.GetFirstNonH())
                        /* EndLigands' !H is on the same side as StartAtomAtom's !H */)
                    {
                        //double bond on the side of StartLigands' !H, bevel both ends
                        return VectorOnSideOfNonHAtomFromStartLigands(StartAtom, EndAtom, startLigands);
                    }

                    // Must be trans
                    return null;
                }

                // atoms2Atoms length = 1
                if (endLigands.GetHCount() == 1)
                {
                    //double bond on side of StartLigands' !H, bevel one end.
                    return VectorOnSideOfNonHAtomFromStartLigands(StartAtom, EndAtom, startLigands);
                }

                // EndLigands must contain 1 !H
                if (AtomsAreCis(startLigands.GetFirstNonH(), endLigands.GetFirstNonH())
                    /* EndLigands' !H is on same side as StartLigands' !H */)
                {
                    //double bond on side of StartLigands' !H, bevel both ends
                    return VectorOnSideOfNonHAtomFromStartLigands(StartAtom, EndAtom, startLigands);
                }

                //Must be trans
                return null;
            }

            return vector;
        }

        private Vector? VectorOnSideOfNonHAtomFromStartLigands(Atom startAtom, Atom endAtom, IEnumerable<Atom> startLigands)
        {
            Vector posDisplacementVector = BondVector.Perpendicular();
            Vector negDisplacementVector = -posDisplacementVector;
            posDisplacementVector.Normalize();
            negDisplacementVector.Normalize();

            posDisplacementVector = posDisplacementVector * 3;
            negDisplacementVector = negDisplacementVector * 3;

            Point posEndPoint = endAtom.Position + posDisplacementVector;
            Point negEndPoint = endAtom.Position + negDisplacementVector;

            Atom nonHAtom = startAtom.Neighbours.First(n => n != endAtom && (Element)n.Element != Globals.PeriodicTable.H);
            Point nonHAtomLoc = nonHAtom.Position;

            double posDist = (nonHAtomLoc - posEndPoint).Length;
            double negDist = (nonHAtomLoc - negEndPoint).Length;

            bool posDisplacement = posDist < negDist;
            Vector displacementVector = posDisplacement ? posDisplacementVector : negDisplacementVector;

            return displacementVector;
        }

        /// <summary>
        /// is the bond cyclic
        /// </summary>
        /// <returns>bool indicating cyclicity</returns>
        public bool IsCyclic()
        {
            return Rings.Any();
        }

        /// <summary>
        /// main ring in which a double bond should be placed
        /// </summary>
        public Ring PrimaryRing
        {
            get
            {
                Debug.Assert(Parent.RingsCalculated);

                if (!Rings.Any()) //no rings
                {
                    return null;
                }
                else
                {
                    List<Ring> ringList = Parent.SortedRings;
                    var firstRing = (
                        from Ring r in ringList
                        where r.Atoms.Contains(this.StartAtom) && r.Atoms.Contains(this.EndAtom)
                        select r
                        ).FirstOrDefault();
                    return firstRing;
                }
            }
        }

        /// <summary>
        /// indicates which side of bond to draw subsidiary double bond
        /// </summary>
        /// if bond order is not 2, returns null
        /// if bond is in single ring, vector points to centre of ring
        /// if bond is in cisoid bond (excluding hydrogens) points to area
        /// including both bonds
        /// if bond is in 3 rings vector is null
        ///
        /// this is not foolproof as we may need to make a benzene ring consistent
        /// <returns></returns>
        public Vector? GetPrettyCyclicDoubleBondVector()
        {
            Debug.Assert(Parent.RingsCalculated);

            Vector? vector = null;

            Ring theRing = PrimaryRing;

            List<Ring> ringList = Rings.Where(x => x.Priority > 0).OrderBy(x => x.Priority).ToList();

            if (ringList.Any()) //no rings
            {
                Point? ringCentroid = theRing.Centroid;
                vector = ringCentroid - MidPoint;
            }

            return vector;
        }

        //bonds go under atoms on a visual display
        public int ZIndex => 1;
    }
}