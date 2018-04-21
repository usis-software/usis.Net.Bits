//
//  @(#) STATPROPSETSTG.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2008-2017 usis GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using ComTypes = System.Runtime.InteropServices.ComTypes;

namespace usis.Platform.Interop
{
    //  ---------------------
    //  STATPROPSETSTG struct
    //  ---------------------

    /// <summary>
    /// The <b>STATPROPSETSTG</b> structure contains information about
    /// a property set. To get this information, call
    /// IPropertyStorage.<see cref="IPropertyStorage.Stat"/>,
    /// which fills in a buffer containing the information describing
    /// the current property set. To enumerate the <b>STATPROPSETSTG</b>
    /// structures for the property sets in the current property-set storage,
    /// call <b>IPropertySetStorage.Enum</b> to get a reference to an enumerator.
    /// You can then call the enumeration methods of the
    /// <b>IEnumSTATPROPSETSTG</b> interface on the enumerator.
    /// </summary>

    [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    [SuppressMessage("Microsoft.Naming", "CA1705:LongAcronymsShouldBePascalCased")]
    [StructLayout(LayoutKind.Sequential)]
    internal struct STATPROPSETSTG
    {
        #region private fields

        private readonly Guid fmtid;
        private readonly Guid clsid;
        private readonly int grfFlags;
        private readonly ComTypes.FILETIME mtime;
        private readonly ComTypes.FILETIME ctime;
        private readonly ComTypes.FILETIME atime;
        private readonly int osVersion;

        #endregion // private fields

        //  -----------------
        //  FormatId property
        //  -----------------

        /// <summary>
        /// The FMTID (GUID) of the current property set,
        /// specified when the property set was initially created.
        /// </summary>

        public Guid FormatId => fmtid;

        //  ----------------
        //  ClassId property
        //  ----------------

        /// <summary>
        /// The CLSID (GUID) associated with this property set,
        /// specified when the property set was initially created
        /// and possibly modified thereafter with
        /// <b>IPropertyStorage.SetClass</b>. If not set, the value
        /// will be <c>Guid.Empty</c>.
        /// </summary>

        public Guid ClassId => clsid;

        //  --------------
        //  Flags property
        //  --------------

        /// <summary>
        /// Flag values of the property set,
        /// as specified in <b>IPropertySetStorage.Create</b>.
        /// </summary>

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags")]
        public int Flags => grfFlags;

        //  -----------------
        //  Modified property
        //  -----------------

        /// <summary>
        /// Time in Universal Coordinated Time (UTC)
        /// when the property set was last modified.
        /// </summary>

        public ComTypes.FILETIME Modified => mtime;

        //  ----------------
        //  Created property
        //  ----------------

        /// <summary>
        /// Time in Universal Coordinated Time (UTC)
        /// when this property set was created.
        /// </summary>

        public ComTypes.FILETIME Created => ctime;

        //  -----------------
        //  Accessed property
        //  -----------------

        /// <summary>
        /// Time in Universal Coordinated Time (UTC)
        /// when this property set was last accessed.
        /// </summary>

        public ComTypes.FILETIME Accessed => atime;

        //  ------------------
        //  OSVersion property
        //  ------------------

        /// <summary>
        /// The undocumented operating system version.
        /// </summary>

        public int OSVersion => osVersion;
    }
}

// eof "STATPROPSETSTG.cs"
