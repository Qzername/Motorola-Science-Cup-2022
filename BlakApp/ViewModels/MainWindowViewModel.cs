using Analyzer;
using Analyzer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlakApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public Sequence TestSequence => new SequenceAnalyzer().CreateSequence("AAAUGAACGAAAAUCUGUUCGCUUCAUUCAUUGCCCCCACAAUCCUAGGCCUACCCUGA");
    }
}
