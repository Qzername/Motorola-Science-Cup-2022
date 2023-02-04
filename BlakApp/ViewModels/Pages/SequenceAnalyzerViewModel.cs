using Analyzer.Analyzers;
using Analyzer.Models;
using Analyzer.Models.Codons;
using Avalonia.Controls;
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
        
        int? _typeOfProteinShowing;
        public int? TypeOfProteinShowing
        {
            get => _typeOfProteinShowing;
            set => this.RaiseAndSetIfChanged(ref _typeOfProteinShowing, value);
        }

        ObservableCollection<ShiftObject> _shifts;
        /// <summary>
        /// Array that contains every page, modify list if you want to add/modify/delete page
        /// </summary>
        public ObservableCollection<ShiftObject> Shifts { get => _shifts; set => this.RaiseAndSetIfChanged(ref _shifts, value); }


        public SequenceAnalyzerViewModel()
        {
            IsVerboseSelected = true;
            CheckForward = true;
            CheckBackwards = false;
            Error = string.Empty;

            Shifts = new ObservableCollection<ShiftObject>();
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

            if(TypeOfProteinShowing == null)
            {
                Error = "Choose type of protein visualization";
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
            catch(Exception)
            {
                Error = "Sequence contains illegal characters";
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

                //this is hacky way to have good wrapping
                char[] raw = new char[final.Length * 2];

                for(int i = 0; i<final.Length;i++)
                {
                    raw[i * 2] = final[i];
                    raw[i * 2 + 1] = ' ';
                }

                final = new string(raw);

                int lastOne = shift.Length*2;

                List<string> elements = new List<string>();

                for(int i = proteins.Length-1;i>-1;i--)
                {
                    var protein = proteins[i];

                    int endLength = (protein.StartPosition + protein.Codons.Length)*2;
                    int startLength = (protein.StartPosition)*2;

                    elements.Add(StyleCheck(final.Substring(endLength, lastOne - endLength)));
                    elements.Add(ProteinShowingCheck(StyleCheck(final.Substring(startLength, endLength - startLength))));

                    lastOne = startLength;
                }

                if(lastOne != 0)
                {
                    elements.Add(StyleCheck(final.Substring(0, lastOne)));
                }

                elements.Reverse();

                string text = string.Empty;

                foreach (string element in elements)
                    text += element;

                ShiftObject obj = new ShiftObject()
                {
                    Name = name,
                    Text = new TextDocument() { Text = text }
                };

                Shifts.Add(obj);
            }
        }

        string ProteinShowingCheck(string text) => TypeOfProteinShowing switch
        {
            0 => "\n\nProtein:\n\n" + text + "\n\nRest:\n\n",
            1 => "***PROTEIN START*** " +text + "***PROTEIN END*** ",
            _ => text,
        };

        string StyleCheck(string text)
        {
            if (IsVerboseSelected)
            {
                text = text.Replace("M", "M ( s t a r t )");
                text = text.Replace("-", "[ s t o p ]");
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
