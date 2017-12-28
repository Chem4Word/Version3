// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Model;
using Chem4Word.Model.Enums;
using Chem4Word.Model.Geometry;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Chem4Word.View
{
    public class BondShape : Shape
    {
        private List<Point> _enclosingPoly = new List<Point>();

        private static FrameworkPropertyMetadataOptions _everyOption
            = FrameworkPropertyMetadataOptions.AffectsRender
            | FrameworkPropertyMetadataOptions.AffectsMeasure
            | FrameworkPropertyMetadataOptions.AffectsArrange
            | FrameworkPropertyMetadataOptions.AffectsParentMeasure
            | FrameworkPropertyMetadataOptions.AffectsParentArrange;

        public BondShape()
        {
        }

        //private System.Windows.Media.Geometry hitGeometry;

        public Canvas DrawingCanvas
        {
            get { return (Canvas)GetValue(DrawingCanvasProperty); }
            set { SetValue(DrawingCanvasProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DrawingCanvas.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DrawingCanvasProperty =
            DependencyProperty.Register("DrawingCanvas", typeof(Canvas), typeof(BondShape), new FrameworkPropertyMetadata(null,
                _everyOption, DrawingCanvasChanged));

        private static void DrawingCanvasChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
        }

        public Point? StartPoint
        {
            get { return (Point?)GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register("StartPoint", typeof(Point?), typeof(BondShape), new FrameworkPropertyMetadata(null, _everyOption, StartPointChanged));

        private static void StartPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
        }

        internal static readonly DependencyPropertyKey AngleKey = DependencyProperty.RegisterReadOnly("Angle",
                    typeof(double), typeof(BondShape), new FrameworkPropertyMetadata(0.0, _everyOption));

        public static readonly DependencyProperty AngleProperty = AngleKey.DependencyProperty;

        public double Angle
        {
            get { return Vector.AngleBetween(BasicGeometry.ScreenNorth(), EndPoint.Value - StartPoint.Value); }
        }

        public Point? EndPoint
        {
            get
            {
                return (Point?)GetValue(EndPointProperty);
            }
            set
            {
                SetValue(EndPointProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for EndPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndPointProperty =
            DependencyProperty.Register("EndPoint", typeof(Point?), typeof(BondShape),
                new FrameworkPropertyMetadata(null, _everyOption,
                    EndPointChanged));

        private static void EndPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            //SetFillAngle(d);
        }

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            //clips the bond geometry by the space of the start and end atom
            get
            {
                var totalgeo = GetBondGeometry(this.StartPoint, this.EndPoint);
                if (totalgeo != null)
                {
                    if (!string.IsNullOrEmpty(ParentBond.StartAtom?.SymbolText) |
                        !string.IsNullOrEmpty(ParentBond.EndAtom?.SymbolText))

                    {
                        Pen newPen = new Pen(Brushes.Black, 0.1);
                        if (ParentBond.Stereo == BondStereo.Hatch | ParentBond.Stereo == BondStereo.Wedge)
                        {
                            totalgeo = totalgeo.GetOutlinedPathGeometry();
                        }
                        else
                        {
                            totalgeo = totalgeo.GetWidenedPathGeometry(newPen, 0.001, ToleranceType.Absolute);
                        }

                        if (!string.IsNullOrEmpty(ParentBond.StartAtom?.SymbolText))
                        {
                            var atomGeo = AtomShape.GetBoxGeometry(ParentBond.StartAtom);
                            totalgeo = new CombinedGeometry(GeometryCombineMode.Exclude, totalgeo, atomGeo);
                        }

                        if (!string.IsNullOrEmpty(ParentBond.EndAtom?.SymbolText))
                        {
                            var atomGeo = AtomShape.GetBoxGeometry(ParentBond.EndAtom);
                            totalgeo = new CombinedGeometry(GeometryCombineMode.Exclude, totalgeo, atomGeo);
                        }
                    }
                    totalgeo.Freeze();
                }
                return totalgeo;
            }
        }

        public Bond ParentBond
        {
            get { return (Bond)GetValue(ParentBondProperty); }
            set { SetValue(ParentBondProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ParentBond.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ParentBondProperty =
            DependencyProperty.Register("ParentBond", typeof(Bond), typeof(BondShape), new PropertyMetadata(null));

        public BondDirection Placement
        {
            get { return (BondDirection)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Placement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlacementProperty =
            DependencyProperty.Register("Placement", typeof(BondDirection), typeof(BondShape), new PropertyMetadata(BondDirection.None));

        public BondStereo Stereo
        {
            get { return (BondStereo)GetValue(StereoProperty); }
            set { SetValue(StereoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Stereo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StereoProperty =
            DependencyProperty.Register("Stereo", typeof(BondStereo), typeof(BondShape), new PropertyMetadata(BondStereo.None));

        public int OrderValue
        {
            get { return (int)GetValue(OrderValueProperty); }
            set { SetValue(OrderValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OrderValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrderValueProperty =
            DependencyProperty.Register("OrderValue", typeof(int), typeof(BondShape), new PropertyMetadata(1));

        public System.Windows.Media.Geometry GetBondGeometry(Point? startPoint, Point? endPoint)
        {
            //Vector startOffset = new Vector();
            //Vector endOffset = new Vector();

            if (startPoint != null & endPoint != null)
            {
                //check to see if it's a wedge or a hatch yet
                if (ParentBond.Stereo == BondStereo.Wedge | ParentBond.Stereo == BondStereo.Hatch)
                {
                    return BondGeometry.WedgeBondGeometry(startPoint.Value, endPoint.Value);
                }

                if (ParentBond.Stereo == BondStereo.Indeterminate && ParentBond.OrderValue == 1.0)
                {
                    return BondGeometry.WavyBondGeometry(startPoint.Value, endPoint.Value);
                }

                //single or dotted bond
                if (ParentBond.OrderValue <= 1)
                {
                    return BondGeometry.SingleBondGeometry(startPoint.Value, endPoint.Value);
                }
                if (ParentBond.OrderValue == 1.5)
                {
                    //it's a resonance bond, so we deal with this in OnRender
                    //return BondGeometry.SingleBondGeometry(startPoint.Value, endPoint.Value);
                    return new StreamGeometry();
                }

                //double bond
                if (ParentBond.OrderValue == 2)
                {
                    if (ParentBond.Stereo == BondStereo.Indeterminate)
                    {
                        return BondGeometry.CrossedDoubleGeometry(startPoint.Value, endPoint.Value, ref _enclosingPoly);
                    }
                    Point? centroid = null;
                    if (ParentBond.IsCyclic())
                    {
                        centroid = ParentBond.PrimaryRing?.Centroid;
                    }
                    return BondGeometry.DoubleBondGeometry(startPoint.Value, endPoint.Value, Placement,
                        ref _enclosingPoly, centroid);
                }
                //tripe bond
                if (ParentBond.OrderValue == 3)
                {
                    return BondGeometry.TripleBondGeometry(startPoint.Value, endPoint.Value, ref _enclosingPoly);
                }

                return null;
            }

            return null;
        }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            if (ParentBond.Stereo == BondStereo.Wedge | ParentBond.Stereo == BondStereo.Hatch)
            {
                return base.HitTestCore(hitTestParameters);
            }
            else
            {
                Pen widepen = new Pen(Brushes.Black, 20.0);

                if (DefiningGeometry.StrokeContains(widepen, hitTestParameters.HitPoint))
                {
                    return new PointHitTestResult(this, hitTestParameters.HitPoint);
                }
                else
                {
                    return null;
                }
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (ParentBond.OrderValue == 1.5)
            {
                Point point1, point2, point3, point4;

                Point? centroid = null;
                if (ParentBond.IsCyclic())
                {
                    centroid = ParentBond.PrimaryRing?.Centroid;
                }
                _enclosingPoly = BondGeometry.GetDoubleBondPoints(StartPoint.Value, EndPoint.Value,
                    Placement, centroid, out point1,
                    out point2, out point3, out point4);
                Pen solidPen = new Pen(Fill, StrokeThickness);

                Pen dashedPen = solidPen.Clone();

                dashedPen.DashStyle = DashStyles.Dash;

                drawingContext.DrawLine(solidPen, point1, point2);
                drawingContext.DrawLine(dashedPen, point3, point4);

                //drawingContext.Close();
            }
            else
            {
                base.OnRender(drawingContext);
            }
        }

        public string Id => ParentBond.Id;
    }
}