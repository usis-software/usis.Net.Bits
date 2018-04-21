//
//  @(#) EntityBase.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;

namespace usis.Framework
{
    //  ----------------
    //  EntityBase class
    //  ----------------

    /// <summary>
    /// Provides a base class for entities with common properties.
    /// </summary>

    public abstract class EntityBase : IEntityBase
    {
        #region constructor

        //  -----------
        //  constructor
        //  -----------

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBase"/> class.
        /// </summary>

        protected EntityBase()
        {
            Created = DateTime.UtcNow;
        }

        #endregion constructor

        #region properties

        //  ----------------
        //  Created property
        //  ----------------

        /// <summary>
        /// Gets or sets the date when the entity was created.
        /// </summary>
        /// <value>
        /// The date when the entity was created.
        /// </value>

        public DateTime Created { get; set; }

        //  ----------------
        //  Changed property
        //  ----------------

        /// <summary>
        /// Gets or sets the date when the entity was last changed.
        /// </summary>
        /// <value>
        /// The date when the entity was last changed.
        /// </value>

        public DateTime? Changed { get; set; }

        //  ----------------
        //  Deleted property
        //  ----------------

        /// <summary>
        /// Gets or sets a value that indicates whether the entity is marked as deleted.
        /// </summary>
        /// <value>
        /// A value that indicates whether the entity is marked as deleted.
        /// </value>

        public byte Deleted { get; set; }

        #endregion properties
    }

    #region IEntityBase interface

    //  ---------------------
    //  IEntityBase interface
    //  ---------------------

    /// <summary>
    /// Defines common properties for a basic entity.
    /// </summary>

    public interface IEntityBase
    {
        //  ----------------
        //  Created property
        //  ----------------

        /// <summary>
        /// Gets or sets the date when the entity was created.
        /// </summary>
        /// <value>
        /// The date when the entity was created.
        /// </value>

        DateTime Created { get; set; }

        //  ----------------
        //  Changed property
        //  ----------------

        /// <summary>
        /// Gets or sets the date when the entity was last changed.
        /// </summary>
        /// <value>
        /// The date when the entity was last changed.
        /// </value>

        DateTime? Changed { get; set; }

        //  ----------------
        //  Deleted property
        //  ----------------

        /// <summary>
        /// Gets or sets a value that indicates whether the entity is marked as deleted.
        /// </summary>
        /// <value>
        /// A value that indicates whether the entity is marked as deleted.
        /// </value>

        byte Deleted { get; set; }
    }

    #endregion IEntityBase interface
}

// eof "EntityBase.cs"
