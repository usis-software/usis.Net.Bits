//
//  @(#) PropertySet.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2008-2016 usis GmbH. All rights reserved.

using System;
using System.Runtime.InteropServices;
using usis.Platform.Interop;
using ComTypes = System.Runtime.InteropServices.ComTypes;

namespace usis.Platform.StructuredStorage
{
    #region PropertySetStatistics class

    //  ---------------------------
    //  PropertySetStatistics class
    //  ---------------------------

    /// <summary>
    /// Contains information about a property set.
    /// </summary>

    public class PropertySetStatistics
    {
        #region private fields

        private STATPROPSETSTG statpropsetstg;

        #endregion private fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal PropertySetStatistics(STATPROPSETSTG statistics)
        {
            statpropsetstg = statistics;

        } // constructor

        #endregion construction

        #region public properties

        //  -----------------
        //  FormatId property
        //  -----------------

        /// <summary>
        /// Gets the format identifier (<b>FMTID</b>) of the property set.
        /// </summary>

        public Guid FormatId
        {
            get { return statpropsetstg.FormatId; }
        }

        //  ----------------
        //  ClassId property
        //  ----------------

        /// <summary>
        /// Gets the class identifier (<b>CLSID</b>) of the property set.
        /// </summary>

        public Guid ClassId
        {
            get { return statpropsetstg.ClassId; }
        }

        //  ------------------------
        //  Characteristics property
        //  ------------------------

        /// <summary>
        /// Gets the flag values of the property set.
        /// </summary>

        public PropertySetCharacteristics Characteristics
        {
            get { return (PropertySetCharacteristics)statpropsetstg.Flags; }
        }

        //  ----------------
        //  Created property
        //  ----------------

        /// <summary>
        /// Gets the creation time for this property set.
        /// </summary>

        public DateTime Created
        {
            get { return statpropsetstg.Created.ToDateTime(); }
        }

        //  -----------------
        //  Accessed property
        //  -----------------

        /// <summary>
        /// Gets the last access time for this property set.
        /// </summary>

        public DateTime Accessed
        {
            get { return statpropsetstg.Accessed.ToDateTime(); }
        }

        //  -----------------
        //  Modified property
        //  -----------------

        /// <summary>
        /// Gets the last modification time for this property set.
        /// </summary>

        public DateTime Modified
        {
            get { return statpropsetstg.Modified.ToDateTime(); }
        }

        #endregion public properties
    }

    #endregion PropertySetStatistics class

    //  -----------------
    //  PropertySet class
    //  -----------------

    /// <summary>
    /// Manages a collection of persistent properties that are stored in a
    /// stream or storage.
    /// </summary>

    public sealed class PropertySet : IDisposable
    {
        #region private fields

        private IPropertyStorage storage;
        private PropertySetStatistics statistics;

        #endregion private fields

        #region construction/destruction

        //  ------------------------
        //  construction/destruction
        //  ------------------------

        private PropertySet(IPropertyStorage storage)
        {
            this.storage = storage;
        }

        /// <summary>
        /// This member overrides <see cref="object.Finalize"/>.
        /// </summary>

        ~PropertySet()
        {
            Dispose();
        }

        #endregion construction/destruction

        #region IDisposable members

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Releases unmanaged resources.
        /// </summary>

        public void Dispose()
        {
            if (storage != null)
            {
                Marshal.ReleaseComObject(storage);
                storage = null;
            }
            GC.SuppressFinalize(this);

        } // Dispose method

        #endregion IDisposable members

        #region public properties

        //  -------------------
        //  Statistics property
        //  -------------------

        /// <summary>
        /// Gets an <see cref="PropertySetStatistics"/> object that
        /// contains statistical information about the open property set.
        /// </summary>

        public PropertySetStatistics Statistics
        {
            get
            {
                if (statistics == null)
                {
                    STATPROPSETSTG stat = new STATPROPSETSTG();
                    storage.Stat(out stat);
                    statistics = new PropertySetStatistics(stat);
                }
                return statistics;
            }
        }

