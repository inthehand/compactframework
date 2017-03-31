// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NativeMethods.cs" company="In The Hand Ltd">
// Copyright (c) 2010-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using InTheHand.Runtime.InteropServices;

namespace InTheHand.Device.Location
{
    internal static class NativeMethods
    {
        private const string gpsapi = "gpsapi.dll";

        [DllImport(gpsapi, EntryPoint = "GPSOpenDevice", SetLastError = true)]
        internal static extern IntPtr OpenDevice(IntPtr hNewLocationData, IntPtr hDeviceStateChange, string szDeviceName, int dwFlags);

        [DllImport(gpsapi, EntryPoint = "GPSCloseDevice", SetLastError = false)]
        internal static extern int CloseDevice(IntPtr hGPSDevice);

        [DllImport(gpsapi, EntryPoint = "GPSGetPosition", SetLastError = false)]
        internal static extern int GetPosition(IntPtr hGPSDevice, ref GPS_POSITION pGPSPosition, int dwMaximumAge, int dwFlags);

        [DllImport(gpsapi, EntryPoint = "GPSGetDeviceState", SetLastError = false)]
        internal static extern int GetDeviceState(ref GPS_DEVICE pGPSDevice);

        //v2 API
        [DllImport(gpsapi, EntryPoint = "GPSGetDeviceParam", SetLastError = false)]
        internal static extern int GetDeviceParam(IntPtr hGPSDevice, GPS_CONFIG dwParamName, byte[] pbParamData, ref int pcbDataLen);

        [DllImport(gpsapi, EntryPoint = "GPSGetDeviceParam", SetLastError = false)]
        internal static extern int GetDeviceParam(IntPtr hGPSDevice, GPS_CONFIG dwParamName, out GPS_FIX_RATE pbParamData, ref int pcbDataLen);
       
        [DllImport(gpsapi, EntryPoint = "GPSSetDeviceParam", SetLastError = false)]
        internal static extern int SetDeviceParam(IntPtr hGPSDevice, GPS_CONFIG dwParamName, byte[] pbParamData, int cbDataLen);

        [DllImport(gpsapi, EntryPoint = "GPSSetDeviceParam", SetLastError = false)]
        internal static extern int SetDeviceParam(IntPtr hGPSDevice, GPS_CONFIG dwParamName, ref GPS_FIX_RATE pbParamData, int cbDataLen);

        internal enum GPS_CONFIG
        {
            FIX_MODE = 2,
            FIX_RATE = 3,
            QOS = 4,
        }

        internal const double KNOTS_TO_METRES_PER_SECOND = 0.514444444444444;
        internal const double DOP_TO_METRES = 6;

        private const int GPS_MAX_SATELLITES = 12;

        // Matches IOCTL_SERVICE_STATUS
        internal enum SERVICE_STATE : int
        {
            SERVICE_STATE_OFF = 0,
            SERVICE_STATE_ON = 1,
            SERVICE_STATE_STARTING_UP = 2,
            SERVICE_STATE_SHUTTING_DOWN = 3,
            SERVICE_STATE_UNLOADING = 4,
            SERVICE_STATE_UNINITIALIZED = 5,
            SERVICE_STATE_UNKNOWN = -1,
        }

        //
        // GPS_DEVICE contains information about the device driver and the
        // service itself and is returned on a call to GPSGetDeviceState().
        // States are indicated with SERVICE_STATE_XXX flags defined in service.h
        // dwVersion = GPS_VERSION_1 uses the first 8 fields, upto gdsDeviceStatus. 
        // dwVersion = GPS_VERSION_2 uses the first 9 fields, including gdsDeviceStatus.
        // 
        [StructLayout(LayoutKind.Sequential)]
        internal struct GPS_DEVICE
        {
            internal const int GPS_DEVICE_SIZE_V1 = 216;
            internal const int GPS_DEVICE_SIZE_V2 = 336;

