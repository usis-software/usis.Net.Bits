//
//  @(#) JobRepository.cs
//
//  Project:    usis Job Engine
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using usis.Framework;
using usis.Platform;

namespace usis.JobEngine
{
    //  -------------------
    //  JobRepository class
    //  -------------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class JobRepository : ApplicationExtension
    {
        #region fields

        private Dictionary<string, JobRepositoryItem> jobs = new Dictionary<string, JobRepositoryItem>(StringComparer.OrdinalIgnoreCase);

        #endregion fields

        #region methods

        //  ---------------
        //  Register method
        //  ---------------

        internal void Register(JobRepositoryItem job)
        {
            if (job == null) throw new ArgumentNullException(nameof(job));

            lock (jobs)
            {
                jobs[job.Key] = job;
            }
        }

        //  ------------
        //  Start method
        //  ------------

        internal Guid Start(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullOrWhiteSpaceException(nameof(key));

            lock (jobs)
            {
                return Owner.With<Framework.JobEngine>(true).Run(jobs[key].Action);
            }
        }

        #endregion methods
    }

    #region JobRepositoryItem class

    //  -----------------------
    //  JobRepositoryItem class
    //  -----------------------

    internal class JobRepositoryItem
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal JobRepositoryItem(string key, Action<IJob> action)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullOrWhiteSpaceException(nameof(key));
            Key = key;
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        #endregion construction

        #region properties

        //  ------------
        //  Key property
        //  ------------

        internal string Key { get; private set; }

        //  ---------------
        //  Action property
        //  ---------------

        internal Action<IJob> Action { get; private set; }

        #endregion properties
    }

    #endregion JobRepositoryItem class
}

// eof "JobRepository.cs"
