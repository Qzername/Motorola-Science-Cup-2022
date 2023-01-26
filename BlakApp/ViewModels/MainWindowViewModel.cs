using Analyzer;
using Analyzer.Models;
using BlakApp.ViewModels.Pages;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlakApp.ViewModels
{
    public class MainWindowViewModel : PageChanger
    {
        public MainWindowViewModel() : base()
        {
        }

        public void AddTestPanel()
        {
            Pages.Add(new Page("1", new TestViewModel()));
        }

        public void AddPeptidePanel()
        {
            Pages.Add(new Page("2", new AnalizePeptideViewModel()));
        }
    }
}
