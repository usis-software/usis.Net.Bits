//
//  @(#) ScopeNodeBase.cs
//
//  Project:    usis.ManagementConsole
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using System;
using System.Collections.Generic;
using usis.Platform;

namespace usis.ManagementConsole
{
    //  ---------------
    //  ScopeNode class
    //  ---------------

    /// <summary>
    /// Provides a base class for <see cref="Microsoft.ManagementConsole.ScopeNode"/>s.
    /// </summary>
    /// <seealso cref="ScopeNode{TSnapIn}" />

    public abstract class ScopeNode : ScopeNode<SnapIn>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeNode"/> class.
        /// </summary>

        protected ScopeNode() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeNode"/> class
        /// using a flag to determine whether the scope node has an expand icon at creation.
        /// </summary>
        /// <param name="hideExpandIcon">Determines whether a scope node initially has an expand icon.
        /// When nodes are added to the Children collection,
        /// the expand icon is enabled regardless of the value of this parameter.</param>

        protected ScopeNode(bool hideExpandIcon) : base(hideExpandIcon) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeNode"/> class
        /// using the GUID that represents the type of the node as a parameter.
        /// </summary>
        /// <param name="nodeType">The type of node. It is used by the MMC extension mechanism.</param>

        protected ScopeNode(Guid nodeType) : base(nodeType) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeNode"/> class
        /// using as parameters: the GUID for the node type and the flag to determine whether the scope node has an expand icon.
        /// </summary>
        /// <param name="nodeType">
        /// The type of node. It is used by the MMC extension mechanism.
        /// </param>
        /// <param name="hideExpandIcon">
        /// Determines whether a scope node initially has an expand icon.
        /// When nodes are added to the Children collection, the expand icon is enabled regardless of the value of this parameter.
        /// </param>

        protected ScopeNode(Guid nodeType, bool hideExpandIcon) : base(nodeType, hideExpandIcon) { }

