// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Model;
using Chem4Word.Model.Geometry;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using static Chem4Word.View.Globals;

namespace Chem4Word.View
{
    public class AtomShape : Shape
    {
        //private static FrameworkPropertyMetadataOptions _everyOption
        //    = FrameworkPropertyMetadataOptions.AffectsRender
        //    | FrameworkPropertyMetadataOptions.AffectsMeasure
        //    | FrameworkPropertyMetadataOptions.AffectsArrange
        //    | FrameworkPropertyMetadataOptions.AffectsParentMeasure
        //    | FrameworkPropertyMetadataOptions.AffectsParentArrange;

        public AtomShape()
        {
        }

        #region Overrides

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get
            {
                if (ParentAtom.SymbolText == "")
                {
                    /*return a circle - this is styled in XAML as Transparent
                 but highlighted on a  trigger - in the editor anyhow.
                 The atomic symbol block will handle the text */

                    var basicAtomGeo = new EllipseGeometry(Position, AtomWidth / 2, AtomWidth / 2);
                    var isoGeo = DrawIsotopeLabel(ParentAtom, basicAtomGeo);
                    return isoGeo;
                }
                /* return the geometry of the atomic symbol */
                //first get the text

                var textGeometry = GetSymbolGeometry(this.ParentAtom);
                return textGeometry;
            }
        }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            //first work out what the atom bouding box is:
            Pen widepen = new Pen(Brushes.Black, 3.0);
            var bb = DefiningGeometry.GetRenderBounds(widepen);

            if (bb.Contains(hitTestParameters.HitPoint))
                return new PointHitTestResult(this, hitTestParameters.HitPoint);

