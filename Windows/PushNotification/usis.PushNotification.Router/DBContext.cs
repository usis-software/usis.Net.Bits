//
//  @(#) DBContext.cs
//
//  Project:    usis Push Notification Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2010-2017 usis GmbH. All rights reserved.

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using usis.Framework.Data.Entity;
using usis.Platform.Data;

namespace usis.PushNotification
{
    //  ---------------
    //  DBContext class
    //  ---------------

    /// <summary>
    /// Defines the database structure for the push notification router
    /// </summary>
    /// <seealso cref="DBContextBase" />

    public class DBContext : DBContextBase
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="DBContext"/> class
        /// with the specified data source.
        /// </summary>
        /// <param name="dataSource">The data source.</param>

        public DBContext(DataSource dataSource) : base(dataSource) { }

        #endregion construction

        #region overrides

        //  ----------------------
        //  OnModelCreating method
        //  ----------------------

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //if (Database.Connection.GetType().FullName.Equals("System.Data.SQLite.SQLiteConnection", StringComparison.Ordinal))
            //{
            //    var sqliteConnectionInitializer = new SQLite.CodeFirst.SqliteDropCreateDatabaseWhenModelChanges<DBContext>(modelBuilder);
            //    Database.SetInitializer(sqliteConnectionInitializer);
            //}
            //else base.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        #endregion overrides

        #region properties

        //  -----------------
        //  Channels property
        //  -----------------

        /// <summary>
        /// Gets or sets the set of base channel entities.
        /// </summary>
        /// <value>
        /// The set of base channel entities.
        /// </value>

        public DbSet<Channel> Channels { get; set; }

        //  ---------------------
        //  ApnsChannels property
        //  ---------------------

        /// <summary>
        /// Gets or sets the set of APNs channel entities.
        /// </summary>
        /// <value>
        /// The set of APNs channel entities.
        /// </value>

        public DbSet<ApnsChannel> ApnsChannels { get; set; }

        //  --------------------
        //  GcmChannels property
        //  --------------------

        /// <summary>
        /// Gets or sets the set of GCM channel entities.
        /// </summary>
        /// <value>
        /// The set of GCM channel entities.
        /// </value>

        public DbSet<GcmChannel> GcmChannels { get; set; }

        //  --------------------
        //  WnsChannels property
        //  --------------------

        /// <summary>
        /// Gets or sets the set of WNS channel entities.
        /// </summary>
        /// <value>
        /// The set of WNS channel entities.
        /// </value>

        public DbSet<WnsChannel> WnsChannels { get; set; }

        //  ------------------
        //  Receivers property
        //  ------------------

        /// <summary>
        /// Gets or sets the set of base receiver entities.
        /// </summary>
        /// <value>
        /// The set of base receiver entities.
        /// </value>

        public DbSet<Receiver> Receivers { get; set; }

        //  ----------------------
        //  ApnsReceivers property
        //  ----------------------

        /// <summary>
        /// Gets or sets the set of APNs channel entities.
        /// </summary>
        /// <value>
        /// The set of APNs channel entities.
        /// </value>

        public DbSet<ApnsReceiver> ApnsReceivers { get; set; }

        //  ---------------------
        //  GcmReceivers property
        //  ---------------------

        /// <summary>
        /// Gets or sets the set of GCM channel entities.
        /// </summary>
        /// <value>
        /// The set of GCM channel entities.
        /// </value>

        public DbSet<GcmReceiver> GcmReceivers { get; set; }

        //  ---------------------
        //  WnsReceivers property
        //  ---------------------

        /// <summary>
        /// Gets or sets the set of WNS channel entities.
        /// </summary>
        /// <value>
        /// The set of WNS channel entities.
        /// </value>

        public DbSet<WnsReceiver> WnsReceivers { get; set; }

        //  ----------------------
        //  Notifications property
        //  ----------------------

        /// <summary>
        /// Gets or sets the set of notification entities.
        /// </summary>
        /// <value>
        /// The set of notification entities.
        /// </value>

        public DbSet<Notification> Notifications { get; set; }

