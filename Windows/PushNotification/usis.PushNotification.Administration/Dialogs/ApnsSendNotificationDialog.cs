//
//  @(#) ApnsSendNotificationDialog.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using usis.Windows.Forms;

namespace usis.PushNotification.Administration
{
    //  --------------------------------
    //  ApnsSendNotificationDialog class
    //  --------------------------------

    internal partial class ApnsSendNotificationDialog : ModalDialog
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public ApnsSendNotificationDialog()
        {
            InitializeComponent();

            numericUpDownBadge.Maximum = decimal.MaxValue;
            comboBoxSound.Items.Add(ApnsNotification.DefaultSound);
        }

        #endregion construction

        #region properties

        //  --------------
        //  Alert property
        //  --------------

        internal string Alert => textBoxAlert.Text;

        //  --------------
        //  Badge property
        //  --------------

        internal int Badge => Convert.ToInt32(numericUpDownBadge.Value);

        //  --------------
        //  Sound property
        //  --------------

        internal string Sound => comboBoxSound.Text;

        #endregion properties
    }
}

// eof "ApnsSendNotificationDialog.cs"
