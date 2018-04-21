//
//  @(#) WizardPage.cs
//
//  Project:    usis.Windows.Forms
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018 usis GmbH. All rights reserved.

using System.ComponentModel;
using System.Windows.Forms;

namespace usis.Windows.Forms
{
    //  ----------------
    //  WizardPage class
    //  ----------------

    /// <summary>
    /// Represents a page in a wizard dialog user interface.
    /// </summary>

    public class WizardPage
    {
        #region fields

        private Control pageControl;

        #endregion fields

        #region properties

        //  ---------------
        //  Dialog property
        //  ---------------

        /// <summary>
        /// Gets the wizard dialog to which this page was added.
        /// </summary>
        /// <value>
        /// The wizard dialog to which this page was added.
        /// </value>

        public WizardDialog Dialog { get; internal set; }

        //  ----------------
        //  Control property
        //  ----------------

        internal Control Control
        {
            get { if (pageControl == null) pageControl = CreateAndIntializeView(); return pageControl; }
        }

        //  -----------------
        //  NextPage property
        //  -----------------

        internal WizardPage NextPage { get; set; }

        //  ---------------------
        //  PreviousPage property
        //  ---------------------

        internal WizardPage PreviousPage { get; set; }

        #endregion properties

        #region methods

        //  -----------------------------
        //  CreateAndIntializeView method
        //  -----------------------------

        private Control CreateAndIntializeView()
        {
            Control tmp = null;
            Control view = null;
            try
            {
                tmp = CreateView();
                view = tmp;
                InitializeView(tmp);
                tmp = null;
            }
            finally
            {
                if (tmp != null) tmp.Dispose();
            }
            return view;
        }

        //  -----------------
        //  CreateView method
        //  -----------------

        /// <summary>
        /// This method is called to create the wizard page's view.
        /// </summary>
        /// <returns>The newly created wizard page view control.</returns>

        protected virtual Control CreateView() { return new Label(); }

        //  ---------------------
        //  InitializeView method
        //  ---------------------

        /// <summary>
        /// This method is called to initialize the wizard page's view.
        /// </summary>
        /// <param name="view">The view control to initialize.</param>

        protected virtual void InitializeView(Control view)
        {
            if (view is Label label)
            {
                label.Text = Dialog?.ToString() + "\n\n" + ToString();
                label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
        }

        //  ----------------------
        //  CanGoToNextPage method
        //  ----------------------

        /// <summary>
        /// Determines whether the wizard can proceed to the next page.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the wizard can proceed to the next page; otherwise, <c>false</c>.
        /// </returns>

        public virtual bool CanGoToNextPage() { return NextPage != null; }

        //  ----------------
        //  CanFinish method
        //  ----------------

        /// <summary>
        /// Determines whether the wizard can be finished.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the wizard can be finished; otherwise, <c>false</c>.
        /// </returns>

        public virtual bool CanFinish() { return NextPage == null; }

        //  -------------
        //  OnNext method
        //  -------------

        /// <summary>
        /// Raises the <see cref="Next" /> event.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>

        public virtual void OnNext(CancelEventArgs e) { Next?.Invoke(this, e); }

        //  -----------------
        //  OnPrevious method
        //  -----------------

        /// <summary>
        /// Raises the <see cref="Previous" /> event.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>

        public virtual void OnPrevious(CancelEventArgs e) { Previous?.Invoke(this, e); }

        //  ---------------
        //  OnFinish method
        //  ---------------

        /// <summary>
        /// Raises the <see cref="Finish" /> event.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>

        public virtual void OnFinish(CancelEventArgs e) { Finish?.Invoke(this, e); }

        #endregion methods

        #region events

        //  ----------
        //  Next event
        //  ----------

        /// <summary>
        /// Occurs when the button is pressed to go to the next page.
        /// </summary>

        public event CancelEventHandler Next;

        //  ----------------------
        //  GoToPreviousPage event
        //  ----------------------

        /// <summary>
        /// Occurs when the button is pressed to go to the previous page.
        /// </summary>

        public event CancelEventHandler Previous;

        //  ------------
        //  Finish event
        //  ------------

        /// <summary>
        /// Occurs when the button is pressed to finish the wizard.
        /// </summary>

        public event CancelEventHandler Finish;

        #endregion events
    }

