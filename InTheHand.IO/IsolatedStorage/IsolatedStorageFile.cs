// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsolatedStorageFile.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.IO.IsolatedStorage
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Represents an isolated storage area containing files and directories.
    /// </summary>
    /// <remarks>This object corresponds to a specific isolated storage scope, where files represented by IsolatedStorageFileStream objects exist.
    /// Applications can use isolated storage to save data in their own isolated portion of the file system, without having to specify a particular path within the file system.</remarks>
    public sealed class IsolatedStorageFile : IDisposable
    {
        /// <summary>
        /// Obtains user-scoped isolated storage for use by an application that calls from the virtual host domain.
        /// </summary>
        /// <returns>The isolated storage file that corresponds to the isolated storage scope based on the identity of an application in a virtual host domain.</returns>
        public static IsolatedStorageFile GetUserStoreForApplication()
        {

            return new IsolatedStorageFile();
        }

        private string _basePath = null;

        internal string BasePath
        {
            get
            {
                if (_basePath == null)
                {
                    try
                    {
                        _basePath = Path.Combine(EnvironmentInTheHand.CurrentDirectory, "__is");
                    }
                    catch
                    {
                        throw new IsolatedStorageException();
                    }
                }

                return _basePath;
            }
        }

        private IsolatedStorageFile()
        {
            if (!Directory.Exists(BasePath))
            {
                DirectoryInfo di = Directory.CreateDirectory(BasePath);
                di.Attributes |= FileAttributes.Hidden | FileAttributes.System | FileAttributes.Encrypted;
            }
        }

        /// <summary>
        /// Copies an existing file to a new file. 
        /// </summary>
        /// <param name="sourceFileName">The name of the file to copy.</param>
        /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
        /// <exception cref="ArgumentException">sourceFileName or destinationFileName is a zero-length string, contains only white space, or contains one or more invalid characters defined by the Path.GetInvalidPathChars method.</exception>
        /// <exception cref="ArgumentNullException">sourceFileName or destinationFileName is null.</exception>
        /// <exception cref="ObjectDisposedException">The isolated store has been disposed.</exception>
        /// <exception cref="FileNotFoundException">sourceFileName was not found.</exception>
        /// <exception cref="IsolatedStorageException">An I/O error has occurred.</exception>
        public void CopyFile(string sourceFileName, string destinationFileName)
        {
            if (sourceFileName == null)
            {
                throw new ArgumentNullException("sourceFileName");
            }

            if (destinationFileName == null)
            {
                throw new ArgumentNullException("destinationFileName");
            }

            if (sourceFileName.Trim().Length == 0)
            {
                throw new ArgumentException(Properties.Resources.Argument_EmptyPath, "sourceFileName");
            }

            if (destinationFileName.Trim().Length == 0)
            {
                throw new ArgumentException(Properties.Resources.Argument_EmptyPath, "destinationFileName");
            }

            this.CopyFile(sourceFileName, destinationFileName, false);
        }

        /// <summary>
        /// Copies an existing file to a new file, and optionally overwrites an existing file.
        /// </summary>
        /// <param name="sourceFileName">The name of the file to copy.</param>
        /// <param name="destinationFileName">The name of the destination file. This cannot be a directory.</param>
        /// <param name="overwrite">true if the destination file can be overwritten; otherwise, false.</param>
        /// <exception cref="ArgumentException">sourceFileName or destinationFileName is a zero-length string, contains only white space, or contains one or more invalid characters defined by the Path.GetInvalidPathChars method.</exception>
        /// <exception cref="ArgumentNullException">sourceFileName or destinationFileName is null.</exception>
        /// <exception cref="ObjectDisposedException">The isolated store has been disposed.</exception>
        /// <exception cref="FileNotFoundException">sourceFileName was not found.</exception>
        /// <exception cref="IsolatedStorageException">An I/O error has occurred.</exception>
        public void CopyFile(string sourceFileName, string destinationFileName, bool overwrite)
        {
            if (sourceFileName == null)
            {
                throw new ArgumentNullException("sourceFileName");
            }

            if (destinationFileName == null)
            {
                throw new ArgumentNullException("destinationFileName");
            }

            if (sourceFileName.Trim().Length == 0)
            {
                throw new ArgumentException(Properties.Resources.Argument_EmptyPath, "sourceFileName");
            }

            if (destinationFileName.Trim().Length == 0)
            {
                throw new ArgumentException(Properties.Resources.Argument_EmptyPath, "destinationFileName");
            }

            if (this._basePath == null)
            {
                throw new ObjectDisposedException(null, Properties.Resources.IsolatedStorage_StoreNotOpen);
            }

            string fullSourcePath = Path.Combine(BasePath, sourceFileName);
            string fullDestinationPath = Path.Combine(BasePath, destinationFileName);
            if (!File.Exists(fullSourcePath))
            {
                throw new FileNotFoundException(Properties.Resources.IO_PathNotFound_Path);
            }

            if (!overwrite & File.Exists(fullDestinationPath))
            {
                throw new IsolatedStorageException(Properties.Resources.IsolatedStorage_Operation);
            }

            try
            {
                File.Copy(fullSourcePath, fullDestinationPath, overwrite);
            }
            catch
            {
                throw new IsolatedStorageException(Properties.Resources.IsolatedStorage_Operation);
            }
        }

        /// <summary>
        /// Creates a directory in the isolated storage scope.
        /// </summary>
        /// <param name="dir">The relative path of the directory to create within the isolated storage.</param>
        public void CreateDirectory(string dir)
        {
            Directory.CreateDirectory(Path.Combine(BasePath, dir));

            System.IO.DirectoryInfo di = new DirectoryInfo(Path.Combine(BasePath, dir));
            di.Attributes |= FileAttributes.Hidden | FileAttributes.Encrypted;
        }

        /// <summary>
        /// Creates a file in the isolated store.
        /// </summary>
        /// <param name="path">The relative path of the file to be created in the isolated store.</param>
        /// <returns>A new isolated storage file.</returns>
        public IsolatedStorageFileStream CreateFile(string path)
        {
            return new IsolatedStorageFileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, this);
        }

        /// <summary>
        /// Deletes a directory in the isolated storage scope.
        /// </summary>
        /// <param name="dir">The relative path of the directory to delete within the isolated storage scope.</param>
        public void DeleteDirectory(string dir)
        {
            Directory.Delete(Path.Combine(this.BasePath, dir));
        }

        /// <summary>
        /// Deletes a file in the isolated store.
        /// </summary>
        /// <param name="file">The relative path of the file to delete within the isolated store.</param>
        public void DeleteFile(string file)
        {
            File.Delete(Path.Combine(this.BasePath, file));
        }

        /// <summary>
        /// Determines whether the specified path refers to an existing directory in the isolated store.
        /// </summary>
        /// <param name="path">The path to test.</param>
        /// <returns>true if path refers to an existing directory in the isolated store and is not null; otherwise, false.</returns>
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(Path.Combine(this.BasePath, path));
        }

        /// <summary>
        /// Determines whether the specified path refers to an existing file in the isolated store.
        /// </summary>
        /// <param name="path">The path and file name to test.</param>
        /// <returns>true if path refers to an existing file in the isolated store and is not null; otherwise, false.</returns>
        public bool FileExists(string path)
        {
            return File.Exists(Path.Combine(this.BasePath, path));
        }

        /// <summary>
        /// Enumerates directories in an isolated storage scope that match a given pattern.
        /// </summary>
        /// <param name="searchPattern">A search pattern.
        /// Both single-character ("?") and multi-character ("*") wildcards are supported.</param>
        /// <returns>An Array of the relative paths of directories in the isolated storage scope that match searchPattern.
        /// A zero-length array specifies that there are no directories that match.</returns>
        public string[] GetDirectoryNames(string searchPattern)
        {
            string[] directoryNames = Directory.GetDirectories(this.BasePath, searchPattern);

            for (int i = 0; i < directoryNames.Length; i++)
            {
                directoryNames[i] = directoryNames[i].Substring(this.BasePath.Length + 1);
            }

            return directoryNames;
        }

        /// <summary>
        /// Enumerates the directories in the root of an isolated store.
        /// </summary>
        /// <returns>An array of relative paths of directories in the root of the isolated store.
        /// A zero-length array specifies that there are no directories in the root.</returns>
        public string[] GetDirectoryNames()
        {
            string[] directoryNames = Directory.GetDirectories(this.BasePath);

            for (int i = 0; i < directoryNames.Length; i++)
            {
                directoryNames[i] = directoryNames[i].Substring(this.BasePath.Length+1);
            }

            return directoryNames;
        }

        /// <summary>
        /// Enumerates files in isolated storage scope that match a given pattern.
        /// </summary>
        /// <param name="searchPattern">A search pattern.
        /// Both single-character ("?") and multi-character ("*") wildcards are supported.</param>
        /// <returns>An Array of relative paths of files in the isolated storage scope that match searchPattern.
        /// A zero-length array specifies that there are no files that match.</returns>
        public string[] GetFileNames(string searchPattern)
        {
            string[] fileNames = Directory.GetFiles(this.BasePath, searchPattern);

            List<string> fixedFileNames = new List<string>();

            for (int i = 0; i < fileNames.Length; i++)
            {
                string fixedName = fileNames[i].Substring(this.BasePath.Length + 1);
                if (fixedName != "__ApplicationSettings")
                {
                    fixedFileNames.Add(fixedName);
                }
            }

            return fixedFileNames.ToArray();
        }

        /// <summary>
        /// Obtains the names of files in the root of an isolated store.
        /// </summary>
        /// <returns>An array of relative paths of files in the root of the isolated store.
        /// A zero-length array specifies that there are no files in the root.</returns>
        public string[] GetFileNames()
        {
            string[] fileNames = Directory.GetFiles(this.BasePath);

            List<string> fixedFileNames = new List<string>();

            for (int i = 0; i < fileNames.Length; i++)
            {
                string fixedName = fileNames[i].Substring(this.BasePath.Length + 1);
                if (fixedName != "__ApplicationSettings")
                {
                    fixedFileNames.Add(fixedName);
                }
            }

            return fixedFileNames.ToArray();
        }

        /// <summary>
        /// Opens a file in the specified mode with read, write, or read/write access and the specified sharing option.
        /// </summary>
        /// <param name="path">The relative path of the file within the isolated store.</param>
        /// <param name="mode">The mode in which to open the file.</param>
        /// <param name="access"> The type of access to open the file with.</param>
        /// <param name="share">The type of access other <see cref="IsolatedStorageFileStream"/> objects have to this file.</param>
        /// <returns>A file that is opened in the specified mode and access, and with the specified sharing options.</returns>
        public IsolatedStorageFileStream OpenFile(string path, System.IO.FileMode mode, 
            System.IO.FileAccess access, System.IO.FileShare share)
        {
            return new IsolatedStorageFileStream(path, mode, access, share, this);
        }

        /// <summary>
        /// Opens a file in the specified mode with the specified file access.
        /// </summary>
        /// <param name="path">The relative path of the file within the isolated store.</param>
        /// <param name="mode">The mode in which to open the file.</param>
        /// <param name="access"> The type of access to open the file with.</param>
        /// <returns>A file that is opened in the specified mode and access, and is unshared.</returns>
        public IsolatedStorageFileStream OpenFile(string path, System.IO.FileMode mode, System.IO.FileAccess access)
        {
            return new IsolatedStorageFileStream(path, mode, access, this);
        }

        /// <summary>
        /// Opens a file in the specified mode.
        /// </summary>
        /// <param name="path">The relative path of the file within the isolated store.</param>
        /// <param name="mode">The mode in which to open the file.</param>
        /// <returns>A file that is opened in the specified mode, with read/write access, and is unshared.</returns>
        public IsolatedStorageFileStream OpenFile(string path, System.IO.FileMode mode)
        {
            return new IsolatedStorageFileStream(path, mode, this);
        }

        /// <summary>
        /// Removes the isolated storage scope and all its contents.
        /// </summary>
        public void Remove()
        {
            Directory.Delete(BasePath, true);
            Dispose();
        }

        /// <summary>
        /// Gets a value that represents the amount of free space available for isolated storage.
        /// </summary>
        public long AvailableFreeSpace
        {
            get
            {
                return InTheHand.IO.DriveInfo.GetDrives()[0].AvailableFreeSpace;
            }
        }

        /// <summary>
        /// Gets a value that represents the maximum amount of space available for isolated storage.
        /// </summary>
        /// <value>The limit of isolated storage space, in bytes.</value>
        /// <exception cref="ObjectDisposedException">The isolated store has been disposed.</exception>
        public long Quota
        {
            get
            {
                if (_basePath == null)
                {
                    throw new ObjectDisposedException("IsolatedStorageFile");
                }

                return long.MaxValue;
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="IsolatedStorageFile"/>.
        /// </summary>
        public void Dispose()
        {
            _basePath = null;
        }
    }
}
