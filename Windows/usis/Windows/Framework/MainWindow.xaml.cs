//
//  @(#) MainWindow.xaml.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2016 usis GmbH. All rights reserved.

using System;
using System.Windows;

namespace usis.Windows.Framework
{
    //  ----------------
    //  MainWindow class
    //  ----------------

    public partial class MainWindow : Window
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public MainWindow()
        {
            InitializeComponent();
            this.RestoreWindowState();
        }

        #endregion construction

        #region overrides

        //  ---------------
        //  OnClosed method
        //  ---------------

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.SaveWindowState();
        }

        #endregion overrides

    } // MainWindow class

} // namespace usis.Windows.Framework

// eof "MainWindow.xaml.cs"
