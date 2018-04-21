//
//  @(#) Session.cs
//
//  Project:    usis Workflow Server
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using usis.Framework;
using usis.Platform.ServiceModel;

namespace usis.Workflow
{
    //  -------------
    //  Session class
    //  -------------

    /// <summary>
    /// Represents a connection that is established between a client
    /// and a workflow management engine.
    /// </summary>
    /// <seealso cref="IDisposable" />

    public sealed class Session : IDisposable
    {
        #region construction/destruction

        //  -----------
        //  constructor
        //  -----------

        /// <summary>
        /// Initializes a new instance of the <see cref="Session"/> class.
        /// </summary>

        public Session() : this("localhost", null, null, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Session"/> class.
        /// </summary>
        /// <param name="engine">The host name of the engine to connect to.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "scope")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "user")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "password")]
        public Session(string engine, string scope, string user, string password)
        {
            Host = engine;
        }

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>

        public void Dispose() { }

        #endregion construction/destruction

        #region properties

        //  -------------
        //  Host property
        //  -------------

        private string Host { get; }

        #endregion properties

        #region client and internal methods

        #region WapiEngineClient class

        //  ----------------------
        //  WapiEngineClient class
        //  ----------------------

        private class WapiEngineClient : NamedPipeServiceClient<IWapiEngine>
        {
            internal WapiEngineClient(string host) : base(host, "Wapi") { }
        }

        #endregion WapiEngineClient class

        //  -------------------
        //  CreateClient method
        //  -------------------

        private WapiEngineClient CreateClient() { return new WapiEngineClient(Host); }

        //  -------------
        //  Invoke method
        //  -------------

        internal TResult Invoke<TResult>(Func<IWapiEngine, TResult> function)
        {
            using (var client = CreateClient()) return function.Invoke(client.Service);
        }

        internal void Invoke(Func<IWapiEngine, OperationResult> action)
        {
            using (var client = CreateClient())
            {
                action.Invoke(client.Service).ThrowOnFail();
            }
        }

        #endregion client and internal methods

        #region instance methods

        #region process definitions

        //  ------------------------------
        //  CreateProcessDefinition method
        //  ------------------------------

        /// <summary>
        /// Creates a new process definition.
        /// </summary>
        /// <param name="name">The name of the new process definition.</param>
        /// <returns>
        /// The newly created process definition.
        /// </returns>

        public ProcessDefinition CreateProcessDefinition(string name)
        {
            return Invoke((e) => new ProcessDefinition(this, e.CreateProcessDefinition(name).ThrowOrReturnValue()));
         }

        //  ------------------------------
        //  QueryProcessDefinitions method
        //  ------------------------------

        /// <summary>
        /// Retrieves a list of all process definitions.
        /// </summary>
        /// <returns>
        /// An enumerator to iterate all process definitions.
        /// </returns>

        public IEnumerable<ProcessDefinition> QueryProcessDefinitions()
        {
            return QueryProcessDefinitions(null);
        }

        /// <summary>
        /// Retrieves a list of process definitions that match the specified filter criteria.
        /// </summary>
        /// <param name="filter">A filter to restrict the returned list.</param>
        /// <returns>
        /// An enumerator to iterate the retrieved process definitions.
        /// </returns>

        public IEnumerable<ProcessDefinition> QueryProcessDefinitions(Filter filter)
        {
            return Invoke((e) => e.QueryProcessDefinitions(filter).Iterate().Select((d) => new ProcessDefinition(this, d)));
        }

        #endregion process definitions

        //  ----------------------------
        //  QueryProcessInstances method
        //  ----------------------------

        public IEnumerable<ProcessInstance> QueryProcessInstances()
        {
            return QueryProcessInstances(null);
        }

        public IEnumerable<ProcessInstance> QueryProcessInstances(Filter filter)
        {
            return Invoke((e) => e.QueryProcessInstances(filter).Iterate().Select((d) => new ProcessInstance(this, d)));
        }

        //  -------------------------
        //  GetProcessInstance method
        //  -------------------------

        public ProcessInstance GetProcessInstance(ProcessInstanceId id)
        {
            return Invoke((e) => new ProcessInstance(this, e.GetProcessInstance(id).ThrowOrReturnValue()));
        }

        //  -----------------------------
        //  QueryActivityInstances method
        //  -----------------------------

        public IEnumerable<ActivityInstance> QueryActivityInstances(Filter filter)
        {
            return Invoke((e) => e.QueryActivityInstances(filter).Iterate().Select((d) => new ActivityInstance(this, d)));
        }

        //  --------------------------
        //  GetActivityInstance method
        //  --------------------------

        public ActivityInstance GetActivityInstance(ActivityInstanceId id)
        {
            return Invoke((e) => new ActivityInstance(this, e.GetActivityInstance(null, id).ThrowOrReturnValue()));
        }

        #endregion instance methods
    }
}

// eof "Session.cs"
