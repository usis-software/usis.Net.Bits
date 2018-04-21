//
//  @(#) DocumentWorkspaceViewModel.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 12
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014,2015 usis GmbH. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace usis.Windows.Framework
{
    //  --------------------------------
    //  DocumentWorkspaceViewModel class
    //  --------------------------------

    public class DocumentWorkspaceViewModel : DependencyObject
    {
        #region fields

        private readonly DocumentWorkspace workspace = Application.Current.CurrentWorkspace();
        private readonly ObservableCollection<IDocumentViewModel> documents = new ObservableCollection<IDocumentViewModel>();
        private IDocumentViewModel activeDocument;

        #endregion fields

        #region properties

        //  ------------------
        //  Documents property
        //  ------------------

        public ObservableCollection<IDocumentViewModel> Documents
        {
            get
            {
                return this.documents;
            }

        } // Documents property

        //  ------------------
        //  Workspace property
        //  ------------------

        public DocumentWorkspace Workspace
        {
            get
            {
                return this.workspace;
            }
        }

        //  -----------------------
        //  ActiveDocument property
        //  -----------------------

        public IDocumentViewModel ActiveDocument
        {
            get
            {
                return this.activeDocument;
            }
            set
            {
                if (this.activeDocument != value)
                {
                    this.activeDocument = value;
                    this.OnActiveDocumentChanged();
                }
            }

        } // ActiveDocument property

        //  ---------------------------
        //  NewDocumentCommand property
        //  ---------------------------

        public Command NewDocumentCommand
        {
            get;
            private set;

        } // NewDocumentCommand property

        //  ----------------------------
        //  OpenDocumentCommand property
        //  ----------------------------

        public Command OpenDocumentCommand
        {
            get;
            private set;

        } // OpenDocumentCommand property

        //  -----------------------------------
        //  CloseActiveDocumentCommand property
        //  -----------------------------------

        public Command CloseActiveDocumentCommand
        {
            get;
            private set;

        } // CloseActiveDocumentCommand property

        //  ----------------------------------
        //  SaveActiveDocumentCommand property
        //  ----------------------------------

        public Command SaveActiveDocumentCommand
        {
            get;
            set;

        } // SaveActiveDocumentCommand property

        //  ------------------------------------
        //  SaveActiveDocumentAsCommand property
        //  ------------------------------------

        public Command SaveActiveDocumentAsCommand
        {
            get;
            set;

        } // SaveActiveDocumentAsCommand property

        //  -----------------------------------
        //  PrintActiveDocumentCommand property
        //  -----------------------------------

        public Command PrintActiveDocumentCommand
        {
            get;
            set;

        } // PrintActiveDocumentCommand property

        //  --------------------
        //  ExitCommand property
        //  --------------------

        public Command ExitCommand
        {
            get;
            private set;

        } // ExitCommand property

        #endregion properties

        #region virtual methods

        //  ------------------------------
        //  CreateDocumentViewModel method
        //  ------------------------------

        protected virtual IDocumentViewModel CreateDocumentViewModel(IDocument document)
        {
            return new DocumentViewModel(document);

        } // CreateDocumentViewModel method

        //  ------------------------------
        //  OnActiveDocumentChanged method
        //  ------------------------------

        protected virtual void OnActiveDocumentChanged()
        {
            bool hasActiveDocument = this.activeDocument != null;

            this.CloseActiveDocumentCommand.Enabled = hasActiveDocument;
            this.SaveActiveDocumentCommand.Enabled = hasActiveDocument;
            this.SaveActiveDocumentAsCommand.Enabled = hasActiveDocument;
            this.PrintActiveDocumentCommand.Enabled = hasActiveDocument;

        } // OnActiveDocumentChanged method

        #region On...Command methods

        //  ---------------------------
        //  OnNewDocumentCommand method
        //  ---------------------------

        protected virtual void OnNewDocumentCommand()
        {
        } // OnNewDocumentCommand method

        //  ----------------------------
        //  OnOpenDocumentCommand method
        //  ----------------------------

        protected virtual void OnOpenDocumentCommand()
        {
        } // OnOpenDocumentCommand method

        //  -----------------------------------
        //  OnCloseActiveDocumentCommand method
        //  -----------------------------------

        protected virtual void OnCloseActiveDocumentCommand()
        {
            this.workspace.CloseDocument(this.ActiveDocument.Document);

        } // OnCloseActiveDocumentCommand method

        //  ----------------------------------
        //  OnSaveActiveDocumentCommand method
        //  ----------------------------------

        protected virtual void OnSaveActiveDocumentCommand()
        {
        } // OnSaveActiveDocumentCommand method

        //  ------------------------------------
        //  OnSaveActiveDocumentAsCommand method
        //  ------------------------------------

        protected virtual void OnSaveActiveDocumentAsCommand()
        {
        } // OnSaveActiveDocumentAsCommand method

        //  -----------------------------------
        //  OnPrintActiveDocumentCommand method
        //  -----------------------------------

        protected virtual void OnPrintActiveDocumentCommand()
        {
        } // OnPrintActiveDocumentCommand method

        //  --------------------
        //  OnExitCommand method
        //  --------------------

        protected virtual void OnExitCommand()
        {
            if (this.Workspace.CloseAllDocuments())
            {
                Application.Current.Shutdown();
            }

        } // OnExitCommand method

        #endregion On...Command methods

        //  -----------------------
        //  CanCloseDocument method
        //  -----------------------

        protected virtual bool CanCloseDocument(IDocumentViewModel documentViewModel)
        {
            if (documentViewModel == null) throw new ArgumentNullException("documentViewModel");

            return documentViewModel.Document.IsDirty ? this.ShouldSaveDocumentChanges(documentViewModel) : true;

        } // CanCloseDocument method

        //  --------------------------------
        //  ShouldSaveDocumentChanges method
        //  --------------------------------

        protected virtual bool ShouldSaveDocumentChanges(IDocumentViewModel documentViewModel)
        {
            return true;

        } // ShouldSaveDocumentChanges method

        #endregion  virtual methods

        #region construction

        //  -----------
        //  constructor
        //  -----------

        public DocumentWorkspaceViewModel()
        {
            this.workspace.DocumentOpened += workspace_DocumentOpened;
            this.workspace.DocumentClosing += workspace_DocumentClosing;
            this.workspace.DocumentClosed += workspace_DocumentClosed;

            this.InitializeCommands();

        } // constructor

        #endregion construction

        #region event handlers

        //  -------------------------------
        //  workspace_DocumentOpened method
        //  -------------------------------

        private void workspace_DocumentOpened(object sender, DocumentEventArgs e)
        {
            if (this.FindDocumentViewModel(e.Document) == null)
            {
                var document = this.CreateDocumentViewModel(e.Document);
                this.documents.Add(document);
            }

        } // workspace_DocumentOpened method

        //  --------------------------------
        //  workspace_DocumentClosing method
        //  --------------------------------

        private void workspace_DocumentClosing(object sender, DocumentCancelEventArgs e)
        {
            var documentViewModel = this.FindDocumentViewModel(e.Document);
            if (documentViewModel != null)
            {
                e.Cancel = !this.CanCloseDocument(documentViewModel);
            }

        } // workspace_DocumentClosing method

        //  -------------------------------
        //  workspace_DocumentClosed method
        //  -------------------------------

        private void workspace_DocumentClosed(object sender, DocumentEventArgs e)
        {
            var document = this.FindDocumentViewModel(e.Document);
            if (document != null)
            {
                this.documents.Remove(document);
            }

        } // workspace_DocumentClosed method

        //  ----------------------------
        //  OpenDocumentCommand_Executed
        //  ----------------------------

        private void OpenDocumentCommand_Executed(object sender, EventArgs e)
        {
            this.OnOpenDocumentCommand();

        } // OpenDocumentCommand_Executed

        //  ----------------------------------
        //  NewDocumentCommand_Executed method
        //  ----------------------------------

        private void NewDocumentCommand_Executed(object sender, EventArgs e)
        {
            this.OnNewDocumentCommand();

        } // NewDocumentCommand_Executed method

        //  ------------------------------------------
        //  CloseActiveDocumentCommand_Executed method
        //  ------------------------------------------

        private void CloseActiveDocumentCommand_Executed(object sender, EventArgs e)
        {
            this.OnCloseActiveDocumentCommand();

        } // CloseActiveDocumentCommand_Executed method

        //  -----------------------------------------
        //  SaveActiveDocumentCommand_Executed method
        //  -----------------------------------------

        private void SaveActiveDocumentCommand_Executed(object sender, EventArgs e)
        {
            this.OnSaveActiveDocumentCommand();

        } // SaveActiveDocumentCommand_Executed method

        //  -------------------------------------------
        //  SaveActiveDocumentAsCommand_Executed method
        //  -------------------------------------------

        private void SaveActiveDocumentAsCommand_Executed(object sender, EventArgs e)
        {
            this.OnSaveActiveDocumentAsCommand();

        } // SaveActiveDocumentAsCommand_Executed method

        //  ------------------------------------------
        //  PrintActiveDocumentCommand_Executed method
        //  ------------------------------------------

        private void PrintActiveDocumentCommand_Executed(object sender, EventArgs e)
        {
            this.OnPrintActiveDocumentCommand();

        } // PrintActiveDocumentCommand_Executed method

        //  ---------------------------
        //  ExitCommand_Executed method
        //  ---------------------------

        private void ExitCommand_Executed(object sender, EventArgs e)
        {
            this.OnExitCommand();

        } // ExitCommand_Executed method

        #endregion event handlers

        #region private methods

        //  -------------------------
        //  InitializeCommands method
        //  -------------------------

        private void InitializeCommands()
        {
            this.ExitCommand = new Command();
            this.ExitCommand.Executed += ExitCommand_Executed;

            this.OpenDocumentCommand = new Command();
            this.OpenDocumentCommand.Executed += OpenDocumentCommand_Executed;

            this.CloseActiveDocumentCommand = new Command(false);
            this.CloseActiveDocumentCommand.Executed += CloseActiveDocumentCommand_Executed;

            this.NewDocumentCommand = new Command();
            this.NewDocumentCommand.Executed += NewDocumentCommand_Executed;

            this.SaveActiveDocumentCommand = new Command(false);
            this.SaveActiveDocumentCommand.Executed += SaveActiveDocumentCommand_Executed;

            this.SaveActiveDocumentAsCommand = new Command(false);
            this.SaveActiveDocumentAsCommand.Executed += SaveActiveDocumentAsCommand_Executed;

            this.PrintActiveDocumentCommand = new Command(false);
            this.PrintActiveDocumentCommand.Executed += PrintActiveDocumentCommand_Executed;

        } // InitializeCommands method

        //  -----------------------------
        //  FindDocumentViewModel methods
        //  -----------------------------

        private IDocumentViewModel FindDocumentViewModel(IDocument document)
        {
            foreach (var item in this.documents)
            {
                if (item.Document.Equals(document))
                {
                    return item;
                }
            }
            return null;

        } // FindDocumentViewModel methods

        #endregion private methods

    } // DocumentWorkspaceViewModel

} // usis.Windows.Framework namespace

// eof "DocumentWorkspaceViewModel.cs"
