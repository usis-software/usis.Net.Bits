//
//  @(#) StorageDocument.cs
//
//  Project:    Storage Editor
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.ComponentModel;
using System.IO;
using usis.Platform.StructuredStorage;
using usis.StorageEditor.Properties;

namespace usis.StorageEditor
{
    //  ---------------------
    //  StorageDocument class
    //  ---------------------

    internal sealed class StorageDocument : Framework.Document, INotifyPropertyChanged, IDisposable
    {
        #region fields

        private Storage root;
        private bool isNew;

        #endregion fields

        #region events

        //  ---------------------
        //  PropertyChanged event
        //  ---------------------

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion events

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal StorageDocument() { Open(null); }

        internal StorageDocument(string path) { Open(path); }

        #endregion construction

        #region properties

        //  -------------
        //  Root property
        //  -------------

        internal Storage Root => root;

        //  --------------
        //  IsNew property
        //  --------------

        internal bool IsNew
        {
            get => isNew;
            private set
            {
                if (isNew != value)
                {
                    isNew = value;
                    OnPropertyChanged(nameof(IsNew));
                }
            }
        }

        #endregion properties

        #region methods

        //  -----------
        //  Open method
        //  -----------

        private void Open(string path)
        {
            var editMode = StorageModes.ReadWrite | StorageModes.ShareExclusive | StorageModes.Transacted;
            var newFile = !File.Exists(path);

            root = newFile ?
                Storage.CreateCompoundFile(path, editMode | StorageModes.DeleteOnRelease) :
                Storage.OpenCompoundFile(path, editMode);

            IsNew = newFile;
            UpdateTitle();
        }

        //  ------------------
        //  UpdateTitle method
        //  ------------------

        private void UpdateTitle()
        {
            Title = IsNew ? Resources.Untitled : Path.GetFileName(root.Statistics.Name);
            OnPropertyChanged(nameof(Title));
        }

        //  ------------------------
        //  OnPropertyChanged method
        //  ------------------------

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //  -----------
        //  Save method
        //  -----------

        internal void Save()
        {
            if (IsNew) throw new InvalidOperationException();

            Root.Commit();

            SetClean();
            OnPropertyChanged(nameof(IsDirty));
        }

        //  -------------
        //  SaveAs method
        //  -------------

        internal void SaveAs(string path)
        {
            root.SwitchToFile(path);
            IsNew = false;
            UpdateTitle();

            Save();
        }

        #region IDisposable support

        //  --------------
        //  Dispose method
        //  --------------

        private void Dispose(bool disposing)
        {
            if (root != null)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }
                root.Dispose(); root = null;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //  ---------
        //  finalizer
        //  ---------

        ~StorageDocument()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        #endregion IDisposable support

        #endregion methods
    }
}

// eof "StorageDocument.cs"
