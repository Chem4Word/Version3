// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Model;
using Chem4Word.Model.Geometry;
using Chem4Word.Renderer.OoXmlV3.TTF;
using IChem4Word.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Point = System.Windows.Point;

namespace Chem4Word.Renderer.OoXmlV3.OOXML.Atoms
{
    public class AtomLabelPositioner
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType.Name;

        private Dictionary<char, TtfCharacter> m_TtfCharacterSet;

        private List<AtomLabelCharacter> m_AtomLabelCharacters;
        private IChem4WordTelemetry _telemetry;

        public AtomLabelPositioner(List<AtomLabelCharacter> atomLabelCharacters, Dictionary<char, TtfCharacter> characterset, PeriodicTable atomDefinitions, IChem4WordTelemetry telemetry)
        {
            m_AtomLabelCharacters = atomLabelCharacters;
            m_TtfCharacterSet = characterset;
            _telemetry = telemetry;
        }

        public void CreateCharacters(Atom atom, Options options)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            //Debug.WriteLine("Atom: " + atom.Id + " is " + atom.ElementType);

            //Point atomCentre = new Point((double)atom.X2, (double)atom.Y2);
            string atomLabel = atom.Element.Symbol;
            Rect labelBounds;

            //Debug Code
            //atomLabel = "H" + "abcdefHghijklmnopqrstuvwxyz" + "H";
            //atomLabel = "ABCHDEFGHIJKLMNOHPQRSHTUVWXYZ";
            //atomLabel = "-+H01234567890H+-";

            // Get Charge and Isotope valuesfor use later on
            int iCharge = atom.FormalCharge ?? 0;
            int iAbsCharge = Math.Abs(iCharge);
            int isoValue = atom.IsotopeNumber ?? 0;

            // Get Implicit Hydrogen Count for use later on
            int implicitHCount = atom.ImplicitHydrogenCount;

            Point cursorPosition = atom.Position;
            Point chargeCursorPosition = atom.Position;
            Point isotopeCursorPosition = atom.Position;

            double lastOffset = 0;

            //Debug.WriteLine("  X: " + atom.X2 + " Y: " + atom.Y2 + " Implicit H Count: " + implicitHCount);

            int ringCount = atom.Rings.Count();
            int bondCount = atom.Bonds.Count;

            //var bv = atom.BalancingVector;
            //_telemetry.Write(module, "Debugging", $"Atom {atomLabel} [{atom.Id}] at {atom.Position} BalancingVector {bv} [{CoordinateTool.BearingOfVector(bv)}°]");

            #region Decide if atom label is to be displayed

            bool showLabel = true;
            if (atomLabel == "C")
            {
                if (!options.ShowCarbons)
                {
                    if (ringCount > 0 || bondCount > 1)
                    {
                        showLabel = false;
                    }

                    if (bondCount == 2)
                    {
                        Point p1 = atom.Bonds[0].OtherAtom(atom).Position;
                        Point p2 = atom.Bonds[1].OtherAtom(atom).Position;

                        double angle1 = Vector.AngleBetween(-(atom.Position - p1), atom.Position - p2);

                        if (Math.Abs(angle1) < 8)
                        {
                            showLabel = true;
                        }
                    }

                    // Force on if atom has charge
                    if (iAbsCharge > 0)
                    {
                        showLabel = true;
                    }
                    // Force on if atom has isotope value
                    if (isoValue > 0)
                    {
                        showLabel = true;
                    }
                }
            }

            #endregion Decide if atom label is to be displayed

