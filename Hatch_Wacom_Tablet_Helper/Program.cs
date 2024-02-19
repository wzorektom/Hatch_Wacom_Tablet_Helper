using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Wilcom_Hatch_Wacom_Tablet_Helper
{
    internal class Program
    {
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        // Constants for mouse events
        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const uint MOUSEEVENTF_RIGHTUP = 0x10;
        private const uint KEYEVENTF_KEYUP = 0x0002; // Key up flag for keybd_event

        // Variables to store user-selected keys
        private static Keys toggleMouseButtonKey;
        private static Keys simulateRightClickKey;
        private static Keys simulateEnterKey;
        private static Keys simulateBackspaceKey; // Variable for Backspace simulation

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static bool isMouseDown = false;

        public static void Main()
        {
            Console.WriteLine(new string('=', 80));
            Console.WriteLine("Welcome to the Wilcom Hatch Wacom Tablet Helper App!");
            Console.WriteLine("Visit mollypawsup.com if you like to say hi!");
            Console.WriteLine(new string('=', 80));
            Console.WriteLine("\nThis helper app enhances your experience with the Wilcom Hatch Digitizing software\nby allowing you to use a Wacom standard or Intuos tablet more effectively.\nSpecifically, it overrides Hatch default functionality where a left or right mouse click is required to register either a corner or curved point.");
            Console.WriteLine("With this app, you can simulate a left mouse click using your tablet pen\nwith the Freehand tool in Hatch, making digitizing more intuitive and efficient.\n");
            Console.WriteLine("By simulating the mouse right click, you can use your table pen to register the curved corners by simply pressing the assigned key.\n");
            Console.WriteLine("Please follow the prompts to configure the app according to your needs.\nYou can skip any step by pressing Enter if you do not wish to use that particular functionality during this session.\n");
            Console.WriteLine(new string('=', 80));

            Console.WriteLine("Press the key you want to use for toggling the left mouse button (press Enter to skip):");
            toggleMouseButtonKey = ReadKeyOrDefault();

            Console.WriteLine("Press the key you want to use for simulating a right click (press Enter to skip):");
            simulateRightClickKey = ReadKeyOrDefault();

            Console.WriteLine("Press the key you want to use for simulating an Enter press (press Enter to skip):");
            simulateEnterKey = ReadKeyOrDefault();

            Console.WriteLine("Press the key you want to use for simulating a Backspace press (press Enter to skip):");
            simulateBackspaceKey = ReadKeyOrDefault();

            _hookID = SetHook(_proc);
            Console.WriteLine("Configuration complete. Close this window with your mouse to terminate the app.");
            System.Windows.Forms.Application.Run();
            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static Keys ReadKeyOrDefault()
        {
            var keyInfo = Console.ReadKey(true);
            // If Enter is pressed without any other key, return Keys.None to skip.
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine("Skipped.");
                return Keys.None;
            }
            else
            {
                Console.WriteLine($"Selected key: {keyInfo.Key}");
                return (Keys)keyInfo.Key;
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;

                // Check if the key is set to None, skip the action
                if (key == toggleMouseButtonKey && toggleMouseButtonKey != Keys.None)
                {
                    ToggleLeftMouseButton();
                    return (IntPtr)1;
                }
                else if (key == simulateRightClickKey && simulateRightClickKey != Keys.None)
                {
                    SimulateRightClick();
                    return (IntPtr)1;
                }
                else if (key == simulateEnterKey && simulateEnterKey != Keys.None)
                {
                    SimulateEnterKeyPress();
                    return (IntPtr)1;
                }
                else if (key == simulateBackspaceKey && simulateBackspaceKey != Keys.None)
                {
                    SimulateBackspaceKeyPress();
                    return (IntPtr)1;
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private static void ToggleLeftMouseButton()
        {
            if (!isMouseDown)
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                isMouseDown = true;
                Console.WriteLine("Simulated left mouse button DOWN");
            }
            else
            {
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                isMouseDown = false;
                Console.WriteLine("Simulated left mouse button UP");
            }
        }

        private static void SimulateBackspaceKeyPress()
        {
            keybd_event((byte)Keys.Back, 0, 0, 0); // Simulate Backspace key press
            keybd_event((byte)Keys.Back, 0, KEYEVENTF_KEYUP, 0); // Simulate Backspace key release
            Console.WriteLine("Simulated Backspace key press");
        }

        private static void SimulateRightClick()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
            Console.WriteLine("Simulated right mouse click");
        }

        private static void SimulateEnterKeyPress()
        {
            keybd_event((byte)Keys.Return, 0, 0, 0); // Simulate Enter key press
            keybd_event((byte)Keys.Return, 0, KEYEVENTF_KEYUP, 0); // Simulate Enter key release
            Console.WriteLine("Simulated Enter key press");
        }
    }
}
