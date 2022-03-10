using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using DrawingLines.Drawing;
using DrawingLines.Elements.Lines;
using DrawingLines.Elements.Shared;
using DrawingLines.Enums;

namespace DrawingLines
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDrawingTool _drawingTool;
        private readonly Brush NewPointColor = Brushes.DarkRed;
        private readonly Brush CompletedPointColor = Brushes.Black;
        private readonly Brush LineColor = Brushes.Black;
        private readonly int LineThickness = 5;

        public MainWindow()
        {
            InitializeComponent();
            _drawingTool = new DrawingTool(DrawingMode.StraightLine);
            StraightLineMode.IsChecked = true;
        }

        private void DrawingArea_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var x = e.GetPosition(DrawingArea).X;
            var y = e.GetPosition(DrawingArea).Y;

            ClearIntersectionPoints();

            var element = _drawingTool.AddPoint(x, y);

            if (element.GetType() == typeof(StraightLine))
                DrawStraightLine((StraightLine) element);

            if (element.GetType() == typeof(MultiLine))
                DrawMultiLine((MultiLine)element);
        }

        private void ClearIntersectionPoints()
        {
            var intersectionPoints = _drawingTool.GetIntersectionPoints();

            foreach (var intersectionPoint in intersectionPoints)
            {
                DrawingArea.Children.Remove(intersectionPoint.GetUIElement());
            }
        }

        private void AddNewLogEntry(string content)
        {
            var label = new Label
            {
                Content = content
            };

            LogsList.Items.Add(label);
        }

        private void ColorPointsBlack(IEnumerable<LineEndpoint> points)
        {
            ColorPoints(points, CompletedPointColor);
        }

        private void ColorPointsRed(IEnumerable<LineEndpoint> points)
        {
            ColorPoints(points, NewPointColor);
        }

        private static void ColorPoints(IEnumerable<LineEndpoint> points, Brush brush)
        {
            foreach (var point in points)
            {
                ((Ellipse)point.GetUIElement()).Fill = brush;
            }
        }

        private void DrawNewPoint(LineEndpoint point)
        {
            var pointElement = (Ellipse)point.GetUIElement();

            pointElement.Fill = NewPointColor;

            if (!DrawingArea.Children.Contains(pointElement))
            {
                DrawingArea.Children.Add(pointElement);

                Canvas.SetLeft(pointElement, point.X - 5);
                Canvas.SetTop(pointElement, point.Y - 5);
            }
        }

        private void DrawIntersectionPoint(UIElement uiElement, double x, double y)
        {
            if (DrawingArea.Children.Contains(uiElement))
                return;

            DrawingArea.Children.Add(uiElement);
            Canvas.SetLeft(uiElement, x - 5);
            Canvas.SetTop(uiElement, y - 5);
        }

        private void DrawLineEndpoint(LineEndpoint endpoint)
        {
            var uiElement = (Ellipse) endpoint.GetUIElement();

            if (DrawingArea.Children.Contains(uiElement))
                return;

            DrawingArea.Children.Add(uiElement);
            Canvas.SetLeft(uiElement, endpoint.X - 5);
            Canvas.SetTop(uiElement, endpoint.Y - 5);
        }

        private void DrawStraightLine(StraightLine line)
        {
            if (line.CanDraw())
            {
                var uiElement = (Line) line.GetUIElement();
                uiElement.StrokeThickness = LineThickness;
                uiElement.Stroke = LineColor;

                DrawingArea.Children.Add(uiElement);
                line.Drawn = true;
                AddNewLogEntry($"New line added A({line.Start.X}, {line.Start.Y}), B({line.Start.X}, {line.Start.Y})");
            }

            if (line.IsDrawn())
                ColorPointsBlack(line.GetAllPoints());
            else
                ColorPointsRed(line.GetAllPoints());

            var intersection = line.Intersection;

            var intersectionPoint = intersection?.GetUIElement();

            if (intersectionPoint != null)
            {
                DrawIntersectionPoint(intersectionPoint, intersection.X, intersection.Y);
                AddNewLogEntry(
                    $"Intersection at P({Math.Round(intersection.X, 2)}, {Math.Round(intersection.Y, 2)})");

                RemoveStraightLine(line);
                _drawingTool.RemoveLine(line);
                return;
            }

            if (line.Start is not null)
                DrawLineEndpoint(line.Start);

            if (line.End is not null)
                DrawLineEndpoint(line.End);
        }

        private void DrawMultiLine(MultiLine multiLine)
        {
            var uiElement = (Polyline)multiLine.GetUIElement();

            if (multiLine.Intersection is not null)
            {
                var intersectionPoint = multiLine.Intersection.GetUIElement();
                DrawingArea.Children.Add(intersectionPoint);
                Canvas.SetLeft(intersectionPoint, multiLine.Intersection.X - 5);
                Canvas.SetTop(intersectionPoint, multiLine.Intersection.Y - 5);

                AddNewLogEntry($"Intersection at ({Math.Round(multiLine.Intersection.X, 2)},{Math.Round(multiLine.Intersection.Y, 2)})");
                multiLine.Intersection = null;
            }

            ColorPointsRed(multiLine.GetAllPoints());

            foreach (var point in multiLine.Points)
            {
                DrawNewPoint(point);
            }

            if (!DrawingArea.Children.Contains(uiElement))
            {
                uiElement.StrokeThickness = LineThickness;
                uiElement.Stroke = LineColor;
                DrawingArea.Children.Add(uiElement);
            }
        }

        private void RemoveStraightLine(StraightLine line)
        {
            var startPoint = line.Start?.GetUIElement();
            var endPoint = line.End?.GetUIElement();
            var lineElement = line.GetUIElement();

            if (startPoint is not null && DrawingArea.Children.Contains(startPoint))
                DrawingArea.Children.Remove(startPoint);

            if (endPoint is not null && DrawingArea.Children.Contains(endPoint))
                DrawingArea.Children.Remove(endPoint);

            if (DrawingArea.Children.Contains(lineElement))
                DrawingArea.Children.Remove(line.GetUIElement());
        }

        private void StraightLineMode_OnChecked(object sender, RoutedEventArgs e)
        {
            _drawingTool.SetMode(DrawingMode.StraightLine);
        }

        private void Polyline_OnChecked(object sender, RoutedEventArgs e)
        {
            _drawingTool.SetMode(DrawingMode.PolygonalLine);
        }

        private void DrawingArea_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var multiLine = _drawingTool.FinishMultiline();

                ColorPointsBlack(multiLine.GetAllPoints());

                AddNewLogEntry($"New polyline added");
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _drawingTool.Reset();
            DrawingArea.Children.RemoveRange(0, DrawingArea.Children.Count);
            var logs = LogsList.Items.OfType<Label>().ToList();

            foreach (var log in logs)
                LogsList.Items.Remove(log);
        }
    }
}