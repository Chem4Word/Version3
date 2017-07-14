using Chem4Word.Renderer.OoXmlV3.TTF;
using System;
using System.Windows;

namespace Chem4Word.Renderer.OoXmlV3.OOXML.Atoms
{
    public class AtomLabelCharacter
    {
        public string Parent { get; set; }
        public Point Position { get; set; }
        public TtfCharacter Character { get; set; }
        public String Colour { get; set; }
        public bool IsSubScript { get; set; }
        public char Ascii { get; set; }

        public AtomLabelCharacter(Point position, TtfCharacter character, String colour, char ascii, string parent)
        {
            Position = position;
            Character = character;
            Colour = colour;
            Ascii = ascii;
            Parent = parent;
        }
    }
}