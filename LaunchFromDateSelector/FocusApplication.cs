using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

public static class FocusApplication {

    [DllImport("user32.dll")]
    private static extern int ShowWindowAsync(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern int SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern int IsIconic(IntPtr hWnd);

    private static IntPtr GetApplicationWindowHandle(string filePath) {
        IntPtr hWnd = IntPtr.Zero;
        foreach (Process process in Process.GetProcessesByName(Path.GetFileNameWithoutExtension(filePath)).Where(p => p.SessionId == Process.GetCurrentProcess().SessionId).ToArray()) {
            string fileName = null;
            try {
                fileName = process.MainModule.FileName;
            } catch (Exception exception) {
                Debug.WriteLine(exception);
            }
            if (fileName == filePath && process.MainWindowHandle != IntPtr.Zero) {
                hWnd = process.MainWindowHandle;
                break;
            }
        }
        return hWnd;
    }

    public static bool SwitchToRunningInstance(string filePath) {
        IntPtr hWnd = GetApplicationWindowHandle(filePath);
        if (hWnd != IntPtr.Zero) {
            if (IsIconic(hWnd) != 0) {
                ShowWindowAsync(hWnd, SW_RESTORE);
            }
            SetForegroundWindow(hWnd);
            return true;
        }
        return false;
    }

    const int SW_RESTORE = 9;
}
