using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

public static class SingleMainForm {

    [DllImport("user32.dll")]
    private static extern int ShowWindowAsync(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern int SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern int IsIconic(IntPtr hWnd);

    private static IntPtr GetCurrentInstanceWindowHandle() {
        IntPtr hWnd = IntPtr.Zero;
        Process process = Process.GetCurrentProcess();
        FileSystemInfo processFileInfo = new FileInfo(process.MainModule.FileName);
        Process[] processes = Process.GetProcessesByName(process.ProcessName);
        foreach (Process p in processes) {
            if (p.Id != process.Id && p.MainWindowHandle != IntPtr.Zero) {
                FileSystemInfo _processFileInfo = new FileInfo(p.MainModule.FileName);
                if (processFileInfo.Name == _processFileInfo.Name) {
                    hWnd = p.MainWindowHandle;
                    break;
                }
            }
        }
        return hWnd;
    }

    private static void SwitchToCurrentInstance() {
        IntPtr hWnd = GetCurrentInstanceWindowHandle();
        if (hWnd != IntPtr.Zero) {
            if (IsIconic(hWnd) != 0) {
                ShowWindowAsync(hWnd, SW_RESTORE);
            }
            SetForegroundWindow(hWnd);
        }
    }

    public static bool Run(Form form) {
        if (IsAlreadyRunning(form)) {
            SwitchToCurrentInstance();
            return false;
        } else {
            Application.Run(form);
            return true;
        }
    }

    private static bool IsAlreadyRunning(Form form) {
        bool createdNew;
        mutex = new Mutex(true, Path.Combine("Local", Application.CompanyName + "_" + Application.ProductName + "_" + form.Name), out createdNew);
        if (createdNew) {
            mutex.ReleaseMutex();
        }
        return !createdNew;
    }

    static Mutex mutex;
    const int SW_RESTORE = 9;
}
