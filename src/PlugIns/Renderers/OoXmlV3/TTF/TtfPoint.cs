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