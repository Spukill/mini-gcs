namespace MiniGcs.App.ViewModels;

// The ViewModel for the main window: a plain C# class that holds the STATE the screen
// shows. It knows nothing about the window (no TextBlocks, no colors); the XAML reads
// its properties through data binding, e.g. {Binding Altitude}. Because it has no UI
// code, it can be tested without opening a window. For now it has a single fixed value.
public class MainViewModel
{
    // The private field that actually stores the value (like a private field in Java).
    private double altitude = 120.5;

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
        }
    }
}
