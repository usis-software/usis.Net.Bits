//
//  @(#) IStorage.cs - internal interface
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2008-2016 usis GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using ComTypes = System.Runtime.InteropServices.ComTypes;

namespace usis.Platform.Interop
{
    //  ------------------
    //  IStorage interface
    //  ------------------

    /// <summary>
    /// Supports the creation and management of structured storage objects.
    /// Structured storage allows hierarchical storage of information
    /// within a single file,
    /// and is often referred to as 'a file system within a file'.
    /// </summary>
    /// <remarks>
    /// Elements of a structured storage object are storages and streams.
    /// Storages are analogous to directories,
    /// and streams are analogous to files.
    /// Within a structured storage there will be a primary storage object
    /// that may contain substorages, possibly nested, and streams.
    /// Storages provide the structure of the object,
    /// and streams contain the data, which is manipulated through the
    /// <see cref="ComTypes.IStream"/>
    /// interface.
    /// The <b>IStorage</b> interface provides methods for creating and
    /// managing the root storage object, child storage objects,
    /// and stream objects. These methods can create, open, enumerate,
    /// move, copy, rename, or delete the elements in the storage object.
    /// </remarks>

    [ComImport]
    [Guid(IID.IStorage)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IStorage
    {
        //  -------------------
        //  CreateStream method
        //  -------------------

        /// <summary>
        /// Creates and opens a stream object with the specified name
        /// contained in this storage object.
        /// All elements within a storage objects,
        /// both streams and other storage objects,
        /// are kept in the same name space.
        /// </summary>
        /// <param name="name">
        /// The name of the newly created stream.
        /// The name can be used later to open or reopen the stream.
        /// The name must not exceed 31 characters in length.
        /// The 0x0 through 0x1f characters,
        /// serving as the first character of the stream/storage name,
        /// are reserved for use by OLE.
        /// This is a compound file restriction,
        /// not a structured storage restriction.
        /// </param>
        /// <param name="mode">
        /// Specifies the access mode to use when opening the newly created stream.
        /// </param>
        /// <param name="reserved1">
        /// Reserved for future use; must be 0.
        /// </param>
        /// <param name="reserved2">
        /// Reserved for future use; must be 0.
        /// </param>
        /// <returns>
        /// A <see cref="ComTypes.IStream"/> interface to the new stream.
        /// When an error occurs, <b>null</b> is returned.
        /// </returns>

        [return: MarshalAs(UnmanagedType.Interface)]
        ComTypes.IStream CreateStream(
            string name,
            int mode,
            int reserved1,
            int reserved2);

        //  ------------------
        //  OpenStream method
        //  ------------------

        /// <summary>
        /// Opens an existing stream object within this storage object
        /// in the specified access mode.
        /// </summary>
        /// <param name="name">
        /// The name of the stream to open.
        /// The 0x0 through 0x1f characters,
        /// serving as the first character of the stream/storage name,
        /// are reserved for use by OLE.
        /// This is a compound file restriction,
        /// not a structured storage restriction.
        /// </param>
        /// <param name="reserved1">
        /// Reserved for future use; must be <see cref="IntPtr.Zero"/>.
        /// </param>
        /// <param name="mode">
        /// The access mode to be assigned to the open stream.
        /// </param>
        /// <param name="reserved2">
        /// Reserved for future use; must be 0.
        /// </param>
        /// <returns>
        /// The newly opened stream.
        /// </returns>

        [return: MarshalAs(UnmanagedType.Interface)]
        ComTypes.IStream OpenStream(
            string name,
            IntPtr reserved1,
            int mode,
            int reserved2);

        //  --------------------
        //  CreateStorage method
        //  --------------------

        /// <summary>
        /// Creates and opens a new storage object nested within this storage object
        /// with the specified name in the specified access mode.
        /// </summary>
        /// <param name="name">
        /// The name of the newly created storage object.
        /// The name can be used later to reopen the storage object.
        /// The name must not exceed 31 characters in length.
        /// The 0x0 through 0x1f characters,
        /// serving as the first character of the stream/storage name,
        /// are reserved for use by OLE.
        /// This is a compound file restriction,
        /// not a structured storage restriction.
        /// </param>
        /// <param name="mode">
        /// The access mode to use when opening the newly created storage object.
        /// </param>
        /// <param name="reserved1">
        /// Reserved for future use; must be 0.
        /// </param>
        /// <param name="reserved2">
        /// Reserved for future use; must be 0.
        /// </param>
        /// <returns>
        /// The newly created storage object.
        /// </returns>

        [return: MarshalAs(UnmanagedType.Interface)]
        IStorage CreateStorage(
            string name,
            int mode,
            int reserved1,
            int reserved2);

        //  ------------------
        //  OpenStorage method
        //  ------------------

        /// <summary>
        /// Opens an existing storage object with the specified name
        /// in the specified access mode.
        /// </summary>
        /// <param name="name">
        /// A string that contains the name of the storage object to open.
        /// The 0x0 through 0x1f characters, serving as the first character
        /// of the stream/storage name, are reserved for use by OLE.
        /// This is a compound file restriction,
        /// not a structured storage restriction.
        /// It is ignored if <i>priority</i> is not <b>null</b>.
        /// </param>
        /// <param name="priority">
        /// Must be <b>null</b>. A non-<b>null</b> value will return
        /// <b>STG_E_INVALIDPARAMETER</b>.
        /// </param>
        /// <param name="mode">
        /// Specifies the access mode to use when opening the storage object.
        /// For descriptions of the possible values, see <b>STGM</b> constants.
        /// Other modes you choose must at least specify <b>STGM_SHARE_EXCLUSIVE</b>
        /// when calling this method.
        /// </param>
        /// <param name="snbExclude">
        /// Must be <b>null</b>. A non-<b>null</b> value will return
        /// <b>STG_E_INVALIDPARAMETER</b>.
        /// </param>
        /// <param name="reserved">
        /// Reserved for future use; must be 0.
        /// </param>
        /// <returns>
        /// An <see cref="IStorage"/> interface to the opened storage object.
        /// </returns>

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "snb")]
        [return: MarshalAs(UnmanagedType.Interface)]
        IStorage OpenStorage(
            string name,
            IStorage priority,
            int mode,
            IntPtr snbExclude,
            int reserved);

