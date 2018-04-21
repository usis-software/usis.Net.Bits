//
//  @(#) ApnsDeviceToken.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.Linq;

namespace usis.PushNotification
{
    //  -----------------------
    //  ApnsDeviceToken methods
    //  -----------------------

    /// <summary>
    /// Represents an APNs device token.
    /// </summary>

    public sealed class ApnsDeviceToken
    {
        #region fields

        private byte[] data;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        private ApnsDeviceToken() { }

        #endregion construction

        #region methods

        //  -----------------
        //  FromBase64 method
        //  -----------------

        /// <summary>
        /// Creates a device token from the specified base-64 encoded string representation.
        /// </summary>
        /// <param name="base64">The base-64 encoded string representation.</param>
        /// <returns>
        /// A newly created device token.
        /// </returns>

        public static ApnsDeviceToken FromBase64(string base64)
        {
            return new ApnsDeviceToken() { data = Convert.FromBase64String(base64) };
        }

        //  --------------------
        //  FromHexString method
        //  --------------------

        /// <summary>
        /// Creates a device token from the specified hexadecimal string representation.
        /// </summary>
        /// <param name="hex">The a hexadecimal string representation.</param>
        /// <returns>
        /// A newly created device token.
        /// </returns>

        public static ApnsDeviceToken FromHexString(string hex)
        {
            return new ApnsDeviceToken() { data = PushNotification.HexString.ToBytes(hex) };
        }

        /// <summary>
        /// Creates a device token from a specified base-64 encoded or hexadecimal string representation.
        /// </summary>
        /// <param name="deviceToken">The device token.</param>
        /// <param name="hexDeviceToken">The hexadecimal device token.</param>
        /// <returns>
        /// A newly created device token.
        /// </returns>

        public static ApnsDeviceToken FromString(string deviceToken, string hexDeviceToken)
        {
            return string.IsNullOrWhiteSpace(hexDeviceToken) ? FromBase64(deviceToken) : FromHexString(hexDeviceToken);
        }

        //  ----------------
        //  FromBytes method
        //  ----------------

        /// <summary>
        /// Creates a device token fros an array of bytes.
        /// </summary>
        /// <param name="data">An array of bytes.</param>
        /// <returns>
        /// A device token.
        /// </returns>

        public static ApnsDeviceToken FromBytes(byte[] data)
        {
            return new ApnsDeviceToken() { data = data };
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

        public string HexString => PushNotification.HexString.FromBytes(data);

        //  ---------------------
        //  Base64String property
        //  ---------------------

        /// <summary>
        /// Gets the base-64 encoded string representation of the device token.
        /// </summary>
        /// <value>
        /// The base-64 encoded string representation.
        /// </value>

        public string Base64String => Convert.ToBase64String(data);

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

        public override string ToString() { return HexString; }

        #endregion overrides
    }

    #region HexString class

    //  ---------------
    //  HexString class
    //  ---------------    

    internal static class HexString
    {
        //  ----------------
        //  FromBytes method
        //  ----------------

        public static string FromBytes(byte[] value)
        {
            return BitConverter.ToString(value).Replace("-", string.Empty);
        }

        //  --------------
        //  ToBytes method
        //  --------------

        public static byte[] ToBytes(string hex)
        {
            if (hex == null) throw new ArgumentNullException(nameof(hex));

            int length = hex.Length;
            if (length % 2 > 0) throw new FormatException();

            return Enumerable.Range(0, length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }

    #endregion HexString class
}

// eof "ApnsDeviceToken.cs"
