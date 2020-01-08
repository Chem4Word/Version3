// ---------------------------------------------------------------------------
//  Copyright (c) 2020, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Chem4Word.Model;

namespace Chem4Word.Renderer.OoXmlV3
{
    /// <summary>
    /// Methods to calculate various values derived from sets of atoms/bonds etc
    /// </summary>
    internal class GeometryTool
    {
        /// <summary>
        /// Returns the median 2D bond length.
        /// </summary>
        /// <param name="bonds"></param>
        /// <returns></returns>
        public static double GetMedianBondLength2D(ICollection<Bond> bonds)
        {
            double result = -1;

            if (bonds.Any())
            {
                int nbonds = bonds.Count;
                double[] len = new double[nbonds];
                for (int i = 0; i < nbonds; i++)
                {
                    Bond b = bonds.ElementAt(i);

                    var p0 = b.StartAtom.Position;
                    var p1 = b.EndAtom.Position;
                    Vector v = p1 - p0;
                    len[i] = v.LengthSquared;
                }

                Array.Sort(len);

                // Division/cast to int rounds down
                if (nbonds % 2 == 0)
                {
                    result = 0.5 * (Math.Sqrt(len[nbonds / 2]) + Math.Sqrt(len[nbonds / 2 - 1]));
                }
                else
                {
                    result = Math.Sqrt(len[nbonds / 2]);
                }
            }

            return result;
        }

        // modified from http://csharphelper.com/blog/2014/07/find-the-convex-hull-of-a-set-of-points-in-c/

        // For debugging.
        private static Point[] _minMaxCorners;

        private static Rect _minMaxBox;
        private static Point[] _nonCulledPoints;

        // Find the points nearest the upper left, upper right,
        // lower left, and lower right corners.
        private static void GetMinMaxCorners(List<Point> points, ref Point ul, ref Point ur, ref Point ll, ref Point lr)
        {
            // Start with the first point as the solution.
            ul = points[0];
            ur = ul;
            ll = ul;
            lr = ul;

            // Search the other points.
            foreach (Point pt in points)
            {
                if (-pt.X - pt.Y > -ul.X - ul.Y)
                {
                    ul = pt;
                }

                if (pt.X - pt.Y > ur.X - ur.Y)
                {
                    ur = pt;
                }

                if (-pt.X + pt.Y > -ll.X + ll.Y)
                {
                    ll = pt;
                }

                if (pt.X + pt.Y > lr.X + lr.Y)
                {
                    lr = pt;
                }
            }

            _minMaxCorners = new Point[] { ul, ur, lr, ll }; // For debugging.
        }

        // Find a box that fits inside the MinMax quadrilateral.
        private static Rect GetMinMaxBox(List<Point> points)
        {
            // Find the MinMax quadrilateral.
            Point ul = new Point(0, 0), ur = ul, ll = ul, lr = ul;
            GetMinMaxCorners(points, ref ul, ref ur, ref ll, ref lr);

            // Get the coordinates of a box that lies inside this quadrilateral.
            double xmin, xmax, ymin, ymax;
            xmin = ul.X;
            ymin = ul.Y;

            xmax = ur.X;
            if (ymin < ur.Y)
            {
                ymin = ur.Y;
            }

            if (xmax > lr.X)
            {
                xmax = lr.X;
            }
            ymax = lr.Y;

            if (xmin < ll.X)
            {
                xmin = ll.X;
            }

            if (ymax > ll.Y)
            {
                ymax = ll.Y;
            }

            Rect result = new Rect();
            if (xmax - xmin > 0 && ymax - ymin > 0)
            {
                result = new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
            }
            _minMaxBox = result;    // For debugging.
            return result;
        }

