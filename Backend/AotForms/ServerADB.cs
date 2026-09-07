using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace AotForms
{
    public partial class ServerADB : Form
    {
        IntPtr mainHandle;
        public ServerADB(IntPtr handle)
        {
            mainHandle = handle;
            InitializeComponent();
        }

        private async void ServerADB_Load(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(0, 0);

            var processes = Process.GetProcessesByName("HD-Player");
            if (processes.Length != 1)
            {
                MessageBox.Show("Open emulator.");
                return;
            }

            var process = processes[0];
            string mainModulePath = Path.GetDirectoryName(process.MainModule?.FileName);
            if (string.IsNullOrEmpty(mainModulePath))
            {
                MessageBox.Show("Reinstall emulator.");
                return;
            }

            var adbPath = Path.Combine(mainModulePath, "HD-Adb.exe");
            if (!File.Exists(adbPath))
            {
                MessageBox.Show("Adb not Found. Reinstall emulator.");
                return;
            }

            // Store ADB path so PipeServer can use it when "adbhook" command arrives
            Config.EmulatorAdbPath = adbPath;
            Core.Handle = FindRenderWindow(mainHandle);

            // Start ONLY PipeServer — it waits for "adbhook:0xXXXXXXX" from Python
            // ADB setup + all worker threads start inside PipeServer after adbhook arrives
            new Thread(() => PipeServer.Start()) { IsBackground = true }.Start();
            await Task.Delay(500);
        }

        static IntPtr FindRenderWindow(IntPtr parent)
        {
            IntPtr renderWindow = IntPtr.Zero;
            WinAPI.EnumChildWindows(parent, (hWnd, lParam) =>
            {
                StringBuilder sb = new StringBuilder(256);
                WinAPI.GetWindowText(hWnd, sb, sb.Capacity);
                string windowName = sb.ToString();
                if (!string.IsNullOrEmpty(windowName))
                {
                    if (windowName != "HD-Player")
                    {
                        renderWindow = hWnd;
                    }
                }
                return true;
            }, IntPtr.Zero);

            return renderWindow;
        }
    }
}