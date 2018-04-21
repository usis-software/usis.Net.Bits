//
//  @(#) DBContext.cs
//
//  Project:    usis.FinTS.Server
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using usis.Framework.Entity;

namespace usis.FinTS.Server.Database
{
    #region DBContext class

    //  ---------------
    //  DBContext class
    //  ---------------

    internal class DBContext : DBContextBase
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal DBContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        #endregion construction

        #region properties

        //  ----------------
        //  Dialogs property
        //  ----------------

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DbSet<Dialog> Dialogs { get; }

        //  -------------------
        //  FinTSBanks property
        //  -------------------

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DbSet<FinTSBank> FinTSBanks { get; }

        #endregion properties
    }

    #endregion DBContext class

    #region Dialog class

    //  ------------
    //  Dialog class
    //  ------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    [Table(nameof(Dialog))]
    internal class Dialog : EntityBase
    {
        [Key]
        public Guid Id { get; internal set; }
    }

    #endregion Dialog class

    #region FinTSBank class

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    [Table(nameof(FinTSBank))]
    internal class FinTSBank : EntityBase
    {
        [Key]
        public int Number { get; internal set; }

        [MaxLength(30)]
        public string BankCode { get; internal set; }
    }

    #endregion FinTSBank class
}

// eof "DBContext.cs"
