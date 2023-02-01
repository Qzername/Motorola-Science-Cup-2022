using Analyzer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Analyzer.Analyzers;
using System.Diagnostics;

namespace BlakApp.ViewModels.Pages
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
            string code = SequenceAnalyzer.CreateCode(CodonAnalyzer.CreateCodonsFromString("ARNDCQEGHILKMFPSTWYV"));
            Debug.WriteLine(code);
            TestSequence =/**/ SequenceAnalyzer.CreateSequence(code);/**/
        }

        public void Test()
        {
            ProteinTest = PeptideAnalyzer.CalculateMass(TestSequence.CodonsShift1).ToString();
        }
    }
}
