//
//  @(#) SnapIn.cs
//
//  Project:    Storage Editor
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using usis.Framework;
using usis.Platform.Windows;
using usis.StorageEditor.Properties;
using usis.Windows.Forms;

namespace usis.StorageEditor
{
    //  ------------
    //  SnapIn class
    //  ------------

    internal sealed class SnapIn : Windows.Forms.Framework.SnapIn, IController<StorageDocument, Window>, IDisposable
    {
        #region fields

        private StorageDocument document;

        #endregion fields

        #region properties

        //  --------------
        //  Model property
        //  --------------

        public StorageDocument Model => document;

        //  -------------
        //  View property
        //  -------------

        public Window View => MainForm as Window;

        //  -----------------
        //  Settings property
        //  -----------------

        private RegistryValueStorage Settings { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        public SnapIn() { Settings = RegistryValueStorage.OpenCurrentUser(@"SOFTWARE\usis\Storage Editor", true); }

        #endregion construction

        #region overrides

        //  -------------------
        //  OnConnecting method
        //  -------------------

        protected override void OnConnecting(CancelEventArgs e)
        {
            var commandLine = Application.GetCommandLine();
            var argument = commandLine.Arguments.FirstOrDefault(a => !a.IsOption);

            Invoke(() =>
            {
                CreateDocument(argument?.Value);
                MainForm = new Window(Model);
                MainForm.RestoreFormState(Settings);
                AttachHandlers();
            });

            base.OnConnecting(e);
        }

        #endregion overrides

        #region methods

        //  ---------------------
        //  CreateDocument method
        //  ---------------------

        private void CreateDocument(string path)
        {
            var tmp = new StorageDocument(path);
            document?.Dispose();
            document = tmp;
        }

        //  ---------------------
        //  AttachHandlers method
        //  ---------------------

        private void AttachHandlers()
        {
            View.FormClosing += (sender, e) => { Invoke(() => { e.Cancel = !SaveChanges(); }, e); };
            View.FormClosed += (sender, e) => Invoke(() => { View.SaveFormState(Settings); });
            View.DocumentNew += (sender, e) => Invoke(NewDocument);
            View.DocumentOpen += (sender, e) => Invoke(OpenDocument);
            View.DocumentSave += (sender, e) => Invoke(SaveDocument);
            View.DocumentSaveAs += (sender, e) => Invoke(SaveDocumentAs);

            View.View.ElementSelected += (sender, e) =>
            {
                switch (e.Statistics.ElementType)
                {
                    case Platform.StructuredStorage.ElementType.None:
                        break;
                    case Platform.StructuredStorage.ElementType.Storage:
                        break;
                    case Platform.StructuredStorage.ElementType.Stream:
                        break;
                    case Platform.StructuredStorage.ElementType.ByteArray:
                        break;
                    case Platform.StructuredStorage.ElementType.PropertySet:
                        break;
                    default:
                        break;
                }
            };
        }

        //  ------------------
        //  NewDocument method
        //  ------------------

        private void NewDocument()
        {
            if (SaveChanges())
            {
                CreateDocument(null);
                Inject(Model);
            }
        }

        //  -------------------
        //  OpenDocument method
        //  -------------------

        private void OpenDocument()
        {
            if (SaveChanges())
            {
                using (var dialog = new OpenFileDialog())
                {
                    if (dialog.ShowDialog(View) == DialogResult.OK)
                    {
                        CreateDocument(dialog.FileName);
                        Inject(Model);
                    }
                }
            }
        }

        //  -------------------
        //  SaveDocument method
        //  -------------------

        private void SaveDocument() { Save(); }

        //  ---------------------
        //  SaveDocumentAs method
        //  ---------------------

        private void SaveDocumentAs() { SaveAs(); }

        //  -------------
        //  SaveAs method
        //  -------------

        private bool SaveAs()
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = Resources.SaveAsFilter;
                if (dialog.ShowDialog(View) == DialogResult.OK)
                {
                    Model.SaveAs(dialog.FileName);
                }
                else return false;
            }
            return true;
        }

        //  ------------------
        //  SaveChanges method
        //  ------------------

        private bool SaveChanges()
        {
            if (Model.IsDirty)
            {
                var result = MessageBox.Show(
                    View,
                    Resources.QuestionSaveChanges,
                    View.Text,
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1,
                    0);
                if (result == DialogResult.Cancel)
                {
                    return false;
                }
                else if (result == DialogResult.Yes)
                {
                    return Save();
                }
            }
            return true;
        }

        //  -----------
        //  Save method
        //  -----------

        private bool Save()
        {
            if (Model.IsNew)
            {
                return SaveAs();
            }
            else Model.Save();
            return true;
        }

        //  -------------
        //  Inject method
        //  -------------

        public void Inject(StorageDocument dependency)
        {
            View.Inject(dependency);
        }

        //  --------------
        //  Dispose method
        //  --------------

        public void Dispose()
        {
            if (document != null) document.Dispose();
        }

        #region helper methods

        //  -------------
        //  Invoke method
        //  -------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void Invoke(Action action, CancelEventArgs e = null)
        {
            try { action.Invoke(); }
            catch (Exception exception)
            {
                ShowException(exception);
                if (e != null) e.Cancel = true;
            }
        }

        //  --------------------
        //  ShowException method
        //  --------------------

        private void ShowException(Exception exception)
        {
            MessageBox.Show(
                View,
                exception.Message,
                Resources.Exception,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                0);
        }

        #endregion helper methods

        #endregion methods
    }
}

// eof "SnapIn.cs"
