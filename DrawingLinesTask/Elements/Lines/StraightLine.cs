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
        public LineEndPoint? Start { get; set; }
        public LineEndPoint? End { get; set; }

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

        public override UIElement CanDrawNewElement()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<LineSegment> GetLineSegments()
        {
            var segments = new List<LineSegment>();

            if(Start is not null && End is not null)
                segments.Add(new LineSegment(Start.X, Start.Y, End.X, End.Y));

            return segments;
        }

        public StraightLine EndLine(GeometryPoint end)
        {
            End = new LineEndPoint
            {
                X = end.X,
                Y = end.Y,
            };

            return this;
        }

        public override bool IsCompleted()
        {
            //Start.PointElement.Fill = Brushes.Black;
            //End.PointElement.Fill = Brushes.Black;

            return true;
        }

        public override bool CanDraw() =>  
            !Drawn && Start != null && End != null;

        public static StraightLine Create(GeometryPoint start) => new()
        {
            Start = new LineEndPoint
            {
                X = start.X,
                Y = start.Y,
            }
        };

        public static StraightLine Create(GeometryPoint start, GeometryPoint end) => new()
        {
            Start = new LineEndPoint
            {
                X = start.X,
                Y = start.Y,
            },
            End = new LineEndPoint
            {
                X = end.X,
                Y = end.Y,
            },
        };
    }
}
