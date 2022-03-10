namespace DrawingLines.Elements.Points
{
    public record GeometryPoint : BasePoint
    {
        public static GeometryPoint Create(double x, double y) => new ()
        {
            X = x,
            Y = y,
        };
    }
}
