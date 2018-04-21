//
//  @(#) IModel.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2016 usis GmbH. All rights reserved.

using System;
using usis.PushNotification;

namespace usis.Server.PushNotification
{
    //  ----------------
    //  IModel interface
    //  ----------------

    internal interface IModel
    {
        IChannel GetChannel(DbContext db, Guid channelId);

        IChannel FindChannel(DbContext db, ChannelKey channelKey);

        IChannel FindOrCreateChannel(DbContext db, ChannelKey channelKey);

        IReceiver GetReceiver(DbContext db, Guid receiverId);

        IReceiver FindReceiver(DbContext db, ReceiverKey receiverKey);

        IReceiver CreateReceiver(DbContext db, IChannel channel, ReceiverKey receiverKey);
    }
}

// eof "IModel.cs"
