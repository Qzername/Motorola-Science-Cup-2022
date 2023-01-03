using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Analyzer;
using Analyzer.Models;
using Analyzer.Models.Codons;

namespace BlakApp.Controls
{
    public class ProteinControl : Control
    {
        public static readonly AvaloniaProperty<Sequence> CodonSequenceProperty = AvaloniaProperty.RegisterAttached<ProteinControl, Sequence>(nameof(CodonSequence), typeof(ProteinControl));

        public Sequence CodonSequence
        {
            get { return (Sequence)GetValue(CodonSequenceProperty); }
            set { SetValue(CodonSequenceProperty, value); }
        }

        public override void Render(DrawingContext context)
        {
            Pen pen = new Pen(Brushes.White, 5f, lineCap: PenLineCap.Round);

            Point offset = new Point(10, 10);

            for(int i = 0; i < CodonSequence.CodonsShift1.Length; i++)
            {
                context.DrawLine(pen, new Point(i*50, (1-i%2)*20) + offset, new Point(i*50 + 50, i%2*20) + offset);
            }
        }
    }
}
