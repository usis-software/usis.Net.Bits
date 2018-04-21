//
//  @(#) ProcessDefinition.cs
//
//  Project:    usis Workflow Server
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

#pragma warning disable 1591

using System;
using System.Collections.Generic;

namespace usis.Workflow
{
    //  -----------------------
    //  ProcessDefinition class
    //  -----------------------

    /// <summary>
    /// Represents a process definition of the workflow engine.
    /// </summary>
    /// <seealso cref="WapiEntity{ProcessDefinitionId, ProcessDefinitionData}" />
    /// <seealso cref="IHasState{ProcessDefinitionState}" />

    public sealed class ProcessDefinition : WapiEntity<ProcessDefinitionId, ProcessDefinitionData>,
        IHasState<ProcessDefinitionState>
    {
        #region construction

        //  -----------
        //  constructor
        //  -----------

        internal ProcessDefinition(Session session, ProcessDefinitionData data) : base(session, data) { }

        #endregion construction

        #region properties

        //  -------------
        //  Name property
        //  -------------

        /// <summary>
        /// Gets the name of the process definition.
        /// </summary>
        /// <value>
        /// The name of the process definition.
        /// </value>

        public string Name { get { return Data.Name; } }

        //  --------------
        //  State property
        //  --------------

        /// <summary>
        /// Gets the state of the process definition.
        /// </summary>
        /// <value>
        /// The state of the process definition.
        /// </value>

        public ProcessDefinitionState State { get { return Data.State; } }

        //  --------------------
        //  Description property
        //  --------------------

        /// <summary>
        /// Gets the description of the process definition.
        /// </summary>
        /// <value>
        /// The description of the process definition.
        /// </value>

        public string Description { get { return Data.Description; } }

        //  ----------------
        //  Created property
        //  ----------------

        /// <summary>
        /// Gets the date and time when the process definition was created.
        /// </summary>
        /// <value>
        /// The date and time when the process definition was created.
        /// </value>

        public DateTime Created { get { return Data.Created; } }

        //  ----------------
        //  Changed property
        //  ----------------

        /// <summary>
        /// Gets the date and time when the process definition was last changed.
        /// </summary>
        /// <value>
        /// The date and time when the process definition was last changed.
        /// </value>

        public DateTime? Changed { get { return Data.Changed; } }

        #endregion properties

        #region instance methods

        //  ---------------------------
        //  QueryAvailableStates method
        //  ---------------------------

        /// <summary>
        /// Queries the available states of the process definition.
        /// </summary>
        /// <param name="filter">The filter to restrict the returned states.</param>
        /// <returns>
        /// An enumerator to iterate the available states.
        /// </returns>

        public IEnumerable<ProcessDefinitionState> QueryAvailableStates(Filter filter)
        {
            return Session.Invoke((e) => e.QueryProcessDefinitionStates(Id, filter).Iterate());
        }

        //  ------------------
        //  ChangeState method
        //  ------------------

        /// <summary>
        /// Changes the state of the process definition.
        /// </summary>
        /// <param name="newState">The new state.</param>

        public void ChangeState(ProcessDefinitionState newState)
        {
            Session.Invoke((e) => e.ChangeProcessDefinitionState(Id, newState));
        }

        //  -------------
        //  Delete method
        //  -------------

        /// <summary>
        /// Deletes this process definition.
        /// </summary>

        public void Delete()
        {
            Session.Invoke((e) => e.DeleteProcessDefinition(Id));
        }

        //  ---------------------
        //  CreateInstance method
        //  ---------------------

        /// <summary>
        /// Creates a new instance for the process definition with the specified name.
        /// </summary>
        /// <param name="name">The name for the new process instance.</param>
        /// <returns>
        /// The newly created process instance.
        /// </returns>

        public ProcessInstance CreateInstance(string name)
        {
            return new ProcessInstance(Session, Session.Invoke((e) => e.CreateProcessInstance(Id, name).ThrowOrReturnValue()));
        }

        //  ---------------------------
        //  ChangeInstancesState method
        //  ---------------------------

        public void ChangeInstancesState(Filter filter)
        {
            Session.Invoke((e) => e.ChangeProcessInstancesState(Id, filter));
        }

        //  -------------------------
        //  TerminateInstances method
        //  -------------------------

        public void TerminateInstances(Filter filter)
        {
            Session.Invoke((e) => e.TerminateProcessInstances(Id, filter));
        }

        #endregion instance methods

        #region overrides

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString() { return Name; }

        #endregion overrides
    }
}

// eof "ProcessDefinition.cs"
