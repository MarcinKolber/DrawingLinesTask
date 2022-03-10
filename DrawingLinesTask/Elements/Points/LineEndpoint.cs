using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using DrawingLines.Drawing.Constants;

namespace DrawingLines.Elements.Points;

public record LineEndpoint : BasePoint
{
    public Ellipse? PointElement { get; set; }

    public UIElement GetUIElement()
    {
        if (PointElement is null)
        {
            PointElement = new Ellipse
            {
                Width = PointStyling.Width,
                Height = PointStyling.Height,
            };
        }
        return PointElement;
    }
}