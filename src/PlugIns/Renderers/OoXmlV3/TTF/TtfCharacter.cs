// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System.Collections.Generic;

namespace Chem4Word.Renderer.OoXmlV3.TTF
{
    public class TtfCharacter
    {
        public char Character { get; set; }
        public int OriginX { get; set; }
        public int OriginY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int IncrementX { get; set; }
        public int IncrementY { get; set; }
        public List<TtfContour> Contours { get; set; }

        public TtfCharacter()
        {
            Contours = new List<TtfContour>();
        }
    }
}