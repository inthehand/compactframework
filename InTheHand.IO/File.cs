// --------------------------------------------------------------------------------------------------------------------
// <copyright file="File.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.IO
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using InTheHand.IO.IsolatedStorage;

    /// <summary>
    /// Provides helper methods for the <see cref="System.IO.File"/> class.
    /// </summary>
    /// <remarks>For Windows Phone 7 all file paths are relative to the application package and only read-only operations are supported.
    /// Use the isostore: prefix to denote a file in isolated storage which supports readand write operations.</remarks>
    /// <seealso cref="System.IO.File"/>
    public static class FileInTheHand
    {
        private static readonly Encoding defaultEncoding = Encoding.Default;

        /// <summary>
        /// Returns the MIME content type for a specific file name.
        /// </summary>
        /// <param name="filename">Full path to a file.</param>
        /// <returns></returns>
        /// <remarks>Does not interrogate file contents, the file extension is used to match the content type.</remarks>
        public static string GetContentType(string filename)
        {
            string contentType = null;

            string extension = Path.GetExtension(filename);
            if (!string.IsNullOrEmpty(extension))
            {
                Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(extension);
                if (rk != null)
                {
                    contentType = rk.GetValue("Content Type", string.Empty).ToString();
                    rk.Close();
                }
            }

            return contentType;
        }

        private const string isostorePrefix = "isostore:";

        private static FileStream GetFileStream(string path, FileMode mode, FileAccess access, FileShare share)
        {
            if (path.StartsWith(isostorePrefix))
            {
                return IsolatedStorageFile.GetUserStoreForApplication().OpenFile(path.Substring(isostorePrefix.Length), mode, access, share);
            }
            else
            {
                return new FileStream(path, mode, access, share);
            }
        }

        #region Beam
        /// <summary>
        /// Beam a file through the standard OBEX Push mechanism using IrDA or Bluetooth.
        /// </summary>
        /// <param name="path">The full path of the file to send.</param>
        /// <exception cref="ArgumentNullException">path is a null reference (Nothing in Visual Basic).</exception>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
        /// </list></remarks>
        public static void Beam(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("path");
            }

            Process.Start("beam.exe", path);
        }
        #endregion

        #region Append All Lines
        /// <summary>
        /// Appends lines to a file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to append the lines to.
        /// The file is created if it does not already exist.</param>
        /// <param name="contents">The lines to append to the file.</param>
        /// <exception cref="ArgumentException">path is a zero-length string.</exception>
        /// <exception cref="ArgumentNullException">Either path or contents is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
        public static void AppendAllLines(string path, IEnumerable<string> contents)
        {
            AppendAllLines(path, contents, defaultEncoding);
        }
        /// <summary>
        /// Appends lines to a file by using a specified encoding, and then closes the file.
        /// </summary>
        /// <param name="path">The file to append the lines to.</param>
        /// <param name="contents">The lines to append to the file.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <exception cref="ArgumentException">path is a zero-length string.</exception>
        /// <exception cref="ArgumentNullException">Either path or contents is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
        public static void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("path");
            }
            if (contents == null)
            {
                throw new ArgumentNullException("contents");
            }

            using (FileStream fs = GetFileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                using (StreamWriter writer = new StreamWriter(fs, encoding))
                {
                    foreach (string text in contents)
                    {
                        writer.WriteLine(text);
                    }

                    writer.Close();
                }

                fs.Close();
            }
        }
        #endregion

        #region Append All Text
        /// <summary>
        /// Appends the specified string to the file, creating the file if it does not already exist.
        /// </summary>
        /// <param name="path">The file to append the specified string to.</param>
        /// <param name="contents">The string to append to the file.</param>
        /// <exception cref="ArgumentNullException">Either path or contents is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
        public static void AppendAllText(string path, string contents)
        {
            AppendAllText(path, contents, defaultEncoding);
        }

        /// <summary>
        /// Appends the specified string to the file, creating the file if it does not already exist.
        /// </summary>
        /// <param name="path">The file to append the specified string to.</param>
        /// <param name="contents">The string to append to the file.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <exception cref="ArgumentNullException">Either path or contents is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
        public static void AppendAllText(string path, string contents, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            if (string.IsNullOrEmpty(contents))
            {
                throw new ArgumentNullException("contents");
            }
            using (FileStream fs = GetFileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                using (StreamWriter writer = new StreamWriter(fs, encoding))
                {
                    writer.Write(contents);
                    writer.Close();
                }

                fs.Close();
            }
        }

        #endregion


        #region Read All Bytes
        /// <summary>
        /// Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <returns>A byte array containing the contents of the file.</returns>
        /// <exception cref="ArgumentNullException">path is a null reference (Nothing in Visual Basic).</exception>
        public static byte[] ReadAllBytes(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("path");
            }
            byte[] buffer = null;

            using (FileStream stream = GetFileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                int offset = 0;
                long length = stream.Length;
                if (length > int.MaxValue)
                {
                    throw new IOException();
                }
                int count = (int)length;
                buffer = new byte[count];
                while (count > 0)
                {
                    int bytesRead = stream.Read(buffer, offset, count);
                    if (bytesRead == 0)
                    {
                        throw new EndOfStreamException();
                    }
                    offset += bytesRead;
                    count -= bytesRead;
                }

                stream.Close();
            }

            return buffer;
        }
        #endregion

        #region Read All Lines
        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <returns>A string array containing all lines of the file.</returns>
        /// <exception cref="ArgumentNullException">path is a null reference (Nothing in Visual Basic).</exception>
        public static string[] ReadAllLines(string path)
        {
            return ReadAllLines(path, defaultEncoding);
        }

        /// <summary>
        /// Opens a file, reads all lines of the file with the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <param name="encoding">The encoding applied to the contents of the file.</param>
        /// <returns>A string array containing all lines of the file.</returns>
        /// <exception cref="ArgumentNullException">path is a null reference (Nothing in Visual Basic).</exception>
        public static string[] ReadAllLines(string path, Encoding encoding)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("path");
            }

            System.Collections.Generic.List<string> lines = new List<string>();

            using (FileStream fs = GetFileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader reader = new StreamReader(fs, encoding))
                {
                    string text;
                    while ((text = reader.ReadLine()) != null)
                    {
                        lines.Add(text);
                    }

                    reader.Close();
                }

                fs.Close();
            }

            return lines.ToArray();

        }
        #endregion

        #region Read All Text
        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <returns>A string containing all of the file.</returns>
        /// <exception cref="ArgumentNullException">path is a null reference (Nothing in Visual Basic).</exception>
        public static string ReadAllText(string path)
        {
            return ReadAllText(path, defaultEncoding);
        }
        /// <summary>
        /// Opens a file, reads all lines of the file with the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <param name="encoding">The encoding applied to the contents of the file.</param>
        /// <returns>A string containing all of the file.</returns>
        /// <exception cref="ArgumentNullException">path is a null reference (Nothing in Visual Basic).</exception>
        public static string ReadAllText(string path, System.Text.Encoding encoding)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("path");
            }

            string contents = null;

            using (FileStream fs = GetFileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                StreamReader sr = new StreamReader(fs, encoding);
                contents = sr.ReadToEnd();

                sr.Close();

                fs.Close();
            }
            return contents;
        }
        #endregion


        #region Write All Bytes
        /// <summary>
        /// Creates a new file, writes the specified byte array to the file, and then closes the file. 
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="bytes">The bytes to write to the file.</param>
        /// <exception cref="ArgumentNullException">path is a null reference (Nothing in Visual Basic) or the byte array is empty.</exception>
        public static void WriteAllBytes(string path, byte[] bytes)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("path");
            }
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            using (FileStream fs = GetFileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
        }
        #endregion

        #region Write All Lines
        /// <summary>
        /// Creates a new file, writes the specified string array to the file using the default encoding, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The string array to write to the file.</param>
        /// <exception cref="ArgumentNullException">path is a null reference (Nothing in Visual Basic) or contents string is empty.</exception>
        public static void WriteAllLines(string path, string[] contents)
        {
            WriteAllLines(path, contents, defaultEncoding);
        }

        /// <summary>
        /// Creates a new file, writes the specified string array to the file using the specified encoding, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The string array to write to the file.</param>
        /// <param name="encoding">An <see cref="Encoding"/> object that represents the character encoding applied to the string array.</param>
        /// <exception cref="ArgumentNullException">path is a null reference (Nothing in Visual Basic) or contents string is empty.</exception>
        public static void WriteAllLines(string path, string[] contents, Encoding encoding)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("path");
            }
            if (contents == null)
            {
                throw new ArgumentNullException("contents");
            }

            using (FileStream fs = GetFileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                using (StreamWriter writer = new StreamWriter(fs, encoding))
                {
                    foreach (string text in contents)
                    {
                        writer.WriteLine(text);
                    }

                    writer.Close();
                }

                fs.Close();
            }
        }
        /// <summary>
        /// Creates a new file, writes a collection of strings to the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The lines to write to the file.</param>
        /// <exception cref="ArgumentNullException">path is a null reference (Nothing in Visual Basic) or contents string is empty.</exception>
        public static void WriteAllLines(string path, IEnumerable<string> contents)
        {
            WriteAllLines(path, contents, defaultEncoding);
        }
        /// <summary>
        /// Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The lines to write to the file.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <exception cref="ArgumentNullException">path is a null reference (Nothing in Visual Basic) or contents string is empty.</exception>
        public static void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("path");
            }
            if (contents == null)
            {
                throw new ArgumentNullException("contents");
            }

            using (FileStream fs = GetFileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                using (StreamWriter writer = new StreamWriter(fs, encoding))
                {
                    foreach (string text in contents)
                    {
                        writer.WriteLine(text);
                    }

                    writer.Close();
                }

                fs.Close();
            }
        }
        #endregion

        #region Write All Text
        /// <summary>
        /// Creates a new file, writes the specified string array to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The string to write to the file.</param>
        /// <exception cref="ArgumentNullException">path is a null reference (Nothing in Visual Basic) or contents string is empty.</exception>
        public static void WriteAllText(string path, string contents)
        {
            WriteAllText(path, contents, defaultEncoding);
        }
        /// <summary>
        /// Creates a new file, writes the specified string array to the file using the specified encoding, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The string to write to the file.</param>
        /// <param name="encoding">An Encoding object that represents the encoding to apply to the string.</param>
        /// <exception cref="ArgumentNullException">path is a null reference (Nothing in Visual Basic) or contents string is empty.</exception>
        public static void WriteAllText(string path, string contents, Encoding encoding)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("path");
            }
            if (contents == null)
            {
                throw new ArgumentNullException("contents");
            }

            using (FileStream fs = GetFileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                using (StreamWriter writer = new StreamWriter(fs, encoding))
                {
                    writer.Write(contents);

                    writer.Close();
                }

                fs.Close();
            }
        }
        #endregion
    }
}