        // Cull points out of the convex hull that lie inside the
        // trapezoid defined by the vertices with smallest and
        // largest X and Y coordinates.
        // Return the points that are not culled.
        private static List<Point> HullCull(List<Point> points)
        {
            // Find a culling box.
            Rect cullingBox = GetMinMaxBox(points);

            // Cull the points.
            List<Point> results = new List<Point>();
            foreach (Point pt in points)
            {
                // See if (this point lies outside of the culling box.
                if (pt.X <= cullingBox.Left ||
                    pt.X >= cullingBox.Right ||
                    pt.Y <= cullingBox.Top ||
                    pt.Y >= cullingBox.Bottom)
                {
                    // This point cannot be culled.
                    // Add it to the results.
                    results.Add(pt);
                }
            }

            _nonCulledPoints = new Point[results.Count];   // For debugging.
            results.CopyTo(_nonCulledPoints);              // For debugging.
            return results;
        }

        // Return the points that make up a polygon's convex hull.
        // This method leaves the points list unchanged.
        public static List<Point> MakeConvexHull(List<Point> points)
        {
            // Cull.
            points = HullCull(points);

            // Find the remaining point with the smallest Y value.
            // if (there's a tie, take the one with the smaller X value.
            Point bestPt = points[0];
            foreach (Point pt in points)
            {
                if ((pt.Y < bestPt.Y) ||
                   ((pt.Y == bestPt.Y) && (pt.X < bestPt.X)))
                {
                    bestPt = pt;
                }
            }

            // Move this point to the convex hull.
            List<Point> hull = new List<Point>();
            hull.Add(bestPt);
            points.Remove(bestPt);

            // Start wrapping up the other points.
            double sweepAngle = 0;
            for (;;)
            {
                // Find the point with smallest AngleValue
                // from the last point.
                double X = hull[hull.Count - 1].X;
                double Y = hull[hull.Count - 1].Y;
                bestPt = points[0];
                double bestAngle = 3600;

                // Search the rest of the points.
                foreach (Point pt in points)
                {
                    double testAngle = AngleValue(X, Y, pt.X, pt.Y);
                    if ((testAngle >= sweepAngle) &&
                        (bestAngle > testAngle))
                    {
                        bestAngle = testAngle;
                        bestPt = pt;
                    }
                }

                // See if the first point is better.
                // If so, we are done.
                double firstAngle = AngleValue(X, Y, hull[0].X, hull[0].Y);
                if ((firstAngle >= sweepAngle) &&
                    (bestAngle >= firstAngle))
                {
                    // The first point is better. We're done.
                    break;
                }

                // Add the best point to the convex hull.
                hull.Add(bestPt);
                points.Remove(bestPt);

                sweepAngle = bestAngle;

                // If all of the points are on the hull, we're done.
                if (points.Count == 0)
                {
                    break;
                }
            }

            return hull;
        }

        // Return a number that gives the ordering of angles
        // WRST horizontal from the point (x1, y1) to (x2, y2).
        // In other words, AngleValue(x1, y1, x2, y2) is not
        // the angle, but if:
        //   Angle(x1, y1, x2, y2) > Angle(x1, y1, x2, y2)
        // then
        //   AngleValue(x1, y1, x2, y2) > AngleValue(x1, y1, x2, y2)
        // this angle is greater than the angle for another set
        // of points,) this number for
        //
        // This function is dy / (dy + dx).
        private static double AngleValue(double x1, double y1, double x2, double y2)
        {
            double dx, dy, ax, ay, t;

            dx = x2 - x1;
            ax = Math.Abs(dx);
            dy = y2 - y1;
            ay = Math.Abs(dy);
            if (ax + ay == 0)
            {
                // if (the two points are the same, return 360.
                t = 360f / 9f;
            }
            else
            {
                t = dy / (ax + ay);
            }
            if (dx < 0)
            {
                t = 2 - t;
            }
            else if (dy < 0)
            {
                t = 4 + t;
            }
            return t * 90;
        }

        // http://csharphelper.com/blog/2016/01/clip-a-line-segment-to-a-polygon-in-c/

