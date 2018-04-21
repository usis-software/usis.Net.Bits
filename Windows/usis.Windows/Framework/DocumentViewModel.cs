//
//  @(#) DocumentViewModel.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 12
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014,2015 usis GmbH. All rights reserved.

using System;
using System.Windows;

namespace usis.Windows.Framework
{
    #region IDocumentViewModel interface

    //  ----------------------------
    //  IDocumentViewModel interface
    //  ----------------------------

    public interface IDocumentViewModel
    {
        IDocument Document
        {
            get;
        }

    } // IDocumentViewModel

    #endregion IDocumentViewModel interface

    #region DocumentViewModel class

    //  -----------------------
    //  DocumentViewModel class
    //  -----------------------

    internal class DocumentViewModel : IDocumentViewModel
    {
        #region properties

        //  -----------------
        //  Document property
        //  -----------------

        public IDocument Document
        {
            get;
            private set;
        }

        #endregion properties

        #region construction

        //  -----------
        //  constructor
        //  -----------

        public DocumentViewModel(IDocument document)
        {
            if (document == null) throw new ArgumentNullException("document");

            this.Document = document;

        } // constructor

        #endregion construction

    } // DocumentViewModel class

    #endregion DocumentViewModel class

} // usis.Windows.Framework namespace

// eof "DocumentViewModel.cs"
