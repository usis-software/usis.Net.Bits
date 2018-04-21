//
//  @(#) WNsChannelPropertiesControl.cs
//
//  Project:    usis Push Notification Server
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace usis.PushNotification.Administration
{
    //  ---------------------------------
    //  WNsChannelPropertiesControl class
    //  ---------------------------------

    public partial class WNsChannelPropertiesControl : UserControl, IChannelControl<WnsChannelInfo>
    {
        #region construction

        //  -----------
        //  constructor
        //  -----------

        public WNsChannelPropertiesControl() { InitializeComponent(); }

        #endregion construction

        #region IChannelControl implementation

        //  ---------------------
        //  PropertyPage property
        //  ---------------------

        public PropertyPage PropertyPage { get;  set; }

        //  ------------------
        //  RefreshData method
        //  ------------------

        void IChannelControl<WnsChannelInfo>.RefreshData(WnsChannelInfo channel)
        {
            if (channel == null) return;

            textBoxPackageName.Text = channel.PackageName;
            textBoxPackageSid.Text = channel.Key.PackageSid;
            textBoxDescription.Text = channel.Description;
            textBoxClientSecret.Text = channel.ClientSecret;
            textBoxCreated.Text = channel.Created.ToLocalTime().ToString("F", CultureInfo.CurrentCulture);
            textBoxChanged.Text = string.Format(CultureInfo.CurrentCulture, "{0:F}", channel.Changed?.ToLocalTime());
        }

        //  --------------------
        //  UpdateChannel method
        //  --------------------

        void IChannelControl<WnsChannelInfo>.UpdateChannel(WnsChannelInfo channel)
        {
            if (channel == null) throw new ArgumentNullException(nameof(channel));

            channel.PackageName = textBoxPackageName.Text;
            channel.Description = textBoxDescription.Text;
            channel.ClientSecret = textBoxClientSecret.Text;
        }

        #endregion IChannelControl implementation

        #region event handlers

        //  --------------
        //  Changed method
        //  --------------

        private void Changed(object sender, EventArgs e)
        {
            PropertyPage.Dirty = true;
        }

        #endregion event handlers
    }
}

// eof "WNsChannelPropertiesControl.cs"
