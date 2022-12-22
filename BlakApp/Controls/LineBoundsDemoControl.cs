using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Threading;

namespace BlakApp.Controls
{
    public class LineBoundsDemoControl : Control
    {
        public static readonly StyledProperty<Brush> BackgroundProperty =
    AvaloniaProperty.Register<Border, Brush>(nameof(Background));

        public Brush Background
        {
            get { return GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }  

        public static readonly StyledProperty<string> TextTestProperty =
    AvaloniaProperty.Register<Border, string>(nameof(TextTest));

        public string TextTest
        {
            get { return GetValue(TextTestProperty); }
            set { SetValue(TextTestProperty, value); }
        }


        public override void Render(DrawingContext drawingContext)
        {
            var pen = new Pen(Brushes.Green, 20, lineCap: PenLineCap.Square);

            drawingContext.DrawLine(pen, new Point(0,0), new Point(100,100));
            drawingContext.DrawText(Background, new Point(100, 50), new FormattedText(TextTest, new Typeface(new FontFamily("Comic Sans MS")), 200f, TextAlignment.Center, TextWrapping.Wrap, Size.Empty));
        }
    }
}
