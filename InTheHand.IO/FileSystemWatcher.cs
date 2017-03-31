// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystemWatcher.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.IO
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Listens to the file system change notifications and raises events when a directory, or file in a directory, changes.
    /// </summary>
    /// <remarks><para>Use FileSystemWatcher to watch for changes in a specified directory.
    /// You can watch for changes in files and subdirectories of the specified directory.
    /// You can create a component to watch files on a local computer, a network drive, or a remote computer.</para>
    /// <para>To watch for changes in all files, set the <see cref="Filter"/> property to an empty string ("") or use wildcards ("*.*").
    /// To watch a specific file, set the Filter property to the file name.
    /// For example, to watch for changes in the file MyDoc.txt, set the <see cref="Filter"/> property to "MyDoc.txt".
    /// You can also watch for changes in a certain type of file.
    /// For example, to watch for changes in text files, set the <see cref="Filter"/> property to "*.txt".</para>
    /// <para>There are several types of changes you can watch for in a directory or file.
    /// For example, you can watch for changes in <see cref="NotifyFilters.Attributes"/>, the <see cref="NotifyFilters.LastWrite"/> date and time, or the <see cref="NotifyFilters.Size"/> of files or directories.
    /// This is done by setting the <see cref="NotifyFilter"/> property to one of the <see cref="NotifyFilters"/> values.
    /// For more information on the type of changes you can watch, see <see cref="NotifyFilters"/>.</para>
    /// <para>You can watch for renaming, deletion, or creation of files or directories.
    /// For example, to watch for renaming of text files, set the Filter property to "*.txt" and handle the Renamed event.</para>
    /// For a list of initial property values for an instance of FileSystemWatcher, see the FileSystemWatcher constructor.
    /// Please note that Hidden files are not ignored.
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 2003 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows Embedded Compact 4.2 and later</description></item>
    /// </list>
    /// </remarks>
    public sealed class FileSystemWatcher : System.ComponentModel.Component, IDisposable
    {
        private static readonly char[] wildcards;

        static FileSystemWatcher()
        {
            FileSystemWatcher.wildcards = new char[] { '?', '*' };
        }

        private string filter = "*.*";
        private bool enabled = false;
        private bool includeSubdirectories = false;
        private NotifyFilters notifyFilters = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.DirectoryName;
        private FileSystemMessageWindow messageWindow;
        private string directory = string.Empty;

        /// <summary>
        /// Occurs when a file or directory in the specified <see cref="FileSystemWatcher.Path"/> is created.
        /// </summary>
        public event FileSystemEventHandler Created;

        /// <summary>
        /// Occurs when a file or directory in the specified <see cref="FileSystemWatcher.Path"/> is changed.
        /// </summary>
        public event FileSystemEventHandler Changed;

        /// <summary>
        /// Occurs when a file or directory in the specified <see cref="FileSystemWatcher.Path"/> is deleted.
        /// </summary>
        public event FileSystemEventHandler Deleted;

        /// <summary>
        /// Occurs when a file or directory in the specified <see cref="FileSystemWatcher.Path"/> is renamed.
        /// </summary>
        public event RenamedEventHandler Renamed;


        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemWatcher"/> class.
        /// </summary>
        public FileSystemWatcher()
        {
            messageWindow = new FileSystemMessageWindow(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemWatcher"/> class, given the specified directory to monitor.
        /// </summary>
        /// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
        public FileSystemWatcher(string path) : this()
        {
            this.Path = path;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemWatcher"/> class, given the specified directory and type of files to monitor.
        /// </summary>
        /// <param name="path">The directory to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
        /// <param name="filter">The type of files to watch.
        /// For example, "*.txt" watches for changes to all text files.</param>
        public FileSystemWatcher(string path, string filter) : this(path)
        {
            this.Filter = filter;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the component is enabled.
        /// </summary>
        /// <value>true if the component is enabled; otherwise, false.
        /// The default is false.</value>
        public bool EnableRaisingEvents
        {
            get
            {
                return this.enabled;
            }
            set
            {
                if (enabled != value)
                {
                    if (!enabled)
                    {
                        StartRaisingEvents();
                    }
                    else
                    {
                        StopRaisingEvents();
                    }

                    enabled = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether subdirectories within the specified path should be monitored.
        /// </summary>
        /// <value>true if you want to monitor subdirectories; otherwise, false.
        /// The default is false.</value>
        /// <remarks>Set IncludeSubdirectories to true when you want to watch for change notifications for files and directories contained within the directory specified through the Path property, and its subdirectories.
        /// Setting the IncludeSubdirectories property to false helps reduce the number of notifications.</remarks>
        public bool IncludeSubdirectories
        {
            get
            {
                return includeSubdirectories;

            }
            set
            {
                if (this.includeSubdirectories != value)
                {
                    this.includeSubdirectories = value;
                    this.Restart();
                }
            }
        }

        /// <summary>
        /// Gets or sets the type of changes to watch for.
        /// </summary>
        /// <value>One of the NotifyFilters values.
        /// The default is the bitwise OR combination of <see cref="NotifyFilters.LastWrite"/>, <see cref="NotifyFilters.FileName"/>, and <see cref="NotifyFilters.DirectoryName"/>.</value>
        /// <remarks>You can combine the members of the <see cref="NotifyFilters"/> enumeration to watch for more than one type of change at a time.
        /// For example, you can watch for changes in size of a file, and for changes in the LastWrite time.
        /// This raises an event anytime there is a change in file or folder size, or a change in the LastWrite time of the file or folder.</remarks>
        public NotifyFilters NotifyFilter
        {
            get
            {
                return notifyFilters;
            }
            set
            {
                if (value != notifyFilters)
                {
                    notifyFilters = value;
                    Restart();
                }
            }
        }

        /// <summary>
        /// Gets or sets the path of the directory to watch.
        /// </summary>
        /// <value>The path to monitor.
        /// The default is an empty string ("").</value>
        public string Path
        {
            get
            {
                return directory;
            }
            set
            {
                value = (value == null) ? string.Empty : value;

                if (string.Compare(this.directory, value, true) != 0)
                {
                    if ((value.IndexOfAny(FileSystemWatcher.wildcards) != -1) || (value.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1))
                        {
                            throw new ArgumentException(String.Format(Properties.Resources.InvalidDirName, "value"));
                        }
                    
                    if (!Directory.Exists(value))
                    {
                        throw new ArgumentException(String.Format(Properties.Resources.InvalidDirName, "value"));
                    }

                    this.directory = value;
                    this.Restart();
                }
            }
        }

        /// <summary>
        /// Gets or sets the filter string, used to determine what files are monitored in a directory.
        /// </summary>
        public string Filter
        {
            get
            {
                return filter;
            }
            set
            {
                if (value == null || value == String.Empty)
                {
                    value = "*.*";
                }

                if (String.Compare(this.filter, value, true) != 0)
                {
                    this.filter = value;
                }
            }
        }

        #region Start Stop
        private void StartRaisingEvents()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(base.GetType().Name);
            }
            NativeMethods.SHCHANGENOTIFYENTRY notEntry = new NativeMethods.SHCHANGENOTIFYENTRY();

            //Set mask
            notEntry.dwEventMask = NativeMethods.SHCNE.ALLEVENTS;

            notEntry.fRecursive = includeSubdirectories;

            //Set watch dir
            if (directory != string.Empty)
            {
                notEntry.pszWatchDir = directory + "\0";
            }

            //Call API
            bool success = NativeMethods.SHChangeNotifyRegister(messageWindow.Hwnd, ref notEntry);

            if (!success)
            {
                throw InTheHand.ComponentModel.Win32ExceptionInTheHand.Create();
            }
        }

        private void StopRaisingEvents()
        {
            NativeMethods.SHChangeNotifyDeregister(messageWindow.Hwnd);

            this.enabled = false;
        }

        private void Restart()
        {
            if(this.enabled)
            {
                this.StopRaisingEvents();
                this.StartRaisingEvents();
            }
        }
        #endregion

        //Destructor
        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the <see cref="FileSystemWatcher"/> is reclaimed by garbage collection.
        /// </summary>
        ~FileSystemWatcher()
        {
            this.Dispose(true);
        }

        #region Event Raisers

        private void OnChanged(FileSystemEventArgs e)
        {
            lock (this)
            {
                if (this.Changed != null)
                {
                    this.Changed(this, e);
                }
            }
        }

        private void OnCreated(FileSystemEventArgs e)
        {
            lock (this)
            {
                if (this.Created != null)
                    this.Created(this, e);
            }
        }

        private void OnDeleted(FileSystemEventArgs e)
        {
            lock (this)
            {
                if (this.Deleted != null)
                    this.Deleted(this, e);
            }
        }

        private void OnRenamed(RenamedEventArgs e)
        {
            lock (this)
            {
                if (this.Renamed != null)
                    this.Renamed(this, e);
            }
        }


        #endregion

        private static class NativeMethods
        {
            [DllImport("aygshell")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool SHChangeNotifyRegister(IntPtr hwnd, ref SHCHANGENOTIFYENTRY pshcne);

            [DllImport("aygshell")]
            internal static extern void SHChangeNotifyFree(IntPtr pshcne);

            [DllImport("aygshell")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool SHChangeNotifyDeregister(IntPtr hwnd);

            [StructLayout(LayoutKind.Sequential)]
            internal struct SHCHANGENOTIFYENTRY
            {
                internal SHCNE dwEventMask;
                [MarshalAs(UnmanagedType.LPWStr)]
                internal string pszWatchDir;
                [MarshalAs(UnmanagedType.Bool)]
                internal bool fRecursive;
            }

            [Flags()]
            internal enum SHCNE : int
            {
                RENAMEITEM = 0x00000001,
                CREATE = 0x00000002,
                DELETE = 0x00000004,
                MKDIR = 0x00000008,
                RMDIR = 0x00000010,
                MEDIAINSERTED = 0x00000020,
                MEDIAREMOVED = 0x00000040,
                /*DRIVEREMOVED        =0x00000080,
                DRIVEADD            =0x00000100,*/
                NETSHARE = 0x00000200,
                NETUNSHARE = 0x00000400,
                ATTRIBUTES = 0x00000800,
                UPDATEDIR = 0x00001000,
                UPDATEITEM = 0x00002000,
                //SERVERDISCONNECT    =0x00004000,
                //UPDATEIMAGE         =0x00008000,
                //DRIVEADDGUI         =0x00010000,
                RENAMEFOLDER = 0x00020000,

                //ASSOCCHANGED        =0x08000000,

                //DISKEVENTS          =0x0002381F,
                //GLOBALEVENTS        =0x0C0181E0, // Events that dont match pidls first
                ALLEVENTS = 0x7FFFFFFF
            }

            [Flags()]
            internal enum SHCNF : int
            {
                IDLIST = 0x0000,	// LPITEMIDLIST
                PATH = 0x0001,	// path name
                //PRINTER   = 0x0002,	// printer friendly name
                //DWORD     = 0x0003,	// DWORD
                /*TYPE      = 0x00FF,
                FLUSH       = 0x1000,
                FLUSHNOWAIT = 0x2000,*/
            }

            internal const int WM_FILECHANGEINFO = (0x8000 + 0x101);


            [StructLayout(LayoutKind.Sequential)]
            internal struct FILECHANGENOTIFY
            {
                internal int dwRefCount;
                internal int cbSize;
                internal SHCNE wEventId;
                internal SHCNF uFlags;
                [MarshalAs(UnmanagedType.LPWStr)]
                internal string dwItem1;
                [MarshalAs(UnmanagedType.LPWStr)]
                internal string dwItem2;
                internal int dwAttributes;
                internal int dwLowDateTime;
                internal int dwHighDateTime;
                internal uint nFileSize;
            }
        }


        #region IDisposable Members

        private bool disposed = false;

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="FileSystemWatcher"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                if (enabled)
                    this.StopRaisingEvents();

            }
            disposed = true;
        }

        #endregion

        #region MessageWindow

        internal class FileSystemMessageWindow : Microsoft.WindowsCE.Forms.MessageWindow
        {

            private FileSystemWatcher watcher;

            public FileSystemMessageWindow(FileSystemWatcher w)
            {
                watcher = w;
            }

            //helper function
            private bool CheckFilter(string fileName)
            {

                if (watcher.filter != "*.*")
                {
                    string filterName = System.IO.Path.GetFileNameWithoutExtension(watcher.filter);
                    string filterExt = System.IO.Path.GetExtension(watcher.filter);

                    if (filterName == "*") //check ext only
                    {
                        if (System.IO.Path.GetExtension(fileName).ToLower() == filterExt.ToLower())
                        {
                            return true;
                        }
                    }
                    else //we've got name in the filter
                    {
                        if (filterExt == ".*") //star in the ext
                        {
                            if (System.IO.Path.GetFileNameWithoutExtension(fileName).ToLower() == filterName.ToLower())
                            {
                                return true;
                            }
                        }
                        else //name and ext supplied
                        {
                            if ((System.IO.Path.GetFileNameWithoutExtension(fileName).ToLower() == filterName.ToLower()) && (System.IO.Path.GetExtension(fileName).ToLower() == filterExt.ToLower()))
                            {
                                return true;
                            }
                        }
                    }

                    return false;
                }
                return true;
            }


            protected override void WndProc(ref Microsoft.WindowsCE.Forms.Message msg)
            {

                if (msg.Msg == NativeMethods.WM_FILECHANGEINFO)
                {
                    if (msg.LParam == IntPtr.Zero)
                        return;

                    NativeMethods.FILECHANGENOTIFY fchnot = (NativeMethods.FILECHANGENOTIFY)Marshal.PtrToStructure(msg.LParam, typeof(NativeMethods.FILECHANGENOTIFY));

                    string fullPath = fchnot.dwItem1;
                    string newfullPath = fchnot.dwItem2;

                    string fileName = System.IO.Path.GetFileName(fullPath);
                    string dirName = System.IO.Path.GetDirectoryName(fullPath);
                    string newfileName = string.Empty;
                    if (fchnot.dwItem2 != null)
                    {
                        newfileName = System.IO.Path.GetFileName(fchnot.dwItem2);
                    }


                    if (CheckFilter(fileName))
                    {
                        FileSystemEventArgs args;
                        RenamedEventArgs renArgs;

                        switch (fchnot.wEventId)
                        {
                            case NativeMethods.SHCNE.CREATE:
                                if ((watcher.notifyFilters & NotifyFilters.FileName) == NotifyFilters.FileName)
                                {
                                    args = new FileSystemEventArgs(WatcherChangeTypes.Created, dirName, fileName);
                                    watcher.OnCreated(args);
                                }
                                break;
                            case NativeMethods.SHCNE.MKDIR:
                                if ((watcher.notifyFilters & NotifyFilters.DirectoryName) == NotifyFilters.DirectoryName)
                                {
                                    args = new FileSystemEventArgs(WatcherChangeTypes.Created, dirName, fileName);
                                    watcher.OnCreated(args);
                                }
                                break;
                            case NativeMethods.SHCNE.UPDATEDIR:
                                if ((watcher.notifyFilters & NotifyFilters.DirectoryName) == NotifyFilters.DirectoryName)
                                {
                                    args = new FileSystemEventArgs(WatcherChangeTypes.Changed, dirName, fileName);
                                    watcher.OnChanged(args);
                                }
                                break;
                            case NativeMethods.SHCNE.RMDIR:
                                if ((watcher.notifyFilters & NotifyFilters.DirectoryName) == NotifyFilters.DirectoryName)
                                {
                                    args = new FileSystemEventArgs(WatcherChangeTypes.Deleted, dirName, fileName);
                                    watcher.OnDeleted(args);
                                }
                                break;
                            case NativeMethods.SHCNE.DELETE:
                                if ((watcher.notifyFilters & NotifyFilters.FileName) == NotifyFilters.FileName)
                                {
                                    args = new FileSystemEventArgs(WatcherChangeTypes.Deleted, dirName, fileName);
                                    watcher.OnDeleted(args);
                                }
                                break;
                            case NativeMethods.SHCNE.UPDATEITEM:
                                if ((watcher.notifyFilters & NotifyFilters.FileName) == NotifyFilters.FileName)
                                {
                                    args = new FileSystemEventArgs(WatcherChangeTypes.Changed, dirName, fileName);
                                    watcher.OnChanged(args);
                                }
                                break;
                            case NativeMethods.SHCNE.RENAMEFOLDER:
                                if ((watcher.notifyFilters & NotifyFilters.DirectoryName) == NotifyFilters.DirectoryName)
                                {
                                    renArgs = new RenamedEventArgs(WatcherChangeTypes.Renamed, dirName, newfileName, fileName);
                                    watcher.OnRenamed(renArgs);
                                }
                                break;
                            case NativeMethods.SHCNE.RENAMEITEM:
                                if ((watcher.notifyFilters & NotifyFilters.FileName) == NotifyFilters.FileName)
                                {
                                    renArgs = new RenamedEventArgs(WatcherChangeTypes.Renamed, dirName, newfileName, fileName);
                                    watcher.OnRenamed(renArgs);
                                }
                                break;

                            case NativeMethods.SHCNE.ATTRIBUTES:
                                if ((watcher.notifyFilters & NotifyFilters.Attributes) == NotifyFilters.Attributes)
                                {
                                    args = new FileSystemEventArgs(WatcherChangeTypes.Changed, dirName, fileName);
                                    watcher.OnChanged(args);
                                }
                                break;
                        }
                    }

                    NativeMethods.SHChangeNotifyFree(msg.LParam);
                }

                msg.Result = IntPtr.Zero;
                base.WndProc(ref msg);
            }

        }

        #endregion
    }

    /// <summary>
    /// Represents the method that will handle the <see cref="FileSystemWatcher.Changed"/>, <see cref="FileSystemWatcher.Created"/>, or <see cref="FileSystemWatcher.Deleted"/> event of a <see cref="FileSystemWatcher"/> class.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="FileSystemEventArgs"/> that contains the event data.</param>
    /// <seealso cref="FileSystemEventArgs"/>
    /// <seealso cref="RenamedEventArgs"/>
    public delegate void FileSystemEventHandler(object sender, FileSystemEventArgs e);

    /// <summary>
    /// Represents the method that will handle the <see cref="FileSystemWatcher.Renamed"/> event of a <see cref="FileSystemWatcher"/> class.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RenamedEventArgs"/> that contains the event data.</param>
    /// <seealso cref="RenamedEventArgs"/>
    /// <seealso cref="FileSystemEventHandler"/>
    /// <seealso cref="FileSystemEventArgs"/>
    public delegate void RenamedEventHandler(object sender, RenamedEventArgs e);
}