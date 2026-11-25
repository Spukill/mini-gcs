using System.Net;
using System.Net.Sockets;
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

    // TEMPORARY (phase 3, step 1): a BAD example on purpose.
    // This runs on the UI thread (every click handler does). For 10 seconds it sits in a
    // loop calling Receive(), which blocks until a packet arrives. While this loop runs,
    // the UI thread can't do anything else: no repainting, no clicks, no timer ticks,
    // so the window freezes and the fake telemetry stops changing.
    private void OnBlockingListenClick(object sender, RoutedEventArgs e)
    {
        UdpClient udpClient = new UdpClient(14550);

        // If nothing arrives in 2 s (SITL not running), Receive() throws instead of
        // blocking forever.
        udpClient.Client.ReceiveTimeout = 2000;

        IPEndPoint senderEndPoint = new IPEndPoint(IPAddress.Any, 0);
        int packetCount = 0;
        DateTime endTime = DateTime.Now.AddSeconds(10);

        try
        {
            while (DateTime.Now < endTime)
            {
                udpClient.Receive(ref senderEndPoint);
                packetCount = packetCount + 1;
            }
        }
        catch (SocketException)
        {
            // Timeout: no packets arriving. Stop the loop.
        }

        udpClient.Close();

        MessageBox.Show("Received " + packetCount + " UDP packets in 10 seconds.");
    }
}
