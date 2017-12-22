// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Model;
using Chem4Word.Model.Enums;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace Chem4Word.Renderer.OoXmlV3.OOXML.Bonds
{
    public class BondLinePositioner
    {
        private List<BondLine> m_BondLines;
        private double m_medianBondLength;

        public BondLinePositioner(List<BondLine> bondLines, double medianBondLength)
        {
            m_BondLines = bondLines;
            m_medianBondLength = medianBondLength;
        }

        /// <summary>
        /// Creates the lines for a bond
        /// </summary>
        /// <param name="wordprocessingGroup1">Where to add the bond lines</param>
        /// <param name="bond"></param>
        public void CreateLines(Bond bond)
        {
            //Debug.WriteLine("Bond: " + bond.Id);

            //IEnumerable<Atom> bondatoms = bond.GetAtoms();
            IEnumerable<Ring> rings = bond.Rings;
            int ringCount = 0;
            foreach (Ring r in rings)
            {
                ringCount++;
            }
            //Debug.WriteLine("  Ring Count: " + ringCount);

            Point bondStart = bond.StartAtom.Position;

            Point bondEnd = bond.EndAtom.Position;

            //IEnumerable<CmlBond> toAtomBonds = toAtom.GetLigandBonds();
            //int toAtomBondCount = toAtomBonds.ToArray<CmlBond>().Length;

            //Debug.WriteLine("    From : " + fromAtom.ElementType + " [" + fromAtom.Id + "] at " + bondStart.X + ", " + bondStart.Y);
            //Debug.WriteLine("      To : " + toAtom.ElementType + " [" + toAtom.Id + "] at " + bondEnd.X + ", " + bondEnd.Y);

            #region Create Bond Line objects

            switch (bond.Order)
            {
                case Bond.OrderZero:
                case "unknown":
                    m_BondLines.Add(new BondLine(bondStart, bondEnd, BondLineStyle.Dotted, bond.Id));
                    break;

                case Bond.OrderPartial01:
                    m_BondLines.Add(new BondLine(bondStart, bondEnd, BondLineStyle.Dashed, bond.Id));
                    break;

                case "1":
                case Bond.OrderSingle:
                    switch (bond.Stereo)
                    {
                        case BondStereo.None:
                            m_BondLines.Add(new BondLine(bondStart, bondEnd, BondLineStyle.Solid, bond.Id));
                            break;

                        case BondStereo.Hatch:
                            m_BondLines.Add(new BondLine(bondStart, bondEnd, BondLineStyle.Hash, bond.Id));
                            break;

                        case BondStereo.Wedge:
                            m_BondLines.Add(new BondLine(bondStart, bondEnd, BondLineStyle.Wedge, bond.Id));
                            break;

                        case BondStereo.Indeterminate:
                            m_BondLines.Add(new BondLine(bondStart, bondEnd, BondLineStyle.Wavy, bond.Id));
                            break;

                        default:

                            m_BondLines.Add(new BondLine(bondStart, bondEnd, BondLineStyle.Solid, bond.Id));
                            break;
                    }
                    break;

                case Bond.OrderPartial12:
                    //case "A":
                    BondLine a = new BondLine(bondStart, bondEnd, BondLineStyle.Solid, bond.Id);
                    m_BondLines.Add(a);
                    BondLine a1 = a.GetParallel(-BondOffset());
                    Point newStarta = new Point(a1.Start.X, a1.Start.Y);
                    Point newEnda = new Point(a1.End.X, a1.End.Y);
                    CoordinateTool.AdjustLineAboutMidpoint(ref newStarta, ref newEnda, -(BondOffset() / 1.75));
                    a1 = new BondLine(newStarta, newEnda, BondLineStyle.Dotted, bond.Id);
                    m_BondLines.Add(a1);
                    break;

                case Bond.OrderDouble:
                    if (bond.Stereo == BondStereo.Indeterminate) //crossing bonds
                    {
                        // Crossed lines
                        BondLine d = new BondLine(bondStart, bondEnd, BondLineStyle.Solid, bond.Id);
                        BondLine d1 = d.GetParallel(-(BondOffset() / 2));
                        BondLine d2 = d.GetParallel(BondOffset() / 2);
                        m_BondLines.Add(new BondLine(new Point(d1.Start.X, d1.Start.Y), new Point(d2.End.X, d2.End.Y), BondLineStyle.Solid, bond.Id));
                        m_BondLines.Add(new BondLine(new Point(d2.Start.X, d2.Start.Y), new Point(d1.End.X, d1.End.Y), BondLineStyle.Solid, bond.Id));
                    }
                    else
                    {
                        bool shifted = false;
                        if (ringCount == 0)
                        {
                            if (bond.StartAtom.Element as Element == Globals.PeriodicTable.C && bond.EndAtom.Element as Element == Globals.PeriodicTable.C)
                            {
                                shifted = false;
                            }
                            else
                            {
                                shifted = true;
                            }
                        }

                        if (bond.StartAtom.Degree == 1 | bond.EndAtom.Degree == 1)
                        {
                            shifted = true;
                        }

                        if (shifted)
                        {
                            BondLine d = new BondLine(bondStart, bondEnd, BondLineStyle.Solid, bond.Id);
                            m_BondLines.Add(d.GetParallel(-(BondOffset() / 2)));
                            m_BondLines.Add(d.GetParallel(BondOffset() / 2));
                        }
                        else
                        {
                            Debug.WriteLine($"bond.Placement {bond.Placement}");
                            Point outIntersectP1;
                            Point outIntersectP2;
                            bool linesIntersect;
                            bool segmentsIntersect;
                            Point centre;

                            switch (bond.Placement)
                            {
                                case BondDirection.Anticlockwise:
                                    BondLine da = new BondLine(bondStart, bondEnd, BondLineStyle.Solid, bond.Id);
                                    m_BondLines.Add(da);

                                    BondLine bla = da.GetParallel(-BondOffset());
                                    Point startPointa = bla.Start;
                                    Point endPointa = bla.End;

                                    if (bond.PrimaryRing != null)
                                    {
                                        centre = bond.PrimaryRing.Centroid.Value;
                                        // Diagnostics
                                        //m_BondLines.Add(new BondLine(bondStart, centre, BondLineStyle.Dotted, null));
                                        //m_BondLines.Add(new BondLine(bondEnd, centre, BondLineStyle.Dotted, null));

                                        CoordinateTool.FindIntersection(startPointa, endPointa, bondStart, centre,
                                            out linesIntersect, out segmentsIntersect, out outIntersectP1);
                                        CoordinateTool.FindIntersection(startPointa, endPointa, bondEnd, centre,
                                            out linesIntersect, out segmentsIntersect, out outIntersectP2);

                                        m_BondLines.Add(new BondLine(outIntersectP1, outIntersectP2, BondLineStyle.Solid, bond.Id));
                                    }
                                    else
                                    {
                                        CoordinateTool.AdjustLineAboutMidpoint(ref startPointa, ref endPointa, -(BondOffset() / 1.75));
                                        m_BondLines.Add(new BondLine(startPointa, endPointa, BondLineStyle.Solid, bond.Id));
                                    }
                                    break;

                                case BondDirection.Clockwise:
                                    BondLine dc = new BondLine(bondStart, bondEnd, BondLineStyle.Solid, bond.Id);
                                    m_BondLines.Add(dc);

                                    BondLine blc = dc.GetParallel(+BondOffset());
                                    Point startPointc = blc.Start;
                                    Point endPointc = blc.End;

                                    if (bond.PrimaryRing != null)
                                    {
                                        centre = bond.PrimaryRing.Centroid.Value;
                                        // Diagnostics
                                        //m_BondLines.Add(new BondLine(bondStart, centre, BondLineStyle.Dotted, null));
                                        //m_BondLines.Add(new BondLine(bondEnd, centre, BondLineStyle.Dotted, null));

                                        CoordinateTool.FindIntersection(startPointc, endPointc, bondStart, centre,
                                            out linesIntersect, out segmentsIntersect, out outIntersectP1);
                                        CoordinateTool.FindIntersection(startPointc, endPointc, bondEnd, centre,
                                            out linesIntersect, out segmentsIntersect, out outIntersectP2);

                                        m_BondLines.Add(new BondLine(outIntersectP1, outIntersectP2, BondLineStyle.Solid, bond.Id));
                                    }
                                    else
                                    {
                                        CoordinateTool.AdjustLineAboutMidpoint(ref startPointc, ref endPointc, -(BondOffset() / 1.75));
                                        m_BondLines.Add(new BondLine(startPointc, endPointc, BondLineStyle.Solid, bond.Id));
                                    }
                                    break;

                                default:
                                    switch (bond.Stereo)
                                    {
                                        case BondStereo.Cis:
                                            BondLine dcc = new BondLine(bondStart, bondEnd, BondLineStyle.Solid, bond.Id);
                                            m_BondLines.Add(dcc);
                                            BondLine blnewc = dcc.GetParallel(+BondOffset());
                                            Point startPointn = blnewc.Start;
                                            Point endPointn = blnewc.End;
                                            CoordinateTool.AdjustLineAboutMidpoint(ref startPointn, ref endPointn, -(BondOffset() / 1.75));
                                            m_BondLines.Add(new BondLine(startPointn, endPointn, BondLineStyle.Solid, bond.Id));
                                            break;

                                        case BondStereo.Trans:
                                            BondLine dtt = new BondLine(bondStart, bondEnd, BondLineStyle.Solid, bond.Id);
                                            m_BondLines.Add(dtt);
                                            BondLine blnewt = dtt.GetParallel(+BondOffset());
                                            Point startPointt = blnewt.Start;
                                            Point endPointt = blnewt.End;
                                            CoordinateTool.AdjustLineAboutMidpoint(ref startPointt, ref endPointt, -(BondOffset() / 1.75));
                                            m_BondLines.Add(new BondLine(startPointt, endPointt, BondLineStyle.Solid, bond.Id));
                                            break;

                                        default:
                                            BondLine dp = new BondLine(bondStart, bondEnd, BondLineStyle.Solid, bond.Id);
                                            m_BondLines.Add(dp.GetParallel(-(BondOffset() / 2)));
                                            m_BondLines.Add(dp.GetParallel(BondOffset() / 2));
                                            break;
                                    }
                                    break;
                            }
                        }
                    }
                    break;

                case "3":
                case "T":
                    // Draw main bond line
                    BondLine t = new BondLine(bondStart, bondEnd, BondLineStyle.Solid, bond.Id);
                    m_BondLines.Add(t);
                    m_BondLines.Add(t.GetParallel(BondOffset()));
                    m_BondLines.Add(t.GetParallel(-BondOffset()));
                    break;

                default:
                    m_BondLines.Add(new BondLine(bondStart, bondEnd, BondLineStyle.Solid, bond.Id));
                    break;
            }

            #endregion Create Bond Line objects
        }

        private double BondOffset()
        {
            return (m_medianBondLength * OoXmlHelper.MULTIPLE_BOND_OFFSET_PERCENTAGE);
        }

        private double BondOffsetPercent(Vector v)
        {
            double standardOffset = m_medianBondLength * OoXmlHelper.MULTIPLE_BOND_OFFSET_PERCENTAGE;
            double ratio = standardOffset / v.Length;
            return ratio;
        }
    }
}