//
//  @(#) Interfaces.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using usis.Framework.Data.Entity;

namespace usis.PushNotification
{
    #region IModel interface

    //  ----------------
    //  IModel interface
    //  ----------------

    /// <summary>
    /// Defines method that a push service model must implement to be accessed by WCF services.
    /// </summary>

    public interface IModel
    {
        //  -------------------
        //  ListChannels method
        //  -------------------

        /// <summary>
        /// Lists the channels of the push service type.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <returns>
        /// An enumerator to iterate thru all channels.
        /// </returns>

        IEnumerable<IChannelInfo> ListChannels(DBContext db);

        //  --------------------
        //  UpdateChannel method
        //  --------------------

        /// <summary>
        /// Updates a channel with the specified channel information.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="channel">The channel.</param>
        /// <param name="channelInfo">The channel information.</param>

        void UpdateChannel(DBContext db, IChannel channel, IChannelInfo channelInfo);

        //  -----------------
        //  GetChannel method
        //  -----------------

        /// <summary>
        /// Gets the channel with the specifed channel identifier.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="channelId">The channel identifier.</param>
        /// <returns>
        /// The channel with the specifed channel identifier.
        /// </returns>

        IChannel GetChannel(DBContext db, Guid channelId);

        //  ------------------
        //  FindChannel method
        //  ------------------

        /// <summary>
        /// Finds the channel with the specifed channel key.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="channelKey">The channel key.</param>
        /// <returns>
        /// The channel with the specifed channel key
        /// or <b>null</b> if the channel was not found.
        /// </returns>

        IChannel FindChannel(DBContext db, ChannelKey channelKey);

        //  --------------------------
        //  FindOrCreateChannel method
        //  --------------------------

        /// <summary>
        /// Finds the channel with the specifed channel key or creates new one.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="channelKey">The channel key.</param>
        /// <returns>
        /// The channel with the specifed channel key
        /// or a newly created channel.
        /// </returns>

        IChannel FindOrCreateChannel(DBContext db, ChannelKey channelKey);

        //  ------------------
        //  GetReceiver method
        //  ------------------

        /// <summary>
        /// Gets the receiver with the specified receiver identifier.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="receiverId">The receiver identifier.</param>
        /// <returns>
        /// The receiver with the specified receiver identifier.
        /// </returns>

        IReceiver GetReceiver(DBContext db, Guid receiverId);

        //  -------------------
        //  FindReceiver method
        //  -------------------

        /// <summary>
        /// Finds the receiver with the specified receiver key.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="receiverKey">The receiver key.</param>
        /// <returns>
        /// The receiver with the specified receiver key
        /// or <b>null</b> if the receiver was not found.
        /// </returns>

        IReceiver FindReceiver(DBContext db, ReceiverKey receiverKey);

        //  ---------------------
        //  CreateReceiver method
        //  ---------------------

        /// <summary>
        /// Creates a receiver for a channel with the specified receiver key.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <param name="channel">The channel.</param>
        /// <param name="receiverKey">The receiver key.</param>
        /// <returns>
        /// A newly created receiver with the specified receiver key.
        /// </returns>

        IReceiver CreateReceiver(DBContext db, IChannel channel, ReceiverKey receiverKey);

        //  ---------------------
        //  UpdateReceiver method
        //  ---------------------

        /// <summary>
        /// Updates a receiver with the specified receiver key.
        /// </summary>
        /// <param name="receiver">The receiver.</param>
        /// <param name="receiverKey">The receiver key.</param>

        void UpdateReceiver(IReceiver receiver, ReceiverKey receiverKey);

        //  --------------------
        //  ListReceivers method
        //  --------------------

        /// <summary>
        /// Lists all receivers of a specified channel.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="channel">The channel.</param>
        /// <param name="firstRegistration">The date and time of the first registration.</param>
        /// <param name="includeExpired">if set to <c>true</c> the list includes expired receivers.</param>
        /// <returns>
        /// An enumerator to iterate thru the receivers.
        /// </returns>

        IEnumerable<IReceiverInfo> ListReceivers(DBContext db, IChannel channel, DateTime? firstRegistration, bool includeExpired);

        //  -------------------
        //  CreatePusher method
        //  -------------------

        /// <summary>
        /// Creates a pusher for the specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="keepChannelOpenInterval">
        /// The time in interval to keep the channel open after a notification was pushed.
        /// </param>
        /// <returns>
        /// A newly created pusher for the specified channel.
        /// </returns>

        IPusher CreatePusher(IChannel channel, int keepChannelOpenInterval);
    }

    #endregion IModel interface

    #region IChannel interface

    //  ------------------
    //  IChannel interface
    //  ------------------

    /// <summary>
    /// Provides a property to access the base channel entity.
    /// </summary>
    /// <seealso cref="IEntityBase" />

    public interface IChannel : IEntityBase
    {
        //  ----------------
        //  Channel property
        //  ----------------

        /// <summary>
        /// Gets the channel base entity.
        /// </summary>
        /// <value>
        /// The channel base entity.
        /// </value>

        Channel Channel { get; }
    }

    #endregion IChannel interface

    #region IReceiver interface

    //  -------------------
    //  IReceiver interface
    //  -------------------

    /// <summary>
    /// Provides a property to access the base receiver entity.
    /// </summary>
    /// <seealso cref="IEntityBase" />

    public interface IReceiver : IEntityBase
    {
        //  -----------------
        //  Receiver property
        //  -----------------

        /// <summary>
        /// Gets the receiver base entity.
        /// </summary>
        /// <value>
        /// The receiver base entity.
        /// </value>

        Receiver Receiver { get; }
    }

    #endregion IReceiver interface
}

// eof "Interfaces.cs"
