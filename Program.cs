using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.Radios;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        Logger.Write("=== BluetoothSwitch started ===");

        Application.ThreadException += (s, e) =>
        {
            Logger.Write("UI THREAD EXCEPTION: " + e.Exception);
        };

        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        {
            Logger.Write("UNHANDLED EXCEPTION: " + e.ExceptionObject);
        };

        TaskScheduler.UnobservedTaskException += (s, e) =>
        {
            Logger.Write("UNOBSERVED TASK EXCEPTION: " + e.Exception);
            e.SetObserved();
        };

        ApplicationConfiguration.Initialize();
        Application.Run(new BluetoothTrayContext());

        Logger.Write("=== BluetoothSwitch stopped ===");
    }
}

internal static class Logger
{
    private static readonly string LogDirectory =
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "BluetoothSwitch");

    private static readonly string LogFile =
        Path.Combine(LogDirectory, "BluetoothSwitch.log");

    public static void Write(string message)
    {
        try
        {
            Directory.CreateDirectory(LogDirectory);

            File.AppendAllText(
                LogFile,
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} | {message}{Environment.NewLine}");
        }
        catch
        {
            // Logging must never crash the application.
        }
    }
}

internal class BluetoothTrayContext : ApplicationContext
{
    private readonly NotifyIcon trayIcon;
    private readonly ContextMenuStrip menu;
    private bool busy;

    public BluetoothTrayContext()
    {
        Logger.Write("Creating tray context.");

        menu = new ContextMenuStrip();

        var toggleItem = new ToolStripMenuItem(
            "Включить / выключить Bluetooth");

        toggleItem.Click += async (s, e) => await ToggleBluetooth();

        var settingsItem = new ToolStripMenuItem(
            "Параметры Bluetooth");

        settingsItem.Click += (s, e) => OpenBluetoothSettings();

        var exitItem = new ToolStripMenuItem("Выход");
        exitItem.Click += (s, e) => ExitApplication();

        menu.Items.Add(toggleItem);
        menu.Items.Add(settingsItem);
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add(exitItem);

        trayIcon = new NotifyIcon
        {
            Icon = CreateBluetoothIcon(Color.Gray),
            Text = "Bluetooth",
            ContextMenuStrip = menu,
            Visible = true
        };

        trayIcon.MouseClick += async (s, e) =>
        {
            if (e.Button == MouseButtons.Left)
                await ToggleBluetooth();
        };

        Logger.Write("Tray icon created.");

        _ = UpdateTrayStatus();
    }

