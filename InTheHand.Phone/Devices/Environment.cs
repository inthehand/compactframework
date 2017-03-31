// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Environment.cs" company="In The Hand Ltd">
// Copyright (c) 2010-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Devices
{
    /// <summary>
    /// Gives applications access to information about the environment in which they are running.
    /// </summary>
    public static class Environment
    {
        private static bool haveDeviceType = false;
        private static DeviceType deviceType;

        /// <summary>
        /// Gets the type of device on which the application is running.
        /// Use this property to determine if your application is running on an actual device or on the device emulator.
        /// </summary>
        public static DeviceType DeviceType
        {
            get
            {
                if (!haveDeviceType)
                {
                    // determine if on emulator
                    string oemInfo;
                    bool success = InTheHand.NativeMethods.SystemParametersInfoString(NativeMethods.SPI.GETOEMINFO, out oemInfo);
                    if (success)
                    {
                        switch (oemInfo)
                        {
                            case "Microsoft DeviceEmulator":
                                deviceType = DeviceType.Emulator;
                                break;

                            default:
                                deviceType = DeviceType.Device;
                                break;
                        }

                        haveDeviceType = true;
                    }                  
                }

                return deviceType;
            }
        }
    }
}
