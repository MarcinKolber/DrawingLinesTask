using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DrawingLines.Drawing.Constants;

public static class PointStyling
{
    public const int Width = 10;
    public const int Height = 10;
    public static readonly Brush NewPointColor = Brushes.DarkRed;
    public static readonly Brush CompletedPointColor = Brushes.Black;
    public static readonly Brush LineColor = Brushes.Black;
    public static readonly int LineThickness = 5;
}