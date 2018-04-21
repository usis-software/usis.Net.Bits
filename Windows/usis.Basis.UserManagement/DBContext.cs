//
//  @(#) DBContext.cs
//
//  Project:    Basis - User Management
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using usis.Framework.Entity;
using usis.Platform.Data;

namespace usis.Basis.UserManagement
{
    //  ---------------
    //  DBContext class
    //  ---------------

    internal sealed class DBContext : DbContext
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal DBContext(DataSource dataSource) : base(dataSource.ConnectionString) { }

        #endregion construction

        #region properties

        //  -----------------
        //  Accounts property
        //  -----------------

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public DbSet<Account> Accounts { get; set; }

        #endregion properties
    }

    #region Account class

    //  -------------
    //  Account class
    //  -------------

    [Table(nameof(Account))]
    internal sealed class Account : EntityBase
    {
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [Key]
        public string Name { get; set; }
    }

    #endregion Account class
}

// eof "DBContext.cs"
