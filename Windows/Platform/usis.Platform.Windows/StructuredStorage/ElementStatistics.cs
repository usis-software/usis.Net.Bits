//
//  @(#) ElementStatistics.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2008-2017 usis GmbH. All rights reserved.

using System;
using System.Globalization;
using usis.Platform.Interop;
using ComTypes = System.Runtime.InteropServices.ComTypes;

namespace usis.Platform.StructuredStorage
{
    //  -----------------------
    //  ElementStatistics class
    //  -----------------------

    /// <summary>
    /// Contains statistical information about an storage, stream, or byte-array element.
    /// </summary>

    public sealed class ElementStatistics
    {
        #region private members

        private ComTypes.STATSTG statstg;

        #endregion private members

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal ElementStatistics(ComTypes.STATSTG statistics)
        {
            statstg = statistics;
        }

        internal ElementStatistics(IStorage storage)
        {
            storage.Stat(out statstg, (int)STATFLAG.DEFAULT);
        }

        internal ElementStatistics(StorageStream stream, bool noName)
        {
            stream.Stat(out statstg, noName ? STATFLAG.DEFAULT : STATFLAG.NONAME);
        }

        #endregion construction

        #region public properties

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets the name of the storage, stream, or byte-array.
        /// </summary>

        public string Name => statstg.pwcsName;

        //  --------------------
        //  ElementType property
        //  --------------------

        /// <summary>
        /// Gets the type of the storage element.
        /// </summary>

        public ElementType ElementType => (ElementType)statstg.type;

        //  -------------
        //  Size property
        //  -------------

        /// <summary>
        /// Gets the size, in bytes of the stream or byte-array.
        /// </summary>

        public long Size => statstg.cbSize;

        //  ----------------
        //  Created property
        //  ----------------

        /// <summary>
        /// Gets the creation time for this storage element.
        /// </summary>

        public DateTime Created => statstg.ctime.ToDateTime();

        //  -----------------
        //  Accessed property
        //  -----------------

        /// <summary>
        /// Gets the last access time for this storage element.
        /// </summary>

        public DateTime Accessed => statstg.atime.ToDateTime();

        //  -----------------
        //  Modified property
        //  -----------------

        /// <summary>
        /// Gets the last modification time for this storage element.
        /// </summary>

        public DateTime Modified => statstg.mtime.ToDateTime();

        //  -------------
        //  Mode property
        //  -------------

        /// <summary>
        /// Gets the access mode specified when the storage element was opened.
        /// </summary>

        public StorageModes Mode => (StorageModes)statstg.grfMode;

        //  -----------------------
        //  LocksSupported property
        //  -----------------------

        /// <summary>
        /// Gets the types of region locking supported by the stream or byte-array.
        /// For more information about the values available,
        /// see the <see cref="LockTypes"/> enumeration.
        /// This member is not used for storage objects. 
        /// </summary>

        public LockTypes LocksSupported => (LockTypes)statstg.grfLocksSupported;

        //  ----------------
        //  ClassId property
        //  ----------------

        /// <summary>
        /// Gets the class identifier (<b>CLSID</b>) for the storage element.
        /// </summary>

        public Guid ClassId
        {
            get => statstg.clsid;
            internal set => statstg.clsid = value;
        }

        //  ------------------
        //  StateBits property
        //  ------------------

        /// <summary>
        /// Gets the current state bits of the storage object;
        /// that is, the value most recently set by the SetStateBits method.
        /// This member is not valid for streams or byte arrays.
        /// </summary>

        [Obsolete("Reserved for future use.")]
        public int StateBits => statstg.grfStateBits;

        //  -----------------
        //  Reserved property
        //  -----------------

        /// <summary>
        /// Reserved for future use.
        /// </summary>

        [Obsolete("Reserved for future use.")]
        public int Reserved => statstg.reserved;

        #endregion public properties

        #region overrides

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Creates a human-readable string that represents this element.
        /// </summary>
        /// <returns>
        /// A string that represents this element.
        /// </returns>

        public override string ToString() { return string.Format(CultureInfo.CurrentCulture, "Name = \"{0}\"", Name); }

        #endregion overrides
    }

    #region IStatistics interface

    //  ---------------------
    //  IStatistics interface
    //  ---------------------

    /// <summary>
    /// Describes the basic properties of a storage element.
    /// </summary>

    public interface IStatistics
    {
        //  --------------------
        //  ElementType property
        //  --------------------

        /// <summary>
        /// Gets the type of the storage element.
        /// </summary>

        ElementType ElementType { get; }

        //  ----------------
        //  Created property
        //  ----------------

        /// <summary>
        /// Gets the creation time for this storage element.
        /// </summary>

        DateTime Created { get; }

        //  -----------------
        //  Accessed property
        //  -----------------

        /// <summary>
        /// Gets the last access time for this storage element.
        /// </summary>

        DateTime Accessed { get; }

        //  -----------------
        //  Modified property
        //  -----------------

        /// <summary>
        /// Gets the last modification time for this storage element.
        /// </summary>

        DateTime Modified { get; }
    }

    #endregion IStatistics interface
}

// eof "ElementStatistics.cs"
