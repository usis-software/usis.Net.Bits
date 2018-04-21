//
//  @(#) FormViewControl.cs
//
//  Project:    usis.ManagementConsole
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using System;
using System.Windows.Forms;

namespace usis.ManagementConsole
{
    //  ---------------------
    //  FormViewControl class
    //  ---------------------

    /// <summary>
    /// Provides an empty control that can be used to create the content of a Windows Forms view.
    /// </summary>
    /// <seealso cref="FormViewControl{NamespaceSnapInBase}" />

    public class FormViewControl : FormViewControl<NamespaceSnapInBase> { }

    /// <summary>
    /// Provides an empty control that can be used to create the content of a Windows Forms view.
    /// </summary>
    /// <typeparam name="TSnapIn">The type of the MMC snap-in.</typeparam>
    /// <seealso cref="UserControl" />
    /// <seealso cref="IFormViewControl" />

    public class FormViewControl<TSnapIn> : UserControl, IFormViewControl where TSnapIn : NamespaceSnapInBase
    {
        #region fields

        private Control oldParent;

        #endregion fields

        #region properties

        //  -----------------
        //  FormView property
        //  -----------------

        /// <summary>
        /// Gets the associated Windows Forms view.
        /// </summary>
        /// <value>
        /// The form view.
        /// </value>

        protected FormView FormView { get; private set; }

        //  ---------------
        //  SnapIn property
        //  ---------------

        /// <summary>
        /// Gets the scope node's snap-in.
        /// </summary>
        /// <value>
        /// The scope node's snap-in.
        /// </value>

        protected TSnapIn SnapIn => FormView.ScopeNode.SnapIn as TSnapIn;

        #endregion properties

        #region overrides

        //  ----------------------
        //  OnParentChanged method
        //  ----------------------

        /// <summary>
        /// Raises the <see cref="Control.ParentChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>

        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null)
            {
                if (!DesignMode) Size = Parent.ClientSize;
                Parent.SizeChanged += Parent_SizeChanged;
            }
            if (oldParent != null)
            {
                oldParent.SizeChanged -= Parent_SizeChanged;
            }
            oldParent = Parent;
            base.OnParentChanged(e);
        }

        #endregion overrides

        #region IFormViewControl implementation

        //  -----------------
        //  Initialize method
        //  -----------------

        /// <summary>
        /// Uses the associated Windows Forms view to initialize the control.
        /// </summary>
        /// <param name="view">The associated <c>FormView</c> value.</param>

        public void Initialize(FormView view)
        {
            FormView = view;
            OnInitialize();
        }

        //  -------------------
        //  OnInitialize method
        //  -------------------

        /// <summary>
        /// Called when the control is initialized.
        /// </summary>

        protected virtual void OnInitialize() { }

        #endregion IFormViewControl implementation

        #region private methods

        //  -------------------------
        //  Parent_SizeChanged method
        //  -------------------------

        private void Parent_SizeChanged(object sender, EventArgs e)
        {
            if (!DesignMode) Size = Parent.ClientSize;
        }

        #endregion private methods
    }
}

// eof "FormViewControl.cs"
