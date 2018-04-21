//
//  @(#) NamespaceSnapInBaseExtensions.cs
//
//  Project:    usis.ManagementConsole
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using Microsoft.ManagementConsole;
using Microsoft.ManagementConsole.Advanced;

namespace usis.ManagementConsole
{
    //  -----------------------------------
    //  NamespaceSnapInBaseExtensions class
    //  -----------------------------------

    /// <summary>
    /// Provides extension methods for the <see cref="NamespaceSnapInBase"/> class.
    /// </summary>

    public static class NamespaceSnapInBaseExtensions
    {
        #region ShowDialog method

        //  -----------------
        //  ShowDialog method
        //  -----------------

        /// <summary>
        /// Shows a message box modal to the MMC console window using the parameters specified.
        /// </summary>
        /// <param name="snapIn">The snap-in.</param>
        /// <param name="parameters">The parameters for the message box.</param>
        /// <returns>
        /// <see cref="DialogResult.OK"/> if the user clicks OK in the dialog box; otherwise, <see cref="DialogResult.Cancel"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="snapIn"/> is a null reference.
        /// </exception>

        [Obsolete("Use the methods of the Console object instead.")]
        public static DialogResult ShowDialog(this NamespaceSnapInBase snapIn, MessageBoxParameters parameters)
        {
            if (snapIn == null) throw new ArgumentNullException(nameof(snapIn));
            return snapIn.Console.ShowDialog(parameters);
        }

        /// <summary>
        /// Shows a message box modal to the MMC console window with informations about the specified exception.
        /// </summary>
        /// <param name="snapIn">The snap in.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>
        /// This method always returns <see cref="DialogResult.OK"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="snapIn"/> is a null reference.
        /// </exception>

        [Obsolete("Use the methods of the Console object instead.")]
        public static DialogResult ShowDialog(this NamespaceSnapInBase snapIn, Exception exception)
        {
            if (snapIn == null) throw new ArgumentNullException(nameof(snapIn));
            return snapIn.Console.ShowDialog(exception);
        }

        #endregion ShowDialog method

        #region Invoke method

        //  -------------
        //  Invoke method
        //  -------------

        /// <summary>
        /// Invokes the specified action
        /// and shows a dialog when an exception occurs.
        /// </summary>
        /// <param name="snapIn">The snap-in.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="ArgumentNullException">
        /// <c>snapIn</c> is a null reference.
        /// </exception>

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [Obsolete("Use the console's Invoke method instead.")]
        public static void Perform(this NamespaceSnapInBase snapIn, System.Action action)
        {
            if (snapIn == null) throw new ArgumentNullException(nameof(snapIn));
            if (action == null) throw new ArgumentNullException(nameof(action));

            try { action.Invoke(); }
            catch (Exception exception) { snapIn.Console.ShowDialog(exception); }
        }

        /// <summary>
        /// Invokes the specified status.
        /// and shows a dialog and set the specified <seealso cref="AsyncStatus"/>
        /// when an exception occurs.
        /// </summary>
        /// <param name="snapIn">The snap-in.</param>
        /// <param name="status">The status information for asynchronous operations.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="ArgumentNullException">
        /// <c>snapIn</c> is a null reference.
        /// </exception>

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [Obsolete("Use the console's Invoke method instead.")]
        public static void Perform(this NamespaceSnapInBase snapIn, AsyncStatus status, Action<AsyncStatus> action)
        {
            if (snapIn == null) throw new ArgumentNullException(nameof(snapIn));
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (status == null) throw new ArgumentNullException(nameof(status));

            try { action.Invoke(status); }
            catch (Exception exception)
            {
                status.Complete(string.Empty, false);
                snapIn.Console.ShowDialog(exception);
            }
        }

        #endregion Invoke method
    }
}

// eof "NamespaceSnapInBaseExtensions.cs"
