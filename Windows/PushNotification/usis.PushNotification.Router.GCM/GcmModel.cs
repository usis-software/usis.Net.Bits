//
//  @(#) GcmModel.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using PushSharp.Google;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;

namespace usis.PushNotification
{
    //  --------------
    //  GcmModel class
    //  --------------

    internal sealed class GcmModel : IModel
    {
        #region IModel implementation

        #region channel

        #region ListChannels method

        //  -------------------
        //  ListChannels method
        //  -------------------

        IEnumerable<IChannelInfo> IModel.ListChannels(DBContext db)
        {
            foreach (var channel in db.GcmChannels.Include(nameof(Channel)))
            {
                if (channel.Deleted != 0) continue;
                var channelInfo = new GcmChannelInfo()
                {
                    Key = new GcmChannelKey(channel.SenderId),
                    Description = channel.Channel.Description,
                    // TODO: set all properties
                    Created = DateTime.SpecifyKind(channel.Created, DateTimeKind.Utc),
                    Changed = channel.Changed.Later(channel.Channel.Changed)
                };
                if (channelInfo.Changed.HasValue) channelInfo.Changed = DateTime.SpecifyKind(channelInfo.Changed.Value, DateTimeKind.Utc);
                yield return channelInfo;
            }
        }

        #endregion ListChannels method

        #region GetChannel method

        //  -----------------
        //  GetChannel method
        //  -----------------

