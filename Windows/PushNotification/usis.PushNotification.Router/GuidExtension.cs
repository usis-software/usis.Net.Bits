//
//  @(#) GuidExtension.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015,2016 usis GmbH. All rights reserved.

using System;

namespace usis.Server.PushNotification
{
    //  -------------------
    //  GuidExtension class
    //  -------------------

    /// <summary>
    /// Provides extension methods to the <see cref="Guid"/> type.
    /// </summary>

    internal static class GuidExtension
    {
        public static bool IsEmpty(this Guid guid)
        {
            return guid == Guid.Empty;
        }

        public static Guid? NullIfEmpty(this Guid guid)
        {
            if (guid.IsEmpty()) return null;
            return guid;
        }
    }
}

// eof "GuidExtension.cs"
