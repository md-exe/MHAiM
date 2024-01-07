using System.Diagnostics;
using System.Runtime.InteropServices;

public class KeyboardHook
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;

    private readonly LowLevelKeyboardProc keyboardProc;
    private IntPtr hookId = IntPtr.Zero;

    public event EventHandler<KeyEventArgs> KeyDown;

    public KeyboardHook()
    {
        keyboardProc = HookCallback;
        hookId = SetHook(keyboardProc);
    }

    ~KeyboardHook()
    {
        UnhookWindowsHookEx(hookId);
    }

    private IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (ProcessModule curModule = Process.GetCurrentProcess().MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            KeyDown?.Invoke(this, new KeyEventArgs((Keys)vkCode));
        }

        return CallNextHookEx(hookId, nCode, wParam, lParam);
    }

    #region WinAPI
    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);
    #endregion
}