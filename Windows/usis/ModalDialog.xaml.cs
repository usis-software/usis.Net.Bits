//
//  @(#) ModalDialog.xaml.cs
//
//  Project:    usis.Windows
//  System:     Microsoft Visual Studio 12
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
                return this.dialogControl;
            }
            set
            {
                if (this.dialogControl == null && value != null)
                {
                    this.dialogContent.Children.Remove(this.dummy);
                }
                if (value != null)
                {
                    this.dialogContent.Children.Add(value);
                }
                else this.dialogContent.Children.Add(this.dummy);

                this.dialogControl = value;
            }

        } // DialogControl property

        //  -------------------
        //  SettingsName method
        //  -------------------

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

        private Visibility CloseButtonVisibility
        {
            get
            {
                return this.CloseButtonOnly ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Visibility OkCancelButtonVisibility
        {
            get
            {
                return this.CloseButtonOnly ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public Button OkButton
        {
            get
            {
                return this.btnOk;
            }
        }

        #endregion properties

        #region contruction

        public ModalDialog()
        {
            this.InitializeComponent();
        }

        #endregion contruction

        #region overrides

        //  --------------------------
        //  OnSourceInitialized method
        //  --------------------------

        protected override void OnSourceInitialized(EventArgs e)
        {
            this.MinHeight = this.Height;
            this.MinWidth = this.Width;

            if (!string.IsNullOrWhiteSpace(this.SettingsName))
            {
                this.SizeToContent = SizeToContent.Manual;
                this.RestoreWindowState(this.SettingsName);
            }

            base.OnSourceInitialized(e);

            this.SetModalDialogFrame();
            this.HideMaximizeBox();
            this.HideMinimizeBox();

            this.btnOk.Visibility = this.OkCancelButtonVisibility;
            this.btnCancel.Visibility = this.OkCancelButtonVisibility;
            this.btnClose.Visibility = this.CloseButtonVisibility;

            var control = this.DialogControl as FrameworkElement;
            if (control != null)
            {
                control.Width = double.NaN;
                control.Height = double.NaN;
            }

        } // OnSourceInitialized method

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

    } // ModalDialog class

} // audius.C2.UI namespace

// eof "ModalDialog.xaml.cs"
