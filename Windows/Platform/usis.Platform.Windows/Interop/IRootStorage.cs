//
//  @(#) IRootStorage.cs - internal interface
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2008-2016 usis GmbH. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace usis.Platform.Interop
{
    //  ----------------------
    //  IRootStorage interface
    //  ----------------------

    /// <summary>
    /// The <b>IRootStorage</b> interface contains a single method
    /// that switches a storage object to a different underlying file
    /// and saves the storage object to that file.
    /// The save operation occurs even with low-memory conditions
    /// and uncommitted changes to the storage object.
    /// A subsequent call to <b>IStorage.Commit</b> is guaranteed
    /// to not consume additional memory.
    /// </summary>

    [ComImport]
    [Guid(IID.IRootStorage)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IRootStorage
    {
        //  -------------------
        //  SwitchToFile method
        //  -------------------

        /// <summary>
        /// Copies the file underlying this root storage object,
        /// and then associates this storage with the copied file.
        /// </summary>
        /// <param name="fileName">
        /// The file name for the new file.
        /// It cannot be the name of an existing file.
        /// If <b>null</b>, this method creates a temporary file
        /// with a unique name, and you can call <b>IStorage.Stat</b>
        /// to retrieve the name of the temporary file.
        /// </param>

        void SwitchToFile(
            [MarshalAs(UnmanagedType.LPWStr)]
            string fileName);
    }
}

// eof "IRootStorage.cs"
