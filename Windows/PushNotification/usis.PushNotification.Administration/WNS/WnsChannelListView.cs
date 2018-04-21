//
//  @(#) WnsChannelListView.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using Microsoft.ManagementConsole.Advanced;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using usis.Framework.ManagementConsole;
using usis.ManagementConsole;
using usis.Platform;

namespace usis.PushNotification.Administration
{
    //  ------------------------
    //  WnsChannelListView class
    //  ------------------------

    internal sealed class WnsChannelListView : ListView<SnapIn, ResultNode, WnsChannelInfo, WnsChannelListNode>
    {
        #region properties

        //  --------------------
        //  Description property
        //  --------------------

        internal static MmcListViewDescription Description => new MmcListViewDescription()
        {
            DisplayName = Strings.Channels,
            LanguageIndependentName = "wns-channels",
            Options = MmcListViewOptions.ExcludeScopeNodes | MmcListViewOptions.SingleSelect,
            ViewType = typeof(WnsChannelListView)
        };

        //  ---------------
        //  Router property
        //  ---------------

        private Router Router => SnapIn?.Router;

        //  ------------------------
        //  SelectedChannel property
        //  ------------------------

        private WnsChannelInfo SelectedChannel
        {
            get
            {
                ResultNode node = SelectionData.SelectionObject as ResultNode;
                return node?.Tag as WnsChannelInfo;
            }
        }

        #endregion properties

        #region overrides

        //  -------------------
        //  OnInitialize method
        //  -------------------

        protected override void OnInitialize(AsyncStatus status)
        {
            base.OnInitialize(status);

            DescriptionBarText = Strings.Channels;

            // define content

            Columns[0].Title = Strings.ColumnPackageName;
            Columns[0].SetWidth(150);

            Columns.Add(Strings.ColumnPackageSid, 200);
            Columns.Add(Strings.ColumnDescription, 300);

            //  new channel action

            ActionsPaneItems.AddAction(Strings.NewChannel, Strings.NewChannelDescription, 7, (e) =>
            {
                using (var dialog = new WnsNewChannelDialog())
                {
                    if (SnapIn.Console.ShowDialog(dialog) == DialogResult.OK)
                    {
                        var channelKey = new WnsChannelKey(dialog.PackageSid);
                        var result = SnapIn.Router.CreateChannel(channelKey);
                        if (result.Succeeded)
                        {
                            SnapIn.Router.WnsChannels.Reload();
                            SelectItem(new WnsChannelInfo() { Key = channelKey });
                        }
                        else SnapIn.Console.ShowDialog(result.ToMessageBoxParameters());
                    }
                }
            });

            // (re)load content

            Router.WnsChannels.Reloaded += (sender, e) => { ReloadChannels(); };
            Router.WnsChannels.ReloadFailed += (sender, e) => { ReloadChannels(); };
            Router.WnsChannels.Reload(false);
        }

        //  -------------------------
        //  OnSelectionChanged method
        //  -------------------------

        protected override void OnSelectionChanged(SyncStatus status)
        {
            if (SelectedNodes.Count == 0) SelectionData.Clear();
            else
            {
                Debug.Assert(SelectedNodes.Count == 1);
                SelectionData.Update(SelectedNodes[0], false, null, null);
                SelectionData.EnabledStandardVerbs = StandardVerbs.Delete | StandardVerbs.Properties;
            }
        }

        //  -------------------------
        //  OnAddPropertyPages method
        //  -------------------------

        protected override void OnAddPropertyPages(PropertyPageCollection propertyPageCollection)
        {
            if (propertyPageCollection == null) throw new ArgumentNullException(nameof(propertyPageCollection));
            propertyPageCollection.Add(new ChannelPropertyPage<WNsChannelPropertiesControl, WnsChannelInfo>());
        }

        //  ---------------
        //  OnDelete method
        //  ---------------

        protected override void OnDelete(SyncStatus status)
        {
            if (SnapIn.Console.ShowDialog(new MessageBoxParameters()
            {
                Caption = Strings.DeleteChannel,
                Icon = MessageBoxIcon.Exclamation,
                Text = Strings.DeleteChannelQuestion,
                Buttons = MessageBoxButtons.YesNo

            }) == DialogResult.Yes)
            {
                var result = SnapIn.Router.DeleteChannel(SelectedChannel.Key);
                if (result.Succeeded) Router.WnsChannels.Reload();
                else SnapIn.Console.ShowDialog(result.ToMessageBoxParameters());
            }
        }

        #endregion overrides

        #region methods

        //  ---------------------
        //  ReloadChannels method
        //  ---------------------

        private void ReloadChannels()
        {
            ResultNodes.Clear();
            foreach (var channel in Router.WnsChannels)
            {
                var resultNode = new ResultNode()
                {
                    DisplayName = channel.PackageName ?? string.Empty,
                    Tag = channel,
                    ImageIndex = 6
                };
                resultNode.SubItemDisplayNames.AddString(channel.Key.PackageSid);
                resultNode.SubItemDisplayNames.AddString(channel.Description);
                ResultNodes.Add(resultNode);
                if (channel.Key.Equals(SelectedChannel?.Key)) resultNode.SendSelectionRequest(true);
            }
        }

        #endregion methods
    }
}

// eof "WnsChannelListView.cs"
