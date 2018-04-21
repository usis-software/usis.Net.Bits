//
//  @(#) STATPROPSTG.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2008-2016 usis GmbH. All rights reserved.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace usis.Platform.Interop
{
    //  ---------------------
    //  STATPROPSTG structure
    //  ---------------------

    /// <summary>
    /// The <b>STATPROPSTG</b> structure contains data about a
    /// single property in a property set. This data is the
    /// property ID and type tag, and the optional string name
    /// that may be associated with the property.
    /// </summary>

    [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    [SuppressMessage("Microsoft.Naming", "CA1705:LongAcronymsShouldBePascalCased")]
    [StructLayout(LayoutKind.Sequential)]
    public struct STATPROPSTG
    {
        #region private fields

        [MarshalAs(UnmanagedType.LPWStr)]
        private string lpwstrName;
        private int propid;
        private short vt;

        #endregion // private fields

        #region public properties

        //  -----------
        //  VT property
        //  -----------

        /// <summary>
        /// Gets the value type tag.
        /// </summary>

        public VarEnum VT
        {
            get { return (VarEnum)vt; }
        }

        //  -------------------
        //  PropertyId property
        //  -------------------

        /// <summary>
        /// Gets the property identifier.
        /// </summary>

        public int PropertyId
        {
            get { return propid; }
        }

        //  ---------------------
        //  PropertyName property
        //  ---------------------

        /// <summary>
        /// Gets the property name.
        /// </summary>

        public string PropertyName
        {
            get { return lpwstrName; }
        }

        #endregion public properties
    }
}

// eof "STATPROPSTG.cs"
