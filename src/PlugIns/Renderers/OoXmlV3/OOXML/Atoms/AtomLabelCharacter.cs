// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Renderer.OoXmlV3.TTF;
using System;
using System.Windows;

namespace Chem4Word.Renderer.OoXmlV3.OOXML.Atoms
{
    public class AtomLabelCharacter
    {
        public string ParentMolecule { get; set; }
        public string ParentAtom { get; set; }
        public Point Position { get; set; }
        public TtfCharacter Character { get; set; }
        public String Colour { get; set; }
        public bool IsSubScript { get; set; }
        public char Ascii { get; set; }

        public AtomLabelCharacter(Point position, TtfCharacter character, String colour, char ascii, string parentAtom, string parentMolecule)
        {
            Position = position;
            Character = character;
            Colour = colour;
            Ascii = ascii;
            ParentAtom = parentAtom;
            ParentMolecule = parentMolecule;
        }
    }
}