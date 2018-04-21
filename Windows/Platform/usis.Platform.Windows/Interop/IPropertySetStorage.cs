//
//  @(#) IPropertySetStorage.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2008-2016 usis GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace usis.Platform.Interop
{
    //  -----------------------------
    //  IPropertySetStorage interface
    //  -----------------------------

    /// <summary>
    /// Creates, opens, deletes, and enumerates property set storages
    /// that support instances of the <see cref="IPropertyStorage"/>
    /// interface.
    /// </summary>

    [ComImport]
    [Guid(IID.IPropertySetStorage)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPropertySetStorage
    {
        //  -------------
        //  Create method
        //  -------------

        /// <summary>
        /// creates and opens a new property set in the property set storage object.
        /// </summary>
        /// <param name="formatId">
        /// The FMTID of the property set to be created.
        /// </param>
        /// <param name="classId">
        /// A pointer to the initial class identifier (CLSID) for this property set.
        /// May be <b>null</b>, in which case it is set to <see cref="Guid.Empty"/>.
        /// The CLSID is the CLSID of a class that displays and/or
        /// provides programmatic access to the property values.
        /// If there is no such class, it is recommended that the FMTID be used. 
        /// </param>
        /// <param name="flags">
        /// The values from <b>PROPSETFLAG</b> constants.
        /// </param>
        /// <param name="mode">
        /// An access mode in which the newly created property set is to be opened,
        /// taken from certain values of <b>STGM</b> constants.
        /// </param>
        /// <returns>
        /// The created <see cref="IPropertyStorage"/>.
        /// </returns>

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "flags")]
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        [return: MarshalAs(UnmanagedType.Interface)]
        IPropertyStorage Create(
            ref Guid formatId,
            IntPtr classId,
            int flags,
            int mode);

        //  -----------
        //  Open method
        //  -----------

        /// <summary>
        /// Opens a property set contained in the property set storage object.
        /// </summary>
        /// <param name="formatId">
        /// The format identifier (FMTID) of the property set to be opened.
        /// </param>
        /// <param name="mode">
        /// An access mode in which the property set is to be opened,
        /// taken from certain values of <b>STGM</b> constants.
        /// </param>
        /// <returns>
        /// The opened <see cref="IPropertyStorage"/>.
        /// </returns>

        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        [return: MarshalAs(UnmanagedType.Interface)]
        IPropertyStorage Open(
            ref Guid formatId,
            int mode);

        //  -------------
        //  Delete method
        //  -------------

        /// <summary>
        /// Deletes one of the property sets contained
        /// in the property set storage object.
        /// </summary>
        /// <param name="formatId">
        /// FMTID of the property set to be deleted.
        /// </param>

        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        void Delete(ref Guid formatId);

        //  -----------
        //  Enum method
        //  -----------

        /// <summary>
        /// Creates an enumerator object which contains information
        /// on the property sets stored in this property set storage.
        /// </summary>
        /// <returns>
        /// The newly created enumerator object.
        /// </returns>

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Enum")]
        [return: MarshalAs(UnmanagedType.Interface)]
        IEnumSTATPROPSETSTG Enum();
    }
}

// eof "IPropertySetStorage.cs"
