//
//  @(#) ApnsNewChannelDialog.cs
//
//  Project:    usis Push Notification Server
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System;
using System.Windows.Forms;

namespace usis.PushNotification.Administration
{
    //  --------------------------
    //  ApnsNewChannelDialog class
    //  --------------------------

    public partial class ApnsNewChannelDialog : Form
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public ApnsNewChannelDialog()
        {
            InitializeComponent();

            comboBoxEnvironment.DataSource = Enum.GetValues(typeof(Environment));
        }

        #endregion construction

        #region properties

        //  -----------------
        //  BundleId property
        //  -----------------

        internal string BundleId => textBoxBundleId.Text;

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
            buttonOK.Enabled = !string.IsNullOrWhiteSpace(textBoxBundleId.Text);
        }

        #endregion event handlers
    }
}

// eof "ApnsNewChannelDialog.cs"
