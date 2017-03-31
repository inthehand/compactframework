// In The Hand - .NET Components for Mobility
//
// InTheHand.WindowsMobile.Configuration.ConfigurationManager
// 
// Copyright (c) 2003-2013 In The Hand Ltd, All rights reserved.

namespace InTheHand.WindowsMobile.Configuration
{
    using System;
    using System.Runtime.InteropServices;
    using System.Xml;

    /// <summary>
	/// Provides methods for sending device configuration files to Configuration Manager for either complete processing, or just for testing.
	/// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 2003 (not Classic Edition) and later</description></item>
    /// <item><term>Windows Embedded Compact</term><description>Windows Embedded CE 6.0 and later</description></item>
    /// <item><term>Windows Embedded NavReady</term><description>Windows Embedded NavReady 2009 and later</description></item>
    /// </list>
    /// </remarks>
    public static class ConfigurationManager
    {
        private static XmlDocument ProcessConfig(XmlDocument config, NativeMethods.CFGFLAG flags)
		{
			IntPtr pOut = IntPtr.Zero;

			uint hresult = 0;

			try
			{
                hresult = NativeMethods.ProcessConfigXML(config.OuterXml, flags, ref pOut);
			}
			catch(MissingMethodException mme)
			{
				throw new PlatformNotSupportedException("ConfigurationManager requires a phone enabled Windows Mobile 2003 or later device",mme);
			}

			if(hresult != 0)
			{
				switch(hresult)
				{
					case 0x42003:
					case 0x42005://ProcessingCanceled
					case 0x42006:
					case 0x42010:
						break;
                    case 0x80042001:    //ObjectBusy = 
                    case 0x80042002: //CancelTimeout = 
                        throw new TimeoutException();
                    
					case 0x8007000e:
						throw new OutOfMemoryException();

                    case 0x80042004: //EntryNotFound = 
                    case 0x80042007: //CspException = 
                    case 0x80042008: //TransactioningFailure = 
                    case 0x80042009: //BadXml = 

					case 0x80070057:
						throw new ArgumentException("Invalid argument passed to DMProcessConfigXML");

					default:
                        Marshal.ThrowExceptionForHR(unchecked((int)hresult));
                        break;
						//throw new SystemException("Error Code #: " + hresult.ToString("X"));
				}
			}
			
			string s = Marshal.PtrToStringUni(pOut);
            NativeMethods.free(pOut);

			XmlDocument xd = new XmlDocument();
			xd.LoadXml(s);
			return xd;
		}

        /// <summary>
		/// Sends a device configuration file to Configuration Manager for processing.
		/// </summary>
		/// <param name="configDoc">The configuration document used to provision the device.</param>
		/// <param name="metadata">true to return metadata associated with the XML parm elements in the new configuration; otherwise, false.</param>
		/// <returns>If metadata is true, this method returns metadata associated with the XML parm elements in this configuration.</returns>
		public static XmlDocument ProcessConfiguration(XmlDocument configDoc, bool metadata)
		{
            return ProcessConfig(configDoc, NativeMethods.CFGFLAG.PROCESS | (metadata ? NativeMethods.CFGFLAG.METADATA : 0));
        }

        /// <summary>
		/// Sends a device configuration file to Configuration Manager for testing.
		/// </summary>
		/// <param name="configDoc">The configuration document used to provision the device.</param>
		/// <param name="metadata">true to return metadata associated with the XML parm elements in the new configuration; otherwise, false.</param>
		/// <returns>If metadata is true, this method returns metadata associated with the XML parm elements in this configuration.</returns>
		/// <remarks>Configuration Manager will not process the configuration file, but it can still return useful metadata associated with the XML parm elements.</remarks>
		public static XmlDocument TestConfiguration(XmlDocument configDoc, bool metadata)
		{
            return ProcessConfig(configDoc, 0 | (metadata ? NativeMethods.CFGFLAG.METADATA : 0));
        }

        /// <summary>
        /// Retrieves the value of the policy specified.
        /// </summary>
        /// <param name="policy">The policy identifier.</param>
        /// <returns>The value of the policy identified by policy.</returns>
        /// <remarks>If the policy setting does not exist, zero (0) is returned.
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
        /// </list>
        /// </remarks>
        public static int QueryPolicy(SecurityPolicy policy)
        {
            int policyValue;

            int hresult = NativeMethods.QueryPolicy(policy, out policyValue);

            if (hresult < 0)
            {
                Marshal.ThrowExceptionForHR(hresult);
            }

            return policyValue;
        }

        /// <summary>
        /// Retrieves the assigned trust level of a process.
        /// </summary>
        /// <remarks>
        /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
        /// <item><term>Windows Mobile</term><description>Pocket PC 2003 and later, Windows Mobile Version 5.0 and later</description></item>
        /// <item><term>Windows Embedded</term><description>Windows CE 4.1 to Windows Embedded CE 6.0</description></item>
        /// </list>
        /// </remarks>
        public static TrustLevel CurrentTrust
        {
            get
            {
                return NativeMethods.GetCurrentTrust();
            }
        }

        internal const string document = "<wap-provisioningdoc>{0}</wap-provisioningdoc>";
    }
}
