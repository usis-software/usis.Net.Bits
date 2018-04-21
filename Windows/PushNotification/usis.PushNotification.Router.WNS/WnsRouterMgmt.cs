//
//  @(#) WnsRouterMgmt.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using usis.Framework;
using usis.Framework.ServiceModel;

namespace usis.PushNotification
{
    //  -------------------
    //  WnsRouterMgmt class
    //  -------------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal sealed class WnsRouterMgmt : ServiceBase, IWnsRouterMgmt
    {
        #region CreateChannel method

        //  --------------------
        //  CreateChannel method
        //  --------------------

        OperationResult IWnsRouterMgmt.CreateChannel(WnsChannelKey channelKey)
        {
            return OperationResult.Invoke(() => Model.FindOrCreateChannel(channelKey));
        }

        #endregion CreateChannel method

        #region UpdateChannel method

        //  --------------------
        //  UpdateChannel method
        //  --------------------

        OperationResult IWnsRouterMgmt.UpdateChannel(WnsChannelInfo channelInfo)
        {
            return OperationResult.Invoke(() => Router.UpdateChannel(channelInfo));
        }

        #endregion UpdateChannel method

        #region ListChannels method

        //  -------------------
        //  ListChannels method
        //  -------------------

        IEnumerable<WnsChannelInfo> IWnsRouterMgmt.ListChannels()
        {
            return Model.ListChannels(ChannelType.WindowsNotificationService).Cast<WnsChannelInfo>();
        }

        #endregion ListChannels method

        #region DeleteChannel method

        //  --------------------
        //  DeleteChannel method
        //  --------------------

        OperationResult IWnsRouterMgmt.DeleteChannel(WnsChannelKey channelKey)
        {
            return OperationResult.Invoke(() => Router.DeleteChannel(channelKey));
        }

        #endregion DeleteChannel method

        #region ListDevices method

        //  ------------------
        //  ListDevices method
        //  ------------------

        OperationResultList<WnsReceiverInfo> IWnsRouterMgmt.ListDevices(WnsChannelKey channelKey)
        {
            return OperationResultList.Invoke(() => Model.ListReceivers(channelKey, null, false).Cast<WnsReceiverInfo>());
        }

        #endregion ListDevices method
    }

    #region WnsRouterMgmtSnapIn class

    //  -------------------------
    //  WnsRouterMgmtSnapIn class
    //  -------------------------

    internal class WnsRouterMgmtSnapIn : NamedPipeServiceHostSnapIn<WnsRouterMgmt, IWnsRouterMgmt>
    {
        public WnsRouterMgmtSnapIn() : base(false, false) { }
    }

    #endregion WnsRouterMgmtSnapIn class
}

// eof "WnsRouterMgmt.cs"
