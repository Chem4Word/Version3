using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Chem4Word.View
{
    public class CanvasAutoSize : Canvas
    {
        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint)

        {
            base.MeasureOverride(constraint);
            double width, height;
            width = Width;
            height = Height;
            if (base.InternalChildren.OfType<Shape>().Any())
            {
                width = base
                    .InternalChildren
                    .OfType<Shape>()
                    .Max(i => i.DesiredSize.Width + (double)i.GetValue(Canvas.LeftProperty));

                height = base
                    .InternalChildren
                    .OfType<Shape>()
                    .Max(i => i.DesiredSize.Height + (double)i.GetValue(Canvas.TopProperty));
            }
            return new Size(width, height);
        }
    }
}