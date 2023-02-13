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
using Analyzer.Models.Terminuses;
using Analyzer.Analyzers;
using Analyzer.Models.Drawing;
using Avalonia.Threading;

namespace BlakApp.Controls
{
    public class ProteinDrawingManager : Control
    {
        public static readonly AvaloniaProperty<Sequence> PeptideSequenceProperty = AvaloniaProperty.RegisterAttached<ProteinDrawingManager, Sequence>(nameof(PeptideSequence), typeof(ProteinDrawingManager));

        /// <summary>
        /// Drawing tool will get only first shift to render
        /// </summary>
        public Sequence PeptideSequence
        {
            get { return (Sequence)GetValue(PeptideSequenceProperty); }
            set { SetValue(PeptideSequenceProperty, value); CalculateSequenceOffset(); CalculateDrawingData(); }
        }

        Pen drawPen;
        FormattedText text;

        Point offset;
        Point pointOffset;
        Point pointFlipedOffset { get => new Point(pointOffset.X, -pointOffset.Y); }
        Point lineOffset;
        Point sequenceOffset; //for example we need to add that offset if proline is first

        FullTerminus terminus;
        DrawingData[] dataToDraw;

        public ProteinDrawingManager()
        {
            offset = new Point(10, 200);
            pointOffset = new Point(20, 20);
            lineOffset = new Point(2, 0);

            drawPen = new Pen(Brushes.White, 2d);
            
            text = new FormattedText("ABC", System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Typeface.Default, 12f, Brushes.White);

            terminus = DrawingHelper.ConnectTerminuses(DatabaseReader.Terminuses[0], DatabaseReader.Terminuses[1]);

            Height = 420;

            PropertyChanged += ProteinDrawingManager_PropertyChanged;
        }

        private void ProteinDrawingManager_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if(e.Property == PeptideSequenceProperty)
            {
                PeptideSequence = (Sequence)e.NewValue;
                InvalidateVisual();
            }
        }

        public void CalculateSequenceOffset()
        {
            sequenceOffset = new Point(0, 0);

            if (PeptideSequence.CodonsShift1 is null)
                return;

            if (PeptideSequence.CodonsShift1[0].Letter == "P")
                sequenceOffset = new Point(pointOffset.X, 0);
        }

        public void CalculateDrawingData()
        {
            if (PeptideSequence.CodonsShift1 is null)
                return;

            var sequenceToDraw = PeptideSequence.CodonsShift1.Where(x => x.CodonType != CodonType.End).ToArray();

            dataToDraw = new DrawingData[sequenceToDraw.Length];

            for (int i = 0; i < sequenceToDraw.Length; i++)
            {
                var codon = sequenceToDraw[i];
                dataToDraw[i] = DrawingHelper.ConnectCodonWithTerminus(codon, terminus);
            }
        }

