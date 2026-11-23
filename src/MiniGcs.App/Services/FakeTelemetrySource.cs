using System.Windows.Threading;
using MiniGcs.App.Models;

namespace MiniGcs.App.Services;

// A fake drone, so I can build the screen before connecting to the real simulator.
// Every 200 ms (5 times per second) it creates a new Telemetry reading: the drone
// climbs to 100 m, then flies in a circle around a home point, with the battery slowly
// draining and a bit of random noise so it looks like real sensor data.
// It uses a DispatcherTimer, which runs on the UI thread, so there are no threads to
// worry about yet; the real MAVLink source (phase 3) will read on a background thread.
public class FakeTelemetrySource : ITelemetrySource
{
    // Home point: FEUP, Porto.
    private const double HomeLatitude = 41.1780;
    private const double HomeLongitude = -8.5980;

    private const double TargetAltitude = 100.0;  // meters
    private const double ClimbPerTick = 1.0;      // meters per tick (= 5 m/s)
    private const double CircleRadius = 0.001;    // degrees (about 100 m)
    private const double AnglePerTick = 0.01;     // radians the drone moves along the circle each tick

    private DispatcherTimer timer;
    private Random random;

    // Current state of the fake drone, updated on every tick.
    private double altitude = 0.0;
    private double battery = 100.0;
    private double angle = 0.0;

    public event EventHandler<Telemetry>? TelemetryReceived;

    public FakeTelemetrySource()
    {
        random = new Random();

        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(200);
        timer.Tick += OnTimerTick;
    }

    public void Start()
    {
        timer.Start();
    }

    public void Stop()
    {
        timer.Stop();
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        // 1. Altitude: the TRUE altitude climbs until the target, then stays there.
        //    The noise is only added to the MEASURED value sent in the telemetry (below),
        //    just like a real sensor: the drone is at 100 m, the barometer reads 99.7.
        if (altitude < TargetAltitude)
        {
            altitude = altitude + ClimbPerTick;
        }

        // 2. Position: move a little along a circle around home.
        angle = angle + AnglePerTick;
        double latitude = HomeLatitude + CircleRadius * Math.Cos(angle);
        double longitude = HomeLongitude + CircleRadius * Math.Sin(angle);

        // 3. Battery: drains slowly, never below 0.
        battery = battery - 0.02;
        if (battery < 0)
        {
            battery = 0;
        }

        // 4. Build the reading and send it to whoever subscribed.
        Telemetry telemetry = new Telemetry();
        telemetry.Latitude = latitude;
        telemetry.Longitude = longitude;
        telemetry.Altitude = altitude + RandomNoise(0.5);  // measured = true + noise
        telemetry.GroundSpeed = 8.0 + RandomNoise(0.3);
        telemetry.BatteryPercent = battery;
        telemetry.IsArmed = true;
        telemetry.FlightMode = "GUIDED";

        if (TelemetryReceived != null)
        {
            TelemetryReceived(this, telemetry);
        }
    }

    // Returns a random number between -amount and +amount.
    private double RandomNoise(double amount)
    {
        return (random.NextDouble() * 2.0 - 1.0) * amount;
    }
}
