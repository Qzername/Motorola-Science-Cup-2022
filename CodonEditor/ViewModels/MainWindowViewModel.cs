using Analyzer;
using Analyzer.Models.Codons;
using Analyzer.Models.Drawing;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using CodonEditor.Controls;
using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodonEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        ObservableCollection<Codon> Codons { get; set; }

        Codon _selectedItem;
        public Codon SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        string _data;
        public string Data
        {
            get => _data;
            set => this.RaiseAndSetIfChanged(ref _data, value);
        }

        string _cValue;
        public string CValue
        {
            get => _cValue;
            set => this.RaiseAndSetIfChanged(ref _cValue, value);
        }

        string _nValue;
        public string NValue
        {
            get => _nValue;
            set => this.RaiseAndSetIfChanged(ref _nValue, value);
        }

        string _restValue;
        public string RestValue
        {
            get => _restValue;
            set => this.RaiseAndSetIfChanged(ref _restValue, value);
        }

        int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set { this.RaiseAndSetIfChanged(ref _selectedIndex, value); CodonChanged();}
        }

        public MainWindowViewModel()
        {
            Codons = new ObservableCollection<Codon>();
        }

        public void ReadDatabase()
        {
            while (Codons.Count > 0)
                Codons.RemoveAt(0);

            foreach (Codon codon in DatabaseReader.Codons)
                Codons.Add(new Codon()
                {
                    Letter = codon.Letter,
                    Name = codon.Name,
                    IDs = codon.IDs,
                    CodonType = codon.CodonType,
                    DrawingData = codon.DrawingData
                });

            DrawingManager.Current.CleanDrawing();
        }

        public void AddData() => DrawingManager.Current.SetAdditionalData(Data);
        public void RevertChange() => DrawingManager.Current.RevertChange();
        public void Clean() => DrawingManager.Current.CleanDrawing();

        public void Save()
        {
            SaveCurrent();
            List<Codon> Data = new List<Codon>();

            foreach (var raw in Codons)
            {
                if (raw.Letter == "F")
                    Debug.WriteLine(raw.DrawingData.Value.Data.CValue + " "+ Codons);

                if(raw.DrawingData == null)
                {
                    Data.Add(new Codon(raw.Letter, raw.Name, raw.IDs, raw.CodonType));
                    continue;
                }

                if (raw.Letter == "G")
                {
                    var copy = raw.DrawingData.Value.Data;
                    copy.Formula = "CH4";
                    Data.Add(new Codon(raw.Letter, raw.Name, raw.IDs, raw.CodonType, new Analyzer.Models.Draw.DrawingData(null, null, copy)));
                }
                else if(raw.Letter == "P")
                {
                    var copy = raw.DrawingData.Value.Data;
                    copy.Formula = "C-1H-4";
                    Data.Add(new Codon(raw.Letter, raw.Name, raw.IDs, raw.CodonType, new Analyzer.Models.Draw.DrawingData(null, null, copy)));

                }
                else
                   Data.Add(new Codon(raw.Letter, raw.Name, raw.IDs, raw.CodonType, raw.DrawingData.Value));
            }

            string json = JsonConvert.SerializeObject(Data.ToArray());

            Directory.CreateDirectory("./CodonDatabase/");
            File.WriteAllText("./CodonDatabase/database.json", json);
        }

        void CodonChanged()
        {
            //SelectedItem happens to change before CurrentItem to im using that to save the old drawing to old codon and read new drawing from new codon
            //saving old codon
            SaveCurrent();

            Debug.WriteLine("CC: " + Codons.Single(x=>x.Letter == "F").DrawingData.Value.Data.CValue);

            //reading new codon
            if (SelectedIndex != -1 && Codons[SelectedIndex].DrawingData is not null)
                DrawingManager.Current.SetRecalculatedDrawing(Codons[SelectedIndex].DrawingData.Value);
            else
                DrawingManager.Current.CleanDrawing();

            if( Codons[SelectedIndex].DrawingData.HasValue)
            {
                var data = Codons[SelectedIndex].DrawingData.Value.Data;
                CValue = data.CValue.ToString();
                NValue = data.NValue.ToString();
                RestValue = data.RestValue.ToString();
            }
            else
            {
                CValue = "";
                NValue = "";
                RestValue = "";
            }
        }

        void SaveCurrent()
        {
            int index = Codons.IndexOf(SelectedItem);

            if (index != -1)
            {
                var copy = Codons[index];
                copy.DrawingData = DrawingManager.Current.GetRecalculatedDrawing();

                if (copy.DrawingData is null)
                    copy.DrawingData = new Analyzer.Models.Draw.DrawingData();

                var drawingData = copy.DrawingData.Value;

                var data = drawingData.Data;

                data.CValue = float.Parse(CValue.Replace('.', ','));
                data.NValue = float.Parse(NValue.Replace('.', ','));
                data.RestValue = float.Parse(RestValue.Replace('.', ','));

                drawingData.Data = data;

                copy.DrawingData = drawingData;

                Codons[index] = copy;
            }
        }
    }
}
