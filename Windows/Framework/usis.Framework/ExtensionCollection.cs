//
//  @(#) ExtensionCollection.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using usis.Platform;

namespace usis.Framework
{
    //  ----------------------------------
    //  ExtensionCollection<TObject> class
    //  ----------------------------------

    /// <summary>
    /// Represents a collection of extensions.
    /// </summary>
    /// <typeparam name="TObject">The type for the collection.</typeparam>

    public sealed class ExtensionCollection<TObject> :
        Collection<IExtension<TObject>>,
        IExtensionCollection<TObject>,
        ICollection<IExtension<TObject>>,
        IEnumerable<IExtension<TObject>>,
        IEnumerable
        where TObject : IExtensibleObject<TObject>
    {
        #region fields

        private TObject extensibleObject;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionCollection{TObject}"/> class
        /// with a specified owner.
        /// </summary>
        /// <param name="owner">The owner of the collection.</param>

        public ExtensionCollection(TObject owner)
        {
            extensibleObject = owner;
        }

        #endregion construction

        #region methods

        //  -----------------------
        //  Find<TExtension> method
        //  -----------------------

        /// <summary>
        /// Finds the specified extension object in the collection.
        /// </summary>
        /// <typeparam name="TExtension">The type of extension object.</typeparam>
        /// <returns>
        /// The extension object that was found, or null if no extensions implement the type.
        /// If more than one extension implements the type, the most recently added is returned.
        /// </returns>

        public TExtension Find<TExtension>()
        {
            TExtension extension = default(TExtension);
            foreach (var item in this)
            {
                if (item is TExtension)
                {
                    extension = (TExtension)item;
                }
            }
            return extension;
        }

        //  --------------------------
        //  FindAll<TExtension> method
        //  --------------------------

        /// <summary>
        /// Finds all extension object in the collection specified by <i>TExtension</i>.
        /// </summary>
        /// <typeparam name="TExtension">The type of extension object.</typeparam>
        /// <returns>
        /// A collection of all extension objects in the collection that implement the specified type.
        /// If no extensions implement this type, a non-null empty collection is returned.
        /// </returns>

        public Collection<TExtension> FindAll<TExtension>()
        {
            var list = new Collection<TExtension>();
            foreach (var item in this)
            {
                if (item is TExtension)
                {
                    list.Add((TExtension)item);
                }
            }
            return list;
        }

        //  -----------------
        //  ClearItems method
        //  -----------------

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>

        protected override void ClearItems()
        {
            foreach (var item in this)
            {
                item.Detach(extensibleObject);
            }
            base.ClearItems();
        }

        //  -----------------
        //  InsertItem method
        //  -----------------

        /// <summary>
        /// Inserts an item into the collection at a specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the collection where the object is to be inserted.</param>
        /// <param name="item">The object to be inserted into the collection.</param>

        protected override void InsertItem(int index, IExtension<TObject> item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            base.InsertItem(index, item);
            item.Attach(extensibleObject);
        }

        //  -----------------
        //  RemoveItem method
        //  -----------------

        /// <summary>
        /// Removes an item at a specified index from the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the element to be retrieved from the collection.</param>

        protected override void RemoveItem(int index)
        {
            var item = base[index];
            item.Detach(extensibleObject);
            base.RemoveItem(index);
        }

        //  --------------
        //  SetItem method
        //  --------------

        /// <summary>
        /// Replaces the item at a specified index with another item.
        /// </summary>
        /// <param name="index">The zero-based index of the object to be replaced.</param>
        /// <param name="item">The object to replace.</param>

        protected override void SetItem(int index, IExtension<TObject> item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var oldItem = base[index];
            oldItem.Detach(extensibleObject);
            base.SetItem(index, item);
            item.Attach(extensibleObject);
        }

        #endregion methods
    }
}

// eof "ExtensionCollection.cs"
