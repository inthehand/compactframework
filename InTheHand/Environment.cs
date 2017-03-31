// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Environment.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand
{
    using System;
    using System.Runtime.InteropServices;

    using Microsoft.Win32;

    /// <summary>
    /// Extends the functionality of <see cref="System.Environment"/>
    /// </summary>
    /// <seealso cref="System.Environment"/>
    public static class EnvironmentInTheHand
    {
        /// <summary>
        /// Gets the newline string defined for this environment.
        /// </summary>
        /// <value>A string containing "\r\n".</value>
        /// <remarks>The property value is a constant customized specifically for the current platform.
        /// This value is automatically appended to text when using WriteLine methods, such as <see cref="M:T:System.Console.WriteLine(System.String)">Console.WriteLine</see>.</remarks>
        /// <seealso cref="System.Environment.NewLine"/>
        public static string NewLine
        {
            get
            {
                return "\r\n";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public const int MaxPath = 260;

        private static string _currentDirectory;
        /// <summary>
        /// Gets the fully qualified path of the current working directory.
        /// </summary>
        /// <value>A string containing a directory path.</value>
        /// <remarks>Under the .NET Compact Framework this value is read-only and determined from the path of the entry assembly</remarks>
        /// <seealso cref="InTheHand.Reflection.AssemblyInTheHand.GetEntryAssembly()"/>
        public static string CurrentDirectory
        {
            get
            {
                if (_currentDirectory == null)
                {
                    _currentDirectory = System.IO.Path.GetDirectoryName(InTheHand.Reflection.AssemblyInTheHand.GetModuleFileName());
                }

                return _currentDirectory;
            }
        }

        /// <summary>
        /// Gets the fully qualified path of the system directory.
        /// </summary>
        /// <value>A string containing a directory path.</value>
        /// <remarks>An example of the value returned is the string "\Windows".</remarks>
        /// <seealso cref="P:System.Environment.SystemDirectory">System.Environment.SystemDirectory Property</seealso>
        public static string SystemDirectory
        {
            get
            {
                try
                {
                    return GetFolderPath(SpecialFolder.Windows);
                }
                catch
                {
                    return "\\Windows";
                }
            }
        }

        /// <summary>
        /// Gets the path to the system special folder identified by the specified enumeration.
        /// </summary>
        /// <param name="folder">An enumerated constant that identifies a system special folder.</param>
        /// <returns>The path to the specified system special folder, if that folder physically exists on your computer; otherwise, the empty string ("").
        /// A folder will not physically exist if the operating system did not create it, the existing folder was deleted, or the folder is a virtual directory, such as My Computer, which does not correspond to a physical path.</returns>
        /// <seealso cref="M:System.Environment.GetFolderPath(System.Environment.SpecialFolder)">System.Environment.GetFolderPath Method</seealso>
        public static string GetFolderPath(SpecialFolder folder)
        {
            System.Text.StringBuilder path = new System.Text.StringBuilder(InTheHand.EnvironmentInTheHand.MaxPath + 1);

            bool success = NativeMethods.SHGetSpecialFolderPath(IntPtr.Zero, path, (int)folder, false);

            return path.ToString();
        }

        private static class NativeMethods
        {
            [DllImport("coredll", EntryPoint = "SHGetSpecialFolderPath")]
            [return:MarshalAs(UnmanagedType.Bool)]
            internal static extern bool SHGetSpecialFolderPath(
                IntPtr hwndOwner, 
                System.Text.StringBuilder lpszPath, 
                int nFolder, 
                [MarshalAs(UnmanagedType.Bool)]
                bool fCreate);
        }

        /// <summary>
        /// Specifies enumerated constants used to retrieve directory paths to system special folders.
        /// </summary>
        /// <remarks>Not all platforms support all of these constants.</remarks>
        /// <seealso cref="T:System.Environment.SpecialFolder">System.Environment.SpecialFolder Enumeration</seealso>
        public enum SpecialFolder
        {
            /// <summary>
            /// The logical Desktop rather than the physical file system location. 
            /// <para><b>Not supported in Windows Mobile.</b></para>
            /// </summary>
            Desktop	= 0x00,
            /// <summary>
            /// The directory that contains the user's program groups.
            /// </summary>
            Programs = 0x02,
            // <summary>
            // control panel icons
            // </summary>
            //Controls		= 0x03,
            // <summary>
            // printers folder
            // </summary>
            //Printers		= 0x04,
            /// <summary>
            /// The directory that serves as a common repository for documents.
            /// </summary>
            Personal = 0x05,
            /// <summary>
            /// The "My Documents" folder.
            /// </summary>
            MyDocuments = 0x5,
            /// <summary>
            /// The directory that serves as a common repository for the user's favorite items.
            /// </summary>
            Favorites = 0x06,
            /// <summary>
            /// The directory that corresponds to the user's Startup program group.
            /// The system starts these programs whenever a user starts Windows CE.
            /// </summary>
            Startup = 0x07,
            /// <summary>
            /// The directory that contains the user's most recently used documents.
            /// <para><b>Not supported in Windows Mobile.</b></para>
            /// </summary>
            Recent = 0x08,
            /// <summary>
            /// The directory that contains the Send To menu items.
            /// <para><b>Not supported in Windows Mobile.</b></para>
            /// </summary>
            SendTo			= 0x09,
            // <summary>
            // Recycle bin.
            // </summary>
            //RecycleBin		= 0x0A,
            /// <summary>
            /// The directory that contains the Start menu items.
            /// </summary>
            StartMenu = 0x0B,
            /// <summary>
            /// The "My Music" folder.
            /// <para><b>Requires Windows Mobile</b></para>
            /// </summary>
            MyMusic = 0xd,
            /// <summary>
            /// The file system directory that serves as a repository for videos that belong to a user.
            /// <para><b>Requires Windows Mobile</b></para>
            /// </summary>
            MyVideo = 0xe,
            /// <summary>
            /// The directory used to physically store file objects on the desktop.
            /// Do not confuse this directory with the desktop folder itself, which is a virtual folder.
            /// <para><b>Not supported in Windows Mobile.</b></para>
            /// </summary>
            DesktopDirectory = 0x10,
            // <summary>
            // The "My Computer" folder.
            // </summary>
            //MyComputer		= 0x11,
            // <summary>
            // Network Neighbourhood
            // <para><b>Not supported in Windows Mobile.</b></para></summary>
            //NetworkNeighborhood = 0x12,
            /// <summary>
            /// The Fonts folder.
            /// </summary>
            Fonts = 0x14,
            /// <summary>
            /// The directory that serves as a common repository for application-specific data for the current user.
            /// </summary>
            ApplicationData = 0x1a,
            /// <summary>
            /// The Windows folder.
            /// </summary>
            Windows = 0x24,
            /// <summary>
            /// The program files directory.
            /// </summary>
            ProgramFiles = 0x26,
            /// <summary>
            /// The "My Pictures" folder.
            /// <para><b>Requires Windows Mobile</b></para> 
            /// </summary>
            MyPictures = 0x27,
        }

        /// <summary>
        /// Returns an array of string containing the names of the logical drives on the current computer.
        /// </summary>
        /// <returns>An array of string where each element contains the name of a logical drive.</returns>
        /// <example>The following example shows how to display the logical drives of the current computer using the GetLogicalDrives method.
        /// <code lang="vb">
        /// ' Sample for the EnvironmentHelper.GetLogicalDrives method
        /// Imports System
        /// Imports InTheHand
        /// 
        /// Class Sample
        ///     Public Shared Sub Main()
        ///         System.Diagnostics.Debug.WriteLine()
        ///         Dim drives As [String]() = EnvironmentHelper.GetLogicalDrives()
        ///         System.Diagnostics.Debug.WriteLine("GetLogicalDrives: {0}", [String].Join(", ", drives))
        ///     End Sub 'Main
        /// End Class 'Sample
        /// '
        /// 'This example produces the following results:
        /// '
        /// 'GetLogicalDrives: \, Storage Card
        /// '
        /// </code>
        /// <code lang="cs">
        /// // Sample for the EnvironmentHelper.GetLogicalDrives method
        /// using System;
        /// using InTheHand;
        /// 
        /// class Sample 
        /// {
        ///     public static void Main() 
        ///     {
        ///         System.Diagnostics.Debug.WriteLine();
        ///         String[] drives = EnvironmentHelper.GetLogicalDrives();
        ///         System.Diagnostics.Debug.WriteLine("GetLogicalDrives: {0}", String.Join(", ", drives));
        ///     }
        /// }
        /// /*
        /// This example produces the following results:
        /// 
        /// GetLogicalDrives: \, Storage Card
        /// */
        /// </code>
        /// </example>
        public static string[] GetLogicalDrives()
        {
            // storage cards are directories with the temporary attribute

            System.Collections.Generic.List<string> drives = new System.Collections.Generic.List<string>();

            string root = "\\";
            drives.Add(root);

            System.IO.DirectoryInfo rootDir = new System.IO.DirectoryInfo(root);
            foreach (System.IO.DirectoryInfo di in rootDir.GetDirectories())
            {
                // if directory and temporary
                if (di.Attributes.HasFlag(System.IO.FileAttributes.Temporary))
                {
                    // add to collection of storage cards
                    drives.Add(root + di.Name);
                }
            }

            return drives.ToArray();
        }

        /// <summary>
        /// Gets the name of this local device.
        /// </summary>
        /// <value>A string containing the name of this computer.</value>
        public static string MachineName
        {
            get
            {
                string machineName = string.Empty;

                try
                {
                    RegistryKey ident = Registry.LocalMachine.OpenSubKey("Ident");
                    machineName = ident.GetValue("Name").ToString();
                    ident.Close();
                }
                catch
                {
                    throw new PlatformNotSupportedException();
                }

                return machineName;
            }
        }

        private static string userName = string.Empty;
        /// <summary>
        /// Gets the user name of the person who started the current thread.
        /// </summary>
        /// <remarks>Supported only on Windows Mobile platforms.</remarks>
        public static string UserName
        {
            get
            {
                if (String.IsNullOrEmpty(userName))
                {


                    try
                    {
                        RegistryKey ownerKey = Registry.CurrentUser.OpenSubKey("ControlPanel\\Owner");
                        if (ownerKey != null)
                        {
                            //new style string registry key = smartphone and v5
                            userName = (string)ownerKey.GetValue("Name", string.Empty);
                            ownerKey.Close();
                        }

                        if (userName != string.Empty)
                        {
                            return userName;
                        }

                        //old style pocket pc bytes
                        byte[] ownerData = (byte[])ownerKey.GetValue("Owner", null);
                        if (ownerData != null)
                        {
                            userName = System.Text.Encoding.Unicode.GetString(ownerData, 0, 72);
                            int nullIndex = userName.IndexOf("\0");
                            if (nullIndex > -1)
                            {
                                userName = userName.Substring(0, nullIndex);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                return userName;
            }
        }


        /// <summary>
        /// Gets current stack trace information.
        /// </summary>
        /// <value>A <see cref="String"/> containing stack trace information.
        /// This value can be <see cref="System.String.Empty"/>.</value>
        /// <remarks>Equivalent to System.Environment.StackTrace</remarks>
        /// <seealso cref="System.Environment"/>
        public static string StackTrace
        {
            get
            {
                string trace = string.Empty;

                try
                {
                    throw new Exception();
                }
                catch(Exception ex)
                {
                    trace = ex.StackTrace;
                }

                return trace;
            }
        }
    }
}