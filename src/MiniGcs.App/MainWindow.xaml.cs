using System.Windows;

namespace MiniGcs.App;

// Code-behind for MainWindow.xaml (the other half of the same partial class).
// InitializeComponent() is generated from the XAML (see obj/.../MainWindow.g.cs):
// it reads the layout and actually creates the Grid, the TextBlock, etc. With MVVM
// (phase 1) this file should stay almost empty: the window's data and logic will live
// in a separate ViewModel class, and the XAML will just bind to it.
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
}
