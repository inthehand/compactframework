// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Marshal.cs" company="In The Hand Ltd">
// Copyright (c) 2003-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Runtime.InteropServices
{
    using System;
    using System.Runtime.InteropServices;

    using InTheHand;

    /// <summary>
    /// Provides a collection of methods for allocating unmanaged memory, copying unmanaged memory blocks, and converting managed to unmanaged types, as well as other miscellaneous methods used when interacting with unmanaged code.
    /// </summary>
    public static class MarshalInTheHand
    {
        /// <summary>
        /// Allocates memory from the unmanaged memory of the process using LocalAlloc.
        /// </summary>
        /// <param name="cb">The number of bytes in memory required.</param>
        /// <returns>An <see cref="IntPtr"/> to the newly allocated memory.
        /// This memory must be released using the <see cref="Marshal.FreeHGlobal(System.IntPtr)"/> method.</returns>
        public static IntPtr AllocHGlobal(int cb)
        {
            if (cb < 1)
            {
                throw new ArgumentException("cb");
            }

            return NativeMethods.LocalAlloc(0x40, cb);
        }


        /// <summary>
        /// Converts an <see cref="System.Object">object</see> to a COM VARIANT.
        /// </summary>
        /// <param name="obj">The object for which to get a COM VARIANT.</param>
        /// <param name="pDstNativeVariant">An <see cref="IntPtr"/> to receive the VARIANT corresponding to the obj parameter.</param>
        public static void GetNativeVariantForObject(object obj, IntPtr pDstNativeVariant)
        {
            // used when multiple types could be requested
            VarEnum preferredType = (VarEnum)Marshal.ReadInt16(pDstNativeVariant);
            IntPtr pValue = IntPtrInTheHand.Add(pDstNativeVariant, 8);

            if (obj is Guid)
            {
                IntPtr bytePointer = Marshal.AllocHGlobal(16);
                Marshal.StructureToPtr(((Guid)obj).ToByteArray(), bytePointer, false);
                Marshal.WriteInt16(pDstNativeVariant, 0, (short)VarEnum.VT_INT);
                Marshal.WriteInt32(pDstNativeVariant, 8, bytePointer.ToInt32());
            }
            else
            {
                switch (Type.GetTypeCode(obj.GetType()))
                {
                    case TypeCode.DateTime:
                        if (preferredType == VarEnum.VT_FILETIME)
                        {
                            long filetime = 0;
                            try
                            {
                                filetime = ((System.DateTime)obj).ToFileTime();
                            }
                            catch
                            {
                            }

                            Marshal.StructureToPtr(filetime, pValue, false);
                        }
                        else
                        {
                            Marshal.WriteInt16(pDstNativeVariant, 0, (short)VarEnum.VT_DATE);
                            double dTime = ((System.DateTime)obj).ToOADate();
                            Marshal.StructureToPtr(dTime, pValue, false);
                        }
                        break;

                    case TypeCode.String:
                        if (preferredType == VarEnum.VT_LPWSTR)
                        {
                            IntPtr stringPointer = StringToHGlobalUni(obj.ToString());
                            Marshal.WriteInt32(pDstNativeVariant, 8, stringPointer.ToInt32());
                        }
                        else
                        {
                            // Default behaviour is to marshal as BStr
                            Marshal.GetNativeVariantForObject(obj, pDstNativeVariant);
                        }

                        break;

                    default:
                        Marshal.GetNativeVariantForObject(obj, pDstNativeVariant);
                        break;
                }
            }
        }

        /// <summary>
        /// Converts a COM VARIANT to an object.
        /// </summary>
        /// <param name="pSrcNativeVariant">An <see cref="IntPtr"/> containing a COM VARIANT.</param>
        /// <returns>An object corresponding to the <paramref name="pSrcNativeVariant"/> parameter.</returns>
        /// <exception cref="ArgumentException">pSrcNativeVariant is not a valid VARIANT type.</exception>
        /// <remarks>GetObjectForNativeVariant returns a managed object corresponding to a raw pointer to an unmanaged VARIANT type.
        /// GetObjectForNativeVariant provides the opposite functionality of <see cref="Marshal.GetNativeVariantForObject"/>.</remarks>
        public static object GetObjectForNativeVariant(IntPtr pSrcNativeVariant)
        {
            if (pSrcNativeVariant != IntPtr.Zero)
            {
                object result;

                //read the type
                VarEnum vt = (VarEnum)Marshal.ReadInt16(pSrcNativeVariant, 0);
                IntPtr pValue;

                if ((vt & VarEnum.VT_BYREF) == VarEnum.VT_BYREF)
                {
                    pValue = (IntPtr)Marshal.ReadInt32(pSrcNativeVariant, 8);
                }
                else
                {
                    pValue = IntPtrInTheHand.Add(pSrcNativeVariant, 8);
                }

                switch (vt)
                {
                    //byte array PT_BINARY
                    case (VarEnum)0x102:
                    //CEVT_BLOB
                    case (VarEnum)0x41:
                    //CEVT_STREAM
                    case (VarEnum)0x64:
                    //CEVT_RECID
                    case (VarEnum)0x65:
                        int len = Marshal.ReadInt32(pSrcNativeVariant, 8);
                        IntPtr ptr = (IntPtr)Marshal.ReadInt32(pSrcNativeVariant, 12);
                        byte[] data = new byte[len];
                        Marshal.Copy(ptr, data, 0, len);
                        return data;

                    //CEVT_AUTO_I8
                    case (VarEnum)0x67:
                        return Marshal.ReadInt64(pSrcNativeVariant, 8);

                    //CEVT_AUTO_I4
                    case (VarEnum)0x66:
                        return Marshal.ReadInt32(pSrcNativeVariant, 8);

                    case VarEnum.VT_CLSID:
                    case VarEnum.VT_CLSID | VarEnum.VT_BYREF:
                        byte[] guidBuffer = new byte[16];
                        IntPtr pGuid = Marshal.ReadIntPtr(pValue);
                        Marshal.Copy(pGuid, guidBuffer, 0, 16);
                        result = new Guid(guidBuffer);
                        break;
                    case VarEnum.VT_CY:
                    case VarEnum.VT_CY | VarEnum.VT_BYREF:
                        long lCy = (long)Marshal.PtrToStructure(pValue, typeof(long));
                        result = DecimalInTheHand.FromOACurrency(lCy);
                        break;
                    case VarEnum.VT_FILETIME:
                        long filetime = (long)Marshal.PtrToStructure(pValue, typeof(long));
                        result = DateTime.FromFileTime(filetime);
                        break;

                    case VarEnum.VT_LPWSTR:
                    case VarEnum.VT_LPWSTR | VarEnum.VT_BYREF:
                        IntPtr pwString = Marshal.ReadIntPtr(pValue);
                        if (pwString == IntPtr.Zero)
                        {
                            result = string.Empty;
                        }
                        else
                        {
                            result = Marshal.PtrToStringUni(pwString);
                        }
                        break;

                    default:
                        return Marshal.GetObjectForNativeVariant(pSrcNativeVariant);
                }

                return result;
            }
            else
            {
                throw new ArgumentNullException("GetObjectForNativeVariant requires a valid pointer");
            }
        }

        /// <summary>
        /// Converts an array of COM VARIANTs to an array of objects.
        /// </summary>
        /// <param name="aSrcNativeVariant">An <see cref="IntPtr"/> containing the first element of an array of COM VARIANTs.</param>
        /// <param name="cVars">The count of COM VARIANTs in aSrcNativeVariant.</param>
        /// <returns>An object array corresponding to aSrcNativeVariant.</returns>
        public static Object[] GetObjectsForNativeVariants(IntPtr aSrcNativeVariant, int cVars)
        {

            if (cVars < 0)
            {
                throw new ArgumentOutOfRangeException("cVars cannot be a negative number.");
            }

            if (cVars == 0)
            {
                return new object[0];
            }

            if (aSrcNativeVariant == IntPtr.Zero)
            {
                throw new ArgumentNullException("aSrcNativeVariant");
            }

            object[] vals = new object[cVars];
            for (int ivar = 0; ivar < cVars; ivar++)
            {
                try
                {
                    vals[ivar] = GetObjectForNativeVariant(IntPtrInTheHand.Add(aSrcNativeVariant, (16 * ivar)));
                }
                catch
                {
                    vals[ivar] = null;
                }
            }

            return vals;
        }


        /// <summary>
        /// Copies the contents of a managed <see cref="String"/> into unmanaged memory.
        /// </summary>
        /// <param name="s">A managed string to be copied.</param>
        /// <returns>The address, in unmanaged memory, to where the s was copied, or 0 if null string was supplied.</returns>
        /// <remarks><b>StringToHGlobalUni</b> is useful for custom marshaling or for use when mixing managed and unmanaged code.
        /// Since this method allocates the unmanaged memory required for a string, always free the memory by calling <see cref="Marshal.FreeHGlobal"/>.
        /// This method provides the opposite functionality of <see cref="Marshal.PtrToStringUni(System.IntPtr)"/>.</remarks>
        public static IntPtr StringToHGlobalUni(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                // alloc native memory
                IntPtr ptr = AllocHGlobal((s.Length + 1) * 2);

                // copy string contents
                Marshal.Copy(s.ToCharArray(), 0, ptr, s.Length);
                // return native pointer
                return ptr;
            }
            else
            {
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Copies the contents of a managed String into unmanaged memory, converting into ANSI format as it copies.
        /// </summary>
        /// <param name="s">A managed string to be copied. </param>
        /// <returns>The address, in unmanaged memory, to where s was copied, or 0 if a null string was supplied.</returns>
        public static IntPtr StringToHGlobalAnsi(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                // alloc native memory
                IntPtr ptr = AllocHGlobal((s.Length + 1));

                // copy string contents
                byte[] ansibytes = System.Text.Encoding.ASCII.GetBytes(s);
                Marshal.Copy(ansibytes, 0, ptr, ansibytes.Length);

                // return native pointer
                return ptr;
            }
            else
            {
                return IntPtr.Zero;
            }
        }


        /// <summary>
        /// Returns the instance handle (HINSTANCE) for the specified module.
        /// </summary>
        /// <param name="m">The Module whose HINSTANCE is desired. </param>
        /// <returns>The HINSTANCE for m; -1 if the module does not have an HINSTANCE.</returns>
        /// <exception cref="ArgumentNullException">The m parameter is a null reference (Nothing in Visual Basic)</exception>
        public static IntPtr GetHINSTANCE(System.Reflection.Module m)
        {
            if (m == null)
            {
                throw new ArgumentNullException();
            }

            return GetHINSTANCE(m.Assembly.GetName().CodeBase);
        }

        /// <summary>
        /// Returns the instance handle (HINSTANCE) for the specified module.
        /// </summary>
        /// <param name="path">The file path of the native dll.</param>
        /// <returns>The HINSTANCE for m; -1 if the module does not have an HINSTANCE.</returns>
        /// <exception cref="ArgumentNullException">The m parameter is a null reference (Nothing in Visual Basic)</exception>
        public static IntPtr GetHINSTANCE(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException();
            }

            IntPtr hinstance = NativeMethods.GetModuleHandle(path);
            if (hinstance == IntPtr.Zero)
            {
                return new IntPtr(-1);
            }

            return hinstance;
        }

        private static class NativeMethods
        {
            [DllImport("coredll")]
            internal static extern IntPtr LocalAlloc(uint uFlags, int uBytes);

            [DllImport("coredll")]
            internal static extern IntPtr GetModuleHandle(string lpModuleName);
        }
    }
}
