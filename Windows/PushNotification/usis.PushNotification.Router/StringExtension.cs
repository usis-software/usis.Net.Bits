//
//  @(#) StringExtension.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System;
using usis.Platform;

namespace usis.Server.PushNotification
{
    //  ---------------------
    //  StringExtension class
    //  ---------------------

    internal static class StringExtension
    {
        //  ---------------------------------
        //  FromBase64DeviceTokenToHex method
        //  ---------------------------------

        [Obsolete("Use APNsDeviceToken class instead.")]
        public static string FromBase64DeviceTokenToHex(this string base64DeviceToken)
        {
            byte[] binaryDeviceToken = Convert.FromBase64String(base64DeviceToken);
            return HexString.FromBytes(binaryDeviceToken);
        }
    }
}

// eof "StringExtension.cs"
