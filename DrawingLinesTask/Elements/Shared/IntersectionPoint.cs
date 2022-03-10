using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using DrawingLines.Drawing.Constants;

namespace DrawingLines.Elements.Shared
{
    public record IntersectionPoint : BasePoint, IElement
    {
        public Ellipse? PointElement { get; set; }

        public bool CanDraw()
        {
            throw new NotImplementedException();
        }

        public bool IsDrawn()
        {
            throw new NotImplementedException();
        }

        public UIElement GetUIElement()
        {
            if (PointElement is null)
            {
                PointElement = new Ellipse
                {
                    Width = PointStyling.Width,
                    Height = PointStyling.Height,
                    Fill = Brushes.Yellow,
                };
            }
            return PointElement;
        }

        public UIElement CanDrawNewElement()
        {
            throw new NotImplementedException();
        }

        public bool Drawn { get; set; }

        public static IntersectionPoint Create(double x, double y) => new()
        {
            X = x,
            Y = y,
            PointElement =  new Ellipse
            {
            Width = PointStyling.Width,
            Height = PointStyling.Height,
            Fill = Brushes.Yellow,
        },
            Drawn = false
        };
    }
}
