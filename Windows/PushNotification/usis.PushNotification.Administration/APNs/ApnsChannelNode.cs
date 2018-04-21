//
//  @(#) ApnsChannelNode.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using System.Globalization;
using usis.ManagementConsole;

namespace usis.PushNotification.Administration
{
    //  ---------------------
    //  ApnsChannelNode class
    //  ---------------------

    internal class ApnsChannelNode : ScopeNode<SnapIn>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public ApnsChannelNode(ApnsChannelInfo channelInfo) : base(true)
        {
            DisplayName = string.Format(
                CultureInfo.InvariantCulture, Strings.ChannelScopeNodeFormat,
                channelInfo.Key.BundleId, channelInfo.Key.Environment);
            Tag = channelInfo;
            ImageIndex = SelectedImageIndex = 6;

            ViewDescriptions.Add(ApnsDeviceListView.Description);

            EnabledStandardVerbs = StandardVerbs.Refresh;

            IncludeExpiredAction = ActionsPaneItems.AddAction(Strings.ShowExpiredDevices, (e) =>
            {
                IncludeExpiredAction.Checked = !IncludeExpiredAction.Checked;
                Refresh();
            });
        }

        #endregion construction

        #region properties

        //  --------------------
        //  ChannelInfo property
        //  --------------------

        public ApnsChannelInfo ChannelInfo => Tag as ApnsChannelInfo;

        //  -----------------------------
        //  IncludeExpiredAction property
        //  -----------------------------

        public Action IncludeExpiredAction { get; private set; }

        #endregion properties
    }
}

// eof "ApnsChannelNode.cs"
