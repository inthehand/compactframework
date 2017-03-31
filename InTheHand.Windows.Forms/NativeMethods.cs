// In The Hand - .NET Components for Mobility
//
// InTheHand.Windows.Forms.NativeMethods
// 
// Copyright (c) 2007-2012 In The Hand Ltd, All rights reserved.

using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using InTheHand.Drawing;

namespace InTheHand.Windows.Forms
{
    internal static class NativeMethods
    {
        //Combo/List
        internal const int LB_SETITEMHEIGHT = 0x01A0;
        internal const int LB_FINDSTRING = 0x018F;
        internal const int LB_FINDSTRINGEXACT = 0x01A2;
        internal const int CB_FINDSTRING = 0x14c;
        internal const int CB_FINDSTRINGEXACT = 0x158;
        internal const int CBS_EX_THEME = 0x00000004;

        [DllImport("coredll")]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("coredll", EntryPoint="GetWindow")]
        internal static extern IntPtr GetWindow(IntPtr hWnd, GW uCmd);

        internal enum GW : int
        {
            HWNDFIRST = 0,
            HWNDLAST = 1,
            HWNDNEXT = 2,
            HWNDPREV = 3,
            OWNER = 4,
            CHILD = 5,
        }

        //[DllImport(InTheHand.NativeMethods.coredll, EntryPoint = "GetClassName")]
        //internal static extern int GetClassName(IntPtr hWnd, System.Text.StringBuilder lpClassName, int nMaxCount); 

        [DllImport("coredll", EntryPoint = "FindWindow")]
        internal static extern IntPtr FindWindow(string className, string windowName);

        [DllImport("coredll", EntryPoint = "SetForegroundWindow")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetForegroundWindow(IntPtr hWnd); 

        [DllImport("coredll", EntryPoint = "GetParent")]
        internal static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("coredll", EntryPoint = "GetWindowRect")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect); 

        //[DllImport(coredll)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //internal static extern bool InvalidateRect(IntPtr hWnd,ref InTheHand.Drawing.RECT lpRect, bool bErase);

        [DllImport("coredll", EntryPoint = "RedrawWindow")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool RedrawWindow(IntPtr hwnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, RDW flags);

        [Flags]
        internal enum RDW
        {
            INVALIDATE         = 0x0001,
            INTERNALPAINT      = 0x0002,
            ERASE              = 0x0004,

            VALIDATE           = 0x0008,
//#define RDW_NOINTERNALPAINT     0x0010   //don't support
            NOERASE            = 0x0020,

            NOCHILDREN         = 0x0040,
            ALLCHILDREN        = 0x0080,

            UPDATENOW          = 0x0100,
            ERASENOW           = 0x0200,

//#define RDW_FRAME               0x0400
//#define RDW_NOFRAME             0x0800
        }

        //[DllImport(coredll)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //internal static extern bool UpdateWindow(IntPtr hWnd); 

        [DllImport("coredll", EntryPoint = "GetFocus")]
        internal static extern IntPtr GetFocus();


        [DllImport("coredll", EntryPoint = "SetWindowText")]
        internal static extern bool SetWindowText(IntPtr hWnd, string lpString);

        [DllImport("coredll", EntryPoint = "GetWindowText")]
        internal static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);


        [DllImport("coredll", EntryPoint = "SendMessage")]
        internal static extern int SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

