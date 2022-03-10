using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DrawingLines.Elements.Interfaces;
using DrawingLines.Elements.Points;

namespace DrawingLines.Elements.Lines
{
    public abstract class BaseLine : IElement
    {
        public bool Drawn { get; set; }
        public IntersectionPoint? Intersection { get; set; }

        public virtual bool CanDraw()
        {
            return !Drawn;
        }

        public virtual bool IsDrawn()
        {
            return Drawn;
        }

        public abstract UIElement GetUIElement();
        public abstract IEnumerable<LineSegment> GetLineSegments();
        public abstract IEnumerable<LineEndpoint> GetAllPoints();
    }
}
