using Analyzer.Models;
using Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace BlakApp.ViewModels
{
    internal class TestViewModel : ViewModelBase
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

        public TestViewModel()
        {
            TestSequence = SequenceAnalyzer.CreateSequence("AAAAUGACGAAAAUCUGUUGAUCGCUUCAUUCAUUGAUGCCCCCACAAUCCUAGGCCUACCCUGA");
        }

        public void Test()
        {
            ProteinTest = SequenceAnalyzer.CalculateCodonSequenceMass(TestSequence.CodonsShift1).ToString();
        }
    }
}
