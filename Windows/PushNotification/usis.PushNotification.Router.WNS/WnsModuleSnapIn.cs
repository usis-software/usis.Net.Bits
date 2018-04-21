//
//  @(#) WnsModuleSnapIn.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System.ComponentModel;

namespace usis.PushNotification
{
    //  ---------------------
    //  WnsModuleSnapIn class
    //  ---------------------

    /// <summary>
    /// Provides a snap-in to use the Windows Notification Service
    /// to send notifications.
    /// </summary>
    /// <seealso cref="Framework.SnapIn" />

    public sealed class WnsModuleSnapIn : Framework.SnapIn
    {
        #region overrides

        //  -------------------
        //  OnConnecting method
        //  -------------------

        /// <summary>
        /// Raises the <see cref="Framework.SnapIn.Connecting" /> event.
        /// </summary>
        /// <param name="e">A <see cref="CancelEventArgs" /> object that contains the event data.</param>

        protected override void OnConnecting(CancelEventArgs e)
        {
            Model.RegisterPushServiceModel(ChannelType.WindowsNotificationService, new WnsModel());

            Application.ConnectRequiredSnapIns(this,
                typeof(Web.WnsSnapIn),
                typeof(WnsSnapIn),
                typeof(WnsRouterSnapIn),
                typeof(WnsRouterMgmtSnapIn));

            base.OnConnecting(e);
        }

        #endregion overrides
    }
}

// eof "WnsModuleSnapIn.cs"
