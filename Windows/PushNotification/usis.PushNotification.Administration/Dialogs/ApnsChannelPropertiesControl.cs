//
//  @(#) ApnsChannelPropertiesControl.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace usis.PushNotification.Administration
{
    //  ----------------------------------
    //  ApnsChannelPropertiesControl class
    //  ----------------------------------

    internal partial class ApnsChannelPropertiesControl : UserControl, IChannelControl<ApnsChannelInfo>
    {
        #region construction

        //  -----------
        //  constructor
        //  -----------

        public ApnsChannelPropertiesControl() { InitializeComponent(); }

        #endregion construction

        #region IChannelControl implementation

        //  ---------------------
        //  PropertyPage property
        //  ---------------------

        public PropertyPage PropertyPage { get; set; }

        //  ------------------
        //  RefreshData method
        //  ------------------

        void IChannelControl<ApnsChannelInfo>.RefreshData(ApnsChannelInfo channel)
        {
            if (channel == null) return;

            textBoxBundleId.Text = channel.Key.BundleId;
            textBoxEnvironment.Text = channel.Key.Environment.ToString();
            textBoxDescription.Text = channel.Description;
            textBoxCertificate.Text = channel.CertificateStateText();
            textBoxThumbprint.Text = channel.CertificateThumbprint;
            textBoxCreated.Text = channel.Created.ToLocalTime().ToString("F", CultureInfo.CurrentCulture);
            textBoxChanged.Text = string.Format(CultureInfo.CurrentCulture, "{0:F}", channel.Changed?.ToLocalTime());
        }

        //  --------------------
        //  UpdateChannel method
        //  --------------------

        void IChannelControl<ApnsChannelInfo>.UpdateChannel(ApnsChannelInfo channel)
        {
            if (channel == null) throw new ArgumentNullException(nameof(channel));
            channel.Description = textBoxDescription.Text;
        }

        #endregion IChannelControl implementation

        #region event handler

        //  -------------------------
        //  DescriptionChanged method
        //  -------------------------

        private void DescriptionChanged(object sender, EventArgs e)
        {
            PropertyPage.Dirty = true;
        }

        #endregion event handler
    }
}

// eof "ApnsChannelPropertiesControl.cs"
