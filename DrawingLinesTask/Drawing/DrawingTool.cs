using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using DrawingLines.Elements;
using DrawingLines.Elements.Interfaces;
using DrawingLines.Elements.Lines;
using DrawingLines.Elements.Points;
using DrawingLines.Enums;
using LineSegment = DrawingLines.Elements.Lines.LineSegment;

namespace DrawingLines.Drawing
{
    public class DrawingTool : IDrawingTool
    {
        private readonly List<IElement> _elements;
        private readonly List<IntersectionPoint> _intersectionPoints;
        private DrawingMode _mode;

        public DrawingTool(DrawingMode mode)
        {
            _mode = mode;
            _elements = new List<IElement>();
            _intersectionPoints = new List<IntersectionPoint>();
        }

        public DrawingTool SetMode(DrawingMode mode)
        {
            _mode = mode;

            return this;
        }

        public MultiLine FinishMultiline()
        {
            if (_mode == DrawingMode.PolygonalLine)
            {
                var line = _elements.OfType<MultiLine>().LastOrDefault();

                if (line is not null && line.GetType() == typeof(MultiLine))
                {
                    var multiline = (MultiLine) line;
                    multiline.Completed = true;
                    return line;
                }
            }

            return null;
        }

        public void Reset()
        {
            _elements.Clear();
            _intersectionPoints.Clear();
        }

        public IElement AddPoint(double x, double y)
        {
            if (_mode == DrawingMode.StraightLine)
                return AddPointToStraightLine(x, y);

            if (_mode == DrawingMode.PolygonalLine)
                return AddPointToPolyline(x, y);

            return null;
        }

        public void RemoveLine(BaseLine line)
        {
            var elems = _elements.OfType<BaseLine>();

            _elements.Remove(line);
        }

        private Point? IntersectsWithAny(LineSegment lineSegment)
        {
            var elements = _elements.OfType<BaseLine>();
            foreach (var element in elements)
            {
                var intersectionPoint = Intersects(lineSegment, element);

                if (intersectionPoint is not null)
                    return intersectionPoint;
            }

            return null;
        }

        private Point? IntersectsWithAny(BaseLine el)
        {
            var elements = _elements.OfType<BaseLine>();
            foreach (var element in elements)
            {
                var intersectionPoint = Intersects(el, element);

                if (intersectionPoint is not null)
                    return intersectionPoint;
            }

            return null;
        }

        private IElement AddPointToStraightLine(double x, double y)
        {
            var currentLine = _elements.OfType<StraightLine>().LastOrDefault();
            
            if (currentLine is null || currentLine.Drawn)
            {
                var straightLine = StraightLine.Create(GeometryPoint.Create(x, y));
                _elements.Add(straightLine);
                return straightLine;
            }

            currentLine.EndLine(GeometryPoint.Create(x, y));
            var intersectionPoint = IntersectsWithAny(currentLine);

            if (intersectionPoint is not null)
            {
                currentLine.Intersection = IntersectionPoint.Create(intersectionPoint.Value.X, intersectionPoint.Value.Y);
                _intersectionPoints.Add(currentLine.Intersection);
                _elements.Add(currentLine.Intersection);

                RemoveLine(currentLine);
                return currentLine;
            }

            currentLine.Line = new Line
            {
                X1 = currentLine.Start?.X ?? 0,
                X2 = currentLine.End?.X ?? 0,
                Y1 = currentLine.Start?.Y ?? 0,
                Y2 = currentLine.End?.Y ?? 0,
            };

            return currentLine;
        }

        private IElement AddPointToPolyline(double x, double y)
        {
            var currentLine = _elements.OfType<MultiLine>().LastOrDefault();

            if (currentLine is null || currentLine.IsCompleted())
            {
                var polyline = MultiLine.Create(GeometryPoint.Create(x, y));
                _elements.Add(polyline);
                return polyline;
            }

            currentLine.AddPoint(GeometryPoint.Create(x, y));
            var intersectionPoint = IntersectsWithAny(currentLine.GetLastLineSegment());

            if (intersectionPoint is not null)
            {
                currentLine.Intersection = IntersectionPoint.Create(intersectionPoint.Value.X, intersectionPoint.Value.Y);
                _intersectionPoints.Add(currentLine.Intersection);
                _elements.Add(currentLine.Intersection);
                currentLine.RemoveLastPoint();
                return currentLine;
            }

            return currentLine;
        }


        public UIElement GetLastElement()
        {
            return _elements?.LastOrDefault()?.GetUIElement();
        }

        public Point? Intersects(BaseLine elementA, BaseLine elementB)
        {

            var lineSegmentsA = elementA.GetLineSegments();
            var lineSegmentsB = elementB.GetLineSegments();

            Point? intersection = null;

            foreach (var lineSegmentA in lineSegmentsA)
            {
                foreach (var lineSegmentB in lineSegmentsB)
                {
                    if (lineSegmentA != lineSegmentB)
                    {
                        intersection = MathHelper.LineIntersection(lineSegmentA, lineSegmentB);

                        if (intersection is not null)
                            return intersection;
                    }
                }
            }
            
            return intersection;
        }

        public Point? Intersects(LineSegment segment, BaseLine elementB)
        {
            var lineSegmentsB = elementB.GetLineSegments();

            Point? intersection = null;

            foreach (var lineSegmentA in lineSegmentsB)
            {
                if(lineSegmentA != segment)
                    intersection = MathHelper.LineIntersection(lineSegmentA, segment, 0.001);

                if (intersection is not null)
                    return intersection;
            }

            return intersection;
        }

        public IEnumerable<IntersectionPoint> GetIntersectionPoints()
        {
            return _intersectionPoints;
        }
    }
}