        //  ---------------------
        //  ComInterface property
        //  ---------------------
        //  Gets the encapsulated COM interface.

        /// <summary>
        /// Gets the encapsulated COM interface.
        /// </summary>
        /// <value>
        /// The encapsulated COM interface.
        /// </value>

        public IPropertyStorage ComInterface
        {
            get { return storage; }
        }

        #endregion public properties

        #region unused code

        //  ---------------------
        //  ComInterface property
        //  ---------------------

        ///// <summary>
        ///// Gets the encapsulated COM interface.
        ///// </summary>

        //[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        //internal IPropertyStorage ComInterface
        //{
        //    get
        //    {
        //        return this.storage;
        //    }

        //} // ComInterface property

        //  --------------------------
        //  ComReferenceCount property
        //  --------------------------

        ///// <summary>
        ///// Gets the current reference counter of the underlying COM object,
        ///// </summary>

        //[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        //internal int ComReferenceCount
        //{
        //    get
        //    {
        //        return Marshal.Release(Marshal.GetIUnknownForObject(this.storage));
        //    }

        //} // ComReferenceCount property

        #endregion unused code

        #region public methods

        #region static methods

        //  -------------
        //  Attach method
        //  -------------

        /// <summary>
        /// Creates a new <b>PropertySet</b> object by attaching the specified
        /// property storage.
        /// </summary>
        /// <param name="propertyStorage">
        /// A COM object that implements the
        /// <see cref="IPropertyStorage"/> interface.
        /// </param>
        /// <returns>
        /// A newly created <see cref="PropertySet"/> object.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <i>propertyStorage</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>

        internal static PropertySet Attach(IPropertyStorage propertyStorage)
        {
            if (propertyStorage == null) throw new ArgumentNullException(nameof(propertyStorage));

            // get IUnknown (increments reference counter)
            IntPtr unknown = Marshal.GetIUnknownForObject(propertyStorage);

            PropertySet propertySet = null;
            PropertySet propertySetTemp = null;
            try
            {
                // create a new RCW (runtime callable wrapper)
                propertySetTemp = new PropertySet(Marshal.GetUniqueObjectForIUnknown(unknown) as IPropertyStorage);
                Marshal.Release(unknown);

                propertySet = propertySetTemp;
                propertySetTemp = null;
            }
            finally
            {
                if (propertySetTemp != null)
                {
                    propertySetTemp.Dispose();
                }
            }
            return propertySet;
        }

        //  ---------------------
        //  NameToFormatId method
        //  ---------------------

        /// <summary>
        /// Converts a property set storage or stream name to its format identifier.
        /// </summary>
        /// <param name="name">
        /// A string that contains the stream name of a simple property set
        /// or the storage name of a nonsimple property set.
        /// </param>
        /// <returns>
        /// A <see cref="Guid"/> that represents the format identifier
        /// of the property set specified by <i>name</i>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <i>name</i> is a null reference (<b>Nothing</b> in Visual Basic).
        /// </exception>
        /// <exception cref="COMException">
        /// STG_E_INVALIDNAME: The <i>name</i> argument is invalid.
        /// </exception>

        public static Guid NameToFormatId(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            Guid formatId = Guid.Empty;
            int hr = NativeMethods.PropStgNameToFmtId(name, ref formatId);
            Marshal.ThrowExceptionForHR(hr);
            return formatId;
        }

        #endregion static methods

        #region Get... methods

        //  ----------------
        //  GetString method
        //  ----------------

        /// <summary>
        /// Gets the string with the specified property ID from the
        /// current property set.
        /// </summary>
        /// <param name="propertyId">
        /// The property ID of the property.
        /// </param>
        /// <returns>
        /// The string stored in the property set or an empty string if the string
        /// with the specified property ID does not exist in the property set.
        /// </returns>
        /// <remarks>
        /// The type indicator of a string is <b>VT_BSTR</b>.
        /// </remarks>

