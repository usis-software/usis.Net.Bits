//
//  @(#) JsonDictionary.cs
//
//  Project:    usis.Platform
//  System:     Microsoft Visual Studio 12
//  Author:     Udo Schäfer
//
//  Copyright (c) 2015 usis GmbH. All rights reserved.

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace usis.Platform
{
    //  --------------------
    //  JsonDictionary class
    //  --------------------

    [Serializable]
    public class JsonDictionary : ISerializable, IDictionary<string, object>
    {
        #region fields

        private Dictionary<string, object> data = new Dictionary<string, object>();

        #endregion fields

        #region serialization implementation

        //  -----------
        //  constructor
        //  -----------

        protected JsonDictionary(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException("info");

            foreach (var item in info)
            {
                data.Add(item.Name, item.Value);
            }

        } // constructor

        //  --------------------
        //  GetObjectData method
        //  --------------------

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException("info");

            foreach (var item in data)
            {
                info.AddValue(item.Key, item.Value);
            }

        } // GetObjectData method

        #endregion serialization implementation

        #region IDictionary<string, object>

        public object this[string key]
        {
            get
            {
                return data[key];
            }

            set
            {
                data[key] = value;
            }
        }

        public int Count
        {
            get
            {
                return data.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                return data.Keys;
            }
        }

        public ICollection<object> Values
        {
            get
            {
                return data.Values;
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            data.Add(item.Key, item.Value);
        }

        public void Add(string key, object value)
        {
            data.Add(key, value);
        }

        public void Clear()
        {
            data.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return data.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return data.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            array = data.Skip(arrayIndex).ToArray();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return data.Remove(item.Key);
        }

        public bool Remove(string key)
        {
            return data.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return data.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        #endregion IDictionary<string, object>

        #region overrides

        //  ---------------
        //  ToString method
        //  ---------------

        public override string ToString()
        {
            return JsonConvert.SerializeObject(data);

        } // ToString method

        #endregion overrides

    } // JsonDictionary class

} // namespace usis.Platform

// eof "JsonDictionary.cs"
