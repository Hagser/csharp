using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
class MyShowWindow
{
    [DllImport("user32.dll")]
    private static extern
        bool SetForegroundWindow(IntPtr hWnd);
    [DllImport("user32.dll")]
    private static extern
        bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
    [DllImport("user32.dll")]
    private static extern
        bool IsIconic(IntPtr hWnd);
    
    [DllImport("user32.dll")]
    private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

    private const int SW_HIDE = 0;
    private const int SW_SHOWNORMAL = 1;
    private const int SW_SHOWMINIMIZED = 2;
    private const int SW_SHOWMAXIMIZED = 3;
    private const int SW_SHOWNOACTIVATE = 4;
    private const int SW_RESTORE = 9;
    private const int SW_SHOWDEFAULT = 10;

    public static void ShowWindow(IntPtr hWnd)
    {
        if (IsIconic(hWnd))
        {
            ShowWindowAsync(hWnd, SW_RESTORE);
        }
        SetForegroundWindow(hWnd);
    }
}