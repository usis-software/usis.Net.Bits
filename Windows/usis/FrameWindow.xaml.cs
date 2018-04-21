//
//  @(#) FrameWindow.cs
//
//  Project:    usis
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015,2016 usis GmbH. All rights reserved.

using System;
using System.Windows;

namespace usis.Windows
{
    //  -----------------
    //  FrameWindow class
    //  -----------------

    public partial class FrameWindow : Window
    {
        //  ------------
        //  construction
        //  ------------

        public FrameWindow()
        {
            InitializeComponent();
            this.RestoreWindowState();
        }

        //  ---------------
        //  OnClosed method
        //  ---------------

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.SaveWindowState();
        }
    }
}

// eof "FrameWindow.cs"