            internal int dwVersion; // Current version of GPSID client is using.
            internal int dwSize; // sizeof this structure
            internal SERVICE_STATE dwServiceState; // State of the GPS Intermediate Driver service. 
            internal SERVICE_STATE dwDeviceState; // Status of the actual GPS device driver.
            internal long ftLastDataReceived; // Last time that the actual GPS device sent information to the intermediate driver.
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            internal string szGPSDriverPrefix; // Prefix name we are using to communicate to the base GPS driver
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            internal string szGPSMultiplexPrefix; // Prefix name that GPS Intermediate Driver Multiplexer is running on
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            internal string szGPSFriendlyName; // Friendly name real GPS device we are currently using
//#if GPSV2
            internal GPS_DEVICE_STATUS gdsDeviceStatus; // Information about GPS Hardware
//#endif
        }



        internal enum GPS_FIX_TYPE
        {
            GPS_FIX_UNKNOWN = 0,
            GPS_FIX_2D,
            GPS_FIX_3D,
        }
        internal enum GPS_FIX_SELECTION
        {
	        GPS_FIX_SELECTION_UNKNOWN = 0,
	        GPS_FIX_SELECTION_AUTO,
	        GPS_FIX_SELECTION_MANUAL
        }

        internal enum GPS_FIX_QUALITY
        {
            GPS_FIX_QUALITY_UNKNOWN = 0,
            GPS_FIX_QUALITY_GPS,
            GPS_FIX_QUALITY_DGPS
        }

        [Flags()]
        internal enum GPS_VALID
        {
            UTC_TIME = 0x00000001,
            LATITUDE = 0x00000002,
            LONGITUDE = 0x00000004,
            SPEED = 0x00000008,
            HEADING = 0x00000010,
            MAGNETIC_VARIATION = 0x00000020,
            ALTITUDE_WRT_SEA_LEVEL = 0x00000040,
            ALTITUDE_WRT_ELLIPSOID = 0x00000080,
            POSITION_DILUTION_OF_PRECISION = 0x00000100,
            HORIZONTAL_DILUTION_OF_PRECISION = 0x00000200,
            VERTICAL_DILUTION_OF_PRECISION = 0x00000400,
            SATELLITE_COUNT = 0x00000800,
            SATELLITES_USED_PRNS = 0x00001000,
            SATELLITES_IN_VIEW = 0x00002000,
            SATELLITES_IN_VIEW_PRNS = 0x00004000,
            SATELLITES_IN_VIEW_ELEVATION = 0x00008000,
            SATELLITES_IN_VIEW_AZIMUTH = 0x00010000,
            SATELLITES_IN_VIEW_SIGNAL_TO_NOISE_RATIO = 0x00020000,
// GPSV2
            POSITION_UNCERTAINTY_ERROR = 0x00040000,
            FIX_MODE =  0x00080000,
            FIX_ERROR = 0x00100000,
        }


        internal enum GPS_DATA_FLAGS
        {
            GPS_DATA_FLAGS_HARDWARE_OFF = 0x00000001,
        }

//#if GPSV2

        //
        // This enumeration contains values that specify the current GPS Hardware state.
        //
        internal enum GPS_HW_STATE
        {
            GPS_HW_STATE_UNKNOWN = 0,
            GPS_HW_STATE_ON,
            GPS_HW_STATE_IDLE,
        }

