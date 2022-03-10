using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingLines.Elements.Shared
{
    public abstract record BasePoint : IPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
