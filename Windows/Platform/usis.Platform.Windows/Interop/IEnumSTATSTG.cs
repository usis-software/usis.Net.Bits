//
//  @(#) IEnumSTATSTG.cs - internal interface
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
    //  ----------------------
    //  IEnumSTATSTG interface
    //  ----------------------

    /// <summary>
    /// Enumerates an array of <see cref="STATSTG"/> structures.
    /// </summary>

    [SuppressMessage("Microsoft.Naming", "CA1705:LongAcronymsShouldBePascalCased")]
    [ComImport]
    [Guid(IID.IEnumSTATSTG)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IEnumSTATSTG
    {
        //  -----------
        //  Next method
        //  -----------

        /// <summary>
        /// Retrieves a specified number of <see cref="STATSTG"/> structures,
        /// that follow in the enumeration sequence.
        /// If there are fewer than the requested number of
        /// <b>STATSTG</b> structures that remain in the enumeration sequence,
        /// it retrieves the remaining <b>STATSTG</b> structures.
        /// </summary>
        /// <param name="count">
        /// The number of <b>STATSTG</b> structures requested.
        /// </param>
        /// <param name="elements">
        /// An array of <b>STATSTG</b> structures returned.
        /// </param>
        /// <returns>
        /// The number of <b>STATSTG</b> structures retrieved
        /// in the <i>elements</i> parameter. 
        /// </returns>

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Next")]
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        int Next(int count,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0), Out]
            ComTypes.STATSTG[] elements);

        //  -----------
        //  Skip method
        //  -----------

        /// <summary>
        /// Skips a specified number of
        /// <see cref="STATSTG"/> structures in the enumeration sequence.
        /// </summary>
        /// <param name="count">
        /// The number of <b>STATSTG</b> structures to skip.
        /// </param>

        void Skip(int count);

        //  ------------
        //  Reset method
        //  ------------

        /// <summary>
        /// Resets the enumeration sequence to the beginning of the
        /// <see cref="STATSTG"/> structure array.
        /// </summary>

        void Reset();

        //  ------------
        //  Clone method
        //  ------------

        /// <summary>
        /// Creates a new enumerator that contains the same enumeration state
        /// as the current <see cref="STATSTG"/> structure enumerator.
        /// </summary>
        /// <returns>
        /// A <b>IEnumSTATSTG</b> interface to the new enumerator.
        /// </returns>

        [return: MarshalAs(UnmanagedType.Interface)]
        IEnumSTATSTG Clone();
    }
}

// eof "IEnumSTATSTG.cs"