        public string GetString(int propertyId)
        {
            using (Property property = new Property(propertyId))
            {
                if (property.Read(storage, VarEnum.VT_BSTR))
                {
                    unsafe
                    {
                        return new string((char*)property.DataPointer);
                    }
                }
                else return string.Empty;
            }
        }

        //  --------------------
        //  GetAnsiString method
        //  --------------------

        /// <summary>
        /// Gets the ANSI string with the specified property ID from the
        /// current property set.
        /// </summary>
        /// <param name="propertyId">
        /// The property ID of the property.
        /// </param>
        /// <returns>
        /// The string stored in the property set or an empty string if the string
        /// with the specified property ID does not exist in the property set.
        /// </returns>
        /// <remarks>
        /// The type indicator of a ANSI string is <b>VT_LPSTR</b>.
        /// </remarks>

        public string GetAnsiString(int propertyId)
        {
            using (Property property = new Property(propertyId))
            {
                if (property.Read(storage, VarEnum.VT_LPSTR))
                {
                    unsafe
                    {
                        return new string((sbyte*)property.DataPointer);
                    }
                }
                else return string.Empty;
            }
        }

        //  -----------------------
        //  GetUnicodeString method
        //  -----------------------

        /// <summary>
        /// Gets the unicode string with the specified property ID from the
        /// current property set.
        /// </summary>
        /// <param name="propertyId">
        /// The property ID of the property.
        /// </param>
        /// <returns>
        /// The string stored in the property set or an empty string if the string
        /// with the specified property ID does not exist in the property set.
        /// </returns>
        /// <remarks>
        /// The type indicator of a unicode string is <b>VT_LPWSTR</b>.
        /// </remarks>

        public string GetUnicodeString(int propertyId)
        {
            using (Property property = new Property(propertyId))
            {
                if (property.Read(storage, VarEnum.VT_LPWSTR))
                {
                    unsafe
                    {
                        return new string((char*)property.DataPointer);
                    }
                }
                else return string.Empty;
            }
        }

        //  --------------
        //  GetGuid method
        //  --------------

        /// <summary>
        /// Gets the GUID with the specified property ID from the
        /// current property set.
        /// </summary>
        /// <param name="propertyId">
        /// The property ID of the property.
        /// </param>
        /// <returns>
        /// The GUID stored in the property set or an empty GUID
        /// if the GUID with the specified property ID
        /// does not exist in the property set.
        /// </returns>
        /// <remarks>
        /// The type indicator of a GUID is <b>VT_CLSID</b>.
        /// </remarks>

        public Guid GetGuid(int propertyId)
        {
            using (Property property = new Property(propertyId))
            {
                if (property.Read(storage, VarEnum.VT_CLSID))
                {
                    unsafe
                    {
                        return *(Guid*)property.DataPointer;
                    }
                }
                else return Guid.Empty;
            }
        }

        //  ----------------
        //  GetUInt32 method
        //  ----------------

        /// <summary>
        /// Gets the unsigned 32-bit integer with the specified property ID
        /// from the current property set.
        /// </summary>
        /// <param name="propertyId">
        /// The property ID of the property.
        /// </param>
        /// <returns>
        /// The unsigned 32-bit integer stored in the property set or 0
        /// if the unsigned 32-bit integer with the specified property ID
        /// does not exist in the property set.
        /// </returns>
        /// <remarks>
        /// The type indicator of a unsigned 32-bit integer is <b>VT_UI4</b>.
        /// </remarks>

        [CLSCompliant(false)]
        public uint GetUInt32(int propertyId)
        {
            using (Property property = new Property(propertyId))
            {
                if (property.Read(storage, VarEnum.VT_UI4))
                {
                    return (uint)property.Data.ToInt32();
                }
                else return 0;
            }
        }

        //  ----------------
        //  GetUInt64 method
        //  ----------------

