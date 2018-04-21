//
//  @(#) Window.cs
//
//  Project:    Storage Editor
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Globalization;
using usis.Framework;
using usis.Platform;

namespace usis.StorageEditor
{
    //  ------------
    //  Window class
    //  ------------

    internal partial class Window : Windows.Forms.Window, IView<StorageDocument>
    {
        #region fields

        private string titleFormat;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal Window(StorageDocument document)
        {
            InitializeComponent();

            titleFormat = Text;

            Inject(document);
        }

        #endregion construction

        #region properties

        //  --------------
        //  Model property
        //  --------------

        public StorageDocument Model => storageControl.Model;

        //  -------------
        //  View property
        //  -------------

        public StorageControl View => storageControl;

        #endregion properties

        #region events

        internal event EventHandler DocumentNew;
        internal event EventHandler DocumentOpen;
        internal event EventHandler DocumentSave;
        internal event EventHandler DocumentSaveAs;

        #endregion events

        #region event handlers

        private void FileNewClick(object sender, EventArgs e) { DocumentNew?.Invoke(this, e); }
        private void FileOpenClick(object sender, EventArgs e) { DocumentOpen?.Invoke(this, e); }
        private void FileSaveClick(object sender, EventArgs e) { DocumentSave?.Invoke(this, e); }
        private void FileSaveAsClick(object sender, EventArgs e) { DocumentSaveAs?.Invoke(this, e); }
        private void FileExitClick(object sender, EventArgs e) { Close(); }

        #endregion event handlers

        #region methods

        //  -------------
        //  Inject method
        //  -------------

        public void Inject(StorageDocument dependency)
        {
            if (dependency == null) throw new ArgumentNullException(nameof(dependency));

            storageControl.Inject(dependency);
            UpdateTitle();
            dependency.PropertyChanged += (sender, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(dependency.Title):
                        UpdateTitle();
                        break;
                    default:
                        break;
                }
            };
        }

        //  ------------------
        //  UpdateTitle method
        //  ------------------

        private void UpdateTitle()
        {
            Text = string.Format(CultureInfo.CurrentCulture, titleFormat, Model.Title);
        }

        #endregion methods

        protected override void OnSaveSettings(IValueStorage storage)
        {
            base.OnSaveSettings(storage);

            storage.SetValue(nameof(StorageControl.TreeWidth), View.TreeWidth);
        }

        protected override void OnRestoreSettings(IValueStorage storage)
        {
            base.OnRestoreSettings(storage);

            View.TreeWidth = storage.GetInt32(nameof(StorageControl.TreeWidth), View.TreeWidth, CultureInfo.InvariantCulture);
        }

#pragma warning disable IDE1006 // Naming Styles
        private void isDirtyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isDirtyToolStripMenuItem.Checked) Model.SetDirty();
            else Model.SetClean();
        }
#pragma warning restore IDE1006 // Naming Styles
    }
}

// eof "Window.cs"
