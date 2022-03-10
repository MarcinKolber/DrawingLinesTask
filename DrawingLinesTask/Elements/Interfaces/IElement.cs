using System.Windows;

namespace DrawingLines.Elements.Interfaces
{
    public interface IElement
    {
        UIElement GetUIElement();
        bool Drawn { get; set; }
    }
}
