//
//  @(#) BindingExtensions.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;

namespace usis.Platform.Data
{
    //  -----------------------
    //  BindingExtensions class
    //  -----------------------

    /// <summary>
    /// Provides extension methods for data binding types.
    /// </summary>

    public static class BindingExtensions
    {
        #region SetBinding methods

        //  -----------------
        //  SetBinding method
        //  -----------------

        /// <summary>
        /// Sets a binding for the specified target property.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="targetPropertyName">Name of the target property.</param>
        /// <param name="binding">The binding.</param>
        /// <exception cref="ArgumentNullException"></exception>

        internal static void SetBinding(this IBindingTarget target, string targetPropertyName, Binding binding)
        {
            if (binding == null) throw new ArgumentNullException(nameof(binding));
            binding.Target = new BindingProperty(target, targetPropertyName);
            target.SetBindings(binding);
        }

        /// <summary>
        /// Sets the binding for the target property to a specified source property.
        /// </summary>
        /// <typeparam name="TBindingTarget">The type of the binding target.</typeparam>
        /// <param name="target">The binding target.</param>
        /// <param name="targetPropertyName">Name of the target property.</param>
        /// <param name="source">The binding source.</param>
        /// <param name="sourcePropertyName">Name of the source property.</param>
        /// <returns>The binding target.</returns>

        public static TBindingTarget SetBinding<TBindingTarget>(this TBindingTarget target, string targetPropertyName, object source, string sourcePropertyName)
            where TBindingTarget : IBindingTarget
        {
            target.SetBinding(targetPropertyName, new Binding(new BindingProperty(source, sourcePropertyName)));
            return target;
        }

        #endregion SetBinding methods
    }
}

// eof "BindingExtensions.cs"
