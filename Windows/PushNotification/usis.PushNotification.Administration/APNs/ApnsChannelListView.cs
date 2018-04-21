//
//  @(#) ApnsChannelListView.cs
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
using System.IO;
using System.Linq;
using System.Windows.Forms;
using usis.Framework;
using usis.Framework.ManagementConsole;
using usis.ManagementConsole;
using usis.Platform;

namespace usis.PushNotification.Administration
{
    //  -------------------------
    //  ApnsChannelListView class
    //  -------------------------

    internal sealed class ApnsChannelListView : ListView<SnapIn, ResultNode, ApnsChannelListNode>
    {
        #region properties

        //  --------------------
        //  Description property
        //  --------------------

        internal static MmcListViewDescription Description => new MmcListViewDescription()
        {
            DisplayName = Strings.Channels,
            LanguageIndependentName = "apns-channels",
            Options = MmcListViewOptions.ExcludeScopeNodes | MmcListViewOptions.SingleSelect,
            ViewType = typeof(ApnsChannelListView)
        };

        //  ------------------------
        //  SelectedChannel property
        //  ------------------------

        private ApnsChannelInfo SelectedChannel
        {
            get
            {
                ResultNode node = SelectionData.SelectionObject as ResultNode;
                return node?.Tag as ApnsChannelInfo;
            }
        }

        //  ---------------
        //  Router property
        //  ---------------

        private Router Router => SnapIn?.Router;

        #endregion properties

        #region overrides

        #region OnInitialize method

        //  -------------------
        //  OnInitialize method
        //  -------------------

        protected override void OnInitialize(AsyncStatus status)
        {
            base.OnInitialize(status);

            DescriptionBarText = Strings.Channels;

            // define content

            Columns[0].Title = Strings.ColumnBundleID;
            Columns[0].SetWidth(150);

            Columns.Add(new MmcListViewColumn(Strings.ColumnEnvironment, 100));
            Columns.Add(new MmcListViewColumn(Strings.ColumnCertificate, 75));
            Columns.Add(new MmcListViewColumn(Strings.ColumnDescription, 300));

            // new channel action

            var newChannelAction = new Microsoft.ManagementConsole.Action(
                Strings.NewChannel,
                Strings.NewChannelDescription);
            newChannelAction.Triggered += NewChannel;
            newChannelAction.ImageIndex = 7;
            ActionsPaneItems.Add(newChannelAction);

            // (re)load content

            Router.ApnsChannels.Reloaded += (sender, e) => { ReloadChannels(); };
            Router.ApnsChannels.ReloadFailed += (sender, e) => { ReloadChannels(); };
            Router.ApnsChannels.Reload(false);
        }

        #endregion OnInitialize method

        #region OnSelectionChanged method

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
                Debug.Assert(SelectedNodes.Count == 1);
                SelectionData.Update(SelectedNodes[0], false, null, null);
                SelectionData.EnabledStandardVerbs = StandardVerbs.Delete | StandardVerbs.Properties;

                // Upload Certificate

                SelectionData.ActionsPaneItems.AddAction(
                    Strings.ActionUploadCertificate,
                    string.Empty,
                    8, (e) => UploadCertificate());

                // Install Certificate

                var installCertificateAction = SelectionData.ActionsPaneItems.AddAction(
                    Strings.ActionInstallCertificate,
                    string.Empty,
                    9, (e) => InstallCertificate());
                installCertificateAction.Enabled = SelectedChannel.HasCertificate;

                // Uninstall Certificate

                var uninstallCertificateAction = SelectionData.ActionsPaneItems.AddAction(
                    Strings.ActionUninstallCertificate,
                    string.Empty,
                    10, (e) => UninstallCertificate());
                uninstallCertificateAction.Enabled = !string.IsNullOrWhiteSpace(SelectedChannel.CertificateThumbprint);

                // delete certificate

                var deleteCertificateAction = SelectionData.ActionsPaneItems.AddAction(
                    Strings.ActionDeleteCertificate,
                    string.Empty,
                    11, (e) => DeleteCertificate());
                deleteCertificateAction.Enabled = SelectedChannel.HasCertificate;

                // Go to Devices

