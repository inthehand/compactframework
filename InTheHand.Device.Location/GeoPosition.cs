// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeoPosition.cs" company="In The Hand Ltd">
// Copyright (c) 2010-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Device.Location
{
    /// <summary>
    /// Contains location data of a type specified by the type parameter of the <see cref="GeoPosition{T}"/> class.
    /// </summary>
    /// <typeparam name="T">The type of the location data.</typeparam>
    /// <remarks><para>Equivalent to System.Device.Location.GeoPosition&lt;T&gt; in the .NET Framework 4</para></remarks>
    public class GeoPosition<T>
    {
        DateTimeOffset timestamp;
        T position;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoPosition{T}"/> class.
        /// </summary>
        public GeoPosition()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoPosition{T}"/> class with a timestamp and position.
        /// </summary>
        /// <param name="timestamp">The time the location data was obtained.</param>
        /// <param name="position">The location data to use to initialize the <see cref="GeoPosition{T}"/> object.</param>
        public GeoPosition(DateTimeOffset timestamp, T position)
        {
            this.timestamp = timestamp;
            this.position = position;
        }

        /// <summary>
        /// Gets or sets the location data for the <see cref="GeoPosition{T}"/> object.
        /// </summary>
        /// <value>An object of type T that contains the location data for the <see cref="GeoPosition{T}"/> object.</value>
        public T Location
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        /// <summary>
        /// Gets or sets the time when the location data was obtained.
        /// </summary>
        /// <value>Gets or sets the time when the location data was obtained.</value>
        public DateTimeOffset Timestamp
        {
            get
            {
                return timestamp;
            }
            set
            {
                timestamp = value;
            }
        }
    }
}