            return null;
        }

        #endregion Overrides

        //atom symbol drawing code

        /// <summary>
        ///
        /// </summary>
        /// <param name="parentAtom"></param>
        /// <returns></returns>
        public Geometry GetSymbolGeometry(Atom parentAtom)
        {
            const double nextSymbolPadding = 5.0;
            Point startingPoint = parentAtom.Position;
            Point nextPos;

            //double width = 0.0;

            var symbolText = BuildSymbolLabel(parentAtom.SymbolText, ref startingPoint, out nextPos);

            var symbolGeometry = symbolText.BuildGeometry(startingPoint);

            symbolGeometry = DrawHydrogens(parentAtom, nextPos, symbolGeometry, nextSymbolPadding);
            symbolGeometry = DrawCharges(parentAtom, symbolGeometry);
            symbolGeometry = DrawIsotopeLabel(parentAtom, symbolGeometry);
            symbolGeometry.Freeze();
            return symbolGeometry;
        }

        private Geometry DrawIsotopeLabel(Atom parentAtom, Geometry symbolGeometry)
        {
            Point startingPoint = parentAtom.Position;
            Point nextPos;

            if (parentAtom.IsotopeNumber == null)
            {
                return symbolGeometry;
            }
            else
            {
                Geometry atomGeo = null;
                string isoLabel = Isotope.ToString();

                if (!(symbolGeometry is EllipseGeometry)) //that is, weve got an explcit, non carbon element
                {
                    var atomText = BuildSymbolLabel(parentAtom.SymbolText, ref startingPoint, out nextPos);

                    atomGeo = atomText.BuildGeometry(startingPoint);
                }
                else //its a vertex carbon
                {
                    atomGeo = symbolGeometry;
                }

                //have a stab at placing the isotope label.

                //placementvector gives the CENTRE of the charge text!
                //first generate  a vector pointing straight up
                Vector placementVector = new Vector(0.0, -Math.Max(atomGeo.Bounds.Width, atomGeo.Bounds.Height));
                Matrix rotator = new Matrix();
                //isotope labels always go  in at 10-o-clock from the atom.
                rotator.Rotate(-60);
                placementVector *= rotator;

                //Point firstPlacementPoint = new Point(atomGeo.Bounds.Right, atomGeo.Bounds.Top - chargeText.Height/2);

                FormattedText isoText = GetSubscriptText(isoLabel);

                Point firstPlacementPoint = parentAtom.Position + placementVector -
                                            new Vector(isoText.Width / 2, isoText.Height / 2);

                System.Windows.Media.Geometry isoGeo = isoText.BuildGeometry(firstPlacementPoint);

                return new CombinedGeometry(symbolGeometry, isoGeo);
            }
        }

        //draws a charge symbol at two-o-clock to the atom
        private System.Windows.Media.Geometry DrawCharges(Atom parentAtom, System.Windows.Media.Geometry symbolGeometry)
        {
            Point startingPoint = parentAtom.Position;
            Point nextPos;

            if ((Charge ?? 0) != 0)
            {
                string sign;
                string count;
                GetChargeText(out sign, out count);

                string chargeString = count + sign;

                FormattedText chargeText = GetSubscriptText(chargeString);
                FormattedText chargeDigit;
                if (Charge > 1)
                {
                    chargeDigit = GetSubscriptText(count.ToString());
                }
                else
                {
                    chargeDigit = GetSubscriptText("+");
                }

                var atomText = BuildSymbolLabel(parentAtom.SymbolText, ref startingPoint, out nextPos);

                var atomGeo = atomText.BuildGeometry(startingPoint);

                //have a stab at placing the charge.

                //placementvector gives the CENTRE of the charge text!
                Vector placementVector = new Vector(Math.Max(atomGeo.Bounds.Width, atomGeo.Bounds.Height), 0.0);
                Matrix rotator = new Matrix();
                rotator.Rotate(-30);
                placementVector *= rotator;

                //Point firstPlacementPoint = new Point(atomGeo.Bounds.Right, atomGeo.Bounds.Top - chargeText.Height/2);
                Point firstPlacementPoint;

                firstPlacementPoint = parentAtom.Position + placementVector -
                                      new Vector(chargeDigit.Width / 2, chargeDigit.Height / 2); //offset the text

                //If it overlaps the existing geometry, move it around
                Geometry outlineGeometry = symbolGeometry.GetOutlinedPathGeometry(0.0001, ToleranceType.Relative);

                System.Windows.Media.Geometry chargeGeo = chargeText.BuildGeometry(firstPlacementPoint);
                int iCount = 0;
                while (iCount < 12 && symbolGeometry.FillContainsWithDetail(chargeGeo) != IntersectionDetail.Empty)
                {
                    rotator.Rotate(-20);
                    placementVector *= rotator;
                    chargeGeo = chargeText.BuildGeometry(parentAtom.Position + placementVector);
                    iCount++;
                }

                return new CombinedGeometry(symbolGeometry, chargeGeo);
            }
            else
            {
                //don't draw Anything
                return symbolGeometry;
            }
        }

        private void GetChargeText(out string sign, out string count)
        {
            sign = (Charge ?? 0) > 0 ? "+" : "-";

            count = Math.Abs(Charge ?? 0) > 1 ? Math.Abs(Charge ?? 0).ToString() : "";
        }

        /// <summary>
        /// Draws any implicit hydrogens in the right place
        /// </summary>
        /// <param name="parentAtom">Owner of the hydrogens</param>
        /// <param name="nextPos">Where the insertion point ends up</param>
        /// <param name="symbolGeometry">Pre-drawn geometry of the atomic symbol text</param>
        /// <param name="nextSymbolPadding">How much space to insert between symbols</param>
        /// <returns>geometry of the symbol text plus hydrogens and any subscripts</returns>
        private System.Windows.Media.Geometry DrawHydrogens(Atom parentAtom, Point nextPos, System.Windows.Media.Geometry symbolGeometry, double nextSymbolPadding)
        {
            if (parentAtom.ImplicitHydrogenCount >= 1)
            {
                System.Windows.Media.Geometry hGeometry;
                if (parentAtom.Bonds.Count == 0)
                {
                    hGeometry = DrawHEast(parentAtom, nextPos);
                    DefaultHOrientation = CompassPoints.East;
                }
                else if (parentAtom.Bonds.Count == 1)
                {
                    if (Vector.AngleBetween(BasicGeometry.ScreenNorth(),
                        parentAtom.Bonds[0].OtherAtom(parentAtom).Position - parentAtom.Position) > 0)
                    //the bond is on the right
                    {
                        hGeometry = DrawHWest(parentAtom, nextPos, symbolGeometry, nextSymbolPadding);
                        DefaultHOrientation = CompassPoints.West;
                    }
                    else
                    {
                        //default to any old rubbish for now
                        DefaultHOrientation = CompassPoints.East;
                        hGeometry = DrawHEast(parentAtom, nextPos);
                    }
                }
                else
                {
                    double baFromNorth = Vector.AngleBetween(BasicGeometry.ScreenNorth(),
                        parentAtom.BalancingVector);

                    switch (BasicGeometry.SnapTo4NESW(baFromNorth))
                    {
                        case CompassPoints.East:
                            hGeometry = DrawHEast(parentAtom, nextPos);
                            DefaultHOrientation = CompassPoints.East;
                            break;

                        case CompassPoints.North:
                            hGeometry = DrawHNorth(parentAtom, nextPos, symbolGeometry, nextSymbolPadding);
                            DefaultHOrientation = CompassPoints.North;
                            break;

                        case CompassPoints.South:
                            hGeometry = DrawHSouth(parentAtom, nextPos, symbolGeometry, nextSymbolPadding);
                            DefaultHOrientation = CompassPoints.South;
                            break;

                        case CompassPoints.West:
                            hGeometry = DrawHWest(parentAtom, nextPos, symbolGeometry, nextSymbolPadding);
                            DefaultHOrientation = CompassPoints.West;
                            break;

                        default:
                            hGeometry = DrawHEast(parentAtom, nextPos);
                            DefaultHOrientation = CompassPoints.East;
                            break;
                    }
                }
                symbolGeometry = new CombinedGeometry(symbolGeometry, hGeometry);
            }
            return symbolGeometry;
        }

        /// <summary>
        /// Where the H atoms ended up getting drawn by default
        /// </summary>
        public CompassPoints? DefaultHOrientation { get; set; }

        private static System.Windows.Media.Geometry DrawHSouth(Atom parentAtom, Point nextPos, System.Windows.Media.Geometry symbolGeometry, double nextSymbolPadding)
        {
            //first measure up the H geometry
            //var tempPos = nextPos;
            //System.Windows.Media.Geometry tempGeo = GetSubscriptedGeometry("H", ref tempPos,
            //    parentAtom.ImplicitHydrogenCount);
            Point tempPos = new Point(0, 0);

            System.Windows.Media.Geometry oneHGeo = GetSubscriptedGeometry("H", ref tempPos, 0);//get the width of one "H"

            //var tempPos = symbolGeometry.Bounds.BottomLeft;

            tempPos.X = symbolGeometry.Bounds.Left + (symbolGeometry.Bounds.Width - oneHGeo.Bounds.Width) / 2 - nextSymbolPadding / 2; ;
            tempPos.Y = symbolGeometry.Bounds.Bottom;

            return GetSubscriptedGeometry("H", ref tempPos, parentAtom.ImplicitHydrogenCount);
        }

        private static System.Windows.Media.Geometry DrawHNorth(Atom parentAtom, Point nextPos, System.Windows.Media.Geometry symbolGeometry, double nextSymbolPadding)
        {
            //first measure up the H geometry
            var topLeft = symbolGeometry.Bounds.TopLeft;
            Point tempPos = topLeft;
            System.Windows.Media.Geometry oneHGeo = GetSubscriptedGeometry("H", ref tempPos, 0);//get the width of one "H"

            System.Windows.Media.Geometry tempGeo = GetSubscriptedGeometry("H", ref tempPos,
                parentAtom.ImplicitHydrogenCount);//and get the dimensions of the same H with its subscript

            nextPos.Y = symbolGeometry.Bounds.Top - oneHGeo.Bounds.Height - nextSymbolPadding;
            nextPos.X = symbolGeometry.Bounds.Left + (symbolGeometry.Bounds.Width - oneHGeo.Bounds.Width) / 2 - nextSymbolPadding / 2;

            return GetSubscriptedGeometry("H", ref nextPos, parentAtom.ImplicitHydrogenCount);
        }

        private static System.Windows.Media.Geometry DrawHEast(Atom parentAtom, Point nextPos)
        {
            return GetSubscriptedGeometry("H", ref nextPos, parentAtom.ImplicitHydrogenCount);
        }

        private System.Windows.Media.Geometry DrawHWest(Atom parentAtom, Point nextPos, System.Windows.Media.Geometry symbolGeometry, double NextSymbolPadding)
        {
            //push the subscript to the left
            var tempPos = nextPos;
            //measure the geometry of the subscripted H
            var tempGeo = GetSubscriptedGeometry("H", ref tempPos, parentAtom.ImplicitHydrogenCount);
            var bounds = tempGeo.Bounds;
            var width = bounds.Width;
            //now we've got the bounds, draw it for real
            //move the insertion point left)
            nextPos.X = symbolGeometry.Bounds.Left - (width + NextSymbolPadding);
            var hGeometry = GetSubscriptedGeometry("H", ref nextPos, parentAtom.ImplicitHydrogenCount);
            return hGeometry;
        }

        private static System.Windows.Media.Geometry GetSubscriptedGeometry(string symbol, ref Point nextPos, int implicitHydrogenCount = 0)
        {
            FormattedText elementText = GetFormattedSymbolText(symbol);

            System.Windows.Media.Geometry geo = elementText.BuildGeometry(nextPos);

            nextPos += new Vector(elementText.Width, 0.0);

            if (implicitHydrogenCount > 1)
            {
                var subText = GetSubscriptText(implicitHydrogenCount.ToString());
                //move the current drawing position
                //and drop the starting point for the subscript
                var subscriptStartPos = nextPos + new Vector(0.0, (elementText.Height - SubscriptHeight / 2));
                var subGeo = subText.BuildGeometry(subscriptStartPos);
                geo = new CombinedGeometry(geo, subGeo);
                nextPos += new Vector(subText.Width, 0.0);
            }

            return geo;
        }

        private static FormattedText BuildSymbolLabel(string parentAtomText, ref Point startingPoint, out Point currentPosition)
        {
            var formattedText = GetFormattedSymbolText(parentAtomText);
            //offset the text by half its width and height
            double xOffset = 0.0, yOffset = 0.0;
            xOffset = formattedText.Width / 2;
            yOffset = formattedText.Height / 2;

            var descenderHeight = formattedText.Height - formattedText.Baseline;
            startingPoint = new Point(startingPoint.X - xOffset, startingPoint.Y - yOffset
                + descenderHeight);

            currentPosition = startingPoint + new Vector(formattedText.Width, 0.0);

            return formattedText;
        }

        private static FormattedText GetFormattedSymbolText(string symbolText)
        {
            FormattedText formattedText = new FormattedText(
                symbolText,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Ariel"),
                FontHeight,
                Brushes.Black);
            formattedText.LineHeight = FontHeight;
            formattedText.SetFontWeight(System.Windows.FontWeights.Bold);
            formattedText.TextAlignment = TextAlignment.Left;

            return formattedText;
        }

        private static FormattedText GetSubscriptText(string symbolText)
        {
            FormattedText formattedText = new FormattedText(
                symbolText,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Ariel"),
                SubscriptHeight,
                Brushes.Black);
            formattedText.LineHeight = SubscriptHeight;
            formattedText.SetFontWeight(System.Windows.FontWeights.Bold);
            return formattedText;
        }

        /// <summary>
        /// gets the geometry of the bounding box of the atom label
        /// </summary>
        /// <param name="parentAtom">Atom to get the geometry for</param>
        /// <returns></returns>
        public static System.Windows.Media.Geometry GetBoxGeometry(Atom parentAtom)
        {
            Point startingPoint = parentAtom.Position;
            Point currentPos;
            var formattedText = BuildSymbolLabel(parentAtom.SymbolText, ref startingPoint, out currentPos);
            return formattedText.BuildHighlightGeometry(startingPoint);
        }

        public Rect GetBoundingRect(Atom parentAtom)
        {
            if (parentAtom.SymbolText == "")
            {
                return new Rect(parentAtom.Position, new Size(AtomWidth, AtomWidth));
            }

            return GetSymbolGeometry(parentAtom).Bounds;
        }

        /// <summary>
        /// gets the geometry of the bounding box of the atom label
        /// </summary>
        /// <param name="parentAtom">Atom to get the geometry for</param>
        /// <returns></returns>
        public static System.Windows.Media.Geometry GetWidenedGeometry(Atom parentAtom)
        {
            Point startingPoint = parentAtom.Position, currentPos;
            var pen = new Pen(Brushes.Black, 2.0);
            var formattedText = BuildSymbolLabel(parentAtom.SymbolText, ref startingPoint, out currentPos);
            var initialGeo = formattedText.BuildGeometry(startingPoint);
            var borderGeo = initialGeo.GetWidenedPathGeometry(pen);
            var finalgeo = new CombinedGeometry(initialGeo, borderGeo);
            return finalgeo;
        }

        public Canvas DrawingCanvas
        {
            get { return (Canvas)GetValue(DrawingCanvasProperty); }
            set { SetValue(DrawingCanvasProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DrawingCanvas.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DrawingCanvasProperty =
            DependencyProperty.Register("DrawingCanvas", typeof(Canvas), typeof(AtomShape),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits,
                    DrawingCanvasChanged));

        private static void DrawingCanvasChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
        }

        #region Positioning DPs

        public Point Position
        {
            get { return (Point)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Position.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(Point), typeof(AtomShape),
                new FrameworkPropertyMetadata(new Point(0, 0),
                    FrameworkPropertyMetadataOptions.AffectsRender, PositionChanged));

        private static void PositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
        }

        #endregion Positioning DPs

        #region Atom DPs

        public string AtomSymbol
        {
            get { return (string)GetValue(AtomSymbolProperty); }
            set { SetValue(AtomSymbolProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AtomSymbol.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AtomSymbolProperty =
            DependencyProperty.Register("AtomSymbol", typeof(string), typeof(AtomShape),
                new FrameworkPropertyMetadata("",
                    FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure,
                    AtomSymbolChangedCallback));

        private static void AtomSymbolChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
        }

        public Atom ParentAtom
        {
            get { return (Atom)GetValue(ParentAtomProperty); }
            set { SetValue(ParentAtomProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ParentAtom.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ParentAtomProperty =
            DependencyProperty.Register("ParentAtom", typeof(Atom), typeof(AtomShape), new PropertyMetadata(null));

        #endregion Atom DPs

        #region Charge DP

        public int? Charge
        {
            get { return (int?)GetValue(ChargeProperty); }
            set { SetValue(ChargeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Charge.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChargeProperty =
            DependencyProperty.Register("Charge",
                typeof(int?),
                typeof(AtomShape),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure, ChargeChanged));

        private static void ChargeChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var changedAtomShape = (AtomShape)d;
            var newval = (int?)args.NewValue;
        }

        #endregion Charge DP

        public int? Isotope
        {
            get { return (int?)GetValue(IsotopeProperty); }
            set { SetValue(IsotopeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Isotope.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsotopeProperty =
            DependencyProperty.Register("Isotope", typeof(int?), typeof(AtomShape), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure, IsotopeChangedCallback));

        private static void IsotopeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var changedAtomShape = (AtomShape)d;
            var newval = (int?)args.NewValue;
        }
    }
}