//
//  @(#) ConsoleExtensions.cs
//
//  Project:    usis.ManagementConsole
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using Microsoft.ManagementConsole.Advanced;

namespace usis.ManagementConsole
{
    //  -----------------------
    //  ConsoleExtensions class
    //  -----------------------

    /// <summary>
    /// Provides extension methods for the <see cref="Microsoft.ManagementConsole.Advanced.Console"/> class.
    /// </summary>

    public static class ConsoleExtensions
    {
        //  -----------------
        //  ShowDialog method
        //  -----------------

        /// <summary>
        /// Shows a message box modal to the MMC console window
        /// with informations about the specified exception.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>
        /// This method always returns <see cref="DialogResult.OK"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="console"/> is a null reference.
        /// </exception>

        public static DialogResult ShowDialog(
            this Microsoft.ManagementConsole.Advanced.Console console,
            Exception exception)
        {
            if (console == null) throw new ArgumentNullException(nameof(console));
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            return console.ShowDialog(new MessageBoxParameters()
            {
                Caption = Strings.Exception,
                Icon = MessageBoxIcon.Error,
                Text = exception.Message,
                Buttons = MessageBoxButtons.OK
            });
        }

        //  -------------
        //  Invoke method
        //  -------------

        /// <summary>
        /// Invokes the specified action and shows a message box
        /// when an exception occurs.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="console"/> is a null reference.
        /// </exception>

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void Invoke(this Microsoft.ManagementConsole.Advanced.Console console, Action action)
        {
            if (console == null) throw new ArgumentNullException(nameof(console));
            if (action == null) throw new ArgumentNullException(nameof(action));

            try { action.Invoke(); }
            catch (Exception exception) { console.ShowDialog(exception); }
        }
    }
}

// eof "ConsoleExtensions.cs"
