// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Windows;

namespace Chem4Word.Renderer.OoXmlV3.OOXML.Bonds
{
    public class BondLine
    {
        public string ParentMolecule { get; set; }
        public string ParentBond { get; set; }

        private Point _start;
        private Point _end;

        public BondLineStyle Type { get; set; }

        public Point Start
        {
            get { return _start; }
            set
            {
                _start = value;
                BoundingBox = new Rect(_start, _end);
            }
        }

        public Point End
        {
            get { return _end; }
            set
            {
                _end = value;
                BoundingBox = new Rect(_start, _end);
            }
        }

        public Rect BoundingBox { get; set; }

        public BondLine(Point startPoint, Point endPoint, BondLineStyle type, string parentBond, string parentMolecule)
        {
            _start = startPoint;
            _end = endPoint;
            BoundingBox = new Rect(startPoint, endPoint);
            Type = type;
            ParentBond = parentBond;
            ParentMolecule = parentMolecule;
        }

        public BondLine GetParallel(double offset)
        {
            double xDifference = _start.X - _end.X;
            double yDifference = _start.Y - _end.Y;
            double length = Math.Sqrt(Math.Pow(xDifference, 2) + Math.Pow(yDifference, 2));

            Point newStartPoint = new Point((float)(_start.X - offset * yDifference / length),
                                            (float)(_start.Y + offset * xDifference / length));
            Point newEndPoint = new Point((float)(_end.X - offset * yDifference / length),
                                          (float)(_end.Y + offset * xDifference / length));

            return new BondLine(newStartPoint, newEndPoint, Type, ParentBond, ParentMolecule);
        }
    }

    public enum BondLineStyle
    {
        Solid,
        Dotted,
        Dashed,
        Wavy,
        Wedge,
        Hatch
    }
}