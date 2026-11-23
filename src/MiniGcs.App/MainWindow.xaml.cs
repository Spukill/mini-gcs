using System.Windows;
using MiniGcs.App.ViewModels;

namespace MiniGcs.App;

// Code-behind for MainWindow.xaml (the other half of the same partial class).
// InitializeComponent() is generated from the XAML (see obj/.../MainWindow.g.cs):
// it reads the layout and actually creates the Grid, the TextBlock, etc. With MVVM
// this file should stay almost empty: the window's data and logic live in the
// ViewModel, and the XAML just binds to it.
public partial class MainWindow : Window
{
    private MainViewModel viewModel;

    public MainWindow()
    {
        InitializeComponent();

        viewModel = new MainViewModel();

        // DataContext = "the object this window's bindings read from".
        DataContext = viewModel;
    }

    // TEMPORARY: runs when the button is clicked (Click="OnShowValueClick" in the XAML).
    // Shows the value stored in the ViewModel right now, to compare with the screen.
    private void OnShowValueClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Altitude no ViewModel: " + viewModel.Altitude + " m");
    }
}
