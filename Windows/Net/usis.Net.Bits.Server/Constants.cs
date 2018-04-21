//
//  @(#) Constants.cs
//
//  Project:    usis.Net.Bits.Server
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;

namespace usis.Net.Bits
{
    #region Constants class

    //  ---------------
    //  Constants class
    //  ---------------

    internal static class Constants
    {
        public const string BitsPostMethodName = "BITS_POST";
        public const string RangeUnit = "bytes";
        public const string EncodingIdentity = "identity";
    }

    #endregion Constants class

    #region UploadPacketType class

    //  ----------------------
    //  UploadPacketType class
    //  ----------------------

    internal static class UploadPacketType
    {
        public const string Ping = "Ping";
        public const string CreateSession = "Create-Session";
        public const string Fragment = "Fragment";
        public const string CloseSession = "Close-Session";
        public const string CancelSession = "Cancel-Session";
        public const string Acknowledge = "Ack";
    }

    #endregion UploadPacketType class

    #region UploadHeaderNames class

    //  -----------------------
    //  UploadHeaderNames class
    //  -----------------------

    internal static class UploadHeaderNames
    {
        public const string SupportedProtocols = "BITS-Supported-Protocols";
        public const string PacketType = "BITS-Packet-Type";
        public const string Protocol = "BITS-Protocol";
        public const string SessionId = "BITS-Session-Id";
        public const string ReceivedContentRange = "BITS-Received-Content-Range";
        public const string ReplyUrl = "BITS-Reply-URL";
    }

    #endregion UploadHeaderNames class

    #region UploadProtocol class

    //  --------------------
    //  UploadProtocol class
    //  --------------------

    internal static class UploadProtocol
    {
        public static Guid Bits_1_5 = new Guid("7df0354d-249b-430f-820d-3d2a9bef4931");
    }

    #endregion UploadProtocol class
}

namespace usis.Platform
{
    #region CharConstants class

    //  ------------------
    // CharConstants class
    //  ------------------

    internal static partial class CharConstants
    {
        public const char Space = ' ';
    }

    #endregion CharConstants class
}

namespace usis.Platform.Net
{
    #region HttpRequestHeaderName class

    //  ---------------------------
    //  HttpRequestHeaderName class
    //  ---------------------------

    internal static class HttpRequestHeaderName
    {
        public const string Range = "Range";
        public const string ContentRange = "Content-Range";
    }

    #endregion HttpRequestHeaderName class

    #region HttpResponseHeaderName class

    //  ----------------------------
    //  HttpResponseHeaderName class
    //  ----------------------------

    internal static class HttpResponseHeaderName
    {
        public const string AcceptEncoding = "Accept-Encoding";
    }

    #endregion HttpResponseHeaderName class
}

// eof "Constants.cs"
