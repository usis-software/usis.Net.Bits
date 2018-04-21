//
//  @(#) ApnsModel.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using PushSharp.Apple;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using usis.Framework;

namespace usis.PushNotification
{
    //  ---------------
    //  ApnsModel class
    //  ---------------

    internal sealed class ApnsModel : IModel
    {
        #region IModel implementation

        #region channel

        #region ListChannelInfos method

        //  -----------------------
        //  ListChannelInfos method
        //  -----------------------

        IEnumerable<IChannelInfo> IModel.ListChannels(DBContext db)
        {
            using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadWrite);

                foreach (var channel in db.ApnsChannels.Include(nameof(Channel)))
                {
                    if (channel.Deleted != 0) continue;
                    var channelInfo = new ApnsChannelInfo()
                    {
                        Key = new ApnsChannelKey(channel.BundleId, channel.Environment),
                        Description = channel.Channel.Description,
                        HasCertificate = !string.IsNullOrWhiteSpace(channel.Certificate),
                        Created = DateTime.SpecifyKind(channel.Created, DateTimeKind.Utc),
                        Changed = channel.Changed.Later(channel.Channel.Changed)
                    };
                    if (channelInfo.Changed.HasValue) channelInfo.Changed = DateTime.SpecifyKind(channelInfo.Changed.Value, DateTimeKind.Utc);
                    if (!string.IsNullOrWhiteSpace(channel.Thumbprint))
                    {
                        var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, channel.Thumbprint, false);
                        if (certificates.Count == 1)
                        {
                            channelInfo.CertificateThumbprint = channel.Thumbprint;
                        }
                    }
                    yield return channelInfo;
                }
            }
        }

        #endregion ListChannelInfos method

        #region GetChannel method

        //  -----------------
        //  GetChannel method
        //  -----------------

        IChannel IModel.GetChannel(DBContext db, Guid channelId)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));

            var query = from c in db.ApnsChannels
                        where c.ChannelId == channelId
                        select c;
            return query.FirstOrDefault();
        }

        #endregion GetChannel method

        #region FindChannel method

        //  ----------------------
        //  FindChannel method
        //  ----------------------

        IChannel IModel.FindChannel(DBContext db, ChannelKey channelKey)
        {
            return FindChannel(db, channelKey as ApnsChannelKey, false);
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

            var apnsChannelKey = channelKey as ApnsChannelKey;
            var channel = FindChannel(db, apnsChannelKey, true);
            if (channel == null)
            {
                channel = ApnsChannel.NewChannel(apnsChannelKey);
                db.ApnsChannels.Add(channel);
                return db.SaveChanges() > 0 ? channel : null;
            }
            else if (channel.Deleted != 0)
            {
                db.Entry(channel).Reference(c => c.Channel).Load();
                channel.Channel.ChannelType = channelKey.ChannelType;
                channel.Channel.Description = null;
                channel.Certificate = null;
                channel.Thumbprint = null;
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
            UpdateChannel(channel as ApnsChannel, channelInfo as ApnsChannelInfo);
        }

        private static void UpdateChannel(ApnsChannel channel, ApnsChannelInfo channelInfo)
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

            var apnsReceiverKey = receiverKey as ApnsReceiverKey;
            Debug.Assert(apnsReceiverKey != null);
            var receiver = ApnsReceiver.NewReceiver(channel.Channel, apnsReceiverKey.DeviceToken);
            db.ApnsReceivers.Add(receiver);
            return receiver;
        }

        #endregion CreateReceiver method

        #region ListReceivers method

        //  --------------------
        //  ListReceivers method
        //  --------------------

        IEnumerable<IReceiverInfo> IModel.ListReceivers(DBContext db, IChannel channel, DateTime? firstRegistration, bool includeExpired)
        {
            var query = from r in db.ApnsReceivers
                        where r.Receiver.ChannelId == channel.Channel.ChannelId && r.Deleted == 0 && r.Receiver.Deleted == 0
                        select r;
            if (!includeExpired) query = query.Where(e => e.Expired == null);
            if (firstRegistration.HasValue) query = query.Where(e => e.Created >= firstRegistration);
            foreach (var device in query.Include(r => r.Receiver).OrderBy(d => d.Receiver.Created))
            {
                yield return new ApnsReceiverInfo()
                {
                    Base64DeviceToken = ApnsDeviceToken.FromHexString(device.DeviceToken).Base64String,
                    ReceiverId = device.ReceiverId,
                    Name = device.Receiver?.Name,
                    Account = device.Receiver?.Account,
                    Groups = device.Receiver?.Groups,
                    Info = device.Receiver?.Info,
                    FirstRegistration = DateTime.SpecifyKind(device.Created, DateTimeKind.Utc),
                    LastRegistration = DateTime.SpecifyKind(device.Changed ?? device.Created, DateTimeKind.Utc),
                    Expired = device.Expired.HasValue ? DateTime.SpecifyKind(device.Expired.Value, DateTimeKind.Utc) : (DateTime?)null
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

            var apnsReceiverKey = receiverKey as ApnsReceiverKey;
            return db.ApnsReceivers.Find(apnsReceiverKey.DeviceToken.ToString());
        }

        #endregion FindReceiver method

        #region GetReceiver method

        //  ------------------
        //  GetReceiver method
        //  ------------------

        IReceiver IModel.GetReceiver(DBContext db, Guid receiverId)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));

            var query = from r in db.ApnsReceivers
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
            return new ApnsPusher(channel as ApnsChannel, keepChannelOpenInterval);
        }

        #endregion CreatePusher method

        #endregion IModel implementation

        #region private methods

        //  ------------------
        //  FindChannel method
        //  ------------------

        private static ApnsChannel FindChannel(DBContext db, ApnsChannelKey channelKey, bool includeDeleted)
        {
            var channel = db.ApnsChannels.Find(channelKey.BundleId, channelKey.Environment);
            if (channel != null)
            {
                if (channel.Deleted != 0 && !includeDeleted) channel = null;
            }
            return channel;
        }

        //  -------------------
        //  FindReceiver method
        //  -------------------

        private static ApnsReceiver FindReceiver(DBContext db, ApnsDeviceToken deviceToken)
        {
            var query = from r in db.ApnsReceivers
                        where r.DeviceToken == deviceToken.HexString
                        select r;
            return query.FirstOrDefault();
        }

        #endregion private methods

        #region static methods

        #region ListChannels method

        //  -------------------
        //  ListChannels method
        //  -------------------

        internal static IEnumerable<ApnsChannel> ListChannels(Model model)
        {
            using (var db = model.CreateContext())
            {
                foreach (var channel in db.ApnsChannels) { yield return channel; }
            }
        }

        #endregion ListChannels method

        #region CreateConfiguration method

        //  --------------------------
        //  CreateConfiguration method
        //  --------------------------

        internal static ApnsConfiguration CreateConfiguration(ApnsChannel channel)
        {
            if (string.IsNullOrWhiteSpace(channel.Thumbprint)) return null;

            using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
                var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, channel.Thumbprint, false);
                if (certificates.Count == 1)
                {
                    var certificate = certificates[0];
                    return new ApnsConfiguration(channel.Environment == Environment.Production ?
                        ApnsConfiguration.ApnsServerEnvironment.Production :
                        ApnsConfiguration.ApnsServerEnvironment.Sandbox,
                        certificate);
                }
                else return null;
            }
        }

        #endregion CreateConfiguration method

        #region SaveNotification method

        //  -----------------------
        //  SaveNotification method
        //  -----------------------

        internal static Guid SaveNotification(OperationResult result, Model model, ApnsDeviceToken deviceToken, string payload)
        {
            return model.UsingContext(db =>
            {
                var receiver = FindReceiver(db, deviceToken);
                if (receiver != null)
                {
                    return Model.SaveNotification(result, db, receiver.ReceiverId, payload);
                }
                throw new RouterException(string.Format(CultureInfo.CurrentCulture, ApnsStrings.ReceiverNotRegistered, deviceToken));
            });
        }

        #endregion SaveNotification method

        #region MarkReceiverAsExpired method

        //  ----------------------------
        //  MarkReceiverAsExpired method
        //  ----------------------------

        internal static void MarkReceiverAsExpired(Model model, ApnsDeviceToken deviceToken, DateTime timestamp)
        {
            model.UsingContext(db =>
            {
                var receiver = FindReceiver(db, deviceToken);
                if (receiver != null)
                {
                    receiver.Expired = timestamp.ToUniversalTime();
                    db.SaveChanges();
                }
            });
        }

        #endregion MarkReceiverAsExpired method

        #region SaveCertificate method

        //  ----------------------
        //  SaveCertificate method
        //  ----------------------

        internal static Guid? SaveCertificate(Model model, ChannelKey channelKey, byte[] certificateFileData)
        {
            return model.ChangeChannel(channelKey, (db, channel) =>
            {
                var apnsChannel = channel as ApnsChannel;
                apnsChannel.Certificate = Convert.ToBase64String(certificateFileData);
                apnsChannel.Thumbprint = null;

            });
        }

        #endregion SaveCertificate method

        #region InstallCertificate method

        //  -------------------------
        //  InstallCertificate method
        //  -------------------------

        internal static Guid? InstallCertificate(Model model, OperationResult result, ChannelKey channelKey, string password)
        {
            var channelId = model.ChangeChannel(channelKey, (db, channel) =>
            {
                var apnsChannel = channel as ApnsChannel;
                using (var certificate = new X509Certificate2(
                    Convert.FromBase64String(apnsChannel.Certificate), password,
                    X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet))
                {
                    using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
                    {
                        store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadWrite);
                        store.Add(certificate);
                    }
                    apnsChannel.Thumbprint = certificate.Thumbprint;
                    apnsChannel.Changed = DateTime.UtcNow; // force update for equal thumbprints
                    result.ReportSuccess(ApnsStrings.CertificateInstalled);
                }
            });
            return channelId;
        }

        #endregion InstallCertificate method

        #region UninstallCertificate method

        //  ---------------------------
        //  UninstallCertificate method
        //  ---------------------------

        internal static Guid? UninstallCertificate(Model model, OperationResult result, ChannelKey channelKey)
        {
            var channelid = model.ChangeChannel(channelKey, (db, channel) =>
            {
                var apnsChannel = channel as ApnsChannel;
                if (db.ApnsChannels.Count(c => c.Thumbprint == apnsChannel.Thumbprint) == 1)
                {
                    using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
                    {
                        store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadWrite);
                        var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, apnsChannel.Thumbprint, false);
                        if (certificates.Count == 1)
                        {
                            var certificate = certificates[0];
                            store.Remove(certificate);
                            result.ReportSuccess(ApnsStrings.CertificateUninstalled);
                        }
                    }
                }
                apnsChannel.Thumbprint = null;
            });
            return channelid;
        }

        #endregion UninstallCertificate method

        #region DeleteCertificate method

        //  ------------------------
        //  DeleteCertificate method
        //  ------------------------

        internal static Guid? DeleteCertificate(Model model, OperationResult result, ChannelKey channelKey)
        {
            var channelId = model.ChangeChannel(channelKey, (db, channel) =>
            {
                var apnsChannel = channel as ApnsChannel;
                if (!string.IsNullOrWhiteSpace(apnsChannel.Thumbprint))
                {
                    using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
                    {
                        store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadWrite);
                        var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, apnsChannel.Thumbprint, false);
                        if (certificates.Count == 1)
                        {
                            var certificate = certificates[0];
                            store.Remove(certificate);
                        }
                    }
                    apnsChannel.Thumbprint = null;
                }
                apnsChannel.Certificate = null;
            });
            if (channelId.HasValue)
            {
                result.ReportSuccess(ApnsStrings.CertificateDeleted);
            }
            return channelId;
        }

        #endregion DeleteCertificate method

        #endregion static methods
    }
}

// eof "ApnsModel.cs"
