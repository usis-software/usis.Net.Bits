//
//  @(#) WnsDeviceListView.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using System;
using usis.ManagementConsole;

namespace usis.PushNotification.Administration
{
    internal sealed class WnsDeviceListView : ListView<SnapIn, ResultNode, WnsReceiverInfo, WnsChannelNode>
    {
        #region properties

        //  --------------------
        //  Description property
        //  --------------------

        internal static MmcListViewDescription Description => new MmcListViewDescription()
        {
            DisplayName = Strings.Devices,
            LanguageIndependentName = "wns-devices",
            Options = MmcListViewOptions.ExcludeScopeNodes | MmcListViewOptions.SingleSelect,
            ViewType = typeof(WnsDeviceListView)
        };

        #endregion properties

        #region overrides

        //  -------------------
        //  OnInitialize method
        //  -------------------

        protected override void OnInitialize(AsyncStatus status)
        {
            DescriptionBarText = Strings.Devices;

            Columns[0].SetWidth(150);
            Columns.Add("Channel URI", 150);
            Columns.Add("Device Identifier", 150);
            Columns.Add(Strings.ColumnFirstRegistration, 120);
            Columns.Add(Strings.ColumnLastRegistration, 120);
            Columns.Add(Strings.ColumnAccount, 150);
            Columns.Add(Strings.ColumnGroups, 150);
            Columns.Add(Strings.ColumnInfo, 150);

            Reload();
            ScopeNode.Refreshed += (sender, e) => Reload();
        }

        //  -----------------------
        //  CreateResultNode method
        //  -----------------------

        protected override ResultNode CreateResultNode(WnsReceiverInfo item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            var node = new ResultNode()
            {
                DisplayName = string.IsNullOrWhiteSpace(item.Name) ? item.ChannelUri : item.Name,
                Tag = item,
                ImageIndex = 4
            };
            node.SubItemDisplayNames.Add(item.ChannelUri);
            node.SubItemDisplayNames.Add(item.DeviceIdentifier);
            node.SubItemDisplayNames.AddFormat("{0}", item.FirstRegistration.ToLocalTime());
            node.SubItemDisplayNames.AddFormat("{0}", item.LastRegistration.ToLocalTime());
            node.SubItemDisplayNames.AddString(item.Account);
            node.SubItemDisplayNames.AddString(item.Groups);
            node.SubItemDisplayNames.AddString(item.Info);
            return node;
        }

        #endregion overrides

        #region methods

        //  -------------
        //  Reload method
        //  -------------

        private void Reload()
        {
            SnapIn.Console.Invoke(() => Reload(SnapIn.Router.LoadDevices(ScopeNode.ChannelInfo.Key)));
        }

        #endregion methods
    }
}

// eof "WnsDeviceListView.cs"
