//
//  @(#) IExtension.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace usis.Platform
{
    #region IExtensibleObject<T> interface

    //  ------------------------------
    //  IExtensibleObject<T> interface
    //  ------------------------------

    /// <summary>
    /// Enables an object to participate in custom behavior that is provided by extensions.
    /// </summary>
    /// <typeparam name="TObject">The type of the extension class.</typeparam>

    public interface IExtensibleObject<TObject> where TObject : IExtensibleObject<TObject>
    {
        //  -------------------
        //  Extensions property
        //  -------------------

        /// <summary>
        /// Gets a collection of extension objects for this extensible object.
        /// </summary>
        /// <value>
        /// The collection of extension objects for this extensible object.
        /// </value>

        IExtensionCollection<TObject> Extensions { get; }
    }

    #endregion IExtensibleObject<T> interface

    #region IExtensionCollection<TObject> interface

    //  ---------------------------------------
    //  IExtensionCollection<TObject> interface
    //  ---------------------------------------

    /// <summary>
    /// A collection of the <see cref="IExtension{TObject}" /> objects that allow for retrieving the <see cref="IExtension{TObject}"/> by its type.
    /// </summary>
    /// <typeparam name="TObject">The type of the extension objects.</typeparam>

    public interface IExtensionCollection<TObject> : ICollection<IExtension<TObject>>, IEnumerable<IExtension<TObject>>, IEnumerable where TObject : IExtensibleObject<TObject>
    {
        //  --------------
        //  Find<E> method
        //  --------------

        /// <summary>
        /// Finds the specified extension object in the collection.
        /// </summary>
        /// <typeparam name="TExtension">The type of extension object.</typeparam>
        /// <returns>The extension object that was found.</returns>

        TExtension Find<TExtension>();

        //  -----------------
        //  FindAll<E> method
        //  -----------------

        /// <summary>
        /// Finds all extension object in the collection specified by <i>TExtension</i>.
        /// </summary>
        /// <typeparam name="TExtension">The type of extension object.</typeparam>
        /// <returns>A collection of all extension objects in the collection that implement the specified type.</returns>

        Collection<TExtension> FindAll<TExtension>();
    }

    #endregion IExtensionCollection<TObject> interface

    #region IExtension<T> interface

    //  -----------------------
    //  IExtension<T> interface
    //  -----------------------

    /// <summary>
    /// Enables an object to extend another object through aggregation.
    /// </summary>
    /// <typeparam name="TObject">The object that participates in the custom behavior.</typeparam>

    public interface IExtension<TObject> where TObject : IExtensibleObject<TObject>
    {
        //  -------------
        //  Attach method
        //  -------------

        /// <summary>
        /// Enables an extension object to find out when it has been aggregated. Called when
        /// the extension is added to the <see cref="IExtensibleObject{TObject}.Extensions"/> property.
        /// </summary>
        /// <param name="owner">The extensible object that aggregates this extension.</param>

        void Attach(TObject owner);

        //  -------------
        //  Detach method
        //  -------------

        /// <summary>
        /// Enables an object to find out when it is no longer aggregated. Called when an
        /// extension is removed from the <see cref="IExtensibleObject{TObject}.Extensions"/> property.
        /// </summary>
        /// <param name="owner">The extensible object that aggregates this extension.</param>

        void Detach(TObject owner);
    }

    #endregion IExtension<T> interface

    #region Extension<TObject> class

    //  ------------------------
    //  Extension<TObject> class
    //  ------------------------

    /// <summary>
    /// Provides a base class that enables a derived class to extend another object through aggregation.
    /// </summary>
    /// <typeparam name="TObject">The object that participates in the custom behavior.</typeparam>

    public abstract class Extension<TObject> : IExtension<TObject> where TObject : class, IExtensibleObject<TObject>
    {
        #region properties

        //  --------------
        //  Owner property
        //  --------------

        /// <summary>
        /// Gets the owner of the extension.
        /// </summary>
        /// <value>
        /// The owner of the extension.
        /// </value>

        protected TObject Owner { get; private set; }

        #endregion properties

        #region IExtension<TObject> implementation

        //  -------------
        //  Attach method
        //  -------------

        /// <summary>
        /// Enables an extension object to find out when it has been aggregated.
        /// </summary>
        /// <param name="owner">The extensible object that aggregates this extension.</param>

        public void Attach(TObject owner)
        {
            if (Owner != null) throw new InvalidOperationException();
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            OnAttach();
        }

        //  -------------
        //  Detach method
        //  -------------

        /// <summary>
        /// Enables an object to find out when it is no longer aggregated.
        /// </summary>
        /// <param name="owner">The extensible object that aggregates this extension.</param>

        public void Detach(TObject owner)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            if (!owner.Equals(Owner)) throw new InvalidOperationException();
            OnDetach();
        }

        #endregion IExtension<TObject> implementation

        #region virtual methods

        //  ---------------
        //  OnAttach method
        //  ---------------

        /// <summary>
        /// Called when the extension is added to the
        /// <see cref="IExtensibleObject{TObject}.Extensions"/> property.
        /// </summary>

        protected virtual void OnAttach() { }

        //  ---------------
        //  OnDetach method
        //  ---------------

        /// <summary>
        /// Called when an extension is removed from the
        /// <see cref="IExtensibleObject{TObject}.Extensions"/> property.
        /// </summary>

        protected virtual void OnDetach() { }

        #endregion virtual methods
    }

    #endregion Extension<TObject> class
}

// eof "IExtension.cs"
