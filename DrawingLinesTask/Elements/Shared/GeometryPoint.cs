using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DrawingLines.Elements.Shared
{
    public record GeometryPoint : BasePoint
    {
        public static GeometryPoint Create(double x, double y) => new ()
        {
            X = x,
            Y = y,
        };
    }
}
