# MiniGcs.Mavlink

C# MAVLink code **generated** by [`mavgen`](https://mavlink.io/en/getting_started/generate_libraries.html)
from ArduPilot's official dialect `ardupilotmega.xml` (which includes `common.xml`).
It's the same generator family used by Mission Planner, ArduPilot's C# GCS.

Do not edit the files in `Generated/` by hand: regenerate them instead.

## How to regenerate

In WSL, inside the ArduPilot checkout:

```bash
cd ~/ardupilot/modules/mavlink
python -m pymavlink.tools.mavgen --lang=CS --wire-protocol=2.0 --output=/tmp/mavlink-cs message_definitions/v1.0/ardupilotmega.xml
cp /tmp/mavlink-cs/*.cs /mnt/c/Dev/mini-gcs/src/MiniGcs.Mavlink/Generated/
```

## License

The generated code and the C# support files come from the MAVLink project and
[pymavlink](https://github.com/ArduPilot/pymavlink) (LGPLv3).
