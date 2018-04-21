//
//  @(#) EnterPasswordDialog.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.Windows.Forms;

namespace usis.PushNotification.Administration
{
    //  -------------------------
    //  EnterPasswordDialog class
    //  -------------------------

    internal partial class EnterPasswordDialog : Form
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public EnterPasswordDialog()
        {
            InitializeComponent();
        }

        #endregion construction

        #region properties

        //  ------------------
        //  Password  property
        //  ------------------

        public string Password => textBoxPassword.Text;

        #endregion properties

        #region methods

        //  ----------------------
        //  PasswordChanged method
        //  ----------------------

        private void PasswordChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = textBoxPassword.Text.Length > 0;
        }

        #endregion methods
    }
}

// eof "EnterPasswordDialog.cs"
