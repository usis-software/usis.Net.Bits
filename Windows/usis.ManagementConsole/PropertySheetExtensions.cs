//
//  @(#) PropertySheetExtensions.cs
//
//  Project:    usis.ManagementConsole
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System;
using System.Windows.Forms;
using Microsoft.ManagementConsole;
using Microsoft.ManagementConsole.Advanced;

namespace usis.ManagementConsole
{
    //  -----------------------------
    //  PropertySheetExtensions class
    //  -----------------------------

    /// <summary>
    /// Provides extension methods for the <see cref="PropertySheet"/> class.
    /// </summary>

    public static class PropertySheetExtensions
    {
        //  -----------------
        //  ShowDialog method
        //  -----------------

        /// <summary>
        /// Shows a message box with informations about the specified exception.
        /// This message box is modal to the property sheet. 
        /// </summary>
        /// <param name="propertySheet">The property sheet.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>
        /// This method always returns <see cref="DialogResult.OK"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertySheet"/> is a null reference.
        /// </exception>

        public static DialogResult ShowDialog(this PropertySheet propertySheet, Exception exception)
        {
            if (propertySheet == null) throw new ArgumentNullException(nameof(propertySheet));
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            return propertySheet.ShowDialog(new MessageBoxParameters()
            {
                Caption = Strings.Exception,
                Icon = MessageBoxIcon.Error,
                Text = exception.Message,
                Buttons = MessageBoxButtons.OK
            });
        }
    }
}

// eof "PropertySheetExtensions.cs"
