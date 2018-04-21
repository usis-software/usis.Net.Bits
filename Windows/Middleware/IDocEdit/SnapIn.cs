//
//  @(#) SnapIn.cs
//
//  Project:    IDoc Editor
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System.ComponentModel;
using usis.Platform.Windows;
using usis.Windows.Forms;

namespace usis.Middleware.SAP.IDocEditor
{
    //  ------------
    //  SnapIn class
    //  ------------

    internal class SnapIn : Framework.Windows.Forms.SnapIn
    {
        #region properties

        //  -----------------
        //  Settings property
        //  -----------------

        private RegistryValueStorage Settings { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        public SnapIn() { Settings = RegistryValueStorage.OpenCurrentUser(@"SOFTWARE\usis\IDoc Editor", true); }

        #endregion construction

        #region overrides

        //  -------------------
        //  OnConnecting method
        //  -------------------

        protected override void OnConnecting(CancelEventArgs e)
        {
            MainForm = new Window();
            MainForm.RestoreFormState(Settings);
            MainForm.FormClosed += (sender, ea) => { MainForm.SaveFormState(Settings); };

            base.OnConnecting(e);
        }

        #endregion overrides
    }
}

// eof "SnapIn.cs"
