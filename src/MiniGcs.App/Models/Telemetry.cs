namespace MiniGcs.App.Models;

// One telemetry reading: a snapshot of the drone's state at one moment, like one
// received frame/packet. It's just data, with no logic and no UI (think of a struct
// in C). Plain public fields are enough here because nothing binds to this class
// directly: the ViewModel copies these values into its own properties for the screen.
public class Telemetry
{
    public double Latitude;        // degrees
    public double Longitude;       // degrees
    public double Altitude;        // meters, relative to the takeoff point
    public double GroundSpeed;     // meters per second
    public double BatteryPercent;  // 0 to 100
    public bool IsArmed;           // armed = motors can spin
    public string FlightMode = ""; // e.g. "GUIDED", "AUTO", "RTL"
}
