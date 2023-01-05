using Analyzer;
using Analyzer.Models.Codons;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.ReactiveUI;
using CodonEditor.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodonEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        ObservableCollection<CodonRaw> Codons { get; set; }

        CodonRaw _currentItem;
        public CodonRaw CurrentItem
        {
            get => _currentItem;
            set => this.RaiseAndSetIfChanged(ref _currentItem, value);
        }

        public MainWindowViewModel()
        {
            Codons = new ObservableCollection<CodonRaw>();
        }

        public void ReadDatabase()
        {
            while (Codons.Count > 0)
                Codons.RemoveAt(0);

            foreach (Codon codon in CodonDatabase.Codons)
                Codons.Add(new CodonRaw()
                    {
                        Letter = codon.Letter,
                        Name = codon.Name,
                        IDs = codon.IDs,
                        CodonType = codon.CodonType
                    });
        }
    }
}
