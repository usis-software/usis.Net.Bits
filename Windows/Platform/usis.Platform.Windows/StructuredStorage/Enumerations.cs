//
//  @(#) Enumerations.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2008-2017 usis GmbH. All rights reserved.

using System;

namespace usis.Platform.StructuredStorage
{
    #region StorageModes enumeration

    //  ------------------------
    //  StorageModes enumeration
    //  ------------------------

    /// <summary>
    /// Provides flags that indicate conditions for creating and deleting
    /// a storage element and access modes for the storage element.
    /// </summary>

    [Flags]
    public enum StorageModes : int
    {
        /// <summary>
        /// No flags set.
        /// </summary>
        
        None = 0x00000000,
        
        /// <summary>
        /// Indicates that, in transacted mode, changes are buffered and written
        /// only if an explicit commit operation is called.
        /// </summary>
        
        Transacted = 0x00010000,
        
        /// <summary>
        /// Provides a faster implementation of a compound file in a limited,
        /// but frequently used, case.
        /// </summary>
        
        Simple = 0x08000000,
        
        /// <summary>
        /// Enables you to save changes to a storage element,
        /// but does not permit access to its data.
        /// </summary>
        
        Write = 0x00000001,

        /// <summary>
        /// Enables access and modification of a storage element.
        /// </summary>

        ReadWrite = 0x00000002,

        /// <summary>
        /// Specifies that subsequent openings of a storage element
        /// are not denied read or write access.
        /// </summary>

        ShareDenyNone = 0x00000040,

        /// <summary>
        /// Prevents others from subsequently opening a storage element in read mode.
        /// It is typically used on a root storage.
        /// </summary>

        ShareDenyRead = 0x00000030,

        /// <summary>
        /// Prevents others from subsequently opening a storage element
        /// for write or read/write access.
        /// </summary>

        ShareDenyWrite = 0x00000020,

        /// <summary>
        /// Prevents others from subsequently opening a storage element in any mode.
        /// </summary>

        ShareExclusive = 0x00000010,

        /// <summary>
        /// Opens a storage element with exclusive access
        /// to the most recently committed version.
        /// </summary>

        Priority = 0x00040000,

        /// <summary>
        /// Indicates that the underlying file is to be automatically destroyed
        /// when the root storage is released.
        /// </summary>

        DeleteOnRelease = 0x04000000,

        /// <summary>
        /// Indicates that, in transacted mode, a temporary scratch file is usually used
        /// to save modifications until the <see cref="Storage.Commit(CommitConditions)"/> method is called.
        /// Specifying <see cref="NoScratch"/> permits the unused portion of the original file to be used as work space
        /// instead of creating a new file for that purpose.
        /// This does not affect the data in the original file, and in certain cases can result in improved performance.
        /// It is not valid to specify this flag without also specifying <see cref="Transacted"/>,
        /// and this flag may only be used in a root open.
        /// </summary>

        NoScratch = 0x00100000,

        /// <summary>
        /// Indicates that an existing storage or stream should be removed
        /// before a new storage element replaces it.
        /// </summary>

        Create = 0x00001000,
        
        /// <summary>
        /// Creates a storage element while preserving existing data in a stream named "Contents".
        /// </summary>
        
        Convert = 0x00020000,

        /// <summary>
        /// This flag is used when opening a storage with <see cref="Transacted"/>
        /// and without <see cref="ShareExclusive"/> or <see cref="ShareDenyWrite"/>.
        /// In this case, specifying <see cref="NoSnapshot"/> prevents the
        /// system-provided implementation from creating a snapshot copy of the file.
        /// </summary>

        NoSnapshot = 0x00200000,
        
        /// <summary>
        /// Supports direct mode for single-writer, multireader file operations.
        /// </summary>
        
        DirectSingleWriterMultireader = 0x00400000,
    }

    #endregion StorageModes enumeration

    #region STATFLAG enumeration

    //  --------------------
    //  STATFLAG enumeration
    //  --------------------

    internal enum STATFLAG : int
    {
        DEFAULT = 0,
        NONAME = 1,
        NOOPEN =2
    }

    #endregion STATFLAG enumeration

    #region CommitConditions enumeration

    //  ----------------------------
    //  CommitConditions enumeration
    //  ----------------------------

    /// <summary>
    /// Specify the conditions for performing the commit operation
    /// in the
    /// <see cref="Storage"/>.<see cref="Storage.Commit(CommitConditions)"/>
    /// and
    /// <see cref="StorageStream"/>.<see cref="StorageStream.Commit(CommitConditions)"/> methods.
    /// </summary>

