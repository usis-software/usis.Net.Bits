//
//  @(#) IWapiEngine.cs
//
//  Project:    usis Workflow Server
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using usis.Framework.Portable;
using usis.Framework.ServiceModel;

namespace usis.Workflow
{
    //  ---------------------
    //  IWapiEngine interface
    //  ---------------------

    public interface IWapiEngine
    {
        #region Process Definitions

        OperationResultList<ProcessDefinitionData> QueryProcessDefinitions(Filter filter);

        OperationResultList<ProcessDefinitionState> QueryProcessDefinitionStates(ProcessDefinitionId processDefinitionId, Filter filter);

        OperationResult ChangeProcessDefinitionState(ProcessDefinitionId processDefinitionId, ProcessDefinitionState newState);

        OperationResult<ProcessInstanceData> CreateProcessInstance(ProcessDefinitionId processDefinitionId, string name);

        #endregion Process Definitions

        #region Process Instances

        OperationResultList<ProcessInstanceData> QueryProcessInstances(Filter filter);

        OperationResult<ProcessInstanceData> GetProcessInstance(ProcessInstanceId id);

        OperationResult StartProcessInstance(ProcessInstanceId id);

        OperationResult TerminateProcessInstance(ProcessInstanceId id);

        OperationResultList<ProcessInstanceState> QueryProcessInstanceStates(ProcessInstanceId id, Filter filter);

        OperationResult ChangeProcessInstanceState(ProcessInstanceId id, ProcessInstanceState newState);

        OperationResultList<Attribute> QueryProcessInstanceAttributes(ProcessInstanceId id, Filter filter);

        OperationResult<Attribute> GetProcessInstanceAttribute(ProcessInstanceId id, string name);

        OperationResult AssignProcessInstanceAttribute(ProcessInstanceId id, Attribute attribute);

        #endregion Process Instances

        #region Activity Instances

        OperationResultList<ActivityInstanceData> QueryActivityInstances(Filter filter);

        OperationResult<ActivityInstanceData> GetActivityInstance(ProcessInstanceId processInstanceId, ActivityInstanceId id);

        OperationResultList<ActivityInstanceState> QueryActivityInstanceStates(ProcessInstanceId processInstanceId, ActivityInstanceId id, Filter filter);

        OperationResult ChangeActivityInstanceState(ProcessInstanceId processInstanceId, ActivityInstanceId id, ActivityInstanceState newState);

        OperationResultList<Attribute> QueryActivityInstanceAttributes(ProcessInstanceId processInstanceId, ActivityInstanceId id, Filter filter);

        OperationResult<Attribute> GetActivityInstanceAttribute(ProcessInstanceId processInstanceId, ActivityInstanceId id, string name);

        OperationResult AssignActivityInstanceAttribute(ProcessInstanceId processInstanceId, ActivityInstanceId id, Attribute attribute);

        #endregion Activity Instances

        OperationResult ChangeProcessInstancesState(ProcessDefinitionId id, Filter filter);

        //OperationResult ChangeActivityInstancesState(ProcessDefinitionId processDefinitionId, ...);

        OperationResult TerminateProcessInstances(ProcessDefinitionId id, Filter filter);
    }

    #region WapiEngineClient class

    //  ----------------------
    //  WapiEngineClient class
    //  ----------------------

    internal class WapiEngineClient : NamedPipeClientBase<IWapiEngine>
    {
        internal WapiEngineClient(string host) : base(host, "wapi") { }
    }

    #endregion WapiEngineClient class
}

// eof "IWapiEngine.cs"
