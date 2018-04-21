//
//  @(#) PROPVARIANT.cs
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
    //  ---------------------
    //  PROPVARIANT structure
    //  ---------------------

    /// <summary>
    /// The PROPVARIANT structure is used in the
    /// <see cref="IPropertyStorage.ReadMultiple"/> and
    /// <see cref="IPropertyStorage.WriteMultiple"/> methods of
    /// <see cref="IPropertyStorage"/> to define
    /// the type tag and the value of a property in a property set.
    /// </summary>

    [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    [SuppressMessage("Microsoft.Naming", "CA1705:LongAcronymsShouldBePascalCased")]
    [StructLayout(LayoutKind.Sequential)]
    internal struct PROPVARIANT : IDisposable
    {
        #region private fields

        // [FieldOffset(0)]
        private ushort vt;
        // [FieldOffset(2)]
        private short wReserved1;
        // [FieldOffset(4)]
        private short wReserved2;
        // [FieldOffset(6)]
        private short wReserved3;
        // [FieldOffset(8)]
        private long data;

        #endregion private fields

        #region public properties

        //  -----------
        //  VT property
        //  -----------

        /// <summary>
        /// Gets the value type tag.
        /// </summary>

        public VarEnum VT
        {
            get => (VarEnum)vt;
            internal set => vt = (ushort)value;
        }

        //  -------------
        //  Data property
        //  -------------

        /// <summary>
        /// Gets the data of the <b>PROPVARIANT</b> structure.
        /// </summary>

        public long Data => data;

        #endregion public properties

        #region public methods

        //  ------------
        //  Clear method
        //  ------------

        /// <summary>
        /// Sets the <b>PROPVARIANT</b> to VT_EMPTY and frees any
        /// allocated contents.
        /// </summary>

        public void Clear()
        {
            if (VT != VarEnum.VT_EMPTY)
            {
                Marshal.ThrowExceptionForHR(NativeMethods.PropVariantClear(ref this));
            }
        }

        #region get/set methods

        //  -------------------
        //  TryGetString method
        //  -------------------

        /// <summary>
        /// Tries to get the PROPVARIANT value as a BSTR (VT_BSTR).
        /// </summary>
        /// <param name="value">
        /// The BSTR value of the PROPVARIANT structure.
        /// </param>
        /// <returns>
        /// <b>true</b> if the PROPVARIANT contains a BSTR
        /// and a string is returned; otherwise <b>false</b>.
        /// </returns>

        public bool TryGetString(out string value)
        {
            if (VT == VarEnum.VT_BSTR)
            {
                value = Marshal.PtrToStringBSTR(new IntPtr(data));
                return true;
            }
            else { value = null; return false; }
        }

        //  ----------------
        //  SetString method
        //  ----------------

        /// <summary>
        /// Sets the PROPVARIANT value to a BSTR (VT_BSTR).
        /// </summary>
        /// <param name="value">
        /// The string to set as PROPVARIANT value.
        /// </param>

        public void SetString(string value)
        {
            Clear();
            data = Marshal.StringToBSTR(value).ToInt64();
            VT = VarEnum.VT_BSTR;
        }

        //  --------------------
        //  TryGetBoolean method
        //  --------------------

        /// <summary>
        /// Tries to get the PROPVARIANT value as a boolean value (VT_BOOL).
        /// </summary>
        /// <param name="value">
        /// The boolean value of the PROPVARIANT structure.
        /// </param>
        /// <returns>
        /// <b>true</b> if the PROPVARIANT contains a boolean value;
        /// otherwise <b>false</b>.
        /// </returns>

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public bool TryGetBoolean(out bool value)
        {
            if (VT == VarEnum.VT_BOOL)
            {
                value = data != 0;
                return true;
            }
            else { value = false; return false; }
        }

        //  -----------------
        //  SetBoolean method
        //  -----------------

        /// <summary>
        /// Sets the PROPVARIANT value to a boolean value (VT_BOOL).
        /// </summary>
        /// <param name="value">
        /// The boolean value to set as PROPVARIANT value.
        /// </param>

        public void SetBoolean(bool value)
        {
            Clear();
            data = value ? -1 : 0;
            VT = VarEnum.VT_BOOL;
        }

        //  --------------
        //  SetGuid method
        //  --------------

        /// <summary>
        /// Sets the PROPVARIANT value to a GUID (VT_CLSID).
        /// </summary>
        /// <param name="value">
        /// The GUID to set as PROPVARIANT value.
        /// </param>

        public void SetGuid(Guid value)
        {
            Clear();
            IntPtr p = Marshal.AllocCoTaskMem(Marshal.SizeOf(value));
            Marshal.StructureToPtr(value, p, false);
            data = p.ToInt64();
            VT = VarEnum.VT_CLSID;
        }

        #endregion get/set methods

        #endregion public methods

        #region IDisposable members

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Performs application-defined tasks associated
        /// with freeing, releasing, or resetting unmanaged resources.
        /// </summary>

        public void Dispose()
        {
            Clear();
        }

        #endregion IDisposable members
    }
}

// eof "PROPVARIANT.cs"