    [Flags]
    public enum CommitConditions : int
    {
        /// <summary>
        /// (Default) You can specify this condition with
        /// <see cref="Consolidate"/>,
        /// or some combination of the other three flags
        /// in this enumeration.
        /// Use this value to increase the readability of code.
        /// </summary>
        
        None = 0,

        /// <summary>
        /// The commit operation can overwrite existing data to reduce
        /// overall space requirements. This value is not recommended
        /// for typical usage because it is not as robust as the
        /// default value. In this case, it is possible for the
        /// commit operation to fail after the old data is overwritten,
        /// but before the new data is completely committed.
        /// Then, neither the old version nor the new version
        /// of the storage object will be intact.
        /// </summary>
        
        Overwrite = 1,

        /// <summary>
        /// Prevents multiple users of a storage object
        /// from overwriting each other's changes.
        /// </summary>
        
        OnlyIfCurrent = 2,

        /// <summary>
        /// Commits the changes to a write-behind disk cache,
        /// but does not save the cache to the disk.
        /// </summary>
        
        DangerouslyCommitMerelyToDiskCache = 4,

        /// <summary>
        /// Windows 2000 and Windows XP:
        /// Indicates that a storage should be consolidated after it is committed,
        /// resulting in a smaller file on disk.
        /// This flag is valid only on the outermost storage object
        /// that has been opened in transacted mode.
        /// It is not valid for streams.
        /// The <see cref="Consolidate"/> flag can be combined
        /// with any other <see cref="CommitConditions"/> flags.
        /// </summary>
        
        Consolidate = 8
    }

    #endregion CommitConditions enumeration

    #region ElementType enumeration

    //  -----------------------
    //  ElementType enumeration
    //  -----------------------

    /// <summary>
    /// Indicates the type of a storage element.
    /// </summary>

    public enum ElementType : int
    {
        /// <summary>
        /// No storage element type specified.
        /// </summary>
        
        None = 0,
        
        /// <summary>
        /// Indicates that the storage element is a storage object.
        /// </summary>
        
        Storage = 1,
        
        /// <summary>
        /// Indicates that the storage element is a stream object.
        /// </summary>
        
        Stream = 2,
        
        /// <summary>
        /// Indicates that the storage element is a byte-array object.
        /// </summary>
        
        ByteArray = 3,
        
        /// <summary>
        /// Indicates that the storage element is a property set object.
        /// </summary>
        
        PropertySet = 4
    }

    #endregion ElementType enumeration

    #region PropertySetCharacteristics enumeration

    //  --------------------------------------
    //  PropertySetCharacteristics enumeration
    //  --------------------------------------

    /// <summary>
    /// Provides values to define characteristics of a property set.
    /// </summary>

    [Flags]
    public enum PropertySetCharacteristics : int
    {
        /// <summary>
        /// Nothing specified.
        /// </summary>
        
        None = 0,
        
        /// <summary>
        /// If specified, nonsimple property values can be written
        /// to the property set and the property set is saved in a storage object.
        /// </summary>
        
        NoSimple = 1,
        
        /// <summary>
        /// If specified, all string values in the property set that are not
        /// explicitly Unicode are stored with the current system ANSI code page.
        /// </summary>
        
        Ansi = 2,
        
        /// <summary>
        /// If specified changes to the property set are not buffered.
        /// </summary>
        
        Unbuffered = 4,
        
        /// <summary>
        /// If specified, property names are case sensitive.
        /// </summary>
        
        CaseSensitive = 8
    }

    #endregion PropertySetCharacteristics enumeration

    #region LockTypes enumeration

    //  ---------------------
    //  LockTypes enumeration
    //  ---------------------

    /// <summary>
    /// Provides flags that indicate the type of locking requested
    /// for the specified range of bytes
    /// </summary>

    [Flags]
    public enum LockTypes : int
    {
        /// <summary>
        /// No locking requested.
        /// </summary>
        
        None = 0,
        
        /// <summary>
        /// If this lock is granted, the specified range of bytes
        /// can be opened and read any number of times,
        /// but writing to the locked range is prohibited except
        /// for the owner that was granted this lock.
        /// </summary>
        
        Write = 1,
        
        /// <summary>
        /// If this lock is granted, writing to the specified range of bytes
        /// is prohibited except by the owner that was granted this lock.
        /// </summary>
        
        Exclusive = 2,
        
        /// <summary>
        /// If this lock is granted, no other <see cref="OnlyOnce"/> lock
        /// can be obtained on the range.
        /// Usually this lock type is an alias
        /// for some other lock type.
        /// Thus, specific implementations can have additional
        /// behavior associated with this lock type.
        /// </summary>
        
        OnlyOnce = 4
    }

    #endregion LockTypes enumeration
}

// eof "Enumerations.cs"
