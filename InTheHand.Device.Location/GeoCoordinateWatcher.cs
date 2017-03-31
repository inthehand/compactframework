// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeoCoordinateWatcher.cs" company="In The Hand Ltd">
// Copyright (c) 2010-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using InTheHand.Threading;

namespace InTheHand.Device.Location
{
    /// <summary>
    /// Supplies location data that is based on latitude and longitude coordinates. 
    /// </summary>
    /// <remarks>
    /// <para>Equivalent to System.Device.Location.GeoCoordinateWatcher in the .NET Framework 4</para>
    /// The GeoCoordinateWatcher class supplies coordinate-based location data from the current location provider, which is the location provider that is currently prioritized the highest on the computer, based on a number of factors such as the age and accuracy of the data from all providers, the accuracy requested by location applications, and the power consumption and performance impact associated with the location provider.
    /// Currently only a GPS provider is supported on .NET Compact Framework platforms.
    /// <para>To begin accessing location data, create a <see cref="GeoCoordinateWatcher"/> and call <see cref="Start()"/> or <see cref="TryStart"/> to initiate the acquisition of data from the current location provider.</para>
    /// <para>The <see cref="Status"/> property can be checked to determine if data is available.
    /// If data is available, you can get the location once from the <see cref="Position"/> property, or receive continuous location updates by handling the <see cref="PositionChanged"/> event.</para>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows Embedded Compact 6.0 and later</description></item>
    /// </list>
    /// </remarks>
    /// <example>The following program demonstrates how to create a <see cref="GeoCoordinateWatcher"/> and start acquiring data using an initialization timeout.
    /// It prints the location's coordinates, if known.
    /// <code lang="cs">
    /// using System;
    /// using System.Device.Location;
    /// 
    /// namespace GetLocationProperty
    /// {
    ///     class Program
    ///     {
    ///         static void Main(string[] args)
    ///         {
    ///             GetLocationProperty();
    ///         }
    ///         
    ///         static void GetLocationProperty()
    ///         {
    ///             GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
    ///         
    ///             // Do not suppress prompt, and wait 1000 milliseconds to start.
    ///             watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));
    ///         
    ///             GeoCoordinate coord = watcher.Position.Location;
    ///         
    ///             if (coord.IsUnknown != true)
    ///             {
    ///                 Debug.WriteLine("Lat: {0}, Long: {1}", coord.Latitude, coord.Longitude);
    ///             }
    ///             else
    ///             {
    ///                 Debug.WriteLine("Unknown latitude and longitude.");
    ///             }
    ///         }
    ///     }
    /// }
    /// </code></example> 
    /// <example>The following program demonstrates how to receive continuous location updates by subscribing to PositionChanged events.
    /// <code lang="cs">
    /// using System;
    /// using System.Device.Location;
    /// 
    /// namespace GetLocationEvent
    /// {
    ///     class Program
    ///     {
    ///         static void Main(string[] args)
    ///         {
    ///             CLocation myLocation = new CLocation();
    ///             myLocation.GetLocationEvent();
    ///             Console.WriteLine("Enter any key to quit.");
    ///             Console.ReadLine();            
    ///         }
    ///         class CLocation
    ///         {
    ///             GeoCoordinateWatcher watcher;
    /// 
    ///             public void GetLocationEvent()
    ///             {
    ///                 this.watcher = new GeoCoordinateWatcher();
    ///                 this.watcher.PositionChanged += new EventHandler&lt;GeoPositionChangedEventArgs&lt;GeoCoordinate&gt;&gt;(watcher_PositionChanged);
    ///                 bool started = this.watcher.TryStart(false, TimeSpan.FromMilliseconds(2000));
    ///                 if (!started)
    ///                 {
    ///                     Debug.WriteLine("GeoCoordinateWatcher timed out on start.");
    ///                 }
    ///             }
    /// 
    ///             void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs&lt;GeoCoordinate&gt; e)
    ///             {
    ///                 PrintPosition(e.Position.Location.Latitude, e.Position.Location.Longitude);
    ///             }
    /// 
    ///             void PrintPosition(double Latitude, double Longitude)
    ///             {
    ///                 Debug.WriteLine("Latitude: {0}, Longitude {1}", Latitude, Longitude);
    ///             }
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    public sealed class GeoCoordinateWatcher : IDisposable, INotifyPropertyChanged, IGeoPositionWatcher<GeoCoordinate>
    {
        private bool active = false;
        // gps id handle
        private IntPtr handle = IntPtr.Zero;
        // handles to GPS events
        private InTheHand.Threading.EventWaitHandle[] waitHandles = new InTheHand.Threading.EventWaitHandle[3];

        // device info
        private NativeMethods.GPS_DEVICE device;
        // position info
        private NativeMethods.GPS_POSITION position;

        private static int version = 1;

        static GeoCoordinateWatcher()
        {
            // determine API version based on device version
            if (System.Environment.OSVersion.Version > new Version(5, 2, 19000))
            {
                version = 2;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="GeoCoordinateWatcher"/>.
        /// </summary>
        public GeoCoordinateWatcher()
        {
            waitHandles[0] = new InTheHand.Threading.EventWaitHandle(false, EventResetMode.AutoReset, "Gps.Close");
            waitHandles[1] = new InTheHand.Threading.EventWaitHandle(false, EventResetMode.AutoReset, "Gps.PositionChanged");
            waitHandles[2] = new InTheHand.Threading.EventWaitHandle(false, EventResetMode.AutoReset, "Gps.DeviceStateChanged");

            device = new NativeMethods.GPS_DEVICE();
            position = new NativeMethods.GPS_POSITION();
            position.dwVersion = version;
            device.dwVersion = 1;
            switch (version)
            {
                case 1:
                    device.dwSize = NativeMethods.GPS_DEVICE.GPS_DEVICE_SIZE_V1;
                    position.dwSize = NativeMethods.GPS_POSITION.GPS_POSITION_SIZE_V1;
                    break;

                case 2:
                    device.dwSize = NativeMethods.GPS_DEVICE.GPS_DEVICE_SIZE_V1;
                    position.dwSize = NativeMethods.GPS_POSITION.GPS_POSITION_SIZE_V2;
                    break;
            }
        }

        private GeoPositionStatus status = GeoPositionStatus.Disabled;
        /// <summary>
        /// Gets the current status of the <see cref="GeoCoordinateWatcher"/>.
        /// </summary>
        public GeoPositionStatus Status 
        { 
            get 
            { 
                return status; 
            } 
        }

        private double movementThreshold = 0.0;
        /// <summary>
        /// The distance that must be moved, in meters relative to the coordinate from the last <see cref="PositionChanged"/> event, before the location provider will raise another <see cref="PositionChanged"/> event.
        /// </summary>
        /// <value>Distance in meters.</value>
        public double MovementThreshold
        {
            get
            {
                return movementThreshold;
            }

            set
            {
                if (movementThreshold != value)
                {
                    if (value < 0.0)
                    {
                        throw new ArgumentOutOfRangeException(Properties.Resources.Argument_MustBeNonNegative);
                    }

                    movementThreshold = value;
                    OnPropertyChanged("MovementThreshold");
                }
            }
        }

        private uint _reportInterval = 0;
        /// <summary>
        /// The requested minimum time interval between location updates, in milliseconds.
        /// If your application requires updates infrequently, set this value so that the location provider can conserve power by calculating location only when needed.
        /// </summary>
        /// <value>The requested minimum time interval between location updates.</value>
        /// <remarks>If another application has requested more frequent updates, by specifying a smaller value for ReportInterval, your application may receive updates at a higher frequency than requested.
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile Version 6.1 and later</description></item>
        /// <item><term>Windows Embedded Compact</term><description>Windows Embedded Compact 7 and later</description></item>
        /// </list>
        /// </remarks>
        public uint ReportInterval
        {
            get
            {
                return _reportInterval;
            }

            set
            {
                if (_reportInterval != value)
                {
                    _reportInterval = value;
                    if (version == 2)
                    {
                        NativeMethods.GPS_FIX_RATE gfr = new NativeMethods.GPS_FIX_RATE();
                        gfr.dwTimeBetweenFixes = value;
                        gfr.dwNumFixes = -1;
                        int result = NativeMethods.SetDeviceParam(this.handle, NativeMethods.GPS_CONFIG.FIX_RATE, ref gfr, 8);
                    }

                    this.OnPropertyChanged("ReportInterval");
                }
            }
        }

        // Throws exception if response code is an error
        private static void CheckResponse(int response)
        {
            if (response != 0)
            {
                throw InTheHand.ComponentModel.Win32ExceptionInTheHand.Create(response);
            }
        }

        private bool OpenDevice()
        {
            try
            {
                handle = NativeMethods.OpenDevice(waitHandles[1].Handle, waitHandles[2].Handle, null, 0);
                if (handle != IntPtr.Zero)
                {
                    return true;
                }
            }
            catch(MissingMethodException)
            {
                // gps API is not present on target device.
            }

            return false;
        }

        private void GetDeviceState(bool raiseEvent)
        {
            // get device state
            int result = NativeMethods.GetDeviceState(ref device);

            CheckResponse(result);

            // debug
            System.Diagnostics.Debug.WriteLine(string.Format("Service state: {0}", device.dwServiceState));
            System.Diagnostics.Debug.WriteLine(string.Format("Device state: {0}", device.dwDeviceState));
            System.Diagnostics.Debug.WriteLine(string.Format("Last data received: {0}", DateTime.FromFileTimeUtc(device.ftLastDataReceived)));
            System.Diagnostics.Debug.WriteLine(string.Format("Friendly name: {0}", device.szGPSFriendlyName));
            System.Diagnostics.Debug.WriteLine(string.Format("GPS driver prefix: {0}", device.szGPSDriverPrefix));
            System.Diagnostics.Debug.WriteLine(string.Format("GPS multiplex prefix: {0}", device.szGPSMultiplexPrefix));

            status = ToGeoPositionStatus(device.dwDeviceState);

            if(raiseEvent)
            {
                try
                {
                    OnPositionStatusChanged(new GeoPositionStatusChangedEventArgs(this.Status));
                }
                catch 
                {
                    System.Diagnostics.Debug.WriteLine("Exception in GetDeviceState->OnPositionStatusChanged");
                }
            }
        }

        private void GetPosition(bool raiseEvent)
        {
            bool isValid = false;

            // reset flags
            position.dwValidFields = 0;

            // get position
            int result = NativeMethods.GetPosition(handle, ref position, 2000, 0);
            // fix for WM6.1 devices which broke the version checking
            if (result == 87)
            {
                position.dwSize = NativeMethods.GPS_POSITION.GPS_POSITION_SIZE_V2;
                // now check again
                result = NativeMethods.GetPosition(handle, ref position, 2000, 0);
            }

            CheckResponse(result);

            // must be at least one valid field
            if (position.dwValidFields > 0)
            {
                // check that lat, long and time are valid
                if (position.dwValidFields.HasFlag(NativeMethods.GPS_VALID.LATITUDE | NativeMethods.GPS_VALID.LONGITUDE | NativeMethods.GPS_VALID.UTC_TIME))
                {
                    System.Diagnostics.Debug.WriteLine(position.stUTCTime.ToDateTime(DateTimeKind.Utc).ToString() + "," + position.dblLatitude.ToString() + "," + position.dblLongitude.ToString());
                
                    // ignore readings where the time is said to be valid but is zero
                    if (position.stUTCTime.year != 0)
                    {
                        isValid = true;
                    }

                    // motorola sometimes returns this field as valid but with 0 - which means no satellites used to obtain a fix
                    if (/*position.dwValidFields.HasFlag(NativeMethods.GPS_VALID.SATELLITE_COUNT) &&*/ (position.dwSatelliteCount < 3))
                    {
                        isValid = false;
                    }

                    // widely accepted that 7 is maximum for a useful position
                    if (position.dwValidFields.HasFlag(NativeMethods.GPS_VALID.HORIZONTAL_DILUTION_OF_PRECISION) && position.flHorizontalDilutionOfPrecision > 7)
                    {
                        isValid = false;
                    }
                }
            }

            if (isValid)
            {
                // get the timestamp
                DateTimeOffset timestamp = new DateTimeOffset(position.stUTCTime.year, position.stUTCTime.month, position.stUTCTime.day, position.stUTCTime.hour, position.stUTCTime.minute, position.stUTCTime.second, new TimeSpan(0));
                currentTimeStamp = timestamp;

                // calculate the horizontal uncertainty
                double accuracy = 0.0;
                if (position.dwValidFields.HasFlag(NativeMethods.GPS_VALID.POSITION_UNCERTAINTY_ERROR))
                {
                    // v2 method
                    double avgError = (position.PositionUncertaintyError.dwHorizontalErrorAlong + position.PositionUncertaintyError.dwHorizontalErrorPerp) / 2f;
                    // get the 1 sigma radius
                    accuracy = avgError / position.PositionUncertaintyError.dwHorizontalConfidence * 39;
                }
                else
                {
                    // legacy method
                    if(position.dwValidFields.HasFlag(NativeMethods.GPS_VALID.HORIZONTAL_DILUTION_OF_PRECISION))
                    {
                        accuracy = (position.flHorizontalDilutionOfPrecision <= 50.0 ? position.flHorizontalDilutionOfPrecision : 50.0) * NativeMethods.DOP_TO_METRES;
                    }
                    else
                    {
                        accuracy = double.NaN;
                    }
                }

                // get the coordinates
                GeoCoordinate latLon = new GeoCoordinate(position.dblLatitude, position.dblLongitude,
                                                position.dwValidFields.HasFlag(NativeMethods.GPS_VALID.ALTITUDE_WRT_SEA_LEVEL) ? position.flAltitudeWRTSeaLevel : (position.dwValidFields.HasFlag(NativeMethods.GPS_VALID.ALTITUDE_WRT_ELLIPSOID) ? position.flAltitudeWRTEllipsoid : double.NaN),
                                                accuracy,
                                                position.dwValidFields.HasFlag(NativeMethods.GPS_VALID.POSITION_UNCERTAINTY_ERROR) ? position.PositionUncertaintyError.dwVerticalError : (position.dwValidFields.HasFlag(NativeMethods.GPS_VALID.VERTICAL_DILUTION_OF_PRECISION) ? (position.flVerticalDilutionOfPrecision <= 50.0 ? position.flVerticalDilutionOfPrecision : 50.0) * NativeMethods.DOP_TO_METRES : double.NaN),
                                                position.dwValidFields.HasFlag(NativeMethods.GPS_VALID.SPEED) ? position.flSpeed * NativeMethods.KNOTS_TO_METRES_PER_SECOND : double.NaN,
                                                position.dwValidFields.HasFlag(NativeMethods.GPS_VALID.HEADING) ? position.flHeading : double.NaN);

                currentPosition = latLon;

                // raise event?
                if (raiseEvent)
                {
                    if (Status != GeoPositionStatus.Ready)
                    {
                        status = GeoPositionStatus.Ready;
                        OnPositionStatusChanged(new GeoPositionStatusChangedEventArgs(GeoPositionStatus.Ready));
                    }

                    // only raise an event if movement threshold is exceeded
                    if (this.MovementThreshold == 0.0 || latLon.GetDistanceTo(currentPosition) > this.MovementThreshold)
                    {
                        OnPositionChanged(new GeoPositionChangedEventArgs<GeoCoordinate>(new GeoPosition<GeoCoordinate>(timestamp, latLon)));
                    }
                }
            }
            else
            {
                // running but no current fix
                if (raiseEvent && (Status == GeoPositionStatus.Ready))
                {
                    status = GeoPositionStatus.NoData;
                    OnPositionStatusChanged(new GeoPositionStatusChangedEventArgs(GeoPositionStatus.NoData));
                }
            }
        }

        private void OnPositionChanged(GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (PositionChanged != null)
            {
                try
                {
                    PositionChanged(this, e);
                }
                catch { }
            }

            if (PropertyChanged != null)
            {
                try
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Position"));
                }
                catch { }
            }
        }

        private void OnPositionStatusChanged(GeoPositionStatusChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Status changed: {0}", e.Status));

            if (this.StatusChanged != null)
            {
                try
                {
                    StatusChanged(this, e);
                }
                catch { }
            }

            OnPropertyChanged("Status");
        }

        /// <summary>
        /// Initiates the acquisition of data from the current location provider. This method returns synchronously.
        /// </summary>
        /// <param name="suppressPermissionPrompt">Not used.</param>
        /// <param name="timeout">Time in milliseconds to wait for the location provider to start before timing out.</param>
        /// <returns>true if data acquisition is started within the time period specified by timeout; otherwise, false.</returns>
        /// <remarks>This method blocks during the time period specified by timeout.
        /// Use caution when calling TryStart from the user interface thread of your application.</remarks>
        /// <example>The following example demonstrates how to call TryStart.
        /// <code lang="cs">
        /// using System;
        /// using System.Device.Location;
        /// 
        /// namespace GetLocationProperty
        /// {
        ///     class Program
        ///     {
        ///         static void Main(string[] args)
        ///         {
        ///             GetLocationProperty();
        ///         }
        ///         
        ///         static void GetLocationProperty()
        ///         {
        ///             GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
        ///         
        ///             // Do not suppress prompt, and wait 1000 milliseconds to start.
        ///             watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));
        ///         
        ///             GeoCoordinate coord = watcher.Position.Location;
        ///         
        ///             if (coord.IsUnknown != true)
        ///             {
        ///                 Debug.WriteLine("Lat: {0}, Long: {1}", coord.Latitude, coord.Longitude);
        ///             }
        ///             else
        ///             {
        ///                 Debug.WriteLine("Unknown latitude and longitude.");
        ///             }
        ///         }
        ///     }
        /// }
        /// </code></example>
        public bool TryStart(bool suppressPermissionPrompt, TimeSpan timeout)
        {
            if (!active)
            {
                active = OpenDevice();

                if (!active)
                {
                    status = GeoPositionStatus.Disabled;
                    return false;
                }

                Stopwatch st = new Stopwatch();
                st.Start();
                while (st.ElapsedMilliseconds < timeout.TotalMilliseconds)
                {
                    GetDeviceState(false);
                    if(status == GeoPositionStatus.Ready)
                        break;
                }
                st.Stop();

                // start worker thread
                System.Threading.Thread workerThread = new System.Threading.Thread(new System.Threading.ThreadStart(Worker));
                workerThread.Name = "InTheHand.Device.Location.GeoCoordinateWatcher_Worker";
                workerThread.IsBackground = true;
                workerThread.Start();

                return status == GeoPositionStatus.Ready;
            }

            return false;
        }

        /// <summary>
        /// Initiate the acquisition of data from the current location provider.
        /// This method enables <see cref="PositionChanged"/> events and allows access to the <see cref="Position"/> property.
        /// </summary>
        /// <remarks>Calling this method will initiate the acquisition of data from the current location provider.
        /// Currently only a GPS location provider is supported on .NET Compact Framework platforms.
        /// If the current prioritized location provider does not have data when the Start method is called, it will start to acquire data.
        /// Once data is available, data can be accessed synchronously, and will be delivered asynchronously if events are being handled.
        /// <para>If the GPS Intermediate Driver is not present, <see cref="Start()"/> will immediately return, <see cref="PositionChanged"/> events will not be raised, and the location returned by the <see cref="Location"/> property of <see cref="Position"/> will contain <see cref="GeoCoordinate.Unknown"/>.</para></remarks>
        public void Start()
        {
            Start(false);
        }

        /// <summary>
        /// Initiate the acquisition of data from the current location provider.
        /// This method enables <see cref="PositionChanged"/> events and allows access to the <see cref="Position"/> property.
        /// </summary>
        /// <param name="suppressPermissionPrompt">Not used.</param>
        public void Start(bool suppressPermissionPrompt)
        {
            if (active)
            {
                return;
                //already running
            }

            System.Diagnostics.Debug.WriteLine("InTheHand.Device.Location.GeoCoordinateWatcher.Start");

            active = OpenDevice();

            if (active)
            {
                //get initial device state
                GetDeviceState(false);

                //get initial position
                GetPosition(false);

                //start worker thread
                System.Threading.Thread workerThread = new System.Threading.Thread(new System.Threading.ThreadStart(Worker));
                workerThread.Name = "InTheHand.Device.Location.GeoCoordinateWatcher_Worker";
                workerThread.IsBackground = true;
                workerThread.Start();
            }
            else
            {
                status = GeoPositionStatus.Disabled;
            }
        }

        // converts GPSAPI status to System.Device status
        private static GeoPositionStatus ToGeoPositionStatus(NativeMethods.SERVICE_STATE state)
        {
            GeoPositionStatus newStatus = GeoPositionStatus.NoData;

            switch (state)
            {
                case NativeMethods.SERVICE_STATE.SERVICE_STATE_UNKNOWN:
                case NativeMethods.SERVICE_STATE.SERVICE_STATE_OFF:
                    newStatus = GeoPositionStatus.Disabled;
                    break;
                case NativeMethods.SERVICE_STATE.SERVICE_STATE_UNLOADING:
                case NativeMethods.SERVICE_STATE.SERVICE_STATE_UNINITIALIZED:
                case NativeMethods.SERVICE_STATE.SERVICE_STATE_SHUTTING_DOWN:
                    newStatus = GeoPositionStatus.NoData;
                    break;
                case NativeMethods.SERVICE_STATE.SERVICE_STATE_STARTING_UP:
                case NativeMethods.SERVICE_STATE.SERVICE_STATE_ON:
                    newStatus = GeoPositionStatus.Initializing;
                    break;
            }

            return newStatus;
        }

        private void Worker()
        {
            lock (this)
            {
                while (active)
                {
                    int result = InTheHand.Threading.EventWaitHandle.WaitAny(waitHandles);

                    if (result != -1)
                    {
                        switch (result)
                        {
                            case 0:
                                //stop
                                active = false;
                                break;

                            case 1:
                                //position changed
                                try
                                {
                                    GetPosition(true);
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine(ex);
                                }
                                break;

                            case 2:
                                //device state changed
                                lock (this)
                                {
                                    try
                                    {
                                        GetDeviceState(true);
                                    }
                                    catch (Exception ex)
                                    {
                                        System.Diagnostics.Debug.WriteLine(ex);
                                    }
                                }
                                break;
                        }//switch result

                    }//if

                }// while active

            }//lock
        }

        /// <summary>
        /// Stops the <see cref="GeoCoordinateWatcher"/> from providing location data and events.
        /// </summary>
        /// <remarks>After <see cref="Stop"/> is called, no further <see cref="PositionChanged"/> events occur, and the <see cref="Position"/> property will return <see cref="GeoCoordinate.Unknown"/>.
        /// When <see cref="Stop"/> is called, the <see cref="Status"/> property is set to <see cref="GeoPositionStatus.Disabled"/>.</remarks>
        public void Stop()
        {
            if (active)
            {
                this.active = false;

                System.Diagnostics.Debug.WriteLine("InTheHand.Device.Location.GeoCoordinateWatcher.Stop");

                //reset status
                this.status = GeoPositionStatus.Disabled;
                OnPositionStatusChanged(new GeoPositionStatusChangedEventArgs(GeoPositionStatus.Disabled));

                currentPosition = GeoCoordinate.Unknown;
                currentTimeStamp = new DateTimeOffset(0, new TimeSpan(0));

                //inform worker thread to stop
                if (waitHandles[0] != null)
                {
                    waitHandles[0].Set();
                }

                //worker thread must give up the lock
                lock (this)
                {
                    if (handle != IntPtr.Zero)
                    {
                        int hresult = NativeMethods.CloseDevice(handle);
                        handle = IntPtr.Zero;
                    }
                }
            }
        }

        private GeoCoordinate currentPosition = GeoCoordinate.Unknown;
        private DateTimeOffset currentTimeStamp = new DateTimeOffset(0, new TimeSpan(0));
        /// <summary>
        /// Gets the GeoCoordinate that indicates the current location.
        /// </summary>
        /// <value>The GeoCoordinate that indicates the current location.</value>
        public GeoPosition<GeoCoordinate> Position
        {
            get 
            { 
                return new GeoPosition<GeoCoordinate>(currentTimeStamp, currentPosition); 
            }
        }

        /// <summary>
        /// Indicates that the latitude or longitude of the location data has changed.
        /// </summary>
        public event EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>> PositionChanged;
        /// <summary>
        /// Indicates that the status of the GeoCoordinateWatcher object has changed.
        /// </summary>
        public event EventHandler<GeoPositionStatusChangedEventArgs> StatusChanged;

        #region IDisposable Members
        /// <summary>
        /// Free resources and perform other cleanup operations before the <see cref="GeoCoordinateWatcher"/> is reclaimed by garbage collection.
        /// </summary>
        ~GeoCoordinateWatcher()
        {
            GC.SuppressFinalize(this);
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            Stop();

            lock (this)
            {
                if (waitHandles[1] != null)
                {
                    waitHandles[1].Close();
                    waitHandles[1] = null;
                }

                if (waitHandles[2] != null)
                {
                    waitHandles[2].Close();
                    waitHandles[2] = null;
                }

                if (waitHandles[0] != null)
                {
                    waitHandles[0].Close();
                    waitHandles[0] = null;
                }
            }
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="GeoCoordinateWatcher"/> class.
        /// </summary>
        void IDisposable.Dispose()
        {
            Dispose(true);
        }
        #endregion

        #region INotifyPropertyChanged Members
        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                try
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
                catch { }
            }
        }
        #endregion
    }
}
