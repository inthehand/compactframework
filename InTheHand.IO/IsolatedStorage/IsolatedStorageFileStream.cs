// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsolatedStorageFileStream.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.IO.IsolatedStorage
{
    using System.IO;

    /// <summary>
    /// Exposes a file within isolated storage.
    /// </summary>
    public sealed class IsolatedStorageFileStream : System.IO.FileStream
    {
        private IsolatedStorageFile _isolatedStorageFile;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageFileStream"/> class giving access to the file designated by path, in the specified mode, and in the context of the <see cref="IsolatedStorageFile"/> specified by isf.
        /// </summary>
        /// <param name="path">The relative path of the file within isolated storage.</param>
        /// <param name="mode">One of the <see cref="FileMode"/> values.</param>
        /// <param name="isf">The <see cref="IsolatedStorageFile"/> in which to open the <see cref="IsolatedStorageFileStream"/>.</param>
        public IsolatedStorageFileStream(string path, System.IO.FileMode mode, IsolatedStorageFile isf) 
            : this(path, mode, FileAccess.ReadWrite, FileShare.None, isf)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageFileStream"/> class giving access to the file designated by path in the specified mode, with the specified file access, and in the context of the <see cref="IsolatedStorageFile"/> specified by isf.
        /// </summary>
        /// <param name="path">The relative path of the file within isolated storage.</param>
        /// <param name="mode">One of the <see cref="FileMode"/> values.</param>
        /// <param name="access">A bitwise combination of the <see cref="FileAccess"/> values.</param>
        /// <param name="isf">The <see cref="IsolatedStorageFile"/> in which to open the <see cref="IsolatedStorageFileStream"/>.</param>
        public IsolatedStorageFileStream(string path, System.IO.FileMode mode, System.IO.FileAccess access, 
            IsolatedStorage.IsolatedStorageFile isf) : this(path, mode, access, FileShare.None, isf)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatedStorageFileStream"/> class giving access to the file designated by path, 
        /// in the specified mode, with the specified file access, using the file sharing mode specified by share, 
        /// and in the context of the <see cref="IsolatedStorageFile"/> specified by isf.
        /// </summary>
        /// <param name="path">The relative path of the file within isolated storage.</param>
        /// <param name="mode">One of the <see cref="FileMode"/> values.</param>
        /// <param name="access">A bitwise combination of the <see cref="FileAccess"/> values.</param>
        /// <param name="share">A bitwise combination of the <see cref="FileShare"/> values.</param>
        /// <param name="isf">The <see cref="IsolatedStorageFile"/> in which to open the <see cref="IsolatedStorageFileStream"/>.</param>
        public IsolatedStorageFileStream(string path, System.IO.FileMode mode, System.IO.FileAccess access, 
            System.IO.FileShare share, IsolatedStorage.IsolatedStorageFile isf)
            : base(Path.Combine(isf.BasePath, path), mode, access, share)
        {
            //also set the hidden attribute (just in case)
            System.IO.FileInfo fi = new System.IO.FileInfo(System.IO.Path.Combine(isf.BasePath, path));
            fi.Attributes |= System.IO.FileAttributes.Hidden;

            _isolatedStorageFile = isf;
        }

        /// <summary>
        /// Gets the name of the file that was used to create the instance of the <see cref="IsolatedStorageFileStream"/>.
        /// </summary>
        public new string Name
        {
            get
            {
                return base.Name.Replace(_isolatedStorageFile.BasePath, "");
            }
        }
    }
}
