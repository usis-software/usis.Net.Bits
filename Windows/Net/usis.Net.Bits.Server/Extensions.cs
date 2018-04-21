//
//  @(#) Extensions.cs
//
//  Project:    usis.Net.Bits.Server
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System.Net;

namespace usis.Net.Bits
{
    //  ----------------
    //  Extensions class
    //  ----------------

    internal static class Extensions
    {
        //  ----------------
        //  SetStatus method
        //  ----------------

        public static void SetStatus(this HttpListenerResponse response, HttpStatusCode status)
        {
            response.StatusCode = (int)status;
        }
    }
}

// eof "Extensions.cs"
