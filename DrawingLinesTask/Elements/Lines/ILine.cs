using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DrawingLines.Elements.Shared;

namespace DrawingLines.Elements.Lines
{
    public interface ILine : IElement
    {
        IntersectionPoint? Intersection { get; set; }
    }
}
