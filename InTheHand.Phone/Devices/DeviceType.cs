// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeviceType.cs" company="In The Hand Ltd">
// Copyright (c) 2010-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

namespace InTheHand.Devices
{
    /// <summary>
    /// Defines the device type values used by the <see cref="Environment.DeviceType"/> property.
    /// </summary>
    public enum DeviceType
    {
        /// <summary>
        /// The device type is a device emulator.
        /// </summary>
        Emulator,

        /// <summary>
        /// The device type is an actual device.
        /// </summary>
        Device,
    }
}