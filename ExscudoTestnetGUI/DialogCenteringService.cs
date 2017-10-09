using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExscudoTestnetGUI
{


    public class DialogCenteringService : IDisposable
    {
        private readonly IWin32Window owner;
        private readonly HookProc hookProc;
        private readonly IntPtr hHook = IntPtr.Zero;

        public DialogCenteringService(IWin32Window owner)
        {
            if (owner == null) throw new ArgumentNullException("owner");

            this.owner = owner;
            hookProc = DialogHookProc;

            hHook = SetWindowsHookEx(WH_CALLWNDPROCRET, hookProc, IntPtr.Zero, GetCurrentThreadId());
        }

        private IntPtr DialogHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return CallNextHookEx(hHook, nCode, wParam, lParam);
            }

            CWPRETSTRUCT msg = (CWPRETSTRUCT)Marshal.PtrToStructure(lParam, typeof(CWPRETSTRUCT));
            IntPtr hook = hHook;

            if (msg.message == (int)CbtHookAction.HCBT_ACTIVATE)
            {
                try
                {
                    CenterWindow(msg.hwnd);
                }
                finally
                {
                    UnhookWindowsHookEx(hHook);
                }
            }

            return CallNextHookEx(hook, nCode, wParam, lParam);
        }

        public void Dispose()
        {
            UnhookWindowsHookEx(hHook);
        }

        private void CenterWindow(IntPtr hChildWnd)
        {
            Rectangle? recParent = GetWindowRect(owner.Handle);

            if (recParent == null)
            {
                return;
            }

            CenterWindow(hChildWnd, recParent.Value);
        }

        public static Rectangle? GetWindowRect(IntPtr hWnd)
        {
            Rectangle rect = new Rectangle(0, 0, 0, 0);
            bool success = GetWindowRect(hWnd, ref rect);

            if (!success)
            {
                return null;
            }

            return rect;
        }

        public static Rectangle GetCenterRectangle(Rectangle recParent, Rectangle recChild)
        {
            int width = recChild.Width - recChild.X;
            int height = recChild.Height - recChild.Y;

            Point ptCenter = new Point(0, 0);
            ptCenter.X = recParent.X + ((recParent.Width - recParent.X) / 2);
            ptCenter.Y = recParent.Y + ((recParent.Height - recParent.Y) / 2);

            Point ptStart = new Point(0, 0);
            ptStart.X = (ptCenter.X - (width / 2));
            ptStart.Y = (ptCenter.Y - (height / 2));

            // get centered rectangle
            Rectangle centeredRectangle = new Rectangle(ptStart.X, ptStart.Y, width, height);

            // fit the window to the screen
            Screen parentScreen = Screen.FromRectangle(recParent);
            Rectangle workingArea = parentScreen.WorkingArea;

            // various collision checks
            if (workingArea.X > centeredRectangle.X)
            {
                centeredRectangle = new Rectangle(workingArea.X, centeredRectangle.Y, centeredRectangle.Width, centeredRectangle.Height);
            }
            if (workingArea.Y > centeredRectangle.Y)
            {
                centeredRectangle = new Rectangle(centeredRectangle.X, workingArea.Y, centeredRectangle.Width, centeredRectangle.Height);
            }
            if (workingArea.Right < centeredRectangle.Right)
            {
                centeredRectangle = new Rectangle(workingArea.Right - centeredRectangle.Width, centeredRectangle.Y, centeredRectangle.Width, centeredRectangle.Height);
            }
            if (workingArea.Bottom < centeredRectangle.Bottom)
            {
                centeredRectangle = new Rectangle(centeredRectangle.X, workingArea.Bottom - centeredRectangle.Height, centeredRectangle.Width, centeredRectangle.Height);
            }

            return centeredRectangle;
        }

        public static void CenterWindow(IntPtr hChildWnd, Rectangle recParent)
        {
            Rectangle? recChild = GetWindowRect(hChildWnd);

            if (recChild == null)
            {
                return;
            }

            Rectangle centerRectangle = GetCenterRectangle(recParent, recChild.Value);

            Task.Factory.StartNew(() => SetWindowPos(hChildWnd, (IntPtr)0, centerRectangle.X, centerRectangle.Y, centerRectangle.Width, centerRectangle.Height, SetWindowPosFlags.SynchronousWindowPosition | SetWindowPosFlags.IgnoreResize | SetWindowPosFlags.DoNotActivate | SetWindowPosFlags.DoNotReposition | SetWindowPosFlags.IgnoreZOrder));
        }

        // some p/invoke

        // ReSharper disable InconsistentNaming
        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        public delegate void TimerProc(IntPtr hWnd, uint uMsg, UIntPtr nIDEvent, uint dwTime);

        private const int WH_CALLWNDPROCRET = 12;

        // ReSharper disable EnumUnderlyingTypeIsInt
        private enum CbtHookAction : int
        // ReSharper restore EnumUnderlyingTypeIsInt
        {
            // ReSharper disable UnusedMember.Local
            HCBT_MOVESIZE = 0,
            HCBT_MINMAX = 1,
            HCBT_QS = 2,
            HCBT_CREATEWND = 3,
            HCBT_DESTROYWND = 4,
            HCBT_ACTIVATE = 5,
            HCBT_CLICKSKIPPED = 6,
            HCBT_KEYSKIPPED = 7,
            HCBT_SYSCOMMAND = 8,
            HCBT_SETFOCUS = 9
            // ReSharper restore UnusedMember.Local
        }

        [DllImport("kernel32.dll")]
        static extern int GetCurrentThreadId();

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);

        [DllImport("user32.dll")]
        private static extern int MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SetWindowPosFlags uFlags);

        [DllImport("User32.dll")]
        public static extern UIntPtr SetTimer(IntPtr hWnd, UIntPtr nIDEvent, uint uElapse, TimerProc lpTimerFunc);

        [DllImport("User32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll")]
        public static extern int UnhookWindowsHookEx(IntPtr idHook);

        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxLength);

        [DllImport("user32.dll")]
        public static extern int EndDialog(IntPtr hDlg, IntPtr nResult);

        [StructLayout(LayoutKind.Sequential)]
        public struct CWPRETSTRUCT
        {
            public IntPtr lResult;
            public IntPtr lParam;
            public IntPtr wParam;
            public uint message;
            public IntPtr hwnd;
        };
        // ReSharper restore InconsistentNaming

        private enum SetWindowPosFlags : uint
        {
            SynchronousWindowPosition = 0x4000,
            DeferErase = 0x2000,
            DrawFrame = 0x0020,
            FrameChanged = 0x0020,
            HideWindow = 0x0080,
            DoNotActivate = 0x0010,
            DoNotCopyBits = 0x0100,
            IgnoreMove = 0x0002,
            DoNotChangeOwnerZOrder = 0x0200,
            DoNotRedraw = 0x0008,
            DoNotReposition = 0x0200,
            DoNotSendChangingEvent = 0x0400,
            IgnoreResize = 0x0001,
            IgnoreZOrder = 0x0004,
            ShowWindow = 0x0040,
        }
    }
}
