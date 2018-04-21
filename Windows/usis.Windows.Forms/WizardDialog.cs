//
//  @(#) WizardDialog.cs
//
//  Project:    usis.Windows.Forms
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018 usis GmbH. All rights reserved.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace usis.Windows.Forms
{
    //  ------------------
    //  WizardDialog class
    //  ------------------

    /// <summary>
    /// Provides a base class for a user interface that presents the user a sequence of dialog pages.
    /// </summary>
    /// <seealso cref="Form" />

    public partial class WizardDialog : Form
    {
        #region fields

        private WizardPage firstPage;
        private WizardPage currentPage;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardDialog"/> class.
        /// </summary>

        public WizardDialog() { InitializeComponent(); }

        #endregion construction

        #region overrides

        //  --------------
        //  OnShown method
        //  --------------

        /// <summary>
        /// Raises the <see cref="Form.Shown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="EventArgs" /> that contains the event data.</param>

        protected override void OnShown(EventArgs e)
        {
            ShowPage(firstPage);

            base.OnShown(e);
        }

        #endregion overrides

        #region private method

        #region event handlers

        //  ----------------------
        //  ButtonBackClick method
        //  ----------------------

        private void ButtonBackClick(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Assert(currentPage?.PreviousPage != null);
            ShowPage(currentPage?.PreviousPage);
        }

        //  ----------------------
        //  ButtonNextClick method
        //  ----------------------

        private void ButtonNextClick(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Assert(currentPage?.NextPage != null);
            ShowPage(currentPage?.NextPage);
        }

        //  ------------------------
        //  ButtonFinishClick method
        //  ------------------------

        private void ButtonFinishClick(object sender, EventArgs e)
        {
            ShowWorkingFace(() =>
            {
                var eventArgs = new CancelEventArgs();
                currentPage?.OnFinish(eventArgs);
                if (eventArgs.Cancel) return;
                if (!Modal) Close();
            });
        }

        //  ------------------------
        //  ButtonCancelClick method
        //  ------------------------

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            if (!Modal) Close();
        }

        #endregion event handlers

        //  ---------------
        //  LastPage method
        //  ---------------

        private WizardPage LastPage()
        {
            var page = firstPage;
            while (page.NextPage != null)
            {
                page = page.NextPage;
            }
            return page;
        }

        //  ---------------
        //  ShowPage method
        //  ---------------

        private void ShowPage(WizardPage page)
        {
            ShowWorkingFace(() =>
            {
                var e = new CancelEventArgs();
                if (currentPage != null)
                {
                    if (page == currentPage.NextPage)
                    {
                        currentPage.OnNext(e);
                    }
                    else if (page == currentPage.PreviousPage)
                    {
                        currentPage.OnPrevious(e);
                    }
                    if (!e.Cancel) currentPage.Control.Visible = false;
                }
                if (page != null && !e.Cancel)
                {
                    if (Controls.Contains(page.Control))
                    {
                        page.Control.Visible = true;
                    }
                    else
                    {
                        page.PreviousPage = currentPage;
                        page.Control.Dock = DockStyle.Fill;
                        Controls.Add(page.Control);
                        page.Control.BringToFront();
                    }
                }
                currentPage = page;
            });
        }

        //  ---------------------
        //  DisableButtons method
        //  ---------------------

        private void DisableButtons()
        {
            buttonBack.Enabled = false;
            buttonNext.Enabled = false;
            buttonFinish.Enabled = false;
            buttonCancel.Enabled = false;
        }

        //  ----------------------
        //  ShowWorkingFace method
        //  ----------------------

        private void ShowWorkingFace(Action action)
        {
            DisableButtons();

            UseWaitCursor = true;
            Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + 1);
            Application.DoEvents();

            action.Invoke();

            currentPage?.Control.Focus();
            UpdateButtons();

            UseWaitCursor = false;
            Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y - 1);
        }

        #endregion private method

        #region protected methods

        //  ---------------
        //  AddPages method
        //  ---------------

        /// <summary>
        /// Adds a sequence of pages to the wizard dialog.
        /// </summary>
        /// <param name="pages">The pages to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="pages" /> is a <c>null</c> reference.</exception>

        protected void AddPages(params WizardPage[] pages)
        {
            if (pages == null) throw new ArgumentNullException(nameof(pages));

            System.Diagnostics.Debug.Assert(firstPage == null);

            foreach (var page in pages)
            {
                page.Dialog = this;

                if (firstPage == null) firstPage = page;
                else LastPage().NextPage = page;
            }
        }

        #endregion protected methods

        #region public methods

        //  --------------------
        //  UpdateButtons method
        //  --------------------

        /// <summary>
        /// Enables or disables the wizard's buttons.
        /// </summary>

        public void UpdateButtons()
        {
            buttonBack.Enabled = currentPage?.PreviousPage != null;
            buttonNext.Enabled = currentPage == null ? false : currentPage.CanGoToNextPage();
            buttonFinish.Enabled = currentPage == null ? false : currentPage.CanFinish();
            buttonCancel.Enabled = true;
        }

        #endregion public methods
    }
}

// eof "WizardDialog.cs"
