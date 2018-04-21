//
//  @(#) BindingProperty.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using System.Reflection;

namespace usis.Platform.Portable.Data
{
    //  ---------------------
    //  BindingProperty class
    //  ---------------------

    /// <summary>
    /// Provides an implementation of the <see cref="IBindingProperty"/> interface
    /// that allows data binding of properties via reflection.
    /// </summary>
    /// <seealso cref="IBindingProperty" />

    public class BindingProperty : IBindingProperty
    {
        #region fields

        private object sourceObject;
        private PropertyInfo propertyInfo;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingProperty"/> class.
        /// </summary>
        /// <param name="source">The object to bind to.</param>
        /// <param name="name">The name of the property to bind.</param>
        /// <exception cref="ArgumentNullException"></exception>

        public BindingProperty(object source, string name)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            propertyInfo = source.GetType().GetTypeInfo().GetDeclaredProperty(name);
            sourceObject = source;
        }

        #endregion construction

        #region properties

        //  -------------
        //  Type property
        //  -------------

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <value>
        /// The type of the property.
        /// </value>

        public Type Type { get { return propertyInfo.PropertyType; } }

        //  --------------
        //  Value property
        //  --------------

        /// <summary>
        /// Gets or sets the value of the property.
        /// </summary>
        /// <value>
        /// The value of the property.
        /// </value>

        public object Value
        {
            get { return propertyInfo.GetValue(sourceObject); }
            set { propertyInfo.SetValue(sourceObject, value); }
        }

        #endregion properties
    }
}

// eof "BindingProperty.cs"
