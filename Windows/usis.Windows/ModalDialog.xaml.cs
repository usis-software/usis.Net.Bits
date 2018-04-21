//
//  @(#) ModalDialog.xaml.cs
//
//  Project:    usis.Windows
//  System:     Microsoft Visual Studio 14
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015 usis GmbH. All rights reserved.

using System;
using System.Windows;
using System.Windows.Controls;

namespace usis.Windows
{
    //  -----------------
    //  ModalDialog class
    //  -----------------

    public partial class ModalDialog : Window
    {
        #region properties

        //  ----------------------
        //  DialogControl property
        //  ----------------------

        private UIElement dialogControl;

        public UIElement DialogControl
        {
            get
            {
                return dialogControl;
            }
            set
            {
                if (dialogControl == null && value != null)
                {
                    dialogContent.Children.Remove(dummy);
                }
                if (value != null)
                {
                    dialogContent.Children.Add(value);
                    var control = value as IModalDialogControl;
                    if (control != null) control.ModalDialog = this;
                }
                else dialogContent.Children.Add(dummy);

                dialogControl = value;
            }

        } // DialogControl property

        //  ---------------------
        //  SettingsName property
        //  ---------------------

        public string SettingsName
        {
            get;
            set;
        }

        //  ------------------------
        //  CloseButtonOnly property
        //  ------------------------

        public bool CloseButtonOnly
        {
            get;
            set;
        }

        //  -----------------
        //  OkButton property
        //  -----------------

        public Button OkButton
        {
            get
            {
                return btnOk;
            }

        } // OkButton property

        #region private properties

        private Visibility CloseButtonVisibility
        {
            get
            {
                return CloseButtonOnly ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Visibility OkCancelButtonVisibility
        {
            get
            {
                return this.CloseButtonOnly ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        #endregion private properties

        #endregion properties

        #region contruction

        public ModalDialog()
        {
            InitializeComponent();

            btnCancel.TabIndex = Int32.MaxValue;
            btnClose.TabIndex = Int32.MaxValue;
            btnOk.TabIndex = Int32.MaxValue - 1;
        }

        #endregion contruction

        #region overrides

        //  --------------------------
        //  OnSourceInitialized method
        //  --------------------------

        protected override void OnSourceInitialized(EventArgs e)
        {
            MinHeight = Height;
            MinWidth = Width;

            if (!string.IsNullOrWhiteSpace(SettingsName))
            {
                SizeToContent = SizeToContent.Manual;
                this.RestoreWindowState(SettingsName);
            }

            base.OnSourceInitialized(e);

            this.SetModalDialogFrame();
            this.HideMaximizeBox();
            this.HideMinimizeBox();

            btnOk.Visibility = OkCancelButtonVisibility;
            btnCancel.Visibility = OkCancelButtonVisibility;
            btnClose.Visibility = CloseButtonVisibility;

            var control = DialogControl as FrameworkElement;
            if (control != null)
            {
                control.Width = double.NaN;
                control.Height = double.NaN;
            }

        } // OnSourceInitialized method

        //  ------------------------
        //  OnContentRendered method
        //  ------------------------

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            if (DialogControl != null) DialogControl.Focus();

            SizeToContent = SizeToContent.Manual;

        } // OnContentRendered method

        //  ---------------
        //  OnClosed method
        //  ---------------

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (!string.IsNullOrWhiteSpace(this.SettingsName))
            {
                this.SaveWindowState(this.SettingsName);
            }

        } // OnClosed method

        #endregion overrides

        #region event handlers

        //  ------------------
        //  btnOk_Click method
        //  ------------------

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

        } // btnOk_Click method

        //  ---------------------
        //  btnClose_Click method
        //  ---------------------

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

        } // btnClose_Click method

        #endregion event handlers

    } // ModalDialog class

    #region IModalDialogControl interface

    //  -----------------------------
    //  IModalDialogControl interface
    //  -----------------------------

    public interface IModalDialogControl
    {
        ModalDialog ModalDialog
        {
            get;
            set;
        }

    } // IModalDialogControl interface

    #endregion IModalDialogControl interface

} // audius.C2.UI namespace

// eof "ModalDialog.xaml.cs"
