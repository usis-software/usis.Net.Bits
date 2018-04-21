//
//  @(#) IPropertyStorage.cs - internal interface
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
    //  --------------------------
    //  IPropertyStorage interface
    //  --------------------------

    /// <summary>
    /// Manages the persistent properties of a single property set.
    /// Persistent properties consist of information
    /// that can be stored persistently in a property set.
    /// </summary>

    [ComImport]
    [Guid(IID.IPropertyStorage)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPropertyStorage
    {
        //  -------------------
        //  ReadMultiple method
        //  -------------------

        /// <summary>
        /// Reads the specified properties from the current property set.
        /// </summary>
        /// <param name="count">
        /// The number of properties specified in the <i>propSpecs</i> array.
        /// </param>
        /// <param name="propSpecs">
        /// An array of <see cref="PROPSPEC"/> structures
        /// that specifies which properties to read.
        /// </param>
        /// <param name="propVariants">
        /// A caller-allocated array of <see cref="PROPVARIANT"/> structures that,
        /// on return, contains the values of the properties specified by
        /// the corresponding elements in the <i>propSpecs</i> array.
        /// </param>

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#")]
        void ReadMultiple(
            int count,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)]
            PROPSPEC[] propSpecs,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0), Out]
            PROPVARIANT[] propVariants);

        //  --------------------
        //  WriteMultiple method
        //  --------------------

        /// <summary>
        /// Writes a specified group of properties to the current property set.
        /// If a property with a specified name or property identifier
        /// already exists, it is replaced, even when the old and new types
        /// for the property value are different.
        /// If a property of a given name or property ID does not exist,
        /// it is created.
        /// </summary>
        /// <param name="count">
        /// The number of properties set.
        /// </param>
        /// <param name="propSpecs">
        /// An array of the property IDs (<see cref="PROPSPEC"/>) to which
        /// properties are set. These need not be in any particular order, and
        /// may contain duplicates, however the last specified property ID is
        /// the one that takes effect.
        /// A mixture of property IDs and string names is permitted.
        /// </param>
        /// <param name="propVariants">
        /// An array (of size <i>count</i>) of <see cref="PROPVARIANT"/>
        /// structures that contain the property values to be written.
        /// The array must be the size specified by <i>count</i>. 
        /// </param>
        /// <param name="propIdNameFirst">
        /// The minimum value for the property IDs that the method must assign
        /// if the <i>propSpecs</i> parameter specifies string-named properties
        /// for which no property IDs currently exist.
        /// If all string-named properties specified already exist in this set,
        /// and thus already have property IDs, this value is ignored.
        /// When not ignored, this value must be greater than, or equal to,
        /// two and less than 0x80000000.
        /// Property IDs 0 and 1 and greater than 0x80000000
        /// are reserved for special use.
        /// </param>

        void WriteMultiple(
            int count,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)]
            PROPSPEC[] propSpecs,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)]
            PROPVARIANT[] propVariants,
            int propIdNameFirst);

        //  ---------------------
        //  DeleteMultiple method
        //  ---------------------

        /// <summary>
        /// Deletes as many of the indicated properties as exist in this property set.
        /// </summary>
        /// <param name="count">
        /// The numerical count of properties to be deleted.
        /// </param>
        /// <param name="propSpecs">
        /// Properties to be deleted. A mixture of property identifiers and
        /// string-named properties is permitted. There may be duplicates,
        /// and there is no requirement that properties be specified in any order.
        /// </param>

        void DeleteMultiple(
            int count,
            [MarshalAs(UnmanagedType.LPArray)]
            PROPSPEC[] propSpecs);

        //  ------------------------
        //  ReadPropertyNames method
        //  ------------------------

        /// <summary>
        /// Retrieves any existing string names for the specified property IDs.
        /// </summary>
        /// <param name="count">
        /// The Number of elements on input of the array <i>propIds</i>.
        /// </param>
        /// <param name="propIds">
        /// An array of property IDs for which names are to be retrieved.
        /// </param>
        /// <param name="names">
        /// A caller-allocated array of size <i>propIds</i> of string members.
        /// On return, the implementation fills in this array.
        /// A given entry contains either the corresponding string name of a
        /// property ID or it can be empty if the property ID has no string names.
        /// <br/><br/>
        /// Each string member of the array should be freed using the
        /// <see cref="Marshal.FreeCoTaskMem"/> function.
        /// </param>

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#")]
        void ReadPropertyNames(
            int count,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)]
            int[] propIds,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0), Out]
            string[] names);

        //  -------------------------
        //  WritePropertyNames method
        //  -------------------------

        /// <summary>
        /// Assigns string names to a specified array of property IDs
        /// in the current property set.
        /// </summary>
        /// <param name="count">
        /// The size on input of the array <i>propIds</i>.
        /// </param>
        /// <param name="propIds">
        /// An array of the property IDs for which names are to be set.
        /// </param>
        /// <param name="names">
        /// An array of new names to be assigned to the corresponding
        /// property IDs in the <i>propIds</i> array.
        /// These names may not exceed 255 characters.
        /// </param>

        void WritePropertyNames(
            int count,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)]
            int[] propIds,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)]
            string[] names);

        //  --------------------------
        //  DeletePropertyNames method
        //  --------------------------

        /// <summary>
        /// Deletes specified string names from the current property set.
        /// </summary>
        /// <param name="count">
        /// The size on input of the array <i>propIds</i>.
        /// If 0, no property names are deleted.
        /// </param>
        /// <param name="propIds">
        /// The Property identifiers for which string names are to be deleted.
        /// </param>

        void DeletePropertyNames(
            int count,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)]
            int[] propIds);

        //  -------------
        //  Commit method
        //  -------------

        /// <summary>
        /// Saves changes made to a property storage object
        /// to the parent storage object.
        /// </summary>
        /// <param name="conditions">
        /// The flags that specify the conditions
        /// under which the commit is to be performed.
        /// </param>

        void Commit(int conditions);

        //  -------------
        //  Revert method
        //  -------------

        /// <summary>
        /// Discards all changes to the named property set
        /// since it was last opened or discards changes
        /// that were last committed to the property set.
        /// This method has no effect on a direct-mode property set.
        /// </summary>

        void Revert();

        //  -----------
        //  Enum method
        //  -----------

        /// <summary>
        /// Creates an enumerator object designed to enumerate data
        /// of type <b>STATPROPSTG</b>,
        /// which contains information on the current property set.
        /// </summary>
        /// <returns>
        /// The new enumerator object.
        /// </returns>

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Enum")]
        [return: MarshalAs(UnmanagedType.Interface)]
        IEnumSTATPROPSTG Enum();

        //  ---------------
        //  SetTimes method
        //  ---------------

        /// <summary>
        /// Sets the modification, access, and creation times of this property set.
        /// </summary>
        /// <param name="created">
        /// The new creation time for the property set.
        /// May be <b>null</b>, indicating that this time is not to be modified by this call.
        /// </param>
        /// <param name="accessed">
        /// The new access time for the property set.
        /// May be <b>null</b>, indicating that this time is not to be modified by this call.
        /// </param>
        /// <param name="modified">
        /// The new modification time for the property set.
        /// May be <b>null</b>, indicating that this time is not to be modified by this call.
        /// </param>

        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#")]
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "2#")]
        void SetTimes(
            ref ComTypes.FILETIME created,
            ref ComTypes.FILETIME accessed,
            ref ComTypes.FILETIME modified);

        //  ---------------
        //  SetClass method
        //  ---------------

        /// <summary>
        /// Assigns a new CLSID to the current property storage object,
        /// and persistently stores the CLSID with the object.
        /// </summary>
        /// <param name="classId">
        /// The new CLSID to be associated with the property set.
        /// </param>

        void SetClass(Guid classId);

        //  -----------
        //  Stat method
        //  -----------

        /// <summary>
        /// Retrieves information about the current open property set.
        /// </summary>
        /// <param name="statistics">
        /// A <see cref="STATPROPSETSTG"/> structure,
        /// which contains statistics about the current open property set.
        /// </param>

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#")]
        void Stat(out STATPROPSETSTG statistics);
    }
}

// eof "IPropertyStorage.cs"
