// In The Hand - .NET Components for Mobility
//
// InTheHand.Windows.Forms.PowerStatus
// 
// Copyright (c) 2002-2011 In The Hand Ltd, All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace InTheHand.Windows.Forms
{
	/// <summary>
	/// Indicates current system power status information.
	/// </summary>
    /// <remarks>The <see cref="PowerStatus"/> class represents information about the current AC line power status, battery charging status, and battery charge status.
    /// <para>This class is used by the <see cref="SystemInformationInTheHand.PowerStatus"/> property of the <see cref="SystemInformationInTheHand"/> class to indicate current system power information.</para></remarks>
	public class PowerStatus
    {
        private static bool supportsApi = true;

#pragma warning disable 0169, 0649

        private byte aCLineStatus;
		private byte batteryFlag;
		private byte batteryLifePercent;
		private byte reserved1;
		private int batteryLifeTime;
		private int batteryFullLifeTime;
		private byte reserved2;
		private byte backupBatteryFlag;
		private byte backupBatteryLifePercent;
		private byte reserved3;
		private int backupBatteryLifeTime;
		private int backupBatteryFullLifeTime;
        private int BatteryVoltage;
        private int BatteryCurrent;
        private int BatteryAverageCurrent;
        private int BatteryAverageInterval;
        private int BatterymAHourConsumed;
        private int BatteryTemperature;
        private int BackupBatteryVoltage;
        private byte BatteryChemistry;

#pragma warning restore 0169, 0649

        internal PowerStatus(){}

		/// <summary>
		/// AC power status.
		/// </summary>
		public PowerLineStatus PowerLineStatus
		{
			get
			{
				Update();
				return (PowerLineStatus)aCLineStatus;
			}
		}

		/// <summary>
		/// Gets the current battery charge status.
		/// </summary>
		public BatteryChargeStatus BatteryChargeStatus
		{
			get
			{
				Update();
				return (BatteryChargeStatus)batteryFlag;
			}
		}
		/// <summary>
		/// Gets the approximate percentage of full battery time remaining.
		/// </summary>
		/// <remarks>The approximate percentage, from 0 to 100, of full battery time remaining, or 255 if the percentage is unknown.</remarks>
		public float BatteryLifePercent
		{
			get
			{
				Update();
				return Convert.ToSingle(batteryLifePercent);
			}
		}

		/// <summary>
		/// Gets the approximate number of seconds of battery time remaining.
		/// </summary>
		/// <value>The approximate number of seconds of battery life remaining, or -1 if the approximate remaining battery life is unknown.</value>
		public int BatteryLifeRemaining
		{
			get
			{
				Update();
				return batteryLifeTime;
			}
		}

		/// <summary>
		/// Gets the reported full charge lifetime of the primary battery power source in seconds.
		/// </summary>
		/// <value>The reported number of seconds of battery life available when the battery is fullly charged, or -1 if the battery life is unknown.</value>
		public int BatteryFullLifeTime
		{
			get
			{
				Update();
				return batteryFullLifeTime;
			}
		}

        // <summary>
        // Gets the current backup battery charge status.
        // </summary>
        internal BatteryChargeStatus BackupBatteryChargeStatus
        {
            get
            {
                Update();
                return (BatteryChargeStatus)backupBatteryFlag;
            }
        }
        // <summary>
        // Gets the approximate percentage of full backup battery time remaining.
        // </summary>
        // <remarks>The approximate percentage, from 0 to 100, of full backup battery time remaining, or 255 if the percentage is unknown.</remarks>
        internal int BackupBatteryLifePercent
        {
            get
            {
                Update();
                return backupBatteryLifePercent;
            }
        }
		

		private void Update()
		{
            if (supportsApi)
            {
                try
                {
                    bool success = NativeMethods.GetSystemPowerStatusEx2(this, Marshal.SizeOf(this), true);
                }
                catch
                {
                    //on generic CE SDK there is no function so don't call again
                    supportsApi = false;
                }
            }
		}	
	}
}
