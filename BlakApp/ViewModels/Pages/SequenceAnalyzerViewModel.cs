using Analyzer.Analyzers;
using Analyzer.Models;
using Analyzer.Models.Codons;
using Avalonia.Controls;
using Avalonia.Media;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

            if(SequenceRaw?.Length == 0)
            {
                Error = "Sequence is empty";
                return;
            }

            for (int i = Shifts.Count - 1; i > -1; i--)
                Shifts.RemoveAt(i);

            SequenceRaw = new string(SequenceRaw.Where(char.IsLetter).ToArray());

            if (CheckForward)
                AnalyzeSequence("Forward", SequenceRaw);

            if(CheckBackwards)
            {
                var reverse = new string(SequenceRaw.Reverse().ToArray());
                AnalyzeSequence("Backwards", reverse);
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
                    raw[i * 2 + 1] = '­';
                }

                final = new string(raw);

                List<ProteinElement> elements = new List<ProteinElement>();

                int lastOne = shift.Length*2;

                string text = string.Empty;

                for(int i = proteins.Length-1;i>-1;i--)
                {
                    var protein = proteins[i];

                    int endLength = (protein.StartPosition + protein.Codons.Length)*2;
                    int startLength = (protein.StartPosition)*2;

                    text += StyleCheck(final.Substring(endLength, lastOne - endLength));
                    text += "%{color:red}" + StyleCheck(final.Substring(startLength, endLength - startLength)) + "%";


                    lastOne = startLength;
                }

                if(lastOne != 0)
                {
                    text += StyleCheck(final.Substring(0, lastOne));
                }

                elements.Reverse();

                ShiftObject obj = new ShiftObject()
                {
                    Name = name,
                    Text = text
                };

                Shifts.Add(obj);
            }
        }

        string StyleCheck(string text)
        {
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
            public string Text { get; set; }
        }

        public struct ProteinElement
        {
            public static readonly SolidColorBrush ProteinColor = new SolidColorBrush(Colors.Red);
            public static readonly SolidColorBrush NormalColor = new SolidColorBrush(Colors.White);

            public SolidColorBrush TextColor { get; set; }
            public string Text { get; set; }
        }
    }
}
