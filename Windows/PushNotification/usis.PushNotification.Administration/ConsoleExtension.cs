//
//  @(#) ConsoleExtension.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System.Globalization;
using System.Windows.Forms;
using Microsoft.ManagementConsole;
using Microsoft.ManagementConsole.Advanced;
using usis.Framework;

namespace usis.Server.PushNotification.Administration
{
    //  ----------------------
    //  ConsoleExtension class
    //  ----------------------

    internal static class ConsoleExtension
    {
        //  --------------------------
        //  ShowExceptionDialog method
        //  --------------------------

        public static DialogResult ShowExceptionDialog(
            this Console console,
            System.Exception exception)
        {
            return console.ShowDialog(new MessageBoxParameters()
            {
                Caption = Strings.Exception,
                Icon = MessageBoxIcon.Error,
                Text = exception.Message,
                Buttons = MessageBoxButtons.OK
            });
        }
    }

    //  ----------------------------
    //  PropertySheetExtension class
    //  ----------------------------

    internal static class PropertySheetExtension
    {
        //  --------------------------
        //  ShowExceptionDialog method
        //  --------------------------

        public static DialogResult ShowExceptionDialog(
            this PropertySheet propertySheet,
            System.Exception exception)
        {
            return propertySheet.ShowDialog(new MessageBoxParameters()
            {
                Caption = Strings.Exception,
                Icon = MessageBoxIcon.Error,
                Text = exception.Message,
                Buttons = MessageBoxButtons.OK
            });
        }
    }

    //  ------------------------------
    //  OperationResultExtension class
    //  ------------------------------

    internal static class OperationResultExtension
    {
        //  -----------------------------
        //  ToMessageBoxParameters method
        //  -----------------------------

        public static MessageBoxParameters ToMessageBoxParameters(this OperationResult operationResult)
        {
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

// eof "ConsoleExtension.cs"