        //
        // This structure contains the status information about GPS Hardware used by GPSID. 
        //
        [StructLayout(LayoutKind.Sequential, Size = 116)]
        internal struct GPS_DEVICE_STATUS
        {
            internal int dwValidFields;
            GPS_HW_STATE ghsHardwareState;
            internal int dwEphSVMask;
            internal int dwAlmSVMask;
            [MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = GPS_MAX_SATELLITES)]
            internal int[] rgdwSatellitesInViewPRNs;
            [MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = GPS_MAX_SATELLITES)]
            internal int[] rgdwSatellitesInViewCarrierToNoiseRatio;
            internal int dwDeviceError;
        }

        [Flags()]
        internal enum GPS_DEV_VALID
        {
            GPS_DEV_VALID_HW_STATE = 0x00000001,
            GPS_DEV_VALID_EPH_SV_MASK = 0x00000002,
            GPS_DEV_VALID_ALM_SV_MASK = 0x00000004,
            GPS_DEV_VALID_SAT_IN_VIEW_PRN = 0x00000008,
            GPS_DEV_VALID_SAT_IN_VIEW_CARRIER_TO_NOISE_RATIO = 0x00000010,
            GPS_DEV_VALID_DEV_ERROR = 0x00000020,
        }

        //
        // This structure contains estimated position error information returned in GPS_POSITION
        // structure with dwVersion = GPS_VERSION_2. 
        //
        [StructLayout(LayoutKind.Sequential)]
        internal struct GPS_POSITION_ERROR
        {
            //
            // Horizontal position uncertainty in meters of axis aligned with the angle 
            // specified in dWHorizontalErrorAngle of a two-dimension horizontal error ellipse. 
            // The value dwHorizontalConfidence gives the percentage of positions expected to 
            // fall within this ellipse, e.g. dwHorizontalConfidence = 39 indicates a 1-sigma 
            // error ellipse is given.
            //
            internal uint dwHorizontalErrorAlong;

            //
            // Angle of horizontal axis, with respect to true north, for a two-dimension 
            // horizontal error ellipse.
            //
            internal uint dwHorizontalErrorAngle;

            //
            // Horizontal position uncertainty in meters of axis perpendicular to angle 
            // specified in dwHorizontalErrorAngle of a two-dimension horizontal error ellipse. 
            // The value dwHorizontalConfidence gives the percentage of positions expected to 
            // fall within this ellipse, e.g. dwHorizontalConfidence = 39 indicates a 1-sigma 
            // error ellipse is given.
            //
            internal uint dwHorizontalErrorPerp;

            //
            // Vertical position uncertainty in meters with 1 sigma uncertainty. 
            // This value is always given as the 1-sigma estimate of vertical position error. 
            // It is not adjusted based on the value of  dwHorizontalConfidence.
            //
            internal uint dwVerticalError;

            //
            // Identifies the calculated probability in percent that the position estimate 
            // resides within the two dimension horizontal error ellipse specified by the 
            // three horizontal error values above. Note that appropriate rescaling of the 
            // ellipse dimensions can be used to achieve other confidence values. The special 
            // case where dwHorizontalErrorAlong are dwHorizontalErrorPerp set to the estimated 
            // 1 dimension standard deviation values will yield a confidence value of 39%. 
            // (2.45 sigma yield 95% confidence, 3.03 x sigma yields 99% confidence).
            //
            internal int dwHorizontalConfidence;
        }

        //
        // The following define the fix modes known to the operating system.
        // The OEMs are free to define their own GPS_FIX_MODE starting with values
        // greater than or equal to GPS_FIX_MODE_CUSTOM.
        //
        internal enum GPS_FIX_MODE
        {
            GPS_FIX_MODE_UNKNOWN = 0,
            GPS_FIX_MODE_MSA,
            GPS_FIX_MODE_MSB,
            GPS_FIX_MODE_MSS,

            // This must be the last entry for OS-supplied GPS_FIX_MODE_XXX values.
            // It's not an actual fix mode value - It's only a place holder
            // to keep count of values in the enum.
            GPS_FIX_MODE_COUNT,
            
            //
            // This is the value which defines the start of the range where OEMs are 
            // permitted to use their own fix mode values.
            //
            GPS_FIX_MODE_CUSTOM = 256,
        } 
