//
//  @(#) IEnumSTATPROPSETSTG.cs - internal interface
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
    //  IEnumSTATPROPSETSTG interface
    //  -----------------------------

    /// <summary>
    /// Iterates through an array of <see cref="STATPROPSETSTG"/>
    /// structures. The <b>STATPROPSETSTG</b> structures contain
    /// statistical data about the property sets managed by the current
    /// <see cref="IPropertySetStorage"/> instance.
    /// </summary>

    [SuppressMessage("Microsoft.Naming", "CA1705:LongAcronymsShouldBePascalCased")]
    [ComImport]
    [Guid(IID.IEnumSTATPROPSETSTG)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IEnumSTATPROPSETSTG
    {
        //  -----------
        //  Next method
        //  -----------

        /// <summary>
        /// Retrieves a specified number of <see cref="STATPROPSETSTG"/>
        /// structures, that follow in the enumeration sequence.
        /// If there are fewer than the requested number of
        /// <b>STATPROPSETSTG</b> structures that remain in the enumeration
        /// sequence, it retrieves the remaining <b>STATPROPSETSTG</b>
        /// structures.
        /// </summary>
        /// <param name="count">
        /// The number of <b>STATPROPSETSTG</b> structures requested.
        /// </param>
        /// <param name="elements">
        /// An array of <b>STATSTG</b> structures returned.
        /// </param>
        /// <returns>
        /// The number of <b>STATPROPSETSTG</b> structures retrieved
        /// in the <i>elements</i> parameter. 
        /// </returns>

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Next")]
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        int Next(int count,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0), Out]
            STATPROPSETSTG[] elements);

        //  -----------
        //  Skip method
        //  -----------

        /// <summary>
        /// Skips a specified number of
        /// <see cref="STATPROPSETSTG"/> structures in the enumeration sequence.
        /// </summary>
        /// <param name="count">
        /// The number of <b>STATPROPSETSTG</b> structures to skip.
        /// </param>

        void Skip(int count);

        //  ------------
        //  Reset method
        //  ------------

        /// <summary>
        /// Resets the enumeration sequence to the beginning of the
        /// <see cref="STATPROPSETSTG"/> structure array.
        /// </summary>

        void Reset();

        //  ------------
        //  Clone method
        //  ------------

        /// <summary>
        /// Creates a new enumerator that contains the same enumeration state
        /// as the current <see cref="STATPROPSETSTG"/> structure enumerator.
        /// </summary>
        /// <returns>
        /// A <b>IEnumSTATPROPSETSTG</b> interface to the new enumerator.
        /// </returns>

        [return: MarshalAs(UnmanagedType.Interface)]
        IEnumSTATPROPSETSTG Clone();
    }
}

// eof "IEnumSTATPROPSETSTG.cs"
