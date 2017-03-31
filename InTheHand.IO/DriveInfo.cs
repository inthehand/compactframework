// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DriveInfo.cs" company="In The Hand Ltd">
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
    /// Provides access to information on a drive.
    /// </summary>
    /// <remarks><para>Equivalent to System.IO.DriveInfo</para>This class models a drive and provides methods and properties to query for drive information.
    /// Use <see cref="DriveInfo"/> to determine what drives are available, and the capacity and available free space on the drive.</remarks>
    /// <example>The following code example demonstrates the use of the DriveInfo class to display information about all of the drives on the current system
    /// <code lang="vbnet">
    /// Imports System
    /// Imports System.IO
    /// 
    /// Class Test
    ///     Public Shared Sub Main()
    ///         Dim allDrives() As DriveInfo = DriveInfo.GetDrives()
    ///         
    ///         Dim d As DriveInfo
    ///         For Each d In allDrives
    ///         Debug.WriteLine("Drive {0}", d.Name)
    ///         Debug.WriteLine("  Available space to current user:{0, 15} bytes", _
    ///             d.AvailableFreeSpace)
    ///
    ///         Debug.WriteLine("  Total available space:          {0, 15} bytes", _
    ///             d.TotalFreeSpace)
    ///
    ///         Debug.WriteLine("  Total size of drive:            {0, 15} bytes ", _
    ///             d.TotalSize)
    ///         Next
    ///     End Sub
    /// End Class
    /// 
    /// 'This code produces output similar to the following:
    /// '
    /// 'Drive \
    /// '  Available space to current user:     4770430976 bytes
    /// '  Total available space:               4770430976 bytes
    /// '  Total size of drive:                10731683840 bytes 
    /// 'Drive \Storage Card
    /// '  Available space to current user:    15114977280 bytes
    /// '  Total available space:              15114977280 bytes
    /// '  Total size of drive:                25958948864 bytes 
    /// '
    /// 'The actual output of this code will vary based on machine and the permissions
    /// 'granted to the user executing it.
    /// </code></example>
    public sealed class DriveInfo
    {
        private string root;

        /// <summary>
        /// Provides access to information on the specified drive.
        /// </summary>
        /// <param name="driveName"></param>
        /// <remarks>Use this class to obtain information on drives.
        /// The drive name must be a valid Windows CE volume path e.g. "\Storage Card".
        /// You cannot use this method to obtain information on drive names that are a null reference (Nothing in Visual Basic) or use UNC (\\server\share) paths.</remarks>
        /// <exception cref="ArgumentNullException">The drive name cannot be a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">driveName does not refer to a valid drive.</exception>
        public DriveInfo(string driveName)
        {
            if (string.IsNullOrEmpty(driveName))
            {
                throw new ArgumentNullException("driveName");
            }

            this.root = driveName;

            bool success = NativeMethods.GetDiskFreeSpaceEx(driveName, ref available, ref size, ref total);

            if (!success)
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Gets the root directory of a drive.
        /// </summary>
        /// <value>A <see cref="DirectoryInfo"/> object that contains the root directory of the drive.</value>
        public DirectoryInfo RootDirectory
        {
            get
            {
                return new DirectoryInfo(root);
            }
        }

        private long available;
        /// <summary>
        /// Indicates the amount of available free space on a drive.
        /// </summary>
        /// <value>The amount of free space available on the drive, in bytes.</value>
        public long AvailableFreeSpace
        {
            get
            {
                return available;
            }
        }

        private long total;
        /// <summary>
        /// Gets the total amount of free space available on a drive.
        /// </summary>
        /// <value>The total free space available on a drive, in bytes.</value>
        public long TotalFreeSpace
        {
            get
            {
                return total;
            }
        }

        private long size;
        /// <summary>
        /// Gets the total size of storage space on a drive.
        /// </summary>
        /// <value>The total size of the drive, in bytes.</value>
        public long TotalSize
        {
            get
            {
                return size;
            }
        }

        /// <summary>
        /// Returns a drive name as a string.
        /// </summary>
        /// <returns>The name of the drive.</returns>
        public override string ToString()
        {
            return root;
        }

        /// <summary>
        /// Retrieves the drive names of all logical drives on a device.
        /// </summary>
        /// <returns>An array of type <see cref="DriveInfo"/> that represents the logical drives on a device.</returns>
        /// <remarks>This method retrieves all logical drive names on a computer. 
        /// You can use this information to iterate through the array and obtain information on the drives using other <see cref="DriveInfo"/> methods and properties.</remarks>
        public static DriveInfo[] GetDrives()
        {
            
            //get all the drive names
            string[] strDrives = EnvironmentInTheHand.GetLogicalDrives();

            //create array of driveinfos and fill
            DriveInfo[] drives = new DriveInfo[strDrives.Length];
            for (int i = 0; i < strDrives.Length; i++)
            {
                drives[i] = new DriveInfo(strDrives[i]);
            }

            return drives;
        }

        private static class NativeMethods
        {
            [DllImport("coredll", EntryPoint = "GetDiskFreeSpaceEx")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetDiskFreeSpaceEx(string directoryName, ref long freeBytesAvailable, ref long totalBytes, ref long totalFreeBytes);
        }
    }
}