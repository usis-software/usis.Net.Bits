//
//  @(#) HttpException.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

#pragma warning disable 1591

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security;
using usis.Cocoa;
using usis.Platform;

namespace usis.Framework.Portable.Net
{
    [Serializable]
    public class HttpException : Exception
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public HttpException() { }

        public HttpException(string message) : base(message) { }

        public HttpException(string message, Exception innerException) : base(message, innerException) { }

        public HttpException(int statusCode, string statusText) :
            base(string.Format(CultureInfo.CurrentCulture, Strings.HttpError, statusCode, statusText))
        {
            StatusCode = statusCode;
        }

        protected HttpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            info.AddValue(nameof(StatusCode), StatusCode);
        }

        #endregion construction

        #region properties

        //  -------------------
        //  StatusCode property
        //  -------------------

        public int StatusCode { get; private set; }

        #endregion properties

        #region overrides

        //  --------------------
        //  GetObjectData method
        //  --------------------

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));

            StatusCode = info.GetValue<int>(nameof(StatusCode));

            base.GetObjectData(info, context);
        }

        #endregion overrides
    }
}

// eof "HttpException.cs"
