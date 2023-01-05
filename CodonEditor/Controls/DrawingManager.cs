using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CodonEditor.Models.Draw;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CodonEditor.Controls
{
    public class DrawingManager : Control
    {
        public static DrawingManager Current;

        Point GridCellSize;

        int nextID;
        Point? SelectedPoint;

        List<ChemPointRaw> chemPoints;
        List<LineRaw> lines;

        Pen singleBind, moreBind, errorBind;

        public DrawingManager()
        {
            Current = this;

            GridCellSize = new Point(25, 25);

            nextID = 0;
            SelectedPoint = null;

            lines = new List<LineRaw>();
            chemPoints = new List<ChemPointRaw>();

            singleBind = new Pen(Brushes.White, 5d);
            moreBind = new Pen(Brushes.Orange, 5d);
            errorBind = new Pen(Brushes.Red, 5d);

            PointerPressed += DrawingManager_PointerPressed;
        }

        private void DrawingManager_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            Point mousePosition = e.GetPosition(this);

            Point correctedMousePosition = new Point(Math.Round(mousePosition.X/GridCellSize.X)*GridCellSize.X,
                            Math.Round(mousePosition.Y/GridCellSize.Y) * GridCellSize.Y);

            if (SelectedPoint == null)
                SelectedPoint = correctedMousePosition;
            else if (SelectedPoint == correctedMousePosition )
                SelectedPoint = null;
            else
            {
                ChemPointRaw point1 = GetChemPoint(SelectedPoint.Value);
                ChemPointRaw point2 = GetChemPoint(correctedMousePosition);

                if(lines.Any(x=>(x.IDChemPoint1 == point1.ID || x.IDChemPoint2 == point1.ID) && (x.IDChemPoint1 == point2.ID || x.IDChemPoint2 == point2.ID)))
                {
                    //im not proud of this section, but this app is not something that will user use so yeah.
                    LineRaw line = lines.Single(x => (x.IDChemPoint1 == point1.ID || x.IDChemPoint2 == point1.ID) && (x.IDChemPoint1 == point2.ID || x.IDChemPoint2 == point2.ID));
                    int index = lines.IndexOf(line);

                    var copy = lines[index];
                    copy.NumberOfBind++;
                    lines[index] = copy;
                }
                else
                    lines.Add(new LineRaw(point1.ID, point2.ID));
                
                SelectedPoint = null;
            }

            InvalidateVisual();
        }

        public override void Render(DrawingContext context)
        {
            Debug.WriteLine($"points: {chemPoints.Count} lines: {lines.Count}");

            //if that line wouldnt exist, avalonia would not detect pointer click
            context.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, Parent.Bounds.Width, Parent.Bounds.Height));

            //draw grid
            for (int x = 1; x < Parent.Bounds.Width / GridCellSize.X-1; x++)
                for (int y = 1; y < Parent.Bounds.Height / GridCellSize.Y; y++)
                    context.DrawEllipse(Brushes.Orange, null, new Point(x * GridCellSize.X, y * GridCellSize.Y), 1f, 1f);

            //draw lines
            for(int i = 0; i < lines.Count;i++)
            {
                LineRaw line = lines[i];

                Pen drawPen;

                if (line.NumberOfBind == 1)
                    drawPen = singleBind;
                else if (line.NumberOfBind == 2)
                    drawPen = moreBind;
                else
                    drawPen = errorBind;

                context.DrawLine(drawPen, 
                    chemPoints.Single(x => x.ID == line.IDChemPoint1).Position, 
                    chemPoints.Single(x => x.ID == line.IDChemPoint2).Position);
            }

            //drawing points and their additional data (charges and special formulas)
            for(int i = 0; i <chemPoints.Count;i++)
            {
                var current = chemPoints[i];

                context.DrawEllipse(Brushes.Red, null, current.Position, 5f, 5f);

                if (current.Charge != 0)
                {
                    int diff = Math.Abs(current.Charge);

                    if (current.Charge < 0)
                        context.DrawText(diff <= 1 ? Brushes.Aqua : errorBind.Brush, new Point(current.Position.X-10f, current.Position.Y -15), new FormattedText("-", Typeface.Default, 42, TextAlignment.Center, TextWrapping.Wrap, new Size(20,20)));
                    else
                        context.DrawText(diff <= 1 ? Brushes.Aqua : errorBind.Brush, new Point(current.Position.X - 10f, current.Position.Y-10), new FormattedText("+", Typeface.Default, 36, TextAlignment.Center, TextWrapping.Wrap, new Size(20, 20)));
                }

                if (!string.IsNullOrEmpty(current.MolecularFormula))
                    context.DrawText(Brushes.Aqua, new Point(current.Position.X - 50, current.Position.Y - 35f), new FormattedText(current.MolecularFormula, Typeface.Default, 21, TextAlignment.Center, TextWrapping.Wrap, new Size(100, 20)));
            }

            //draw selected point
            if (SelectedPoint is not null)
                context.DrawEllipse(null, singleBind, SelectedPoint.Value, 10d,10d);
        }

        public void RevertChange()
        {
            LineRaw lineToRevert = lines.Last();

            if (lines.Count(x => x.IDChemPoint1 == lineToRevert.IDChemPoint1 || x.IDChemPoint2 == lineToRevert.IDChemPoint1) == 1)
                chemPoints.Remove(chemPoints.Single(x => x.ID == lineToRevert.IDChemPoint1));

            if (lines.Count(x => x.IDChemPoint1 == lineToRevert.IDChemPoint2 || x.IDChemPoint2 == lineToRevert.IDChemPoint2) == 1)
                chemPoints.Remove(chemPoints.Single(x => x.ID == lineToRevert.IDChemPoint2));

            lines.Remove(lineToRevert);
            InvalidateVisual();
        }

        public void CleanDrawing()
        {
            lines.Clear();
            chemPoints.Clear();
            InvalidateVisual();
        }

        public void SetRecalculatedDrawing(DrawingDataRaw data, Point GridOffest)
        {
            lines.Clear();
            chemPoints.Clear();

            lines.AddRange(data.Lines);

            for (int i = 0; i < data.Points.Length; i++)
            {
                var copy = data.Points[i];
                copy.Position = new Point((copy.Position.X + GridOffest.X) * GridCellSize.X, (copy.Position.Y + GridOffest.Y) * GridCellSize.Y);
                chemPoints.Add(copy);
            }
           
            InvalidateVisual();
        }

        public DrawingDataRaw? GetRecalculatedDrawing()
        {
            if (chemPoints.Count == 0)
                return null;

            DrawingDataRaw data = new DrawingDataRaw()
            {
                Lines = lines.ToArray(),
                Points = new ChemPointRaw[chemPoints.Count]
            };

            Point offset = new Point(chemPoints.Min(x=>x.Position.X), chemPoints.Min(y=>y.Position.Y));

            for(int i = 0;i<chemPoints.Count;i++)
            {
                var copy = chemPoints[i];

                copy.Position -= offset;
                copy.Position = new Point(copy.Position.X / GridCellSize.X, copy.Position.Y / GridCellSize.Y);

                data.Points[i] = copy;
            }

            return data;
        }

        public void SetAdditionalData(string data)
        {
            if (SelectedPoint == null || !chemPoints.Any(x=>x.Position == SelectedPoint))
                return;

            ChemPointRaw point = chemPoints.Single(x => x.Position == SelectedPoint);
            int index = chemPoints.IndexOf(point);

            if (data == "+")
                point.Charge++;
            else if (data == "-")
                point.Charge--;
            else
                point.MolecularFormula = data;

            chemPoints[index] = point;

            SelectedPoint = null;
            InvalidateVisual();
        }

        ChemPointRaw GetChemPoint(Point Point)
        {
            ChemPointRaw chemPoint;

            if (chemPoints.Any(x => x.Position == Point))
                chemPoint = chemPoints.Single(x => x.Position == Point);
            else
            {
                chemPoint = new ChemPointRaw() { ID = nextID++, Position = Point };
                chemPoints.Add(chemPoint);
            }

            return chemPoint;
        }
    }
}
