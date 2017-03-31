// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INTERNET_STATUS.cs" company="In The Hand Ltd">
// Copyright (c) 2008-14 In The Hand Ltd. All Rights Reserved.
// </copyright>
// <author>Peter Foot</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Net;
using System.Text;
using System.Runtime.InteropServices;

namespace InTheHand.Net
{
    internal enum INTERNET_STATUS
    {
        RESOLVING_NAME          =10,
        NAME_RESOLVED           =11,
        CONNECTING_TO_SERVER    =20,
        CONNECTED_TO_SERVER     =21,
        SENDING_REQUEST         =30,
        REQUEST_SENT            =31,
        RECEIVING_RESPONSE      =40,
        RESPONSE_RECEIVED       =41,
        CTL_RESPONSE_RECEIVED   =42,
        PREFETCH                =43,
        CLOSING_CONNECTION      =50,
        CONNECTION_CLOSED       =51,
        HANDLE_CREATED          =60,
        HANDLE_CLOSING          =70,
        DETECTING_PROXY         =80,
        REQUEST_COMPLETE        =100,
        REDIRECT                =110,
        INTERMEDIATE_RESPONSE   =120,
        USER_INPUT_REQUIRED     =140,
        STATE_CHANGE            =200,
        COOKIE_SENT             =320,
        COOKIE_RECEIVED         =321,
        COOKIE_STATE            =322,
        COOKIE_SUPPRESSED       =323,
        PRIVACY_IMPACTED        =324,
        P3P_HEADER              =325,
        P3P_POLICYREF           =326,
        COOKIE_HISTORY          =327,
    }
}