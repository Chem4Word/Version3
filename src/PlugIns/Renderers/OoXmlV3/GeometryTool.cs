// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
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
    /// Methods to calulate various values derived from sets of atoms/bonds etc
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
    }
}