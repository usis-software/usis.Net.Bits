//
//  @(#) JobEngineMgmt.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.ServiceModel;
using usis.Platform;

namespace usis.Framework
{
    #region IJobEngineMgmt interface

    //  ------------------------
    //  IJobEngineMgmt interface
    //  ------------------------

    /// <summary>
    /// Provides methods to manage a job engine.
    /// </summary>

    [ServiceContract]
    public interface IJobEngineMgmt
    {
        //  --------------------
        //  EnumerateJobs method
        //  --------------------

        /// <summary>
        /// Enumerates the jobs activated by the job engine.
        /// </summary>
        /// <returns>
        /// An enumerator to iterate the active jobs of the job engine.
        /// </returns>

        [OperationContract]
        IEnumerable<JobProgress> EnumerateJobs();

        //  ---------------------
        //  GetJobProgress method
        //  ---------------------

        /// <summary>
        /// Gets informations about the progress of a job with the specified unique identifier.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        /// <returns>
        /// Informations about the progress of a job with the specified unique identifier.
        /// </returns>

        [OperationContract]
        JobProgress GetJobProgress(Guid jobId);

        //  ----------------
        //  CancelJob method
        //  ----------------

        /// <summary>
        /// Sends a cancellation request for the job with the specified unique identifier to the job engine.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>

        [OperationContract]
        void CancelJob(Guid jobId);
    }

    #endregion IJobEngineMgmt interface

    //  -------------------
    //  JobEngineMgmt class
    //  -------------------

    /// <summary>
    /// Provides a service to manage the <see cref="JobEngine"/> extension.
    /// </summary>
    /// <seealso cref="ContextInjectable{IApplication}" />
    /// <seealso cref="IJobEngineMgmt" />

    public sealed class JobEngineMgmt : ContextInjectable<IApplication>, IJobEngineMgmt
    {
        //  --------------------
        //  EnumerateJobs method
        //  --------------------

        /// <summary>
        /// Enumerates the jobs activated by the job engine.
        /// </summary>
        /// <returns>
        /// An enumerator to iterate the active jobs of the job engine.
        /// </returns>

        public IEnumerable<JobProgress> EnumerateJobs()
        {
            var jobEngine = Context?.With<JobEngine>();
            if (jobEngine == null) yield break;

            foreach (var job in jobEngine.EnumerateJobs()) yield return job;
        }

        //  ---------------------
        //  GetJobProgress method
        //  ---------------------

        /// <summary>
        /// Gets informations about the progress of a job with the specified unique identifier.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        /// <returns>
        /// Informations about the progress of a job with the specified unique identifier.
        /// </returns>

        public JobProgress GetJobProgress(Guid jobId)
        {
            return Context?.With<JobEngine>()?.GetProgress(jobId);
        }

        //  ----------------
        //  CancelJob method
        //  ----------------

        /// <summary>
        /// Sends a cancellation request for the job with the specified unique identifier to the job engine.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>

        public void CancelJob(Guid jobId)
        {
            Context?.With<JobEngine>()?.CancelJob(jobId);
        }
    }
}

// eof "JobEngineMgmt.cs"
