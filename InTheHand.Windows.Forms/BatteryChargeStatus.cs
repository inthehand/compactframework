// In The Hand - .NET Components for Mobility
//
// InTheHand.Windows.Forms.BatteryChargeStatus
// 
// Copyright (c) 2002-2010 In The Hand Ltd, All rights reserved.

using System;

namespace InTheHand.Windows.Forms
{
	/// <summary>
	/// Defines identifiers that indicate the current battery charge level or charging state information.
	/// </summary>
    /// <remarks>This enumeration is used by the <see cref="PowerStatus.BatteryChargeStatus"/> property of the <see cref="PowerStatus"/> class.</remarks>
	[Flags()]
	public enum BatteryChargeStatus : byte
	{
		/// <summary>
		/// Indicates a high level of battery charge.
		/// </summary>
		High = 0x01,

		/// <summary>
		/// Indicates a low level of battery charge.
		/// </summary>
		Low	= 0x02,

		/// <summary>
		/// Indicates a critically low level of battery charge.
		/// </summary>
		Critical = 0x04,

		/// <summary>
		/// Indicates a battery is charging.
		/// </summary>
		Charging = 0x08,

		/// <summary>
		/// Indicates that no battery is present.
		/// </summary>
		NoSystemBattery	= 0x80,

		/// <summary>
		/// Indicates an unknown battery condition.
		/// </summary>
		Unknown	= 0xFF,
	}
}
