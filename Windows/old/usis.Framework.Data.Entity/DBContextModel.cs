//
//  @(#) DBContextModel.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Data.Entity;
using System.Diagnostics;

namespace usis.Framework.Data.Entity
{
    //  --------------------
    //  DBContextModel class
    //  --------------------

    /// <summary>
    /// Provides a base class for models that access a Entity Framework database context.
    /// </summary>
    /// <typeparam name="TDBContext">The type of the database context.</typeparam>
    /// <seealso cref="DataSourceModel" />

    public abstract class DBContextModel<TDBContext> : DataSourceModel where TDBContext : DbContext
    {
        #region properties

        //  -----------------------
        //  LoggingEnabled property
        //  -----------------------

        /// <summary>
        /// Gets or sets a value indicating whether EF logging is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if EF logging is enabled; otherwise, <c>false</c>.
        /// </value>

        protected bool LoggingEnabled { get; set; }

        #endregion properties

        #region overrides

        //  --------------
        //  OnStart method
        //  --------------

        /// <summary>
        /// Called after all snap-ins of an application are loaded and connected.
        /// </summary>
        /// <remarks>The base class implementation calls the EF database initializer.</remarks>

        protected override void OnStart()
        {
            // run the database initializer
            UsingContext((context) => context.Database.Initialize(false));

            base.OnStart();
        }

        #endregion overrides

        #region protected methods

        //  -----------------
        //  NewContext method
        //  -----------------

        /// <summary>
        /// Must be implemented in a derived class to create a new database context object.
        /// </summary>
        /// <returns>A newly created database context object.</returns>

        protected abstract TDBContext NewContext();

        /// <summary>
        /// Called to logs Entity Framework activities.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <remarks>
        /// THe base class implementation writes the text to the trace listeners in the
        /// <see cref="Debug.Listeners"/> collection.
        /// </remarks>

        protected virtual void Log(string text) { Debug.Write(text); }

        #endregion protected methods

        #region public methods

        //  --------------------
        //  CreateContext method
        //  --------------------

        /// <summary>
        /// Creates and configures the EF database context object to access
        /// the model's database.
        /// </summary>
        /// <returns>A newly created and configured database context object.</returns>

        public TDBContext CreateContext()
        {
            var context = NewContext();
            if (LoggingEnabled) context.Database.Log = Log;
            return context;
        }

        //  -------------------
        //  UsingContext method
        //  -------------------

        /// <summary>
        /// Invokes the specified action and wraps the call by using a context created by <see cref="CreateContext"/>.
        /// </summary>
        /// <param name="action">The action to invoks.</param>
        /// <exception cref="ArgumentNullException">action</exception>

        public void UsingContext(Action<TDBContext> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            using (var context = CreateContext()) { action.Invoke(context); }
        }

        /// <summary>
        /// Invokes the specified function and wraps the call by using a context created by <see cref="CreateContext"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="function">The function to invoke.</param>
        /// <returns>The function's return value.</returns>
        /// <exception cref="ArgumentNullException">function</exception>

        public TResult UsingContext<TResult>(Func<TDBContext, TResult> function)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));
            using (var context = CreateContext()) { return function.Invoke(context); }
        }

        #endregion public methods
    }
}

// eof "DBContextModel.cs"
