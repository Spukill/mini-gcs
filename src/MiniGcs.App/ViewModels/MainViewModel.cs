using System.ComponentModel;
using MiniGcs.App.Models;
using MiniGcs.App.Services;

namespace MiniGcs.App.ViewModels;

// The ViewModel for the main window: a plain C# class that holds the STATE the screen
// shows. It knows nothing about the window (no TextBlocks, no colors); the XAML reads
// its properties through data binding, e.g. {Binding Altitude}. Because it has no UI
// code, it can be tested without opening a window.
//
// It implements INotifyPropertyChanged (Observer pattern, like PropertyChangeListener
// in Java): when a property changes, the ViewModel raises the PropertyChanged event,
// and WPF (which subscribed to it when the window opened) re-reads that property and
// updates the screen. Without this, the View only ever sees the first value.
//
// It doesn't produce data itself: it receives an ITelemetrySource in the constructor
// and listens to it. It doesn't know (or care) whether the data is fake or real.
public class MainViewModel : INotifyPropertyChanged
{
    // The event that WPF subscribes to. The `?` means it can be null: it stays null
    // until someone subscribes.
    public event PropertyChangedEventHandler? PropertyChanged;

    private ITelemetrySource telemetrySource;

    // The private field that actually stores the value (like a private field in Java).
    private double altitude;

    // The data source is passed in from outside instead of being created here
    // ("dependency injection"): this class works with ANY ITelemetrySource.
    public MainViewModel(ITelemetrySource telemetrySource)
    {
        this.telemetrySource = telemetrySource;

        // "When a new reading arrives, call OnTelemetryReceived".
        this.telemetrySource.TelemetryReceived += OnTelemetryReceived;
    }

    // A C# "property": from outside it looks like a field (viewModel.Altitude = 10),
    // but reading it runs the `get` block and assigning it runs the `set` block, just
    // like calling getAltitude() / setAltitude(value) in Java. Inside `set`, the keyword
    // `value` is the new value being assigned. WPF bindings only work with properties.
    public double Altitude
    {
        get
        {
            return altitude;
        }
        set
        {
            altitude = value;

            // Tell whoever is listening (WPF) that "Altitude" changed.
            // The name must match the property name exactly, or nothing updates.
            OnPropertyChanged("Altitude");
        }
    }

    // Called by the telemetry source with every new reading.
    private void OnTelemetryReceived(object? sender, Telemetry telemetry)
    {
        Altitude = telemetry.Altitude;
    }

    // Raises the PropertyChanged event, if anyone is subscribed to it.
    private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
