//
//  @(#) DBContextModel.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

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

        //  -------------------------
        //  IsLoggingEnabled property
        //  -------------------------

        /// <summary>
        /// Gets or sets a value indicating whether EF logging is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if EF logging is enabled; otherwise, <c>false</c>.
        /// </value>

        public bool IsLoggingEnabled { get; set; }

        //  -------------------------------------
        //  IsAutomaticMigrationsEnabled property
        //  -------------------------------------

        /// <summary>
        /// Gets a value indicating whether automatic migrations are enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if automatic migrations are enabled; otherwise, <c>false</c>.
        /// </value>

        public bool IsAutomaticMigrationsEnabled { get; set; }

        //  ------------------------------
        //  IsDatabaseInitialized property
        //  ------------------------------

        /// <summary>
        /// Gets a value indicating whether the database is initialized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the database is initialized; otherwise, <c>false</c>.
        /// </value>

        public bool IsDatabaseInitialized { get; private set; }

        #endregion properties

        #region events

        //  -------------------------
        //  DatabaseInitialized event
        //  -------------------------

        /// <summary>
        /// Occurs when the database was initialized.
        /// </summary>

        public event EventHandler DatabaseInitialized;

        #endregion events

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="DBContextModel{TDBContext}"/> class.
        /// </summary>

        protected DBContextModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DBContextModel{TDBContext}"/> class
        /// with an action that is invoked when the databse was initialized.
        /// </summary>
        /// <param name="initialized">The action that is invoked when the databse was initialized.</param>

        protected DBContextModel(Action initialized)
        {
            DatabaseInitialized += (sender, e) => { initialized.Invoke(); };
        }

        #endregion construction

        #region overrides

        //  --------------
        //  OnStart method
        //  --------------

        /// <summary>
        /// Called after all snap-ins of an application are loaded and connected.
        /// </summary>
        /// <remarks>The base class implementation calls the EF database initializer.</remarks>

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected override void OnStart()
        {
            try
            {
                if (IsAutomaticMigrationsEnabled)
                {
                    Database.SetInitializer(
                        new MigrateDatabaseToLatestVersion<TDBContext, DbMigrationsConfiguration<TDBContext>>(
                            true,
                            new DbMigrationsConfiguration<TDBContext>()
                            {
                                AutomaticMigrationsEnabled = true,
                                AutomaticMigrationDataLossAllowed = true
                            }));
                }

                InitializeDatabase();

                IsDatabaseInitialized = true;
                DatabaseInitialized?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception exception)
            {
                Owner.ReportException(exception);
            }
            base.OnStart();
        }

        #endregion overrides

        #region protected methods

        //  -------------------------
        //  InitializeDatabase method
        //  -------------------------

        /// <summary>
        /// Called by <see cref="OnStart"/> to initializes the database.
        /// </summary>

        protected virtual void InitializeDatabase()
        {
            // run the database initializer
            UsingContext((context) => context.Database.Initialize(false));
        }

        //  -----------------
        //  NewContext method
        //  -----------------

        /// <summary>
        /// Must be implemented in a derived class to create a new database context object.
        /// </summary>
        /// <returns>A newly created database context object.</returns>

        protected abstract TDBContext NewContext();

        //  ----------
        //  Log method
        //  ----------

        /// <summary>
        /// Called to logs Entity Framework activities.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <remarks>
        /// THe base class implementation writes the text to the trace listeners in the
        /// <see cref="Debug.Listeners"/> collection.
        /// </remarks>

        protected virtual void Log(string text) { Trace.Write(text); }

        #endregion protected methods

        #region public methods

        //  --------------------
        //  CreateContext method
        //  --------------------

        /// <summary>
        /// Creates and configures an EF database context object to access the model's database.
        /// </summary>
        /// <returns>A newly created and configured database context object.</returns>

        public TDBContext CreateContext()
        {
            var context = NewContext();
            if (IsLoggingEnabled) context.Database.Log = Log;
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