    private async Task ToggleBluetooth()
    {
        if (busy)
        {
            Logger.Write("Toggle ignored: operation already in progress.");
            return;
        }

        busy = true;
        Logger.Write("Toggle started.");

        try
        {
            var radios = await Radio.GetRadiosAsync();

            Logger.Write($"Radios found: {radios.Count}");

            var bluetooth = radios.FirstOrDefault(
                r => r.Kind == RadioKind.Bluetooth);

            if (bluetooth == null)
            {
                Logger.Write("Bluetooth radio not found.");

                trayIcon.Icon = CreateBluetoothIcon(Color.Gray);
                trayIcon.Text = "Bluetooth: не найден";

                MessageBox.Show(
                    "Bluetooth-радио не найдено.",
                    "Bluetooth Switch",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            Logger.Write($"Bluetooth current state: {bluetooth.State}");

            var newState = bluetooth.State == RadioState.On
                ? RadioState.Off
                : RadioState.On;

            Logger.Write($"Requesting Bluetooth state: {newState}");

            var result = await bluetooth.SetStateAsync(newState);

            Logger.Write($"SetStateAsync result: {result}");

            if (result != RadioAccessStatus.Allowed)
            {
                MessageBox.Show(
                    "Windows не разрешила изменить состояние Bluetooth.\n\n" +
                    "Статус: " + result,
                    "Bluetooth Switch",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            await UpdateTrayStatus();

            Logger.Write("Toggle finished.");
        }
        catch (Exception ex)
        {
            Logger.Write("Toggle exception: " + ex);

            trayIcon.Icon = CreateBluetoothIcon(Color.Gray);
            trayIcon.Text = "Bluetooth: ошибка";

            MessageBox.Show(
                "Ошибка управления Bluetooth:\n\n" + ex.Message,
                "Bluetooth Switch",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            busy = false;
        }
    }

    private void OpenBluetoothSettings()
    {
        Logger.Write("Opening Bluetooth settings.");

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "ms-settings:bluetooth",
                UseShellExecute = true
            });

            Logger.Write("Bluetooth settings opened.");
        }
        catch (Exception ex)
        {
            Logger.Write("Opening Bluetooth settings failed: " + ex);

            MessageBox.Show(
                "Не удалось открыть параметры Bluetooth:\n\n" +
                ex.Message,
                "Bluetooth Switch",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private async Task UpdateTrayStatus()
    {
        try
        {
            Logger.Write("Updating tray status.");

            var radios = await Radio.GetRadiosAsync();

            var bluetooth = radios.FirstOrDefault(
                r => r.Kind == RadioKind.Bluetooth);

            if (bluetooth == null)
            {
                Logger.Write("UpdateTrayStatus: Bluetooth not found.");

                trayIcon.Icon = CreateBluetoothIcon(Color.Gray);
                trayIcon.Text = "Bluetooth: не найден";
                return;
            }

            Logger.Write(
                $"UpdateTrayStatus: Bluetooth state = {bluetooth.State}");

            if (bluetooth.State == RadioState.On)
            {
                trayIcon.Icon = CreateBluetoothIcon(
                    Color.FromArgb(0, 120, 215));

                trayIcon.Text = "Bluetooth: ВКЛ";
            }
            else if (bluetooth.State == RadioState.Off)
            {
                trayIcon.Icon = CreateBluetoothIcon(Color.Red);
                trayIcon.Text = "Bluetooth: ВЫКЛ";
            }
            else
            {
                trayIcon.Icon = CreateBluetoothIcon(Color.Gray);
                trayIcon.Text = "Bluetooth: неизвестно";
            }
        }
        catch (Exception ex)
        {
            Logger.Write("UpdateTrayStatus exception: " + ex);

            trayIcon.Icon = CreateBluetoothIcon(Color.Gray);
            trayIcon.Text = "Bluetooth: ошибка";
        }
    }

    private Icon CreateBluetoothIcon(Color backgroundColor)
    {
        const int size = 64;

        using var bitmap = new Bitmap(size, size);

        using (var graphics = Graphics.FromImage(bitmap))
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Clear(Color.Transparent);

            using (var brush = new SolidBrush(backgroundColor))
            {
                graphics.FillEllipse(
                    brush,
                    2,
                    2,
                    60,
                    60);
            }

            using (var pen = new Pen(Color.White, 6))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                pen.LineJoin = LineJoin.Round;

                graphics.DrawLine(pen, 32, 10, 32, 54);
                graphics.DrawLine(pen, 32, 10, 48, 22);
                graphics.DrawLine(pen, 48, 22, 32, 32);
                graphics.DrawLine(pen, 32, 22, 48, 42);
                graphics.DrawLine(pen, 48, 42, 32, 54);
                graphics.DrawLine(pen, 32, 32, 15, 16);
                graphics.DrawLine(pen, 32, 32, 15, 48);
            }
        }

        IntPtr handle = bitmap.GetHicon();

        try
        {
            using var temporaryIcon = Icon.FromHandle(handle);
            return (Icon)temporaryIcon.Clone();
        }
        finally
        {
            DestroyIcon(handle);
        }
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool DestroyIcon(IntPtr hIcon);

    private void ExitApplication()
    {
        Logger.Write("Exit requested.");

        trayIcon.Visible = false;
        trayIcon.Dispose();
        menu.Dispose();

        Application.Exit();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Logger.Write("Disposing tray context.");

            trayIcon.Dispose();
            menu.Dispose();
        }

        base.Dispose(disposing);
    }
}