        //  -------------
        //  CopyTo method
        //  -------------

        /// <summary>
        /// Copies the entire contents of an open storage object
        /// to another storage object.
        /// </summary>
        /// <param name="ciidExclude">
        /// The number of elements in the array pointed to by <i>rgiidExclude</i>.
        /// If <i>rgiidExclude</i> is <b>null</b>,
        /// then <i>ciidExclude</i> is ignored.
        /// </param>
        /// <param name="rgiidExclude">
        /// An array of interface identifiers (IIDs) that either the caller knows
        /// about and does not want copied or that the storage object does not support,
        /// but whose state the caller will later explicitly copy.
        /// The array can include <see cref="IStorage"/>,
        /// indicating that only stream objects are to be copied,
        /// and <see cref="ComTypes.IStream"/>, indicating that only storage objects
        /// are to be copied.
        /// An array length of zero indicates that only the state exposed by the
        /// <see cref="IStorage"/> object is to be copied;
        /// all other interfaces on the object are to be ignored. Passing <b>null</b>
        /// indicates that all interfaces on the object are to be copied.
        /// </param>
        /// <param name="snbExclude">
        /// A pointer to the open storage object into which this storage object
        /// is to be copied. The destination storage object can be a different
        /// implementation of the <b>IStorage</b> interface from the
        /// source storage object. Thus, <b>IStorage.CopyTo</b> can use
        /// only publicly available methods of the destination storage object.
        /// If <i>pstgDest</i> is open in transacted mode,
        /// it can be reverted by calling its <see cref="Revert"/> method.
        /// </param>
        /// <param name="destination">
        /// A interface to the open storage object into which this
        /// storage object is to be copied.
        /// </param>

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ciid")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "rgiid")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "snb")]
        void CopyTo(
            int ciidExclude,
            IntPtr rgiidExclude,
            IntPtr snbExclude,
            IStorage destination);

        //  --------------------
        //  MoveElementTo method
        //  --------------------

