//
//	@(#) PushNotification.cs
//
//  Project:    usisSvr
//  System:     Microsoft Visual Studio 12
//	Author:		Udo Schäfer
//
//	Copyright (c) 2014 usis GmbH. All rights reserved.

using PushSharp;
using PushSharp.Apple;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using usis.Platform;

namespace usis.Server.PushNotification
{
	#region IApplePushNotificationService interface

	//	---------------------------------------
	//	IApplePushNotificationService interface
	//	---------------------------------------

	[ServiceContract]
	interface IApplePushNotificationService
	{
		[OperationContract]
		[WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat=WebMessageFormat.Json)]
		void RegisterDevice(string bundleIdentifier, bool production, string deviceToken);
	
	} // IApplePushNotificationService interface

	#endregion IApplePushNotificationService interface

	#region ApplePushNotificationService class

	//  ----------------------------------
    //  ApplePushNotificationService class
    //  ----------------------------------

	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	internal class ApplePushNotificationService : IApplePushNotificationService
    {
        #region RegisterDevice method

        //	---------------------
		//	RegisterDevice method
		//	---------------------
		//	{"deviceToken":"KRhNpiVPslbGcGtHJTX90jhx4IwJhLS7u1zCyk+UyPo=","bundleIdentifier":"de.usis.Demo"}

		void IApplePushNotificationService.RegisterDevice(string bundleIdentifier, bool production, string deviceToken)
		{
			if (string.IsNullOrWhiteSpace(bundleIdentifier)) throw new ArgumentNullOrWhiteSpaceException("bundleIdentifier");
			if (string.IsNullOrWhiteSpace(deviceToken)) throw new ArgumentNullOrWhiteSpaceException("deviceToken");

			string hexDeviceToken = ConvertDeviceTokenFromBase64ToHex(deviceToken);

			Debug.Print("RegisterDevice: bundleIdentifier=\"{0}\", deviceToken=\"{1}\"", bundleIdentifier, hexDeviceToken);
		
            using (var db = new usis.DbContext())
            {
                db.RegisterAppleDeviceToken(bundleIdentifier, production, hexDeviceToken);
            }

        } // RegisterDevice method

        #endregion RegisterDevice method
        
        #region ConvertDeviceTokenFromBase64ToHex method

        //	----------------------------------------
		//	ConvertDeviceTokenFromBase64ToHex method
		//	----------------------------------------

		private static string ConvertDeviceTokenFromBase64ToHex(string base64DeviceToken)
		{
			byte[] binaryDeviceToken = Convert.FromBase64String(base64DeviceToken);
			return BitConverter.ToString(binaryDeviceToken).Replace("-", string.Empty);

		} // ConvertDeviceTokenFromBase64ToHex method

		#endregion ConvertDeviceTokenFromBase64ToHex method

	} // ApplePushNotificationService class

	#endregion ApplePushNotificationService class

	[ServiceContract]
	interface IApplePushNotificationRouter
	{
		[OperationContract]
		[WebGet]
		void Register(string appID, string certificate);

		[OperationContract]
		[WebGet]
		void Send(string bundleIdentifier, string deviceToken, string payload);
	}

	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	internal class ApplePushNotificationRouter : IApplePushNotificationRouter
	{
		void IApplePushNotificationRouter.Send(string bundleIdentifier, string deviceToken, string payload)
		{
			if (string.IsNullOrWhiteSpace(bundleIdentifier)) throw new ArgumentNullOrWhiteSpaceException("bundleIdentifier");
			if (string.IsNullOrWhiteSpace(payload)) throw new ArgumentNullOrWhiteSpaceException("payload");

			using (var db = new DbContext())
			{
				foreach (var device in db.FindAppleDeviceTokens(bundleIdentifier, deviceToken))
				{
					using (var push = new PushBroker())
					{
						var cert = File.ReadAllBytes("de.usis.Demo.Push.Dev.p12");
						push.RegisterAppleService(new ApplePushChannelSettings(cert, "master"));
						push.QueueNotification(new AppleNotification()
							   .ForDeviceToken(device.DeviceToken)
							   .WithAlert(payload)
                               .WithBadge(0)
							   .WithSound("sound.caf"));
						push.StopAllServices();
					}
                    Debug.Print("Sent: bundleIdentifier=\"{0}\", deviceToken=\"{1}\"", device.AppID, device.DeviceToken);
                }
			}
		}

		void IApplePushNotificationRouter.Register(string appID, string certificate)
		{
			throw new NotImplementedException();
		}
	}

} // usis.Server.PushNotification namespace

// eof "PushNotification.cs"