//
//  @(#) IBindingProperty.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;

namespace usis.Platform.Portable.Data
{
    //  --------------------------
    //  IBindingProperty interface
    //  --------------------------

    /// <summary>
    /// Defines the members that a required to access a property during data binding
    /// with the <see cref="Binding"/> class.
    /// </summary>

    public interface IBindingProperty
    {
        //  --------------
        //  Value property
        //  --------------

        /// <summary>
        /// Gets or sets the value of the property.
        /// </summary>
        /// <value>
        /// The value of the property.
        /// </value>

        object Value { get; set; }

        //  -------------
        //  Type property
        //  -------------

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <value>
        /// The type of the property.
        /// </value>

        Type Type { get; }
    }
}

// eof "IBindingProperty.cs"
