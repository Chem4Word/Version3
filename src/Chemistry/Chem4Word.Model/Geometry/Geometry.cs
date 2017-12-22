// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Chem4Word.Model.Geometry
{
    public enum ClockDirections
    {
        One = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Eleven,
        Twelve
    }

    public enum CompassPoints
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }

    public class Geometry<T>
    {
        /// <summary>
        /// Returns the objects that make up the convex hull, in the right order
        /// </summary>
        /// <param name="sortedObjectList">List of objects to process sorted by X, Y coordinates</param>
        /// <param name="getPosition">Lambda or delegate to obtain the position property of the object</param>
        /// <returns></returns>
        public static List<T> GetHull(IEnumerable<T> sortedObjectList, Func<T, Point> getPosition)
        {
            List<T> upper = new List<T>();
            List<T> lower = new List<T>();
            var sortedObjects = sortedObjectList.ToArray();

            for (int i = 0; i < sortedObjects.Count(); i++)
            {
                while (lower.Count >= 2 &&
                       Vector.AngleBetween((getPosition(lower[lower.Count - 2]) - getPosition(lower[lower.Count - 1])),
                           (getPosition(sortedObjects[i]) - getPosition(lower[lower.Count - 1]))) > 0)
                {
                    lower.RemoveAt(lower.Count() - 1);
                }
                lower.Add(sortedObjects[i]);
            }

            for (int i = sortedObjects.Count() - 1; i >= 0; i--)
            {
                while (upper.Count >= 2 &&
                       Vector.AngleBetween((getPosition(upper[upper.Count - 2]) - getPosition(upper[upper.Count - 1])),
                           (getPosition(sortedObjects[i]) - getPosition(upper[upper.Count - 1]))) > 0)
                {
                    upper.RemoveAt(upper.Count() - 1);
                }
                upper.Add(sortedObjects[i]);
            }
            upper.RemoveAt(upper.Count() - 1);
            lower.RemoveAt(lower.Count() - 1);
            lower.AddRange(upper);
            return lower;
        }

        /// <summary>
        /// gets the centroid of an array of points
        /// </summary>
        /// <param name="poly">Polygon represented as array of objects, sorted in anticlockwise order</param>
        /// <param name="getPosition">Lambda to return position of T</param>
        /// <returns>Point as geocenter</returns>
        public static Point? GetCentroid(T[] poly, Func<T, Point> getPosition)
        {
            double accumulatedArea = 0.0f;
            double centerX = 0.0f;
            double centerY = 0.0f;

            for (int i = 0, j = poly.Length - 1; i < poly.Length; j = i++)
            {
                double temp = getPosition(poly[i]).X * getPosition(poly[j]).Y
                    - getPosition(poly[j]).X * getPosition(poly[i]).Y;
                accumulatedArea += temp;
                centerX += (getPosition(poly[i]).X + getPosition(poly[j]).X) * temp;
                centerY += (getPosition(poly[i]).Y + getPosition(poly[j]).Y) * temp;
            }

            if (Math.Abs(accumulatedArea) < 1E-7f)
            {
                return null; // Avoid division by zero
            }

            accumulatedArea *= 3f;
            return new Point(centerX / accumulatedArea, centerY / accumulatedArea);
        }
    }

    /// See https://www.topcoder.com/community/data-science/data-science-tutorials/geometry-concepts-line-intersection-and-its-applications/
    /// for implementing some good basic operations in geometry
    /// </summary>

    public static class BasicGeometry
    {
        /// <summary>
        /// gets signed angle between three points
        /// direction is anticlockwise
        /// example:
        /// GetAngle(new Point2(1,0), new Point2(0,0), new Point2(0,1)) => Math.PI/2
        /// GetAngle(new Point2(-1,0), new Point2(0,0), new Point2(0,1)) => -Math.PI/2
        /// GetAngle(new Point2(0,1), new Point2(0,0), new Point2(1,0)) => -Math.PI/2
        ///
        /// </summary>
        /// <param name="point0">first point</param>
        /// <param name="point1">centre point</param>
        /// <param name="point2">final point</param>
        /// <param name="epsilon"></param>
        /// <exception cref="ArgumentException">if any atoms are coincident</exception>
        /// <returns>null if any points are null</returns>
        /// <summary>
        public static double? GetAngle(Point? point0, Point? point1, Point? point2, double epsilon)
        {
            double? angle = null;

            if (point0 != null && point1 != null && point2 != null)
            {
                if ((point1 - point0).Value.Length < epsilon || (point2 - point2).Value.Length < epsilon)
                {
                    throw new ArgumentException("coincident points in GetAngle");
                }

                Vector from = point0.Value - point1.Value;
                Vector to = point2.Value - point1.Value;
                angle = Vector.AngleBetween(from, to);
            }

            return angle;
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

        #region extension methods

        public static Vector Perpendicular(this Vector v)
        {
            return new Vector(-v.Y, v.X);
        }

        public static Vector ScreenSouth()
        {
            return new Vector(0, 1);
        }

        public static Vector ScreenEast()
        {
            return new Vector(1, 0);
        }

        public static Vector ScreenNorth()
        {
            return -ScreenSouth();
        }

        public static Vector ScreenWest()
        {
            return -ScreenEast();
        }

        #endregion extension methods

        public static Vector NorthVector()
        {
            return new Vector(0, -1);
        }

        public static Vector SouthVector()
        {
            return -NorthVector();
        }

        public static Vector EastVector()
        {
            return new Vector(1, 0);
        }

        public static Vector WestVector()
        {
            return -EastVector();
        }

        public static double Determinant(Vector vector1, Vector vector2)
        {
            return vector1.X * vector2.Y - vector1.Y * vector2.X;
        }

        /// <summary>
        /// Determines whether two line segments intersect
        /// </summary>
        /// <param name="segment1Start">Point at which first segment starts</param>
        /// <param name="segment1End">Point at which first segment ends</param>
        /// <param name="segment2Start">Point at which second segment starts</param>
        /// <param name="segment2End">Point at which second segment ends</param>
        /// <returns>Point at which both lines intersect, null if otherwise</returns>
        public static Point? LineSegmentsIntersect(Point segment1Start, Point segment1End, Point segment2Start, Point segment2End)
        {
            double t;
            double u;
            IntersectLines(out t, out u, segment1Start, segment1End, segment2Start, segment2End);
            if ((t >= 0) && (u >= 0) && (t <= 1) && (u <= 1)) //voila, we have an intersection
            {
                Vector vIntersect = (segment1End - segment1Start) * t;
                return segment1Start + vIntersect;
            }
            return null;
        }

        /// <summary>
        /// intersects two straight line segments.  Returns two values that indicate
        /// how far along the segments the intersection takes place
        /// </summary>
        /// <param name="t">proportion along the line of the first segment</param>
        /// <param name="u">proportion along the line of the second segmnt</param>
        /// <param name="segment1Start">what it says</param>
        /// <param name="segment1End">what it says</param>
        /// <param name="segment2Start">what it says</param>
        /// <param name="segment2End">what it says</param>
        ///
        public static void IntersectLines(out double t, out double u, Point segment1Start, Point segment1End, Point segment2Start,
            Point segment2End)
        {
            double det = Determinant(segment1End - segment1Start, segment2Start - segment2End);
            t = Determinant(segment2Start - segment1Start, segment2Start - segment2End) / det;
            u = Determinant(segment1End - segment1Start, segment2Start - segment1Start) / det;
        }

        public static CompassPoints SnapTo2EW(double angleFromNorth)
        {
            if (angleFromNorth >= 0 || angleFromNorth <= -180)
            {
                return CompassPoints.East;
            }

            return CompassPoints.West;
        }

        public static CompassPoints SnapTo4NESW(double angleFromNorth)
        {
            if (angleFromNorth >= -45 && angleFromNorth <= 45)
            {
                return CompassPoints.North;
            }

            if (angleFromNorth > 45 && angleFromNorth < 135)
            {
                return CompassPoints.East;
            }

            if (angleFromNorth > -135 && angleFromNorth < -45)
            {
                return CompassPoints.West;
            }

            return CompassPoints.South;
        }
    }
}