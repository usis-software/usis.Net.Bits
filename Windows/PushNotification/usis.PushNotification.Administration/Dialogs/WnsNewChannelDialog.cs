//
//  @(#) WnsNewChannelDialog.cs
//
//  Project:    usis Push Notification Server
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System;
using System.Windows.Forms;

namespace usis.PushNotification.Administration
{
    //  -------------------------
    //  WnsNewChannelDialog class
    //  -------------------------

    public partial class WnsNewChannelDialog : Form
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public WnsNewChannelDialog()
        {
            InitializeComponent();

            comboBoxEnvironment.DataSource = Enum.GetValues(typeof(Environment));
        }

        #endregion construction

        #region properties

        //  -------------------
        //  PackageSid property
        //  -------------------

        internal string PackageSid => textBoxPackageSid.Text;

        //  --------------------
        //  Environment property
        //  --------------------

        internal Environment Environment => (Environment)comboBoxEnvironment.SelectedValue;

        #endregion properties

        #region event handlers

        //  --------------
        //  Changed method
        //  --------------

        private void Changed(object sender, EventArgs e)
        {
            buttonOK.Enabled = !string.IsNullOrWhiteSpace(textBoxPackageSid.Text);
        }

        #endregion event handlers
    }
}

// eof "WnsNewChannelDialog.cs"