        IChannel IModel.GetChannel(DBContext db, Guid channelId)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));

            var query = from c in db.GcmChannels
                        where c.ChannelId == channelId
                        select c;
            return query.FirstOrDefault();
        }

        #endregion GetChannel method

        #region FindChannel method

        //  ------------------
        //  FindChannel method
        //  ------------------

        IChannel IModel.FindChannel(DBContext db, ChannelKey channelKey)
        {
            return FindChannel(db, channelKey as GcmChannelKey, false);
        }

        #endregion FindChannel method

        #region FindOrCreateChannel method

        //  --------------------------
        //  FindOrCreateChannel method
        //  --------------------------

        IChannel IModel.FindOrCreateChannel(DBContext db, ChannelKey channelKey)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));
            if (channelKey == null) throw new ArgumentNullException(nameof(channelKey));

            var gcmChannelKey = channelKey as GcmChannelKey;
            var channel = FindChannel(db, gcmChannelKey, true);
            if (channel == null)
            {
                channel = GcmChannel.NewChannel(gcmChannelKey);
                db.GcmChannels.Add(channel);
                return db.SaveChanges() > 0 ? channel : null;
            }
            else if (channel.Deleted != 0)
            {
                db.Entry(channel).Reference(c => c.Channel).Load();
                channel.Channel.ChannelType = channelKey.ChannelType;
                channel.Channel.Description = null;
                channel.Deleted = 0;
                channel.Channel.Deleted = 0;
                return db.SaveChanges() > 0 ? channel : null;
            }
            else
            {
                db.Entry(channel).Reference(c => c.Channel).Load();
                return channel;
            }
        }

        #endregion FindOrCreateChannel method

        #region UpdateChannel method

        //  --------------------
        //  UpdateChannel method
        //  --------------------

        void IModel.UpdateChannel(DBContext db, IChannel channel, IChannelInfo channelInfo)
        {
            UpdateChannel(channel as GcmChannel, channelInfo as GcmChannelInfo);
        }

        private static void UpdateChannel(GcmChannel channel, GcmChannelInfo channelInfo)
        {
            if (channel == null) throw new ArgumentNullException(nameof(channel));
            if (channelInfo == null) throw new ArgumentNullException(nameof(channelInfo));

            channel.Channel.Description = channelInfo.Description;
        }

        #endregion UpdateChannel method

        #endregion channel

        #region receiver

        #region CreateReceiver method

        //  ---------------------
        //  CreateReceiver method
        //  ---------------------

        IReceiver IModel.CreateReceiver(DBContext db, IChannel channel, ReceiverKey receiverKey)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));
            if (channel == null) throw new ArgumentNullException(nameof(channel));

            var gcmReceiverKey = receiverKey as GcmReceiverKey;
            Debug.Assert(gcmReceiverKey != null);
            var receiver = GcmReceiver.NewReceiver(channel.Channel, gcmReceiverKey.RegistrationToken);
            db.GcmReceivers.Add(receiver);
            return receiver;
        }

        #endregion CreateReceiver method

        #region ListReceivers method

        //  --------------------
        //  ListReceivers method
        //  --------------------

        IEnumerable<IReceiverInfo> IModel.ListReceivers(DBContext db, IChannel channel, DateTime? firstRegistration, bool includeExpired)
        {
            var query = from r in db.GcmReceivers
                        where r.Receiver.ChannelId == channel.Channel.ChannelId && r.Deleted == 0 && r.Receiver.Deleted == 0
                        select r;
            if (firstRegistration.HasValue) query = query.Where(e => e.Created >= firstRegistration);
            foreach (var device in query.Include(d => d.Receiver))
            {
                yield return new GcmReceiverInfo()
                {
                    RegistrationToken = device.RegistrationToken,
                    ReceiverId = device.ReceiverId,
                    Name = device.Receiver.Name,
                    Account = device.Receiver.Account,
                    Groups = device.Receiver.Groups,
                    Info = device.Receiver.Info,
                    FirstRegistration = DateTime.SpecifyKind(device.Created, DateTimeKind.Utc),
                    LastRegistration = DateTime.SpecifyKind(device.Changed ?? device.Created, DateTimeKind.Utc),
                };
            }
        }

        #endregion ListReceivers method

        #region FindReceiver method

        //  -------------------
        //  FindReceiver method
        //  -------------------

        IReceiver IModel.FindReceiver(DBContext db, ReceiverKey receiverKey)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));

            var gcmReceiverKey = receiverKey as GcmReceiverKey;
            Debug.Assert(gcmReceiverKey != null);
            return db.GcmReceivers.Find(gcmReceiverKey.RegistrationToken);
        }

        #endregion FindReceiver method

        #region GetReceiver method

        //  ------------------
        //  GetReceiver method
        //  ------------------

        IReceiver IModel.GetReceiver(DBContext db, Guid receiverId)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));

            var query = from r in db.GcmReceivers
                        where r.ReceiverId == receiverId
                        select r;
            return query.FirstOrDefault();
        }

        #endregion GetReceiver method

        #region UpdateReceiver method

        //  ---------------------
        //  UpdateReceiver method
        //  ---------------------

        void IModel.UpdateReceiver(IReceiver receiver, ReceiverKey receiverKey) { }

        #endregion UpdateReceiver method

        #endregion receiver

        #region CreatePusher method

        //  -------------------
        //  CreatePusher method
        //  -------------------

        IPusher IModel.CreatePusher(IChannel channel, int keepChannelOpenInterval)
        {
            return new GcmPusher(channel as GcmChannel);
        }

        #endregion CreatePusher method

        #endregion IModel implementation

        #region private methods

        //  ------------------
        //  FindChannel method
        //  ------------------

        private static GcmChannel FindChannel(DBContext db, GcmChannelKey channelKey, bool includeDeleted)
        {
            var channel = db.GcmChannels.Find(channelKey.SenderId);
            if (channel != null)
            {
                if (channel.Deleted != 0 && !includeDeleted) channel = null;
            }
            return channel;
        }

        //  -------------------
        //  FindReceiver method
        //  -------------------

        //private static ApnsReceiver FindReceiver(DBContext db, ApnsDeviceToken deviceToken)
        //{
        //    var query = from r in db.ApnsReceivers
        //                where r.DeviceToken == deviceToken.HexString
        //                select r;
        //    return query.FirstOrDefault();
        //}

        #endregion private methods

        #region static methods

        #region SaveNotification method

        //  -----------------------
        //  SaveNotification method
        //  -----------------------

        //internal static Guid SaveNotification(OperationResult result, Model model, string deviceIdentifier, string payload)
        //{
        //    using (var db = model.CreateDBContext())
        //    {
        //        var receiver = FindReceiver(db, deviceIdentifier);
        //        if (receiver != null)
        //        {
        //            return Model.SaveNotification(result, db, receiver.ReceiverId, payload);
        //        }
        //        throw new RouterException(string.Format(CultureInfo.CurrentCulture, WnsStrings.ReceiverNotRegistered, deviceIdentifier));
        //    }
        //}

        #endregion SaveNotification method

        #region CreateConfiguration method

        //  --------------------------
        //  CreateConfiguration method
        //  --------------------------

        internal static GcmConfiguration CreateConfiguration(GcmChannel channel)
        {
            return new GcmConfiguration(channel.SenderId, channel.ServerKey, channel.ApplicationId);
        }

        #endregion CreateConfiguration method

        #endregion static methods
    }
}

// eof "GcmModel.cs"
