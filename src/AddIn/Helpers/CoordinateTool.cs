// -----------------------------------------------------------------------
//  Copyright (c) 2011, The Outercurve Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.TXT at
//  the root directory of the distribution.
// -----------------------------------------------------------------------

using Chem4Word.Euclid;
using Chem4Word.Numbo.Cml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace Chem4Word.Helpers
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
        public static Rect GetBounds2D(IEnumerable<CmlAtom> atoms)
        {
            Rect result = new Rect(0, 0, 0, 0);

            if (atoms.Any())
            {
                double xmax = Double.NegativeInfinity;
                double ymax = Double.NegativeInfinity;
                double xmin = Double.PositiveInfinity;
                double ymin = Double.PositiveInfinity;

                foreach (CmlAtom atom in atoms)
                {
                    Point2 p = atom.Point2;
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
        public static Rect GetBounds2D(IEnumerable<CmlBond> bonds)
        {
            SortedDictionary<string, CmlAtom> atoms = new SortedDictionary<string, CmlAtom>();

            foreach (CmlBond bond in bonds)
            {
                foreach (CmlAtom atom in bond.GetAtoms())
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
                    endPoint.Y -= pixelCount;
                else
                    endPoint.Y += pixelCount;
            }
            else if (dy == 0)
            {
                // horizontal line:
                if (endPoint.X < startPoint.X)
                    endPoint.X -= pixelCount;
                else
                    endPoint.X += pixelCount;
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
        /// Find the point of intersection between the lines p1 --> p2 and p3 --> p4.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <param name="lines_intersect"></param>
        /// <param name="segments_intersect"></param>
        /// <param name="intersection"></param>
        /// <param name="close_p1"></param>
        /// <param name="close_p2"></param>
        public static void FindIntersection(Point p1, Point p2, Point p3, Point p4,
            out bool lines_intersect, out bool segments_intersect,
            out Point intersection, out Point close_p1, out Point close_p2)
        {
            // Source: http://csharphelper.com/blog/2014/08/determine-where-two-lines-intersect-in-c/

            // Get the segments' parameters.
            double dx12 = p2.X - p1.X;
            double dy12 = p2.Y - p1.Y;
            double dx34 = p4.X - p3.X;
            double dy34 = p4.Y - p3.Y;

            // Solve for t1 and t2
            double denominator = (dy12 * dx34 - dx12 * dy34);

            double t1 = ((p1.X - p3.X) * dy34 + (p3.Y - p1.Y) * dx34) / denominator;
            if (double.IsInfinity(t1))
            {
                // The lines are parallel (or close enough to it).
                lines_intersect = false;
                segments_intersect = false;
                intersection = new Point(double.NaN, double.NaN);
                close_p1 = new Point(double.NaN, double.NaN);
                close_p2 = new Point(double.NaN, double.NaN);
                return;
            }

            lines_intersect = true;

            double t2 = ((p3.X - p1.X) * dy12 + (p1.Y - p3.Y) * dx12) / -denominator;

            // Find the point of intersection.
            intersection = new Point(p1.X + dx12 * t1, p1.Y + dy12 * t1);

            // The segments intersect if t1 and t2 are between 0 and 1.
            segments_intersect =
                ((t1 >= 0) && (t1 <= 1) &&
                 (t2 >= 0) && (t2 <= 1));

            // Find the closest points on the segments.
            if (t1 < 0)
            {
                t1 = 0;
            }
            else if (t1 > 1)
            {
                t1 = 1;
            }

            if (t2 < 0)
            {
                t2 = 0;
            }
            else if (t2 > 1)
            {
                t2 = 1;
            }

            close_p1 = new Point(p1.X + dx12 * t1, p1.Y + dy12 * t1);
            close_p2 = new Point(p3.X + dx34 * t2, p3.Y + dy34 * t2);
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

        public static string GetHydrogenPlacement(CmlAtom thisAtom, List<CmlAtom> otherAtoms, int bondCount, int ringCount)
        {
            string nesw = "E";

            double angle1 = 0.0;
            double angle2 = 0.0;
            Point p1;
            Point p2;

            switch (bondCount)
            {
                case 1:
                    p1 = new Point((double)thisAtom.X2, (double)thisAtom.Y2);
                    p2 = new Point((double)otherAtoms[0].X2, (double)otherAtoms[0].Y2);
                    angle2 = AngleBetween(p1, p2);
                    break;

                case 2:
                    if (ringCount > 0)
                    {
                        // Find outgoing angle
                        p1 = new Point((double)thisAtom.X2, (double)thisAtom.Y2);
                        p2 = GetMidPoint(
                            new Point((double)otherAtoms[0].X2, (double)otherAtoms[0].Y2),
                            new Point((double)otherAtoms[1].X2, (double)otherAtoms[1].Y2));
                        angle2 = AngleBetween(p1, p2);
                    }
                    else
                    {
                        if (Math.Abs(angle1) > 98)
                        {
                            // Find average bond angle
                            p1 = new Point((double)thisAtom.X2, (double)thisAtom.Y2);
                            p2 = new Point((double)otherAtoms[0].X2, (double)otherAtoms[0].Y2);
                            double angle2a = AngleBetween(p1, p2);
                            p2 = new Point((double)otherAtoms[1].X2, (double)otherAtoms[1].Y2);
                            double angle2b = AngleBetween(p2, p1);
                            angle2 = (angle2a + angle2b) / 2;
                        }
                        else
                        {
                            // Find outgoing angle
                            p1 = new Point((double)thisAtom.X2, (double)thisAtom.Y2);
                            p2 = GetMidPoint(
                                new Point((double)otherAtoms[0].X2, (double)otherAtoms[0].Y2),
                                new Point((double)otherAtoms[1].X2, (double)otherAtoms[1].Y2));
                            angle2 = AngleBetween(p1, p2);
                        }
                    }

                    break;

                default:
                    break;
            }

            Debug.WriteLine("Angle1: " + angle1);
            Debug.WriteLine("Angle2: " + angle2);

            if (Math.Abs(angle1) > 98)
            {
                if (Math.Abs(angle2) < 45)
                {
                    if (angle1 >= 0)
                    {
                        nesw = "N";
                    }
                    else
                    {
                        nesw = "S";
                    }
                }
                else
                {
                    if (angle1 >= 0)
                    {
                        nesw = "E";
                    }
                    else
                    {
                        nesw = "W";
                    }
                }
            }
            else
            {
                if (angle2 > 45 && angle2 < 135)
                {
                    nesw = "N";
                }
                if (angle2 > -45 && angle2 < 45)
                {
                    nesw = "W";
                }
                if (angle2 > -135 && angle2 < -45)
                {
                    nesw = "S";
                }
            }

            Debug.WriteLine(nesw);

            return nesw;
        }
    }
}