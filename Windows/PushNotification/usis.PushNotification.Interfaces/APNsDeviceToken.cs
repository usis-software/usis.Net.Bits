//
//  @(#) APNsDeviceToken.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;

namespace usis.PushNotification
{
    //  -----------------------
    //  APNsDeviceToken methods
    //  -----------------------

    /// <summary>
    /// Represents an APNs device token.
    /// </summary>

    public sealed class APNsDeviceToken
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

        /// <summary>
        /// Creates a device token froms a base-64 encoded string representation.
        /// </summary>
        /// <param name="base64">The base-64 encoded string representation.</param>
        /// <returns>
        /// A device token.
        /// </returns>

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

        /// <summary>
        /// Creates a device token froms a hexadecimal string representation.
        /// </summary>
        /// <param name="hex">The a hexadecimal string representation.</param>
        /// <returns>
        /// A device token.
        /// </returns>

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

        /// <summary>
        /// Gets the hexadecimal string representation of the device token.
        /// </summary>
        /// <value>
        /// The hexadecimal string representation.
        /// </value>

        public string HexString
        {
            get { return Platform.Portable.HexString.FromBytes(data); }
        }

        //  ---------------
        //  Base64 property
        //  ---------------

        /// <summary>
        /// Gets the base-64 encoded string representation of the device token.
        /// </summary>
        /// <value>
        /// The base-64 encoded string representation.
        /// </value>

        public string Base64
        {
            get { return Convert.ToBase64String(data); }
        }

        #endregion properties

        #region overrides

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            return HexString;
        }

        #endregion overrides
    }
}

// eof "APNsDeviceToken.cs"
