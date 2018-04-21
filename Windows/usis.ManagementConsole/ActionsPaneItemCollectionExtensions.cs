//
//  @(#) ActionsPaneItemCollectionExtensions.cs
//
//  Project:    usis.ManagementConsole
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using System.Globalization;

namespace usis.ManagementConsole
{
    //  -----------------------------------------
    //  ActionsPaneItemCollectionExtensions class
    //  -----------------------------------------

    /// <summary>
    /// Provides extension methods for the <see cref="ActionsPaneItemCollection"/> class.
    /// </summary>

    public static class ActionsPaneItemCollectionExtensions
    {
        //  ----------------
        //  AddAction method
        //  ----------------

        /// <summary>
        /// Adds an <see cref="Action" /> with the specified display name, description and image index to the action pane.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="description">The description.</param>
        /// <param name="imageIndex">Index of the image.</param>
        /// <param name="action">The <see cref="System.Action" /> to invoke when the action is triggered.</param>
        /// <returns>
        /// The newly created action.
        /// </returns>

        public static Action AddAction(
            this ActionsPaneItemCollection collection,
            string displayName,
            string description,
            int imageIndex,
            System.Action<ActionEventArgs> action)
        {
            var item = collection.AddAction(displayName, description, action);
            item.ImageIndex = imageIndex;
            return item;
        }

        /// <summary>
        /// Adds an <see cref="Action" /> with the specified display name and image index to the action pane.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="imageIndex">Index of the image.</param>
        /// <param name="action">The <see cref="System.Action" /> to invoke when the action is triggered.</param>
        /// <returns>
        /// The newly created action.
        /// </returns>

        public static Action AddAction(
            this ActionsPaneItemCollection collection,
            string displayName,
            int imageIndex,
            System.Action<ActionEventArgs> action)
        {
            var item = collection.AddAction(displayName, action);
            item.ImageIndex = imageIndex;
            return item;
        }

        /// <summary>
        /// Adds an <see cref="Action" /> with the specified display name and image index to the action pane.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="imageIndex">Index of the image.</param>
        /// <param name="action">The <see cref="System.Action" /> to invoke when the action is triggered.</param>
        /// <returns>
        /// The newly created action.
        /// </returns>

        public static Action AddAction(
            this ActionsPaneItemCollection collection,
            string displayName,
            System.Enum imageIndex,
            System.Action<ActionEventArgs> action)
        {
            var item = collection.AddAction(displayName, action);
            item.ImageIndex = System.Convert.ToInt32(imageIndex, CultureInfo.InvariantCulture);
            return item;
        }

        /// <summary>
        /// Adds an <see cref="Action" /> with the specified display name and description to the action pane.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="description">The description.</param>
        /// <param name="action">The <see cref="System.Action" /> to invoke when the action is triggered.</param>
        /// <returns>
        /// The newly created action.
        /// </returns>

        public static Action AddAction(
            this ActionsPaneItemCollection collection,
            string displayName,
            string description,
            System.Action<ActionEventArgs> action)
        {
            var item = collection.AddAction(displayName, action);
            item.Description = description;
            return item;
        }

        /// <summary>
        /// Adds an <see cref="Action" /> with the specified display name to the action pane.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="action">The <see cref="System.Action" /> to invoke when the action is triggered.</param>
        /// <returns>
        /// The newly created action.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="collection" /> is a null reference (<c>Nothing</c> in Visual Basic).</exception>

        public static Action AddAction(
            this ActionsPaneItemCollection collection,
            string displayName,
            System.Action<ActionEventArgs> action)
        {
            if (collection == null) throw new System.ArgumentNullException(nameof(collection));

            var item = ActionFactory.Create(action);
            item.DisplayName = displayName;
            collection.Add(item);
            return item;
        }
    }
}

// eof "ActionsPaneItemCollectionExtensions.cs"
