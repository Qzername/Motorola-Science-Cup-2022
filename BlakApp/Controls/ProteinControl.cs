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
            Pen pen = new Pen(Brushes.Orange);
            context.DrawLine(pen, new Point(0, 100), new Point(100, 100));

            context.DrawText(Brushes.Orange, new Point(0, 0), new FormattedText(Sequence.CodonsToString(CodonSequence.CodonsShift1), new Typeface("Arial"), 60d, TextAlignment.Center, TextWrapping.Wrap, Size.Empty));
        }
    }
}
