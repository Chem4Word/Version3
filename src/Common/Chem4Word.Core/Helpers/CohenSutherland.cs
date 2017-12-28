// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System.Windows;

namespace Chem4Word.Core.Helpers
{
    public static class CohenSutherland
    {
        // Fixed with help from https://gist.github.com/oliverheilig/7777382 http://jsil.org/try/#7717256

        public const int Left = 1;
        public const int Right = 2;
        public const int Top = 8;
        public const int Bottom = 4;

        private const double Epsilon = 1e-4;

        private static int ComputeOutCode(Rect rect, double x, double y)
        {
            int code = 0;

            if (y - rect.Bottom > Epsilon)  // y > rect.Bottom
                code |= Bottom;
            if (rect.Top - y > Epsilon)     // y < rect.Top
                code |= Top;
            if (x - rect.Right > Epsilon)   // x > rect.Right
                code |= Right;
            if (rect.Left - x > Epsilon)    // x < rect.Left
                code |= Left;

            return code;
        }

        private static int ComputeInCode(Rect rect, double x, double y)
        {
            int code = 0;

            if (rect.Bottom - y > Epsilon)  // y < rect.Bottom
                code |= Bottom;
            if (y - rect.Top > Epsilon)     // y > rect.Top
                code |= Top;
            if (rect.Right - x > Epsilon)   // x < rect.Right
                code |= Right;
            if (x - rect.Left > Epsilon)    // x > rect.Left
                code |= Left;

            return code;
        }

        public static bool ClipLine(Rect rect, ref Point a, ref Point b, out int attempts)
        {
            double x1 = a.X, x2 = b.X, y1 = a.Y, y2 = b.Y;
            double xmin = rect.Left, xmax = rect.Right, ymin = rect.Top, ymax = rect.Bottom;

            int outCode1 = ComputeOutCode(rect, x1, y1);
            int outCode2 = ComputeOutCode(rect, x2, y2);

            bool accept = false, done = false;
            attempts = 0;
            while (!done)
            {
                // Trivial accept, both points inside rectangle
                if ((outCode1 | outCode2) == 0)
                    done = accept = true;
                // Trivial reject, both points outside rectangle and cannot cross it
                else if ((outCode1 & outCode2) > 0)
                    done = true;
                else
                {
                    // Calculate the line segment to clip from an outside point to an intersection with clip edge
                    double x = 0, y = 0;
                    // At least one endpoint is outside the clip rectangle; pick it.
                    int outCodeOut = outCode1 != 0 ? outCode1 : outCode2;

                    // Now find the intersection point;
                    //  use formulas y = y0 + slope * (x - x0), x = x0 + (1/slope)* (y - y0)
                    if ((outCodeOut & Top) > 0)
                    {
                        x = x1 + (x2 - x1) * (ymin - y1) / (y2 - y1);
                        y = ymin;
                    }
                    else if ((outCodeOut & Bottom) > 0)
                    {
                        x = x1 + (x2 - x1) * (ymax - y1) / (y2 - y1);
                        y = ymax;
                    }
                    else if ((outCodeOut & Right) > 0)
                    {
                        y = y1 + (y2 - y1) * (xmax - x1) / (x2 - x1);
                        x = xmax;
                    }
                    else if ((outCodeOut & Left) > 0)
                    {
                        y = y1 + (y2 - y1) * (xmin - x1) / (x2 - x1);
                        x = xmin;
                    }

                    // Now we move outside point to intersection point to clip and get ready for next pass.
                    if (outCodeOut == outCode1)
                        outCode1 = ComputeOutCode(rect, x1 = x, y1 = y);
                    else
                        outCode2 = ComputeOutCode(rect, x2 = x, y2 = y);
                }

                attempts++;
                if (attempts > 15)
                {
                    done = true;
                }
            }

            if (accept)
            {
                a.X = x1;
                a.Y = y1;
                b.X = x2;
                b.Y = y2;
            }

            return accept;
        }
    }
}