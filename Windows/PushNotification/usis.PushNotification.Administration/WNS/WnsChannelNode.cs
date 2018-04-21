//
//  @(#) WnsChannelNode.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using usis.ManagementConsole;

namespace usis.PushNotification.Administration
{
    //  --------------------
    //  WnsChannelNode class
    //  --------------------

    internal class WnsChannelNode : ScopeNode<SnapIn>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public WnsChannelNode(WnsChannelInfo channelInfo) : base(true)
        {
            DisplayName = channelInfo.PackageName ?? channelInfo.Key.PackageSid;
            Tag = channelInfo;
            ImageIndex = SelectedImageIndex = 6;

            ViewDescriptions.Add(WnsDeviceListView.Description);

            EnabledStandardVerbs = StandardVerbs.Refresh;

            //IncludeExpiredAction = ActionsPaneItems.AddAction(Strings.ShowExpiredDevices, (e) =>
            //{
            //    IncludeExpiredAction.Checked = !IncludeExpiredAction.Checked;
            //    Refresh();
            //});
        }

        #endregion construction

        #region properties

        //  --------------------
        //  ChannelInfo property
        //  --------------------

        public WnsChannelInfo ChannelInfo => Tag as WnsChannelInfo;

        //  -----------------------------
        //  IncludeExpiredAction property
        //  -----------------------------

        //public Action IncludeExpiredAction { get; private set; }

        #endregion properties
    }
}

// eof "WnsChannelNode.cs"
