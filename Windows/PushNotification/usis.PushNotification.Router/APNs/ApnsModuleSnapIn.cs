//
//  @(#) ApnsModuleSnapIn.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System.ComponentModel;

namespace usis.PushNotification
{
    //  ----------------------
    //  ApnsModuleSnapIn class
    //  ----------------------

    public sealed class ApnsModuleSnapIn : Framework.Portable.SnapIn
    {
        #region overrides

        //  -------------------
        //  OnConnecting method
        //  -------------------

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
