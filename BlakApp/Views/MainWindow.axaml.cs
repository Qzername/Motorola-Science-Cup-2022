using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System.Diagnostics;

namespace BlakApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); 
            
            PropertyChanged += MainWindow_PropertyChanged;
        }

        private void MainWindow_PropertyChanged(object? sender, Avalonia.AvaloniaPropertyChangedEventArgs e)
        {
            //hacky approach to avalonia's bug
            //see: https://github.com/AvaloniaUI/Avalonia/issues/9042 for more info
            if (e.Property.PropertyType == typeof(WindowState))
            {
                if ((WindowState)e.NewValue == WindowState.Maximized)
                {
                    Padding = new Avalonia.Thickness(7);
                    ExtendClientAreaTitleBarHeightHint = 37;
                }
                else
                {
                    Padding = new Avalonia.Thickness(0);
                    ExtendClientAreaTitleBarHeightHint = 30;
                }
            }
        }

        public void AddNewPanelContextMenuClicked(object sender, RoutedEventArgs e)
        {
            Button image = sender as Button;
            ContextMenu contextMenu = image.ContextMenu;
            contextMenu.PlacementTarget = image;
            contextMenu.Open();
            e.Handled = true;
        }
   }
}
