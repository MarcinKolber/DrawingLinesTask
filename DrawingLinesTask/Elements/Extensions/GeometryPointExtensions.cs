﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawingLines.Elements.Shared;

namespace DrawingLines.Elements.Extensions
{
    public static class GeometryPointExtensions
    {
        public static LineEndPoint ToEndpoint(this GeometryPoint geometryPoint)
            => new()
            {
                X = geometryPoint.X,
                Y = geometryPoint.Y,
            };
    }
}
