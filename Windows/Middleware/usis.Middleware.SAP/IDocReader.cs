//
//  @(#) IDocReader.cs
//
//  Project:    usis Middleware
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;

namespace usis.Middleware.SAP
{
    #region IDocReaderOptions enumeration

    //  -----------------------------
    //  IDocReaderOptions enumeration
    //  -----------------------------

    /// <summary>
    /// Provides optional settings to IDoc readers.
    /// </summary>

    [Flags]
    public enum IDocReaderOptions
    {
        /// <summary>
        /// No options set.
        /// </summary>

        None = 0,

        /// <summary>
        /// Specifies to ignore the IDoc hierarchy level field (<c>HLEVEL</c>).
        /// </summary>

        IgnoreHierarchyLevel = 1
    }

    #endregion IDocReaderOptions enumeration

    //  ----------------
    //  IDocReader class
    //  ----------------

    /// <summary>
    /// Provides a base class for IDoc readers.
    /// </summary>

    public abstract class IDocReader
    {
        //  ----------------
        //  Options property
        //  ----------------

        /// <summary>
        /// Gets or sets IDoc reader options.
        /// </summary>
        /// <value>
        /// The optional settings used to read IDocs from a reader.
        /// </value>

        public IDocReaderOptions Options { get; protected set; }

        //  -----------------------------
        //  CurrentControlRecord property
        //  -----------------------------

        /// <summary>
        /// Gets the control record of the current IDoc.
        /// </summary>
        /// <value>
        /// The control record.
        /// </value>

        public IDocControlRecord CurrentControlRecord { get; protected set; }

        //  --------------------------
        //  CurrentDataRecord property
        //  --------------------------

        /// <summary>
        /// Gets or sets the current data record.
        /// </summary>
        /// <value>
        /// The current data record.
        /// </value>

        public IDocDataRecord CurrentDataRecord { get; protected set; }

        //  -----------
        //  Read method
        //  -----------

        /// <summary>
        /// Reads the next the segment from the reader.
        /// </summary>
        /// <returns>
        /// <c>true</c> if a segment was read; otherwise <c>false</c>.
        /// </returns>

        public abstract bool Read();
    }
}

// eof "IDocReader.cs"
