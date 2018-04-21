//
//  @(#) ApnsDeviceListView.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System.Windows.Forms;
using Microsoft.ManagementConsole;
using usis.ManagementConsole;

namespace usis.PushNotification.Administration
{
    //  ------------------------
    //  ApnsDeviceListView class
    //  ------------------------

    internal sealed class ApnsDeviceListView : ListView<SnapIn, ApnsDeviceInfoResultNode, ApnsChannelNode>
    {
        #region properties

        //  --------------------
        //  Description property
        //  --------------------

        internal static MmcListViewDescription Description => new MmcListViewDescription()
        {
            DisplayName = Strings.Devices,
            LanguageIndependentName = "apns-devices",
            Options = MmcListViewOptions.ExcludeScopeNodes | MmcListViewOptions.SingleSelect,
            ViewType = typeof(ApnsDeviceListView)
        };

        #endregion properties

        #region overrides

        //  -------------------
        //  OnInitialize method
        //  -------------------

        protected override void OnInitialize(AsyncStatus status)
        {
            base.OnInitialize(status);

            DescriptionBarText = Strings.Devices;

            Columns[0].SetWidth(150);
            Columns.Add(Strings.ColumnDeviceToken, 150);
            Columns.Add(Strings.ColumnFirstRegistration, 120);
            Columns.Add(Strings.ColumnLastRegistration, 120);
            Columns.Add(Strings.ColumnAccount, 150);
            Columns.Add(Strings.ColumnGroups, 150);
            Columns.Add(Strings.ColumnInfo, 150);
            Columns.Add(Strings.ColumnExpired, 120);

            Refresh(null);

            ScopeNode.Refreshed += ScopeNode_Refreshed;
        }

        //  -----------------
        //  OnShutdown method
        //  -----------------

        protected override void OnShutdown(SyncStatus status)
        {
            ScopeNode.Refreshed -= ScopeNode_Refreshed;

            base.OnShutdown(status);
        }

        //  -------------------------
        //  OnSelectionChanged method
        //  -------------------------

        protected override void OnSelectionChanged(SyncStatus status)
        {
            if (SelectedNodes.Count == 0)
            {
                SelectionData.Clear();
                SelectionData.ActionsPaneItems.Clear();
            }
            else
            {
                SelectionData.Update(SelectedNodes[0], false, null, null);
                var sendAction = SelectionData.ActionsPaneItems.AddAction(
                    Strings.SendNotification, Strings.SendNotificationDescription, (e) =>
                {
                    SendNotification(FirstSelectedNode.Item);
                });
                sendAction.ImageIndex = 5;
            }
        }

        #endregion overrides

        #region private methods

        //  -----------------------
        //  SendNotification method
        //  -----------------------

        private void SendNotification(ApnsReceiverInfo device)
        {
            using (var dialog = new ApnsSendNotificationDialog())
            {
                if (SnapIn.Console.ShowDialog(dialog) == DialogResult.OK)
                {
                    SnapIn.Console.Invoke(() =>
                    {
                        using (var client = SnapIn.Router.CreateApnsClient())
                        {
                            client.Service.SendNotification(device.Base64DeviceToken,
                                new ApnsNotification()
                                {
                                    Alert = dialog.Alert,
                                    Badge = dialog.Badge,
                                    Sound = dialog.Sound

                                }.ToString());
                        }
                    });
                }
            }
        }

        //  --------------------------
        //  ScopeNode_Refreshed method
        //  --------------------------

        private void ScopeNode_Refreshed(object sender, AsyncStatusEventArgs e)
        {
            Refresh(FirstSelectedNode);
        }

        //  --------------
        //  Refresh method
        //  --------------

        private void Refresh(ApnsDeviceInfoResultNode selectedNode)
        {
            SnapIn.Console.Invoke(() =>
            {
                ResultNodes.Clear();
                foreach (var device in SnapIn.Router.LoadDevices(ScopeNode.ChannelInfo.Key, ScopeNode.IncludeExpiredAction.Checked))
                {
                    var node = new ApnsDeviceInfoResultNode(device)
                    {
                        ImageIndex = 4
                    };
                    ResultNodes.Add(node);
                    if (node.Item.ReceiverId.Equals(selectedNode?.Item.ReceiverId))
                    {
                        node.SendSelectionRequest(true);
                    }
                }
            });
        }

        #endregion private methods
    }

    #region ApnsDeviceInfoResultNode class

    //  ------------------------------
    //  ApnsDeviceInfoResultNode class
    //  ------------------------------

    internal class ApnsDeviceInfoResultNode : ResultNode<ApnsReceiverInfo>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public ApnsDeviceInfoResultNode(ApnsReceiverInfo deviceInfo) : base(deviceInfo)
        {
            if (Item.Name != null) DisplayName = Item.Name;
            SubItemDisplayNames.AddString(Item.DeviceToken.HexString);
            SubItemDisplayNames.AddFormat("{0}", Item.FirstRegistration.ToLocalTime());
            SubItemDisplayNames.AddFormat("{0}", Item.LastRegistration.ToLocalTime());
            SubItemDisplayNames.AddString(Item.Account);
            SubItemDisplayNames.AddString(Item.Groups);
            SubItemDisplayNames.AddString(Item.Info);
            SubItemDisplayNames.AddFormat("{0}", Item.Expired?.ToLocalTime());
        }

        #endregion construction
    }

    #region ResultNode<TItem> class

    //  -----------------------
    //  ResultNode<TItem> class
    //  -----------------------

    internal abstract class ResultNode<TItem> : ResultNode
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        protected ResultNode(TItem item) { Item = item; }

        #endregion construction

        #region properties

        //  -------------
        //  Item property
        //  -------------

        public TItem Item { get; }

        #endregion properties
    }

    #endregion ResultNode<TItem> class

    #endregion ApnsDeviceInfoResultNode class
}

// eof "ApnsDeviceListView.cs"
