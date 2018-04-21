//
//  @(#) EventLogExtension.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System;
using System.Diagnostics;
using System.Globalization;

namespace usis.Platform.Windows
{
    //  -----------------------
    //  EventLogExtension class
    //  -----------------------

    /// <summary>
    /// Provides extensions to the <see cref="EventLog"/> class.
    /// </summary>

    public static class EventLogExtension
    {
        //  ---------------------
        //  WriteException method
        //  ---------------------

        /// <summary>
        /// Writes an error with information about the specified exception to the event log.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        /// <param name="exception">The exception to write to the evemt log.</param>
        /// <param name="provider">An <see cref="IFormatProvider" />provider used to format the message that is written to the event log.</param>
        /// <param name="format">The format string used to format the message that is written to the event log..</param>
        /// <exception cref="ArgumentNullException"><i>eventLog</i> is a null reference (<b>Nothing</b> in Visual Basic).</exception>

        public static void WriteException(this EventLog eventLog, Exception exception, IFormatProvider provider, string format)
        {
            if (eventLog == null) throw new ArgumentNullException(nameof(eventLog));

            eventLog.WriteEntry(string.Format(provider, format, exception), EventLogEntryType.Error);
        }

        /// <summary>
        /// Writes an error with information about the specified exception to the event log.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        /// <param name="exception">The exception to write to the evemt log.</param>

        public static void WriteException(this EventLog eventLog, Exception exception)
        {
            eventLog.WriteException(exception, CultureInfo.CurrentCulture, Strings.AnExceptionOccurred);
        }
    }
}

// eof "EventLogExtension.cs"
