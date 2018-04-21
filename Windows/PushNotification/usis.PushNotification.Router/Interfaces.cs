//
//  @(#) Interfaces.cs
//
//  Project:    usis Push Notification Server
//  System:     Microsoft Visual Studio 12.0
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2014 usis GmbH. All rights reserved.

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace usis.Server.PushNotification
{
	#region APNsEnvironment enumeration

	//	---------------------------
	//	APNsEnvironment enumeration
	//	---------------------------
	/// <summary>
	/// Specifies the environment in which to send push notifications.
	/// </summary>

	public enum APNsEnvironment
	{
		/// <summary>
		/// Specifies the development environment.
		/// </summary>
		
		Development,

		/// <summary>
		/// Specifies the production environment.
		/// </summary>
		
		Production

	} // APNsEnvironment enumeration

	#endregion APNsEnvironment enumeration

	#region APNsChannelKey class

	//	--------------------
	//	APNsChannelKey class
	//	--------------------
	/// <summary>
	/// Represents a channel uniquely.
	/// </summary>

	[DataContract]
	public class APNsChannelKey
	{
		/// <summary>
		/// Gets or sets the application identifier.
		/// </summary>
		/// <value>
		/// The application identifier.
		/// </value>

		[DataMember]
		public string AppId
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the environment for push notifications.
		/// </summary>
		/// <value>
		/// The environment for push notifications.
		/// </value>

		[DataMember]
		public APNsEnvironment Environment
		{
			get;
			set;
		}

	} // APNsChannelKey class

	#endregion APNsChannelKey class

#pragma warning disable 1591

	#region APNsChannelInfo class

	//	---------------------
	//	APNsChannelInfo class
	//	---------------------

	[DataContract]
	public class APNsChannelInfo : APNsChannelKey
	{
		[DataMember]
		public string Description
		{
			get;
			set;
		}
	
	} // APNsChannelInfo class

	#endregion APNsChannelInfo class

	#region IAPNsRouter interface

	//	---------------------
	//	IAPNsRouter interface
	//	---------------------

	//[ServiceContract]
	//public interface IAPNsRouter
	//{
	//	[OperationContract]
	//	[WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped,
	//		RequestFormat = WebMessageFormat.Json,
	//		ResponseFormat = WebMessageFormat.Json)]
	//	bool SendNotification(
	//		APNsChannelKey channelKey,
	//		string deviceToken,
	//		string payload);

	//} // IAPNsRouter interface

	#endregion IAPNsRouter interface

	#region IAPNsRouterMgmt interface

	//	-------------------------
	//	IAPNsRouterMgmt interface
	//	-------------------------

	[ServiceContract]
	public interface IAPNsRouterMgmt
	{
		[OperationContract]
		[WebInvoke(
			BodyStyle = WebMessageBodyStyle.Wrapped,
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		bool CreateChannel(APNsChannelKey channelKey);

		[OperationContract]
		[WebInvoke(
			BodyStyle = WebMessageBodyStyle.Wrapped,
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		bool UpdateChannel(APNsChannelInfo channelInfo);

		[OperationContract]
		[WebGet(
			BodyStyle = WebMessageBodyStyle.Wrapped,
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		IEnumerable<APNsChannelInfo> ListChannels();

		[OperationContract]
		[WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped,
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		bool UpdateCertificate(APNsChannelKey channelKey, byte[] certificate);
	
	} // IAPNsRouterMgmt interface

	#endregion IAPNsRouterMgmt interface

} // usis.Server.PushNotification namespace

// eof "Interfaces.cs"
