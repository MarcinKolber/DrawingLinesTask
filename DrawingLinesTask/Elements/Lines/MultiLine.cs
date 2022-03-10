using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using DrawingLines.Elements.Extensions;
using DrawingLines.Elements.Points;

namespace DrawingLines.Elements.Lines
{
    public class MultiLine : BaseLine
    {
        public Polyline? Polyline { get; set; }

        public List<LineEndpoint> Points { get; set; } = new List<LineEndpoint>();
        public bool Completed { get; set; }

        public static MultiLine Create(GeometryPoint start) => new()
        {
            Points = new List<LineEndpoint>()
            {
                start.ToEndpoint()
            },
            Polyline = new Polyline
            {
                Points = new PointCollection { new(start.X, start.Y) },
            }
        };

        public void AddPoint(GeometryPoint point)
        {
            Points.Add(point.ToEndpoint());

            if (Polyline is null)
                Polyline = new Polyline
                {
                    Points = new PointCollection(Points.Select(x => new Point(x.X, x.Y))),
                };

            Polyline.Points.Add(new Point(point.X, point.Y));
        }

        public bool IsCompleted()
        {
            return Completed;
        }

        public override UIElement GetUIElement()
        {
            if (Polyline is null)
                return new Polyline
                {
                    Points = new PointCollection(Points.Select(x => new Point(x.X, x.Y))),
                };

            return Polyline;
        }

        public override IEnumerable<LineSegment> GetLineSegments()
        {
            var segments = new List<LineSegment>();

            for (var i = 1; i < Points.Count; i++)
            {
                var segment = new LineSegment(Points[i - 1].X, Points[i - 1].Y, Points[i].X, Points[i].Y);
                segments.Add(segment);
            }

            return segments;
        }

        public override IEnumerable<LineEndpoint> GetAllPoints()
            => Points;

        public void RemoveLastPoint()
        {
            if(Points.Count > 0)
                Points.RemoveAt(Points.Count - 1);

            if(Polyline is not null)
                Polyline.Points.RemoveAt(Polyline.Points.Count - 1);
        }

        public override bool CanDraw()
        {
            return !Drawn && Points.Count >= 2;
        }

        public LineSegment GetLastLineSegment()
        {
            if (Points.Count() >= 2)
            {
                var lastPoints = Points.Skip(Math.Max(0, Points.Count() - 2)).ToArray();
                return new LineSegment(lastPoints[0].X, lastPoints[0].Y, lastPoints[1].X, lastPoints[1].Y);
            }

            return null;
        }
    }
}
