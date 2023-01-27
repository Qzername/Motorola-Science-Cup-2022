using Analyzer.Analyzers;
using Analyzer.Models;
using Analyzer.Models.Draw;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
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

        List<ChemPoint> chemPoints;
        List<Line> lines;

        Pen singleBind, moreBind, errorBind;

        public DrawingManager()
        {
            Current = this;

            GridCellSize = new Point(25, 25);

            nextID = 0;
            SelectedPoint = null;

            lines = new List<Line>();
            chemPoints = new List<ChemPoint>();

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
                ChemPoint point1 = GetChemPoint(SelectedPoint.Value);
                ChemPoint point2 = GetChemPoint(correctedMousePosition);

                if(lines.Any(x=>(x.IDChemPoint1 == point1.ID || x.IDChemPoint2 == point1.ID) && (x.IDChemPoint1 == point2.ID || x.IDChemPoint2 == point2.ID)))
                {
                    //im not proud of this section, but this app is not something that will user use so yeah.
                    Line line = lines.Single(x => (x.IDChemPoint1 == point1.ID || x.IDChemPoint2 == point1.ID) && (x.IDChemPoint1 == point2.ID || x.IDChemPoint2 == point2.ID));
                    int index = lines.IndexOf(line);

                    var copy = lines[index];
                    copy.NumberOfBind++;
                    lines[index] = copy;
                }
                else
                    lines.Add(new Line(point1.ID, point2.ID));
                
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
                Line line = lines[i];

                Pen drawPen;

                if (line.NumberOfBind == 1)
                    drawPen = singleBind;
                else if (line.NumberOfBind == 2)
                    drawPen = moreBind;
                else
                    drawPen = errorBind;

                context.DrawLine(drawPen, 
                    ComparisonTools.Convert(chemPoints.Single(x => x.ID == line.IDChemPoint1).Position), 
                    ComparisonTools.Convert(chemPoints.Single(x => x.ID == line.IDChemPoint2).Position));
            }

            //drawing points and their additional data (charges and special formulas)
            for(int i = 0; i <chemPoints.Count;i++)
            {
                var current = chemPoints[i];

                context.DrawEllipse(Brushes.Red, null, ComparisonTools.Convert(current.Position), 5f, 5f);

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
            Line lineToRevert = lines.Last();

            if (lines.Count(x => x.IDChemPoint1 == lineToRevert.IDChemPoint1 || x.IDChemPoint2 == lineToRevert.IDChemPoint1) == 1)
                chemPoints.Remove(chemPoints.Single(x => x.ID == lineToRevert.IDChemPoint1));

            if (lines.Count(x => x.IDChemPoint1 == lineToRevert.IDChemPoint2 || x.IDChemPoint2 == lineToRevert.IDChemPoint2) == 1)
                chemPoints.Remove(chemPoints.Single(x => x.ID == lineToRevert.IDChemPoint2));

            lines.Remove(lineToRevert);
            InvalidateVisual();
        }

        public void CleanDrawing()
        {
            nextID = 0;

            lines.Clear();
            chemPoints.Clear();
            InvalidateVisual();
        }

        public void SetRecalculatedDrawing(DrawingData data, Point GridOffest)
        {
            lines.Clear();
            chemPoints.Clear();

            lines.AddRange(data.Lines);

            int maxID = 0;

            for (int i = 0; i < data.Points.Length; i++)
            {
                var copy = data.Points[i];
                copy.Position = ComparisonTools.Convert(new Point((copy.Position.X + GridOffest.X) * GridCellSize.X, (copy.Position.Y + GridOffest.Y) * GridCellSize.Y));

                if (copy.ID > maxID)
                    maxID = copy.ID+1;

                chemPoints.Add(copy);
            }

            //for some reason i need to add +1 to this because it can cause application to crash
            //due to fact that for some reason there will be two points with same ID
            nextID = maxID+1; 

            InvalidateVisual();
        }

        public DrawingData? GetRecalculatedDrawing()
        {
            if (chemPoints.Count == 0)
                return null;

            DrawingData data = new DrawingData()
            {
                Lines = lines.ToArray(),
                Points = new ChemPoint[chemPoints.Count],
            };

            data.Data = CodonAnalyzer.CreateCodonData(data);

            Point offset = new Point(chemPoints.Min(x=>x.Position.X), chemPoints.Min(y=>y.Position.Y));

            for(int i = 0;i<chemPoints.Count;i++)
            {
                var copy = chemPoints[i];

                copy.Position = ComparisonTools.Subtraction(copy.Position, offset);

                copy.Position = new Position(Convert.ToInt32(copy.Position.X / GridCellSize.X), Convert.ToInt32(copy.Position.Y / GridCellSize.Y));

                data.Points[i] = copy;
            }

            return data;
        }

        public void SetAdditionalData(string data)
        {
            if (SelectedPoint == null || !chemPoints.Any(x=> ComparisonTools.IsEqual(x.Position, SelectedPoint.Value)))
                return;

            ChemPoint point = chemPoints.Single(x =>ComparisonTools.IsEqual(x.Position, SelectedPoint.Value));
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

        ChemPoint GetChemPoint(Point Point)
        {
            ChemPoint chemPoint;

            if (chemPoints.Any(x => ComparisonTools.IsEqual(x.Position, Point)))
                chemPoint = chemPoints.Single(x => ComparisonTools.IsEqual(x.Position, Point));
            else
            {
                chemPoint = new ChemPoint() { ID = nextID++, Position = ComparisonTools.Convert(Point) };
                chemPoints.Add(chemPoint);
            }

            return chemPoint;
        }

        /// <summary>
        /// Ready methods for comparing Avalonia's Point and Analyzer's Position
        /// I'm not proud of the fact that this class exists,
        /// but it's only solution I could find
        /// </summary>
        internal static class ComparisonTools
        {
            public static bool IsEqual(Position position, Point point) => position.X == point.X && position.Y == point.Y;
            public static Position Convert(Point point) => new Position(System.Convert.ToInt32(point.X), System.Convert.ToInt32(point.Y));
            public static Point Convert(Position position) => new Point(position.X, position.Y);
            public static Position Subtraction(Position position, Point point) => new Position(System.Convert.ToInt32(position.X - point.X), System.Convert.ToInt32(position.Y - point.Y));
            public static Point Subtraction(Point point, Position position) => new Point(point.X - position.X, point.Y - position.Y);
        }
    }
}
