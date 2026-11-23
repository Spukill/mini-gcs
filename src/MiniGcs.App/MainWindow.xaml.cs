using System.Windows;
using MiniGcs.App.Services;
using MiniGcs.App.ViewModels;

namespace MiniGcs.App;

// Code-behind for MainWindow.xaml (the other half of the same partial class).
// InitializeComponent() is generated from the XAML (see obj/.../MainWindow.g.cs):
// it reads the layout and actually creates the Grid, the TextBlock, etc. With MVVM
// this file should stay almost empty: the window's data and logic live in the
// ViewModel, and the XAML just binds to it. For now this is also where the pieces
// are created and connected (fake data source -> ViewModel -> window).
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Swap FakeTelemetrySource for the MAVLink one in phase 3; nothing else changes.
        ITelemetrySource telemetrySource = new FakeTelemetrySource();
        MainViewModel viewModel = new MainViewModel(telemetrySource);

        // DataContext = "the object this window's bindings read from".
        DataContext = viewModel;

        telemetrySource.Start();
    }
}
