// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Windows;

namespace Chem4Word.Model.Geometry
{
    public class Seperator
    {
        private Model _model = null;

        public Seperator(Model model)
        {
            _model = model;
        }

        public bool Seperate(double padding, int maxLoops, out int loops)
        {
            loops = 0;

            Rect a;
            Rect b;

            double dx;
            double dxa;
            double dxb;

            double dy;
            double dya;
            double dyb;

            bool touching = false;

            do
            {
                loops++;
                touching = false;
                for (int i = 0; i < _model.Molecules.Count; i++)
                {
                    a = _model.Molecules[i].BoundingBox;
                    for (int j = i + 1; j < _model.Molecules.Count; j++)
                    {
                        b = _model.Molecules[j].BoundingBox;
                        if (a.IntersectsWith(b))
                        {
                            touching = true;

                            // find the two smallest deltas required to stop the overlap
                            dx = Math.Min(a.Right - b.Left + padding, a.Left - b.Right - padding);
                            dy = Math.Min(a.Bottom - b.Top + padding, a.Top - b.Bottom - padding);

                            if (j % 2 == 0)
                            {
                                // only keep the smallest delta
                                if (Math.Abs(dx) < Math.Abs(dy))
                                {
                                    dy = 0;
                                }
                                else
                                {
                                    dx = 0;
                                }
                            }
                            else
                            {
                                // only keep the smallest delta
                                if (Math.Abs(dy) < Math.Abs(dx))
                                {
                                    dx = 0;
                                }
                                else
                                {
                                    dy = 0;
                                }
                            }

                            // create a delta for each rectangle as half the whole delta.
                            dxa = -dx / 2;
                            dxb = dx + dxa;
                            dya = -dy / 2;
                            dyb = dy + dya;

                            // shift rectangles
                            if (j % 2 == 0)
                            {
                                _model.Molecules[j].MoveAllAtoms(dxb, dyb);
                            }
                            else
                            {
                                _model.Molecules[i].MoveAllAtoms(dxa, dya);
                            }
                        }
                    }
                }
            } while (loops < maxLoops && touching);

            return loops < maxLoops;
        }
    }
}