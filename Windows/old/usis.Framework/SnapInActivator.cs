//
//  @(#) SnapInActivator.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using usis.Framework.Portable;

namespace usis.Framework
{
    //  ---------------------
    //  SnapInActivator class
    //  ---------------------

    /// <summary>
    /// Provides a class to create a snap-in instance.
    /// </summary>
    /// <seealso cref="Portable.SnapInActivator" />

    [Obsolete("Use type from usis.Framework.Windows assembly instead.")]
    public class SnapInActivator : Portable.SnapInActivator
    {
        //  ---------------------
        //  CreateInstance method
        //  ---------------------

        /// <summary>
        /// Creates an instance of a snap-in.
        /// </summary>
        /// <param name="application">The application in which the snap-in instance is created.</param>
        /// <param name="context">The context to create the snap-in instance.</param>
        /// <returns>
        /// The created snap-in instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"><i>context</i> is a null reference (<b>Nothing</b> in Visual Basic).</exception>

        public override object CreateInstance(IApplication application, SnapInActivatorContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (!string.IsNullOrWhiteSpace(context.TypeName) && !string.IsNullOrWhiteSpace(context.AssemblyFile))
            {
                var handle = Activator.CreateInstanceFrom(context.AssemblyFile, context.TypeName);
                return handle?.Unwrap();
            }
            else return base.CreateInstance(application, context);
        }
    }
}

// eof "SnapInActivator.cs"