    #region WizardPage<TControl> class

    //  --------------------------
    //  WizardPage<TView> class
    //  --------------------------

    /// <summary>
    /// Represents a page in a wizard dialog user interface that displays the specified control as content view.
    /// </summary>
    /// <typeparam name="TView">The type of the view control.</typeparam>

    public class WizardPage<TView> : WizardPage where TView : Control, new()
    {
        #region overrides

        //  -----------------
        //  CreateView method
        //  -----------------

        /// <summary>
        /// This method is called to create the wizard page's view.
        /// </summary>
        /// <returns>
        /// The newly created wizard page view control.
        /// </returns>

        protected override Control CreateView() { return new TView(); }

        //  ------------------------
        //  InitializeControl method
        //  ------------------------

        /// <summary>
        /// This method is called to initialize the wizard page's view.
        /// </summary>
        /// <param name="view">The view control to initialize.</param>

        protected override void InitializeView(Control view) { InitializeView(view as TView); }

        #endregion overrides

        #region properties

        //  -------------
        //  View property
        //  -------------

        /// <summary>
        /// Gets the page's view.
        /// </summary>
        /// <value>
        /// The wizard pages's view.
        /// </value>

        public TView View { get { return Control as TView; } }
        
        #endregion properties

        #region methods

        //  ---------------------
        //  InitializeView method
        //  ---------------------

        /// <summary>
        /// This method is called to initialize the wizard page's view.
        /// </summary>
        /// <param name="view">The view control to initialize.</param>

        protected virtual void InitializeView(TView view) { }

        #endregion methods
    }

    #endregion WizardPage<TView> class

    #region WizardPage<TView, TModel> class

    //  -------------------------------
    //  WizardPage<TView, TModel> class
    //  -------------------------------

    /// <summary>
    /// Represents a page in a wizard dialog user interface that receives and holds a model.
    /// </summary>
    /// <typeparam name="TView">The type of the page's view.</typeparam>
    /// <typeparam name="TModel">The type of the model.</typeparam>

    public class WizardPage<TView, TModel> : WizardPage<TView> where TView : Control, new() where TModel : class
    {
        #region properties

        //  --------------
        //  Model property
        //  --------------

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>

        protected TModel Model { get { return (Dialog as WizardDialog<TModel>)?.Model; } }

        #endregion properties

        #region overrides

        //  ---------------------
        //  InitializeView method
        //  ---------------------

        /// <summary>
        /// This method is called to initialize the wizard page's view.
        /// </summary>
        /// <param name="view">The view to initialize.</param>

        protected override void InitializeView(TView view)
        {
            if (view is Platform.IInjectable<TModel> dependant)
            {
                dependant.Inject(Model);
            }
        }

        #endregion overrides
    }

    #endregion WizardPage<TView, TModel> class

    #region WizardDialog<TModel> class

    //  --------------------------
    //  WizardDialog<TModel> class
    //  --------------------------

    /// <summary>
    /// Provides a base class for a user interface that presents the user a sequence of dialog pages.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="WizardDialog" />

    public class WizardDialog<TModel> : WizardDialog
    {
        #region properties

        //  --------------
        //  Model property
        //  --------------

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>

        public TModel Model { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardDialog{TModel}"/> class.
        /// </summary>

        public WizardDialog() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardDialog{TModel}"/> class
        /// with the specified model.
        /// </summary>
        /// <param name="model">The model.</param>

        public WizardDialog(TModel model) { Model = model; }

        #endregion construction
    }

    #endregion WizardDialog<TModel> class
}

// eof "WizardPage.cs"
