using MiniGcs.App.Models;

namespace MiniGcs.App.Services;

// "Something that produces telemetry", without saying from where. Right now the only
// implementation is FakeTelemetrySource; in phase 3 a MAVLink one will read from the
// real (simulated) drone. The ViewModel only knows this interface, so swapping the
// data source doesn't change the ViewModel at all (same idea as coding against List
// instead of ArrayList in Java). It also lets tests feed the ViewModel fake data.
public interface ITelemetrySource
{
    // Raised every time a new reading arrives. Whoever wants the data subscribes with +=.
    event EventHandler<Telemetry>? TelemetryReceived;

    void Start();

    void Stop();
}
