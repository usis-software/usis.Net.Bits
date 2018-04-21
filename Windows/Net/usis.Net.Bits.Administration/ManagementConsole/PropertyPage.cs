//
//  @(#) PropertyPage.cs
//
//  Project:    usis.ManagementConsole
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016-2018 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using usis.Platform;

namespace usis.ManagementConsole
{
    //  ------------------
    //  PropertyPage class
    //  ------------------

    /// <summary>
    /// Creates a property page using a Windows Forms control. 
    /// </summary>
    /// <seealso cref="Microsoft.ManagementConsole.PropertyPage" />

    public class PropertyPage : Microsoft.ManagementConsole.PropertyPage
    {
        #region fields

        private Dictionary<string, object> changes = new Dictionary<string, object>(StringComparer.Ordinal);
        private Dictionary<string, object> values = new Dictionary<string, object>(StringComparer.Ordinal);

        #endregion fields

        #region overrides

        //  -------------------
        //  OnInitialize method
        //  -------------------

        /// <summary>
        /// Called when the page is getting activated for the first time.
        /// </summary>

        protected override void OnInitialize()
        {
            // supply control with a reference to this property page, if supported.
            (Control as IInjectable<Microsoft.ManagementConsole.PropertyPage>)?.Inject(this);
        }

        #endregion overrides

        #region methods

        //  --------------------------
        //  RecordPropertyValue method
        //  --------------------------

        internal void RecordPropertyValue(string name, object value)
        {
            values[name] = value;
        }

        //  ----------------------------
        //  RecordChangedProperty method
        //  ----------------------------

        internal void RecordChangedProperty(string name, object value)
        {
            if (string.IsNullOrWhiteSpace(name)) return;
            if (values.TryGetValue(name, out object oldValue) && oldValue.Equals(value))
            {
                changes.Remove(name);
                if (changes.Count == 0) Dirty = false;
                return;
            }
            if (!changes.TryGetValue(name, out object changedValue) || !changedValue.Equals(value))
            {
                changes[name] = value;
                Dirty = true;
            }
        }

        //  -------------------
        //  ApplyChanges method
        //  -------------------

        internal void ApplyChanges(object properties)
        {
            if (properties == null) throw new ArgumentNullException(nameof(properties));

            foreach (var item in changes)
            {
                Type type = properties.GetType();
                var property = type.GetProperty(item.Key);
                var value = Convert.ChangeType(item.Value, property.PropertyType, CultureInfo.InvariantCulture);
                property.SetValue(properties, value);
            }
        }

        #endregion methods
    }

    #region PropertyPage<T> class

    //  ---------------------
    //  PropertyPage<T> class
    //  ---------------------

    public class PropertyPage<T> : PropertyPage
    {
        //  -----------------------
        //  InjectProperties method
        //  -----------------------

        protected void InjectProperties(T properties)
        {
            (Control as IInjectable<T>)?.Inject(properties);
        }
    }

    #endregion PropertyPage<T> class
}

// eof "PropertyPage.cs"
