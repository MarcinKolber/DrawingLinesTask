using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DrawingLines.Elements;
using DrawingLines.Elements.Interfaces;
using DrawingLines.Elements.Lines;
using DrawingLines.Elements.Points;
using DrawingLines.Enums;

namespace DrawingLines.Drawing
{
    public interface IDrawingTool
    {
        IElement AddPoint(double x, double y);
        void RemoveLine(BaseLine line);
        IEnumerable<IntersectionPoint> GetIntersectionPoints();
        DrawingTool SetMode(DrawingMode mode);
        MultiLine FinishMultiline();
        void Reset();
    }
}
