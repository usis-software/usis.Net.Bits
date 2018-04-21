//
//  @(#) IWapiEngine.cs
//
//  Project:    usis Workflow Server
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System.ServiceModel;
using usis.Framework;

namespace usis.Workflow
{
    //  ---------------------
    //  IWapiEngine interface
    //  ---------------------

    /// <summary>
    /// Defines the API to access and manage the workflow engine.
    /// </summary>

    [ServiceContract]
    public interface IWapiEngine
    {
        #region Process Definitions

        //  ------------------------------
        //  CreateProcessDefinition method
        //  ------------------------------

        /// <summary>
        /// Creates a new process definition.
        /// </summary>
        /// <param name="name">The name of the process definition.</param>
        /// <returns>
        /// An operation result with a data entity of the newly created process definition.
        /// </returns>

        [OperationContract]
        OperationResult<ProcessDefinitionData> CreateProcessDefinition(string name);

        //  ------------------------------
        //  QueryProcessDefinitions method
        //  ------------------------------

        /// <summary>
        /// Retrieves a list of all process definitions.
        /// </summary>
        /// <param name="filter">A filter to restrict the returned list.</param>
        /// <returns>
        /// An operation result with an enumerator to iterate the retrieved process definitions.
        /// </returns>

        [OperationContract]
        OperationResultList<ProcessDefinitionData> QueryProcessDefinitions(Filter filter);

        //  -----------------------------------
        //  QueryProcessDefinitionStates method
        //  -----------------------------------

        /// <summary>
        /// Queries the available states of a process definition.
        /// </summary>
        /// <param name="processDefinitionId">The process definition identifier.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// An operation result with an enumerator to iterate the retrieved process definitions states.
        /// </returns>

        [OperationContract]
        OperationResultList<ProcessDefinitionState> QueryProcessDefinitionStates(ProcessDefinitionId processDefinitionId, Filter filter);

        //  -----------------------------------
        //  ChangeProcessDefinitionState method
        //  -----------------------------------

        /// <summary>
        /// Changes the state of a process definition.
        /// </summary>
        /// <param name="processDefinitionId">The process definition identifier.</param>
        /// <param name="newState">The new state.</param>
        /// <returns>
        /// An <see cref="OperationResult" /> that describes the result of the operation.
        /// </returns>

        [OperationContract]
        OperationResult ChangeProcessDefinitionState(ProcessDefinitionId processDefinitionId, ProcessDefinitionState newState);

        //  ------------------------------
        //  DeleteProcessDefinition method
        //  ------------------------------

        /// <summary>
        /// Deletes a process definition.
        /// </summary>
        /// <param name="processDefinitionId">The process definition identifier.</param>
        /// <returns>
        /// An <see cref="OperationResult" /> that describes the result of the operation.
        /// </returns>

        [OperationContract]
        OperationResult DeleteProcessDefinition(ProcessDefinitionId processDefinitionId);

        //  ----------------------------
        //  CreateProcessInstance method
        //  ----------------------------

        /// <summary>
        /// Creates an instance from a specified process definition.
        /// </summary>
        /// <param name="processDefinitionId">The process definition identifier.</param>
        /// <param name="name">The name of the instance.</param>
        /// <returns>
        /// An <see cref="OperationResult"/> with the data of the newly created process instance.
        /// </returns>

        [OperationContract]
        OperationResult<ProcessInstanceData> CreateProcessInstance(ProcessDefinitionId processDefinitionId, string name);

        #endregion Process Definitions

#pragma warning disable 1591

        #region Process Instances

        //  ----------------------------
        //  QueryProcessInstances method
        //  ----------------------------

        [OperationContract]
        OperationResultList<ProcessInstanceData> QueryProcessInstances(Filter filter);

        //  -------------------------
        //  GetProcessInstance method
        //  -------------------------

        /// <summary>
        /// Gets the process instance with the specified process instance identifier.
        /// </summary>
        /// <param name="id">The process instance identifier.</param>
        /// <returns>
        /// An <see cref="OperationResult"/> with the data of the process instance.
        /// </returns>

