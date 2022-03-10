using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using DrawingLines.Elements.Shared;

namespace DrawingLines.Elements.Lines
{
    public class StraightLine : BaseLine
    {
        public Line? Line { get; set; }
        public LineEndpoint? Start { get; set; }
        public LineEndpoint? End { get; set; }

        public override UIElement GetUIElement()
        {
            if (Line is null && Start is not null && End is not null)
                Line = new Line
                {
                    X1 = Start.X,
                    Y1 = Start.Y,
                    X2 = End.X,
                    Y2 = End.Y,
                    Stroke = Brushes.Black,
                    StrokeThickness = 5,
                };

            return Line;
        }

        public override IEnumerable<LineSegment> GetLineSegments()
        {
            var segments = new List<LineSegment>();

            if(Start is not null && End is not null)
                segments.Add(new LineSegment(Start.X, Start.Y, End.X, End.Y));

            return segments;
        }

        public override IEnumerable<LineEndpoint> GetAllPoints()
        {
            var points = new List<LineEndpoint>();

            if (Start is not null)
                points.Add(Start);

            if (End is not null)
                points.Add(End);

            return points;
        }

        public StraightLine EndLine(GeometryPoint end)
        {
            End = new LineEndpoint
            {
                X = end.X,
                Y = end.Y,
            };

            return this;
        }

        public override bool IsCompleted()
            => Start is not null && End is not null;

        public override bool CanDraw() =>  
            !Drawn && Start is not null && End is not null && Intersection is null;

        public static StraightLine Create(GeometryPoint start) => new()
        {
            Start = new LineEndpoint
            {
                X = start.X,
                Y = start.Y,
            }, 
            Line = new Line
            {
                X1 = start.X,
                Y1 = start.Y,
                Stroke = Brushes.Black,
                StrokeThickness = 5
            }
        };

        public static StraightLine Create(GeometryPoint start, GeometryPoint end) => new()
        {
            Start = new LineEndpoint
            {
                X = start.X,
                Y = start.Y,
            },
            End = new LineEndpoint
            {
                X = end.X,
                Y = end.Y,
            },
        };
    }
}
