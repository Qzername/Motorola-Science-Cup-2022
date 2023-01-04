using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System.Collections.Generic;
using System.Diagnostics;

namespace CodonEditor.Controls
{
    public class DrawingManager : Control
    {
        List<Point> lines; 

        public DrawingManager()
        {
            lines = new List<Point>();
            PointerPressed += DrawingManager_PointerPressed;
        }

        private void DrawingManager_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {  
            lines.Add(e.GetPosition(this));
            InvalidateVisual();
        }

        public override void Render(DrawingContext context)
        {
            var pen = new Pen(Brushes.AliceBlue, 5d);

            context.DrawRectangle(Brushes.Orange, null, new Rect(0, 0, 1000, 500));
                
            for (int i = 0; i < lines.Count / 2; i++)
                context.DrawLine(pen, lines[i * 2], lines[i * 2 + 1]);
        }   
    }
}
