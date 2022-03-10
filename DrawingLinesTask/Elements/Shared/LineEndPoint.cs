using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using DrawingLines.Drawing.Constants;
using DrawingLines.Elements.Lines;

namespace DrawingLines.Elements.Shared
{
    public record LineEndPoint : BasePoint
    {
        public Ellipse? PointElement { get; set; }
        public bool Applied { get; set; }

        public bool CanDraw()
        {
            return !Drawn;
        }

        public bool IsDrawn()
        {
            return Drawn;
        }

        public UIElement Apply()
        {
            Applied = true;

            if (PointElement is null)
            {
                PointElement = new Ellipse();
            }

            PointElement.Fill = Brushes.DarkGray;

            return PointElement;
        }

        public UIElement GetUIElement()
        {
            if (PointElement is null)
            {
                PointElement = new Ellipse
                {
                    Width = PointStyling.Width,
                    Height = PointStyling.Height,
                    Fill = Brushes.Brown,
                };
            }
            return PointElement;
        }

        public UIElement CanDrawNewElement()
        {
            throw new NotImplementedException();
        }

        public bool Drawn { get; set; }

        public static implicit operator GeometryPoint(LineEndPoint lineEndPoint) => new () { X = lineEndPoint.X, Y = lineEndPoint.Y };
    }
}
