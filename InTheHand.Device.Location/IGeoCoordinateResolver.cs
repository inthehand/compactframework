// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGeoCoordinateResolver.cs" company="In The Hand Ltd">
// Copyright (c) 2010-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using InTheHand.ComponentModel;

namespace InTheHand.Device.Location
{
    /// <summary>
    /// Provides a method that when implemented, resolves a civic address to a latitude/longitude location. 
    /// </summary>
    /// <remarks>
    /// To resolve a <see cref="CivicAddress"/> to a <see cref="GeoCoordinate"/> asynchronously, implement <see cref="ResolveCoordinateAsync"/>, and provide the co-ordinate data to the <see cref="ResolveCoordinateCompleted"/> event.</remarks>
    public interface IGeoCoordinateResolver
    {
        /// <summary>
        /// Resolves a <see cref="CivicAddress"/> to a co-ordinate location synchronously.
        /// </summary>
        /// <param name="address">The address (or partial address) to resolve to a latitude/longitude location.</param>
        /// <returns>The <see cref="GeoCoordinate"/> which matches the supplied address.</returns>
        GeoCoordinate ResolveCoordinate(CivicAddress address);

        /// <summary>
        /// Initiates a request to resolve an address to a latitude/longitude location .
        /// </summary>
        /// <param name="address">The address (or partial address) to resolve to a latitude/longitude location.</param>
        void ResolveCoordinateAsync(CivicAddress address);

        /// <summary>
        /// Occurs when an asynchronous request using <see cref="ResolveCoordinateAsync"/> to resolve a civic address to a latitude/longitude is complete.
        /// </summary>
        event EventHandler<ResolveCoordinateCompletedEventArgs> ResolveCoordinateCompleted;
    }

    /// <summary>
    /// Provides data for the <see cref="IGeoCoordinateResolver.ResolveCoordinateCompleted"/> event.
    /// </summary>
    public class ResolveCoordinateCompletedEventArgs : AsyncCompletedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="coordinates">The <see cref="GeoCoordinate"/> containing the latitude/longitude resolved from the address, if successful.</param>
        /// <param name="error">The exception object for any exception that may have occurred during the attempt to resolve the location.</param>
        /// <param name="cancelled">true if the operation was cancelled; otherwise, false.</param>
        /// <param name="userState">A token for tracking the request to resolve the address. 
        /// May be null if not used.</param>
        /// <remarks>This constructor is public so that classes implementing the <see cref="ICivicAddressResolver"/> interface can create this object to fire events.</remarks>
        public ResolveCoordinateCompletedEventArgs(GeoCoordinate coordinates, Exception error, bool cancelled, Object userState)
            : base(error, cancelled, userState)
        {
            this.Coordinates = coordinates;
        }

        /// <summary>
        /// Gets the <see cref="GeoCoordinate"/> associated with the <see cref="IGeoCoordinateResolver.ResolveCoordinateCompleted"/> event.
        /// </summary>
        public GeoCoordinate Coordinates { get; private set; }
    }
}