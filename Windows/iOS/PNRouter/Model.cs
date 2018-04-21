//
//  @(#) Model.cs
//
//  Project:    PNRouter iOS App
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using usis.Cocoa.Net;
using usis.Framework;
using usis.Framework.Net;
using usis.Mobile;
using usis.Platform;
using usis.PushNotification;

namespace usis.Cocoa.PNRouter
{
    //  -----------
    //  Model class
    //  -----------

    internal class Model : AppModel
    {
        Uri hostUri = new Uri("http://usis-push.com");
        //Uri hostUri = new Uri("http://usis-svr05");
        //Uri hostUri = new Uri("http://usis-svr06");

        #region properties

        //  -----------------
        //  Channels property
        //  -----------------

        internal ReloadableCollection<ApnsChannelInfo> Channels { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        public Model()
        {
            Channels = new ReloadableCollection<ApnsChannelInfo>();
            Channels.PerformReload += (sender, e) =>
            {
                var operation = new ListChannelsOperation(hostUri, (result, exception) =>
                {
                    System.Threading.Thread.Sleep(450);
                    Channels.Replace(result?.ReturnValue);
                });
                Owner.PerformNetworkOperation(operation);
            };
        }

        #endregion construction

        #region methods

        //  ------------------
        //  LoadDevices method
        //  ------------------

        public void LoadDevices(ApnsChannelKey channelKey, Action<IEnumerable<ApnsReceiverInfo>> completionHandler)
        {
            var uri = new Uri(hostUri, "APNsRouter.web/ListDevices");
            var operation = new JsonWebServiceNetworkOperation<OperationResult<IEnumerable<ApnsReceiverInfo>>>(
                uri, HttpRequest.Post, channelKey,
                (result, exception) =>
                {
                    if (result != null && result.ReturnCode < 0) Owner.ReportException(result.CreateException());
                    completionHandler(result?.ReturnValue);
                });
            operation.Request.Headers["Authorization"] = "Basic QWRtaW5pc3RyYXRvcjpjYSw5Mzk1MA==";
            Owner.PerformNetworkOperation(operation);
        }

        #endregion methods

        #region ListChannelsOperation class

        //  ---------------------------
        //  ListChannelsOperation class
        //  ---------------------------

        private class ListChannelsOperation : JsonWebServiceNetworkOperation<OperationResultList<ApnsChannelInfo>>
        {
            public ListChannelsOperation(Uri hostUri, Action<OperationResultList<ApnsChannelInfo>, Exception> handler) : base(handler)
            {
                var uri = new Uri(hostUri, "APNsRouter.web/ListChannels");
                Request = CreateJsonRequest(uri, HttpRequest.Post, null);
                Request.Headers["Authorization"] = "Basic QWRtaW5pc3RyYXRvcjpjYSw5Mzk1MA==";
            }
        }

        #endregion ListChannelsOperation class
    }
}

// eof "Model.cs"
