// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenamedEventArgs.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.IO
{
    /// <summary>
    /// Provides data for the Renamed event.
    /// </summary>
    public sealed class RenamedEventArgs : FileSystemEventArgs
    {
        private string oldFullPath;
        private string oldName;

        /// <summary>
        /// Initializes a new instance of the RenamedEventArgs class.
        /// </summary>
        /// <param name="changeType">One of the WatcherChangeTypes values.</param>
        /// <param name="directory">The name of the affected file or directory.</param>
        /// <param name="name">The name of the affected file or directory.</param>
        /// <param name="oldName">The old name of the affected file or directory.</param>
        public RenamedEventArgs(WatcherChangeTypes changeType, string directory, string name, string oldName)
            : base(changeType, directory, name)
        {
            if (!directory.EndsWith("\\"))
            {
                directory = directory + "\\";
            }

            this.oldFullPath = directory + oldName;
            this.oldName = oldName;
        }

        /// <summary>
        /// Gets the previous fully qualified path of the affected file or directory.
        /// </summary>
        public string OldFullPath
        {
            get
            {
                return oldFullPath;
            }
        }

        /// <summary>
        /// Gets the old name of the affected file or directory.
        /// </summary>
        public string OldName
        {
            get
            {
                return oldName;
            }
        }
    }
}
