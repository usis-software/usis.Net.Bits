//
//  @(#) Model.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using usis.Framework;
using usis.Framework.Data.Entity;
using usis.Platform;

namespace usis.PushNotification
{
    //  -----------
    //  Model class
    //  -----------

    /// <summary>
    /// Provides methods to access and persist push notifiction router entities.
    /// </summary>
    /// <seealso cref="DBContextModel{DBContext}" />

    public sealed class Model : DBContextModel<DBContext>
    {
        #region fields

        private static Dictionary<ChannelType, IModel> models = new Dictionary<ChannelType, IModel>();

        #endregion fields

        #region RegisteredChannelTypes property

        //  -------------------------------
        //  RegisteredChannelTypes property
        //  -------------------------------

        /// <summary>
        /// Gets the registered channel types.
        /// </summary>
        /// <value>
        /// The registered channel types.
        /// </value>

        public static IEnumerable<ChannelType> RegisteredChannelTypes { get { lock (models) return models.Keys; } }

        #endregion RegisteredChannelTypes property

        #region RegisterPushServiceModel method

        //  -------------------------------
        //  RegisterPushServiceModel method
        //  -------------------------------

        /// <summary>
        /// Registers a push service model to handel the specified channel type.
        /// </summary>
        /// <param name="channelType">Type of the channel.</param>
        /// <param name="model">The model.</param>

        public static void RegisterPushServiceModel(ChannelType channelType, IModel model)
        {
            lock (models)
            {
                models[channelType] = model;
            }
        }

        #endregion RegisterPushServiceModel method

        #region overrides

        //  ---------------
        //  OnAttach method
        //  ---------------

        /// <summary>
        /// Called when the extension is added to the
        /// <see cref="IExtensibleObject{TObject}.Extensions" /> property.
        /// </summary>

