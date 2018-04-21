//
//  @(#) Window.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Drawing;
using System.Windows.Forms;
using usis.Platform;

namespace usis.Windows.Forms
{
    #region IWindow interface

    //  -----------------
    //  IWindow interface
    //  -----------------

    internal interface IWindow
    {
        //  ---------------------
        //  LastLocation property
        //  ---------------------

        Point LastLocation { get; }

        //  -----------------
        //  LastSize property
        //  -----------------

        Size LastSize { get; }

        //  -------------------
        //  SaveSettings method
        //  -------------------

        void SaveSettings(IValueStorage storage);

        //  ----------------------
        //  RestoreSettings method
        //  ----------------------

        void RestoreSettings(IValueStorage storage);
    }

    #endregion IWindow interface

    #region Window class

    //  ------------
    //  Window class
    //  ------------

    internal class Window : Form, IWindow
    {
        #region IWindow implementation

        //  ---------------------
        //  LastLocation property
        //  ---------------------

        public Point LastLocation { get; private set; }

        //  -----------------
        //  LastSize property
        //  -----------------

        public Size LastSize { get; private set; }

        //  -------------------
        //  SaveSettings method
        //  -------------------

        public void SaveSettings(IValueStorage storage) { OnSaveSettings(storage); }

        //  ----------------------
        //  RestoreSettings method
        //  ----------------------

        public void RestoreSettings(IValueStorage storage) { OnRestoreSettings(storage); }

        #endregion IWindow implementation

        #region overrides

        //  ------------------------
        //  OnLocationChanged method
        //  ------------------------

        protected override void OnLocationChanged(EventArgs e)
        {
            if (WindowState == FormWindowState.Normal) LastLocation = Location;

            base.OnLocationChanged(e);

        }

        //  --------------------
        //  OnSizeChanged method
        //  --------------------

        protected override void OnSizeChanged(EventArgs e)
        {
            if (WindowState == FormWindowState.Normal) LastSize = Size;

            base.OnSizeChanged(e);
        }

        #endregion overrides

        #region virtual methods

        //  ---------------------
        //  OnSaveSettings method
        //  ---------------------

        protected virtual void OnSaveSettings(IValueStorage storage) { }

        //  ------------------------
        //  OnRestoreSettings method
        //  ------------------------

        protected virtual void OnRestoreSettings(IValueStorage storage) { }

        #endregion virtual methods
    }

    #endregion Window class
}

// eof "Window.cs"
