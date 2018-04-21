//
//  @(#) ApnsChannelListNode.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using usis.ManagementConsole;
using usis.Platform;

namespace usis.PushNotification.Administration
{
    //  -------------------------
    //  ApnsChannelListNode class
    //  -------------------------

    internal sealed class ApnsChannelListNode : ScopeNode<SnapIn>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public ApnsChannelListNode()
        {
            DisplayName = Strings.APNs;

            // refresh
            EnabledStandardVerbs = StandardVerbs.Refresh;

            // channel view
            ViewDescriptions.Add(ApnsChannelListView.Description);

            // load children
            SnapIn.Router.ApnsChannels.Reloading += (sender, e) => { Children.Clear(); };
            SnapIn.Router.ApnsChannels.Reloaded += (sender, e) =>
            {
                foreach (var channel in SnapIn.Router.ApnsChannels)
                {
                    Children.Add(new ApnsChannelNode(channel));
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
            SnapIn.Console.Invoke(SnapIn.Router.ApnsChannels.Reload);
        }

        //  ---------------
        //  OnExpand method
        //  ---------------

        protected override void OnExpand(AsyncStatus status)
        {
            SnapIn.Router.ApnsChannels.Reload(false);
        }

        #endregion overrides
    }
}

// eof "ApnsChannelListNode.cs"
