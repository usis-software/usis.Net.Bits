//
//  @(#) DocumentWorkspace.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 12
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014,2015 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Windows;

namespace usis.Windows.Framework
{
    //  -----------------------
    //  DocumentWorkspace class
    //  -----------------------

    public class DocumentWorkspace
    {
        #region fields

        private List<IDocument> documents = new List<IDocument>();

        #endregion fields

        #region events

        public event EventHandler<DocumentEventArgs> DocumentOpened;
        public event EventHandler<DocumentCancelEventArgs> DocumentClosing;
        public event EventHandler<DocumentEventArgs> DocumentClosed;

        #endregion events

        #region public methods

        #region OpenDocument method

        //  -------------------
        //  OpenDocument method
        //  -------------------

        public void OpenDocument(IDocument document)
        {
            if (document == null) throw new ArgumentNullException("document");

            if (!this.documents.Contains(document))
            {
                this.documents.Add(document);

                if (this.DocumentOpened != null)
                {
                    this.DocumentOpened(this, new DocumentEventArgs(document));
                }
            }

        } // OpenDocument method

        #endregion OpenDocument method

        #region CloseDocument method

        //  --------------------
        //  CloseDocument method
        //  --------------------

        public bool CloseDocument(IDocument document)
        {
            if (document == null) throw new ArgumentNullException("document");

            if (this.documents.Contains(document))
            {
                if (this.DocumentClosing != null)
                {
                    var e = new DocumentCancelEventArgs(document);
                    this.DocumentClosing(this, e);
                    if (e.Cancel) return false;
                }
                this.documents.Remove(document);
                if (this.DocumentClosed != null)
                {
                    this.DocumentClosed(this, new DocumentEventArgs(document));
                }
                return true;
            }
            else return false;

        } // CloseDocument method

        #endregion CloseDocument method

        #region CloseAllDocuments method

        //  ------------------------
        //  CloseAllDocuments method
        //  ------------------------

        public bool CloseAllDocuments()
        {
            foreach (var document in new List<IDocument>(this.documents))
            {
                if (!this.CloseDocument(document))
                {
                    return false;
                }
            }
            return true;

        } // CloseAllDocuments method

        #endregion CloseAllDocuments method

        #endregion public methods

        #region public properties

        //	----------------
        //	Current property
        //	----------------

        public static DocumentWorkspace Current
        {
            get
            {
                return Application.Current.CurrentWorkspace();
            }
        }

        #endregion public properties

    } // DocumentWorkspace class

    #region ApplicationExtension class

    //  --------------------------
    //  ApplicationExtension class
    //  --------------------------

    public static class ApplicationExtension
    {
        private const string workspacePropertyName = "usisWorkspace";

        //  ------------------------
        //  RegisterWorkspace method
        //  ------------------------

        public static void RegisterWorkspace(this Application application, DocumentWorkspace workspace)
        {
            if (application == null) throw new ArgumentNullException("application");

            application.Properties[workspacePropertyName] = workspace;

        } // RegisterWorkspace method

        //  -----------------------
        //  CurrentWorkspace method
        //  -----------------------

        public static DocumentWorkspace CurrentWorkspace(this Application application)
        {
            return application.GetProperty<DocumentWorkspace>(workspacePropertyName);

        } // CurrentWorkspace method

    } // ApplicationExtension class

    #endregion ApplicationExtension class

} // usis.Windows.Framework namespace

// eof "DocumentWorkspace.cs"
