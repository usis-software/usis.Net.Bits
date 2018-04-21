//
//  @(#) FrameWindow.cs
//
//  Project:    usis
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

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

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameWindow"/> class.
        /// </summary>

        public FrameWindow()
        {
            InitializeComponent();
            this.RestoreWindowState();
        }

        //  ---------------
        //  OnClosed method
        //  ---------------

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Window.Closed" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.SaveWindowState();
        }
    }
}

// eof "FrameWindow.cs"
