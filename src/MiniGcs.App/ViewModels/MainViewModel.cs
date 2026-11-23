using System.Windows.Threading;

namespace MiniGcs.App.ViewModels;

// The ViewModel for the main window: a plain C# class that holds the STATE the screen
// shows. It knows nothing about the window (no TextBlocks, no colors); the XAML reads
// its properties through data binding, e.g. {Binding Altitude}. Because it has no UI
// code, it can be tested without opening a window.
public class MainViewModel
{
    // The private field that actually stores the value (like a private field in Java).
    private double altitude = 120.5;

    // TEMPORARY: a timer that changes the altitude every second, to test the binding.
    // It will be replaced by a fake telemetry generator in step 4. DispatcherTimer
    // runs its Tick on the UI thread, so we don't have to deal with threads yet.
    private DispatcherTimer timer;

    public MainViewModel()
    {
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);

        // "When the timer ticks, call OnTimerTick" (like addActionListener in Java).
        timer.Tick += OnTimerTick;

        timer.Start();
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
        }
    }

    // Called by the timer every second. `sender` is the object that raised the event
    // (the timer) and `e` carries extra event data; we don't need either here.
    private void OnTimerTick(object? sender, EventArgs e)
    {
        Altitude = Altitude + 1;
    }
}
