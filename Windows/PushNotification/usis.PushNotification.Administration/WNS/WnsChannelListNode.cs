//
//  @(#) WnsChannelListNode.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using usis.ManagementConsole;
using usis.Platform;

namespace usis.PushNotification.Administration
{
    //  ------------------------
    //  WnsChannelListNode class
    //  ------------------------

    internal sealed class WnsChannelListNode : ScopeNode<SnapIn>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal WnsChannelListNode()
        {
            DisplayName = Strings.WNS;

            // refresh
            EnabledStandardVerbs = StandardVerbs.Refresh;

            // channel view
            ViewDescriptions.Add(WnsChannelListView.Description);

            // load children
            SnapIn.Router.WnsChannels.Reloading += (sender, e) => { Children.Clear(); };
            SnapIn.Router.WnsChannels.Reloaded += (sender, e) =>
            {
                foreach (var channel in SnapIn.Router.WnsChannels)
                {
                    Children.Add(new WnsChannelNode(channel));
                }
            };
        }

        #endregion construction

        #region overrides

        //  ----------------
        //  OnRefresh method
        //  ----------------

        protected override void OnRefresh(AsyncStatus status)
        {
            SnapIn.Console.Invoke(SnapIn.Router.WnsChannels.Reload);
        }

        //  ---------------
        //  OnExpand method
        //  ---------------

        protected override void OnExpand(AsyncStatus status)
        {
            SnapIn.Router.WnsChannels.Reload(false);
        }

        #endregion overrides
    }
}

// eof "WnsChannelListNode.cs"
