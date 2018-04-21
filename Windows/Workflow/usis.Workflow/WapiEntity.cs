//
//  @(#) WapiEntity.cs
//
//  Project:    usis Workflow Server
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;

namespace usis.Workflow
{
    //  ----------------
    //  WapiEntity class
    //  ----------------

    /// <summary>
    /// Provides a base class for a workflow entity.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TData">The type of the data.</typeparam>

    public abstract class WapiEntity<TId, TData> : IEquatable<WapiEntity<TId, TData>>
        where TId : WapiEntityId
        where TData : IWapiEntityData<TId>
    {
        #region construction

        //  -----------
        //  constructor
        //  -----------

        /// <summary>
        /// Initializes a new instance of the <see cref="WapiEntity{TId, TData}"/> class
        /// with a specified session object and the entity data.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="data">The data.</param>

        protected internal WapiEntity(Session session, TData data)
        {
            Session = session;
            Data = data;
        }

        #endregion construction

        #region properties

        //  ----------------
        //  Session property
        //  ----------------

        /// <summary>
        /// Gets the session object.
        /// </summary>
        /// <value>
        /// The session object.
        /// </value>

        protected Session Session { get; private set; }

        //  -------------
        //  Data property
        //  -------------

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>

        protected TData Data { get; set; }

        //  -----------
        //  Id property
        //  -----------

        /// <summary>
        /// Gets the unique identifier of the entity.
        /// </summary>
        /// <value>
        /// The unique identifier of the entity.
        /// </value>

        public TId Id { get { return Data.Id; } }

        #endregion properties

        #region IEquatable implementation

        //  -------------
        //  Equals method
        //  -------------

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>

        public override bool Equals(object obj)
        {
            var entity = obj as WapiEntity<TId, TData>;
            if (entity == null) return false;

            return Id.Value == entity.Id.Value;
        }

        bool IEquatable<WapiEntity<TId, TData>>.Equals(WapiEntity<TId, TData> other)
        {
            return Equals(other);
        }

        //  ------------------
        //  GetHashCode method
        //  ------------------

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>

        public override int GetHashCode()
        {
            return Id.Value.GetHashCode();
        }

        #endregion IEquatable implementation
    }
} 

// eof "WapiEntity.cs"
