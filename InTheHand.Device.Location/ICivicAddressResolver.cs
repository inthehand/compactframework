// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICivicAddressResolver.cs" company="In The Hand Ltd">
// Copyright (c) 2010-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace InTheHand.Device.Location
{
    /// <summary>
    /// Provides a method that when implemented, resolves a latitude/longitude location to a civic address. 
    /// </summary>
    /// <remarks><para>Equivalent to System.Device.Location.ICivicAddressResolver in the .NET Framework 4</para>
    /// To resolve a <see cref="GeoCoordinate"/> to a <see cref="CivicAddress"/> asynchronously, implement <see cref="ResolveAddressAsync"/>, and provide the civic address data to the <see cref="ResolveAddressCompleted"/> event.</remarks>
    public interface ICivicAddressResolver
    {
        /// <summary>
        /// Resolves a <see cref="GeoCoordinate"/> to a civic address synchronously.
        /// </summary>
        /// <param name="coordinate">The latitude/longitude location to resolve to an address.</param>
        /// <returns></returns>
        CivicAddress ResolveAddress(GeoCoordinate coordinate);

        /// <summary>
        /// Initiates a request to resolve a latitude/longitude location to an address.
        /// </summary>
        /// <param name="coordinate">The latitude/longitude location to resolve to an address.</param>
        void ResolveAddressAsync(GeoCoordinate coordinate);

        /// <summary>
        /// Occurs when an asynchronous request using <see cref="ResolveAddressAsync"/> to resolve a latitude/longitude to a civic address is complete.
        /// </summary>
        event EventHandler<ResolveAddressCompletedEventArgs> ResolveAddressCompleted;
    }

    /// <summary>
    /// Provides data for the <see cref="ICivicAddressResolver.ResolveAddressCompleted"/> event.
    /// </summary>
    public class ResolveAddressCompletedEventArgs : AsyncCompletedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address">The <see cref="CivicAddress"/> containing the address resolved from the latitude/longitude location, if successful.</param>
        /// <param name="error">The exception object for any exception that may have occurred during the attempt to resolve the address.</param>
        /// <param name="cancelled">true if the operation was cancelled; otherwise, false.</param>
        /// <param name="userState">A token for tracking the request to resolve the address. 
        /// May be null if not used.</param>
        /// <remarks>This constructor is public so that classes implementing the <see cref="ICivicAddressResolver"/> interface can create this object to fire events.</remarks>
        public ResolveAddressCompletedEventArgs(CivicAddress address, Exception error, bool cancelled, Object userState) : base(error, cancelled, userState)
        {
            this.Address = address;
        }

        /// <summary>
        /// Gets the <see cref="CivicAddress"/> associated with the <see cref="ICivicAddressResolver.ResolveAddressCompleted"/> event.
        /// </summary>
        public CivicAddress Address { get; private set; }
    }
}