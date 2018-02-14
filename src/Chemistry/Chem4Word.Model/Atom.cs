// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Model.Geometry;
using Chem4Word.Model.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Chem4Word.Model
{
    [DebuggerDisplay("Id: {Id} Element: {Element.Symbol}")]
    public class Atom : INotifyPropertyChanged
    {
        #region Properties

        private ElementBase _element;

        public ElementBase Element
        {
            get { return _element; }
            set
            {
                _element = value;
                OnPropertyChanged();
                OnPropertyChanged("SymbolText");
            }
        }

        /// <summary>
        /// All the immediately bonded atoms
        /// </summary>
        public List<Atom> Neighbours
        {
            get { return Bonds.Select(b => b.OtherAtom(this)).ToList(); }
        }

        /// <summary>
        /// All the immediately bonded atoms as a set
        /// </summary>
        public HashSet<Atom> NeighbourSet
        {
            get
            {
                var atoms = new HashSet<Atom>();
                atoms.UnionWith(Neighbours);
                return atoms;
            }
        }

        /// <summary>
        /// The rings this atom belongs to
        /// </summary>
        public ObservableCollection<Ring> Rings { get; }

        /// <summary>
        /// Parent molecule of the atom - readonly outside the model
        /// </summary>
        public Molecule Parent { get; internal set; }

        /// <summary>
        /// Collection of bonds for the atom
        /// </summary>
        public readonly ObservableCollection<Bond> Bonds;

        /// <summary>
        /// How many atoms are bonded to this atom
        /// </summary>
        public int Degree => Bonds.Count;

        /// <summary>
        /// How many valencies have been taken up on this atom
        /// </summary>
        public double? Saturation => Bonds.Sum(b => b.OrderValue);

        /// <summary>
        /// get a list of atoms that match an unprocessed criterion
        /// </summary>
        /// <param name="unprocessedTest">Predicate to test degree of processing  -pass as a lambda</param>
        /// <returns></returns>
        public List<Atom> UnprocessedNeighbours(Predicate<Atom> unprocessedTest)
        {
            return Neighbours.Where(a => unprocessedTest(a)).ToList();
        }

        /// <summary>
        /// How many atoms we haven't 'done' yet when we're traversing the graph
        /// </summary>
        public int UnprocessedDegree(Predicate<Atom> unprocessedTest) => UnprocessedNeighbours(unprocessedTest).Count();

        /// <summary>
        /// Where the atom is situated spatially
        /// </summary>
        private Point _position;

        public Point Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                OnPropertyChanged("Position");
            }
        }

        private string _id;
        private bool _explicitC; //backing variable for explicit carbon display

        /// <summary>
        /// Simple arbitrary text label for atom
        /// </summary>
        public String Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        /// <summary>
        /// If null, defaults to the most abundant isotope
        /// </summary>
        public int? IsotopeNumber { get; set; }

        public int? SpinMultiplicity { get; set; }

        public bool ShowHydrogens
        {
            get; set;
        }

        /// <summary>
        /// Do we show the symbol block? Only toggle for Carbon
        /// </summary>
        public bool ShowSymbol
        {
            get
            {
                if (this.Element is Element)
                {
                    if ((Element)Element == Globals.PeriodicTable.C)
                    {
                        if (IsotopeNumber != null)
                        {
                            return true;
                        }
                        else
                        {
                            return _explicitC;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (this.Element is FunctionalGroup)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (this.Element is Element)
                {
                    if ((Element)Element == Globals.PeriodicTable.C)
                    {
                        _explicitC = value;
                        OnPropertyChanged();
                        OnPropertyChanged("SymbolText");
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("ShowSymbol", Resources.ExplicitCError);
                    }
                }
                else if (this.Element is FunctionalGroup)
                {
                    throw new ArgumentOutOfRangeException("ShowSymbol", Resources.ExplicitCError);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("ShowSymbol", Resources.ExplicitCError);
                }
            }
        }

        /// <summary>
        /// Defines the text shown by the atom.  Will default to the empty string
        /// </summary>
        public string SymbolText
        {
            get
            {
                if (Element != null)
                {
                    if (Element is FunctionalGroup)
                    {
                        return ((FunctionalGroup)Element).Symbol;
                    }
                    else
                    {
                        if (Element.Symbol == "C")
                        {
                            if (ShowSymbol | Degree <= 1)
                            {
                                return "C";
                            }

                            return "";
                        }
                        else
                        {
                            return Element.Symbol;
                        }
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Formal charge. Default to null.
        /// </summary>
        private int? _formalCharge;

        public int? FormalCharge
        {
            get { return _formalCharge; }
            set
            {
                _formalCharge = value;
                //Attributed call knows who we are, no need to pass "FormalCharge" as an argument
                //OnPropertyChanged();
            }
        }

        /// <summary>
        /// Doublet radical e.g. carbene, nitrene (packaged with the charge in molfile). Default to false.
        /// </summary>
        private bool _doubletRadical;

        public bool DoubletRadical
        {
            get { return _doubletRadical; }
            set
            {
                _doubletRadical = value;
                //Attributed call knows who we are, no need to pass "DoubletRadical" as an argument
                //OnPropertyChanged();
            }
        }

        public int ImplicitHydrogenCount
        {
            get
            {
                // Return -1 if we don't need to do anything
                int iHydrogenCount = -1;

                // Applies to "B,C,N,O,F,Si,P,S,Cl,As,Se,Br,Te,I,At";
                string appliesTo = Globals.PeriodicTable.ImplicitHydrogenTargets;

                if (appliesTo.Contains(Element.Symbol))
                {
                    int iBondCount = (int)Math.Floor(this.BondOrders);
                    int iCharge = 0;
                    iCharge = FormalCharge ?? 0;
                    int iValence = PeriodicTable.GetValence((Element as Element), iBondCount);
                    int iDiff = iValence - iBondCount;
                    if (iCharge > 0)
                    {
                        int iVdiff = 4 - iValence;
                        if (iCharge <= iVdiff)
                        {
                            iDiff += iCharge;
                        }
                        else
                        {
                            iDiff = 4 - iBondCount - iCharge + iVdiff;
                        }
                    }
                    else
                    {
                        iDiff += iCharge;
                    }
                    // Ensure iHydrogenCount returned is never -ve
                    if (iDiff >= 0)
                    {
                        iHydrogenCount = iDiff;
                    }
                    else
                    {
                        iHydrogenCount = 0;
                    }
                }
                return iHydrogenCount;
            }
        }

        public double BondOrders
        {
            get { return Bonds.Sum(b => b.OrderValue) ?? 0d; }
        }

        public Atom SelfRef => this;

        //atoms go over bonds in a visual display
        public int ZIndex => 2;

        /// <summary>
        /// Returns a vector which points to the most uncrowded side of the atom.
        ///
        /// </summary>
        public Vector BalancingVector
        {
            get
            {
                Vector vsumVector = new Vector();

                if (Bonds.Any())
                {
                    double sumOfLengths = 0;
                    foreach (var bond in Bonds)
                    {
                        Vector v = bond.OtherAtom(this).Position - this.Position;
                        sumOfLengths += v.Length;
                        vsumVector += v;
                    }

                    // Set tiny amount as 10% of average bond length
                    double tinyAmount = sumOfLengths / Bonds.Count * 0.1;
                    double xy = vsumVector.Length;

                    // Is resultant vector is big enough for us to use?
                    if (xy >= tinyAmount)
                    {
                        // Get vector in opposite direction
                        vsumVector = -vsumVector;
                        vsumVector.Normalize();
                    }
                    else
                    {
                        // Get vector of first bond
                        Vector vector = Bonds[0].OtherAtom(this).Position - this.Position;
                        if (Bonds.Count == 2)
                        {
                            // Get vector at right angles
                            vsumVector = vector.Perpendicular();
                        }
                        else
                        {
                            // Get vector in opposite direction
                            vsumVector = -vector;
                        }
                        vsumVector.Normalize();
                    }
                }

                //Debug.WriteLine($"Atom {Id} Resultant Balancing Vector Angle is {Vector.AngleBetween(BasicGeometry.ScreenNorth(), vsumVector)}");
                return vsumVector;
            }
        }

        // This is Clyde's Code saved Just in case it's needed
        //public Vector BalancingVector
        //{
        //    get
        //    {
        //        Vector total= new Vector();
        //        foreach (Bond b in Bonds)
        //        {
        //            Vector bv = b.OtherAtom(this).Position - this.Position;
        //            bv.Normalize();
        //            total += bv;
        //        }

        //        if (total.Length < VectorTolerance)
        //        {
        //            return new Vector();
        //        }
        //        else
        //        {
        //            total.Normalize();
        //            total.Negate();
        //            return total;
        //        }
        //    }
        //}

        #endregion Properties

        #region Constructors

        public Atom()
        {
            //set up the collections for the atom itself
            Bonds = new ObservableCollection<Bond>();
            Rings = new ObservableCollection<Ring>();
            Rings.CollectionChanged += Rings_CollectionChanged;
            //Default values
            FormalCharge = null;
            DoubletRadical = false;
        }

        private void Rings_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    break;

                case NotifyCollectionChangedAction.Remove:
                    break;
            }
        }

        #endregion Constructors

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged

        #region Methods

        /// <summary>
        /// Converts a CML element to a point object
        /// </summary>
        /// <param name="atom">XEelement describing the atom</param>
        /// <returns></returns>

        /// <summary>
        /// returns all borderign atoms except the one listed
        /// </summary>
        /// <param name="toIgnore"></param>
        /// <returns></returns>
        public List<Atom> NeighboursExcept(Atom toIgnore)
        {
            return Neighbours.Where(a => a != toIgnore).ToList();
        }

        public List<Atom> NeighboursExcept(params Atom[] toIgnore)
        {
            return Neighbours.Where(a => !toIgnore.Contains(a)).ToList();
        }

        public Bond BondBetween(Atom neighbour)
        {
            if (!Neighbours.Contains(neighbour))
            {
                // ReSharper disable once NotResolvedInText
                throw new ArgumentOutOfRangeException("Atom is not a neighbour.");
            }
            return Bonds.First(b => b.OtherAtom(this) == neighbour);
        }

        public double GetDistance(Atom other)
        {
            return (other.Position - Position).Length;
        }

        // gets signed angle between three points
        /// direction is anticlockwise
        /// example:
        /// GetAngle(new Point2(1,0), new Point2(0,0), new Point2(0,1)) => Math.PI/2
        /// GetAngle(new Point2(-1,0), new Point2(0,0), new Point2(0,1)) => -Math.PI/2
        /// GetAngle(new Point2(0,1), new Point2(0,0), new Point2(1,0)) => -Math.PI/2
        public static double? GetAngle(Atom atom0, Atom atom1, Atom atom2)
        {
            return BasicGeometry.GetAngle(atom0.Position, atom1.Position, atom2.Position, 0.0001);
        }

        #endregion Methods
    }
}