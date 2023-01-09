using Analyzer.Models.Drawing;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodonEditor.Models.Draw
{
    public struct DrawingDataRaw
    {
        public ChemPointRaw[] Points;
        public LineRaw[] Lines;

        public DrawingDataRaw(ChemPointRaw[] Points, LineRaw[] Lines)
        {
            this.Points = Points;
            this.Lines = Lines;
        }

        public static implicit operator DrawingDataRaw(DrawingData data)
        {
            var dataRaw = new DrawingDataRaw()
            {
                Lines = new LineRaw[data.Lines.Length],
                Points = new ChemPointRaw[data.Points.Length],
            };

            for(int i = 0; i<dataRaw.Lines.Length;i++)
            {
                var current = data.Lines[i];

                dataRaw.Lines[i] = new LineRaw()
                {
                    IDChemPoint1 = current.IDChemPoint1,
                    IDChemPoint2 = current.IDChemPoint2,
                    NumberOfBind = current.NumberOfBind
                };
            }
                
            for (int i = 0; i < dataRaw.Points.Length; i++)
            {
                var current = data.Points[i];

                dataRaw.Points[i] = new ChemPointRaw()
                {
                    ID = current.ID,
                    Charge = current.Charge,
                    MolecularFormula = current.MolecularFormula,
                    Position = new Avalonia.Point(current.Position.X, current.Position.Y)
                };
            }
               
            return dataRaw;
        }

        public static implicit operator DrawingData(DrawingDataRaw dataRaw)
        {
            var rawPoints = dataRaw.Points;
            var rawLines = dataRaw.Lines;

            ChemPoint[] points = new ChemPoint[rawPoints.Length];
            Line[] lines = new Line[rawLines.Length];

            for (int i = 0; i < points.Length; i++)
                points[i] = new ChemPoint()
                {
                    ID = rawPoints[i].ID,
                    Charge = rawPoints[i].Charge,
                    MolecularFormula = rawPoints[i].MolecularFormula,
                    Position = new Analyzer.Models.Position(Convert.ToInt32(rawPoints[i].Position.X), Convert.ToInt32(rawPoints[i].Position.Y))
                };

            for (int i = 0; i < lines.Length; i++)
                lines[i] = new Line(rawLines[i].IDChemPoint1, rawLines[i].IDChemPoint2, rawLines[i].NumberOfBind);

            return new DrawingData(points, lines, CalculateCodonData(points));
        }

        static Data CalculateCodonData(ChemPoint[] points)
        {
            throw new Exception("do zaimplementowania");
            return new Data();
        }
    }
}