        /// <summary>
        /// Gets the unsigned 64-bit integer with the specified property ID
        /// from the current property set.
        /// </summary>
        /// <param name="propertyId">
        /// The property ID of the property.
        /// </param>
        /// <returns>
        /// The unsigned 64-bit integer stored in the property set or 0
        /// if the unsigned 64-bit integer with the specified property ID
        /// does not exist in the property set.
        /// </returns>
        /// <remarks>
        /// The type indicator of a unsigned 64-bit integer is <b>VT_UI8</b>.
        /// </remarks>

        [CLSCompliant(false)]
        public ulong GetUInt64(int propertyId)
        {
            using (Property property = new Property(propertyId))
            {
                if (property.Read(storage, VarEnum.VT_UI8))
                {
                    return (ulong)property.Data.ToInt64();
                }
                else return 0;
            }
        }

        //  ---------------
        //  GetInt32 method
        //  ---------------

        /// <summary>
        /// Gets the signed 32-bit integer with the specified property ID
        /// from the current property set.
        /// </summary>
        /// <param name="propertyId">
        /// The property ID of the property.
        /// </param>
        /// <returns>
        /// The signed 32-bit integer stored in the property set or 0
        /// if the signed 32-bit integer with the specified property ID
        /// does not exist in the property set.
        /// </returns>
        /// <remarks>
        /// The type indicator of a signed 32-bit integer is <b>VT_I4</b>.
        /// </remarks>

        public int GetInt32(int propertyId)
        {
            using (Property property = new Property(propertyId))
            {
                if (property.Read(storage, VarEnum.VT_I4))
                {
                    return property.Data.ToInt32();
                }
                else return 0;
            }
        }

        //  ---------------
        //  GetInt16 method
        //  ---------------

        /// <summary>
        /// Gets the signed 16-bit integer with the specified property ID
        /// from the current property set.
        /// </summary>
        /// <param name="propertyId">
        /// The property ID of the property.
        /// </param>
        /// <returns>
        /// The signed 16-bit integer stored in the property set or 0
        /// if the signed 16-bit integer with the specified property ID
        /// does not exist in the property set.
        /// </returns>
        /// <remarks>
        /// The type indicator of a signed 16-bit integer is <b>VT_I2</b>.
        /// </remarks>

        public short GetInt16(int propertyId)
        {
            using (Property property = new Property(propertyId))
            {
                if (property.Read(storage, VarEnum.VT_I2))
                {
                    return (short)property.Data.ToInt32();
                }
                else return 0;
            }
        }

        //  --------------
        //  GetDate method
        //  --------------

        /// <summary>
        /// Gets the date value with the specified property ID
        /// from the current property set.
        /// </summary>
        /// <param name="propertyId">
        /// The property ID of the property.
        /// </param>
        /// <returns>
        /// The date value stored in the property set
        /// or <see cref="DateTime.MinValue"/> if the date value
        /// with the specified property ID does not exist in the property set.
        /// </returns>
        /// <remarks>
        /// The type indicator of a date value is <b>VT_DATE</b>.
        /// </remarks>

        public DateTime GetDate(int propertyId)
        {
            using (Property property = new Property(propertyId))
            {
                if (property.Read(storage, VarEnum.VT_DATE))
                {
                    double date = property.ToDouble();
                    return DateTime.FromOADate(date);
                }
                else return DateTime.MinValue;
            }
        }

        //  -----------------
        //  GetBoolean method
        //  -----------------

        /// <summary>
        /// Gets the boolean value with the specified property ID
        /// from the current property set.
        /// </summary>
        /// <param name="propertyId">
        /// The property ID of the property.
        /// </param>
        /// <returns>
        /// The boolean value stored in the property set or false
        /// if the boolean value with the specified property ID
        /// does not exist in the property set.
        /// </returns>
        /// <remarks>
        /// The type indicator of a boolean value is <b>VT_BOOL</b>.
        /// </remarks>

        public bool GetBoolean(int propertyId)
        {
            return GetBoolean(propertyId, false);
        }

