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
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace Chem4Word.View
{
    /// <summary>
    /// Static class to define bond geometries
    /// now uses StreamGeometry in preference to PathGeometry
    /// Old code is commented out
    /// </summary>
    public static class BondGeometry
    {
        public static System.Windows.Media.Geometry WedgeBondGeometry(Point startPoint, Point endPoint)
        {
            Vector bondVector = endPoint - startPoint;
            Vector perpVector = bondVector.Perpendicular();
            perpVector.Normalize();
            perpVector *= Globals.WedgeWidth / 2;
            Point point2 = endPoint + perpVector;
            Point point3 = endPoint - perpVector;

            StreamGeometry sg = new StreamGeometry();

            using (StreamGeometryContext sgc = sg.Open())
            {
                sgc.BeginFigure(startPoint, true, true);
                sgc.LineTo(point2, true, true);
                sgc.LineTo(point3, true, true);
                sgc.Close();
            }
            sg.Freeze();

            return sg;
        }

        /// <summary>
        /// Common to both edge and hatch bonds.  The filling of this shape
        /// is done purely in XAML through styles
        /// </summary>
        /// <param name="startPoint">Point object where the bond starts</param>
        /// <param name="angle">The angle from ScreenNorth:  clockwise +ve, anticlockwise -ve</param>
        /// <param name="bondlength">How long the bond is in pixels</param>
        /// <returns></returns>
        public static System.Windows.Media.Geometry WedgeBondGeometry(Point startPoint, double angle, double bondlength)
        {
            //List<PathSegment> wedgesegments = new List<PathSegment>(4);

            //get a right sized vector first
            Vector bondVector = BasicGeometry.ScreenNorth();
            bondVector.Normalize();
            bondVector = bondVector * bondlength;

            //then rotate it to the proper angle
            Matrix rotator = new Matrix();
            rotator.Rotate(angle);
            bondVector = bondVector * rotator;

            //then work out the points at the thick end of the wedge
            var perpVector = bondVector.Perpendicular();
            perpVector.Normalize();
            perpVector = perpVector * Globals.WedgeWidth / 2;

            Point point2 = startPoint + bondVector + perpVector;
            Point point3 = startPoint + bondVector - perpVector;
            //and draw it
            StreamGeometry sg = new StreamGeometry();

            using (StreamGeometryContext sgc = sg.Open())
            {
                sgc.BeginFigure(startPoint, true, true);
                sgc.LineTo(point2, true, true);
                sgc.LineTo(point3, true, true);
                sgc.Close();
            }
            sg.Freeze();

            return sg;
        }

        /// <summary>
        /// Defines the three parallel lines of a Triple bond.
        /// </summary>
        /// <param name="startPoint">Where the bond starts</param>
        /// <param name="endPoint">Where it ends</param>
        /// <param name="enclosingPoly"></param>
        /// <returns></returns>
        public static System.Windows.Media.Geometry TripleBondGeometry(Point startPoint, Point endPoint, ref List<Point> enclosingPoly)
        {
            Vector v = endPoint - startPoint;
            Vector normal = v.Perpendicular();
            normal.Normalize();

            double distance = Globals.Offset;
            Point point1 = startPoint + normal * distance;
            Point point2 = point1 + v;

            Point point3 = startPoint - normal * distance;
            Point point4 = point3 + v;

            enclosingPoly = new List<Point>() { point1, point2, point4, point3 };

            StreamGeometry sg = new StreamGeometry();
            using (StreamGeometryContext sgc = sg.Open())
            {
                sgc.BeginFigure(startPoint, false, false);
                sgc.LineTo(endPoint, true, false);
                sgc.BeginFigure(point1, false, false);
                sgc.LineTo(point2, true, false);
                sgc.BeginFigure(point3, false, false);
                sgc.LineTo(point4, true, false);
                sgc.Close();
            }
            sg.Freeze();

            return sg;
        }

        /// <summary>
        /// draws the two parallel lines of a double bond
        /// These bonds can either straddle the atom-atom line or fall to one or other side of it
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="doubleBondPlacement"></param>
        /// <param name="ringCentroid"></param>
        /// <param name="enclosingPoly"></param>
        /// <returns></returns>
        public static System.Windows.Media.Geometry DoubleBondGeometry(Point startPoint, Point endPoint,
            BondDirection doubleBondPlacement, ref List<Point> enclosingPoly, Point? ringCentroid = null)

        {
            Point point1;
            Point point2;
            Point point3;
            Point point4;
            enclosingPoly = GetDoubleBondPoints(startPoint, endPoint, doubleBondPlacement, ringCentroid, out point1, out point2, out point3, out point4);

            ;

            StreamGeometry sg = new StreamGeometry();
            using (StreamGeometryContext sgc = sg.Open())
            {
                sgc.BeginFigure(point1, false, false);
                sgc.LineTo(point2, true, false);
                sgc.BeginFigure(point3, false, false);
                sgc.LineTo(point4, true, false);
                sgc.Close();
            }
            sg.Freeze();
            return sg;
        }

        /// <summary>
        /// Defines the 4 points that characterise a double bond and returns a list of them in polygon order
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="doubleBondPlacement"></param>
        /// <param name="ringCentroid"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="point3"></param>
        /// <param name="point4"></param>
        /// <returns></returns>
        public static List<Point> GetDoubleBondPoints(Point startPoint, Point endPoint, BondDirection doubleBondPlacement,
            Point? ringCentroid, out Point point1, out Point point2, out Point point3, out Point point4)
        {
            List<Point> enclosingPoly;
            Vector v = endPoint - startPoint;
            Vector normal = v.Perpendicular();
            normal.Normalize();

            Point? point3a, point4a;

            double distance = Globals.Offset;

            if (ringCentroid == null)
            {
                switch (doubleBondPlacement)
                {
                    case BondDirection.None:

                        point1 = startPoint + normal * distance;
                        point2 = point1 + v;

                        point3 = startPoint - normal * distance;
                        point4 = point3 + v;

                        break;

                    case BondDirection.Clockwise:
                        {
                            point1 = startPoint;

                            point2 = endPoint;
                            point3 = startPoint - normal * 2 * distance;
                            point4 = point3 + v;

                            break;
                        }

                    case BondDirection.Anticlockwise:
                        point1 = startPoint;
                        point2 = endPoint;
                        point3 = startPoint + normal * 2 * distance;
                        point4 = point3 + v;
                        break;

                    default:

                        point1 = startPoint + normal * distance;
                        point2 = point1 + v;

                        point3 = startPoint - normal * distance;
                        point4 = point3 + v;
                        break;
                }
            }
            else
            {
                point1 = startPoint;
                point2 = endPoint;

                var bondvector = endPoint - startPoint;
                var centreVector = ringCentroid - startPoint;
                var bondPlacement = (BondDirection)Math.Sign(Vector.CrossProduct(centreVector.Value, bondvector));
                if (bondPlacement == BondDirection.Clockwise)
                {
                    point3 = startPoint - normal * 2 * distance;
                    point4 = point3 + v;
                }
                else
                {
                    point3 = startPoint + normal * 2 * distance;
                    point4 = point3 + v;
                }

                point3a = BasicGeometry.LineSegmentsIntersect(startPoint, ringCentroid.Value, point3, point4);

                var tempPoint3 = point3a ?? point3;

                point4a = BasicGeometry.LineSegmentsIntersect(endPoint, ringCentroid.Value, point3, point4);

                var tempPoint4 = point4 = point4a ?? point4;

                point3 = tempPoint3;
                point4 = tempPoint4;
            }
            //capture  the enclosing polygon for hit testing later

            enclosingPoly = new List<Point>() { point1, point2, point4, point3 };

            //shorten the supporting bond if it's a ring bond
            if (ringCentroid != null)
            {
            }
            return enclosingPoly;
        }

        /// <summary>
        /// Draws the crossed double bond to indicate indeterminate geometry
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="enclosingPoly"></param>
        /// <returns></returns>
        public static Geometry CrossedDoubleGeometry(Point startPoint, Point endPoint, ref List<Point> enclosingPoly)
        {
            Vector v = endPoint - startPoint;
            Vector normal = v.Perpendicular();
            normal.Normalize();

            Point point1, point2, point3, point4;

            double distance = Globals.Offset;

            point1 = startPoint + normal * distance;
            point2 = point1 + v;

            point3 = startPoint - normal * distance;
            point4 = point3 + v;

            enclosingPoly = new List<Point>() { point1, point2, point4, point3 };

            StreamGeometry sg = new StreamGeometry();
            using (StreamGeometryContext sgc = sg.Open())
            {
                sgc.BeginFigure(point1, false, false);
                sgc.LineTo(point4, true, false);
                sgc.BeginFigure(point2, false, false);
                sgc.LineTo(point3, true, false);
                sgc.Close();
            }
            sg.Freeze();
            return sg;
        }

        public static Geometry SingleBondGeometry(Point startPoint, Point endPoint)
        {
            StreamGeometry sg = new StreamGeometry();
            using (StreamGeometryContext sgc = sg.Open())
            {
                sgc.BeginFigure(startPoint, false, false);
                sgc.LineTo(endPoint, true, false);
                sgc.Close();
            }
            sg.Freeze();
            return sg;
        }

        private static List<PathFigure> GetSingleBondSegment(Point startPoint, Point endPoint)
        {
            List<PathSegment> segments = new List<PathSegment> { new LineSegment(endPoint, false) };

            List<PathFigure> figures = new List<PathFigure>();
            PathFigure pf = new PathFigure(startPoint, segments, true);
            figures.Add(pf);
            return figures;
        }

        public static Geometry WavyBondGeometry(Point startPoint, Point endPoint)
        {
            StreamGeometry sg = new StreamGeometry();
            using (StreamGeometryContext sgc = sg.Open())
            {
                Vector bondVector = endPoint - startPoint;
                int noOfWiggles = (int)Math.Ceiling(bondVector.Length / Globals.Offset);
                if (noOfWiggles < 1)
                {
                    noOfWiggles = 1;
                }

                double wiggleLength = bondVector.Length / noOfWiggles;
                Debug.WriteLine($"v.Length: {bondVector.Length} noOfWiggles: {noOfWiggles}");

                Vector originalWigglePortion = bondVector;
                originalWigglePortion.Normalize();
                originalWigglePortion *= wiggleLength / 2;

                Matrix toLeft = new Matrix();
                toLeft.Rotate(-60);
                Matrix toRight = new Matrix();
                toRight.Rotate(60);
                Vector leftVector = originalWigglePortion * toLeft;
                Vector rightVector = originalWigglePortion * toRight;

                List<Point> allpoints = new List<Point>();
                List<List<Point>> allTriangles = new List<List<Point>>();
                List<Point> triangle = new List<Point>();

                Point lastPoint = startPoint;
                allpoints.Add(lastPoint);
                triangle.Add(lastPoint);
                for (int i = 0; i < noOfWiggles; i++)
                {
                    Point leftPoint = lastPoint + leftVector;
                    allpoints.Add(leftPoint);
                    //triangle.Add(leftPoint);

                    //Point midPoint = lastPoint + originalWigglePortion;
                    //allpoints.Add(midPoint);
                    //triangle.Add(midPoint);
                    //allTriangles.Add(triangle);
                    //triangle = new List<Point>();
                    //triangle.Add(midPoint);

                    Point rightPoint = lastPoint + originalWigglePortion + rightVector;
                    allpoints.Add(rightPoint);
                    //triangle.Add(rightPoint);

                    lastPoint += originalWigglePortion * 2;
                    //allpoints.Add(lastPoint);
                    //triangle.Add(lastPoint);
                    //allTriangles.Add(triangle);
                    //triangle = new List<Point>();
                    //triangle.Add(lastPoint);
                }

                sgc.BeginFigure(startPoint, false, false);
                sgc.PolyLineTo(allpoints, true, true);

                sgc.Close();
            }
            return sg;
        }
    }
}