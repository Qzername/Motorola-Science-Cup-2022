using Analyzer.Analyzers;
using Analyzer.Models;
using DynamicData.Kernel;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlakApp.ViewModels.Pages
{
    internal class PeptideDrawToolViewModel : ViewModelBase
    {
        string _sequenceRaw;
        public string SequenceRaw
        {
            get => _sequenceRaw;
            set => this.RaiseAndSetIfChanged(ref _sequenceRaw, value);
        }

        string _error;
        public string Error
        {
            get => _error;
            set => this.RaiseAndSetIfChanged(ref _error, value);
        }

        Sequence _drawSequence;
        public Sequence DrawSequence
        {
            get => _drawSequence;
            set => this.RaiseAndSetIfChanged(ref _drawSequence, value);
        }

        public void GenerateSequence()
        {
            Error = string.Empty;
            
            if (SequenceRaw == null || SequenceRaw.Length == 0)
            {
                Error = "Sequence is empty";
                return;
            }

            SequenceRaw = SequenceRaw.ToUpper();
            SequenceRaw = SequenceRaw.Replace("[STOP]",string.Empty);
            SequenceRaw = SequenceRaw.Replace("(START)",string.Empty);
            SequenceRaw = new string(SequenceRaw.Where(char.IsLetter).ToArray());
            
            try
            {
                DrawSequence = new Sequence()
                {
                    CodonsShift1 = CodonAnalyzer.CreateCodonsFromString(SequenceRaw)
                };
            }
            catch (Exception)
            {
                Error = "Sequence contains illegal characters";
            }
        }
    }
}