        /// <summary>
        /// Copies or moves a substorage or stream from this storage object
        /// to another storage object.
        /// </summary>
        /// <param name="name">
        /// A string that contains the name of the element in this
        /// storage object to be moved or copied
        /// </param>
        /// <param name="destination">
        /// An <see cref="IStorage"/> interface of the destination storage object.
        /// </param>
        /// <param name="newName">
        /// A string that contains the new name for the element in its new storage object.
        /// </param>
        /// <param name="flags">
        /// Specifies whether the operation should be a move (<b>STGMOVE_MOVE</b>)
        /// or a copy (<b>STGMOVE_COPY</b>). See the <b>STGMOVE</b> enumeration.
        /// </param>

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "flags")]
        void MoveElementTo(
            string name,
            IStorage destination,
            string newName,
            int flags);

        //  -------------
        //  Commit method
        //  -------------

        /// <summary>
        /// Ensures that any changes made to a storage object
        /// open in transacted mode are reflected in the parent storage.
        /// </summary>
        /// <param name="commitConditions">
        /// The flags that controls how the changes are committed
        /// to the storage object.
        /// </param>

        void Commit(int commitConditions);

        //  -------------
        //  Revert method
        //  -------------

        /// <summary>
        /// Discards all changes that have been made to the storage object
        /// since the last commit operation.
        /// </summary>

        void Revert();

        //  -------------------
        //  EnumElements method
        //  -------------------

        /// <summary>
        /// Retrieves an interface to an enumerator object
        /// that can be used to enumerate the storage and stream objects
        /// contained within this storage object.
        /// </summary>
        /// <param name="reserved1">
        /// Reserved for future use; must be 0.
        /// </param>
        /// <param name="reserved2">
        /// Reserved for future use; must be <see cref="IntPtr.Zero"/>.
        /// </param>
        /// <param name="reserved3">
        /// Reserved for future use; must be 0.
        /// </param>
        /// <returns>
        /// An interface to the new enumerator object.
        /// </returns>

        [return: MarshalAs(UnmanagedType.Interface)]
        IEnumSTATSTG EnumElements(
            int reserved1,
            IntPtr reserved2,
            int reserved3);

        //  ---------------------
        //  DestroyElement method
        //  ---------------------

        /// <summary>
        /// Removes the specified storage or stream from this storage object.
        /// </summary>
        /// <param name="name">
        /// The name of the storage or stream to be removed.
        /// </param>

        void DestroyElement(string name);

        //  --------------------
        //  RenameElement method
        //  --------------------

        /// <summary>
        /// Renames the specified substorage or stream
        /// in this storage object.
        /// </summary>
        /// <param name="oldName">
        /// The name of the substorage or stream to be changed.
        /// </param>
        /// <param name="newName">
        /// The new name for the specified substorage or stream.
        /// <b>Note:</b> The name must not exceed 31 characters in length.
        /// </param>

        void RenameElement(
            string oldName,
            string newName);

        //  ----------------------
        //  SetElementTimes method
        //  ----------------------

        /// <summary>
        /// sets the modification, access, and creation times of the specified
        /// storage element, if the underlying file system supports this method.
        /// </summary>
        /// <param name="name">
        /// The name of the storage object element whose times are to be
        /// modified. If <b>null</b>, the time is set on the root storage rather
        /// than one of its elements.
        /// </param>
        /// <param name="created">
        /// Either the new creation time for the element or <b>null</b> if the
        /// creation time is not to be modified.
        /// </param>
        /// <param name="accessed">
        /// Either the new access time for the element or <b>null</b> if the
        /// access time is not to be modified.
        /// </param>
        /// <param name="modified">
        /// Either the new modification time for the element or <b>null</b> if
        /// the modification time is not to be modified.
        /// </param>

        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#")]
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "2#")]
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "3#")]
        void SetElementTimes(
            string name,
            ref ComTypes.FILETIME created,
            ref ComTypes.FILETIME accessed,
            ref ComTypes.FILETIME modified);

        //  ---------------
        //  SetClass method
        //  ---------------

        /// <summary>
        /// Assigns the specified class identifier (CLSID) to this storage object.
        /// </summary>
        /// <param name="classId">
        /// The CLSID that is to be associated with the storage object.
        /// </param>

        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        void SetClass(ref Guid classId);

        //  -------------------
        //  SetStateBits method
        //  -------------------

        /// <summary>
        /// stores up to 32 bits of state information in this storage object.
        /// This method is reserved for future use.
        /// </summary>
        /// <param name="stateBits">
        /// Specifies the new values of the bits to set.
        /// No legal values are defined for these bits;
        /// they are all reserved for future use and must not be used by
        /// applications.
        /// </param>
        /// <param name="mask">
        /// A binary mask indicating which bits in <i>stateBits</i> are
        /// significant in this call.
        /// </param>

        void SetStateBits(
            int stateBits,
            int mask);

        //  -----------
        //  Stat method
        //  -----------

        /// <summary>
        /// Retrieves the <see cref="STATSTG"/> structure
        /// for this open storage object.
        /// </summary>
        /// <param name="statistics">
        /// A <b>STATSTG</b> structure where this method places information
        /// about the open storage object.
        /// </param>
        /// <param name="flags">
        /// Specifies that some of the members in the <b>STATSTG</b> structure
        /// are not returned, thus saving a memory allocation operation.
        /// Values are taken from the <b>STATFLAG</b> enumeration.
        /// </param>

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "flags")]
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#")]
        void Stat(out ComTypes.STATSTG statistics, int flags);
    }
}

// eof "IStorage.cs"