            if (showLabel)
            {
                #region Set Up Atom Colours

                string atomColour = "000000";
                if (options.ColouredAtoms)
                {
                    if ((atom.Element as Element).Colour != null)
                    {
                        atomColour = ((Element)atom.Element).Colour;
                        // Strip out # as OoXml does not use it
                        atomColour = atomColour.Replace("#", "");
                    }
                }

                #endregion Set Up Atom Colours

                #region Step 1 - Measure Bounding Box for all Characters of label

                double xMin = double.MaxValue;
                double yMin = double.MaxValue;
                double xMax = double.MinValue;
                double yMax = double.MinValue;

                Point thisCharacterPosition;
                for (int idx = 0; idx < atomLabel.Length; idx++)
                {
                    char chr = atomLabel[idx];
                    TtfCharacter c = m_TtfCharacterSet[chr];
                    if (c != null)
                    {
                        thisCharacterPosition = GetCharacterPosition(cursorPosition, c);

                        xMin = Math.Min(xMin, thisCharacterPosition.X);
                        yMin = Math.Min(yMin, thisCharacterPosition.Y);
                        xMax = Math.Max(xMax, thisCharacterPosition.X + OoXmlHelper.ScaleCsTtfToCml(c.Width));
                        yMax = Math.Max(yMax, thisCharacterPosition.Y + OoXmlHelper.ScaleCsTtfToCml(c.Height));

                        // Uncomment the following to disable offsetting for terminal atoms
                        //if (bonds.Count == 1)
                        //{
                        //    break;
                        //}

                        if (idx < atomLabel.Length - 1)
                        {
                            // Move to next Character position
                            cursorPosition.Offset(OoXmlHelper.ScaleCsTtfToCml(c.IncrementX), 0);
                        }
                    }
                }

                #endregion Step 1 - Measure Bounding Box for all Characters of label

                #region Step 2 - Reset Cursor such that the text is centered about the atom's co-ordinates

                double width = xMax - xMin;
                double height = yMax - yMin;
                cursorPosition = new Point(atom.Position.X - width / 2, atom.Position.Y + height / 2);
                chargeCursorPosition = new Point(cursorPosition.X, cursorPosition.Y);
                isotopeCursorPosition = new Point(cursorPosition.X, cursorPosition.Y);
                labelBounds = new Rect(cursorPosition, new Size(width, height));
                //_telemetry.Write(module, "Debugging", $"Atom {atomLabel} [{atom.Id}] Label Bounds {labelBounds}");

                #endregion Step 2 - Reset Cursor such that the text is centered about the atom's co-ordinates

                #region Step 3 - Place the characters

                foreach (char chr in atomLabel)
                {
                    TtfCharacter c = m_TtfCharacterSet[chr];
                    if (c != null)
                    {
                        thisCharacterPosition = GetCharacterPosition(cursorPosition, c);
                        AtomLabelCharacter alc = new AtomLabelCharacter(thisCharacterPosition, c, atomColour, chr, atom.Id);
                        m_AtomLabelCharacters.Add(alc);

                        // Move to next Character position
                        lastOffset = OoXmlHelper.ScaleCsTtfToCml(c.IncrementX);
                        cursorPosition.Offset(OoXmlHelper.ScaleCsTtfToCml(c.IncrementX), 0);
                        chargeCursorPosition = new Point(cursorPosition.X, cursorPosition.Y);
                    }
                }

                #endregion Step 3 - Place the characters

                #region Determine NESW

                double baFromNorth = Vector.AngleBetween(BasicGeometry.ScreenNorth(), atom.BalancingVector);
                CompassPoints nesw = CompassPoints.East;

                if (bondCount == 1)
                {
                    nesw = BasicGeometry.SnapTo2EW(baFromNorth);
                }
                else
                {
                    nesw = BasicGeometry.SnapTo4NESW(baFromNorth);
                }

                #endregion Determine NESW

                #region Step 4 - Add Implicit H if required

                if (options.ShowHydrogens && implicitHCount > 0)
                {
                    TtfCharacter hydrogenCharacter = m_TtfCharacterSet['H'];
                    string numbers = "012345";
                    TtfCharacter implicitValueCharacter = m_TtfCharacterSet[numbers[implicitHCount]];

                    #region Add H

                    if (hydrogenCharacter != null)
                    {
                        switch (nesw)
                        {
                            case CompassPoints.North:
                                if (atom.Bonds.Count > 1)
                                {
                                    cursorPosition.X = labelBounds.X + (labelBounds.Width / 2) - (OoXmlHelper.ScaleCsTtfToCml(hydrogenCharacter.Width) / 2);
                                    cursorPosition.Y = cursorPosition.Y +
                                                       OoXmlHelper.ScaleCsTtfToCml(-hydrogenCharacter.Height) -
                                                       OoXmlHelper.CHARACTER_CLIPPING_MARGIN;
                                    if (iCharge > 0)
                                    {
                                        if (implicitHCount > 1)
                                        {
                                            cursorPosition.Offset(0, OoXmlHelper.ScaleCsTtfToCml(-implicitValueCharacter.Height * OoXmlHelper.SUBSCRIPT_SCALE_FACTOR / 2) - OoXmlHelper.CHARACTER_CLIPPING_MARGIN);
                                        }
                                    }
                                }
                                break;

                            case CompassPoints.East:
                                // Leave as is
                                break;

                            case CompassPoints.South:
                                if (atom.Bonds.Count > 1)
                                {
                                    cursorPosition.X = labelBounds.X + (labelBounds.Width / 2) - (OoXmlHelper.ScaleCsTtfToCml(hydrogenCharacter.Width) / 2);
                                    cursorPosition.Y = cursorPosition.Y +
                                                       OoXmlHelper.ScaleCsTtfToCml(hydrogenCharacter.Height) +
                                                       OoXmlHelper.CHARACTER_CLIPPING_MARGIN;
                                }
                                break;

                            case CompassPoints.West:
                                if (implicitHCount == 1)
                                {
                                    cursorPosition.Offset(OoXmlHelper.ScaleCsTtfToCml(-(hydrogenCharacter.IncrementX * 2)), 0);
                                }
                                else
                                {
                                    cursorPosition.Offset(OoXmlHelper.ScaleCsTtfToCml(-(hydrogenCharacter.IncrementX * 2.5)), 0);
                                }
                                break;
                        }

                        //_telemetry.Write(module, "Debugging", $"Adding H at {cursorPosition}");
                        thisCharacterPosition = GetCharacterPosition(cursorPosition, hydrogenCharacter);
                        AtomLabelCharacter alc = new AtomLabelCharacter(thisCharacterPosition, hydrogenCharacter, atomColour, 'H', atom.Id);
                        m_AtomLabelCharacters.Add(alc);

                        // Move to next Character position
                        cursorPosition.Offset(OoXmlHelper.ScaleCsTtfToCml(hydrogenCharacter.IncrementX), 0);

                        if (nesw == CompassPoints.East)
                        {
                            chargeCursorPosition = new Point(cursorPosition.X, cursorPosition.Y);
                        }
                        if (nesw == CompassPoints.West)
                        {
                            isotopeCursorPosition = new Point(thisCharacterPosition.X, isotopeCursorPosition.Y);
                        }
                    }

                    #endregion Add H

                    #region Add number

                    if (implicitHCount > 1)
                    {
                        if (implicitValueCharacter != null)
                        {
                            thisCharacterPosition = GetCharacterPosition(cursorPosition, implicitValueCharacter);

                            // Drop the subscript Character
                            thisCharacterPosition.Offset(0, OoXmlHelper.ScaleCsTtfToCml(hydrogenCharacter.Width * OoXmlHelper.SUBSCRIPT_DROP_FACTOR));

                            AtomLabelCharacter alc = new AtomLabelCharacter(thisCharacterPosition, implicitValueCharacter, atomColour, numbers[implicitHCount], atom.Id);
                            alc.IsSubScript = true;
                            m_AtomLabelCharacters.Add(alc);

                            // Move to next Character position
                            cursorPosition.Offset(OoXmlHelper.ScaleCsTtfToCml(implicitValueCharacter.IncrementX) * OoXmlHelper.SUBSCRIPT_SCALE_FACTOR, 0);
                        }
                    }

                    #endregion Add number
                }

                #endregion Step 4 - Add Implicit H if required

                #region Step 5 - Add Charge if required

                if (iCharge != 0)
                {
                    TtfCharacter hydrogenCharacter = m_TtfCharacterSet['H'];

                    char sign = '.';
                    TtfCharacter chargeSignCharacter = null;
                    if (iCharge >= 1)
                    {
                        sign = '+';
                        chargeSignCharacter = m_TtfCharacterSet['+'];
                    }
                    else if (iCharge <= 1)
                    {
                        sign = '-';
                        chargeSignCharacter = m_TtfCharacterSet['-'];
                    }

                    if (iAbsCharge > 1)
                    {
                        string digits = iAbsCharge.ToString();
                        // Insert digits
                        foreach (char chr in digits)
                        {
                            TtfCharacter chargeValueCharacter = m_TtfCharacterSet[chr];
                            thisCharacterPosition = GetCharacterPosition(chargeCursorPosition, chargeValueCharacter);

                            // Raise the superscript Character
                            thisCharacterPosition.Offset(0, -OoXmlHelper.ScaleCsTtfToCml(chargeValueCharacter.Height * OoXmlHelper.CS_SUPERSCRIPT_RAISE_FACTOR));

                            AtomLabelCharacter alcc = new AtomLabelCharacter(thisCharacterPosition, chargeValueCharacter, atomColour, chr, atom.Id);
                            alcc.IsSubScript = true;
                            m_AtomLabelCharacters.Add(alcc);

                            // Move to next Character position
                            chargeCursorPosition.Offset(OoXmlHelper.ScaleCsTtfToCml(chargeValueCharacter.IncrementX) * OoXmlHelper.SUBSCRIPT_SCALE_FACTOR, 0);
                        }
                    }

                    // Insert sign at raised position
                    thisCharacterPosition = GetCharacterPosition(chargeCursorPosition, chargeSignCharacter);
                    thisCharacterPosition.Offset(0, -OoXmlHelper.ScaleCsTtfToCml(hydrogenCharacter.Height * OoXmlHelper.CS_SUPERSCRIPT_RAISE_FACTOR));
                    thisCharacterPosition.Offset(0, -OoXmlHelper.ScaleCsTtfToCml(chargeSignCharacter.Height / 2 * OoXmlHelper.CS_SUPERSCRIPT_RAISE_FACTOR));

                    AtomLabelCharacter alcs = new AtomLabelCharacter(thisCharacterPosition, chargeSignCharacter, atomColour, sign, atom.Id);
                    alcs.IsSubScript = true;
                    m_AtomLabelCharacters.Add(alcs);
                }

                #endregion Step 5 - Add Charge if required

                #region Step 6 Add IsoTope Number if required

                if (isoValue > 0)
                {
                    string digits = isoValue.ToString();

                    xMin = double.MaxValue;
                    yMin = double.MaxValue;
                    xMax = double.MinValue;
                    yMax = double.MinValue;

                    Point isoOrigin = isotopeCursorPosition;

                    // Calculate width of digits
                    foreach (char chr in digits)
                    {
                        TtfCharacter c = m_TtfCharacterSet[chr];
                        thisCharacterPosition = GetCharacterPosition(isotopeCursorPosition, c);

                        // Raise the superscript Character
                        thisCharacterPosition.Offset(0, -OoXmlHelper.ScaleCsTtfToCml(c.Height * OoXmlHelper.CS_SUPERSCRIPT_RAISE_FACTOR));

                        xMin = Math.Min(xMin, thisCharacterPosition.X);
                        yMin = Math.Min(yMin, thisCharacterPosition.Y);
                        xMax = Math.Max(xMax, thisCharacterPosition.X + OoXmlHelper.ScaleCsTtfToCml(c.Width));
                        yMax = Math.Max(yMax, thisCharacterPosition.Y + OoXmlHelper.ScaleCsTtfToCml(c.Height));

                        // Move to next Character position
                        isotopeCursorPosition.Offset(OoXmlHelper.ScaleCsTtfToCml(c.IncrementX) * OoXmlHelper.SUBSCRIPT_SCALE_FACTOR, 0);
                    }

                    // Re-position Isotope Cursor
                    width = xMax - xMin;
                    isotopeCursorPosition = new Point(isoOrigin.X - width, isoOrigin.Y);

                    // Insert digits
                    foreach (char chr in digits)
                    {
                        TtfCharacter c = m_TtfCharacterSet[chr];
                        thisCharacterPosition = GetCharacterPosition(isotopeCursorPosition, c);

                        // Raise the superscript Character
                        thisCharacterPosition.Offset(0, -OoXmlHelper.ScaleCsTtfToCml(c.Height * OoXmlHelper.CS_SUPERSCRIPT_RAISE_FACTOR));

                        AtomLabelCharacter alcc = new AtomLabelCharacter(thisCharacterPosition, c, atomColour, chr, atom.Id);
                        alcc.IsSubScript = true;
                        m_AtomLabelCharacters.Add(alcc);

                        // Move to next Character position
                        isotopeCursorPosition.Offset(OoXmlHelper.ScaleCsTtfToCml(c.IncrementX) * OoXmlHelper.SUBSCRIPT_SCALE_FACTOR, 0);
                    }
                }

                #endregion Step 6 Add IsoTope Number if required
            }
        }

        private Point GetCharacterPosition(Point cursorPosition, TtfCharacter character)
        {
            // Add the (negative) OriginY to raise the character by it
            return new Point(cursorPosition.X, cursorPosition.Y + OoXmlHelper.ScaleCsTtfToCml(character.OriginY));
            // return new Point(cursorPosition.X, cursorPosition.Y + OoXmlHelper.ScaleCsTtfToCml(character.Height) - OoXmlHelper.ScaleCsTtfToCml(character.Height - character.OriginY));
        }
    }
}