                SelectionData.ActionsPaneItems.AddAction(
                    Strings.ActionGoToDevices,
                    Strings.ActionGoToDevicesDescription,
                    4, (e) =>
                {
                    foreach (var node in ScopeNode.Children.Cast<Microsoft.ManagementConsole.ScopeNode>())
                    {
                        if (Equals(node.Tag, FirstSelectedNode.Tag))
                        {
                            SelectScopeNode(node); break;
                        }
                    }
                });
            }
            SelectionData.HelpTopic = null;
        }

        #endregion OnSelectionChanged method

        #region OnDelete method

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
                SnapIn.Console.Invoke(() =>
                {
                    using (var client = Router.CreateApnsClient())
                    {
                        var result = client.Service.DeleteChannel(SelectedChannel.Key);
                        if (result.Succeeded)
                        {
                            Router.ApnsChannels.Reload();
                        }
                        else SnapIn.Console.ShowDialog(result.ToMessageBoxParameters());
                    }
                });
            }
        }

        #endregion OnDelete method

        #region OnAddPropertyPages method

        //  -------------------------
        //  OnAddPropertyPages method
        //  -------------------------

        protected override void OnAddPropertyPages(PropertyPageCollection propertyPageCollection)
        {
            if (propertyPageCollection == null) throw new ArgumentNullException(nameof(propertyPageCollection));
            propertyPageCollection.Add(new ChannelPropertyPage<ApnsChannelPropertiesControl, ApnsChannelInfo>());
        }

        #endregion OnAddPropertyPages method

        #endregion overrides

        #region event handlers

        //  -----------------
        //  NewChannel method
        //  -----------------

        private void NewChannel(object sender, ActionEventArgs e)
        {
            using (var dialog = new ApnsNewChannelDialog())
            {
                if (SnapIn.Console.ShowDialog(dialog) == DialogResult.OK)
                {
                    SnapIn.Console.Invoke(() =>
                    {
                        var channelKey = new ApnsChannelKey(dialog.BundleId, dialog.Environment);
                        using (var client = Router.CreateApnsClient())
                        {
                            var result = client.Service.CreateChannel(channelKey);
                            if (result.Succeeded)
                            {
                                Router.ApnsChannels.Reload();
                                SelectChannel(channelKey);
                            }
                            else
                            {
                                SnapIn.Console.ShowDialog(result.ToMessageBoxParameters());
                            }
                        }
                    });
                }
            }
        }

        #endregion event handlers

        #region methods

        //  ------------------------
        //  UploadCertificate method
        //  ------------------------

        private void UploadCertificate()
        {
            var channel = SelectedChannel;
            if (channel == null) return;

            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = Strings.UploadCertifiateFilter;

                if (SnapIn.Console.ShowDialog(dialog) == DialogResult.OK)
                {
                    SnapIn.Console.Invoke(() =>
                    {
                        byte[] data = File.ReadAllBytes(dialog.FileName);
                        using (var client = Router.CreateApnsClient())
                        {
                            client.Service.UploadCertificate(channel.Key, Convert.ToBase64String(data));
                        }
                        Router.ApnsChannels.Reload();
                    });
                }
            }
        }

        //  -------------------------
        //  InstallCertificate method
        //  -------------------------

        private void InstallCertificate()
        {
            using (var dialog = new EnterPasswordDialog())
            {
                if (SnapIn.Console.ShowDialog(dialog) == DialogResult.OK)
                {
                    SnapIn.Console.Invoke(() =>
                    {
                        using (var client = Router.CreateApnsClient())
                        {
                            OperationResult result = client.Service.InstallCertificate(SelectedChannel.Key, dialog.Password);
                            SnapIn.Console.ShowDialog(result.ToMessageBoxParameters());
                        }
                        Router.ApnsChannels.Reload();
                    });
                }
            }
        }

        //  ---------------------------
        //  UninstallCertificate method
        //  ---------------------------

        private void UninstallCertificate()
        {
            using (var client = Router.CreateApnsClient())
            {
                SnapIn.Console.ShowDialog(client.Service.UninstallCertificate(SelectedChannel.Key).ToMessageBoxParameters());
                Router.ApnsChannels.Reload();
            }
        }

        //  ------------------------
        //  DeleteCertificate method
        //  ------------------------

        private void DeleteCertificate()
        {
            using (var client = Router.CreateApnsClient())
            {
                SnapIn.Console.ShowDialog(client.Service.DeleteCertificate(SelectedChannel.Key).ToMessageBoxParameters());
                Router.ApnsChannels.Reload();
            }
        }

        //  ---------------------
        //  ReloadChannels method
        //  ---------------------

        private void ReloadChannels()
        {
            ResultNodes.Clear();
            if (Router == null) return;

            foreach (var channel in Router.ApnsChannels)
            {
                var resultNode = new ResultNode()
                {
                    DisplayName = channel.Key.BundleId,
                    Tag = channel,
                    ImageIndex = 6
                };
                resultNode.SubItemDisplayNames.Add(channel.Key.Environment.ToString());
                resultNode.SubItemDisplayNames.AddString(channel.CertificateStateText());
                resultNode.SubItemDisplayNames.AddString(channel.Description);
                ResultNodes.Add(resultNode);
                if (channel.Key.Equals(SelectedChannel?.Key)) resultNode.SendSelectionRequest(true);
            }
        }

        //  --------------------
        //  SelectChannel method
        //  --------------------

        private void SelectChannel(ApnsChannelKey channelKey)
        {
            foreach (var node in ResultNodes.Cast<ResultNode>())
            {
                var channel = node.Tag as ApnsChannelInfo;
                if (channelKey.Equals(channel?.Key))
                {
                    node.SendSelectionRequest(true);
                }
            }
        }

        #endregion methods
    }

    #region ApnsChannelInfoExtension class

    //  ------------------------------
    //  ApnsChannelInfoExtension class
    //  ------------------------------

    internal static class ApnsChannelInfoExtension
    {
        public static string CertificateStateText(this ApnsChannelInfo channel)
        {
            if (!string.IsNullOrWhiteSpace(channel.CertificateThumbprint))
            {
                return Strings.CertificateStateInstalled;
            }
            else if (channel.HasCertificate)
            {
                return Strings.CertificateStateUploaded;
            }
            else return string.Empty;
        }
    }

    #endregion ApnsChannelInfoExtension class
}

// eof "ApnsChannelListView.cs"
