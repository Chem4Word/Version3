// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

namespace Chem4Word.Renderer.OoXmlV3.TTF
{
    public class TtfPoint
    {
        public enum PointType
        {
            Start,
            Line,
            CurveOff,
            CurveOn
        }

        public int X { get; set; }
        public int Y { get; set; }
        public PointType Type { get; set; }
    }
}