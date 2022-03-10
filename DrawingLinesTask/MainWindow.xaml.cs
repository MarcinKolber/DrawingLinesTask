using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using DrawingLines.Drawing;
using DrawingLines.Elements.Lines;
using DrawingLines.Enums;

namespace DrawingLines
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDrawingTool _drawingTool;

        public MainWindow()
        {
            InitializeComponent();
            _drawingTool = new DrawingTool(DrawingMode.StraightLine);
            StraightLineMode.IsChecked = true;
        }

        private void Canvas1_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var x = e.GetPosition(DrawingArea).X;
            var y = e.GetPosition(DrawingArea).Y;

            var intersectionPoints = _drawingTool.GetIntersectionPoints();

            foreach (var intersectionPoint in intersectionPoints)
            {
                DrawingArea.Children.Remove(intersectionPoint.GetUIElement());
            }
            
            var element = _drawingTool.AddPoint(x, y);

            if (element.GetType() == typeof(StraightLine))
            {
                DrawStraightLine((StraightLine) element);
            }

            if (element.GetType() == typeof(MultiLine))
            {
                DrawMultiLine((MultiLine)element);
            }
        }

        private void DrawStraightLine(StraightLine line)
        {
            if (line.CanDraw())
            {
                var uiElement = line.GetUIElement();

                if (uiElement is null)
                    return;

                DrawingArea.Children.Add(uiElement);
                line.Drawn = true;
                var label = new Label();
                label.Content = $"New line added A({line.Start.X}, {line.Start.Y}), B({line.Start.X}, {line.Start.Y})";

                LogsList.Items.Add(label);
            }


            if (line.IsDrawn())
            {
                ((Ellipse) line.Start.GetUIElement()).Fill = Brushes.Black;
                ((Ellipse)line.End.GetUIElement()).Fill = Brushes.Black;
            }
            else
            {
                var startingPoint = ((Ellipse)line.Start?.GetUIElement()).Fill = Brushes.DarkRed;

                if(line.End is not null)
                    ((Ellipse)line.End.GetUIElement()).Fill = Brushes.DarkRed;
            }

            var intersection = line.Intersection;

            if (intersection != null)
            {
                var intersectionPoint = intersection.GetUIElement();

                if (intersectionPoint is not null)
                {
                    if (!DrawingArea.Children.Contains(intersectionPoint))
                    {
                        DrawingArea.Children.Add(intersection.GetUIElement());
                        Canvas.SetLeft(intersection.GetUIElement(), intersection.X - 5);
                        Canvas.SetTop(intersection.GetUIElement(), intersection.Y - 5);

                        var label = new Label
                        {
                            Content = $"Intersection at P({Math.Round(intersection.X, 1)}, {Math.Round(intersection.Y, 1)})"
                        };

                        LogsList.Items.Add(label);
                    }

                    RemoveStraightLine(line);
                    _drawingTool.RemoveLine(line);
                    return;
                }
            }

            if (line.Start is not null)
            {
                var startingPoint = line.Start.GetUIElement();

                if (!DrawingArea.Children.Contains(startingPoint))
                {
                    DrawingArea.Children.Add(line.Start.GetUIElement());
                    Canvas.SetLeft(startingPoint, line.Start.X - 5);
                    Canvas.SetTop(startingPoint, line.Start.Y - 5);
                }
            }

            if (line.End is not null)
            {
                var endingPoint = line.End.GetUIElement();

                if (!DrawingArea.Children.Contains(endingPoint))
                {
                    DrawingArea.Children.Add(line.End.GetUIElement());
                    Canvas.SetLeft(endingPoint, line.End.X - 5);
                    Canvas.SetTop(endingPoint, line.End.Y - 5);
                }
            }

        }

        private void DrawMultiLine(MultiLine multiLine)
        {
            var uiElement = (Polyline) multiLine.GetUIElement();

            if (uiElement is null)
                return;

            if (multiLine.Intersection is not null)
            {
                var intersectionPoint = multiLine.Intersection.GetUIElement();
                DrawingArea.Children.Add(intersectionPoint);
                Canvas.SetLeft(intersectionPoint, multiLine.Intersection.X - 5);
                Canvas.SetTop(intersectionPoint, multiLine.Intersection.Y - 5);

                var label = new Label
                {
                    Content = $"Intersection at ({multiLine.Intersection.X},{multiLine.Intersection.Y})"
                };

                LogsList.Items.Add(label);
            }

            if (multiLine.IsDrawn())
            {
                //foreach (var point in multiLine.Points)
                //{
                //    ((Ellipse) point.GetUIElement()).Fill = Brushes.Black;
                //}
            }
            else
            {
                foreach (var point in multiLine.Points)
                {
                    var pointElement = (Ellipse) point.GetUIElement();

                    pointElement.Fill = Brushes.DarkRed;

                    if (!DrawingArea.Children.Contains(pointElement))
                    {
                        DrawingArea.Children.Add(pointElement);

                        Canvas.SetLeft(pointElement, point.X - 5);
                        Canvas.SetTop(pointElement, point.Y - 5);
                    }
                }

                uiElement.Stroke = Brushes.Black;
                //uiElement.StrokeThickness = 5;
                //DrawingArea.Children.Remove(uiElement);
                //DrawingArea.Children.Add(uiElement);
                if (!DrawingArea.Children.Contains(uiElement))
                {
                    DrawingArea.Children.Add(uiElement);
                }

            }

        }


        private void RemoveStraightLine(StraightLine line)
        {
            var startPoint = line.Start?.GetUIElement();
            var endPoint = line.End?.GetUIElement();

            if (startPoint != null && DrawingArea.Children.Contains(startPoint))
            {
                DrawingArea.Children.Remove(startPoint);
            }

            if (endPoint != null && DrawingArea.Children.Contains(endPoint))
            {
                DrawingArea.Children.Remove(endPoint);
            }

            if (line.GetUIElement() != null && DrawingArea.Children.Contains(line.GetUIElement()))
            {
                DrawingArea.Children.Remove(line.GetUIElement());
            }
        }


        private void DrawPoint(ILine line)
        {

        }

        private void StraightLineMode_OnChecked(object sender, RoutedEventArgs e)
        {
            _drawingTool.SetMode(DrawingMode.StraightLine);
        }

        private void ContinuousLineMode_OnChecked(object sender, RoutedEventArgs e)
        {
            _drawingTool.SetMode(DrawingMode.PolygonalLine);
        }

        private void DrawingArea_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                _drawingTool.FinishMultiline();
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _drawingTool.Reset();
            DrawingArea.Children.RemoveRange(0, DrawingArea.Children.Count);
            var logs = LogsList.Items.OfType<Label>().ToList();
            foreach (var log in logs)
            {
                LogsList.Items.Remove(log);

            }
        }
    }
}