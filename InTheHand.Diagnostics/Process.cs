// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessHelper.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Provides methods which extend the <see cref="System.Diagnostics.Process"/> class.
    /// </summary>
    /// <seealso cref="System.Diagnostics.Process"/>
    public static class ProcessHelper
    {
        /// <summary>
        /// Gets the name of the process.
        /// </summary>
        /// <param name="p">The <see cref="Process"/>.</param>
        /// <returns>
        /// The name that the system uses to identify the process to the user.</returns>
        /// <remarks>
        /// <para>Equivalent to System.Diagnostics.Process.ProcessName in the full .NET Framework</para>
        /// The ProcessName property holds an executable file name, such as Outlook, that does not include the .exe extension or the path.
        /// It is helpful for getting and manipulating all the processes that are associated with the same executable file.</remarks>
        /// <seealso cref="System.Diagnostics.Process"/>
        public static string GetProcessName(this Process p)
        {
            string name = System.IO.Path.GetFileNameWithoutExtension(p.StartInfo.FileName);
            return name;
        }

        /// <summary>
        /// Gets the window handle of the main window of the associated process.
        /// </summary>
        /// <param name="p"></param>
        /// <returns>The system-generated window handle of the main window of the associated process.</returns>
        /// <remarks>The main window is the window that is created when the process is started.
        /// After initialization, other windows may be opened, including the Modal and TopLevel windows, but the first window associated with the process remains the main window.</remarks>
        public static IntPtr GetMainWindowHandle(this Process p)
        {
            //default method is quicker if available
            if (p.MainWindowHandle != IntPtr.Zero)
            {
                return p.MainWindowHandle;
            }

            NativeMethods.WNDENUMPARAMS wParams = new NativeMethods.WNDENUMPARAMS();
            wParams.processId = p.Id;

            bool success = NativeMethods.EnumWindows(new NativeMethods.WNDENUMPROC(NativeMethods.WndEnumProc), ref wParams);
            if (wParams.hwnd != IntPtr.Zero)
            {
                return wParams.hwnd;
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Gets the caption of the main window of the process.
        /// </summary>
        /// <param name="p">The Process</param>
        /// <returns>The process's main window title.</returns>
        /// <remarks>A process has a main window associated with it only if the process has a graphical interface.
        /// If the associated process does not have a main window (so that <see cref="Process.MainWindowHandle"/> is zero), MainWindowTitle is an empty string ("").</remarks>
        public static string GetMainWindowTitle(this Process p)
        {
            IntPtr hwnd = GetMainWindowHandle(p);

            if (hwnd != IntPtr.Zero)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder(InTheHand.EnvironmentInTheHand.MaxPath);
                int len = NativeMethods.GetWindowText(hwnd, sb, sb.Capacity);
                return sb.ToString(0, len);
            }

            return string.Empty;
        }

        /// <summary>
        /// Creates a new <see cref="Process"/> component for each process resource on the local computer. 
        /// </summary>
        /// <returns>An array of type <see cref="Process"/> that represents all the process resources running on the local computer.</returns>
        /// <remarks>Use this method to create an array of new <see cref="Process"/> components and associate them with all the process resources on the local computer.
        /// The process resources must already exist on the local computer, because <see cref="GetProcesses"/> does not create system resources but rather associates resources with application-generated <see cref="Process"/> components.
        /// Because the operating system itself is running background processes, this array is never empty.
        /// <para>The name of the process can be retrieved from <see cref="ProcessStartInfo.FileName">Process.StartInfo.FileName</see> or by using the <see cref="GetProcessName"/> extension method.</para></remarks>
        /// <seealso cref="System.Diagnostics.Process"/>
        /// <seealso cref="GetProcessName"/>
        public static Process[] GetProcesses()
        {
            IntPtr hSnap = NativeMethods.CreateToolhelp32Snapshot(NativeMethods.TH32CS.SNAPPROCESS | NativeMethods.TH32CS.SNAPNOHEAPS, 0);

            System.Collections.Generic.List<Process> processes = new System.Collections.Generic.List<Process>();
            NativeMethods.PROCESSENTRY32 pe32 = new NativeMethods.PROCESSENTRY32();
            pe32.dwSize = Marshal.SizeOf(typeof(NativeMethods.PROCESSENTRY32));
            bool success = NativeMethods.Process32First(hSnap, ref pe32);

            while (success)
            {
                Process p = new Process();
                Type t = typeof(Process);
                t.GetField("processId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(p, pe32.th32ProcessID);
                t.GetField("haveProcessId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(p, true);
                p.StartInfo.FileName = pe32.szExeFile;

                //p.processName = pe32.szExeFile.Substring(0, pe32.szExeFile.IndexOf('.'));
                processes.Add(p);

                pe32 = new NativeMethods.PROCESSENTRY32();
                pe32.dwSize = Marshal.SizeOf(typeof(NativeMethods.PROCESSENTRY32));

                success = NativeMethods.Process32Next(hSnap, ref pe32);
            }

            success = NativeMethods.CloseToolhelp32Snapshot(hSnap);

            return processes.ToArray();
        }

        /// <summary>
        /// Gets the modules that have been loaded by the associated process.
        /// </summary>
        /// <param name="p">The Process.</param>
        /// <returns>An array of type <see cref="ProcessModule"/> that represents the modules that have been loaded by the associated process.</returns>
        public static System.Collections.ObjectModel.ReadOnlyCollection<ProcessModule> GetModules(this Process p)
        {
            List<ProcessModule> modules = new List<ProcessModule>();

            IntPtr handle = NativeMethods.CreateToolhelp32Snapshot(NativeMethods.TH32CS.SNAPMODULE, p.Id);
            if (handle != IntPtr.Zero)
            {
                NativeMethods.MODULEENTRY32 me = new NativeMethods.MODULEENTRY32();
                me.dwSize = Marshal.SizeOf(me);
                bool success = NativeMethods.Module32First(handle, ref me);
                while (success)
                {
                    modules.Add(new ProcessModule(me));

                    success = NativeMethods.Module32Next(handle, ref me);
                }
                NativeMethods.CloseToolhelp32Snapshot(handle);
            }

            return new System.Collections.ObjectModel.ReadOnlyCollection<ProcessModule>(modules);
        }

        /// <summary>
        /// Gets the set of threads that are running in the associated process.
        /// </summary>
        /// <param name="p">The Process.</param>
        /// <returns>An array of type <see cref="ProcessThread"/> representing the operating system threads currently running in the associated process.</returns>
        public static System.Collections.ObjectModel.ReadOnlyCollection<ProcessThread> GetThreads(this Process p)
        {
            List<ProcessThread> threads = new List<ProcessThread>();

            IntPtr handle = NativeMethods.CreateToolhelp32Snapshot(NativeMethods.TH32CS.SNAPTHREAD, p.Id);
            if (handle != IntPtr.Zero)
            {
                NativeMethods.THREADENTRY32 te = new NativeMethods.THREADENTRY32();
                te.dwSize = Marshal.SizeOf(te);
                bool success = NativeMethods.Thread32First(handle, ref te);
                while (success)
                {
                    if (te.th32CurrentProcessID == p.Id)
                    {
                        threads.Add(new ProcessThread(te));
                    }

                    success = NativeMethods.Thread32Next(handle, ref te);
                }
                NativeMethods.CloseToolhelp32Snapshot(handle);
            }

            return new System.Collections.ObjectModel.ReadOnlyCollection<ProcessThread>(threads);
        }
    }
}
