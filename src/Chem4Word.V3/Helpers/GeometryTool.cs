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
using System.Linq;

namespace Chem4Word.Helpers

{
    /// <summary>
    /// Methods to calulate various values derived from sets of atoms/bonds etc
    /// </summary>
    internal class GeometryTool
    {
        /// <summary>
        /// Returns the median 2D bond length.
        /// </summary>
        /// <param name="bonds"></param>
        /// <returns></returns>
        public static double GetMedianBondLength2D(ICollection<CmlBond> bonds)
        {
            double result = -1;

            if (bonds.Any())
            {
                int nbonds = bonds.Count;
                double[] len = new double[nbonds];
                for (int i = 0; i < nbonds; i++)
                {
                    CmlBond b = bonds.ElementAt(i);
                    CmlAtom a0 = b.GetAtoms().ElementAt(0);
                    CmlAtom a1 = b.GetAtoms().ElementAt(1);
                    Point2 p0 = a0.Point2;
                    Point2 p1 = a1.Point2;
                    double dx = p0.X - p1.X;
                    double dy = p0.Y - p1.Y;
                    len[i] = dx * dx + dy * dy;
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
    }
}