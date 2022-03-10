using System;
using System.Windows;
using DrawingLines.Elements.Lines;

namespace DrawingLines;

public static class MathHelper
{
    public static Point? LineIntersection(LineSegment segmentAB, LineSegment segmentCD, double tolerance = 0)
    {
        // Code based on:
        // https://www.geeksforgeeks.org/program-for-point-of-intersection-of-two-lines/

        // Calculating 
        var a1 = segmentAB.BY - segmentAB.AY;
        var b1 = segmentAB.AX - segmentAB.BX;
        var c1 = a1 * (segmentAB.AX) + b1 * (segmentAB.AY);

        var a2 = segmentCD.BY - segmentCD.AY;
        var b2 = segmentCD.AX - segmentCD.BX;
        var c2 = a2 * (segmentCD.AX) + b2 * (segmentCD.AY);

        var d = a1 * b2 - a2 * b1;

        // For parallel lines
        if (d == 0)
            return null;

        var x = (b2 * c1 - b1 * c2) / d;
        var y = (a1 * c2 - a2 * c1) / d;
            
        if((x < (Math.Max(Math.Min(segmentAB.AX, segmentAB.BX), Math.Min(segmentCD.AX, segmentCD.BX) ) + tolerance)
            || (x > Math.Min(Math.Max(segmentAB.AX, segmentAB.BX), Math.Max(segmentCD.AX, segmentCD.BX)) - tolerance)))
            return null;

        return new Point(x, y);
    }
}