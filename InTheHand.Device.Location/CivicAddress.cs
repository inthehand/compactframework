// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CivicAddress.cs" company="In The Hand Ltd">
// Copyright (c) 2010-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using InTheHand;

namespace InTheHand.Device.Location
{
    /// <summary>
    /// Represents a civic address.
    /// A civic address can include fields such as street address, postal code, state/province, and country or region.
    /// </summary>
    /// <remarks><para>Equivalent to System.Device.Location.CivicAddress in the .NET Framework 4</para>
    /// A civic address for a location can be obtained from a <see cref="GeoCoordinate"/> by using a class that implements <see cref="ICivicAddressResolver"/>. 
    /// <para><see cref="ICivicAddressResolver.ResolveAddress"/> returns a <see cref="CivicAddress"/> for the current location.
    /// If the location source is unable to resolve the coordinate position to a civic address, <see cref="Unknown"/> is returned.</para></remarks>
    public class CivicAddress
    {
        /// <summary>
        /// Represents a <see cref="CivicAddress"/> that contains no data.
        /// </summary>
        public static readonly CivicAddress Unknown = new CivicAddress();

        /// <summary>
        /// Initializes a new instance of the <see cref="CivicAddress"/> class.
        /// </summary>
        /// <remarks>All fields are initialized to <see cref="String.Empty"/>.</remarks>
        public CivicAddress()
        {
            AddressLine1 = string.Empty;
            AddressLine2 = string.Empty;
            Building = string.Empty;
            City = string.Empty;
            CountryRegion = string.Empty;
            FloorLevel = string.Empty;
            PostalCode = string.Empty;
            StateProvince = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CivicAddress"/> class with address information.
        /// </summary>
        /// <param name="addressLine1">A <see cref="String"/> containing the first line of the street address.</param>
        /// <param name="addressLine2">A <see cref="String"/> containing the second line of the street address.</param>
        /// <param name="building">A <see cref="String"/> containing the building name or number.</param>
        /// <param name="city">A <see cref="String"/> containing the city.</param>
        /// <param name="countryRegion">A <see cref="String"/> containing the country or region. =</param>
        /// <param name="floorLevel">A <see cref="String"/> containing the floor number.</param>
        /// <param name="postalCode">A <see cref="String"/> containing the postal code.</param>
        /// <param name="stateProvince">A <see cref="String"/> containing the state or province.</param>
        /// <exception cref="ArgumentException">At least one parameter must be a non-empty string.</exception>
        public CivicAddress(string addressLine1, string addressLine2, string building, string city, 
            string countryRegion, string floorLevel, string postalCode, string stateProvince) : this()
        {
            bool flag = false;
            if (!string.IsNullOrEmpty(addressLine1))
            {
                this.AddressLine1 = addressLine1;
                flag = true;
            }

            if(!string.IsNullOrEmpty(addressLine2))
            {
                this.AddressLine2 = addressLine2;
                flag = true;
            }

            if(!string.IsNullOrEmpty(building))
            {
                this.Building = building;
                flag = true;
            }

            if(!string.IsNullOrEmpty(city))
            {
                this.City = city;
                flag = true;
            }
                
            if(!string.IsNullOrEmpty(countryRegion))
            {
                this.CountryRegion = countryRegion;
                flag = true;
            }
             
            if(!string.IsNullOrEmpty(floorLevel))
            {
                this.FloorLevel = floorLevel;
                flag = true;
            }
                
            if(!string.IsNullOrEmpty(postalCode))
            {
                this.PostalCode = postalCode;
                flag = true;
            }

            if(!string.IsNullOrEmpty(stateProvince))
            {
                this.StateProvince = stateProvince;
                flag = true;
            }

            //if none of the arguments were set throw an exception
            if (!flag)
            {
                throw new ArgumentException(Properties.Resources.Argument_RequiresAtLeastOneNonEmptyStringParameter);
            }
        }

        /// <summary>
        /// Gets or sets the first line of the address.
        /// </summary>
        public string AddressLine1
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the second line of the address.
        /// </summary>
        public string AddressLine2
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the building name or number.
        /// </summary>
        public string Building
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        public string City
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the country/region of the location.
        /// </summary>
        public string CountryRegion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the floor level of the location.
        /// </summary>
        public string FloorLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value that indicates whether the <see cref="CivicAddress"/> contains data.
        /// </summary>
        public bool IsUnknown
        {
            get
            {
                return ((((string.IsNullOrEmpty(this.AddressLine1) && string.IsNullOrEmpty(this.AddressLine2)) && (string.IsNullOrEmpty(this.Building) && string.IsNullOrEmpty(this.City))) && ((string.IsNullOrEmpty(this.CountryRegion) && string.IsNullOrEmpty(this.FloorLevel)) && string.IsNullOrEmpty(this.PostalCode))) && string.IsNullOrEmpty(this.StateProvince));
            }
        }

        /// <summary>
        /// Gets or sets the postal code of the location.
        /// </summary>
        public string PostalCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the state or province of the location.
        /// </summary>
        public string StateProvince
        {
            get;
            set;
        }
    }
}