        [OperationContract]
        OperationResult<ProcessInstanceData> GetProcessInstance(ProcessInstanceId id);

        //  ---------------------------
        //  StartProcessInstance method
        //  ---------------------------

        [OperationContract]
        OperationResult StartProcessInstance(ProcessInstanceId id);

        //  -------------------------------
        //  TerminateProcessInstance method
        //  -------------------------------

        [OperationContract]
        OperationResult TerminateProcessInstance(ProcessInstanceId id);

        //  ---------------------------------
        //  QueryProcessInstanceStates method
        //  ---------------------------------

        [OperationContract]
        OperationResultList<ProcessInstanceState> QueryProcessInstanceStates(ProcessInstanceId id, Filter filter);

        //  ---------------------------------
        //  ChangeProcessInstanceState method
        //  ---------------------------------

        [OperationContract]
        OperationResult ChangeProcessInstanceState(ProcessInstanceId id, ProcessInstanceState newState);

        //  -------------------------------------
        //  QueryProcessInstanceAttributes method
        //  -------------------------------------

        [OperationContract]
        OperationResultList<Attribute> QueryProcessInstanceAttributes(ProcessInstanceId id, Filter filter);

        //  ----------------------------------
        //  GetProcessInstanceAttribute method
        //  ----------------------------------

        [OperationContract]
        OperationResult<Attribute> GetProcessInstanceAttribute(ProcessInstanceId id, string name);

        //  -------------------------------------
        //  AssignProcessInstanceAttribute method
        //  -------------------------------------

        [OperationContract]
        OperationResult AssignProcessInstanceAttribute(ProcessInstanceId id, Attribute attribute);

        #endregion Process Instances

        #region Activity Instances

        //  -----------------------------
        //  QueryActivityInstances method
        //  -----------------------------

        [OperationContract]
        OperationResultList<ActivityInstanceData> QueryActivityInstances(Filter filter);

        //  --------------------------
        //  GetActivityInstance method
        //  --------------------------

        [OperationContract]
        OperationResult<ActivityInstanceData> GetActivityInstance(ProcessInstanceId processInstanceId, ActivityInstanceId id);

        //  ----------------------------------
        //  QueryActivityInstanceStates method
        //  ----------------------------------

        [OperationContract]
        OperationResultList<ActivityInstanceState> QueryActivityInstanceStates(ProcessInstanceId processInstanceId, ActivityInstanceId id, Filter filter);

        //  ----------------------------------
        //  ChangeActivityInstanceState method
        //  ----------------------------------

        [OperationContract]
        OperationResult ChangeActivityInstanceState(ProcessInstanceId processInstanceId, ActivityInstanceId id, ActivityInstanceState newState);

        //  --------------------------------------
        //  QueryActivityInstanceAttributes method
        //  --------------------------------------

        [OperationContract]
        OperationResultList<Attribute> QueryActivityInstanceAttributes(ProcessInstanceId processInstanceId, ActivityInstanceId id, Filter filter);

        //  -----------------------------------
        //  GetActivityInstanceAttribute method
        //  -----------------------------------

        [OperationContract]
        OperationResult<Attribute> GetActivityInstanceAttribute(ProcessInstanceId processInstanceId, ActivityInstanceId id, string name);

        //  --------------------------------------
        //  AssignActivityInstanceAttribute method
        //  --------------------------------------

        [OperationContract]
        OperationResult AssignActivityInstanceAttribute(ProcessInstanceId processInstanceId, ActivityInstanceId id, Attribute attribute);

        #endregion Activity Instances

        [OperationContract]
        OperationResult ChangeProcessInstancesState(ProcessDefinitionId id, Filter filter);

        //OperationResult ChangeActivityInstancesState(ProcessDefinitionId processDefinitionId, ...);

        [OperationContract]
        OperationResult TerminateProcessInstances(ProcessDefinitionId id, Filter filter);
    }
}

// eof "IWapiEngine.cs"
