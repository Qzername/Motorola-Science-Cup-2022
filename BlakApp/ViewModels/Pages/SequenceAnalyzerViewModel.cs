using Analyzer.Analyzers;
using Analyzer.Models;
using Analyzer.Models.Codons;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using AvaloniaEdit;
using AvaloniaEdit.Document;
using AvaloniaEdit.Highlighting;
using AvaloniaEdit.Highlighting.Xshd;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace BlakApp.ViewModels.Pages
{
    internal class SequenceAnalyzerViewModel : ViewModelBase
    {
        bool _isVerboseSelected;
        public bool IsVerboseSelected
        {
            get => _isVerboseSelected;
            set => this.RaiseAndSetIfChanged(ref _isVerboseSelected, value);
        }
        
        bool _checkForward;
        public bool CheckForward
        {
            get => _checkForward;
            set => this.RaiseAndSetIfChanged(ref _checkForward, value);
        }
        
        bool _checkBackwards;
        public bool CheckBackwards
        {
            get => _checkBackwards;
            set => this.RaiseAndSetIfChanged(ref _checkBackwards, value);
        }
        
        bool _addSpaces;
        public bool AddSpaces
        {
            get => _addSpaces;
            set => this.RaiseAndSetIfChanged(ref _addSpaces, value);
        }

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

        ObservableCollection<ShiftObject> _shifts;
        /// <summary>
        /// Array that contains every page, modify list if you want to add/modify/delete page
        /// </summary>
        public ObservableCollection<ShiftObject> Shifts { get => _shifts; set => this.RaiseAndSetIfChanged(ref _shifts, value); }

        IHighlightingDefinition _syntax;
        public IHighlightingDefinition Syntax
        {
            get=> _syntax;
            set=> this.RaiseAndSetIfChanged(ref _syntax, value);
        }

        public SequenceAnalyzerViewModel()
        {
            IsVerboseSelected = true;
            CheckForward = true;
            CheckBackwards = false;
            Error = string.Empty;

            Shifts = new ObservableCollection<ShiftObject>();

            LoadSyntax();
        }

        void LoadSyntax()
        {
            IHighlightingDefinition xshd;
            using (XmlTextReader reader = new XmlTextReader("Highlighting/Codon.xshd"))
            {
                xshd = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }

            Syntax = xshd;
        }

        public void Reset()
        {
            SequenceRaw = string.Empty;
            Error = string.Empty;

            for (int i = Shifts.Count - 1; i > -1; i--)
                Shifts.RemoveAt(i);
        }

        public void Translate()
        {
            Error = string.Empty;

            if (!CheckForward && !CheckBackwards)
            {
                Error = "Select at least one checking method";
                return;
            }

            if(SequenceRaw == null || SequenceRaw.Length == 0)
            {
                Error = "Sequence is empty";
                return;
            }

            for (int i = Shifts.Count - 1; i > -1; i--)
                Shifts.RemoveAt(i);

            SequenceRaw = new string(SequenceRaw.Where(char.IsLetter).ToArray());

            try
            {
                if (CheckForward)
                    AnalyzeSequence("Forward", SequenceRaw);

                if (CheckBackwards)
                {
                    var reverse = new string(SequenceRaw.Reverse().ToArray());
                    AnalyzeSequence("Backwards", reverse);
                }
            }
            catch(Exception ex)
            {
                Error = "Sequence contains illegal characters" + ex.Message;
            }
        }

        public async void OpenFromFile()
        {
            try
            {
                var dialog = new OpenFileDialog();

                dialog.Filters = new List<FileDialogFilter>()
                {
                    new FileDialogFilter()
                    {
                        Name= "Text files",
                        Extensions = new List<string>()
                        {
                            "txt",
                            "json",
                            "bat",
                        }
                    }
                };

                dialog.AllowMultiple = false;
            
                if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    string[]? result = await dialog.ShowAsync(desktop.MainWindow);

                    if (result is null)
                        return;

                    var path = result.First();

                    SequenceRaw = File.ReadAllText(path);
                }
            }
            catch (Exception)
            {
                Error = "Select valid file";
            }
        }

        void AnalyzeSequence(string name, string sequenceRaw)
        {
            var sequence = SequenceAnalyzer.CreateSequence(sequenceRaw);

            AddObject(name + " 1", sequence.CodonsShift1);
            AddObject(name + " 2", sequence.CodonsShift2);
            AddObject(name + " 3", sequence.CodonsShift3);
        }

        void AddObject(string name, Codon[] shift)
        {
            if (shift?.Length > 0)
            {
                var proteins = SequenceAnalyzer.DetectProteins(shift);
                var final = Sequence.CodonsToString(shift, false);

                ShiftObject obj = new ShiftObject()
                {
                    Name = name,
                    Text = new TextDocument() { Text = StyleCheck(final) }
                };

                Shifts.Add(obj);
            }
        }

        string StyleCheck(string text)
        {
            if(AddSpaces)
            {
                char[] raw = new char[text.Length * 2];

                for (int i = 0; i < text.Length; i++)
                {
                    raw[i * 2] = text[i];
                    raw[i * 2 + 1] = ' ';
                }

                text = new string(raw);
            }

            if (IsVerboseSelected)
            {
                text = text.Replace("M", "M(start)");
                text = text.Replace("-", "[stop]");
            }

            return text;
        }

        public struct ShiftObject
        {
            public string Name { get; set; }
            public TextDocument Text { get; set; }
        }
    }
}
