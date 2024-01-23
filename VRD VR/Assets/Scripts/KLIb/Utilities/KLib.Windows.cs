#if UNITY_STANDALONE_WIN
using System.Runtime.InteropServices;

namespace KLib
{
    public class Windows
    {
        const int SWP_HIDEWINDOW = 0x80; //hide window flag.
        const int SWP_SHOWWINDOW = 0x40; //show window flag.
        const int SWP_NOMOVE = 0x0002; //don't move the window flag.
        const int SWP_NOSIZE = 0x0001; //don't resize the window flag.
        const uint WS_SIZEBOX = 0x00040000;
        const int GWL_STYLE = -16;
        const int WS_BORDER = 0x00800000; //window with border
        const int WS_DLGFRAME = 0x00400000; //window with double border but no title
        const int WS_CAPTION = WS_BORDER | WS_DLGFRAME; //window with a title bar
        const int WS_SYSMENU = 0x00080000;      //window with no borders etc.
        const int WS_MAXIMIZEBOX = 0x00010000;
        const int WS_MINIMIZEBOX = 0x00020000;  //window with minimizebox

        const int SW_HIDE = 0;
        const int SW_NORMAL = 1;
        const int SW_SHOWMINIMIZED = 2;
        const int SW_SHOWMAXIMIZED = 3;
        const int SW_SHOW = 5;
        const int SW_MINIMIZE = 6;
        const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        static extern System.IntPtr SetWindowLong(
             System.IntPtr hWnd, // window handle
             int nIndex,
             uint dwNewLong
        );

        [DllImport("user32.dll")]
        static extern System.IntPtr GetWindowLong(
            System.IntPtr hWnd,
            int nIndex
        );

        [DllImport("user32.dll", EntryPoint = "SetWindowText")]
        static extern bool SetWindowText(System.IntPtr hwnd, System.String lpString);
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        static extern System.IntPtr FindWindow(System.String className, System.String windowName);
        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        static extern bool ShowWindow(System.IntPtr hWnd, int nCmdShow);

        System.IntPtr HWND_TOP = new System.IntPtr(0);
        System.IntPtr HWND_TOPMOST = new System.IntPtr(-1);
        System.IntPtr HWND_NOTOPMOST = new System.IntPtr(-2);

        public static void ChangeTitle(string oldTitle, string newTitle)
        {
            var hWnd = FindWindow(null, oldTitle);
            SetWindowText(hWnd, newTitle);
        }

        public static void MaximizeWindow(string title)
        {
            var hw = FindWindow(null, title);
            ShowWindow(hw, SW_SHOWMAXIMIZED);
        }

        public static void ShowWindowNormal(string title)
        {
            var hw = FindWindow(null, title);
            ShowWindow(hw, SW_SHOW);
        }

        public static void HideBorders(string title, bool value)
        {
            var hWnd = FindWindow(null, title);
            int style = GetWindowLong(hWnd, GWL_STYLE).ToInt32(); //gets current style

            if (value)
            {
                SetWindowLong(hWnd, GWL_STYLE, (uint)(style | WS_CAPTION | WS_SIZEBOX)); //Adds caption and the sizebox back.
            }
            else
            {
                SetWindowLong(hWnd, GWL_STYLE, (uint)(style & ~(WS_CAPTION | WS_SIZEBOX))); //removes caption and the sizebox from current style.
            }
        }

        public static void HideWindow(string title, bool value)
        {
            var hWnd = FindWindow(null, title);

            if (value)
            {
                ShowWindow(hWnd, SW_MINIMIZE);
            }
            else
            {
                ShowWindow(hWnd, SW_SHOW);
            }
        }

    }
}
#endif

