// In The Hand - .NET Components for Mobility
//
// InTheHand.Windows.Forms.Application
// 
// Copyright (c) 2003-2012 In The Hand Ltd, All rights reserved.

using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.WindowsCE.Forms;
using InTheHand.Diagnostics;

namespace InTheHand.Windows.Forms
{
    /// <summary>
    /// Provides properties which extend the <see cref="Application"/> class.
    /// </summary>
    public static class ApplicationInTheHand
    {
        private static object internalSyncObject = new object();
        
        #region Common App Data Path
        /// <summary>
        /// Gets the path for the application data that is shared among all users. 
        /// </summary>
        public static string CommonAppDataPath
        {
            get
            {
                return GetDataPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            }
        }
        #endregion

        #region Common App Data Registry
        /// <summary>
        /// Gets the registry key for the application data that is shared among all users.
        /// </summary>
        public static RegistryKey CommonAppDataRegistry
        {
            get
            {
                return Registry.LocalMachine.CreateSubKey(string.Format(@"Software\{0}\{1}\{2}", new object[] { CompanyName, ProductName, ProductVersion }));
            }
        }
        #endregion

        #region Company Name
        private static string companyName;
        /// <summary>
        /// Gets the company name associated with the application.
        /// </summary>
        public static string CompanyName
        {
            get
            {
                lock (internalSyncObject)
                {
                    if (companyName == null)
                    {
                        Assembly assembly = InTheHand.Reflection.AssemblyInTheHand.GetEntryAssembly();
                        if (assembly != null)
                        {
                            object[] attrs = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                            if ((attrs != null) && (attrs.Length > 0))
                            {
                                companyName = ((AssemblyCompanyAttribute)attrs[0]).Company;
                            }
                        }
                        if (companyName == null)
                        {
                            companyName = string.Empty;
                        }
                    }
                }
                return companyName;
            }
        }
        #endregion

        #region Executable Path
        /// <summary>
        /// Gets the path for the executable file that started the application, including the executable name.
        /// </summary>
        public static string ExecutablePath
        {
            get
            {
                return InTheHand.Reflection.AssemblyInTheHand.GetModuleFileName();
            }
        }
        #endregion


        #region Get Data Path
        private static string GetDataPath(string basePath)
        {
            string formatString = @"{0}\{1}\{2}\{3}";
            string dataPath = string.Format(formatString, new object[] { basePath, CompanyName, ProductName, ProductVersion });
            lock (internalSyncObject)
            {
                if (!Directory.Exists(dataPath))
                {
                    Directory.CreateDirectory(dataPath);
                }
            }
            return dataPath;
        }
        #endregion

        #region Product Name
        private static string productName;
        /// <summary>
        /// Gets the product name associated with this application.
        /// </summary>
        /// <value>The product name.</value>
        /// <remarks>ProductName is taken from the metadata of the assembly containing the main form of the current application.
        /// You can set it by setting <see cref="AssemblyProductAttribute"/> inside of your assembly manifest.</remarks>
        public static string ProductName
        { 
            get 
            {
                lock (internalSyncObject)
                {
                    if (productName == null)
                    {
                        Assembly assembly = InTheHand.Reflection.AssemblyInTheHand.GetEntryAssembly();
                        if (assembly != null)
                        {
                            object[] attrs = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                            if ((attrs != null) && (attrs.Length > 0))
                            {
                                productName = ((AssemblyProductAttribute)attrs[0]).Product;
                            }
                        }
                        if (productName == null)
                        {
                            productName = string.Empty;
                        }
                    }
                }
                return productName;

            }
        }
        #endregion

        #region Product Version
        private static string productVersion;
        /// <summary>
        /// Gets the product version associated with this application.
        /// </summary>
        /// <value>The product version.</value>
        /// <remarks>Typically, a version number displays as major number.minor number.build number.private part number. 
        /// You can set it explicitly by setting the <see cref="AssemblyInformationalVersionAttribute"/> within your assembly manifest.</remarks>
        public static string ProductVersion
        {
            get
            {
                lock (internalSyncObject)
                {
                    if (productVersion == null)
                    {
                        Assembly assembly = InTheHand.Reflection.AssemblyInTheHand.GetEntryAssembly();
                        if (assembly != null)
                        {
                            object[] attrs = assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);
                            if ((attrs != null) && (attrs.Length > 0))
                            {
                                productVersion = ((AssemblyInformationalVersionAttribute)attrs[0]).InformationalVersion;
                            }
                        }
                        if (string.IsNullOrEmpty(productVersion))
                        {
                            productVersion = "1.0.0.0";
                        }
                    }
                }
                return productVersion;
            }
        }
        #endregion

        #region Startup Path
        /// <summary>
        /// Gets the path for the executable file that started the application, not including the executable name.
        /// </summary>
        /// <value>The path for the executable file that started the application.</value>
        public static string StartupPath
        {
            get
            {
                return EnvironmentInTheHand.CurrentDirectory;
            }
        }
        #endregion

        #region User App Data Registry
        /// <summary>
        /// Gets the registry key for the application data of a user.
        /// </summary>
        public static RegistryKey UserAppDataRegistry
        {
            get
            {
                return Registry.CurrentUser.CreateSubKey(string.Format(@"Software\{0}\{1}\{2}", new object[] {CompanyName, ProductName, ProductVersion}));
            }
        }
        #endregion

