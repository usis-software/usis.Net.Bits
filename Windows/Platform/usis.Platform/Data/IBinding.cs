//
//  @(#) IBinding.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;

namespace usis.Platform.Data
{
    //  ------------------
    //  IBinding interface
    //  ------------------

    /// <summary>
    /// Defines methods that a required to provide data binding.
    /// </summary>

    public interface IBinding
    {
        //  -------------------
        //  UpdateTarget method
        //  -------------------

        /// <summary>
        /// Updates the values of the target object.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>

        void UpdateTarget(IFormatProvider provider);
    }

    #region Binding class

    //  -------------
    //  Binding class
    //  -------------

    /// <summary>
    /// Provides data binding between a source and a target property.
    /// </summary>
    /// <seealso cref="IBinding" />

    public class Binding : IBinding
    {
        #region properties

        //  ---------------
        //  Source property
        //  ---------------

        private IBindingProperty Source { get; set; }

        //  ---------------
        //  Target property
        //  ---------------

        /// <summary>
        /// Gets or sets the target property.
        /// </summary>
        /// <value>
        /// The target property.
        /// </value>

        public IBindingProperty Target { get; set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Binding"/> class
        /// with the specified source property.
        /// </summary>
        /// <param name="sourceProperty">The source property.</param>

        public Binding(IBindingProperty sourceProperty) { Source = sourceProperty; }

        //private Binding(IBindingProperty source, IBindingProperty target) { Source = source; Target = target; }

        #endregion construction

        #region methods

        //  -------------------
        //  UpdateTarget method
        //  -------------------

        /// <summary>
        /// Updates the values of the target object.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>

        public void UpdateTarget(IFormatProvider provider)
        {
            Target.Value = ConvertSource(provider);
        }

        //  --------------------
        //  ConvertSource method
        //  --------------------

        private /*virtual*/ object ConvertSource(IFormatProvider provider)
        {
            return Convert.ChangeType(Source.Value, Target.Type, provider);
        }

        #endregion methods
    }

    #endregion Binding class
}

// eof "IBinding.cs"
