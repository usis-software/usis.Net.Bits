//
//  @(#) IBindingTarget.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;

namespace usis.Platform.Data
{
    //  ------------------------
    //  IBindingTarget interface
    //  ------------------------

    /// <summary>
    /// Defines the method that required for an object to act as a binding target.
    /// </summary>

    public interface IBindingTarget
    {
        //  -------------------
        //  SetBindings methods
        //  -------------------

        /// <summary>
        /// Sets the specified bindings on the binding target.
        /// </summary>
        /// <param name="bindings">The bindings to set.</param>

        void SetBindings(params IBinding[] bindings);

        //  -----------
        //  Bind method
        //  -----------

        /// <summary>
        /// Performs the data binding by updating the binding target.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>

        void Bind(IFormatProvider provider);
    }

    #region BindingTarget class

    //  -------------------
    //  BindingTarget class
    //  -------------------

    /// <summary>
    /// Provides a base class implementation of the <see cref="IBindingTarget"/> interface
    /// to support data binding.
    /// </summary>
    /// <seealso cref="IBindingTarget" />

    public abstract class BindingTarget : IBindingTarget
    {
        #region fields

        private List<IBinding> bindingList = new List<IBinding>();

        #endregion fields

        #region methods

        //  ------------------
        //  SetBindings method
        //  ------------------

        /// <summary>
        /// Sets the specified bindings on the binding target.
        /// </summary>
        /// <param name="bindings">The bindings to set.</param>

        public void SetBindings(params IBinding[] bindings)
        {
            bindingList.AddRange(bindings);
        }

        //  -----------
        //  Bind method
        //  -----------

        /// <summary>
        /// Performs the data binding by updating the binding target.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>

        public void Bind(IFormatProvider provider)
        {
            foreach (var binding in bindingList)
            {
                binding.UpdateTarget(provider);
            }
        }

        #endregion methods
    }

    #endregion BindingTarget class
}

// eof "IBindingTarget.cs"