        public override void Render(DrawingContext context)
        {
            if (PeptideSequence.CodonsShift1 == null || PeptideSequence.CodonsShift1.Length == 0)
                return;

            if (dataToDraw is null)
                CalculateDrawingData();

            double sizeOfOneCodon = 6 * pointOffset.X;

            for (int c = 0; c < dataToDraw.Length; c++)
            {
                var data = dataToDraw[c];
                var codonOffset = offset + new Point(c * sizeOfOneCodon, 0) + (c % 2 == 0 ? new Point(0, 0) : new Point(0, pointOffset.Y)) + sequenceOffset;
                var codonPointOffset = (c % 2 == 0 ? pointOffset : pointFlipedOffset);

                //draw lines
                for (int i = 0; i < data.Lines.Length; i++)
                {
                    Line line = data.Lines[i];

                    Point pos1 = ChemPointPosition(data.Points.Single(x => x.ID == line.IDChemPoint1), codonPointOffset);
                    Point pos2 = ChemPointPosition(data.Points.Single(x => x.ID == line.IDChemPoint2), codonPointOffset);

                    if (line.NumberOfBind == 1)
                        context.DrawLine(drawPen, pos1 + codonOffset, pos2 + codonOffset);
                    else
                    {
                        Point specialCase = pos1 - pos2;

                        bool isSpecialCase = Math.Abs(specialCase.X) == 2 * pointOffset.X && Math.Abs(specialCase.Y) == 1 * pointOffset.Y;

                        if (isSpecialCase)
                        {
                            pos1 = pos1 - new Point(lineOffset.Y,lineOffset.X);
                            pos2 = pos2 - new Point(lineOffset.Y, lineOffset.X);
                        }
                        else
                        {
                            pos1 = pos1 - lineOffset;
                            pos2 = pos2 - lineOffset;
                        }

                        context.DrawLine(drawPen, pos1 + codonOffset, pos2 + codonOffset);
                        
                        if(isSpecialCase)
                        {
                            pos1 = pos1 + new Point(lineOffset.Y, lineOffset.X*2);
                            pos2 = pos2 + new Point(lineOffset.Y, lineOffset.X*2);
                        }
                        else
                        {
                            pos1 = pos1 + new Point(lineOffset.X * 2, lineOffset.Y);
                            pos2 = pos2 + new Point(lineOffset.X * 2, lineOffset.Y);
                        }

                        context.DrawLine(drawPen, pos1 + codonOffset, pos2 + codonOffset);
                    }
                }
            }

            //draw texts
            for (int c = 0; c < dataToDraw.Length; c++)
            {
                var data = dataToDraw[c];
                var codonOffset = offset + new Point(c * sizeOfOneCodon, 0) + (c % 2 == 0 ? new Point(0, 0) : new Point(0, pointOffset.Y)) + sequenceOffset;
                var codonPointOffset = (c % 2 == 0 ? pointOffset : pointFlipedOffset);

                for (int i = 0; i < data.Points.Length; i++)
                {
                    ChemPoint point = data.Points[i];

                    if (string.IsNullOrEmpty(point.MolecularFormula))
                        continue;

                    if (c != 0 && point.ID == terminus.ConnectionPoint)
                        continue;
                    if (c != dataToDraw.Length - 1 && point.ID == terminus.ExitPoint)
                    {
                        point.MolecularFormula = "NH";
                        point.Charge = 0;
                    }

                    text = new FormattedText(ReplaceNumbersWithSubscripts(point.MolecularFormula), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Typeface.Default, 12f, Brushes.White);

                    var position = ChemPointPosition(point, codonPointOffset) + codonOffset;

                    context.DrawRectangle(Brushes.Black, null, new Rect(position.X - (point.MolecularFormula.Length > 1 ? 13:6), position.Y - 5, (point.MolecularFormula.Length > 1?26:15), 15));
                    context.DrawText(text, position - ( point.MolecularFormula.Length == 1 ? new Point(5,7) : new Point(10,7)));

                    if (point.Charge == 0)
                        continue;

                    text = new FormattedText((point.Charge < 0 ? "-" : "+"), System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Typeface.Default, 12f, Brushes.White);

                    context.DrawText(text, position + new Point(5 * (point.MolecularFormula.Length > 1 ? 2:0.8), -15));
                }
            }
            Dispatcher.UIThread.Post(() =>
            {
                Width = PeptideSequence.CodonsShift1.Length * sizeOfOneCodon + offset.X + 100;     //set width
            },DispatcherPriority.Render);

           
        }

        string ReplaceNumbersWithSubscripts(string text)
        {
            text = text.Replace('0', '₀');
            text = text.Replace('1', '₁');
            text = text.Replace('2', '₂');
            text = text.Replace('3', '₃');
            text = text.Replace('4', '₄');
            text = text.Replace('5', '₅');
            text = text.Replace('6', '₆');
            text = text.Replace('7', '₇');
            text = text.Replace('8', '₈');
            text = text.Replace('9', '₉');

            return text;
        }

        /// <summary>
        /// Return chempoint position as Avalonia's Point multipled by pointOffset
        /// </summary>
        Point ChemPointPosition(ChemPoint point, Point pointOffest) => new Point(point.Position.X * pointOffest.X, point.Position.Y * pointOffest.Y);
    }
}
