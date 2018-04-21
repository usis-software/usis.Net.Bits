//
//  @(#) APNsDeviceToken.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System;

namespace usis.Server.PushNotification
{
    //  -----------------------
    //  APNsDeviceToken methods
    //  -----------------------

    internal sealed class APNsDeviceToken
    {
        #region fields

        private byte[] data;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        private APNsDeviceToken() { }

        #endregion construction

        #region methods

        //  -----------------
        //  FromBase64 method
        //  -----------------

        public static APNsDeviceToken FromBase64(string base64)
        {
            return new APNsDeviceToken()
            {
                data = Convert.FromBase64String(base64)
            };
        }

        //  --------------------
        //  FromHexString method
        //  --------------------

        public static APNsDeviceToken FromHexString(string hex)
        {
            return new APNsDeviceToken()
            {
                data = Platform.Portable.HexString.ToBytes(hex)
            };
        }

        #endregion methods

        #region properties

        //  ------------------
        //  HexString property
        //  ------------------

        public string HexString
        {
            get
            {
                return Platform.Portable.HexString.FromBytes(data);
            }
        }

        //  ---------------
        //  Base64 property
        //  ---------------

        public string Base64
        {
            get
            {
                return Convert.ToBase64String(data);
            }
        }

        #endregion properties

        #region overrides

        //  ---------------
        //  ToString method
        //  ---------------

        public override string ToString()
        {
            return HexString;
        }

        #endregion overrides
    }
}

// eof "APNsDeviceToken.cs"
