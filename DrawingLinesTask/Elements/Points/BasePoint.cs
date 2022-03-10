using DrawingLines.Elements.Interfaces;

namespace DrawingLines.Elements.Points;

public abstract record BasePoint: IPoint
{
    public double X { get; set; }
    public double Y { get; set; }
}