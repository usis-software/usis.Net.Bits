//
//  @(#) ExceptionDialogControl.xaml.cs
//
//  Project:    usis.Windows
//  System:     Microsoft Visual Studio 14
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015 usis GmbH. All rights reserved.

using System;
using System.Windows.Controls;

namespace usis.Windows.Controls
{
    //  ----------------------------
    //  ExceptionDialogControl class
    //  ----------------------------

    /// <summary>
    /// Interaction logic for ExceptionDialogControl.xaml
    /// </summary>

    public partial class ExceptionDialogControl : UserControl
    {
        #region construction

        public ExceptionDialogControl(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException("exception");
            InitializeComponent();

            textBoxDetails.Text = exception.ToString();
        }

        #endregion construction

    } // ExceptionDialogControl class

} // namespace usis.Windows.Controls

// eof "ExceptionDialogControl.xaml.cs"
