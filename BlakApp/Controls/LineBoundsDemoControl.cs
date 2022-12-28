using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Rendering.SceneGraph;
using System.Diagnostics;
using SkiaSharp;

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
            var pen = new Pen(Brushes.DarkRed, 5, lineCap: PenLineCap.Round);

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            for (int i = 0; i < 1000;i++)
            {
                pen.Brush = new SolidColorBrush(GenerateRandomColor());

                Random rnd = new Random();

                drawingContext.DrawLine(pen, new Point(rnd.Next(0, 1500), rnd.Next(0, 1000)), new Point(rnd.Next(0,1500), rnd.Next(0, 1000)));
            }

            stopwatch.Stop();

            drawingContext.DrawText(Brushes.Red, new Point(0, 0), new FormattedText(stopwatch.ElapsedMilliseconds.ToString(), new Typeface("Arial"), 120f, TextAlignment.Center, TextWrapping.Wrap, Size.Empty));
        }

        Color GenerateRandomColor()
        {
            Random rnd = new Random();
            return new Color(255, Convert.ToByte(rnd.Next(0, 255)), Convert.ToByte(rnd.Next(0, 255)), Convert.ToByte(rnd.Next(0, 255)));
        }
    }
}
