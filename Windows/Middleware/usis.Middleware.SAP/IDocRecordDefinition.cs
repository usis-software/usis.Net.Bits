//
//  @(#) IDocRecordDefinition.cs
//
//  Project:    usis Middleware
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using usis.Platform;

namespace usis.Middleware.SAP
{
    //  --------------------------
    //  IDocRecordDefinition class
    //  --------------------------

    /// <summary>
    /// Provides access to the fields that builds an intermediate document record.
    /// </summary>

    public class IDocRecordDefinition
    {
        #region fields

        private List<IDocFieldDefinition> fields = new List<IDocFieldDefinition>();
        private Dictionary<string, IDocFieldDefinition> names = new Dictionary<string, IDocFieldDefinition>(StringComparer.OrdinalIgnoreCase);
        private bool indexed;

        #endregion fields

        #region properties

        //  ---------------
        //  Fields property
        //  ---------------

        /// <summary>
        /// Gets the fields of the record definition.
        /// </summary>
        /// <value>
        /// The fields of the record definition.
        /// </value>

        public IEnumerable<IDocFieldDefinition> Fields { get { foreach (var field in fields) yield return field; } }

        //  -------
        //  Indexer
        //  -------

        //internal IDocFieldDefinition this[int index] => fields[index];

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal IDocRecordDefinition() { }

        #endregion construction

        #region methods

        //  ---------------
        //  AddField method
        //  ---------------

        internal void AddField(string name, int length)
        {
            AddField(new IDocFieldDefinition(name, length));
        }

        internal void AddField(IDocFieldDefinition field)
        {
            fields.Add(field);
            names.Add(field.Name, field);
            indexed = false;
        }

        //  -------------------
        //  IndexOfField method
        //  -------------------

        internal int IndexOfField(string name)
        {
            if (names.TryGetValue(name, out IDocFieldDefinition field))
            {
                return fields.IndexOf(field);
            }
            else return -1;
        }

        //  ----------------
        //  GetFields method
        //  ----------------

        internal IEnumerable<NamedValue> GetFields(string data)
        {
            ReIndex();
            foreach (var definition in fields)
            {
                var namedValue = GetField(definition, data);
                if (namedValue == null) continue;
                yield return namedValue;
            }
        }

        //  ---------------
        //  GetField method
        //  ---------------

        internal NamedValue GetField(string name, string data)
        {
            ReIndex();
            if (names.TryGetValue(name, out IDocFieldDefinition definition))
            {
                return GetField(definition, data);
            }
            else return null;
        }

        private static NamedValue GetField(IDocFieldDefinition definition, string data)
        {
            return new NamedValue(definition.Name, GetString(definition, data));
        }

        //  ----------------
        //  GetString method
        //  ----------------

        internal string GetString(int index, string data)
        {
            ReIndex();
            return GetString(fields[index], data);
        }

        private static string GetString(IDocFieldDefinition definition, string data)
        {
            Debug.Assert(definition.StartIndex.HasValue);

            string s = string.Empty;
            if (definition.StartIndex < data.Length)
            {
                s = data.CutOut(definition.StartIndex.Value, definition.Length);
                s = s.TrimEnd(CharConstants.Space);
            }
            return s;
        }

        //  ---------------
        //  SetField method
        //  ---------------

        internal string SetField(string name, string data, string s)
        {
            ReIndex();
            if (names.TryGetValue(name, out IDocFieldDefinition definition))
            {
                return SetString(definition, data, s);
            }
            else return data;
        }

        //  ----------------
        //  SetString method
        //  ----------------

        internal string SetString(int index, string data, string s)
        {
            ReIndex();
            return SetString(fields[index], data, s);
        }

        private static string SetString(IDocFieldDefinition definition, string data, string s)
        {
            Debug.Assert(definition.StartIndex.HasValue);

            var r = data.PadRight(definition.StartIndex.Value + definition.Length);
            var f = s.PadRight(definition.Length);
            r = r.Remove(definition.StartIndex.Value, definition.Length);
            r = r.Insert(definition.StartIndex.Value, f.Substring(0, definition.Length));
            return r.TrimEnd(CharConstants.Space);
        }

        //  --------------
        //  ReIndex method
        //  --------------

        internal void ReIndex()
        {
            if (indexed) return;
            int index = 0;
            foreach (var field in fields)
            {
                field.StartIndex = index;
                index += field.Length;
            }
            indexed = true;
        }

        //  -----------------
        //  FieldNames method
        //  -----------------

        internal IEnumerable<string> FieldNames()
        {
            foreach (var field in fields) yield return field.Name;
        }

        #endregion methods

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

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Fields.Count = {0}", fields.Count);
        }

        #endregion overrides
    }
}

// eof "IDocRecordDefinition.cs"
