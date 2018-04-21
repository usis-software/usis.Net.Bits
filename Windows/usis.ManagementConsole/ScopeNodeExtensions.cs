//
//  @(#) ScopeNodeExtensions.cs
//
//  Project:    usis.ManagementConsole
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using System;
using System.Diagnostics.CodeAnalysis;

namespace usis.ManagementConsole
{
    //  -------------------------
    //  ScopeNodeExtensions class
    //  -------------------------

    /// <summary>
    /// Provides extension methods for the <see cref="Microsoft.ManagementConsole.ScopeNode"/> class.
    /// </summary>

    public static class ScopeNodeExtensions
    {
        //  ------------------------
        //  AddActionPaneItem method
        //  ------------------------

        /// <summary>
        /// Adds an action pane item with the specified display name and action to the scope node.
        /// </summary>
        /// <param name="node">The scope node.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="action">The action.</param>
        /// <returns>
        /// A newly created action pane item
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="node"/> is a null reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static Microsoft.ManagementConsole.Action AddActionPaneItem(
            this Microsoft.ManagementConsole.ScopeNode node, string displayName, Action<ActionEventArgs> action)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            return node.ActionsPaneItems.AddAction(displayName, e =>
            {
                try { action.Invoke(e); }
                catch (Exception exception) { node.SnapIn.Console.ShowDialog(exception); }
            });
        }

        /// <summary>
        /// Adds an action pane item with the specified display name and action to the scope node.
        /// </summary>
        /// <param name="node">The scope node.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="imageIndex">Index of an image to display in the action pane.</param>
        /// <param name="action">The action.</param>
        /// <returns>
        /// A newly created action pane item
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="node"/> is a null reference (<c>Nothing</c> in Visual Basic).
        /// </exception>

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static Microsoft.ManagementConsole.Action AddActionPaneItem(
            this Microsoft.ManagementConsole.ScopeNode node,
            string displayName,
            Enum imageIndex,
            Action<ActionEventArgs> action)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            return node.ActionsPaneItems.AddAction(displayName, imageIndex, e =>
            {
                try { action.Invoke(e); }
                catch (Exception exception) { node.SnapIn.Console.ShowDialog(exception); }
            });
        }
    }
}

// eof "ScopeNodeExtensions.cs"
