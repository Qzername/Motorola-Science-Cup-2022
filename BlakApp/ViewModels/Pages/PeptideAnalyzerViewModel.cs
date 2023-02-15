using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Analyzer.Analyzers;
using Analyzer.Models;
using DynamicData.Kernel;
using Analyzer.Models.Codons;

namespace BlakApp.ViewModels.Pages
{
    internal class PeptideAnalyzerViewModel : ViewModelBase
    {
        ISeries[] _hydrophobicitySeries;
        public ISeries[] HydrophobicitySeries { get=>_hydrophobicitySeries; set => this.RaiseAndSetIfChanged(ref _hydrophobicitySeries, value); }
        
        ISeries[] _netChargeSeries;
        public ISeries[] NetChargeSeries { get=> _netChargeSeries; set => this.RaiseAndSetIfChanged(ref _netChargeSeries, value); }

        bool _isDataVisible;
        public bool IsDataVisible
        {
            get => _isDataVisible;
            set => this.RaiseAndSetIfChanged(ref _isDataVisible, value);
        }

        string _sequenceRaw;
        public string SequenceRaw
        {
            get=> _sequenceRaw;
            set => this.RaiseAndSetIfChanged(ref _sequenceRaw, value);
        }

        Codon[] sequence;

        Data _sequenceData;
        public Data SequenceData
        {
            get => _sequenceData;
            set => this.RaiseAndSetIfChanged(ref _sequenceData, value);
        }

        string _error;
        public string Error
        {
            get => _error;
            set => this.RaiseAndSetIfChanged(ref _error, value);
        }

        public void AnalyzeData()
        {
            IsDataVisible = false;
            Error = string.Empty;

            if (SequenceRaw == null || SequenceRaw.Length == 0)
            {
                Error = "Sequence is empty";
                return;
            }

            SequenceRaw = SequenceRaw.ToUpper();
            SequenceRaw = SequenceRaw.Replace("[STOP]", string.Empty);
            SequenceRaw = SequenceRaw.Replace("(START)", string.Empty);
            SequenceRaw = new string(SequenceRaw.Where(char.IsLetter).ToArray());

            try
            {
                sequence = CodonAnalyzer.CreateCodonsFromString(SequenceRaw);
                CalculateData();
            }
            catch (Exception)
            {
                Error = "Sequence contains illegal characters";
                return;
            }

            IsDataVisible = true;
        }

        void CalculateData()
        {
            string polarCodons = string.Empty, nonPolarCodons = string.Empty;
            List<double> hydroSeries = new List<double>();
            List<double> chargeSeries = new List<double>();

            foreach(var codon in sequence)
            {
                if (codon.DrawingData.Value.Data.IsPolar)
                    polarCodons += codon.Letter;
                else
                    nonPolarCodons += codon.Letter;

                hydroSeries.Add(Math.Round(codon.DrawingData.Value.Data.Hydrophobicity,2));
            }

            for (int i = 1; i < 15; i++)
                chargeSeries.Add(PeptideAnalyzer.CalculateCharge(sequence, i));

            this.HydrophobicitySeries = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = hydroSeries.ToArray(),
                }
            };

            NetChargeSeries = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = chargeSeries.ToArray(),
                    Fill=null,
                }
            };

            SequenceData = new Data()
            {
                Mass = Math.Round( PeptideAnalyzer.CalculateMass(sequence),2),
                IzoelectricPoint = Math.Round( PeptideAnalyzer.CalculateIzoelectricPoint(sequence),2),
                ExtinctionCoefficient = PeptideAnalyzer.CalculateExtinctionCoefficient(sequence),
                NetCharge = PeptideAnalyzer.CalculateCharge(sequence, 7),
                Hydrophobicity = Math.Round(sequence.Average(x => x.DrawingData.Value.Data.Hydrophobicity),2),
                PolarCodons = polarCodons,
                NonPolarCodons =nonPolarCodons,
            };
        }

        public struct Data
        {
            public double Mass { get; set; }
            public double IzoelectricPoint { get; set; }
            public double ExtinctionCoefficient { get; set; }
            public double NetCharge { get; set; }
            public double Hydrophobicity { get; set; }
            public string PolarCodons { get; set; }
            public string NonPolarCodons { get; set; }
        }
    }
}