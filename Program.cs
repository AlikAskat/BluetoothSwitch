using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.Radios;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new BluetoothTrayContext());
    }
}

internal class BluetoothTrayContext : ApplicationContext
{
    private readonly NotifyIcon trayIcon;
    private readonly ContextMenuStrip menu;
    private bool busy;

    public BluetoothTrayContext()
    {
        menu = new ContextMenuStrip();

        var toggleItem = new ToolStripMenuItem("Включить / выключить Bluetooth");
        toggleItem.Click += async (s, e) => await ToggleBluetooth();

        var exitItem = new ToolStripMenuItem("Выход");
        exitItem.Click += (s, e) => ExitApplication();

        menu.Items.Add(toggleItem);
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

        _ = UpdateTrayStatus();
    }

    private async Task ToggleBluetooth()
    {
        if (busy)
            return;

        busy = true;

        try
        {
            var radios = await Radio.GetRadiosAsync();

            var bluetooth = radios.FirstOrDefault(
                r => r.Kind == RadioKind.Bluetooth);

            if (bluetooth == null)
            {
                trayIcon.Icon = CreateBluetoothIcon(Color.Gray);
                trayIcon.Text = "Bluetooth: не найден";

                MessageBox.Show(
                    "Bluetooth-радио не найдено.",
                    "Bluetooth Switch",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            var newState = bluetooth.State == RadioState.On
                ? RadioState.Off
                : RadioState.On;

            var result = await bluetooth.SetStateAsync(newState);

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
        }
        catch (Exception ex)
        {
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

    private async Task UpdateTrayStatus()
    {
        try
        {
            var radios = await Radio.GetRadiosAsync();

            var bluetooth = radios.FirstOrDefault(
                r => r.Kind == RadioKind.Bluetooth);

            if (bluetooth == null)
            {
                trayIcon.Icon = CreateBluetoothIcon(Color.Gray);
                trayIcon.Text = "Bluetooth: не найден";
                return;
            }

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
        catch
        {
            trayIcon.Icon = CreateBluetoothIcon(Color.Gray);
            trayIcon.Text = "Bluetooth: ошибка";
        }
    }

    private Icon CreateBluetoothIcon(Color backgroundColor)
    {
        const int size = 64;

        var bitmap = new Bitmap(size, size);

        using (var graphics = Graphics.FromImage(bitmap))
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Clear(Color.Transparent);

            // Цветной круг состояния
            using (var brush = new SolidBrush(backgroundColor))
            {
                graphics.FillEllipse(
                    brush,
                    2,
                    2,
                    60,
                    60);
            }

            // Белый символ Bluetooth
            using (var pen = new Pen(Color.White, 6))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                pen.LineJoin = LineJoin.Round;

                graphics.DrawLine(
                    pen,
                    32, 10,
                    32, 54);

                graphics.DrawLine(
                    pen,
                    32, 10,
                    48, 22);

                graphics.DrawLine(
                    pen,
                    48, 22,
                    32, 32);

                graphics.DrawLine(
                    pen,
                    32, 22,
                    48, 42);

                graphics.DrawLine(
                    pen,
                    48, 42,
                    32, 54);

                graphics.DrawLine(
                    pen,
                    32, 32,
                    15, 16);

                graphics.DrawLine(
                    pen,
                    32, 32,
                    15, 48);
            }
        }

        IntPtr handle = bitmap.GetHicon();
        return Icon.FromHandle(handle);
    }

    private void ExitApplication()
    {
        trayIcon.Visible = false;
        trayIcon.Dispose();
        menu.Dispose();
        Application.Exit();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            trayIcon.Dispose();
            menu.Dispose();
        }

        base.Dispose(disposing);
    }
}