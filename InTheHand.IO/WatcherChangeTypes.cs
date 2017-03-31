// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WatcherChangeTypes.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.IO
{
    /// <summary>
    /// Changes that might occur to a file or directory.
    /// </summary>
    /// <remarks>Each <see cref="WatcherChangeTypes"/> member is associated with an event in <see cref="FileSystemWatcher"/>.
    /// For more information on the events, see <see cref="FileSystemWatcher.Changed"/>, <see cref="FileSystemWatcher.Created"/>, <see cref="FileSystemWatcher.Deleted"/> and <see cref="FileSystemWatcher.Renamed"/>.</remarks>
    public enum WatcherChangeTypes
    {
        /// <summary>
        /// The creation of a file or folder.
        /// </summary>
        Created = 1,

        /// <summary>
        /// The deletion of a file or folder.
        /// </summary>
        Deleted = 2,

        /// <summary>
        /// The change of a file or folder. The types of changes include: changes to size, attributes, security settings, last write, and last access time.
        /// </summary>
        Changed = 4,

        /// <summary>
        /// The renaming of a file or folder.
        /// </summary>
        Renamed = 8,

        /// <summary>
        /// The creation, deletion, change, or renaming of a file or folder. 
        /// </summary>
        All = 15,
    }
}
