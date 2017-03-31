// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessModule.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Diagnostics
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Represents a.dll or .exe file that is loaded into a particular process.
    /// </summary>
    /// <remarks>
    /// A module is an executable file or a dynamic link library (DLL).
    /// Each process consists of one or more modules.
    /// You can use this class to get information about the module.</remarks>
    public sealed class ProcessModule : Component
    {
        private NativeMethods.MODULEENTRY32 me;

        internal ProcessModule(NativeMethods.MODULEENTRY32 me)
        {
            this.me = me;
        }

        /// <summary>
        /// Gets the memory address where the module was loaded.
        /// </summary>
        /// <value>The load address of the module.</value>
        public IntPtr BaseAddress
        {
            get
            {
                return me.modBaseAddr;
            }
        }

        /// <summary>
        /// Gets the full path to the module.
        /// </summary>
        /// <value>The fully qualified path that defines the location of the module.</value>
        public string FileName
        {
            get
            {
                return me.szExePath;
            }
        }

        private FileVersionInfo fvi;
        /// <summary>
        /// Gets version information about the module.
        /// </summary>
        /// <value>A <see cref="FileVersionInfo"/> that contains the module's version information.</value>
        public FileVersionInfo FileVersionInfo
        {
            get
            {
                if (fvi == null)
                {
                    if(!string.IsNullOrEmpty(FileName))
                    {
                        fvi = FileVersionInfo.GetVersionInfo(FileName);
                    }
                }

                return fvi;
            }
        }

        /// <summary>
        /// Gets the amount of memory that is required to load the module.
        /// </summary>
        /// <value>The size, in bytes, of the memory that the module occupies.</value>
        /// <remarks>ModuleMemorySize does not include any additional memory allocations that the module makes once it is running; it includes only the size of the static code and data in the module file.</remarks>
        public int ModuleMemorySize
        {
            get
            {
                return me.modBaseSize;
            }
        }

        /// <summary>
        /// Gets the name of the process module.
        /// </summary>
        /// <value>The name of the module.</value>
        public string ModuleName
        {
            get
            {
                return me.szModule;
            }
        }
    }
}