        #endregion properties
    }

    #region Channel class

    //  -------------
    //  Channel class
    //  -------------

    /// <summary>
    /// Represents a channel base entity.
    /// </summary>
    /// <seealso cref="EntityBase" />

    [Table(nameof(Channel))]
    public class Channel : EntityBase
    {
        #region properties

        //  ------------------
        //  ChannelId property
        //  ------------------

        /// <summary>
        /// Gets the unique channel identifier.
        /// </summary>
        /// <value>
        /// The unique channel identifier.
        /// </value>

        [Key]
        public Guid ChannelId { get; set; }

        //  -------------
        //  Type property
        //  -------------

        /// <summary>
        /// Gets or sets the type of the channel.
        /// </summary>
        /// <value>
        /// The type of the channel.
        /// </value>

        [Column("Type")]
        public ChannelType ChannelType { get; set; }

        //  --------------------
        //  Description property
        //  --------------------

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>

        public string Description { get; set; }

        //  --------------------
        //  Application property
        //  --------------------

        /// <summary>
        /// Gets or sets the application name.
        /// </summary>
        /// <value>
        /// The application name.
        /// </value>

        public string Application { get; set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Prevents a default instance of the <see cref="Channel" /> class from being created.
        /// </summary>

        private Channel() { }

        #endregion construction

        #region methods

        //  -----------------
        //  NewChannel method
        //  -----------------

        /// <summary>
        /// Creates a new channel of the specified type.
        /// </summary>
        /// <param name="type">The type of the channel.</param>
        /// <returns>
        /// A newly created channel base entity.
        /// </returns>

        internal static Channel NewChannel(ChannelType type)
        {
            return new Channel()
            {
                ChannelId = Guid.NewGuid(),
                ChannelType = type
            };
        }

        #endregion methods

        #region overrides

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture,
                 "Channel, ChannelType='{0}', ChannelId={1}, Description='{2}'",
                 ChannelType, ChannelId, Description);
        }

        #endregion overrides
    }

    #endregion Channel class

    #region ApnsChannel class

    //  -----------------
    //  ApnsChannel class
    //  -----------------

    /// <summary>
    /// Represents an APNs channel entity.
    /// </summary>
    /// <seealso cref="EntityBase" />
    /// <seealso cref="IChannel" />

    [Table(nameof(ApnsChannel))]
    public class ApnsChannel : EntityBase, IChannel
    {
        #region properties

        //  ------------------
        //  ChannelId property
        //  ------------------

        /// <summary>
        /// Gets or sets the unique channel identifier.
        /// </summary>
        /// <value>
        /// The unique channel identifier.
        /// </value>

        [Index(IsUnique = true)]
        public Guid ChannelId { get; set; }

        //  ----------------
        //  Channel property
        //  ----------------

        /// <summary>
        /// Gets the channel base entity.
        /// </summary>
        /// <value>
        /// The channel base entity.
        /// </value>

        public virtual Channel Channel { get; set; }

        //  -----------------
        //  BundleId property
        //  -----------------

        /// <summary>
        /// Gets or sets the bundle identifier.
        /// </summary>
        /// <value>
        /// The bundle identifier.
        /// </value>

        [Key]
        [Column(Order = 0)]
        public string BundleId { get; set; }

        //  --------------------
        //  Environment property
        //  --------------------

        /// <summary>
        /// Gets or sets the environment for push notifications.
        /// </summary>
        /// <value>
        /// The environment for push notifications.
        /// </value>

        [Key]
        [Column(Order = 1)]
        public Environment Environment { get; set; }

        //  --------------------
        //  Certificate property
        //  --------------------

        /// <summary>
        /// Gets or sets the certificate.
        /// </summary>
        /// <value>
        /// The certificate.
        /// </value>

        public string Certificate { get; set; }

        //  -------------------
        //  Thumbprint property
        //  -------------------

        /// <summary>
        /// Gets or sets the thumbprint of the certificate.
        /// </summary>
        /// <value>
        /// The thumbprint of the certificate.
        /// </value>

        public string Thumbprint { get; set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Prevents a default instance of the <see cref="ApnsChannel" /> class from being created.
        /// </summary>

        private ApnsChannel() { }

        #endregion construction

        #region methods

        //  -----------------
        //  NewChannel method
        //  -----------------

        /// <summary>
        /// Create a new APNs channel entity with the specified channel key.
        /// </summary>
        /// <param name="channelKey">The channel key.</param>
        /// <returns>
        /// A newly created APNs channel entity.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="channelKey" /> is a null reference.</exception>

        public static ApnsChannel NewChannel(ApnsChannelKey channelKey)
        {
            if (channelKey == null) throw new ArgumentNullException(nameof(channelKey));

            return new ApnsChannel()
            {
                BundleId = channelKey.BundleId,
                Environment = channelKey.Environment,
                Channel = Channel.NewChannel(channelKey.ChannelType)
            };
        }

        #endregion methods

        #region override

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture,
                "{0}, BundeId='{1}', Environment={2}",
                nameof(ApnsChannel), BundleId, Environment);
        }

        #endregion override
    }

    #endregion ApnsChannel class

    #region GcmChannel class

    //  ----------------
    //  GcmChannel class
    //  ----------------

    /// <summary>
    /// Represents a GCM channel entity.
    /// </summary>
    /// <seealso cref="EntityBase" />
    /// <seealso cref="IChannel" />

    [Table(nameof(GcmChannel))]
    public class GcmChannel : EntityBase, IChannel
    {
        #region properties

        //  ------------------
        //  ChannelId property
        //  ------------------

        /// <summary>
        /// Gets or sets the unique channel identifier.
        /// </summary>
        /// <value>
        /// The unique channel identifier.
        /// </value>

        [Index(IsUnique = true)]
        public Guid ChannelId { get; set; }

        //  ----------------
        //  Channel property
        //  ----------------

        /// <summary>
        /// Gets the base channel entity.
        /// </summary>
        /// <value>
        /// The base channel entity.
        /// </value>

        public virtual Channel Channel { get; set; }

        //  -----------------
        //  SenderId property
        //  -----------------

        /// <summary>
        /// Gets or sets the sender identifier.
        /// </summary>
        /// <value>
        /// The sender identifier.
        /// </value>

        [Key]
        public string SenderId { get; set; }

        //  ------------------
        //  ServerKey property
        //  ------------------

        /// <summary>
        /// Gets or sets the server key.
        /// </summary>
        /// <value>
        /// The server key.
        /// </value>

        public string ServerKey { get; set; }

        //  ----------------------
        //  ApplicationId property
        //  ----------------------

        /// <summary>
        /// Gets or sets the application identifier.
        /// </summary>
        /// <value>
        /// The application identifier.
        /// </value>

        public string ApplicationId { get; set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Prevents a default instance of the <see cref="GcmChannel" /> class from being created.
        /// </summary>

        private GcmChannel() { }

        #endregion construction

        #region methods

        //  -----------------
        //  NewChannel method
        //  -----------------

        /// <summary>
        /// Creates a new GCM channel entity with the specified key.
        /// </summary>
        /// <param name="channelKey">The channel key.</param>
        /// <returns>
        /// A newly created GCM channel entity.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="channelKey"/> is a null reference.
        /// </exception>

        public static GcmChannel NewChannel(GcmChannelKey channelKey)
        {
            if (channelKey == null) throw new ArgumentNullException(nameof(channelKey));

            return new GcmChannel()
            {
                SenderId = channelKey.SenderId,
                Channel = Channel.NewChannel(channelKey.ChannelType)
            };
        }

        #endregion methods

        #region override

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}, SenderId='{1}'", nameof(GcmChannel), SenderId);
        }

        #endregion override
    }

    #endregion GcmChannel class

    #region WnsChannel class

    //  ----------------
    //  WnsChannel class
    //  ----------------

    /// <summary>
    /// Represents a WNS channel entity.
    /// </summary>
    /// <seealso cref="EntityBase" />
    /// <seealso cref="IChannel" />

    [Table(nameof(WnsChannel))]
    public class WnsChannel : EntityBase, IChannel
    {
        #region properties

        //  ------------------
        //  ChannelId property
        //  ------------------

        /// <summary>
        /// Gets or sets the unique channel identifier.
        /// </summary>
        /// <value>
        /// The unique channel identifier.
        /// </value>

        [Index(IsUnique = true)]
        public Guid ChannelId { get; set; }

        //  ----------------
        //  Channel property
        //  ----------------

        /// <summary>
        /// Gets the channel base entity.
        /// </summary>
        /// <value>
        /// The channel base entity.
        /// </value>

        public virtual Channel Channel { get; set; }

        //  -------------------
        //  PackageSid property
        //  -------------------

        /// <summary>
        /// Gets or sets the package security identifier.
        /// </summary>
        /// <value>
        /// The package security identifier.
        /// </value>

        [Key]
        public string PackageSid { get; set; }

        //  --------------------
        //  PackageName property
        //  --------------------

        /// <summary>
        /// Gets or sets the name of the package.
        /// </summary>
        /// <value>
        /// The name of the package.
        /// </value>

        public string PackageName { get; set; }

        //  ---------------------
        //  ClientSecret property
        //  ---------------------

        /// <summary>
        /// Gets or sets the secet key that is used to authenticate against WNS.
        /// </summary>
        /// <value>
        /// The secet key that is used to authenticate against WNS.
        /// </value>

        public string ClientSecret { get; set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Prevents a default instance of the <see cref="WnsChannel" /> class from being created.
        /// </summary>

        private WnsChannel() { }

        #endregion construction

        #region methods

        //  -----------------
        //  NewChannel method
        //  -----------------

        /// <summary>
        /// Creates a new channel entity
        /// with the specified channel key.
        /// </summary>
        /// <param name="channelKey">The channel key.</param>
        /// <returns>
        /// A newly created channel entity.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="channelKey" /> is a null reference.</exception>

        public static WnsChannel NewChannel(WnsChannelKey channelKey)
        {
            if (channelKey == null) throw new ArgumentNullException(nameof(channelKey));
            return new WnsChannel()
            {
                PackageSid = channelKey.PackageSid,
                Channel = Channel.NewChannel(channelKey.ChannelType)
            };
        }

        #endregion methods

        #region override

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}, PackageSid='{1}'", nameof(WnsChannel), PackageSid);
        }

        #endregion override
    }

    #endregion WnsChannel class

    #region Receiver class

    //  --------------
    //  Receiver class
    //  --------------

    /// <summary>
    /// Represents a receiver base entity.
    /// </summary>
    /// <seealso cref="EntityBase" />

    [Table(nameof(Receiver))]
    public class Receiver : EntityBase
    {
        #region properties

        //  -------------------
        //  ReceiverId property
        //  -------------------

        /// <summary>
        /// Gets the unique receiver identifier.
        /// </summary>
        /// <value>
        /// The unique receiver identifier.
        /// </value>

        [Key]
        public Guid ReceiverId { get; set; }

        //  ------------------
        //  ChannelId property
        //  ------------------

        /// <summary>
        /// Gets or sets the channel identifier.
        /// </summary>
        /// <value>
        /// The channel identifier.
        /// </value>

        public Guid ChannelId { get; set; }

        //  ----------------
        //  Channel property
        //  ----------------

        /// <summary>
        /// Gets or sets the channel base entity.
        /// </summary>
        /// <value>
        /// The channel base entity.
        /// </value>

        public virtual Channel Channel { get; set; }

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets or sets the name of the receiver.
        /// </summary>
        /// <value>
        /// The name of the receiver.
        /// </value>

        public string Name { get; set; }

        //  ----------------
        //  Account property
        //  ----------------

        /// <summary>
        /// Gets or sets the user account.
        /// </summary>
        /// <value>
        /// The user account.
        /// </value>

        public string Account { get; set; }

        //  ---------------
        //  Groups property
        //  ---------------

        /// <summary>
        /// Gets or sets the groups that the account belongs to.
        /// </summary>
        /// <value>
        /// The groups that the account belongs to.
        /// </value>

        public string Groups { get; set; }

        //  -------------
        //  Info property
        //  -------------

        /// <summary>
        /// Gets or sets additional information about the receiver.
        /// </summary>
        /// <value>
        /// Additional information about the receiver.
        /// </value>

        public string Info { get; set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Prevents a default instance of the <see cref="Receiver"/> class from being created.
        /// </summary>

        private Receiver() { }

        #endregion construction

        #region methods

        //  ------------------
        //  NewReceiver method
        //  ------------------

        /// <summary>
        /// Creates a new receiver base entity
        /// for the specified channel.
        /// </summary>
        /// <param name="channel">The channel that the receiver belongs to.</param>
        /// <returns>
        /// A newly created receiver base entity.
        /// </returns>

        internal static Receiver NewReceiver(Channel channel)
        {
            return new Receiver()
            {
                Channel = channel,
                ReceiverId = Guid.NewGuid()
            };
        }

        #endregion methods

        #region overrides

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "Receiver, Name='{0}'", Name);
        }

        #endregion overrides
    }

    #endregion Receiver class

    #region ApnsReceiver class

    //  ------------------
    //  ApnsReceiver class
    //  ------------------

    /// <summary>
    /// Represents a APNs receiver entity.
    /// </summary>
    /// <seealso cref="EntityBase" />
    /// <seealso cref="IReceiver" />

    [Table(nameof(ApnsReceiver))]
    public class ApnsReceiver : EntityBase, IReceiver
    {
        #region properties

        //  --------------------
        //  DeviceToken property
        //  --------------------

        /// <summary>
        /// Gets or sets the device token.
        /// </summary>
        /// <value>
        /// The device token.
        /// </value>

        [Key]
        [StringLength(64)]
        public string DeviceToken { get; set; }

        //  -------------------
        //  ReceiverId property
        //  -------------------

        /// <summary>
        /// Gets or sets the unique receiver identifier.
        /// </summary>
        /// <value>
        /// The unique receiver identifier.
        /// </value>

        [Index(IsUnique = true)]
        public Guid ReceiverId { get; set; }

        //  -----------------
        //  Receiver property
        //  -----------------

        /// <summary>
        /// Gets or sets the receiver base entity.
        /// </summary>
        /// <value>
        /// The receiver base entity.
        /// </value>

        public virtual Receiver Receiver { get; set; }

        //  ----------------
        //  Expired property
        //  ----------------

        /// <summary>
        /// Gets or sets the date and time when the device registration expired.
        /// </summary>
        /// <value>
        /// The date and time when the device registration expired.
        /// </value>

        public DateTime? Expired { get; set; }

        #endregion properties

        #region methods

        //  ------------------
        //  NewReceiver method
        //  ------------------

        /// <summary>
        /// Creates a new receiver entity for the specified channel
        /// with the specified device token.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="deviceToken">The device token.</param>
        /// <returns>
        /// A newly created receiver entity.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="deviceToken"/> is a null reference.
        /// </exception>

        public static ApnsReceiver NewReceiver(Channel channel, ApnsDeviceToken deviceToken)
        {
            if (deviceToken == null) throw new ArgumentNullException(nameof(deviceToken));

            return new ApnsReceiver()
            {
                Receiver = Receiver.NewReceiver(channel),
                DeviceToken = deviceToken.ToString()
            };
        }

        #endregion methods

        #region overrides

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}, DeviceToken='{1}'", nameof(ApnsReceiver), DeviceToken);
        }

        #endregion overrides
    }

    #endregion ApnsReceiver class

    #region GcmReceiver class

    //  -----------------
    //  GcmReceiver class
    //  -----------------

    /// <summary>
    /// Represents a GCM receiver entity.
    /// </summary>
    /// <seealso cref="EntityBase" />
    /// <seealso cref="IReceiver" />

    [Table(nameof(GcmReceiver))]
    public class GcmReceiver : EntityBase, IReceiver
    {
        #region properties

        //  --------------------------
        //  RegistrationToken property
        //  --------------------------

        /// <summary>
        /// Gets or sets the receiver's registration token.
        /// </summary>
        /// <value>
        /// The registration token.
        /// </value>

        [Key]
        [StringLength(128)]
        public string RegistrationToken { get; set; }

        //  -------------------
        //  ReceiverId property
        //  -------------------

        /// <summary>
        /// Gets or sets the receiver identifier.
        /// </summary>
        /// <value>
        /// The receiver identifier.
        /// </value>

        [Index(IsUnique = true)]
        public Guid ReceiverId { get; set; }

        //  -----------------
        //  Receiver property
        //  -----------------

        /// <summary>
        /// Gets or sets the receiver entity.
        /// </summary>
        /// <value>
        /// The receiver entity.
        /// </value>

        public virtual Receiver Receiver { get; set; }

        #endregion properties

        #region methods

        //  ------------------
        //  NewReceiver method
        //  ------------------

        /// <summary>
        /// Creates a new receiver entity for the specified channel
        /// with the specified device identifier.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="registrationToken">The registration token.</param>
        /// <returns>
        /// A newly created receiver entity.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="registrationToken" /> is a null reference.</exception>

        public static GcmReceiver NewReceiver(Channel channel, string registrationToken)
        {
            return new GcmReceiver()
            {
                Receiver = Receiver.NewReceiver(channel),
                RegistrationToken = registrationToken,
            };
        }

        #endregion methods
    }

    #endregion GcmReceiver class

    #region WnsReceiver class

    //  -----------------
    //  WnsReceiver class
    //  -----------------

    /// <summary>
    /// Represents a WNS receiver entity.
    /// </summary>
    /// <seealso cref="EntityBase" />
    /// <seealso cref="IReceiver" />

    [Table(nameof(WnsReceiver))]
    public class WnsReceiver : EntityBase, IReceiver
    {
        #region properties

        //  -------------------------
        //  DeviceIdentifier property
        //  -------------------------

        /// <summary>
        /// Gets or sets the device identifier.
        /// </summary>
        /// <value>
        /// The device identifier.
        /// </value>

        [Key]
        [StringLength(128)]
        public string DeviceIdentifier { get; set; }

        //  -------------------
        //  ReceiverId property
        //  -------------------

        /// <summary>
        /// Gets or sets the unique receiver identifier.
        /// </summary>
        /// <value>
        /// The unique receiver identifier.
        /// </value>

        [Index(IsUnique = true)]
        public Guid ReceiverId { get; set; }

        //  -----------------
        //  Receiver property
        //  -----------------

        /// <summary>
        /// Gets or sets the receiver base entity.
        /// </summary>
        /// <value>
        /// The receiver base entity.
        /// </value>

        public virtual Receiver Receiver { get; set; }

        //  -------------------
        //  ChannelUri property
        //  -------------------

        /// <summary>
        /// Gets or sets the channel URI.
        /// </summary>
        /// <value>
        /// The channel URI.
        /// </value>

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        public string ChannelUri { get; set; }

        #endregion properties

        #region methods

        //  ------------------
        //  NewReceiver method
        //  ------------------

        /// <summary>
        /// Creates a new receiver entity for the specified channel
        /// with the specified device identifier.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="deviceIdentifier">The device identifier.</param>
        /// <returns>
        /// A newly created receiver entity.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="deviceIdentifier"/> is a null reference.
        /// </exception>

        public static WnsReceiver NewReceiver(Channel channel, string deviceIdentifier)
        {
            return new WnsReceiver()
            {
                Receiver = Receiver.NewReceiver(channel),
                DeviceIdentifier = deviceIdentifier,
            };
        }

        #endregion methods
    }

    #endregion WnsReceiver class

    #region Notification class

    //  ------------------
    //  Notification class
    //  ------------------

    /// <summary>
    /// Represents a push notification entity.
    /// </summary>

    [Table(nameof(Notification))]
    public class Notification
    {
        #region properties

        //  -----------------------
        //  NotificationId property
        //  -----------------------

        /// <summary>
        /// Gets the unique notification identifier.
        /// </summary>
        /// <value>
        /// The unique notification identifier.
        /// </value>

        [Key]
        public Guid NotificationId { get; set; }

        //  -------------------
        //  ReceiverId property
        //  -------------------

        /// <summary>
        /// Gets or sets the receiver identifier.
        /// </summary>
        /// <value>
        /// The receiver identifier.
        /// </value>

        public Guid? ReceiverId { get; set; }

        //  ----------------
        //  Payload property
        //  ----------------

        /// <summary>
        /// Gets or sets the payload.
        /// </summary>
        /// <value>
        /// The payload.
        /// </value>

        public string Payload { get; set; }

        //  ---------------
        //  Queued property
        //  ---------------

        /// <summary>
        /// Gets the date and time when the notification was first saved.
        /// </summary>
        /// <value>
        /// The date and time when the notification was first saved.
        /// </value>

        public DateTime Queued { get; set; }

        //  -------------
        //  Sent property
        //  -------------

        /// <summary>
        /// Gets or sets the date and time when the notifiaction was sent to a push notification service.
        /// </summary>
        /// <value>
        /// The the date and time when the notifiaction was sent to a push notification service.
        /// </value>

        public DateTime? Pushed { get; set; }

        //  --------------
        //  State property
        //  --------------

        /// <summary>
        /// Gets or sets the state of the notification.
        /// </summary>
        /// <value>
        /// The state of the notification.
        /// </value>

        public NotificationState State { get; set; }

        #endregion properties

        #region methods

        //  ----------------------
        //  NewNotification method
        //  ----------------------

        /// <summary>
        /// Creates a new notification entity.
        /// </summary>
        /// <param name="receiverId">The receiver identifier.</param>
        /// <param name="payload">The payload.</param>
        /// <returns>A newly created notification entity.</returns>

        internal static Notification NewNotification(Guid receiverId, string payload)
        {
            return new Notification()
            {
                NotificationId = Guid.NewGuid(),
                ReceiverId = receiverId,
                Payload = payload,
                Queued = DateTime.UtcNow
            };
        }

        #endregion methods

        #region override

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "NotificationId={0}, Queued={1}", NotificationId, Queued);
        }

        #endregion override
    }

    #endregion Notification class
}

// eof "DBContext.cs"