        protected override void OnAttach()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DBContext, Migrations.Configuration>(true));
            base.OnAttach();
        }

        //  --------------
        //  OnStart method
        //  --------------

        /// <summary>
        /// Called after all snap-ins of an application are loaded and connected.
        /// </summary>

        protected override void OnStart()
        {
            Debug.WriteLine("Initializing database...");
            base.OnStart();
        }

        //  -------------------
        //  NewContext method
        //  -------------------

        /// <summary>
        /// Creates a new database context object.
        /// </summary>
        /// <returns>
        /// A newly created database context object.
        /// </returns>

        protected override DBContext NewContext()
        {
            return new DBContext(DataSource);
        }

        #endregion overrides

        #region RegisterReceiver method

        //  -----------------------
        //  RegisterReceiver method
        //  -----------------------

        /// <summary>
        /// Registers a push notification receiver (device).
        /// </summary>
        /// <param name="result">The operation result to return to the caller.</param>
        /// <param name="receiverKey">The receiver key.</param>
        /// <param name="name">The name of the receiver.</param>
        /// <param name="account">The user account of the receiver.</param>
        /// <param name="groups">The groups that the account belongs to.</param>
        /// <param name="info">Additional information about the receiver.</param>
        /// <returns>
        /// The receiver identifier of the registered receiver.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="result"/> or <paramref name="receiverKey"/> is a null reference.
        /// </exception>
        /// <exception cref="NotImplementedException">
        /// The specified channel type is not implemented yet.
        /// </exception>

        public Guid RegisterReceiver(
            OperationResult result, ReceiverKey receiverKey,
            string name, string account, string groups, string info)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            if (receiverKey == null) throw new ArgumentNullException(nameof(receiverKey));

            return UsingContext(db =>
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    var model = GetPushServiceModel(receiverKey.ChannelKey.ChannelType);
                    if (model == null) throw new NotImplementedException();

                    var receiver = model.FindReceiver(db, receiverKey);
                    if (receiver == null)
                    {
                        // get channel
                        var channel = model.FindOrCreateChannel(db, receiverKey.ChannelKey);
                        if (channel == null) throw new NotImplementedException();

                        // add new receiver
                        receiver = model.CreateReceiver(db, channel, receiverKey);
                        if (receiver == null) throw new NotImplementedException();

                        result.ReportSuccess(Strings.NewReceiverRegistered);
                    }
                    else
                    {
                        // return existing receiver
                        model.UpdateReceiver(receiver, receiverKey);
                        // force update of the Changed properties
                        receiver.Changed = DateTime.UtcNow;
                        receiver.Receiver.Changed = receiver.Changed;
                        result.ReportInformation(Strings.ReceiverAlreadyRegistered, 1);
                    }
                    receiver.Receiver.Name = name;
                    receiver.Receiver.Account = account;
                    receiver.Receiver.Groups = groups;
                    receiver.Receiver.Info = info;
                    db.SaveChanges();
                    transaction.Commit();

                    return receiver.Receiver.ReceiverId;
                }
            });
        }

        #endregion RegisterReceiver method

        #region channel management methods

        #region ListChannelInfos method

        //  -------------------
        //  ListChannels method
        //  -------------------

        /// <summary>
        /// Lists all channels of the specified type.
        /// </summary>
        /// <param name="channelType">Type of the channels.</param>
        /// <returns>
        /// An enumerator to iterate thru the channels of the specified type.
        /// </returns>

        public IEnumerable<IChannelInfo> ListChannels(ChannelType channelType)
        {
            using (var db = CreateContext())
            {
                var model = GetPushServiceModel(channelType);
                foreach (var channel in model.ListChannels(db)) { yield return channel; }
            }
        }

        #endregion ListChannelInfos method

        #region FindOrCreateChannel method

        //  --------------------------
        //  FindOrCreateChannel method
        //  --------------------------

        /// <summary>
        /// Creates a new channel with the specified key, if it does not already exists.
        /// </summary>
        /// <param name="channelKey">The channel key.</param>
        /// <exception cref="ArgumentNullException"><paramref name="channelKey"/> is a null reference.</exception>
        /// <exception cref="RouterException">
        /// The specified channel type is not implemented.
        /// </exception>

        public void FindOrCreateChannel(ChannelKey channelKey)
        {
            if (channelKey == null) throw new ArgumentNullException(nameof(channelKey));

            var model = GetPushServiceModel(channelKey.ChannelType);
            UsingContext(db =>
            {
                if (model.FindChannel(db, channelKey) != null)
                {
                    throw new RouterException(string.Format(CultureInfo.CurrentCulture, Strings.ChannelAlreadyExists, channelKey));
                }
                if (model.FindOrCreateChannel(db, channelKey) == null)
                {
                    throw new RouterException(string.Format(CultureInfo.CurrentCulture, Strings.FailedToCreateChannel, channelKey));
                }
            });
        }

        #endregion FindOrCreateChannel method

        #region UpdateChannel method

        //  --------------------
        //  UpdateChannel method
        //  --------------------

        internal Guid? UpdateChannel(IChannelInfo channelInfo)
        {
            var model = GetPushServiceModel(channelInfo.BaseKey.ChannelType);
            return ChangeChannel(channelInfo.BaseKey, (db, channel) =>
            {
                model.UpdateChannel(db, channel, channelInfo);
            });
        }

        #endregion UpdateChannel method

        #region DeleteChannel method

        //  --------------------
        //  DeleteChannel method
        //  --------------------

        internal Guid? DeleteChannel(ChannelKey channelKey)
        {
            var model = GetPushServiceModel(channelKey.ChannelType);
            if (model == null) throw new NotImplementedException();

            return UsingContext(db =>
            {
                var channel = model.FindChannel(db, channelKey);
                if (channel != null)
                {
                    // load related abstract channel object
                    db.Entry(channel).Reference(c => c.Channel).Load();

                    // increment deleted flags
                    channel.Deleted++;
                    channel.Channel.Deleted++;

                    return db.SaveChanges() > 0 ? channel.Channel.ChannelId : (Guid?)null;
                }
                else throw new RouterException(string.Format(CultureInfo.CurrentCulture, Strings.ChannelDoesNotExist, channelKey));
            });
        }

        #endregion DeleteChannel method

        #endregion channel management methods

        #region notification methods

        #region RetrieveNotificationContext method

        //  ----------------------------------
        //  RetrieveNotificationContext method
        //  ----------------------------------

        internal NotificationContext RetrieveNotificationContext(Guid id)
        {
            return UsingContext(db =>
            {
                var notification = db.Notifications.Find(id);
                if (notification != null &&
                    notification.State == NotificationState.Unsent &&
                    notification.ReceiverId.HasValue)
                {
                    var receiver = FindReceiver(db, notification.ReceiverId.Value);
                    if (receiver != null)
                    {
                        SaveNotificationState(db, id, NotificationState.Pending);
                        return new NotificationContext()
                        {
                            Notification = notification,
                            ChannelId = receiver.Receiver.ChannelId.NullIfEmpty(),
                            Receiver = receiver
                        };
                    }
                }
                return null;
            });
        }

        #endregion RetrieveNotificationContext method

        #region GetNotificationState method

        //  ---------------------------
        //  GetNotificationState method
        //  ---------------------------

        /// <summary>
        /// Gets the state of the notification specified.
        /// </summary>
        /// <param name="notificationId">The notification identifier.</param>
        /// <returns>
        /// The state of the notification.
        /// </returns>
        /// <exception cref="RouterException">A notification with the specified identifier does not exist.</exception>

        public NotificationState GetNotificationState(Guid notificationId)
        {
            return UsingContext(db =>
            {
                var notification = db.Notifications.Find(notificationId);
                if (notification == null)
                {
                    throw new RouterException(Strings.NotificationDoesNotExist);
                }
                else return notification.State;
            });
        }

        #endregion GetNotificationState method

        #region MarkNotificationAsFailed method

        //  -------------------------------
        //  MarkNotificationAsFailed method
        //  -------------------------------

        internal void MarkNotificationAsFailed(Guid id)
        {
            SaveNotificationState(id, NotificationState.Failed);
        }

        #endregion MarkNotificationAsFailed method

        #region UnsentNotifications method

        //  --------------------------
        //  UnsentNotifications method
        //  --------------------------

        internal IEnumerable<Guid> UnsentNotifications()
        {
            using (var db = CreateContext())
            {
                var query = from n in db.Notifications
                            where n.State == NotificationState.Unsent
                            orderby n.Queued
                            select n.NotificationId;
                foreach (var item in query) yield return item;
            }
        }

        #endregion UnsentNotifications method

        #endregion notification methods

        #region ListReceivers method

        //  --------------------
        //  ListReceivers method
        //  --------------------

        /// <summary>
        /// Lists all receivers (devices) registered for the specified channel.
        /// </summary>
        /// <param name="channelKey">The channel key.</param>
        /// <param name="firstRegistration">The date and time of the first registration.</param>
        /// <param name="includeExpired">if set to <c>true</c> the result should include expired devices.</param>
        /// <returns>
        /// An enumerator to iterate thru the receivers.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// The channel type specified by <paramref name="channelKey"/> is not implemented.
        /// </exception>

        public IEnumerable<IReceiverInfo> ListReceivers(ChannelKey channelKey, DateTime? firstRegistration, bool includeExpired)
        {
            using (var db = CreateContext())
            {
                var model = GetPushServiceModel(channelKey.ChannelType);
                if (model == null) throw new NotImplementedException();

                var channel = model.FindChannel(db, channelKey);
                if (channel == null) yield break;
                db.Entry(channel).Reference(c => c.Channel).Load();
                foreach (var receiverInfo in model.ListReceivers(db, channel, firstRegistration, includeExpired))
                {
                    yield return receiverInfo;
                }
            }
        }

        #endregion ListReceivers method

        #region GetChannel method

        //  -----------------
        //  GetChannel method
        //  -----------------

        internal IChannel GetChannel(Guid channelId)
        {
            return UsingContext(db =>
            {
                var channel = db.Channels.Find(channelId);
                if (channel != null)
                {
                    var model = GetPushServiceModel(channel.ChannelType);
                    if (model == null) throw new NotImplementedException();
                    return model.GetChannel(db, channelId);
                }
                return null;
            });
        }

        #endregion GetChannel method

        #region ChangeChannel method

        //  --------------------
        //  ChangeChannel method
        //  --------------------

        /// <summary>
        /// Calls an action to change the specified channel.
        /// </summary>
        /// <param name="channelKey">The channel key.</param>
        /// <param name="action">The action that changes to channel data.</param>
        /// <returns>
        /// The channel identifier of the changed channel,
        /// or <b>null</b> if the channel was not changed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="channelKey"/> or <paramref name="action"/> is a null reference.
        /// </exception>
        /// <exception cref="NotImplementedException">The specified channel has a channel type that is not implemented yet.</exception>
        /// <exception cref="RouterException">The specified channel does not exist.</exception>

        public Guid? ChangeChannel(ChannelKey channelKey, Action<DBContext, IChannel> action)
        {
            if (channelKey == null) throw new ArgumentNullException(nameof(channelKey));
            if (action == null) throw new ArgumentNullException(nameof(action));

            return UsingContext(db =>
            {
                var model = GetPushServiceModel(channelKey.ChannelType);
                if (model == null) throw new NotImplementedException();

                var channel = model.FindChannel(db, channelKey);
                if (channel != null)
                {
                    db.Entry(channel).Reference(c => c.Channel).Load();
                    action(db, channel);
                    return db.SaveChanges() > 0 ? channel.Channel.ChannelId : (Guid?)null;
                }
                else throw new RouterException(string.Format(CultureInfo.CurrentCulture, Strings.ChannelDoesNotExist, channelKey));
            });
        }

        #endregion ChangeChannel method

        #region CreatePusher method

        //  -------------------
        //  CreatePusher method
        //  -------------------

        internal static IPusher CreatePusher(IChannel channel, int keepChannelOpenInterval)
        {
            var model = GetPushServiceModel(channel.Channel.ChannelType);
            if (model == null) throw new NotImplementedException();
            return model.CreatePusher(channel, keepChannelOpenInterval);
        }

        #endregion CreatePusher method

        #region ArchiveNotifications method

        //  ---------------------------
        //  ArchiveNotifications method
        //  ---------------------------

        internal int ArchiveNotifications(NotificationState state, int age, string path)
        {
            var date = DateTime.UtcNow.Date.AddDays(-age);
            return UsingContext(db =>
            {
                var query = from notification in db.Notifications
                            where notification.State == state && notification.Queued < date
                            orderby notification.Queued descending
                            select notification;
                int count = query.Count();
                if (count > 0)
                {
                    // archive notifications
                    if (path != null) DataExport.ToCsv(query, path);

                    // delete notifications
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        foreach (var item in query) db.Notifications.Remove(item);
                        db.SaveChanges();
                        transaction.Commit();
                    }
                }
                return count;
            });
        }

        #endregion ArchiveNotifications method

        #region Export method

        //  -----------------
        //  ExportData method
        //  -----------------

        internal void ExportData(IJob job, string path)
        {
            var t = 1000;

            job?.SetProgressStep(1, 1, Strings.BackupPerforming);
            job?.UpdateProgress(0, 8, 0, Strings.BackupStarting);
            System.Threading.Thread.Sleep(t);

            UsingContext(db =>
            {
                using (var export = new DataExport(path))
                {
                    job?.UpdateProgress(1, Strings.BackupChannels);
                    export.Save(db.Channels);
                    System.Threading.Thread.Sleep(t);

                    job?.UpdateProgress(2, Strings.BackupApnsChannels);
                    export.Save(db.ApnsChannels);
                    System.Threading.Thread.Sleep(t);

                    job?.UpdateProgress(3, Strings.BackupWnsChannels);
                    export.Save(db.WnsChannels);
                    System.Threading.Thread.Sleep(t);

                    job?.UpdateProgress(4, Strings.BackupReceivers);
                    export.Save(db.Receivers);
                    System.Threading.Thread.Sleep(t);

                    job?.UpdateProgress(5, Strings.BackupApnsReceivers);
                    export.Save(db.ApnsReceivers);
                    System.Threading.Thread.Sleep(t);

                    job?.UpdateProgress(6, Strings.BackupWnsReceivers);
                    export.Save(db.WnsReceivers);
                    System.Threading.Thread.Sleep(t);

                    job?.UpdateProgress(7, Strings.BackupNotifications);
                    export.Save(db.Notifications);
                    System.Threading.Thread.Sleep(t);
                }
            });
            job?.UpdateProgress(8, Strings.BackupComplete);
            System.Threading.Thread.Sleep(t);
        }

        //  -----------------
        //  ImportData method
        //  -----------------

        internal void ImportData(string path)
        {
            var import = new DataImport(path);

            UsingContext(db =>
            {
                db.Channels.Clear();
                db.Receivers.Clear();
                db.Notifications.Clear();
                db.SaveChanges();

                import.Load(db.Channels);
                import.Load(db.ApnsChannels);
                import.Load(db.GcmChannels);
                import.Load(db.WnsChannels);

                import.Load(db.Receivers);
                import.Load(db.ApnsReceivers);
                import.Load(db.GcmReceivers);
                import.Load(db.WnsReceivers);

                import.Load(db.Notifications);

                db.SaveChanges();
            });
        }

        #endregion Export method

        #region private methods

        #region FindReceiver method

        //  -------------------
        //  FindReceiver method
        //  -------------------

        private static IReceiver FindReceiver(DBContext db, Guid receiverId)
        {
            var receiver = db.Receivers.Find(receiverId);
            if (receiver != null)
            {
                db.Entry(receiver).Reference(r => r.Channel).Load();
                var model = GetPushServiceModel(receiver.Channel.ChannelType);
                if (model == null) throw new NotImplementedException();
                return model.GetReceiver(db, receiverId);
            }
            return null;
        }

        #endregion FindReceiver method

        #region SaveNotification method

        //  -----------------------
        //  SaveNotification method
        //  -----------------------

        /// <summary>
        /// Saves the specified payload as a notification to be send to the receiver with the specified identifier.
        /// </summary>
        /// <param name="result">The result of the operation.</param>
        /// <param name="db">The database context.</param>
        /// <param name="receiverId">The receiver identifier.</param>
        /// <param name="payload">The payload to send.</param>
        /// <returns>
        /// The identifier of the saved notification.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="result" /> or <paramref name="db" /> is a null reference.</exception>
        /// <exception cref="RouterException">The notification was not saved.</exception>

        public static Guid SaveNotification(OperationResult result, DBContext db, Guid receiverId, string payload)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            if (db == null) throw new ArgumentNullException(nameof(db));

            var notification = Notification.NewNotification(receiverId, payload);
            db.Notifications.Add(notification);
            if (db.SaveChanges() > 0)
            {
                result.ReportInformation(Strings.NotificationSavedToSendQueue);
                return notification.NotificationId;
            }
            throw new RouterException(Strings.FailedToSaveNotification);
        }

        #endregion SaveNotification method

        #region SaveNotificationState method

        //  ----------------------------
        //  SaveNotificationState method
        //  ----------------------------

        internal void SaveNotificationState(Guid id, NotificationState state)
        {
            UsingContext(db => SaveNotificationState(db, id, state));
        }

        private static void SaveNotificationState(DBContext db, Guid id, NotificationState state)
        {
            var notification = db.Notifications.Find(id);
            if (notification != null)
            {
                notification.State = state;
                notification.Pushed = DateTime.UtcNow;
                db.SaveChanges();
            }
        }

        #endregion SaveNotificationState method

        #region GetPushServiceModel method

        //  --------------------------
        //  GetPushServiceModel method
        //  --------------------------

        private static IModel GetPushServiceModel(ChannelType channelType)
        {
            lock (models)
            {
                return models.TryGetValue(channelType, out IModel model) ? model : null;
            }
        }

        #endregion GetPushServiceModel method

        #endregion private methods
    }

    #region DbSetExtensions class

    //  ---------------------
    //  DbSetExtensions class
    //  ---------------------

    internal static class DbSetExtensions
    {
        //  ------------
        //  Clear method
        //  ------------

        internal static void Clear<T>(this DbSet<T> set) where T : class
        {
            set.RemoveRange(set);
        }
    }

    #endregion DbSetExtensions class
}

// eof "Model.cs"
