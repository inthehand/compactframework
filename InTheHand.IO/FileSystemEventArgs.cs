// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystemEventArgs.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.IO
{
    /// <summary>
    /// Provides data for the directory events: <see cref="FileSystemWatcher.Changed"/>, <see cref="FileSystemWatcher.Created"/>, <see cref="FileSystemWatcher.Deleted"/>.
    /// </summary>
    /// <remarks>The <b>FileSystemEventArgs</b> class is passed as a parameter to event handlers for these events:
    /// <para>The <see cref="FileSystemWatcher.Changed"/> event occurs when changes are made to the size, system attributes, last write time, last access time, or security permissions in a file or directory in the specified <see cref="FileSystemWatcher.Path"/> of a <see cref="FileSystemWatcher"/>.</para>
    /// <para>The <see cref="FileSystemWatcher.Created"/> event occurs when a file or directory in the specified <see cref="FileSystemWatcher.Path"/> of a <see cref="FileSystemWatcher"/> is created.</para>
    /// <para>The <see cref="FileSystemWatcher.Deleted"/> event occurs when a file or directory in the specified <see cref="FileSystemWatcher.Path"/> of a <see cref="FileSystemWatcher"/> is deleted. For more information, see <see cref="FileSystemWatcher"/>.</para></remarks>
    public class FileSystemEventArgs
    {
        private string fullPath = string.Empty;
        private string name = string.Empty;
        internal WatcherChangeTypes changeType = WatcherChangeTypes.All;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemEventArgs"/> class.
        /// </summary>
        /// <param name="changeType">One of the <see cref="WatcherChangeTypes"/> values, which represents the kind of change detected in the file system.</param>
        /// <param name="directory">The root directory of the affected file or directory.</param>
        /// <param name="name">The name of the affected file or directory.</param>
        public FileSystemEventArgs(WatcherChangeTypes changeType, string directory, string name)
            : base()
        {
            this.changeType = changeType;
            this.name = name;
            if (!(directory.EndsWith("\\")))
                directory = directory + "\\";
            this.fullPath = directory + name;
        }

        /// <summary>
        /// Gets the type of directory event that occurred.
        /// </summary>
        /// <value>One of the <see cref="WatcherChangeTypes"/> values that represents the kind of change detected in the file system.</value>
        /// <seealso cref="FileSystemEventArgs"/>
        /// <seealso cref="WatcherChangeTypes"/>
        public WatcherChangeTypes ChangeType
        {
            get
            {
                return changeType;
            }
        }
        /// <summary>
        /// Gets the fully qualifed path of the affected file or directory.
        /// </summary>
        /// <value>The path of the affected file or directory.</value>
        public string FullPath
        {
            get
            {
                return fullPath;
            }
        }
        /// <summary>
        /// Gets the name of the affected file or directory.
        /// </summary>
        /// <value>The name of the affected file or directory.</value>
        public string Name
        {
            get
            {
                return name;
            }
        }
    }
}
