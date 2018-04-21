//
//  @(#) RegisterForRemoteNotificationNetworkOperation.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Foundation;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Globalization;
using usis.Cocoa.Net;
using usis.Framework;
using usis.Framework.Net;
using usis.PushNotification;

#pragma warning disable 1591

namespace usis.Cocoa.PushNotification
{
    //  ---------------------------------------------------
    //  RegisterForRemoteNotificationNetworkOperation class
    //  ---------------------------------------------------

    public sealed class RegisterForRemoteNotificationNetworkOperation : JsonWebServiceNetworkOperation<OperationResult<Guid>>
    {
        #region properties

        //  ----------
        //  properties
        //  ----------

        [JsonProperty(PropertyName = "deviceToken")]
        public string DeviceToken { get; private set; }

        [JsonProperty(PropertyName = "bundleId")]
        public string BundleId { get; set; }

        [JsonProperty(PropertyName = "environment")]
        public usis.PushNotification.Environment Environment { get; set; }

        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "account", NullValueHandling = NullValueHandling.Ignore)]
        public string Account { get; set; }

        [JsonProperty(PropertyName = "groups", NullValueHandling = NullValueHandling.Ignore)]
        public string Groups { get; set; }

        [JsonProperty(PropertyName = "info", NullValueHandling = NullValueHandling.Ignore)]
        public string Info { get; set; }

        [JsonIgnore]
        public Guid? ReceiverId { get; set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterForRemoteNotificationNetworkOperation"/> class
        /// with the specified device token.
        /// </summary>
        /// <param name="deviceToken">The device token.</param>
        /// <exception cref="ArgumentNullException"><i>deviceToken</i> is a null reference.</exception>

        public RegisterForRemoteNotificationNetworkOperation(NSData deviceToken) :
            base((result, exception) => Debug.WriteLine(result))
        {
            if (deviceToken == null) throw new ArgumentNullException(nameof(deviceToken));

            DeviceToken = ApnsDeviceToken.FromBytes(deviceToken.ToArray()).Base64String;
            BundleId = NSBundle.MainBundle.BundleIdentifier;
            Environment = usis.PushNotification.Environment.Development;
        }

        #endregion construction

        #region overrides

        //  --------------------
        //  CreateRequest method
        //  --------------------

        protected override void CreateRequest(IApplication application)
        {
            string host;
            host = "usis-push.com";
            //host = "82.223.9.88";
            //host = "192.168.178.29";
            //var host = "192.168.214.127";
            //var host = "usis-svr01.swe-qs.lan";
            //var host = "usis-svr03";
            var url = string.Format(CultureInfo.InvariantCulture, "http://{0}/Apns.web/RegisterReceiver", host);

            Request = CreateJsonRequest(new Uri(url), HttpRequest.Post, this);
        }

        #endregion overrides
    }
}

// eof "RegisterForRemoteNotificationNetworkOperation.cs"
