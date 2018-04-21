//
//  @(#) PROPSPEC.cs
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
    //  ------------------
    //  PROPSPEC structure
    //  ------------------

    /// <summary>
    /// The <b>PROPSPEC</b> structure is used by many of the methods of
    /// <see cref="IPropertyStorage"/> to specify a property either by
    /// its property identifier (ID) or the associated string name.
    /// </summary>

    [SuppressMessage("Microsoft.Naming", "CA1705:LongAcronymsShouldBePascalCased")]
    [StructLayout(LayoutKind.Sequential)]
    public struct PROPSPEC : IDisposable
    {
        #region private members

        private int ulKind;
        [SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
        private IntPtr data;

        #endregion // private members

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="PROPSPEC"/> class
        /// with the specified name.
        /// </summary>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <remarks>
        /// String names are optional and can be assigned to a set
        /// of properties when the property is created with a call to
        /// <see cref="IPropertyStorage.WriteMultiple"/> or later with a call to
        /// <see cref="IPropertyStorage.WritePropertyNames"/>.
        /// </remarks>

        public PROPSPEC(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            ulKind = 0;
            data = Marshal.StringToCoTaskMemUni(name);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PROPSPEC"/> class
        /// with the specified property identifier.
        /// </summary>
        /// <param name="propertyId">
        /// The property identifier.
        /// </param>

        public PROPSPEC(int propertyId)
        {
            ulKind = 1;
            data = new IntPtr(propertyId);
        }

        #endregion construction

        #region public properties

        //  ----------------
        //  HasName property
        //  ----------------

        /// <summary>
        /// Gets a value indicating whether the property is specified by an
        /// associated string name or by its property identifier.
        /// </summary>

        public bool HasName
        {
            get { return ulKind == 0; }
        }

        //  ----------------
        //  IsValid property
        //  ----------------

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>

        public bool IsValid
        {
            get
            {
                if (HasName) return data != IntPtr.Zero;
                else
                {
                    long propid = data.ToInt64();
                    return propid > 1 && propid <= (long)int.MaxValue + 1;
                }
            }
        }

        //  -------------------
        //  PropertyId property
        //  -------------------

        /// <summary>
        /// Gets or sets the property identifier.
        /// </summary>

        public int PropertyId
        {
            get { return HasName ? 0 : data.ToInt32(); }
            set { SetPropId(value); }
        }

        //  ---------------------
        //  PropertyName property
        //  ---------------------

        /// <summary>
        /// Gets or sets the property name.
        /// </summary>

        public string PropertyName
        {
            get
            {
                if (HasName) return Marshal.PtrToStringUni(data);
                else return null;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (HasName)
                {
                    if (data != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(data);
                    }
                }
                else ulKind = 0;
                data = Marshal.StringToCoTaskMemUni(value);
            }
        }

        #endregion public properties

        #region private methods

        //  ----------------
        //  SetPropId method
        //  ----------------

        private void SetPropId(int propertyId)
        {
            if (HasName)
            {
                if (data != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(data);
                }
                ulKind = 1;
            }
            data = new IntPtr(propertyId);
        }

        #endregion private methods

        #region public methods

        //  ------------
        //  Clear method
        //  ------------

        /// <summary>
        /// Sets the property specification to property identifier 0.
        /// </summary>

        public void Clear()
        {
            SetPropId(0);
        }

        //  ------------------
        //  CreateArray method
        //  ------------------

        /// <summary>
        /// Creates an array of <b>PROPSPEC</b> structures an initializes them
        /// with the specified property identifiers.
        /// </summary>
        /// <param name="propertyIds">
        /// The property identifiers
        /// </param>
        /// <returns>
        /// A newly created array of <b>PROPSPEC</b> structures.
        /// </returns>

        public static PROPSPEC[] CreateArray(params int[] propertyIds)
        {
            if (propertyIds == null) throw new ArgumentNullException("propertyIds");

            PROPSPEC[] items = new PROPSPEC[propertyIds.Length];
            for (int i = 0; i < propertyIds.Length; i++)
            {
                items[i].PropertyId = propertyIds[i];
            }
            return items;
        }

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
            if (HasName)
            {
                if (data != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(data);
                    data = IntPtr.Zero;
                }
            }
        }

        #endregion IDisposable members

        #region CA1815

        //  -------------
        //  Equals method
        //  -------------

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        /// <b>true</b> if the specified <see cref="object" /> is equal to this instance; otherwise, <b>false</b>.
        /// </returns>

        public override bool Equals(object obj)
        {
            if (!(obj is PROPSPEC)) return false;
            return Equals((PROPSPEC)obj);
        }

        /// <summary>
        /// Determines whether the specified <see cref="PROPSPEC" />, is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="PROPSPEC" /> to compare with this instance.</param>
        /// <returns>
        /// <b>true</b> if the specified <see cref="PROPSPEC" /> is equal to this instance; otherwise, <b>false</b>.
        /// </returns>

        public bool Equals(PROPSPEC other)
        {
            if (HasName)
            {
                return string.Equals(PropertyName, other.PropertyName, StringComparison.Ordinal);
            }
            else
            {
                return PropertyId == other.PropertyId;
            }
        }

        //  ------------------
        //  GetHashCode method
        //  ------------------

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>

        public override int GetHashCode()
        {
            return HasName ? PropertyId ^ PropertyName.GetHashCode() : PropertyId;
        }

        //  -----------
        //  == operator
        //  -----------

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="propSpec1">The PROPSPEC1.</param>
        /// <param name="propSpec2">The PROPSPEC2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>

        public static bool operator ==(PROPSPEC propSpec1, PROPSPEC propSpec2)
        {
            return propSpec1.Equals(propSpec2);
        }

        //  -----------
        //  != operator
        //  -----------

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="propSpec1">The PROPSPEC1.</param>
        /// <param name="propSpec2">The PROPSPEC2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>

        public static bool operator !=(PROPSPEC propSpec1, PROPSPEC propSpec2)
        {
            return !propSpec1.Equals(propSpec2);
        }

        #endregion CA1815
    }
}

// eof "PROPSPEC.cs"
