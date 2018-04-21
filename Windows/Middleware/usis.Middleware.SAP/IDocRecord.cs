//
//  @(#) IDocRecord.cs
//
//  Project:    usis Middleware
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using usis.Platform;

namespace usis.Middleware.SAP
{
    //  ----------------
    //  IDocRecord class
    //  ----------------

    /// <summary>
    /// Provides a base class for intermediate document records.
    /// </summary>

    public abstract class IDocRecord : IValueStorage
    {
        #region properties

        //  -------------
        //  Data property
        //  -------------

        private string Data { get; set; }

        //  -------------------
        //  Definition property
        //  -------------------

        private IDocRecordDefinition Definition { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal IDocRecord(string data, IDocRecordDefinition definition)
        {
            Data = data;
            Definition = definition;
        }

        #endregion construction

        #region overrides

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString() { return Data; }

        #endregion overrides

        #region methods

        //  ---------------------
        //  GetDataString method
        //  ---------------------

        internal string GetDataString(int index) { return Definition.GetString(index, Data); }

        //  --------------------
        //  SetDataString method
        //  --------------------

        internal void SetDataString(int index, string s)
        {
            Data = Definition.SetString(index, Data, s);
            DataStringChanged?.Invoke(this, new DataStringChangedEventArgs(this, index));
        }

        #endregion methods

        #region events

        //  -----------------------
        //  DataStringChanged event
        //  -----------------------

        internal event EventHandler<DataStringChangedEventArgs> DataStringChanged;

        #endregion events

        #region IValueStorage implementation

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets the name of the IDoc record.
        /// </summary>
        /// <value>
        /// The name of the IDoc record.
        /// </value>

        public abstract string Name { get; }

        //  -------------------
        //  ValueNames property
        //  -------------------

        /// <summary>
        /// Gets an enumerator to iterate all field names in the record.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all field names in the record.
        /// </value>

        public IEnumerable<string> ValueNames => Definition.FieldNames();

        //  ---------------
        //  Values property
        //  ---------------

        /// <summary>
        /// Gets an enumerator to iterate all fields in the record.
        /// </summary>
        /// <value>
        /// An enumerator to iterate all fields in the record.
        /// </value>

        public IEnumerable<INamedValue> Values => Definition.GetFields(Data);

        //  ---------------
        //  GetValue method
        //  ---------------

        /// <summary>
        /// Retrieves the value with the specified field name.
        /// </summary>
        /// <param name="name">The name of the field whose value is to be retrieved.</param>
        /// <returns>
        /// A type that implements <see cref="INamedValue" /> and represents the specified field,
        /// or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if the field does not exist.
        /// </returns>

        public virtual INamedValue GetValue(string name) { return Definition.GetField(name, Data); }

        //  ---------------
        //  SetValue method
        //  ---------------

        /// <summary>
        /// Saves the specified named value as field in the record.
        /// </summary>
        /// <param name="value">The named value to save.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is a <c>null</c> reference.</exception>

        public void SetValue(INamedValue value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            var index = Definition.IndexOfField(value.Name);
            if (index >= 0) SetDataString(index, Convert.ToString(value.Value, CultureInfo.InvariantCulture));
        }

        //  ------------------
        //  DeleteValue method
        //  ------------------

        /// <summary>
        /// Deletes the field value with the specified name.
        /// </summary>
        /// <param name="name">The name of the value to delete.</param>
        /// <returns>
        /// <c>true</c> when value was deleted
        /// or
        /// <c>false</c> when a value with the specified name does not exist.
        /// </returns>

        public bool DeleteValue(string name)
        {
            throw new NotImplementedException();
        }

        #endregion IValueStorage implementation

        #region DataStringChangedEventArgs class

        //  --------------------------------
        //  DataStringChangedEventArgs class
        //  --------------------------------

        internal class DataStringChangedEventArgs : EventArgs
        {
            #region construction

            //  ------------
            //  construction
            //  ------------

            public DataStringChangedEventArgs(IDocRecord record, int index) { Record = record; Index = index; }

            #endregion construction

            #region properties

            //  ---------------
            //  Record property
            //  ---------------

            public IDocRecord Record { get; }

            //  --------------
            //  Index property
            //  --------------

            public int Index { get; }

            #endregion properties
        }

        #endregion DataStringChangedEventArgs class
    }
}

// eof "IntermediateDocumentRecord.cs"
