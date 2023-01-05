using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CodonEditor.Models.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CodonEditor.Controls
{
    public class DrawingManager : Control
    {
        Point GridCellSize;

        Point? SelectedPoint;

        List<ChemPoint> chemPoints;
        List<Line> lines;

        Pen pen;

        public DrawingManager()
        {
            GridCellSize = new Point(25, 25);

            SelectedPoint = null;

            lines = new List<Line>();
            chemPoints = new List<ChemPoint>();

            pen = new Pen(Brushes.Red, 5d);

            PointerPressed += DrawingManager_PointerPressed;
        }

        private void DrawingManager_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            Point mousePosition = e.GetPosition(this);

            Point correctedMousePosition = new Point(Math.Round(mousePosition.X/GridCellSize.X)*GridCellSize.X,
                            Math.Round(mousePosition.Y/GridCellSize.Y) * GridCellSize.Y);

            if (SelectedPoint == null || SelectedPoint == correctedMousePosition)
                SelectedPoint = correctedMousePosition;
            else
            {
                ChemPoint point1 = GetChemPoint(SelectedPoint.Value);
                ChemPoint point2 = GetChemPoint(correctedMousePosition);

                lines.Add(new Line(point1.ID, point2.ID));
                SelectedPoint = null;
            }

            InvalidateVisual();
        }

        public override void Render(DrawingContext context)
        {
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
                context.DrawLine(pen, 
                    chemPoints.Single(x => x.ID == line.IDChemPoint1).Position, 
                    chemPoints.Single(x => x.ID == line.IDChemPoint2).Position);
            }

            //draw selected point
            if (SelectedPoint is not null)
                context.DrawEllipse(null, pen, SelectedPoint.Value, 10d,10d);
        }

        ChemPoint GetChemPoint(Point Point)
        {
            ChemPoint chemPoint;

            if (chemPoints.Any(x => x.Position == Point))
                chemPoint = chemPoints.Single(x => x.Position == Point);
            else
            {
                chemPoint = new ChemPoint() { ID = chemPoints.Count, Position = Point };
                chemPoints.Add(chemPoint);
            }

            return chemPoint;
        }
    }
}
