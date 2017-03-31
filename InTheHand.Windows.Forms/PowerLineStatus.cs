// In The Hand - .NET Components for Mobility
//
// InTheHand.Windows.Forms.PowerLineStatus
// 
// Copyright (c) 2002-2010 In The Hand Ltd, All rights reserved.

using System;

namespace InTheHand.Windows.Forms
{
	/// <summary>
    /// Specifies the system power status.
	/// </summary>
	public enum PowerLineStatus : byte
	{
		/// <summary>
        /// The system is offline.
		/// </summary>
		Offline = 0x00,

		/// <summary>
        /// The system is online.
		/// </summary>
		Online = 0x01,

		/// <summary>
		/// The system is on backup power.
		/// </summary>
		BackupPower	= 0x02,

		/// <summary>
        /// The power status of the system is unknown.
		/// </summary>
		Unknown	= 0xFF,
	}
}