        #endregion construction
    }

    //  ------------------------
    //  ScopeNode<TSnapIn> class
    //  ------------------------

    /// <summary>
    ///  Provides a base class for <see cref="Microsoft.ManagementConsole.ScopeNode" />s.
    /// </summary>
    /// <typeparam name="TSnapIn">The type of the snap-in.</typeparam>
    /// <seealso cref="ScopeNode{TSnapIn, TScopeNode}" />

    public abstract class ScopeNode<TSnapIn> : ScopeNode<TSnapIn, Microsoft.ManagementConsole.ScopeNode>
        where TSnapIn : NamespaceSnapInBase
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeNode{TSnapIn}"/> class.
        /// </summary>

        protected ScopeNode() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeNode{TSnapIn}" /> class
        /// using a flag to determine whether the scope node has an expand icon at creation.
        /// </summary>
        /// <param name="hideExpandIcon">Determines whether a scope node initially has an expand icon.
        /// When nodes are added to the Children collection,
        /// the expand icon is enabled regardless of the value of this parameter.</param>

        protected ScopeNode(bool hideExpandIcon) : base(hideExpandIcon) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeNode{TSnapIn}" /> class
        /// using the GUID that represents the type of the node as a parameter.
        /// </summary>
        /// <param name="nodeType">The type of node. It is used by the MMC extension mechanism.</param>

        protected ScopeNode(Guid nodeType) : base(nodeType) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeNode{TSnapIn}"/> class
        /// using as parameters: the GUID for the node type and the flag to determine whether the scope node has an expand icon.
        /// </summary>
        /// <param name="nodeType">
        /// The type of node. It is used by the MMC extension mechanism.
        /// </param>
        /// <param name="hideExpandIcon">
        /// Determines whether a scope node initially has an expand icon.
        /// When nodes are added to the Children collection, the expand icon is enabled regardless of the value of this parameter.
        /// </param>

        protected ScopeNode(Guid nodeType, bool hideExpandIcon) : base(nodeType, hideExpandIcon) { }

        #endregion construction
    }

    //  ------------------------------------
    //  ScopeNode<TSnapIn, TScopeNode> class
    //  ------------------------------------

    /// <summary>
    /// Provides a base class for <see cref="Microsoft.ManagementConsole.ScopeNode" />s.
    /// </summary>
    /// <typeparam name="TSnapIn">The type of the snap-in.</typeparam>
    /// <typeparam name="TScopeNode">The type of the scope nodes.</typeparam>

    public abstract class ScopeNode<TSnapIn, TScopeNode> : Microsoft.ManagementConsole.ScopeNode, IScopeNode // usis.Platform.IInjectable<View>
        where TSnapIn : NamespaceSnapInBase
        where TScopeNode : Microsoft.ManagementConsole.ScopeNode
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeNode{TSnapIn}"/> class.
        /// </summary>

        protected ScopeNode() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeNode{TSnapIn}" /> class
        /// using a flag to determine whether the scope node has an expand icon at creation.
        /// </summary>
        /// <param name="hideExpandIcon">Determines whether a scope node initially has an expand icon.
        /// When nodes are added to the Children collection,
        /// the expand icon is enabled regardless of the value of this parameter.</param>

        protected ScopeNode(bool hideExpandIcon) : base(hideExpandIcon) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeNode{TSnapIn}" /> class
        /// using the GUID that represents the type of the node as a parameter.
        /// </summary>
        /// <param name="nodeType">The type of node. It is used by the MMC extension mechanism.</param>

        protected ScopeNode(Guid nodeType) : base(nodeType) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeNode{TSnapIn}"/> class
        /// using as parameters: the GUID for the node type and the flag to determine whether the scope node has an expand icon.
        /// </summary>
        /// <param name="nodeType">
        /// The type of node. It is used by the MMC extension mechanism.
        /// </param>
        /// <param name="hideExpandIcon">
        /// Determines whether a scope node initially has an expand icon.
        /// When nodes are added to the Children collection, the expand icon is enabled regardless of the value of this parameter.
        /// </param>

        protected ScopeNode(Guid nodeType, bool hideExpandIcon) : base(nodeType, hideExpandIcon) { }

        #endregion construction

        #region properties

        //  ---------------
        //  SnapIn property
        //  ---------------

        /// <summary>
        /// Gets the instance of the snap-in that is associated with the node.
        /// </summary>
        /// <value>
        /// The snap-in that is associated with the node.
        /// </value>

        public new TSnapIn SnapIn => base.SnapIn as TSnapIn;

        //  --------------------
        //  CurrentView property
        //  --------------------

        /// <summary>
        /// Gets the current view.
        /// </summary>
        /// <value>
        /// The current view.
        /// </value>

        public View CurrentView { get; private set; }

        //  --------------
        //  Nodes property
        //  --------------

        /// <summary>
        /// Gets the child scope nodes.
        /// </summary>
        /// <value>
        /// The the child scope nodes.
        /// </value>

        public IEnumerable<TScopeNode> Nodes
        {
            get
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    if (Children[i] is TScopeNode node) yield return node;
                }
            }
        }

        #endregion properties

        #region overrides

        //  ----------------
        //  OnRefresh method
        //  ----------------

        /// <summary>
        /// Called when the standard verb <see cref="StandardVerbs.Refresh"/> is triggered. 
        /// </summary>
        /// <param name="status">The object that holds the status information.</param>

        protected override void OnRefresh(AsyncStatus status)
        {
            Refreshed?.Invoke(this, new AsyncStatusEventArgs(status));
            base.OnRefresh(status);
        }

        #endregion overrides

        #region methods

        //  --------------
        //  Refresh method
        //  --------------        
        /// <summary>
        /// Raises the <see cref="Refreshed"/> event.
        /// </summary>

        protected void Refresh()
        {
            Refreshed?.Invoke(this, null);
        }

        //  -------------
        //  Inject method
        //  -------------

        /// <summary>
        /// Passes a dependency to the implementing (dependend) object.
        /// </summary>
        /// <param name="dependency">The type of the dependency.</param>

        public void Inject(View dependency)
        {
            CurrentView = dependency;
        }

        #endregion methods

        #region events

        //  ---------------
        //  Refreshed event
        //  ---------------

        /// <summary>
        /// Occurs when the standard verb <see cref="StandardVerbs.Refresh"/> was triggered
        /// or when the <see cref="Refresh"/> method was called.
        /// </summary>

        public event EventHandler<AsyncStatusEventArgs> Refreshed;

        #endregion events
    }

    #region IScopeNode interface

    //  --------------------
    //  IScopeNode interface
    //  --------------------

    internal interface IScopeNode : IInjectable<View>
    {
        //  --------------------
        //  CurrentView property
        //  --------------------

        View CurrentView { get; }
    }

    #endregion IScopeNode interface
}

// eof "ScopeNodeBase.cs"
