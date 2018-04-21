//
//  @(#) OperationResultExtensions.cs
//
//  Project:    usis.ManagementConsole
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole.Advanced;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace usis.Framework.ManagementConsole
{
    //  -------------------------------
    //  OperationResultExtensions class
    //  -------------------------------

    /// <summary>
    /// Provides extension methods for the <see cref="OperationResult"/> class.
    /// </summary>

    public static class OperationResultExtensions
    {
        //  -----------------------------
        //  ToMessageBoxParameters method
        //  -----------------------------

        /// <summary>
        /// Creates message box parameters from the result of an operation.
        /// </summary>
        /// <param name="operationResult">The operation result.</param>
        /// <returns>A newly created <see cref="MessageBoxParameters"/> object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="operationResult" /> is a null reference.</exception>

        public static MessageBoxParameters ToMessageBoxParameters(this OperationResult operationResult)
        {
            if (operationResult == null) throw new ArgumentNullException(nameof(operationResult));

            string message = operationResult.ReturnCode < 0 ? Strings.ErrorPerformingAction : Strings.ActionPerformedSuccessfully;
            return new MessageBoxParameters()
            {
                Caption = Strings.OperationResultCaption,
                Icon = operationResult.ReturnCode < 0 ? MessageBoxIcon.Error : MessageBoxIcon.Information,
                Text = string.Format(CultureInfo.CurrentCulture, Strings.OperationResultText, message, operationResult.ToString()),
                Buttons = MessageBoxButtons.OK
            };
        }
    }
}

// eof "OperationResultExtensions.cs"
