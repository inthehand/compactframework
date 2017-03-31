// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeoCoordinate.cs" company="In The Hand Ltd">
// Copyright (c) 2010-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace InTheHand.Device.Location
{
    /// <summary>
    /// Represents a geographical location determined by latitude and longitude coordinates.
    /// May also include altitude, accuracy, speed and course information.
    /// </summary>
    /// <remarks><para>Equivalent to System.Device.Location.GeoCoordinate in the .NET Framework 4</para></remarks>
    public class GeoCoordinate : IEquatable<GeoCoordinate>
    {
        private double latitude;
        private double longitude;
        private double altitude;
        private double horizontalAccuracy;
        private double verticalAccuracy;
        private double course;
        private double speed;

        /// <summary>
        /// Initializes a new instance of <see cref="GeoCoordinate"/> with no data fields set.
        /// </summary>
        /// <remarks>All data fields will be set to <see cref="Double.NaN"/>.
        /// The new instance of <see cref="GeoCoordinate"/> is equivalent to <see cref="Unknown"/>.</remarks>
        public GeoCoordinate()
        {
            this.latitude = double.NaN;
            this.longitude = double.NaN;
            this.altitude = double.NaN;
            this.horizontalAccuracy = double.NaN;
            this.verticalAccuracy = double.NaN;
            this.course = double.NaN;
            this.speed = double.NaN;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoCoordinate"/> class from latitude and longitude data.
        /// </summary>
        /// <param name="latitude">The latitude of the location.
        /// May range from -90.0 to 90.0.</param>
        /// <param name="longitude">The longitude of the location.
        /// May range from -180.0 to 180.0.</param>
        /// <exception cref="ArgumentOutOfRangeException">latitude or longitude is out of range.</exception>
        /// <remarks>The latitude and longitude given must correspond to an actual location on the globe.</remarks>
        public GeoCoordinate(double latitude, double longitude)
            : this(latitude, longitude, double.NaN) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoCoordinate"/> class from latitude, longitude and altitude data.
        /// </summary>
        /// <param name="latitude">The latitude of the location.
        /// May range from -90.0 to 90.0.</param>
        /// <param name="longitude">The longitude of the location.
        /// May range from -180.0 to 180.0.</param>
        /// <param name="altitude">The altitude in meters.
        /// May be negative, 0, positive, or NaN, if unknown.</param>
        /// <exception cref="ArgumentOutOfRangeException">latitude, longitude or altitude is out of range.</exception>
        /// <remarks>The latitude and longitude given must correspond to an actual location on the globe.</remarks>
        public GeoCoordinate(double latitude, double longitude, double altitude)
            : this(latitude, longitude, altitude, double.NaN, double.NaN, double.NaN, double.NaN) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoCoordinate"/> class from latitude, longitude, altitude, horizontal accuracy, vertical accuracy, speed and course.
        /// </summary>
        /// <param name="latitude">The latitude of the location.
        /// May range from -90.0 to 90.0.</param>
        /// <param name="longitude">The longitude of the location.
        /// May range from -180.0 to 180.0.</param>
        /// <param name="altitude">The altitude in meters.
        /// May be negative, 0, positive, or <see cref="Double.NaN"/>, if unknown.</param>
        /// <param name="horizontalAccuracy">The accuracy of the latitude/longitude coordinates, in meters.
        /// Must be greater than or equal to 0.
        /// If a value of 0 is supplied to this constructor, the <see cref="HorizontalAccuracy"/> property will be set to <see cref="Double.NaN"/>.</param>
        /// <param name="verticalAccuracy">The accuracy of the altitude, in meters.
        /// Must be greater than or equal to 0.
        /// If a value of 0 is supplied to this constructor, the <see cref="VerticalAccuracy"/> property will be set to <see cref="Double.NaN"/>.</param>
        /// <param name="speed">The speed measured in meters per second.
        /// May be negative, 0, positive, or <see cref="Double.NaN"/>, if unknown.
        /// A negative speed can indicate moving in reverse.</param>
        /// <param name="course">The direction of travel, rather than orientation.
        /// This parameter is measured in degrees relative to true north. 
        /// Must range from 0 to 360.0, or be <see cref="Double.NaN"/>. </param>
        /// <exception cref="ArgumentOutOfRangeException">latitude, longitude, horizontalAccuracy, verticalAccuracy or course is out of range.</exception>
        public GeoCoordinate(double latitude, double longitude, double altitude, double horizontalAccuracy, double verticalAccuracy, double speed, double course)
        {
            if ((double.IsNaN(latitude) || (latitude > 90.0)) || (latitude < -90.0))
            {
                throw new ArgumentOutOfRangeException("latitude", Properties.Resources.Argument_MustBeInRangeNegative90to90);
            }

            if ((double.IsNaN(longitude) || (longitude > 180.0)) || (longitude < -180.0))
            {
                throw new ArgumentOutOfRangeException("longitude", Properties.Resources.Argument_MustBeInRangeNegative180To180);
            }

            if (horizontalAccuracy < 0.0)
            {
                throw new ArgumentOutOfRangeException("horizontalAccuracy", Properties.Resources.Argument_MustBeNonNegative);
            }

            if (verticalAccuracy < 0.0)
            {
                throw new ArgumentOutOfRangeException("verticalAccuracy", Properties.Resources.Argument_MustBeNonNegative);
            }

            if (course < 0.0 || course > 360.0)
            {
                throw new ArgumentOutOfRangeException("course", Properties.Resources.Argument_MustBeInRangeZeroTo360);
            }

            this.latitude = latitude;
            this.longitude = longitude;
            this.altitude = altitude;
            this.horizontalAccuracy = (horizontalAccuracy == 0.0) ? double.NaN : horizontalAccuracy;
            this.verticalAccuracy = (verticalAccuracy == 0.0) ? double.NaN : verticalAccuracy;
            this.speed = speed;
            this.course = course;           
        }

        /// <summary>
        /// Gets or sets the latitude of the <see cref="GeoCoordinate"/>.
        /// </summary>
        /// <value>Latitude of the location.</value>
        /// <exception cref="ArgumentOutOfRangeException"><see cref="Latitude"/> is set outside the valid range.</exception>
        /// <remarks>Latitude may range from -90.0 to 90.0.
        /// <see cref="Latitude"/> is measured in degrees north or south from the equator.
        /// Positive values are north of the equator and negative values are south of the equator.</remarks>
        public double Latitude
        {
            get 
            { 
                return latitude; 
            }

            set
            {
                if (value < -90.0 || value > 90.0)
                {
                    throw new ArgumentOutOfRangeException(Properties.Resources.Argument_MustBeInRangeNegative90to90);
                }

                latitude = value;
            }
        }

        /// <summary>
        /// Gets or sets the longitude of the <see cref="GeoCoordinate"/>.
        /// </summary>
        /// <value>Longitude of the location.</value>
        /// <exception cref="ArgumentOutOfRangeException">Longitude is set outside the valid range.</exception>
        /// <remarks>The longitude may range from -180.0 to 180.0.
        /// Longitude is measured in degrees east or west of the prime meridian.
        /// Negative values are west of the prime meridian, and positive values are east of the prime meridian.</remarks>
        public double Longitude
        {
            get 
            { 
                return longitude; 
            }

            set
            {
                if (value < -180.0 || value > 180.0)
                {
                    throw new ArgumentOutOfRangeException(Properties.Resources.Argument_MustBeInRangeNegative180To180);
                }

                longitude = value;
            }
        }

        /// <summary>
        /// Gets the altitude of the <see cref="GeoCoordinate"/>, in meters.
        /// </summary>
        /// <value>The altitude, in meters.</value>
        /// <remarks>The altitude is given relative to sea level.</remarks>
        public double Altitude
        {
            get 
            { 
                return altitude; 
            }

            set 
            { 
                altitude = value; 
            }
        }

        /// <summary>
        /// Gets or sets the accuracy of the latitude and longitude given by the <see cref="GeoCoordinate"/>, in meters.
        /// </summary>
        /// <value>The accuracy of the latitude and longitude, in meters.</value>
        /// <remarks>The accuracy can be considered the radius of certainty of the latitude/longitude data.
        /// A circular area that is formed with the accuracy as the radius and the latitude/longitude coordinates as the center contains the actual location.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">The argument must be non-negative.</exception>
        public double HorizontalAccuracy
        {
            get 
            { 
                return horizontalAccuracy; 
            }

            set
            {
                if (value < 0.0)
                {
                    throw new ArgumentOutOfRangeException(Properties.Resources.Argument_MustBeNonNegative);
                }

                horizontalAccuracy = value;
            }
        }

        /// <summary>
        /// Gets or sets the accuracy of the altitude given by the <see cref="GeoCoordinate"/>, in meters.
        /// </summary>
        /// <value>The accuracy of the altitude, in meters.</value>
        /// <remarks>The horizontal accuracy value must be nonnegative.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">The argument must be non-negative.</exception>
        public double VerticalAccuracy
        {
            get 
            { 
                return verticalAccuracy; 
            }

            set
            {
                if (value < 0.0)
                {
                    throw new ArgumentOutOfRangeException(Properties.Resources.Argument_MustBeNonNegative);
                }

                verticalAccuracy = value;
            }
        }

        /// <summary>
        /// Gets or sets the heading in degrees relative to true north.
        /// </summary>
        /// <value>The heading in degrees relative to true north.</value>
        /// <exception cref="ArgumentOutOfRangeException">The argument must be in the range 0.0 to 360.0.</exception>
        /// <remarks>The course value must be between 0.0 and 360.0, and <see cref="Double.NaN"/> if the heading is not defined.</remarks>
        public double Course
        {
            get
            {
                return this.course;
            }

            set
            {
                if (value < 0.0 || value > 360.0)
                {
                    throw new ArgumentOutOfRangeException(Properties.Resources.Argument_MustBeInRangeZeroTo360);
                }

                course = value;
            }
        }

        /// <summary>
        /// Gets or sets the speed in meters per second.
        /// </summary>
        /// <value>The speed in meters per second.
        /// The speed must be greater than or equal to zero, or <see cref="Double.NaN"/>.</value>
        /// <exception cref="ArgumentOutOfRangeException">The argument must be non-negative.</exception>
        /// <example>
        /// The following example prints the Course and Speed properties of the current location's GeoCoordinate.
        /// <code lang="cs">
        /// static void GetLocationCourseAndSpeed()
        /// {
        ///     GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
        ///     watcher.Start();
        ///     System.Threading.Thread.Sleep(1000);
        ///     if (watcher.Position.Location.IsUnknown != true)
        ///     {
        ///         GeoCoordinate coord = watcher.Position.Location;
        ///         Debug.WriteLine("Course: {0}, Speed: {1}", coord.Course, coord.Speed);
        ///     }
        ///     else
        ///     {
        ///         Debug.WriteLine("Unknown");
        ///     }
        /// }
        /// </code></example>
        public double Speed
        {
            get
            {
                return speed;
            }

            set
            {
                if (value != double.NaN && value < 0.0)
                {
                    throw new ArgumentOutOfRangeException(Properties.Resources.Argument_MustBeNonNegative);
                }

                speed = value;
            }
        }

        #region IEquatable<GeoCoordinate> Members
        /// <summary>
        /// Determines if a specified GeoCoordinate is equal to the current GeoCoordinate, based solely on latitude and longitude.
        /// </summary>
        /// <param name="obj">The object to compare the GeoCoordinate to.</param>
        /// <returns>true, if the GeoCoordinate objects are equal; otherwise, false.</returns>
        /// <remarks>Equivalent GeoCoordinate objects have the same <see cref="Latitude"/> and <see cref="Longitude"/> properties.
        /// The <see cref="Altitude"/>, <see cref="HorizontalAccuracy"/>, and <see cref="VerticalAccuracy"/> properties are not used in determining equivalency.</remarks>
        public override bool Equals(object obj)
        {
            if (obj is GeoCoordinate)
            {
                return this.Equals(obj as GeoCoordinate);
            }

            return base.Equals(obj);
        }
        /// <summary>
        /// Determines if the GeoCoordinate object is equivalent to the parameter, based solely on latitude and longitude.
        /// </summary>
        /// <param name="other">The GeoCoordinate object to compare to the calling object.</param>
        /// <returns>true if the GeoCoordinate objects are equal; otherwise, false.</returns>
        /// <remarks>Equivalent GeoCoordinate objects have the same <see cref="Latitude"/> and <see cref="Longitude"/> properties.
        /// The <see cref="Altitude"/>, <see cref="HorizontalAccuracy"/>, and <see cref="VerticalAccuracy"/> properties are not used in determining equivalency.</remarks>
        public bool Equals(GeoCoordinate other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            return (this.Latitude.Equals(other.Latitude) && this.Longitude.Equals(other.Longitude));
        }       
        #endregion

        /// <summary>
        /// Serves as a hash function for the <see cref="GeoCoordinate"/>.
        /// </summary>
        /// <returns>A hash code for the current <see cref="GeoCoordinate"/>.</returns>
        /// <remarks>GeoCoordinate objects that are equivalent have the same hash code.
        /// Equivalent <see cref="GeoCoordinate"/> objects have the same <see cref="Latitude"/> and <see cref="Longitude"/> properties.
        /// The <see cref="Altitude"/>, <see cref="HorizontalAccuracy"/>, and <see cref="VerticalAccuracy"/> properties are not used in determining equivalency.</remarks>
        public override int GetHashCode()
        {
            return (this.Latitude.GetHashCode() ^ this.Longitude.GetHashCode());
        }

        /// <summary>
        /// Returns a string containing the latitude and longitude.
        /// </summary>
        /// <returns>A string containing the latitude and longitude, separated by a comma.</returns>
        /// <remarks>The string representation given by this method is intended only for debugging use.
        /// This method will not return any locale-specific formatting for latitude and longitude.</remarks>
        public override string ToString()
        {
            if (this == Unknown)
            {
                return Properties.Resources.Unknown;
            }

            return (this.Latitude.ToString("G", CultureInfo.InvariantCulture) + ", " + this.Longitude.ToString("G", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Represents a <see cref="GeoCoordinate"/> object with unknown <see cref="Latitude"/> and <see cref="Longitude"/> fields.
        /// </summary>
        /// <remarks>The <see cref="IsUnknown"/> property can be used to check whether a <see cref="GeoCoordinate"/> contains no data.</remarks>
        public static readonly GeoCoordinate Unknown = new GeoCoordinate();

        /// <summary>
        /// Gets a value that indicates whether the <see cref="GeoCoordinate"/> does not contain <see cref="Latitude"/> or <see cref="Longitude"/> data.
        /// </summary>
        /// <value>true if the <see cref="GeoCoordinate"/> does not contain <see cref="Latitude"/> or <see cref="Longitude"/> data; otherwise, false.</value>
        public bool IsUnknown
        {
            get
            {
                return (double.IsNaN(this.latitude) && double.IsNaN(this.longitude));
            }
        }
 
        /// <summary>
        /// Returns the distance between the latitude/longitude coordinate specified by this <see cref="GeoCoordinate"/> and another specified <see cref="GeoCoordinate"/>.
        /// </summary>
        /// <param name="other">The <see cref="GeoCoordinate"/> for the location to calculate the distance to.</param>
        /// <returns>The distance between the two coordinates, in meters.</returns>
        /// <remarks>The Haversine formula is used to calculate the distance.
        /// The Haversine formula accounts for the curvature of the earth, but assumes a spherical earth rather than an ellipsoid.
        /// For long distances, the Haversine formula introduces an error of less than 0.1 percent.
        /// <para><see cref="Altitude"/> is not used to calculate the distance.</para></remarks>
        /// <exception cref="ArgumentException">Latitude or longitude is not a number (NaN).</exception>
        public double GetDistanceTo(GeoCoordinate other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            if ((double.IsNaN(this.Latitude) || double.IsNaN(this.Longitude)) || (double.IsNaN(other.Latitude) || double.IsNaN(other.Longitude)))
            {
                throw new ArgumentException(Properties.Resources.Argument_LatitudeOrLongitudeIsNotANumber);
            }

            double R = 6371000;//earth radius in metres
            double dLat = ToRadians(other.Latitude - this.Latitude);
            double dLon = ToRadians(other.Longitude - this.Longitude);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(ToRadians(this.Latitude)) * Math.Cos(ToRadians(other.Latitude)) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;
            return d;
        }

        // converts from degrees to radians
        private static double ToRadians(double degrees)
        {
            return (Math.PI / 180) * degrees;
        }

        /// <summary>
        /// Determines whether two <see cref="GeoCoordinate"/> objects refer to the same location.
        /// </summary>
        /// <param name="left">The first <see cref="GeoCoordinate"/> to compare.</param>
        /// <param name="right">The second <see cref="GeoCoordinate"/> to compare.</param>
        /// <returns>true, if the <see cref="GeoCoordinate"/> objects are determined to be equivalent; otherwise, false.</returns>
        /// <remarks>Equivalent <see cref="GeoCoordinate"/> objects have the same <see cref="Latitude"/> and <see cref="Longitude"/> properties.
        /// The <see cref="Altitude"/>, <see cref="HorizontalAccuracy"/>, and <see cref="VerticalAccuracy"/> properties are not used in determining equivalency.</remarks>
        public static bool operator ==(GeoCoordinate left, GeoCoordinate right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="GeoCoordinate"/> objects correspond to different locations.
        /// </summary>
        /// <param name="left">The first <see cref="GeoCoordinate"/> to compare.</param>
        /// <param name="right">The second <see cref="GeoCoordinate"/> to compare.</param>
        /// <returns>true, if the <see cref="GeoCoordinate"/> objects are determined to be different; otherwise, false.</returns>
        /// <remarks>Equivalent <see cref="GeoCoordinate"/> objects have the same <see cref="Latitude"/> and <see cref="Longitude"/> properties.
        /// The <see cref="Altitude"/>, <see cref="HorizontalAccuracy"/>, and <see cref="VerticalAccuracy"/> properties are not used in determining equivalency.</remarks>
        public static bool operator !=(GeoCoordinate left, GeoCoordinate right)
        {
            return !(left == right);
        }
    }
}