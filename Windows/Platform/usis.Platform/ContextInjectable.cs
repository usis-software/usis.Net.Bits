//
//  @(#) ContextInjectable.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

namespace usis.Platform
{
    //  ----------------------------
    //  IContextInjectable interface
    //  ----------------------------

    /// <summary>
    /// Defines a context injectable object.
    /// </summary>
    /// <typeparam name="T"> The type of the context.</typeparam>

    public interface IContextInjectable<T> : IInjectable<T>
    {
        //  ----------------
        //  Context property
        //  ----------------

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>

        T Context { get; }
    }

    //  --------------------------
    //  ContextInjectable<T> class
    //  --------------------------

    /// <summary>
    /// Provides a base class that allows to inject a context dependancy object
    /// of the specified type.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the context object to inject.
    /// </typeparam>

    public abstract class ContextInjectable<T> : IContextInjectable<T>
    {
        #region properties

        //  ----------------
        //  Context property
        //  ----------------

        /// <summary>
        /// Gets the injected context object.
        /// </summary>
        /// <value>
        /// The injected context object.
        /// </value>

        public T Context { get; private set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextInjectable{T}"/> class.
        /// </summary>

        protected ContextInjectable() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextInjectable{T}"/> class
        /// with the specified context dependency object.
        /// </summary>
        /// <param name="dependency">
        /// The context dependency object.
        /// </param>

        protected ContextInjectable(T dependency) { Context = dependency; }

        #endregion construction

        #region methods

        //  -------------
        //  Inject method
        //  -------------
        /// <summary>
        /// Injects the specified dependency.
        /// </summary>
        /// <param name="dependency">
        /// The context dependency object.
        /// </param>

        public void Inject(T dependency) { Context = dependency; }

        #endregion methods
    }
}

// eof "ContextInjectable.cs"
