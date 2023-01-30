using Analyzer.Models.Draw;
using Analyzer.Models.Terminuses;
using Analyzer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analyzer.Models.Drawing;
using Analyzer.Models.Codons;
using System.Diagnostics;

namespace Analyzer.Analyzers
{
    public static class DrawingHelper
    {
        /// <summary>
        /// Connect two terminuses to create one connected that can be used later on to connect to codon
        /// </summary>
        public static FullTerminus ConnectTerminuses(Terminus A, Terminus B)
        {
            int IDoffset = A.DrawingData.Points.Max(x => x.ID) + 1;
            Position PositionOffset = A.DrawingData.Points[A.ExitPoint].Position - B.DrawingData.Points[B.ConnectionPoint].Position;

            List<ChemPoint> points = new List<ChemPoint>();
            List<Line> lines = new List<Line>();

            points.AddRange(A.DrawingData.Points);
            lines.AddRange(A.DrawingData.Lines);

            foreach (var point in B.DrawingData.Points)
            {
                if (point.ID == B.ConnectionPoint)
                    continue;

                var copy = point;
                copy.ID += IDoffset;
                copy.Position += PositionOffset;
                points.Add(copy);
            }

            foreach (var line in B.DrawingData.Lines)
            {
                var copy = line;

                if (copy.IDChemPoint1 == B.ConnectionPoint)
                    copy.IDChemPoint1 = A.ExitPoint;
                else
                    copy.IDChemPoint1 += IDoffset;

                if (copy.IDChemPoint2 == B.ConnectionPoint)
                    copy.IDChemPoint2 = A.ExitPoint;
                else
                    copy.IDChemPoint2 += IDoffset;

                lines.Add(copy);
            }

            FullTerminus final = new FullTerminus()
            {
                ConnectionPoint = A.ConnectionPoint,
                ExitPoint = B.ExitPoint + IDoffset,
                CodonConnectionPoint = A.ExitPoint,
                DrawingData = new DrawingData(points.ToArray(), lines.ToArray(), new Data()),
            };

            return final;
        }

        /// <summary>
        /// Connects codon's drawingdata with terminus's drawingdata
        /// </summary>
        public static DrawingData ConnectCodonWithTerminus(Codon codon, FullTerminus fullTerminus)
        {
            var codonDrawingData = !codon.DrawingData.HasValue ? new DrawingData() : codon.DrawingData.Value;

            List<ChemPoint> points = new List<ChemPoint>();
            List<Line> lines = new List<Line>();

            points.AddRange(fullTerminus.DrawingData.Points);
            lines.AddRange(fullTerminus.DrawingData.Lines);

            if (codon.CodonType == CodonType.End)
                return new DrawingData();

            if (!codon.DrawingData.HasValue || codon.Letter == "G")
                return new DrawingData(points.ToArray(), lines.ToArray(), new Data());

            int IDoffset = fullTerminus.DrawingData.Points.Max(x => x.ID) + 1;
            Position PositionOffset = fullTerminus.DrawingData.Points[fullTerminus.CodonConnectionPoint].Position - codonDrawingData.Points[0].Position;

            foreach (var point in codonDrawingData.Points.Skip(1))
            {
                var copy = point;
                copy.ID += IDoffset;
                copy.Position += PositionOffset;
                points.Add(copy);
            }

            foreach(var line in codonDrawingData.Lines)
            {
                var copy = line; 
                copy.IDChemPoint1 += IDoffset;
                copy.IDChemPoint2 += IDoffset;

                lines.Add(copy);
            }

            //Every codon's drawingdata begins from the same line 
            //and we need to delete one starting point from codon's drawingdata
            var lineCopy = lines[fullTerminus.DrawingData.Lines.Length];
            lineCopy.IDChemPoint1 = fullTerminus.CodonConnectionPoint;
            lines[fullTerminus.DrawingData.Lines.Length] = lineCopy;

            //proline connects with start so i need to change that too
            //that point is the last one so i will be using this fact too
            if(codon.Letter == "P")
            {
                lineCopy = lines[lines.Count-1];
                lineCopy.IDChemPoint2 = fullTerminus.ConnectionPoint;
                lines[lines.Count - 1] = lineCopy;

                points.RemoveAt(points.Count - 1);
            }

            return new DrawingData(points.ToArray(), lines.ToArray(), new Data());
        }
    }
}
