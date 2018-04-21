//
//  @(#) ApnsModuleSnapIn.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System.ComponentModel;

namespace usis.PushNotification
{
    //  ----------------------
    //  ApnsModuleSnapIn class
    //  ----------------------

    /// <summary>
    /// Provides a snap-in to use the Apple Push Notification service
    /// to send notifications.
    /// </summary>
    /// <seealso cref="Framework.SnapIn" />

    public sealed class ApnsModuleSnapIn : Framework.SnapIn
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
            Model.RegisterPushServiceModel(ChannelType.ApplePushNotificationService, new ApnsModel());

            Application.Extensions.Add(new ApnsFeedback());

            Application.ConnectRequiredSnapIns(this,
                typeof(Web.ApnsSnapIn),
                typeof(ApnsRouterSnapIn),
                typeof(Web.ApnsRouterSnapIn),
                //typeof(ApnsDownloadSnapIn),
                typeof(ApnsRouterMgmtSnapIn));

            base.OnConnecting(e);
        }

        #endregion overrides
    }
}

// eof "ApnsModuleSnapIn.cs"
