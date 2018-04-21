
//  @(#) FocusBehavior.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 12
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015 usis GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace usis.Windows
{
    //  -------------------
    //  FocusBehavior class
    //  -------------------

    /// <summary>
    /// Base on an stackoverflow article:
    /// http://stackoverflow.com/questions/817610/wpf-and-initial-focus.
    /// </summary>

    public static class FocusBehavior
    {
        #region FocusFirstProperty

        public static readonly DependencyProperty FocusFirstProperty =
            DependencyProperty.RegisterAttached(
                "FocusFirst",
                typeof(bool),
                typeof(Control),
                new PropertyMetadata(false, OnFocusFirstPropertyChanged));

        #endregion FocusFirstProperty

        #region public methods

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static bool GetFocusFirst(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");

            return (bool)control.GetValue(FocusFirstProperty);
        }

        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void SetFocusFirst(Control control, bool value)
        {
            if (control == null) throw new ArgumentNullException("control");

            control.SetValue(FocusFirstProperty, value);
        }

        #endregion public methods

        #region event handler

        private static void OnFocusFirstPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Control control = obj as Control;
            if (control == null || !(args.NewValue is bool))
            {
                return;
            }

            if ((bool)args.NewValue)
            {
                control.Loaded += (sender, e) =>
                    control.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        #endregion event handler

    } // FocusBehavior class

} // namespace usis.Windows

// eof "FocusBehavior.cs"
