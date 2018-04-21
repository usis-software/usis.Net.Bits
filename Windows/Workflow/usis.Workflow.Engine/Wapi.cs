//
//  @(#) Wapi.cs
//
//  Project:    usis Workflow Engine
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.ServiceModel;
using usis.Framework;
using usis.Framework.ServiceModel;
using usis.Platform;

namespace usis.Workflow.Engine
{
    //  ----------
    //  Wapi class
    //  ----------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    internal sealed class Wapi : ContextInjectable<IApplication>, IWapiEngine
    {
        #region process definitions

        //  ------------------------------
        //  CreateProcessDefinition method
        //  ------------------------------

        OperationResult<ProcessDefinitionData> IWapiEngine.CreateProcessDefinition(string name)
        {
            return OperationResult.Invoke(() => Engine.CreateProcessDefinition(name));
        }

        //  ------------------------------
        //  QueryProcessDefinitions method
        //  ------------------------------

        OperationResultList<ProcessDefinitionData> IWapiEngine.QueryProcessDefinitions(Filter filter)
        {
            return OperationResultList.Invoke(() => Engine.QueryProcessDefinitions().ToList());
        }

        //  ------------------------------
        //  DeleteProcessDefinition method
        //  ------------------------------

        OperationResult IWapiEngine.DeleteProcessDefinition(ProcessDefinitionId processDefinitionId)
        {
            return OperationResult.Invoke(() => Engine.DeleteProcessDefinition(processDefinitionId));
        }

        #endregion process definitions

        #region process instances

        #endregion process instances

        #region activities

        #endregion activities

        #region ...

        OperationResult IWapiEngine.AssignActivityInstanceAttribute(ProcessInstanceId processInstanceId, ActivityInstanceId id, Attribute attribute)
        {
            throw new NotImplementedException();
        }

        OperationResult IWapiEngine.AssignProcessInstanceAttribute(ProcessInstanceId id, Attribute attribute)
        {
            throw new NotImplementedException();
        }

        OperationResult IWapiEngine.ChangeActivityInstanceState(ProcessInstanceId processInstanceId, ActivityInstanceId id, ActivityInstanceState newState)
        {
            throw new NotImplementedException();
        }

        OperationResult IWapiEngine.ChangeProcessDefinitionState(ProcessDefinitionId processDefinitionId, ProcessDefinitionState newState)
        {
            throw new NotImplementedException();
        }

        OperationResult IWapiEngine.ChangeProcessInstancesState(ProcessDefinitionId id, Filter filter)
        {
            throw new NotImplementedException();
        }

        OperationResult IWapiEngine.ChangeProcessInstanceState(ProcessInstanceId id, ProcessInstanceState newState)
        {
            throw new NotImplementedException();
        }

        OperationResult<ProcessInstanceData> IWapiEngine.CreateProcessInstance(ProcessDefinitionId processDefinitionId, string name)
        {
            throw new NotImplementedException();
        }

        OperationResult<ActivityInstanceData> IWapiEngine.GetActivityInstance(ProcessInstanceId processInstanceId, ActivityInstanceId id)
        {
            throw new NotImplementedException();
        }

        OperationResult<Attribute> IWapiEngine.GetActivityInstanceAttribute(ProcessInstanceId processInstanceId, ActivityInstanceId id, string name)
        {
            throw new NotImplementedException();
        }

        OperationResult<ProcessInstanceData> IWapiEngine.GetProcessInstance(ProcessInstanceId id)
        {
            throw new NotImplementedException();
        }

        OperationResult<Attribute> IWapiEngine.GetProcessInstanceAttribute(ProcessInstanceId id, string name)
        {
            throw new NotImplementedException();
        }

        OperationResultList<Attribute> IWapiEngine.QueryActivityInstanceAttributes(ProcessInstanceId processInstanceId, ActivityInstanceId id, Filter filter)
        {
            throw new NotImplementedException();
        }

        OperationResultList<ActivityInstanceData> IWapiEngine.QueryActivityInstances(Filter filter)
        {
            throw new NotImplementedException();
        }

        OperationResultList<ActivityInstanceState> IWapiEngine.QueryActivityInstanceStates(ProcessInstanceId processInstanceId, ActivityInstanceId id, Filter filter)
        {
            throw new NotImplementedException();
        }

        OperationResultList<ProcessDefinitionState> IWapiEngine.QueryProcessDefinitionStates(ProcessDefinitionId processDefinitionId, Filter filter)
        {
            throw new NotImplementedException();
        }

        OperationResultList<Attribute> IWapiEngine.QueryProcessInstanceAttributes(ProcessInstanceId id, Filter filter)
        {
            throw new NotImplementedException();
        }

        OperationResultList<ProcessInstanceData> IWapiEngine.QueryProcessInstances(Filter filter)
        {
            throw new NotImplementedException();
        }

        OperationResultList<ProcessInstanceState> IWapiEngine.QueryProcessInstanceStates(ProcessInstanceId id, Filter filter)
        {
            throw new NotImplementedException();
        }

        OperationResult IWapiEngine.StartProcessInstance(ProcessInstanceId id)
        {
            throw new NotImplementedException();
        }

        OperationResult IWapiEngine.TerminateProcessInstance(ProcessInstanceId id)
        {
            throw new NotImplementedException();
        }

        OperationResult IWapiEngine.TerminateProcessInstances(ProcessDefinitionId id, Filter filter)
        {
            throw new NotImplementedException();
        }

        #endregion ...

        #region properties

        //  ---------------
        //  Engine property
        //  ---------------

        private Engine Engine { get { return Context.With<Engine>(); } }

        #endregion properties
    }

    #region WapiSnapIn class

    //  ----------------
    //  WapiSnapIn class
    //  ----------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal sealed class WapiSnapIn : NamedPipeServiceHostSnapIn<Wapi, IWapiEngine>
    {
        public WapiSnapIn() : base(false) { }
    }

    #endregion WapiSnapIn class
}

// eof "Wapi.cs"
