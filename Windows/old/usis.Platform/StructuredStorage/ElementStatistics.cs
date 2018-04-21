//
//  @(#) ElementStatistics.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2008-2016 usis GmbH. All rights reserved.

using System;
using System.Diagnostics;
using usis.Platform.Interop;
using ComTypes = System.Runtime.InteropServices.ComTypes;

namespace usis.Platform.StructuredStorage
{
    //  -----------------------
    //  ElementStatistics class
    //  -----------------------

    /// <summary>
    /// Contains statistical information about an open
    /// storage, stream, or byte-array element.
    /// </summary>

    public class ElementStatistics
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
            Refresh(storage);
        }

        internal ElementStatistics(ComTypes.IStream stream, bool noName)
        {
            Refresh(stream, noName);
        }

        #endregion construction

        #region public properties

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets the name of the storage, stream, or byte-array element.
        /// </summary>

        public string Name
        {
            get { return statstg.pwcsName; }
        }

        //  --------------------
        //  ElementType property
        //  --------------------

        /// <summary>
        /// Gets the type of the storage element.
        /// </summary>

        public ElementType ElementType
        {
            get { return (ElementType)statstg.type; }
        }

        //  -------------
        //  Size property
        //  -------------

        /// <summary>
        /// Gets the size, in bytes of the stream or byte array.
        /// </summary>
                
        public long Size
        {
            get { return statstg.cbSize; }
        }

        //  ----------------
        //  Created property
        //  ----------------

        /// <summary>
        /// Gets the creation time for this storage, stream, or byte array.
        /// </summary>
                
        public DateTime Created
        {
            get { return statstg.ctime.ToDateTime(); }
        }

        //  -----------------
        //  Accessed property
        //  -----------------

        /// <summary>
        /// Gets the last access time for this storage, stream, or byte array.
        /// </summary>
                
        public DateTime Accessed
        {
            get { return statstg.atime.ToDateTime(); }
        }

        //  -----------------
        //  Modified property
        //  -----------------

        /// <summary>
        /// Gets the last modification time for this storage, stream, or byte array.
        /// </summary>

        public DateTime Modified
        {
            get { return statstg.mtime.ToDateTime(); }
        }

        //  -------------
        //  Mode property
        //  -------------

        /// <summary>
        /// Gets the access mode specified when the object was opened.
        /// </summary>
        
        public StorageModes Mode
        {
            get { return (StorageModes)statstg.grfMode; }
        }

        //  -----------------------
        //  LocksSupported property
        //  -----------------------

        /// <summary>
        /// Gets the types of region locking supported by the stream or byte array.
        /// For more information about the values available,
        /// see the <see cref="LockTypes"/> enumeration.
        /// This member is not used for storage objects. 
        /// </summary>
        
        public LockTypes LocksSupported
        {
            get { return (LockTypes)statstg.grfLocksSupported; }
        }

        //  ----------------
        //  ClassId property
        //  ----------------

        /// <summary>
        /// Gets the class identifier (<b>CLSID</b>) for the storage element.
        /// </summary>
        
        public Guid ClassId
        {
            get { return statstg.clsid; }
            internal set { statstg.clsid = value; }
        }

        //  ------------------
        //  StateBits property
        //  ------------------

        /// <summary>
        /// Gets the current state bits of the storage object;
        /// that is, the value most recently set by the SetStateBits method.
        /// This member is not valid for streams or byte arrays.
        /// </summary>
        
        public int StateBits
        {
            get { return statstg.grfStateBits; }
        }

        //  -----------------
        //  Reserved property
        //  -----------------

        /// <summary>
        /// Reserved for future use.
        /// </summary>
                
        public int Reserved
        {
            get { return statstg.reserved; }
        }

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

        public override string ToString()
        {
            return Name;
        }

        #endregion overrides

        #region internal methods

        //  --------------
        //  Refresh method
        //  --------------
        
        internal void Refresh(IStorage storage)
        {
            Debug.Assert(storage != null);
            storage.Stat(out statstg, (int)STATFLAG.DEFAULT);
        }

        internal void Refresh(ComTypes.IStream stream, bool noName)
        {
            Debug.Assert(stream != null);
            stream.Stat(out statstg, (int)(noName ? STATFLAG.DEFAULT : STATFLAG.NONAME));
        }

        #endregion internal methods
    }
}

// eof "ElementStatistics.cs"
