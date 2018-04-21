//
//  @(#) Document.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.ComponentModel;

namespace usis.Framework
{
    #region IDocument interface

    //  -------------------
    //  IDocument interface
    //  -------------------

    public interface IDocument
    {
        //  --------------
        //  Title property
        //  --------------

        string Title { get; }

        //  ----------------
        //  IsDirty property
        //  ----------------

        bool IsDirty { get; }
    }

    #endregion IDocument interface

    #region DocumentEventArgs class

    //  -----------------------
    //  DocumentEventArgs class
    //  -----------------------

    public class DocumentEventArgs : EventArgs
    {
        //  -----------------
        //  Document property
        //  -----------------

        public IDocument Document { get; }

        //  ------------
        //  construction
        //  ------------

        public DocumentEventArgs(IDocument document)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));
        }
    }

    #endregion DocumentEventArgs class

    #region DocumentCancelEventArgs class

    //  -----------------------------
    //  DocumentCancelEventArgs class
    //  -----------------------------

    public class DocumentCancelEventArgs : CancelEventArgs
    {
        //  -----------------
        //  Document property
        //  -----------------

        public IDocument Document { get; }

        //  ------------
        //  construction
        //  ------------

        public DocumentCancelEventArgs(IDocument document)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));
        }
    }

    #endregion DocumentCancelEventArgs class

    #region Document class

    //  --------------
    //  Document class
    //  --------------

    public abstract class Document : IDocument
    {
        #region properties

        //  --------------
        //  Title property
        //  --------------

        public string Title { get; protected set; }

        //  ----------------
        //  isDirty property
        //  ----------------

        private bool isDirty;

        public bool IsDirty => OnIsDirty();

        #endregion properties

        #region virtual methods

        //  ---------------
        //  SetDirty method
        //  ---------------

        public virtual void SetDirty() { isDirty = true; }

        //  ---------------
        //  SetClean method
        //  ---------------

        public virtual void SetClean() { isDirty = false; }

        //  ----------------
        //  OnIsDirty method
        //  ----------------

        protected virtual bool OnIsDirty() { return isDirty; }

        #endregion virtual methods
    }

    #endregion Document class
}

// eof "Document.cs"
