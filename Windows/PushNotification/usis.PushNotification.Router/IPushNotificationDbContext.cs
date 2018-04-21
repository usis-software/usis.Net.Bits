//
//  @(#) IPushNotificationDbContext.cs
//
//  Project:    usis Push Notification Server
//  System:     Microsoft Visual Studio 12.0
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2014 usis GmbH. All rights reserved.

#pragma warning disable 1591

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using usis.Framework;

namespace usis.Server.PushNotification
{
	#region IPushNotificationDbContext interface

	//	------------------------------------
	//	IPushNotificationDbContext interface
	//	------------------------------------

	public interface IDbContext : usis.Data.Entity.IDbContext
	{
		DbSet<APNsDeviceToken> DeviceTokens
		{
			get;
		}
	
		DbSet<APNsChannel> Channels
		{
			get;
		}

	} // IPushNotificationDbContext interface

	#endregion IPushNotificationDbContext interface

	#region APNsDeviceToken class

	//	---------------------
	//	APNsDeviceToken class
	//	---------------------

	public class APNsDeviceToken : BaseEntity
	{
		[Key]
		[Column(Order = 0)]
		public string AppId
		{
			get;
			set;
		}

		[Key]
		[Column(Order = 1)]
		public Environment Environment
		{
			get;
			set;
		}

		[Key]
		[Column(Order = 2)]
		[StringLength(64)]
		public string DeviceToken
		{
			get;
			set;
		}

	} // APNsDeviceToken class

	#endregion // APNsDeviceToken class

	#region APNsChannel class

	//	-----------------
	//	APNsChannel class
	//	-----------------

	public class APNsChannel : BaseEntity
	{
		[Key]
		[Column(Order = 0)]
		public string AppId
		{
			get;
			set;
		}

		[Key]
		[Column(Order = 1)]
		public Environment Environment
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string Thumbprint
		{
			get;
			set;
		}

		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		public byte[] Certificate
		{
			get;
			set;
		}
	
	} // APNsChannel class

	#endregion APNsChannel class

} // usis.Server.PushNotification namespace

// eof "IPushNotificationDbContext.cs"