        /// <summary>
        /// Gets the boolean value with the specified property ID
        /// from the current property set.
        /// </summary>
        /// <param name="propertyId">
        /// The property ID of the property.
        /// </param>
        /// <param name="defaultValue">
        /// The default value to return if the specified
        /// property does not exist in the property set.
        /// </param>
        /// <returns>
        /// The boolean value stored in the property set or
        /// the specified default value
        /// if the boolean value with the specified property ID
        /// does not exist in the property set.
        /// </returns>
        /// <remarks>
        /// The type indicator of a boolean value is <b>VT_BOOL</b>.
        /// </remarks>

        public bool GetBoolean(int propertyId, bool defaultValue)
        {
            using (Property property = new Property(propertyId))
            {
                if (property.Read(storage, VarEnum.VT_BOOL))
                {
                    return property.Data != IntPtr.Zero;
                }
                else return defaultValue;
            }
        }

        //  --------------------
        //  TryGetBoolean method
        //  --------------------

        /// <summary>
        /// Tries to get the boolean value with the specified property ID
        /// from the current property set.
        /// </summary>
        /// <param name="propertyId">
        /// The property ID of the property.
        /// </param>
        /// <returns>
        /// The boolean value stored in the property set or <b>null</b>
        /// if the boolean value with the specified property ID
        /// does not exist in the property set.
        /// </returns>
        /// <remarks>
        /// The type indicator of a boolean value is <b>VT_BOOL</b>.
        /// </remarks>

        public bool? TryGetBoolean(int propertyId)
        {
            using (Property property = new Property(propertyId))
            {
                if (property.Read(storage, VarEnum.VT_BOOL))
                {
                    return property.Data != IntPtr.Zero;
                }
                else return null;
            }
        }

        //  ----------------
        //  GetStream method
        //  ----------------

        /// <summary>
        /// Gets the stream with the specified property ID
        /// from the current property set.
        /// </summary>
        /// <param name="propertyId">
        /// The property ID of the property.
        /// </param>
        /// <returns>
        /// The stream stored in the property set or <b>null</b>
        /// if the stream with the specified property ID
        /// does not exist in the property set.
        /// </returns>
        /// <remarks>
        /// The type indicator of a stream is <b>VT_STREAM</b>.
        /// </remarks>

        public StorageStream GetStream(int propertyId)
        {
            using (Property property = new Property(propertyId))
            {
                if (property.Read(storage, VarEnum.VT_STREAM))
                {
                    object o = Marshal.GetTypedObjectForIUnknown(property.Data, typeof(ComTypes.IStream));
                    return new StorageStream(o as ComTypes.IStream);
                }
                else return null;
            }
        }

        //  -----------------
        //  GetStorage method
        //  -----------------

        /// <summary>
        /// Gets the storage with the specified property ID
        /// from the current property set.
        /// </summary>
        /// <param name="propertyId">
        /// The property ID of the property.
        /// </param>
        /// <returns>
        /// The storage stored in the property set or <b>null</b>
        /// if the storage with the specified property ID
        /// does not exist in the property set.
        /// </returns>
        /// <remarks>
        /// The type indicator of a storage is <b>VT_STORAGE</b>.
        /// </remarks>

        public Storage GetStorage(int propertyId)
        {
            using (Property property = new Property(propertyId))
            {
                if (property.Read(storage, VarEnum.VT_STORAGE))
                {
                    object o = Marshal.GetTypedObjectForIUnknown(property.Data, typeof(IStorage));
                    return new Storage(o as IStorage, string.Empty);
                }
                else return null;
            }
        }

        #endregion Get... methods

        #region Set... methods

        //  ----------------
        //  SetString method
        //  ----------------

        /// <summary>
        /// Sets the property with the specified property ID to a string value.
        /// </summary>
        /// <param name="propertyId">
        /// The ID of the property to set.
        /// </param>
        /// <param name="value">
        /// The value to set.
        /// </param>

        public void SetString(int propertyId, string value)
        {
            PROPSPEC[] propSpecs = new PROPSPEC[1];
            PROPVARIANT[] propVariants = new PROPVARIANT[1];
            propSpecs[0].PropertyId = propertyId;
            propVariants[0].SetString(value);
            storage.WriteMultiple(1, propSpecs, propVariants, 2);
        }

