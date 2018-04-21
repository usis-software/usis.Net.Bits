//
//  @(#) DBContext.cs
//
//  Project:    IZYTRON.IQ.SyncSvc
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 audius GmbH. All rights reserved.

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using usis.Framework.Entity;

namespace IZYTRON.IQ
{
    //  ---------------
    //  DBContext class
    //  ---------------

    public class DBContext : DBContextBase
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal DBContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        #endregion construction

        #region properties

        //  ----------------
        //  Clients property
        //  ----------------

        public DbSet<Client> Clients { get; set; }

        #endregion properties
    }

    #region Client class

    //  ------------
    //  Client class
    //  ------------

    [Table(nameof(Client))]
    public class Client : EntityBase
    {
        #region properties

        [Key]
        public Guid Id { get; set; }

        public SyncState State { get; set; }

        #endregion properties

        #region methods

        //  ----------------
        //  NewClient method
        //  ----------------

        public static Client NewClient(Guid databaseId)
        {
            return new Client() { Id = databaseId, State = SyncState.ReadyToUpload };
        }

        #endregion methods
    }

    #endregion Client class
}

// eof "DBContext.cs"
