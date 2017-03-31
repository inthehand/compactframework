// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeoPositionStatus.cs" company="In The Hand Ltd">
// Copyright (c) 2010-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Device.Location
{
    /// <summary>
    /// Indicates the ability of the location provider to provide location updates.
    /// </summary>
    public enum GeoPositionStatus
    {
        /// <summary>
        /// A location provider is ready to supply new data.
        /// </summary>
        Ready,

        /// <summary>
        /// The location provider is initializing.
        /// For example, a GPS still obtaining a fix has this status.
        /// </summary>
        Initializing,

        /// <summary>
        /// There are no devices than can currently resolve location. 
        /// If the conditions for Disabled do not apply, <see cref="GeoCoordinateWatcher"/> has this status before it has been started and after it has been stopped.
        /// </summary>
        NoData,

        /// <summary>
        /// The location system feature has been disabled.
        /// </summary>
        Disabled,
    }
}
