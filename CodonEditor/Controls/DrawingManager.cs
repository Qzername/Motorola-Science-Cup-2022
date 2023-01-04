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
            var pen = new Pen(Brushes.Red, 5d);

            //if that line wouldnt exist, avalonia would not detect pointer click
            context.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, Parent.Bounds.Width, Parent.Bounds.Height));

            for (int i = 0; i < lines.Count / 2; i++)
                context.DrawLine(pen, lines[i * 2], lines[i * 2 + 1]);

            if (lines.Count % 2 == 1)
                context.DrawEllipse(null, pen, lines[^1], 10d,10d);
        }   
    }
}
