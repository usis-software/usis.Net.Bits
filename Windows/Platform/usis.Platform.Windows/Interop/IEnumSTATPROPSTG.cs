//
//  @(#) IEnumSTATPROPSTG.cs - internal interface
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2008-2017 usis GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace usis.Platform.Interop
{
    //  --------------------------
    //  IEnumSTATPROPSTG interface
    //  --------------------------

    /// <summary>
    /// Iterates through an array of <see cref="STATPROPSTG"/> structures.
    /// These structures contain statistical data
    /// about open storage, stream, or byte array objects.
    /// </summary>

    [SuppressMessage("Microsoft.Naming", "CA1705:LongAcronymsShouldBePascalCased")]
    [ComImport]
    [Guid(IID.IEnumSTATPROPSTG)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IEnumSTATPROPSTG
    {
        //  -----------
        //  Next method
        //  -----------

        /// <summary>
        /// Retrieves a specified number of <see cref="STATPROPSTG"/> structures,
        /// that follow in the enumeration sequence.
        /// If there are fewer than the requested number of
        /// <b>STATPROPSTG</b> structures that remain in the enumeration sequence,
        /// it retrieves the remaining <b>STATPROPSTG</b> structures.
        /// </summary>
        /// <param name="count">
        /// The number of <b>STATPROPSTG</b> structures requested.
        /// </param>
        /// <param name="elements">
        /// An array of <b>STATPROPSTG</b> structures returned.
        /// </param>
        /// <returns>
        /// The number of <b>STATPROPSTG</b> structures retrieved
        /// in the <i>elements</i> parameter. 
        /// </returns>

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Next")]
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        int Next(int count,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0), Out]
            STATPROPSTG[] elements);

        //  -----------
        //  Skip method
        //  -----------

        /// <summary>
        /// Skips a specified number of
        /// <see cref="STATPROPSTG"/> structures in the enumeration sequence.
        /// </summary>
        /// <param name="count">
        /// The number of <b>STATPROPSTG</b> structures to skip.
        /// </param>

        void Skip(int count);

        //  ------------
        //  Reset method
        //  ------------

        /// <summary>
        /// Resets the enumeration sequence to the beginning of the
        /// <see cref="STATPROPSTG"/> structure array.
        /// </summary>

        void Reset();

        //  ------------
        //  Clone method
        //  ------------

        /// <summary>
        /// Creates a new enumerator that contains the same enumeration state
        /// as the current <see cref="STATPROPSTG"/> structure enumerator.
        /// </summary>
        /// <returns>
        /// A <b>IEnumSTATPROPSTG</b> interface to the new enumerator.
        /// </returns>

        [return: MarshalAs(UnmanagedType.Interface)]
        IEnumSTATPROPSTG Clone();
    }
}

// eof "IEnumSTATPROPSTG.cs"
