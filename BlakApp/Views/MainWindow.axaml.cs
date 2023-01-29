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