        // Return points where the segment enters and leaves the polygon.
        public static Point[] ClipLineWithPolygon(Point point1, Point point2, List<Point> points, out bool startsOutsidePolygon)
        {
            // Make lists to hold points of
            // intersection and their t values.
            List<Point> intersections = new List<Point>();
            List<double> tValues = new List<double>();

            // Add the segment's starting point.
            intersections.Add(point1);
            tValues.Add(0f);
            startsOutsidePolygon = !PointIsInPolygon(point1.X, point1.Y, points.ToArray());

            // Examine the polygon's edges.
            for (int i1 = 0; i1 < points.Count; i1++)
            {
                // Get the end points for this edge.
                int i2 = (i1 + 1) % points.Count;

                // See where the edge intersects the segment.
                bool linesIntersect, segmentsIntersect;
                Point intersection;

                CoordinateTool.FindIntersection(point1, point2,
                   points[i1], points[i2],
                   out linesIntersect, out segmentsIntersect,
                   out intersection);

                // See if the segment intersects the edge.
                if (segmentsIntersect)
                {
                    // See if we need to record this intersection.

                    // Record this intersection.
                    intersections.Add(intersection);
                }
            }

            // Add the segment's ending point.
            intersections.Add(point2);
            tValues.Add(1f);

            // Sort the points of intersection by t value.
            Point[] intersectionsArray = intersections.ToArray();
            double[] tArray = tValues.ToArray();
            Array.Sort(tArray, intersectionsArray);

            // Return the intersections.
            return intersectionsArray;
        }

        // Return True if the point is in the polygon.
        public static bool PointIsInPolygon(double x, double y, Point[] points)
        {
            // Get the angle between the point and the
            // first and last vertices.
            int maxPoint = points.Length - 1;
            double totalAngle = GetAngle(
                points[maxPoint].X, points[maxPoint].Y,
                x, y,
                points[0].X, points[0].Y);

            // Add the angles from the point
            // to each other pair of vertices.
            for (int i = 0; i < maxPoint; i++)
            {
                totalAngle += GetAngle(
                    points[i].X, points[i].Y,
                    x, y,
                    points[i + 1].X, points[i + 1].Y);
            }

            // The total angle should be 2 * PI or -2 * PI if
            // the point is in the polygon and close to zero
            // if the point is outside the polygon.
            // The following statement was changed. See the comments.
            //return (Math.Abs(total_angle) > 0.000001);
            return (Math.Abs(totalAngle) > 1);
        }

        // Return the angle ABC.
        // Return a value between PI and -PI.
        // Note that the value is the opposite of what you might
        // expect because Y coordinates increase downward.
        public static double GetAngle(double ax, double ay, double bx, double by, double cx, double cy)
        {
            // Get the dot product.
            double dotProduct = DotProduct(ax, ay, bx, by, cx, cy);

            // Get the cross product.
            double crossProductLength = CrossProductLength(ax, ay, bx, by, cx, cy);

            // Calculate the angle.
            return Math.Atan2(crossProductLength, dotProduct);
        }

        // Return the dot product AB · BC.
        // Note that AB · BC = |AB| * |BC| * Cos(theta).
        private static double DotProduct(double ax, double ay, double bx, double by, double cx, double cy)
        {
            // Get the vectors' coordinates.
            double bax = ax - bx;
            double bay = ay - by;
            double bcx = cx - bx;
            double bcy = cy - by;

            // Calculate the dot product.
            return (bax * bcx + bay * bcy);
        }

        // Return the cross product AB x BC.
        // The cross product is a vector perpendicular to AB
        // and BC having length |AB| * |BC| * Sin(theta) and
        // with direction given by the right-hand rule.
        // For two vectors in the X-Y plane, the result is a
        // vector with X and Y components 0 so the Z component
        // gives the vector's length and direction.
        public static double CrossProductLength(double ax, double ay, double bx, double by, double cx, double cy)
        {
            // Get the vectors' coordinates.
            double bax = ax - bx;
            double bay = ay - by;
            double bcx = cx - bx;
            double bcy = cy - by;

            // Calculate the Z coordinate of the cross product.
            return (bax * bcy - bay * bcx);
        }
    }
}