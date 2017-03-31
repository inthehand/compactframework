// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGeoPositionWatcher.cs" company="In The Hand Ltd">
// Copyright (c) 2010-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace InTheHand.Device.Location
{
    /// <summary>
    /// Interface that can be implemented for providing accessing location data and receiving location updates.
    /// </summary>
    /// <typeparam name="T">The type of the object that contains the location data.</typeparam>
    /// <remarks><para>Equivalent to System.Device.Location.IGeoPositionWatcher&lt;T&gt; in the .NET Framework 4</para>
    /// The <see cref="GeoCoordinateWatcher"/> class implements <see cref="IGeoPositionWatcher{T}"/>, using <see cref="GeoCoordinate"/> as the type parameter.</remarks>
    public interface IGeoPositionWatcher<T>
    {
        /// <summary>
        /// Initiate the acquisition of location data.
        /// </summary>
        void Start();

        /// <summary>
        /// Start acquiring location data, specifying whether or not to suppress prompting for permissions.
        /// This method returns synchronously.
        /// </summary>
        /// <param name="suppressPermissionPrompt">If true, do not prompt the user to enable location providers and only start if location data is already enabled. 
        /// If false, a dialog box may be displayed to prompt the user to enable location sensors that are disabled.</param>
        void Start(bool suppressPermissionPrompt);

        /// <summary>
        /// Stop acquiring location data.
        /// </summary>
        void Stop();

        /// <summary>
        /// Start acquiring location data, specifying an initialization timeout.
        /// This method returns synchronously.
        /// </summary>
        /// <param name="suppressPermissionPrompt">If true, do not prompt the user to enable location providers and only start if location data is already enabled. 
        /// If false, a dialog box may be displayed to prompt the user to enable location sensors that are disabled.</param>
        /// <param name="timeout">Time in milliseconds to wait for initialization to complete.</param>
        /// <returns>true if succeeded, false if timed out.</returns>
        bool TryStart(bool suppressPermissionPrompt, TimeSpan timeout);

        /// <summary>
        /// Gets the location data.
        /// </summary>
        /// <value>The <see cref="GeoPosition{T}"/> containing the location data.</value>
        GeoPosition<T> Position { get; }

        /// <summary>
        /// Gets the status of location data.
        /// </summary>
        /// <value>The status of location data.</value>
        GeoPositionStatus Status { get; }

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<GeoPositionChangedEventArgs<T>> PositionChanged;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<GeoPositionStatusChangedEventArgs> StatusChanged;
    }

    /// <summary>
    /// Contains data for a <see cref="IGeoPositionWatcher{T}.StatusChanged"/> event.
    /// </summary>
    public class GeoPositionStatusChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoPositionStatusChangedEventArgs"/> class.
        /// </summary>
        /// <param name="status">The new status.</param>
        public GeoPositionStatusChangedEventArgs(GeoPositionStatus status)
        {
            this.Status = status;
        }

        /// <summary>
        /// Gets the updated status.
        /// </summary>
        /// <value>The updated status.</value>
        public GeoPositionStatus Status { get; private set; }
    }

    /// <summary>
    /// Provides data for the <see cref="IGeoPositionWatcher{T}.PositionChanged"/> event.
    /// </summary>
    /// <typeparam name="T">The type of the location data in the Location property of this event's <see cref="Position"/> property</typeparam>
    public class GeoPositionChangedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoPositionChangedEventArgs{T}"/> class
        /// </summary>
        /// <param name="position">The updated position.</param>
        public GeoPositionChangedEventArgs(GeoPosition<T> position)
        {
            this.Position = position;
        }

        /// <summary>
        /// Gets the location data associated with the event.
        /// </summary>
        /// <value>A <see cref="GeoPosition{T}"/> object that contains the location data in its <see cref="GeoPosition{T}.Location"/> property.</value>
        public GeoPosition<T> Position { get; private set; }
    }
}