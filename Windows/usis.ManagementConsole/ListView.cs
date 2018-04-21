//
//  @(#) ListView.cs
//
//  Project:    usis.ManagementConsole
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace usis.ManagementConsole
{
    #region ListView<TSnapIn> class

    //  -----------------------
    //  ListView<TSnapIn> class
    //  -----------------------

    /// <summary>
    /// Provides a base class for a MMC list view with a typed <see cref="SnapIn"/> property.
    /// </summary>
    /// <typeparam name="TSnapIn">The type of the snap-in.</typeparam>
    /// <seealso cref="ListView{TSnapIn, ResultNode, ScopeNode}"/>

    public abstract class ListView<TSnapIn> : ListView<TSnapIn, ResultNode, Microsoft.ManagementConsole.ScopeNode> where TSnapIn : NamespaceSnapInBase { }

    #endregion ListView<TSnapIn> class

    #region ListView<TSnapIn, TResultNode, TScopeNode> class

    //  ------------------------------------------------
    //  ListView<TSnapIn, TResultNode, TScopeNode> class
    //  ------------------------------------------------

    /// <summary>
    /// Provides a base class for a MMC list view with typed snap-in and nodes members.
    /// </summary>
    /// <typeparam name="TSnapIn">The type of the snap in.</typeparam>
    /// <typeparam name="TResultNode">The type of the result node.</typeparam>
    /// <typeparam name="TScopeNode">The type of the scope node.</typeparam>
    /// <seealso cref="ListView{TSnapIn, ResultNode, ScopeNode}" />

    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public abstract class ListView<TSnapIn, TResultNode, TScopeNode> : ListView<TSnapIn, TResultNode, object, TScopeNode> 
        where TSnapIn : NamespaceSnapInBase
        where TResultNode : ResultNode
        where TScopeNode : Microsoft.ManagementConsole.ScopeNode
    { }

    #endregion ListView<TSnapIn, TResultNode, TScopeNode> class

    #region ListView<TSnapIn, TResultNode, TItem, TScopeNode> class

    //  -------------------------------------------------------
    //  ListView<TSnapIn, TResultNode, TItem, TScopeNode> class
    //  -------------------------------------------------------

    /// <summary>
    /// Provides a base class for a MMC list view with typed snap-in, nodes and item members.
    /// </summary>
    /// <typeparam name="TSnapIn">The type of the snap-in.</typeparam>
    /// <typeparam name="TResultNode">The type of the result node.</typeparam>
    /// <typeparam name="TItem">The type of the items.</typeparam>
    /// <typeparam name="TScopeNode">The type of the scope node.</typeparam>
    /// <seealso cref="ListView{TSnapIn, ResultNode, ScopeNode}" />

    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public abstract class ListView<TSnapIn, TResultNode, TItem, TScopeNode> : MmcListView
        where TSnapIn : NamespaceSnapInBase
        where TResultNode : ResultNode
        where TItem : class
        where TScopeNode : Microsoft.ManagementConsole.ScopeNode
    {
        #region properties

        //  ---------------
        //  SnapIn property
        //  ---------------

        /// <summary>
        /// Gets the snap-in associated with the view.
        /// </summary>
        /// <value>
        /// The snap-in associated with the view.
        /// </value>

        public new TSnapIn SnapIn => base.SnapIn as TSnapIn;

        //  ------------------
        //  ScopeNode property
        //  ------------------

        /// <summary>
        ///  Gets the scope node with which this view is associated.
        /// </summary>
        /// <value>
        /// The scope node with which this view is associated.
        /// </value>

        public new TScopeNode ScopeNode => base.ScopeNode as TScopeNode;

        //  ----------------------------
        //  SelectedResultNodes property
        //  ----------------------------

        /// <summary>
        /// Gets the selected result nodes.
        /// </summary>
        /// <value>
        /// An enumerator, to iterate the selected result nodes.
        /// </value>

        public IEnumerable<TResultNode> SelectedResultNodes => SelectedNodes.Cast<TResultNode>();

        //  --------------------------
        //  FirstSelectedNode property
        //  --------------------------

        /// <summary>
        /// Gets the first selected node.
        /// </summary>
        /// <value>
        /// The first selected node.
        /// </value>

        public TResultNode FirstSelectedNode => SelectedResultNodes.FirstOrDefault();

        #endregion properties

        #region methods

        //  -------------
        //  Attach method
        //  -------------

        /// <summary>
        /// Attaches the specified observable collection as the source of the list view.
        /// </summary>
        /// <typeparam name="T">The type of the list items.</typeparam>
        /// <param name="collection">The collection to attach.</param>

        protected void Attach<T>(ObservableCollection<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            collection.CollectionChanged += (sender, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        e.AddItems<TItem>((item, i) => ResultNodes.Insert(i, CreateResultNode(item)));
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        e.RemoveItems<TItem>((item, i) => ResultNodes.RemoveAt(i));
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        e.ReplaceItems<TItem>((oldItem, newItem, i) =>
                        {
                            var selected = SelectedNodes.Contains(ResultNodes[i]);
                            ResultNodes[i] = CreateResultNode(newItem);
                            if (selected) ResultNodes[i].SendSelectionRequest(true);
                        });
                        break;
                    case NotifyCollectionChangedAction.Move:
                        throw new NotImplementedException();
                    case NotifyCollectionChangedAction.Reset:
                        ResultNodes.Clear();
                        break;
                    default:
                        break;
                }
            };
        }

        //  -------------
        //  Reload method
        //  -------------

        /// <summary>
        /// Reloads the list view result nodes from the specified list.
        /// </summary>
        /// <typeparam name="T">The type of the list items.</typeparam>
        /// <param name="list">The list.</param>

        protected void Reload<T>(IEnumerable<T> list)
        {
            System.Diagnostics.Debug.WriteLine("Reloading list view result nodes...");

            var equatable = typeof(T).GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEquatable<T>).GetGenericTypeDefinition());
            IEnumerable<T> tags = equatable ? SelectedItems<T>() : null;
            ResultNodes.Replace(list.Select(item => CreateResultNode(item as TItem)));
            if (equatable) foreach (var node in ResultNodes.Cast<ResultNode>()) node.SendSelectionRequest(tags.Contains((T)node.Tag));
        }

        private IEnumerable<T> SelectedItems<T>()
        {
            return SelectedNodes.Cast<Node>().Where(node => node is ResultNode).Select(node => (T)node.Tag);
        }

        //  -----------------------
        //  CreateResultNode method
        //  -----------------------

        /// <summary>
        /// Creates a result node from the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// A newly created result node.
        /// </returns>

        protected virtual ResultNode CreateResultNode(TItem item)
        {
            return new ResultNode() { DisplayName = item.ToString(), Tag = item };
        }

        //  -----------------
        //  SelectItem method
        //  -----------------

        /// <summary>
        /// Selects the result node for the specified item.
        /// </summary>
        /// <param name="item">The item.</param>

        protected void SelectItem(TItem item)
        {
            foreach (var node in ResultNodes.Cast<ResultNode>())
            {
                if (node.Tag != null && node.Tag.Equals(item)) node.SendSelectionRequest(true);
            }
        }

        #endregion methods

        #region overrides

        //  -------------
        //  OnShow method
        //  -------------

        /// <summary>
        /// Called when a view is shown in the UI.
        /// </summary>
        /// <remarks>
        /// This implementation sets the <see cref="ScopeNode{TSnapIn, TScopeNode}.CurrentView"/> property of its <see cref="ScopeNode"/> object.
        /// </remarks>

        protected override void OnShow()
        {
            if (base.ScopeNode is IScopeNode node) node.Inject(this);
        }

        //  -------------
        //  OnHide method
        //  -------------

        /// <summary>
        /// Called when a view is hidden in the UI.
        /// </summary>

        protected override void OnHide()
        {
            if (base.ScopeNode is IScopeNode node) if (node.CurrentView == this) node.Inject(null);
        }

        #endregion overrides
    }

    #endregion ListView<TSnapIn, TResultNode, TItem, TScopeNode> class

    #region NotifyCollectionChangedEventArgsExtensions class

    //  ------------------------------------------------
    //  NotifyCollectionChangedEventArgsExtensions class
    //  ------------------------------------------------

    internal static class NotifyCollectionChangedEventArgsExtensions
    {
        //  ---------------
        //  AddItems method
        //  ---------------

        internal static void AddItems<T>(this NotifyCollectionChangedEventArgs e, Action<T, int> action)
        {
            if (e.Action != NotifyCollectionChangedAction.Add) throw new ArgumentException("The Action must be 'Add'.", nameof(e));
            for (int i = 0; i < e.NewItems.Count; i++)
            {
                action.Invoke((T)e.NewItems[i], e.NewStartingIndex + i);
            }
        }

        //  -------------------
        //  ReplaceItems method
        //  -------------------

        internal static void ReplaceItems<T>(this NotifyCollectionChangedEventArgs e, Action<T, T, int> action)
        {
            if (e.Action != NotifyCollectionChangedAction.Replace) throw new ArgumentException("The Action must be 'Replace'.", nameof(e));
            for (int i = 0; i < e.NewItems.Count; i++)
            {
                action.Invoke((T)e.OldItems[i], (T)e.NewItems[i], e.NewStartingIndex + i);
            }
        }

        //  ------------------
        //  RemoveItems method
        //  ------------------

        internal static void RemoveItems<T>(this NotifyCollectionChangedEventArgs e, Action<T, int> action)
        {
            if (e.Action != NotifyCollectionChangedAction.Remove) throw new ArgumentException("The Action must be 'Remove'.", nameof(e));
            for (int i = 0; i < e.OldItems.Count; i++)
            {
                action.Invoke((T)e.OldItems[i], e.OldStartingIndex);
            }
        }
    }

    #endregion NotifyCollectionChangedEventArgsExtensions class
}

// eof "ListView.cs"