        [DllImport("coredll", EntryPoint = "SendMessage")]
        internal static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref LVBKIMAGE lParam);

        [DllImport("coredll", EntryPoint = "SendMessage")]
        internal static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref RECT lParam);

        [DllImport("coredll", EntryPoint = "SendMessage")]
        internal static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);


        /*[DllImport(coredll, SetLastError=true)]
        internal static extern IntPtr CreateWindowEx(uint dwExStyle, string lpClassName,
            string lpWindowName, uint dwStyle, int x, int y, int nWidth, int nHeight,
            IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, int lpParam);

        [DllImport(coredll)]
        internal static extern IntPtr DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport(coredll)]
        internal static extern IntPtr GetModuleHandle(string modName);*/

        [DllImport("coredll", EntryPoint = "SetWindowLong", SetLastError = true)]
        internal static extern int SetWindowLong(IntPtr hWnd, GWL nIndex, int dwNewLong);

        [DllImport("coredll", EntryPoint = "SetWindowLong", SetLastError = true)]
        internal static extern int SetWindowLong(IntPtr hWnd, GWL nIndex, WndProcDelegate newProc);

        [DllImport("coredll", EntryPoint = "GetWindowLong", SetLastError = true)]
        internal static extern IntPtr GetWindowLong(IntPtr hWnd, GWL nIndex);

        internal enum GWL
        {
            WNDPROC = -4,
            STYLE = -16,
            EXSTYLE = -20,
            USERDATA = -21,
            ID = -12,
        }

        [DllImport("coredll", EntryPoint = "CallWindowProc", SetLastError = true)]
        internal static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("coredll", SetLastError = true)]
        internal static extern IntPtr CreateWindowEx(uint dwExStyle, string lpClassName,
            string lpWindowName, uint dwStyle, int x, int y, int nWidth, int nHeight,
            IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, int lpParam);

        [DllImport("coredll", SetLastError = true)]
        internal static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("coredll")]
        internal static extern IntPtr DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("coredll")]
        internal static extern IntPtr GetModuleHandle(string modName);

        internal const int WM_CREATE = 0x0001;
        internal const int WM_ERASEBKGND = 0x0014;
        internal const int WM_NOTIFY = 0x4E;
        internal const int NM_CUSTOMDRAW = (-12);
        internal const int WM_DRAWITEM = 0x002B;
        internal const int WM_COMMAND = 0x0111;
        internal const int BM_GETCHECK = 0x00F0;
        internal const int BM_SETCHECK = 0x00F1;

        internal const int WS_EX_TRANSPARENT = 0x00000020;
        internal const int WS_EX_LAYERED = 0x00080000;

        //WM6.5
        [DllImport("coredll", EntryPoint = "SetLayeredWindowAttributes", SetLastError = true)]
        [return:MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, LWA dwFlags);
  
        [Flags]
        internal enum LWA
        {
            COLORKEY           = 0x00000001,
            ALPHA              = 0x00000002,
        }

        [DllImport("coredll", EntryPoint = "GetAsyncKeyState")]
        internal static extern short GetAsyncKeyState(Keys vKey); 

        #region System Metrics
        [DllImport("coredll", EntryPoint = "GetSystemMetrics", SetLastError = true)]
        internal static extern int GetSystemMetrics(SM nIndex);

        internal enum SM : int
        {
            /*CXSCREEN = 0,
            CYSCREEN = 1,*/
            CXVSCROLL = 2,
            CYHSCROLL = 3,
            CYCAPTION = 4,
            CXBORDER = 5,
            CYBORDER = 6,
            CXDLGFRAME = 7,
            CYDLGFRAME = 8,
            CXICON = 11,
            CYICON = 12,
            // @CESYSGEN IF GWES_ICONCURS
            CXCURSOR = 13,
            CYCURSOR = 14,
            // @CESYSGEN ENDIF
            //CYMENU = 15,
            //CXFULLSCREEN = 16,
            //CYFULLSCREEN = 17,
            //MOUSEPRESENT = 19,
            CYVSCROLL = 20,
            CXHSCROLL = 21,
            DEBUG = 22,
            CXDOUBLECLK = 36,
            CYDOUBLECLK = 37,
            CXICONSPACING = 38,
            CYICONSPACING = 39,
            CXEDGE = 45,
            CYEDGE = 46,
            CXSMICON = 49,
            CYSMICON = 50,

            XVIRTUALSCREEN = 76,
            YVIRTUALSCREEN = 77,
            CXVIRTUALSCREEN = 78,
            CYVIRTUALSCREEN = 79,
            CMONITORS = 80,
            SAMEDISPLAYFORMAT = 81,
        }
        #endregion

        [DllImport("coredll")]
        internal static extern bool GetSystemPowerStatusEx2(PowerStatus pStatus, int dwLen, bool fUpdate);

    }

    [Flags()]
    internal enum BST
    {
        UNCHECKED = 0x0000,
        CHECKED = 0x0001,
        INDETERMINATE = 0x0002,
        PUSHED = 0x0004,
        FOCUS = 0x0008,
    }

    internal enum ODT
    {
        MENU = 1,
        LISTBOX = 2,
        COMBOBOX = 3,
        BUTTON = 4,
    }

    [Flags()]
    internal enum ODA
    {
        DRAWENTIRE = 0x0001,
        SELECT = 0x0002,
        FOCUS = 0x0004,
    }

    [Flags()]
    internal enum ODS
    {
        SELECTED = 0x0001,
        GRAYED = 0x0002,
        DISABLED = 0x0004,
        CHECKED = 0x0008,
        FOCUS = 0x0010,
    }

    internal struct DRAWITEMSTRUCT
    {
        public ODT CtlType;
        public uint CtlID;
        public uint itemID;
        public ODA itemAction;
        public ODS itemState;
        public IntPtr hwndItem;
        public IntPtr hDC;
        public RECT rcItem;
        public IntPtr itemData;
    }

    internal delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    
    [StructLayout(LayoutKind.Sequential)]
    internal struct NMHDR
    {
        public IntPtr hwndFrom;
        public uint idFrom;
        public int code;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    internal struct NMCUSTOMDRAW
    {
        public NMHDR hdr;

        public CDDS dwDrawStage;
        public IntPtr hdc;
        public RECT rc;
        public int dwItemSpec;
        public CDIS uItemState;
        public IntPtr lItemlParam;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    internal struct NMLVCUSTOMDRAW
    {
        public NMCUSTOMDRAW nmcd;

        public int clrText;
        public int clrTextBk;
        //ce 5.0
        public int iSubItem;
        public LVCDI dwItemType;

        public RECT rcText;
        public LVGA uAlign;
    }

    [Flags()]
    internal enum CDIS
    {
        SELECTED          = 0x0001,
        GRAYED            = 0x0002,
        DISABLED          = 0x0004,
        CHECKED           = 0x0008,
        FOCUS             = 0x0010,
        DEFAULT           = 0x0020,
        HOT               = 0x0040,
        NOCONTROLFOCUS    = 0x8000,
    }

    [Flags()]
    internal enum CDDS
    {
        PREPAINT          = 0x00000001,
        POSTPAINT         = 0x00000002,
        PREERASE          = 0x00000003,
        POSTERASE         = 0x00000004,
// the 0x000010000 bit means it's individual item specific
        ITEM              = 0x00010000,
        ITEMPREPAINT      = (ITEM | PREPAINT),
        ITEMPOSTPAINT     = (ITEM | POSTPAINT),
        ITEMPREERASE      = (ITEM | PREERASE),
        ITEMPOSTERASE     = (ITEM | POSTERASE),
        SUBITEM           = 0x00020000,
    }

    

    [Flags()]
    internal enum CDRF
    {
        DODEFAULT = 0x00000000,
        SKIPDEFAULT = 0x00000004,
        NOTIFYPOSTPAINT = 0x00000010,
        NOTIFYITEMDRAW = 0x00000020,
        NOTIFYSUBITEMDRAW = NOTIFYITEMDRAW,
    }
}