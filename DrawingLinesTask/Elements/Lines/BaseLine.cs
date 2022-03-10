using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DrawingLines.Elements.Shared;

namespace DrawingLines.Elements.Lines
{
    public abstract class BaseLine : IElement
    {
        public bool Drawn { get; set; }
        public abstract bool IsCompleted();
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

        public abstract UIElement CanDrawNewElement();

        public abstract IEnumerable<LineSegment> GetLineSegments();
    }
}
