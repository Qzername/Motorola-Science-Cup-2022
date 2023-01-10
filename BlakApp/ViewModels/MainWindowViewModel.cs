using Analyzer;
using Analyzer.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlakApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        Sequence _testSequence;
        public Sequence TestSequence
        {
            get => _testSequence;
            set => this.RaiseAndSetIfChanged(ref _testSequence, value);
        }

        string _proteinTest;
        public string ProteinTest
        {
            get => _proteinTest;
            set => this.RaiseAndSetIfChanged(ref _proteinTest, value);
        }

        public MainWindowViewModel()
        {
            TestSequence = SequenceAnalyzer.CreateSequence("AAAAUGACGAAAAUCUGUUGAUCGCUUCAUUCAUUGAUGCCCCCACAAUCCUAGGCCUACCCUGA");
        }

        public void Test()
        {
            ProteinTest = SequenceAnalyzer.CalculateCodonSequenceMass(TestSequence.CodonsShift1).ToString();
        }
    }
}
