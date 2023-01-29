using Analyzer;
using Analyzer.Models;
using BlakApp.ViewModels.Pages;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BlakApp.ViewModels
{
    public class MainWindowViewModel : PageChanger
    {
        public MainWindowViewModel() : base()
        {
        }

        public void CreatePage(int id)
        {
            //you cant do it in other ways
            Page page = id switch
            {
                0 => new Page("Sequence Analyzer", new SequenceAnalyzerViewModel()),
                1 => new Page("Peptide Analyzer", new PeptideAnalyzerViewModel()),
                2 => new Page("Peptide Draw Tool", new PeptideDrawToolViewModel()),
                3 => new Page("test view", new TestViewModel()),
                _ => throw new Exception("bad id")
            };

            Pages.Add(page);
        }

        public void RemovePage(Page page)
        {
            Pages.Remove(page);
        }
    }
}
