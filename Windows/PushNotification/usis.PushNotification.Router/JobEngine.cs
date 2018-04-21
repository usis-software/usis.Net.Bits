//
//  @(#) JobEngine.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using usis.Platform;

namespace usis.Framework
{
    //  ---------------
    //  JobEngine class
    //  ---------------

    /// <summary>
    /// Provides functionalities to perform jobs asynchronous.
    /// </summary>
    /// <seealso cref="ApplicationExtension" />

    public sealed class JobEngine : ApplicationExtension
    {
        #region fields

        private Dictionary<Guid, Job> jobs = new Dictionary<Guid, Job>();

        #endregion fields

        #region public methods

        //  ----------
        //  Run method
        //  ----------

        /// <summary>
        /// Runs the specified action as an asynchronous job.
        /// </summary>
        /// <param name="action">The work to be performed asynchronously.</param>
        /// <returns>An unique identifier for the job.</returns>

        public Guid Run(Action action)
        {
            return CreateAndStartJob(() => new Job(action, job => CompleteJob(job))).Id;
        }

        /// <summary>
        /// Runs the specified function as an asynchronous job.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="function">The work to be performed asynchronously.</param>
        /// <returns>An unique identifier for the job.</returns>

        public Guid Run<TResult>(Func<TResult> function) where TResult : class
        {
            return CreateAndStartJob(() => new Job<TResult>(function, job => CompleteJob(job))).Id;
        }

        //  ------------------
        //  GetJobState method
        //  ------------------

        /// <summary>
        /// Gets the state of the job.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        /// <returns>The current state of the job.</returns>

        public JobState GetJobState(Guid jobId)
        {
            return GetJobResult<object>(jobId).State;
        }

        /// <summary>
        /// Gets the job result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="jobId">The job identifier.</param>
        /// <returns>The current status with an optional result if the job is completed.</returns>

        public JobResult<TResult> GetJobResult<TResult>(Guid jobId) where TResult : class
        {
            lock (jobs)
            {
                if (jobs.TryGetValue(jobId, out Job job))
                {
                    if (job.State == JobState.Completed)
                    {
                        RemoveJob(job);
                        return new JobResult<TResult>(JobState.Completed, (job as Job<TResult>)?.Result);
                    }
                    else return new JobResult<TResult>(job.State);
                }
                else return new JobResult<TResult>(JobState.Unknown);
            }
        }

        #endregion public methods

        #region private methods

        //  ------------------------
        //  CreateAndStartJob method
        //  ------------------------

        private Job CreateAndStartJob(Func<Job> creation)
        {
            return CreateJob(creation).Start();
        }

        //  ----------------
        //  CreateJob method
        //  ----------------

        private Job CreateJob(Func<Job> creation)
        {
            Job job = null;
            Job tmp = null;
            try
            {
                tmp = creation.Invoke();
                AddJob(tmp);
                job = tmp;
                tmp = null;
            }
            finally
            {
                if (tmp != null) tmp.Dispose();
            }
            return job;
        }

        //  -------------
        //  AddJob method
        //  -------------

        private void AddJob(Job job)
        {
            lock (jobs)
            {
                jobs.Add(job.Id, job);
                Debug.WriteLine("JobEngine: {0} jobs", jobs.Count);
            }
        }

        //  ---------------
        //  Complete method
        //  ---------------

        private static void CompleteJob(Job job)
        {
            job.Complete();
        }

        //  ----------------
        //  RemoveJob method
        //  ----------------

        private void RemoveJob(Job job)
        {
            jobs.Remove(job.Id);
            Debug.WriteLine("JobEngine: {0} jobs", jobs.Count);
            job.Dispose();
        }

        #endregion private methods
    }

    #region JobResult class

    //  ---------------
    //  JobResult class
    //  ---------------

    /// <summary>
    /// Represent the state and result of an asynchronously running job.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>

    public sealed class JobResult<TResult> where TResult : class
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal JobResult(JobState state) : this(state, null) { }

        internal JobResult(JobState state, TResult result) { State = state; Result = result; }

        #endregion construction

        #region properties

        //  --------------
        //  State property
        //  --------------

        /// <summary>
        /// Gets the state of the job.
        /// </summary>
        /// <value>
        /// The state of the job.
        /// </value>

        public JobState State { get; }

        //  ---------------
        //  Result property
        //  ---------------

        /// <summary>
        /// Gets the result of the job.
        /// </summary>
        /// <value>
        /// The result of the job or <b>null</b> if the job is still running.
        /// </value>

        public TResult Result { get; }

        #endregion properties
    }

    #endregion JobResult class

    #region Job classes

    //  ---------
    //  Job class
    //  ---------

    internal class Job : IDisposable
    {
        #region properties

        //  -----------
        //  Id property
        //  -----------

        internal Guid Id { get; }

        //  --------------
        //  State property
        //  --------------

        internal JobState State { get; private set; }

        //  -------------
        //  Task property
        //  -------------

        protected Task Task { get; set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        protected Job(Func<Task> task, Action<Job> completion)
        {
            Id = Guid.NewGuid();
            State = JobState.New;
            Task = task.Invoke();
            Task.ContinueWith(t => completion(this));
        }

        internal Job(Action action, Action<Job> completion) : this(() => new Task(action), completion) { }

        #endregion construction

        #region methods

        //  ------------
        //  Start method
        //  ------------

        internal Job Start()
        {
            State = JobState.Running;
            Task.Start();
            return this;
        }

        //  ---------------
        //  Complete method
        //  ---------------

        internal void Complete()
        {
            State = JobState.Completed;
        }

        //  --------------
        //  Dispose method
        //  --------------

        public void Dispose()
        {
            if (Task != null) Task.Dispose();
        }

        #endregion methods
    }

    //  ------------------
    //  Job<TResult> class
    //  ------------------

    internal class Job<TResult> : Job where TResult : class
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        internal Job(Func<TResult> function, Action<Job> completion) : base(() => new Task<TResult>(function), completion) { }

        #endregion construction

        #region properties

        //  ---------------
        //  Result property
        //  ---------------

        internal TResult Result => (Task as Task<TResult>)?.Result;

        #endregion properties
    }

    #endregion Job classes

    #region JobEngineExtensions class

    //  -------------------------
    //  JobEngineExtensions class
    //  -------------------------

    internal static class JobEngineExtensions
    {
        //  ---------------
        //  RunAsJob method
        //  ---------------

        internal static Guid RunAsJob(this IContextInjectable<IApplication> service, Action action)
        {
            return service.Context.With<JobEngine>(true).Run(action);
        }
    }

    #endregion JobEngineExtensions class
}

// eof "JobEngine.cs"
