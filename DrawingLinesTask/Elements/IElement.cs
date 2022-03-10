using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DrawingLines.Elements.Lines;

namespace DrawingLines.Elements
{
    public interface IElement
    {
        bool CanDraw();
        bool IsDrawn();
        UIElement GetUIElement();
        bool Drawn { get; set; }
    }
}
