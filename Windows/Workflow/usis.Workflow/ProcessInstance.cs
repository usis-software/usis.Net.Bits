//
//  @(#) ProcessInstance.cs
//
//  Project:    usis Workflow Server
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

#pragma warning disable 1591

using System.Collections.Generic;

namespace usis.Workflow
{
    //  ---------------------
    //  ProcessInstance class
    //  ---------------------

    public sealed class ProcessInstance : WapiEntity<ProcessInstanceId, ProcessInstanceData>,
        IHasState<ProcessInstanceState>, IHasAttributes
    {
        #region construction

        //  -----------
        //  constructor
        //  -----------

        internal ProcessInstance(Session session, ProcessInstanceData data) : base(session, data) { }

        #endregion construction

        #region properties

        //  --------------
        //  State property
        //  --------------

        public ProcessInstanceState State { get { return Data.State; } }

        #endregion properties

        #region instance methods

        //  ------------
        //  Start method
        //  ------------

        public void Start()
        {
            Session.Invoke(((e) => e.StartProcessInstance(Id)));
        }

        //  ----------------
        //  Terminate method
        //  ----------------

        public void Terminate()
        {
            Session.Invoke((e) => e.TerminateProcessInstance(Id));
        }

        #region IHasState implementation

        //  ---------------------------
        //  QueryAvailableStates method
        //  ---------------------------

        public IEnumerable<ProcessInstanceState> QueryAvailableStates(Filter filter)
        {
            return Session.Invoke((e) => e.QueryProcessInstanceStates(Id, filter).Iterate());
        }

        //  ------------------
        //  ChangeState method
        //  ------------------

        public void ChangeState(ProcessInstanceState newState)
        {
            Session.Invoke((e) => e.ChangeProcessInstanceState(Id, newState));
        }

        #endregion IHasState implementation

        #region IHasAttributes implementation

        //  ----------------------
        //  QueryAttributes method
        //  ----------------------

        public IEnumerable<Attribute> QueryAttributes(Filter filter)
        {
            return Session.Invoke((e) => e.QueryProcessInstanceAttributes(Id, filter).Iterate());
        }

        //  -------------------
        //  GetAttribute method
        //  -------------------

        public Attribute GetAttribute(string name)
        {
            return Session.Invoke((e) => e.GetProcessInstanceAttribute(Id, name).ThrowOrReturnValue());
        }

        //  ----------------------
        //  AssignAttribute method
        //  ----------------------

        public void AssignAttribute(Attribute attribute)
        {
            Session.Invoke((e) => e.AssignProcessInstanceAttribute(Id, attribute));
        }

        #endregion IHasAttributes implementation

        //  --------------------------
        //  GetActivityInstance method
        //  --------------------------

        public ActivityInstance GetActivityInstance(ActivityInstanceId id)
        {
            return new ActivityInstance(Session, Session.Invoke((e) => e.GetActivityInstance(Id, id).ThrowOrReturnValue()));
        }

        #endregion instance methods
    }
}

// eof "ProcessInstance.cs"
