//
//  @(#) IInjectable.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;

namespace usis.Platform.Portable
{
    //  -------------------------------
    //  IInjectable<TDependency> method
    //  -------------------------------

    /// <summary>
    /// Provides a definition to implement the "dependency injection" pattern.
    /// </summary>
    /// <typeparam name="TDependency">
    /// The type of the dependency.
    /// </typeparam>

    [Obsolete("Use type from usis.Platform namespace instead.")]
    public interface IInjectable<TDependency>
    {
        //  -------------
        //  Inject method
        //  -------------

        /// <summary>
        /// Passes a dependency to the implementing (dependend) object.
        /// </summary>
        /// <param name="dependency">
        /// The type of the dependency.
        /// </param>

        void Inject(TDependency dependency);
    }
}

// eof "IInjectable.cs"