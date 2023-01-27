using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Analyzer;
using Analyzer.Models;
using Analyzer.Models.Codons;
using Analyzer.Models.Draw;
using Avalonia.Controls.Shapes;
using System.Drawing.Drawing2D;
using Line = Analyzer.Models.Draw.Line;
using System.Linq;
using System;
using System.Diagnostics;

namespace BlakApp.Controls
{
    public class ProteinDrawingManager : Control
    {
        public static readonly AvaloniaProperty<Sequence> CodonSequenceProperty = AvaloniaProperty.RegisterAttached<ProteinDrawingManager, Sequence>(nameof(CodonSequence), typeof(ProteinDrawingManager));

        public Sequence CodonSequence
        {
            get { return (Sequence)GetValue(CodonSequenceProperty); }
            set { SetValue(CodonSequenceProperty, value); }
        }

        Pen singleBind, moreBind;
        FormattedText text;

        Point offset;
        Point pointOffset;
        Point pointFlipedOffset { get => new Point(pointOffset.X, -pointOffset.Y); }

        public ProteinDrawingManager()
        {
            offset = new Point(10, 200);
            pointOffset = new Point(20, 20);

            singleBind = new Pen(Brushes.White);
            moreBind = new Pen(Brushes.Orange);

            text = new FormattedText("ABC", Typeface.Default, 12f, TextAlignment.Center, TextWrapping.NoWrap, Size.Empty);
        }

        public override void Render(DrawingContext context)
        {
            context.DrawText(Brushes.White, new Point(10,10), new FormattedText(Sequence.CodonsToString(CodonSequence.CodonsShift1), new Typeface("Arial"), 60f, TextAlignment.Center, TextWrapping.Wrap, Size.Empty));

            double sizeOfOneCodon = 6 * pointOffset.X;

            var sequenceToDraw = CodonSequence.CodonsShift1.Where(x => x.CodonType != CodonType.End).ToArray();

            for (int i = 0; i < sequenceToDraw.Length; i++)
            {
                var codon = sequenceToDraw[i];

                Point codonOffset = (codon.Letter == "P" ? new Point(-pointOffset.X, 0) : new Point(0, 0));

                DrawCodon(context, codon.DrawingData.Value, offset + new Point(i* sizeOfOneCodon, (i % 2 == 1 ? 5*pointOffset.Y : 0)) + codonOffset, (i % 2 == 0 ? pointOffset : pointFlipedOffset));
            }

            Width = CodonSequence.CodonsShift1.Length * sizeOfOneCodon + offset.X +100;
        }

        void DrawCodon(DrawingContext context, DrawingData data, Point offset, Point pointOffset)
        {
            //draw lines
            for (int i = 0; i < data.Lines.Length; i++)
            {
                Line line = data.Lines[i];

                Pen drawPen;

                if (line.NumberOfBind == 1)
                    drawPen = singleBind;
                else
                    drawPen = moreBind;

                Point pos1 = ChemPointPosition(data.Points.Single(x => x.ID == line.IDChemPoint1), pointOffset);
                Point pos2 = ChemPointPosition(data.Points.Single(x => x.ID == line.IDChemPoint2), pointOffset);

                context.DrawLine(drawPen, pos1 + offset, pos2 + offset);
            }

            //draw texts
            for(int i = 0; i < data.Points.Length;i++)
            {
                ChemPoint point = data.Points[i];

                if (point.Charge == 0 && string.IsNullOrEmpty(point.MolecularFormula))
                    continue;

                text.Text = point.MolecularFormula;
                context.DrawText(Brushes.Red, ChemPointPosition(point, pointOffset) + offset, text);
            }
        }

        /// <summary>
        /// Return chempoint position as Avalonia's Point multipled by pointOffset
        /// </summary>
        Point ChemPointPosition(ChemPoint point, Point pointOffest) => new Point(point.Position.X * pointOffest.X, point.Position.Y * pointOffest.Y);
    }
}