        #region Is Single Instance
        private static IntPtr GetHiddenWindow()
        {
            //need full path of executable
            string appPath = InTheHand.Reflection.AssemblyInTheHand.GetModuleFileName();
            //search for window class e.g. #NETCF_AGL_PARK_\Program Files\AppName\AppName.exe"
            IntPtr appHandle = NativeMethods.FindWindow("#NETCF_AGL_PARK_" + appPath, null);
            return appHandle;
        }

        private static bool isSingleInstance = InTheHand.WindowsCE.Forms.SystemSettingsInTheHand.Platform != WinCEPlatform.WinCEGeneric;
        /// <summary>
        /// Allows you to enforce a single instance application on platforms other than Windows Mobile.
        /// </summary>
        /// <remarks>On generic Windows CE platforms the .NET Compact Framework does not enforce a single instance of your application.
        /// If you want to override this behaviour and provide a behaviour to match Windows Mobile then set this property to true in your startup (e.g. static void Main()) procedure before Application.Run is called.
        /// If a second instance of the application is created it will be closed and the initial instance will be brought to the foreground.</remarks>
        /// <example>
        /// <code lang="cs">
        /// using InTheHand.Windows.Forms;
        /// 
        /// [MTAThread]
        /// static void Main()
        /// {
        ///     ApplicationHelper.IsSingleInstance = true;
        ///     Application.Run(new Form1());
        /// }</code>
        /// </example>
        /// <exception cref="PlatformNotSupportedException">Property is readonly on Windows Mobile (and will always return true).</exception>
        public static bool IsSingleInstance
        {
            get
            {
                return isSingleInstance;
            }
            set
            {
                if (InTheHand.WindowsCE.Forms.SystemSettingsInTheHand.Platform != WinCEPlatform.WinCEGeneric)
                {
                    if (!value)
                    {
                        IntPtr hwnd = GetHiddenWindow();
                        NativeMethods.SetWindowText(hwnd, GuidInTheHand.NewGuid().ToString());
                    }
                }

                isSingleInstance = value;
                if (value)
                {
                    //need full path of executable
                    string appPath = InTheHand.Reflection.AssemblyInTheHand.GetModuleFileName();
                    //search for window class e.g. #NETCF_AGL_PARK_\Program Files\AppName\AppName.exe"
                    IntPtr appHandle = NativeMethods.FindWindow("#NETCF_AGL_PARK_" + appPath, null);

                    if (appHandle != IntPtr.Zero)
                    {
                        //already running - activate
                        NativeMethods.SendMessage(appHandle, 0x8001, 0, 0);
                        //kill this (the second) process
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }
                }
            }
        }
        #endregion

        private static System.Collections.Generic.List<IMessageFilter> filters = new System.Collections.Generic.List<IMessageFilter>();

        /// <summary>
        /// Adds a message filter to monitor Windows messages as they are routed to their destinations.
        /// </summary>
        /// <param name="value">The implementation of the <see cref="IMessageFilter"/> interface you want to install.</param>
        /// <remarks>Use a message filter to prevent specific events from being raised or to perform special operations for an event before it is passed to an event handler.
        /// Message filters are unique to a specific thread.</remarks>
        public static void AddMessageFilter(IMessageFilter value)
        {
            lock (filters)
            {
                filters.Add(value);

                //hook the main window if not already done
                if (nativeWindow.Handle == IntPtr.Zero)
                {
                    nativeWindow.AssignHandle(System.Diagnostics.Process.GetCurrentProcess().GetMainWindowHandle());
                }
            }
        }
        /// <summary>
        /// Removes a message filter from the message pump of the application.
        /// </summary>
        /// <param name="value">The implementation of the <see cref="IMessageFilter"/> to remove from the application.</param>
        /// <remarks>You can remove a message filter when you no longer want to capture Windows messages before they are dispatched.</remarks>
        public static void RemoveMessageFilter(IMessageFilter value)
        {
            lock (filters)
            {
                if(filters.Contains(value))
                {
                    filters.Remove(value);
                }

                if (filters.Count == 0)
                {
                    //no more filters - remove subclass
                    nativeWindow.ReleaseHandle();
                }
            }
        }

        /// <summary>
        /// Runs any filters against a window message, and returns a copy of the modified message.
        /// </summary>
        /// <param name="message">The Windows event message to filter.</param>
        /// <returns>True if the filters were processed; otherwise, false.</returns>
        public static bool FilterMessage(ref Message message)
        {
            foreach (IMessageFilter filter in filters)
            {
                if (filter.PreFilterMessage(ref message))
                    return true;

            }

            return false;
        }

        private static WindowsNativeWindow nativeWindow = new WindowsNativeWindow();

        private class WindowsNativeWindow : NativeWindow
        {
            protected override void WndProc(ref Microsoft.WindowsCE.Forms.Message m)
            {
                foreach (IMessageFilter filter in filters)
                {
                    if (filter.PreFilterMessage(ref m))
                        return;
                    
                }
                base.WndProc(ref m);
            }
        }
    }

    /// <summary>
    /// Defines a message filter interface.
    /// </summary>
    /// <remarks>This interface allows an application to capture a message before it is dispatched to a control or form.</remarks>
    public interface IMessageFilter
    {
        /// <summary>
        /// Filters out a message before it is dispatched.
        /// </summary>
        /// <param name="m">The message to be dispatched. You cannot modify this message. </param>
        /// <returns>true to filter the message and stop it from being dispatched; false to allow the message to continue to the next filter or control.</returns>
        bool PreFilterMessage(ref Message m);
    }
}