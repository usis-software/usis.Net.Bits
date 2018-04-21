//
//  @(#) Document.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 12
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014,2015 usis GmbH. All rights reserved.

using System;
using System.ComponentModel;

namespace usis.Windows.Framework
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

        string Title
        {
            get;
        }

        //  ----------------
        //  IsDirty property
        //  ----------------

        bool IsDirty
        {
            get;
        }

    } // IDocument interface

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

        public IDocument Document
        {
            get;
            private set;
        }

        //  -----------
        //  constructor
        //  -----------

        public DocumentEventArgs(IDocument document)
        {
            if (document == null) throw new ArgumentNullException("document");

            this.Document = document;

        } // constructor

    } // DocumentEventArgs class

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

        public IDocument Document
        {
            get;
            private set;
        }

        //  -----------
        //  constructor
        //  -----------

        public DocumentCancelEventArgs(IDocument document)
        {
            if (document == null) throw new ArgumentNullException("document");

            this.Document = document;

        } // constructor

    } // DocumentCancelEventArgs class

    #endregion DocumentCancelEventArgs class

    #region DocumentBase class

    //  ------------------
    //  DocumentBase class
    //  ------------------

    public abstract class DocumentBase : ModelBase, IDocument
    {
        #region properties

        //  --------------
        //  Title property
        //  --------------

        public string Title
        {
            get;
            protected set;
        }

        //  ----------------
        //  isDirty property
        //  ----------------

        private bool isDirty;

        public bool IsDirty
        {
            get
            {
                return this.OnIsDirty();
            }
        }

        #endregion properties

        #region virtual methods

        //  ---------------
        //  SetDirty method
        //  ---------------

        public virtual void SetDirty()
        {
            this.isDirty = true;
        }

        //  ---------------
        //  SetClean method
        //  ---------------

        public virtual void SetClean()
        {
            this.isDirty = false;
        }

        //  ----------------
        //  OnIsDirty method
        //  ----------------

        protected virtual bool OnIsDirty()
        {
            return this.isDirty;
        }

        #endregion virtual methods

    } // DocumentBase class

    #endregion DocumentBase class

} // usis.Windows.Framework namespace

// eof "Document.cs"
