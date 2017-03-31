// In The Hand - .NET Components for Mobility
//
// InTheHand.Reflection.Assembly
// 
// Copyright (c) 2003-2014 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Reflection
{
	/// <summary>
	/// Contains helper functions for the <see cref="System.Reflection.Assembly"/> class.
	/// </summary>
	/// <seealso cref="System.Reflection.Assembly"/>
	public static class AssemblyInTheHand
	{
        private static string moduleFileName;
        private static System.Reflection.Assembly entryAssembly;
        
		/// <summary>
		/// Gets the process executable.
		/// </summary>
		/// <returns>The <see cref="System.Reflection.Assembly"/> that is the process executable.</returns>
		public static System.Reflection.Assembly GetEntryAssembly()
		{
            if (entryAssembly == null)
            {
                string assemblyPath = GetModuleFileName();
                entryAssembly = System.Reflection.Assembly.LoadFrom(assemblyPath);
            }
            return entryAssembly;
		}

        internal static string GetModuleFileName()
        {
            if (string.IsNullOrEmpty(moduleFileName))
            {
                byte[] buffer = new byte[InTheHand.EnvironmentInTheHand.MaxPath * 2];
                int chars = GetModuleFileName(IntPtr.Zero, buffer, InTheHand.EnvironmentInTheHand.MaxPath);

                if (chars > 0)
                {
                    if (chars > InTheHand.EnvironmentInTheHand.MaxPath)
                    {
                        throw new System.IO.PathTooLongException(InTheHand.Properties.Resources.IO_PathTooLong);
                    }

                    moduleFileName = System.Text.Encoding.Unicode.GetString(buffer, 0, chars * 2);
                }
            }
            return moduleFileName;
        }

        //Reflection.Assembly
        [DllImport("coredll", EntryPoint = "GetModuleFileName")]
        private static extern int GetModuleFileName(IntPtr hModule, byte[] filename, int size);
	}
}