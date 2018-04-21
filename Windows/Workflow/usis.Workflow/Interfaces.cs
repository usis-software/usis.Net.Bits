//
//  @(#) Interfaces.cs
//
//  Project:    usis Workflow Server
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System.Collections.Generic;

namespace usis.Workflow
{
    #region IHasState interface

    //  -------------------
    //  IHasState interface
    //  -------------------

    /// <summary>
    /// Defines the members used to manage state.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>

    public interface IHasState<TState>
    {
        //  --------------
        //  State property
        //  --------------

        /// <summary>
        /// Gets the state of the entity.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>

        TState State { get; }

        //  ---------------------------
        //  QueryAvailableStates method
        //  ---------------------------

        /// <summary>
        /// Queries the available states.
        /// </summary>
        /// <param name="filter">
        /// The filter to restrict the returned states.
        /// </param>
        /// <returns>
        /// An enumerator to iterate the available states.
        /// </returns>

        IEnumerable<TState> QueryAvailableStates(Filter filter);

        //  ------------------
        //  ChangeState method
        //  ------------------

        /// <summary>
        /// Changes the state.
        /// </summary>
        /// <param name="newState">The new state.</param>

        void ChangeState(TState newState);
    }

    #endregion IHasState interface

    #region IHasAttributes interface

    //  ------------------------
    //  IHasAttributes interface
    //  ------------------------

    /// <summary>
    /// Defines the members used to manage attributes
    /// </summary>

    public interface IHasAttributes
    {
        //  ----------------------
        //  QueryAttributes method
        //  ----------------------

        /// <summary>
        /// Retrieves a list of attributes that match the specified filter criteria.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// An enumerator to iterate the retrieved attributes.
        /// </returns>

        IEnumerable<Attribute> QueryAttributes(Filter filter);

        //  -------------------
        //  GetAttribute method
        //  -------------------

        /// <summary>
        /// Gets the attribute with the specifed name.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <returns>
        /// The attribute with the specifed name.
        /// </returns>

        Attribute GetAttribute(string name);

        //  ----------------------
        //  AssignAttribute method
        //  ----------------------

        /// <summary>
        /// Assigns the specifed attribute to the entity.
        /// </summary>
        /// <param name="attribute">The attribute.</param>

        void AssignAttribute(Attribute attribute);
    }

    #endregion IHasAttributes interface
}

// eof "Interfaces.cs"
