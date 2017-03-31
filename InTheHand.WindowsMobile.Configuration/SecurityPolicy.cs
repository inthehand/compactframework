// In The Hand - .NET Components for Mobility
//
// InTheHand.WindowsMobile.Configuration.SecurityPolicy
// 
// Copyright (c) 2003-2010 In The Hand Ltd, All rights reserved.

using System;
using System.Xml;
using System.Runtime.InteropServices;

namespace InTheHand.WindowsMobile.Configuration
{
    /// <summary>
    /// Contains standard security policy identifiers.
    /// </summary>
    /// <remarks>
    /// <list type="table"><listheader><term>Platforms Supported</term><description></description></listheader>
    /// <item><term>Windows Mobile</term><description>Windows Mobile Version 5.0 and later</description></item>
    /// </list>
    /// </remarks>
    public enum SecurityPolicy
    {
        /// <summary>
        /// Indicates whether applications stored on a storage card are allowed to auto-run when inserted into the device.
        /// </summary>
        AutoRun = 2,
        /// <summary>
        /// Restricts the access of remote applications that are using Remote API (RAPI) to implement ActiveSync operations on Windows Mobile powered devices.
        /// </summary>
        Rapi = 4097,
        /// <summary>
        /// Indicates whether unsigned .cab files can be installed on the device.
        /// </summary>
        UnsignedCabs = 4101,
        /// <summary>
        /// Indicates whether unsigned applications are allowed to run on Windows Mobile powered devices.
        /// </summary>
        UnsignedApplications = 4102,
        /// <summary>
        /// Indicates whether theme files can be installed on the device.
        /// </summary>
        UnsignedThemes = 4103,
        /// <summary>
        /// Indicates whether mobile operators can be assigned the Trusted Provisioning Server (TPS) role.
        /// </summary>
        TpsCarrierRole = 4104,
        /// <summary>
        /// Specifies the maximum number of times the user is allowed to try authenticating a Wireless Application Protocol (WAP) PIN-signed message.
        /// </summary>
        MaxAuthenticationRetry = 4105,
        /// <summary>
        /// Indicates whether a WAP signed message is accepted based on whether the role assigned to the message matches any of the roles specified in the policy setting.
        /// All messages are assigned role masks based on its security level and origin. The result of AND combination of the message role mask with the policy role mask determines how the message is processed. 
        /// If the result is non-zero, the message is accepted. 
        /// <para>Deprecated - Use OmaCPNetworkPINMessage and OmaCPUserPINMessage policies</para>
        /// </summary>
        WapSignedMessage = 4107,
        /// <summary>
        /// Indicates whether SL messages are accepted.
        /// An SL message downloads new services or provisioning XML to the Windows Mobile powered device.
        /// </summary>
        SLMessage = 4108,
        /// <summary>
        /// Indicates whether SI messages are accepted.
        /// An SI message is sent to Windows Mobile 6 Standard to notify users of new services, service updates, and provisioning services.
        /// </summary>
        SIMessage = 4109,
        /// <summary>
        /// Indicates whether to accept unsigned WAP messages processed by the default security provider in the Security Module (Push Router), based on their origin.
        /// </summary>
        UnauthenticatedMessages = 4110,
        /// <summary>
        /// Specifies which provisioning messages are accepted by the configuration host based on the roles assigned to the messages.
        /// </summary>
        OtaProvisioning = 4111,
        /// <summary>
        /// Indicates whether Wireless Session Protocol (WSP) notifications from the WAP stack are routed.
        /// </summary>
        WspPush = 4113,
        /// <summary>
        /// Grants the system administrative privileges held by SECROLE_MANAGER to other security roles, without modifying metabase role assignments.
        /// </summary>
        GrantManager = 4119,
        /// <summary>
        /// Grants privileges held by SECROLE_USER_AUTH to other security roles without modifying metabase role assignments.
        /// </summary>
        GrantUserAuthenticated = 4120,
        /// <summary>
        /// This setting specifies the level of permissions required to create, modify, or delete a trusted proxy.
        /// WAP proxies are configured by means of the PXLOGICAL characteristic element in a WAP provisioning XML document.
        /// A WAP proxy is trusted when the TRUST parameter is specified in the PXLOGICAL characteristic element.
        /// </summary>
        TrustedWapProxy = 4121,
        /// <summary>
        /// This setting indicates whether a user is prompted to accept or reject unsigned .cab, theme, .dll and .exe files.
        /// </summary>
        UnsignedPrompt = 4122,
        /// <summary>
        /// Specifies which security model is implemented on the device.
        /// </summary>
        PrivilegedApplications = 4123,
        /// <summary>
        /// Allows the operator to override https to use http or wsps to use wsp.
        /// </summary>
        SLSecureDownload = 4124,
        /// <summary>
        /// Determines whether software certificates can be used to sign outgoing messages.
        /// You can use this security policy with a tool that you create to allow people to import certificates.
        /// </summary>
        SoftwareCertificates = 4127,
        /// <summary>
        /// Specifies which DRM rights messages are accepted by the DRM engine based on the role assigned to the message.
        /// </summary>
        DrmWapRights = 4129,
        /// <summary>
        /// Indicates whether a password must be configured on the device.
        /// </summary>
        LassPasswordRequired = 4131,
        /// <summary>
        /// Used when the over the air (OTA) OMA Client Provisioning message is signed with only a network personal identification number (PIN).
        /// Indicates whether or not to prompt the user to accept device setting changes.
        /// </summary>
        WapNetworkPinPrompt = 4132,
        /// <summary>
        /// Specifies if the user is allowed to change mobile encryption settings for the removable storage media.
        /// </summary>
        MobileEncryptRemovable = 4134,
        /// <summary>
        /// Specifies if a Bluetooth enabled device allows other devices to perform a search on the device.
        /// </summary>
        Bluetooth = 4135,
        /// <summary>
        /// Specifies whether message transports will allow HTML messages.
        /// </summary>
        HtmlMessage = 4136,
        /// <summary>
        /// Specifies whether the Inbox application will send all messaged signed.
        /// </summary>
        SMimeSigning = 4137,
        /// <summary>
        /// Specifies whether the Inbox application will send all messages encrypted.
        /// </summary>
        SMimeEncryption = 4138,
        /// <summary>
        /// Specifies which algorithm to use to sign a message.
        /// </summary>
        SMimeSigningAlgorithm = 4139,
        /// <summary>
        /// Specifies which algorithm to use to encrypt a message.
        /// </summary>
        SMimeEncryptionAlgorithm = 4140,
        /// <summary>
        /// Determines whether the OMA network PIN signed message will be accepted.
        /// The message's role mask and the policy's role mask are combined using the AND operator.
        /// If the result is non-zero, then the message is accepted.
        /// </summary>
        OmaCPNetworkPinMessage = 4141,
        /// <summary>
        /// Determines whether the OMA user PIN or user MAC signed message will be accepted.
        /// The message's role mask and the policy's role mask are combined using the AND operator.
        /// If the result is non-zero, then the message is accepted.
        /// </summary>
        OmaCPUserPinMessage = 4142,
        /// <summary>
        /// Determines whether the OMA user network PIN signed message will be accepted.
        /// The message's role mask and the policy's role mask are combined using the AND operator.
        /// If the result is non-zero, then the message is accepted.
        /// </summary>
        OmaCPUserNetworkPinMessage = 4143,
        /// <summary>
        /// Specifies whether the Inbox application can negotiate the encryption algorithm in case a recipient's certificate does not support the specified encryption algorithm.
        /// </summary>
        SMimeEncryptionNegotiation = 4144,
        /// <summary>
        /// Enables or disables Outlook Mobile SharePoint or UNC access through ActiveSync protocol to get documents.
        /// </summary>
        SharepointUncProtocolAccess = 4145,
        /// <summary>
        /// Specifies how device authentication is handled when connecting to the desktop.
        /// </summary>
        LassDesktopQuickConnect = 4146,
    }
}
