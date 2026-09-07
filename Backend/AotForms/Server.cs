using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Reborn;

namespace AotForms
{
    public partial class FormAh : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

        public static api KeyAuthApp = new api(
            name: "Kripamayd's Application",
            ownerid: "ScPf9TjHsz",
            secret: "6e71408cc6b90de63157713a84666826dbd27ac5004777940f9005abf92f936a",
            version: "1.0"
        );

        IntPtr mainHandle;
        public FormAh(IntPtr handle)
        {
            this.Hide();
            InitializeComponent();
            mainHandle = handle;
            KeyAuthApp.init();

        }

        private async void loginBtn_Click(object sender, EventArgs e)
        {
            KeyAuthApp.login(user.Text, pass.Text);
            if (KeyAuthApp.response.success)
            {
                var esp = new ESP();
                await esp.Start();
                new Thread(Data.Work) { IsBackground = true }.Start();
                new Thread(AimbotAi.Work) { IsBackground = true }.Start();
                new Thread(AimbotV2.Work) { IsBackground = true }.Start();
                await Task.Delay(1000);

                this.Hide();
            }
            else
            {
                MessageBox.Show(KeyAuthApp.response.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            KillProcess("HD-Adb");
            await Task.Delay(2000);
            KillProcess("HD-Player");
            await Task.Delay(1000);
            Environment.Exit(0);
        }

        public void KillProcess(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            foreach (var process in processes)
            {
                process.Kill();
                process.WaitForExit();
            }
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

        private async void FormAh_Load(object sender, EventArgs e)
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

            sta.Text = "Ready — Select FF version and Apply Hook";

            // Start ONLY PipeServer — it waits for "adbhook:0xXXXXXXX" from Python
            // ADB setup + all worker threads start inside PipeServer after adbhook arrives
            new Thread(() => PipeServer.Start()) { IsBackground = true }.Start();
            await Task.Delay(500);
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}