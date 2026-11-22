# mini-gcs

A minimal **Ground Control Station (GCS)** for drones, written in C# / .NET (WPF, MVVM).

It connects to a simulated drone (ArduPilot SITL) over **MAVLink/UDP** and shows real-time telemetry:
position, altitude, speed, battery and flight state.

> Learning project: built step by step to learn the stack used in professional GCS software
> (C#/.NET, WPF/MVVM, async and multi-threading, real-time telemetry, logging, CI/CD).

## Tech stack

| Area | Choice |
|---|---|
| Runtime | .NET 10 (LTS), pinned via `global.json` |
| UI | WPF + MVVM (data binding) |
| Protocol | MAVLink over UDP |
| Simulator | ArduPilot SITL |
| CI | GitHub Actions (build + tests on every push) |

## Roadmap

- [x] **Phase 0 – Setup:** .NET SDK, WPF project, solution layout, repository
- [ ] **Phase 1 – UI with fake data:** fake telemetry generator, MVVM screen with data binding
- [ ] **Phase 2 – Simulator:** ArduPilot SITL running, first MAVLink `HEARTBEAT` received over UDP
- [ ] **Phase 3 – Real telemetry:** background reading (async/threads) without blocking the UI
- [ ] **Phase 4 – Quality:** structured logging, unit tests, CI with GitHub Actions
- [ ] **Phase 5 – Extras:** map with drone position, simple commands (arm, takeoff, RTL)

## Project structure

```
mini-gcs/
├── MiniGcs.slnx          # solution (groups all projects)
├── global.json           # pins the .NET SDK version
└── src/
    └── MiniGcs.App/      # WPF desktop app
```

## Build and run

Requirements: Windows, [.NET 10 SDK](https://dotnet.microsoft.com/download).

```powershell
dotnet build
dotnet run --project src/MiniGcs.App
```