//#endif

        [StructLayout(LayoutKind.Sequential)]
        internal struct GPS_POSITION
        {
            internal const int GPS_POSITION_SIZE_V1 = 344;
            internal const int GPS_POSITION_SIZE_V2 = 376;

            internal int dwVersion; // Current version of GPSID client is using.
            internal int dwSize; // sizeof(_GPS_POSITION)

            // Not all fields in the structure below are guaranteed to be valid.  
            // Which fields are valid depend on GPS device being used, how stale the API allows
            // the data to be, and current signal.
            // Valid fields are specified in dwValidFields, based on GPS_VALID_XXX flags.
            internal GPS_VALID dwValidFields;

            // Additional information about this location structure (GPS_DATA_FLAGS_XXX)
            internal GPS_DATA_FLAGS dwFlags;

            //** Time related
            internal SYSTEMTIME stUTCTime; //  UTC according to GPS clock.

            //** Position + heading related
            internal double dblLatitude; // Degrees latitude.  North is positive
            internal double dblLongitude; // Degrees longitude.  East is positive
            internal float flSpeed; // Speed in knots
            internal float flHeading; // Degrees heading (course made good).  True North=0
            internal double dblMagneticVariation; // Magnetic variation.  East is positive
            internal float flAltitudeWRTSeaLevel; // Altitute with regards to sea level, in meters
            internal float flAltitudeWRTEllipsoid; // Altitude with regards to ellipsoid, in meters

            //** Quality of this fix
            internal GPS_FIX_QUALITY FixQuality; // Where did we get fix from?
            internal GPS_FIX_TYPE FixType; // Is this 2d or 3d fix?
            internal GPS_FIX_SELECTION SelectionType; // Auto or manual selection between 2d or 3d mode
            internal float flPositionDilutionOfPrecision; // Position Dilution Of Precision
            internal float flHorizontalDilutionOfPrecision; // Horizontal Dilution Of Precision
            internal float flVerticalDilutionOfPrecision; // Vertical Dilution Of Precision

            //** Satellite information
            internal int dwSatelliteCount; // Number of satellites used in solution
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GPS_MAX_SATELLITES)]
            internal int[] rgdwSatellitesUsedPRNs; // PRN numbers of satellites used in the solution

            internal int dwSatellitesInView; // Number of satellites in view.  From 0-GPS_MAX_SATELLITES
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GPS_MAX_SATELLITES)]
            internal int[] rgdwSatellitesInViewPRNs; // PRN numbers of satellites in view
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GPS_MAX_SATELLITES)]
            internal int[] rgdwSatellitesInViewElevation; // Elevation of each satellite in view
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GPS_MAX_SATELLITES)]
            internal int[] rgdwSatellitesInViewAzimuth; // Azimuth of each satellite in view
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GPS_MAX_SATELLITES)]
            internal int[] rgdwSatellitesInViewSignalToNoiseRatio; // Signal to noise ratio of each satellite in view

//#if GPSV2
            //** GPS API Extended fields (API v2)
            internal GPS_POSITION_ERROR PositionUncertaintyError; // Estimated position uncertainty error 
            internal GPS_FIX_MODE FixMode; // Fix mode used by the GPS hardware to calculate the position
            internal uint dwFixError; // GPS fix session error reported by GPS hardware
//#endif
        }

        //API v2
        [StructLayout(LayoutKind.Sequential)]
        internal struct GPS_FIX_RATE
        {
            // Number of fixes desired by applications. (-1 always on)
            internal int dwNumFixes;
            // Time, in seconds, between position fix attempts
            internal uint dwTimeBetweenFixes;
        }

        // This structure contains information about different quality of service (QoS) values an application can use.
        // There are pointers to this structure in the pbParamData field in calls to GPSGetDeviceParam or GPSSetDeviceParam functions when the dwParamName field is set to GPS_CONFIG_QOS.
        [StructLayout(LayoutKind.Sequential)]
        internal struct GPS_QOS
        {
            // Desired position fix accuracy in meters; a value of 50 implies fixes should have a position fix uncertainty of 50m or less.
            internal uint dwAccuracy;
            // Desired performance response quality in terms of time (in seconds).
            // A higher value translates to more time allowed to perform a GPS position fix, which can possibly result in more accurate/sensitive fix calculations.
            internal uint dwPerformance;
        }
    }
}