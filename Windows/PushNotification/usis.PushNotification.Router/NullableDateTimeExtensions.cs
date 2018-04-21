//
//  @(#) NullableDateTimeExtensions.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015,2016 usis GmbH. All rights reserved.

using System;

namespace usis.PushNotification
{
    //  --------------------------------
    //  NullableDateTimeExtensions class
    //  --------------------------------

    /// <summary>
    /// Provides extension methods to timestamp objects.
    /// </summary>

    public static class NullableDateTimeExtensions
    {
        //  ------------
        //  Later method
        //  ------------

        /// <summary>
        /// Return the timestamp which is later in time.
        /// </summary>
        /// <param name="first">The first timestamp.</param>
        /// <param name="second">The second timestamp.</param>
        /// <returns>
        /// The timestamp which is later in time.
        /// </returns>

        public static DateTime? Later(this DateTime? first, DateTime? second)
        {
            if (!first.HasValue) return second;
            if (!second.HasValue) return first;
            return first.Value > second.Value ? first : second;
        }
    }
}

// eof "NullableDateTimeExtensions.cs"
