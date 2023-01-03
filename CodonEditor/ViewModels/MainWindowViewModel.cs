using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodonEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public void Clicked(object sender, PointerReleasedEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("sfsdfdg");
        }
    }
}
