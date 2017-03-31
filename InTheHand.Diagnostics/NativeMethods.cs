// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NativeMethods.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Diagnostics
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    internal static class NativeMethods
    {
        //toolhelp
        private const string toolhelp = "toolhelp";

        [DllImport(toolhelp, EntryPoint = "CreateToolhelp32Snapshot")]
        internal static extern IntPtr CreateToolhelp32Snapshot(TH32CS dwFlags, int th32ProcessID);

        [DllImport(toolhelp, EntryPoint = "CloseToolhelp32Snapshot")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseToolhelp32Snapshot(IntPtr hSnapshot);


        [DllImport(toolhelp, EntryPoint = "Process32First")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool Process32First(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [DllImport(toolhelp, EntryPoint = "Process32Next")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool Process32Next(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);


        [DllImport(toolhelp, EntryPoint = "Thread32First", SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool Thread32First(IntPtr hSnapshot, ref THREADENTRY32 lpte);

        [DllImport(toolhelp, EntryPoint = "Thread32Next", SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool Thread32Next(IntPtr hSnapshot, ref THREADENTRY32 lpte);


        [DllImport(toolhelp, EntryPoint = "Module32First", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool Module32First(IntPtr hSnapshot, ref MODULEENTRY32 lpme);

        [DllImport(toolhelp, EntryPoint = "Module32Next", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool Module32Next(IntPtr hSnapshot, ref MODULEENTRY32 lpme);


        [DllImport(toolhelp, EntryPoint = "Heap32ListFirst", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool Heap32ListFirst(IntPtr hSnapshot, ref HEAPLIST32 lphl);

        [DllImport(toolhelp, EntryPoint = "Heap32ListNext", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool Heap32ListNext(IntPtr hSnapshot, ref HEAPLIST32 lphl);

        [DllImport(toolhelp, EntryPoint = "Heap32First", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool Heap32First(IntPtr hSnapshot, ref HEAPENTRY32 lphe, int th32ProcessID, int th32HeapID);
        
        [DllImport(toolhelp, EntryPoint = "Heap32Next", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool Heap32Next(IntPtr hSnapshot, ref HEAPENTRY32 lphe );


        [Flags()]
        internal enum TH32CS
        {
            SNAPHEAPLIST = 0x00000001,
            SNAPPROCESS  = 0x00000002,
            SNAPTHREAD   = 0x00000004,
            SNAPMODULE   = 0x00000008,
            SNAPNOHEAPS  = 0x40000000,	// optimization to not snapshot heaps
            //SNAPALL	    = SNAPHEAPLIST | SNAPPROCESS | SNAPTHREAD | SNAPMODULE,
            //GETALLMODS	= 0x80000000,
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
        internal struct PROCESSENTRY32 
        {
            internal int dwSize;
            internal uint cntUsage;
            internal int th32ProcessID;
            internal uint th32DefaultHeapID;
            internal uint th32ModuleID;
            internal uint cntThreads;
            internal uint th32ParentProcessID;
            internal int pcPriClassBase;
            internal uint dwFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=260)]
            internal string szExeFile;
            internal uint th32MemoryBase;
            internal uint th32AccessKey;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct THREADENTRY32
        {
            internal int dwSize;
            internal int cntUsage;
            internal int th32ThreadID;
            internal int th32OwnerProcessID;
            internal int tpBasePri;
            internal int tpDeltaPri;
            private int dwFlags; //reserved
            internal int th32AccessKey;
            internal int th32CurrentProcessID;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MODULEENTRY32
        {
            internal int dwSize;
            internal int th32ModuleID;
            internal int th32ProcessID;
            private int GlblcntUsage;
            private int ProccntUsage;
            internal IntPtr modBaseAddr;
            internal int modBaseSize;
            private IntPtr hModule;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = InTheHand.EnvironmentInTheHand.MaxPath)]
            internal string szModule;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = InTheHand.EnvironmentInTheHand.MaxPath)]
            internal string szExePath;
            private int dwFlags; //reserved
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct HEAPLIST32
        {
            internal int dwSize;
            internal int th32ProcessID;
            internal int th32HeapID;
            internal uint dwFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct HEAPENTRY32
        {
            internal int dwSize;
            internal uint hHandle;
            internal uint dwAddress;
            internal uint dwBlockSize;
            internal LF32 dwFlags;
            internal uint dwLockCount;
            private uint dwResvd;
            internal int th32ProcessID;
            internal int th32HeapID;
        }

        [Flags()]
        internal enum LF32
        {
            FIXED = 0x00000001,
            FREE = 0x00000002,
            MOVEABLE = 0x00000004,
            DECOMMIT = 0x00000008,
            BIGBLOCK = 0x00000010,
        }

        [DllImport("coredll", EntryPoint = "GetThreadTimes", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetThreadTimes(IntPtr hThread, out long creationTime, out long exitTime, out long kernelTime, out long userTime);

        // Used for Process.MainWindowHandle
        [DllImport("coredll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool EnumWindows(WNDENUMPROC enumFunc, ref WNDENUMPARAMS lParam);

        [DllImport("coredll")]
        internal static extern int GetWindowThreadProcessId(IntPtr hWnd, out int processId);

        internal delegate int WNDENUMPROC(IntPtr hwnd, ref WNDENUMPARAMS lParam);

        [StructLayout(LayoutKind.Sequential)]
        internal struct WNDENUMPARAMS
        {
            internal int processId;
            internal IntPtr hwnd;
        }

        [DllImport("coredll", EntryPoint = "GetWindowRect", SetLastError = true)]
        internal static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

        [DllImport("coredll", EntryPoint = "GetWindow")]
        internal static extern IntPtr GetWindow(IntPtr hWnd, GW cmd);

        internal enum GW : int
        {
            HWNDFIRST = 0,
            HWNDLAST = 1,
            HWNDNEXT = 2,
            HWNDPREV = 3,
            OWNER = 4,
            CHILD = 5,
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            internal int left;
            internal int top;
            internal int right;
            internal int bottom;
        }

        [DllImport("coredll", EntryPoint = "IsWindowVisible", SetLastError = true)]
        internal static extern bool IsWindowVisible(IntPtr hWnd);

        internal static int WndEnumProc(IntPtr hwnd, ref WNDENUMPARAMS lParam)
        {
            int processId;
            int threadId = GetWindowThreadProcessId(hwnd, out processId);
            if (processId == lParam.processId)
            {
                //if(IsWindowVisible(hwnd))
                //{
                //must be bigger than 0,0
                RECT r;
                GetWindowRect(hwnd, out r);
                if (r.right - r.left > 0 && r.bottom - r.top > 0)
                {
                    if (GetWindow(hwnd, GW.OWNER) == IntPtr.Zero)
                    {
                        //window process id matches our process
                        lParam.hwnd = hwnd;
                        System.Diagnostics.Debug.WriteLine(hwnd.ToInt32().ToString("X"));
                        //end enumeration
                        return 0;
                    }
                }
                //}
            }

            //try next
            return -1;
        }

        [DllImport("coredll", EntryPoint="GetWindowText")]
        internal static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int maxCount); 
    }
}