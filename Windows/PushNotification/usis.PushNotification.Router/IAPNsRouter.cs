//
//  @(#) IAPNsRouter.cs
//
//  Project:    usis Push Notification Server
//  System:     Microsoft Visual Studio 12.0
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2014 usis GmbH. All rights reserved.

using System.ServiceModel;
using System.ServiceModel.Web;

namespace usis.Server.PushNotification
{
	#region IAPNsRouter interface

	//	---------------------
	//	IAPNsRouter interface
	//	---------------------

	[ServiceContract]
	public interface IAPNsRouter
	{
		[OperationContract]
		[WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped,
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		bool SendNotification(
			APNsChannelKey channelKey,
			string deviceToken,
			string payload);

	} // IAPNsRouter interface

	#endregion IAPNsRouter interface

} // usis.Server.PushNotification namespace

// eof "IAPNsRouter.cs"
