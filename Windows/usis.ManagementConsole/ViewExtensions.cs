//
//  @(#) ViewExtensions.cs
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
    //  --------------------
    //  ViewExtensions class
    //  --------------------

    public static class ViewExtension
    {
        //  -----------------
        //  ShowDialog method
        //  -----------------

        [Obsolete("Use the methods of the Console object instead.")]
        public static DialogResult ShowDialog(this Microsoft.ManagementConsole.View view, MessageBoxParameters parameters)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));

            return view.SnapIn.Console.ShowDialog(parameters);
        }

        [Obsolete("Use the methods of the Console object instead.")]
        public static DialogResult ShowDialog(this Microsoft.ManagementConsole.View view, Form form)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));

            return view.SnapIn.Console.ShowDialog(form);
        }

        [Obsolete("Use the methods of the Console object instead.")]
        public static DialogResult ShowDialog(this Microsoft.ManagementConsole.View view, Exception exception)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));

            return view.SnapIn.Console.ShowDialog(exception);
        }

        [Obsolete("Use the methods of the Console object instead.")]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void Invoke(this Microsoft.ManagementConsole.View view, Action action)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            if (action == null) throw new ArgumentNullException(nameof(action));
            try
            {
                action.Invoke();
            }
            catch (Exception exception) { view.SnapIn.Console.ShowDialog(exception); }
        }
    }
}

// eof "ViewExtensions.cs"
