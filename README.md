# BluetoothSwitch

A lightweight Windows 11 system tray utility for quickly toggling Bluetooth on and off.

## Features

- Toggle Bluetooth with a left click on the tray icon
- Right-click context menu
- Persistent tray icon when Bluetooth is turned off
- Visual status indicator:
  - Blue — Bluetooth is ON
  - Red — Bluetooth is OFF
  - Gray — status unavailable or unknown
- Status shown in the tray tooltip
- Uses the Windows Radio API
- No administrator privileges required for normal operation

## How it works

BluetoothSwitch uses the Windows Runtime `Windows.Devices.Radios.Radio` API to control the Bluetooth radio.

It does not disable or enable the Bluetooth device through Device Manager or Plug and Play.

## Requirements

- Windows 11
- .NET 10 SDK for building from source

## Build

```powershell
dotnet build
```

For a self-contained Windows x64 release:

```powershell
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

The published executable will be located in:

`bin\Release\net10.0-windows10.0.26100.0\win-x64\publish\`

## Autostart

BluetoothSwitch can be configured to start automatically when you sign in to Windows using Windows Task Scheduler.

## License

MIT
