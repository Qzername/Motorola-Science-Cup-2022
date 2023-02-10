using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace BlakApp.ViewModels.Pages
{
    internal class PeptideAnalyzerViewModel : ViewModelBase
    {
        public ISeries[] Series { get; set; }
            = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
                    Fill = null
                }
            };

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

        public void AnalyzeData()
        {
            IsDataVisible = true;
        }
    }
}