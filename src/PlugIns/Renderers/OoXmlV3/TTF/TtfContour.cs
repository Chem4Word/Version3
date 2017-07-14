using System.Collections.Generic;

namespace Chem4Word.Renderer.OoXmlV3.TTF
{
    public class TtfContour
    {
        public List<TtfPoint> Points { get; set; }

        public TtfContour()
        {
            Points = new List<TtfPoint>();
        }
    }
}