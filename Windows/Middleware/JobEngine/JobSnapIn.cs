//
//  @(#) JobSnapIn.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018 usis GmbH. All rights reserved.

using System;
using System.ComponentModel;
using usis.JobEngine;

namespace usis.Framework
{
    //  ---------------
    //  JobSnapIn class
    //  ---------------

    public abstract class JobSnapIn : SnapIn
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        protected JobSnapIn(string key) { Key = key ?? throw new ArgumentNullException(nameof(key)); }

        #endregion construction

        #region properties

        //  ------------
        //  Key property
        //  ------------

        public string Key { get; }

        #endregion properties

        #region overrides

        //  -------------------
        //  OnConnecting method
        //  -------------------

        protected override void OnConnecting(CancelEventArgs e)
        {
            Application.With<JobRepository>(true).Register(new JobRepositoryItem(Key, Run));

            base.OnConnecting(e);
        }

        #endregion overrides

        #region methods

        //  ----------
        //  Run method
        //  ----------

        protected abstract void Run(IJob job);

        #endregion methods
    }
}

// eof "JobSnapIn.cs"
