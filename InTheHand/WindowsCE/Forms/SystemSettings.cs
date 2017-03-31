// In The Hand - .NET Components for Mobility
//
// InTheHand.WindowsCE.Forms.SystemSettings
// 
// Copyright (c) 2003-2014 In The Hand Ltd, All rights reserved.

using System.Runtime.InteropServices;
using Microsoft.WindowsCE.Forms;

namespace InTheHand.WindowsCE.Forms
{
    /// <summary>
    /// Provides access to user interface and native Windows CE operating system settings on a device.
    /// </summary>
    /// <seealso cref="Microsoft.WindowsCE.Forms.SystemSettings"/>
    public static class SystemSettingsInTheHand
	{
        private static WinCEPlatform platform = (WinCEPlatform)(-1);

        /// <summary>
        /// Gets the well-known Windows CE based operating system of the device.
        /// </summary>
        /// <value>A <see cref="WinCEPlatform"/> enumeration value that specifies the device platform.</value>
        public static WinCEPlatform Platform
        {
            get
            {

                if (platform == (WinCEPlatform)(-1))
                {
                    if (System.Environment.OSVersion.Version.Major > 5)
                    {
                        platform = WinCEPlatform.WinCEGeneric;
                    }
                    else
                    {
                        // This API is useful for Windows Mobile but throws an OS exception on CE6 and above because it is deprecated
                        // But in those cases we know it's WinCEGeneric!
                        string typeString = null;
                        bool success = InTheHand.NativeMethods.SystemParametersInfoString(InTheHand.NativeMethods.SPI.GETPLATFORMTYPE, out typeString);
                        if (success)
                        {
                            switch (typeString)
                            {
                                case "PocketPC":
                                    platform = WinCEPlatform.PocketPC;
                                    break;
                                case "SmartPhone":
                                    platform = WinCEPlatform.Smartphone;
                                    break;
                                default:
                                    platform = WinCEPlatform.WinCEGeneric;
                                    break;
                            }
                        }
                        else
                        {
                            // on security error return Smartphone
                            if (Marshal.GetLastWin32Error() == 0x5)
                            {
                                platform = WinCEPlatform.Smartphone;
                            }
                            else
                            {
                                platform = WinCEPlatform.WinCEGeneric;
                            }
                        }
                    }
                }
                return platform;
            }
        }
	}
}