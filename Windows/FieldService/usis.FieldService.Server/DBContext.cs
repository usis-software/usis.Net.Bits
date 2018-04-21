//
//  @(#) DBContext.cs
//
//  Project:    usis.FieldService.Server
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using usis.Framework.Data.Entity;
using usis.Platform.Data;

namespace usis.FieldService.Server
{
    #region DBContext class

    //  ---------------
    //  DBContext class
    //  ---------------

    public sealed class DBContext : DBContextBase
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal DBContext(DataSource dataSource) : base(dataSource) { }

        #endregion construction

        #region properties

        //public DbSet<Client> Clients { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<WindowsUser> WindowsUsers { get; set; }

        //public DbSet<Contact> Contacts { get; set; }

        //public DbSet<WorkItem> WorkItems { get; set; }

        #endregion properties

        #region overrides

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<WindowsUser>().HasKey(e => new { e.UserId, e.Sid });
        //}

        #endregion overrides
    }

    #endregion DBContext class

    #region Client class

    //  ------------
    //  Client class
    //  ------------

    [Table(nameof(Client))]
    public sealed class Client : EntityBase
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(128, MinimumLength = 1)]
        public string Name { get; set; }
    }

    #endregion Client class

    #region User class

    //  ----------
    //  User class
    //  ----------

    [Table(nameof(User))]
    public class User : EntityBase
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public virtual Client Client { get; set; }

        [StringLength(128, MinimumLength = 1)]
        public string AccountName { get; set; }

        public Guid? ContactId { get; set; }

        public virtual Contact Contact { get; set; }
    }

    #endregion User class

    #region WindowsUser class

    //  -----------------
    //  WindowsUser class
    //  -----------------

    [Table(nameof(WindowsUser))]
    public class WindowsUser : EntityBase
    {
        [Key]
        [Column(Order = 0)]
        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        [Key]
        [Column(Order = 1)]
        public string Sid { get; set; }
    }

    #endregion WindowsUser class

    #region Contact class

    //  -------------
    //  Contact class
    //  -------------

    [Table(nameof(Contact))]
    public sealed class Contact : EntityBase
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(128, MinimumLength = 1)]
        public string DisplayName { get; set; }
    }

    #endregion Contact class

    //[Table(nameof(WorkItem))]
    //public class WorkItem : EntityBase
    //{
    //    [Key] 
    //    public Guid Id { get; set; }

    //    public Guid OwnerId { get; set; }

    //    public virtual Contact Owner { get; set; }

    //    public string Subject { get; set; }
    //}

    //[Table(nameof(Equipment))]
    //public class Equipment
    //{
    //    [Key]
    //    public Guid Id { get; set; }

    //    public string DisplayName { get; set; }
    //}
}

// eof "DBContext.cs"
