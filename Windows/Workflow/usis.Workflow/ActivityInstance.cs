//
//  @(#) ActivityInstance.cs
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
    //  ----------------------
    //  ActivityInstance class
    //  ----------------------

    public sealed class ActivityInstance : WapiEntity<ActivityInstanceId, ActivityInstanceData>,
        IHasState<ActivityInstanceState>, IHasAttributes
    {
        #region construction

        //  -----------
        //  constructor
        //  -----------

        internal ActivityInstance(Session session, ActivityInstanceData data) : base(session, data) { }

        #endregion construction

        #region properties

        //  --------------
        //  State property
        //  --------------

        public ActivityInstanceState State { get { return Data.State; } }

        #endregion properties

        #region instance methods

        #region IHasState implementation

        //  ---------------------------
        //  QueryAvailableStates method
        //  ---------------------------

        public IEnumerable<ActivityInstanceState> QueryAvailableStates(Filter filter)
        {
            return Session.Invoke((e) => e.QueryActivityInstanceStates(null, Id, filter).Iterate());
        }

        //  ------------------
        //  ChangeState method
        //  ------------------

        public void ChangeState(ActivityInstanceState newState)
        {
            Session.Invoke((e) => e.ChangeActivityInstanceState(null, Id, newState));
        }

        #endregion IHasState implementation

        #region IHasAttributes implementation

        //  ----------------------
        //  QueryAttributes method
        //  ----------------------

        public IEnumerable<Attribute> QueryAttributes(Filter filter)
        {
            return Session.Invoke((e) => e.QueryActivityInstanceAttributes(null, Id, filter).Iterate());
        }

        //  -------------------
        //  GetAttribute method
        //  -------------------

        public Attribute GetAttribute(string name)
        {
            return Session.Invoke((e) => e.GetActivityInstanceAttribute(null, Id, name).ThrowOrReturnValue());
        }

        //  ----------------------
        //  AssignAttribute method
        //  ----------------------

        public void AssignAttribute(Attribute attribute)
        {
            Session.Invoke((e) => e.AssignActivityInstanceAttribute(null, Id, attribute));
        }

        #endregion IHasAttributes implementation

        #endregion instance methods
    }
}

// eof "ActivityInstance.cs"