        //  -----------------
        //  SetBoolean method
        //  -----------------

        /// <summary>
        /// Sets the property with the specified property ID to a boolean value.
        /// </summary>
        /// <param name="propertyId">
        /// The ID of the property to set.
        /// </param>
        /// <param name="value">
        /// The value to set.
        /// </param>

        public void SetBoolean(int propertyId, bool value)
        {
            using (Property property = new Property(propertyId))
            {
                property.SetBoolean(value);
                property.Write(storage);
            }
        }

        //  --------------
        //  SetGuid method
        //  --------------

        /// <summary>
        /// Sets the property with the specified property ID to a GUID.
        /// </summary>
        /// <param name="propertyId">
        /// The ID of the property to set.
        /// </param>
        /// <param name="value">
        /// The value to set.
        /// </param>

        public void SetGuid(int propertyId, Guid value)
        {
            using (Property property = new Property(propertyId))
            {
                property.SetGuid(value);
                property.Write(storage);
            }
        }

        #endregion Set... methods

        //  -------------
        //  Delete method
        //  -------------

        /// <summary>
        /// Deletes the property with the specified property ID.
        /// </summary>
        /// <param name="propertyId">
        /// The property ID of the property to be deleted.
        /// </param>

        public void Delete(int propertyId)
        {
            storage.DeleteMultiple(1, PROPSPEC.CreateArray(propertyId));
        }

        #endregion public methods

        #region Property class

        //  --------------
        //  Property class
        //  --------------

        private class Property : IDisposable
        {
            #region private members

            PROPSPEC[] propspecs = new PROPSPEC[1];
            PROPVARIANT[] propvars = new PROPVARIANT[1];

            #endregion private members

            #region construction/destruction

            //  -----------
            //  constructor
            //  -----------

            public Property(int propertyId)
            {
                propspecs[0].PropertyId = propertyId;
            }

            //  ----------
            //  destructor
            //  ----------

            ~Property()
            {
                Dispose();
            }

            #endregion construction/destruction

            #region IDisposable members

            //  --------------
            //  Dispose method
            //  --------------

            public void Dispose()
            {
                if (propspecs != null)
                {
                    propspecs[0].Clear();
                    propspecs = null;
                }
                if (propvars != null)
                {
                    propvars[0].Clear();
                    propvars = null;
                }
                GC.SuppressFinalize(this);
            }

            #endregion IDisposable members

            #region public properties

            //  --------------------
            //  DataPointer property
            //  --------------------

            public unsafe void* DataPointer
            {
                get { return new IntPtr(propvars[0].Data).ToPointer(); }
            }

            //  -------------
            //  Data property
            //  -------------

            public IntPtr Data
            {
                get { return new IntPtr(propvars[0].Data); }
            }

            #endregion public properties

            #region public methods

            //  -----------
            //  Read method
            //  -----------

            public bool Read(IPropertyStorage storage, VarEnum vt)
            {
                storage.ReadMultiple(1, propspecs, propvars);
                return propvars[0].VT == vt;
            }

            //  ------------
            //  Write method
            //  ------------

            public void Write(IPropertyStorage storage)
            {
                storage.WriteMultiple(1, propspecs, propvars, 2);
            }

            //  ---------------
            //  ToDouble method
            //  ---------------

            public double ToDouble()
            {
                unsafe
                {
                    long data = propvars[0].Data;
                    double* pDouble = (double*)&data;
                    return *pDouble;
                }
            }

            //  -----------------
            //  SetBoolean method
            //  -----------------

            public void SetBoolean(bool b)
            {
                propvars[0].SetBoolean(b);
            }

            //  --------------
            //  SetGuid method
            //  --------------

            public void SetGuid(Guid guid)
            {
                propvars[0].SetGuid(guid);
            }

            #endregion public methods
        }

        #endregion Property class
    }
}

// eof "PropertySet.cs"
