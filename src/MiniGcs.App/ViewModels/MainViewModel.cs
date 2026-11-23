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

    // Private fields that actually store the values (like private fields in Java).
    private double latitude;
    private double longitude;
    private double altitude;
    private double groundSpeed;
    private double batteryPercent;
    private bool isArmed;
    private string flightMode = "";

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
    // Every setter calls OnPropertyChanged with the property's exact name, or the
    // screen won't update.

    public double Latitude
    {
        get
        {
            return latitude;
        }
        set
        {
            latitude = value;
            OnPropertyChanged("Latitude");
        }
    }

    public double Longitude
    {
        get
        {
            return longitude;
        }
        set
        {
            longitude = value;
            OnPropertyChanged("Longitude");
        }
    }

    public double Altitude
    {
        get
        {
            return altitude;
        }
        set
        {
            altitude = value;
            OnPropertyChanged("Altitude");
        }
    }

    public double GroundSpeed
    {
        get
        {
            return groundSpeed;
        }
        set
        {
            groundSpeed = value;
            OnPropertyChanged("GroundSpeed");
        }
    }

    public double BatteryPercent
    {
        get
        {
            return batteryPercent;
        }
        set
        {
            batteryPercent = value;
            OnPropertyChanged("BatteryPercent");
        }
    }

    public bool IsArmed
    {
        get
        {
            return isArmed;
        }
        set
        {
            isArmed = value;
            OnPropertyChanged("IsArmed");

            // ArmedText is calculated from IsArmed, so it changed too: notify it as well.
            OnPropertyChanged("ArmedText");
        }
    }

    // A read-only property (only `get`) that turns the bool into text for the screen.
    // Preparing data in the format the View needs is exactly the ViewModel's job.
    public string ArmedText
    {
        get
        {
            if (isArmed)
            {
                return "ARMED";
            }
            else
            {
                return "DISARMED";
            }
        }
    }

    public string FlightMode
    {
        get
        {
            return flightMode;
        }
        set
        {
            flightMode = value;
            OnPropertyChanged("FlightMode");
        }
    }

    // Called by the telemetry source with every new reading:
    // copy each value from the reading into the matching property.
    private void OnTelemetryReceived(object? sender, Telemetry telemetry)
    {
        Latitude = telemetry.Latitude;
        Longitude = telemetry.Longitude;
        Altitude = telemetry.Altitude;
        GroundSpeed = telemetry.GroundSpeed;
        BatteryPercent = telemetry.BatteryPercent;
        IsArmed = telemetry.IsArmed;
        FlightMode = telemetry.FlightMode;
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
