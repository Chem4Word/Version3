// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Chem4Word.Renderer.OoXmlV3
{
    /// <summary>
    /// Contains methods to calculate bounding boxes from molecules.
    /// </summary>
    internal class CoordinateTool
    {
        /// <summary>
        /// Calculate the bounding box from a set of CMLAtoms (should be anything with 2D coords)
        /// Calculation is performed in model coordinates.
        /// </summary>
        /// <param name="atoms"></param>
        /// <returns></returns>
        public static Rect GetBounds2D(IEnumerable<Atom> atoms)
        {
            Rect result = new Rect(0, 0, 0, 0);

            if (atoms.Any())
            {
                double xmax = Double.NegativeInfinity;
                double ymax = Double.NegativeInfinity;
                double xmin = Double.PositiveInfinity;
                double ymin = Double.PositiveInfinity;

                foreach (Atom atom in atoms)
                {
                    Point p = atom.Position;
                    xmax = Math.Max(xmax, p.X);
                    xmin = Math.Min(xmin, p.X);
                    ymax = Math.Max(ymax, p.Y);
                    ymin = Math.Min(ymin, p.Y);
                }

                result = new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
            }

            return result;
        }

        /// <summary>
        /// Calculate the bounding box from a set of CmlBonds (should be anything with 2D coords)
        /// Calculation is performed in model coordinates.
        /// </summary>
        /// <param name="bonds"></param>
        /// <returns></returns>
        public static Rect GetBounds2D(IEnumerable<Bond> bonds)
        {
            SortedDictionary<string, Atom> atoms = new SortedDictionary<string, Atom>();

            foreach (Bond bond in bonds)
            {
                foreach (Atom atom in bond.GetAtoms())
                {
                    if (!atoms.ContainsKey(atom.Id))
                    {
                        atoms.Add(atom.Id, atom);
                    }
                }
            }

            return GetBounds2D(atoms.Values);
        }

        /// <summary>
        /// Gets the mid point between two points
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Point GetMidPoint(Point start, Point end)
        {
            double xx = (start.X + end.X) / 2;
            double yy = (start.Y + end.Y) / 2;

            return new Point(xx, yy);
        }

        /// <summary>
        /// Shrinks line by n pixels about midpoint
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="pixelCount"></param>
        public static void AdjustLineAboutMidpoint(ref Point startPoint, ref Point endPoint, double pixelCount)
        {
            Point midPoint = GetMidPoint(startPoint, endPoint);
            AdjustLineEndPoint(midPoint, ref startPoint, pixelCount);
            AdjustLineEndPoint(midPoint, ref endPoint, pixelCount);
        }

        private static void AdjustLineEndPoint(Point startPoint, ref Point endPoint, double pixelCount)
        {
            double dx = endPoint.X - startPoint.X;
            double dy = endPoint.Y - startPoint.Y;

            if (dx == 0)
            {
                // vertical line:
                if (endPoint.Y < startPoint.Y)
                {
                    endPoint.Y -= pixelCount;
                }
                else
                {
                    endPoint.Y += pixelCount;
                }
            }
            else if (dy == 0)
            {
                // horizontal line:
                if (endPoint.X < startPoint.X)
                {
                    endPoint.X -= pixelCount;
                }
                else
                {
                    endPoint.X += pixelCount;
                }
            }
            else
            {
                // non-horizontal, non-vertical line:
                double length = Math.Sqrt(dx * dx + dy * dy);
                double scale = (length + pixelCount) / length;
                dx *= scale;
                dy *= scale;
                endPoint.X = startPoint.X + Convert.ToSingle(dx);
                endPoint.Y = startPoint.Y + Convert.ToSingle(dy);
            }
        }

        /// <summary>
        /// Find the point of intersection between the lines line1Start --> line1End and line2Start --> line2End.
        /// </summary>
        /// <param name="line1Start"></param>
        /// <param name="line1End"></param>
        /// <param name="line2Start"></param>
        /// <param name="line2End"></param>
        /// <param name="canIntersect">True if the lines containing the segments can intersect</param>
        /// <param name="doIntersect">True if the segments intersect</param>
        /// <param name="intersection">The point where the lines do or would intersect</param>
        public static void FindIntersection(Point line1Start, Point line1End, Point line2Start, Point line2End,
            out bool canIntersect, out bool doIntersect, out Point intersection)
        {
            // Source: http://csharphelper.com/blog/2014/08/determine-where-two-lines-intersect-in-c/

            // Get the segments' parameters.
            double dx12 = line1End.X - line1Start.X;
            double dy12 = line1End.Y - line1Start.Y;
            double dx34 = line2End.X - line2Start.X;
            double dy34 = line2End.Y - line2Start.Y;

            // Solve for t1 and t2
            double denominator = (dy12 * dx34 - dx12 * dy34);

            double t1 = ((line1Start.X - line2Start.X) * dy34 + (line2Start.Y - line1Start.Y) * dx34) / denominator;
            if (double.IsInfinity(t1))
            {
                // The lines are parallel (or close enough to it).
                canIntersect = false;
                doIntersect = false;
                intersection = new Point(double.NaN, double.NaN);
                //close_p1 = new Point(double.NaN, double.NaN);
                //close_p2 = new Point(double.NaN, double.NaN);
                return;
            }

            canIntersect = true;

            double t2 = ((line2Start.X - line1Start.X) * dy12 + (line1Start.Y - line2Start.Y) * dx12) / -denominator;

            // Find the point of intersection.
            intersection = new Point(line1Start.X + dx12 * t1, line1Start.Y + dy12 * t1);

            // The segments intersect if t1 and t2 are between 0 and 1.
            doIntersect = (t1 >= 0) && (t1 <= 1) && (t2 >= 0) && (t2 <= 1);

            //// Find the closest points on the segments.
            //if (t1 < 0)
            //{
            //    t1 = 0;
            //}
            //else if (t1 > 1)
            //{
            //    t1 = 1;
            //}

            //if (t2 < 0)
            //{
            //    t2 = 0;
            //}
            //else if (t2 > 1)
            //{
            //    t2 = 1;
            //}

            //close_p1 = new Point(line1Start.X + dx12 * t1, line1Start.Y + dy12 * t1);
            //close_p2 = new Point(line2Start.X + dx34 * t2, line2Start.Y + dy34 * t2);
        }

        /// <summary>
        /// AngleBetween - the angle between 2 vectors
        /// </summary>
        /// <returns>
        /// Returns the the angle in degrees between vector1 and vector2
        /// </returns>
        /// <param name="vector1"> The first Vector </param>
        /// <param name="vector2"> The second Vector </param>
        public static double AngleBetween(Vector vector1, Vector vector2)
        {
            double sin = vector1.X * vector2.Y - vector2.X * vector1.Y;
            double cos = vector1.X * vector2.X + vector1.Y * vector2.Y;

            return Math.Atan2(sin, cos) * (180 / Math.PI);
        }

        /// <summary>
        /// Finds the angle between two points
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double AngleBetween(Point p1, Point p2)
        {
            double xDiff = p2.X - p1.X;
            double yDiff = p2.Y - p1.Y;
            return Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI;
        }

        public static double DistanceBetween(Point p1, Point p2)
        {
            double a = p2.X - p1.X;
            double b = p2.Y - p1.Y;

            return Math.Sqrt(a * a + b * b);
        }
    }
}