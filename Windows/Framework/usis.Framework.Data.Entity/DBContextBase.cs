//
//  @(#) DBContextBase.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.Data.Common;
using System.Data.Entity;
using usis.Platform.Data;

namespace usis.Framework.Data.Entity
{
    //  -------------------
    //  DBContextBase class
    //  -------------------

    /// <summary>
    /// Provides a base class for implementations of database contexts
    /// that support the updating of <see cref="EntityBase"/> derived class
    /// on a <see cref="SaveChanges"/> method call.
    /// </summary>
    /// <seealso cref="DbContext" />

    public abstract class DBContextBase : DbContext
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="DBContextBase"/> class.
        /// </summary>

        protected DBContextBase() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DBContextBase"/> class.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>

        protected DBContextBase(string nameOrConnectionString) : base(nameOrConnectionString) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DBContextBase" /> class.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="contextOwnsConnection">If set to <c>true</c> the connection is disposed when the context is disposed, otherwise the caller must dispose the connection.</param>

        protected DBContextBase(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DBContextBase"/> class
        /// with the specified data source.
        /// </summary>
        /// <param name="dataSource">The data source.</param>

        protected DBContextBase(DataSource dataSource) : base(dataSource?.ConnectionString) { }

        #endregion construction

        #region overrides

        //  ------------------
        //  SaveChanges method
        //  ------------------

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        /// The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).
        /// </returns>

        public override int SaveChanges()
        {
            foreach (var item in ChangeTracker.Entries<IEntityBase>())
            {
                if (item.State == EntityState.Modified)
                {
                    if (!item.Property(nameof(EntityBase.Changed)).IsModified)
                    {
                        item.Entity.Changed = DateTime.UtcNow;
                    }
                    //if (!item.Property(nameof(EntityBase.Changed)).IsModified)
                    //{
                    //    item.Entity.UpdateCount++;
                    //}
                }
            }
            return base.SaveChanges();
        }

        #endregion overrides
    }
}

// eof "DBContextBase.cs"
