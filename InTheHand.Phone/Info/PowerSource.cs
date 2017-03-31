// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PowerSource.cs" company="In The Hand Ltd">
// Copyright (c) 2010-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Phone.Info
{
    /// <summary>
    /// Indicates if the device is currently running on battery power or is plugged in to an external power supply.
    /// </summary>
    public enum PowerSource
    {
        /// <summary>
        /// The device is running on battery power.
        /// </summary>
        Battery = 0,

        /// <summary>
        /// The device is plugged in to an external power source, such as being docked to a computer or connected to a power supply.
        /// </summary>
        External = 1,
    }
}
