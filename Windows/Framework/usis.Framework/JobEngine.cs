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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
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

        #region Run methods

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
            return CreateAndStartJob(() => new Job(Owner, action, job => CompleteJob(job), null)).Id;
        }

        /// <summary>
        /// Runs the specified action as an asynchronous job.
        /// </summary>
        /// <param name="action">The work to be performed asynchronously.</param>
        /// <returns>An unique identifier for the job.</returns>

        public Guid Run(Action<IJob> action)
        {
            return CreateAndStartJob(() => new Job(Owner, action, job => CompleteJob(job), null)).Id;
        }

        /// <summary>
        /// Checks if a job with the specified identifier exists.
        /// Iff not, runs the specified action as an asynchronous job with that identifier.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        /// <param name="action">The work to be performed asynchronously.</param>
        /// <returns><c>true</c> when a job with the specified identifier does not exist
        /// and a new job was created and started, otherwise <c>false</c>.</returns>

        public bool Run(Guid jobId, Action action)
        {
            return FindOrCreateAndStartJob(jobId, () => new Job(Owner, action, job => CompleteJob(job), jobId));
        }


        /// <summary>
        /// Checks if a job with the specified identifier exists.
        /// Iff not, runs the specified action as an asynchronous job with that identifier.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        /// <param name="action">The work to be performed asynchronously.</param>
        /// <returns><c>true</c> when a job with the specified identifier does not exist
        /// and a new job was created and started, otherwise <c>false</c>.</returns>

        public bool Run(Guid jobId, Action<IJob> action)
        {
            return FindOrCreateAndStartJob(jobId, () => new Job(Owner, action, job => CompleteJob(job), jobId));
        }

        /// <summary>
        /// Runs the specified function as an asynchronous job.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="function">The work to be performed asynchronously.</param>
        /// <returns>An unique identifier for the job.</returns>

        public Guid Run<TResult>(Func<TResult> function) where TResult : class
        {
            return CreateAndStartJob(() => new Job<TResult>(Owner, function, job => CompleteJob(job), null)).Id;
        }

        #endregion Run methods

        #region state/result/progress methods

        //  ------------------
        //  GetJobState method
        //  ------------------

        /// <summary>
        /// Gets the state of the job with the specified identifier.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        /// <returns>The current state of the job.</returns>

        public JobState GetJobState(Guid jobId)
        {
            return GetJobResult<object>(jobId).State;
        }

        /// <summary>
        /// Gets the result of the job with the specified identifier.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="jobId">The job identifier.</param>
        /// <returns>The current state of the with an optional result if the job is completed.</returns>

        public JobResult<TResult> GetJobResult<TResult>(Guid jobId) where TResult : class
        {
            lock (jobs)
            {
                var job = FindJob(jobId);
                if (job != null)
                {
                    return GetJobResult<TResult>(job);
                }
                else return new JobResult<TResult>(JobState.Unknown);
            }
        }

        //  ------------------
        //  GetProgress method
        //  ------------------

        /// <summary>
        /// Retrieves the progress information for the job with the specified identifier.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        /// <returns>
        /// The progress information for the job.
        /// </returns>
        /// <remarks>
        /// The job is automatically removed when it is finished.
        /// </remarks>

        public JobProgress GetProgress(Guid jobId)
        {
            return GetProgress(jobId, true);
        }

        /// <summary>
        /// Retrieves the progress information for the job with the specified identifier.
        /// </summary>
        /// <param name="jobId">The job identifier.</param>
        /// <param name="removeWhenFinished">if set to <c>true</c> the job is removed when it is finished.</param>
        /// <returns>
        /// The progress information for the job.
        /// </returns>

        public JobProgress GetProgress(Guid jobId, bool removeWhenFinished)
        {
            lock (jobs)
            {
                var job = FindJob(jobId);
                if (job == null) return null;
                var progress = job.CreateProgress();
                if (removeWhenFinished) RemoveJobIfFinished(job);
                return progress;
            }
        }

        #endregion state/result/progress methods

        #endregion public methods

        #region internal methods

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
            lock (jobs)
            {
                foreach (var job in jobs.Values)
                {
                    yield return job.CreateProgress();
                }
            }
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
            lock (jobs)
            {
                FindJob(jobId)?.Cancel();
            }
        }

        #endregion internal methods

        #region private methods

        //  -------------------
        //  GetJobResult method
        //  -------------------

        private JobResult<TResult> GetJobResult<TResult>(Job job) where TResult : class
        {
            if (job.State.IsOneOf(JobState.Completed, JobState.Failed))
            {
                RemoveJob(job);
                return new JobResult<TResult>(job.State, (job as Job<TResult>)?.Result);
            }
            else return new JobResult<TResult>(job.State);
        }

        //  ------------------------------
        //  FindOrCreateAndStartJob method
        //  ------------------------------

        private bool FindOrCreateAndStartJob(Guid jobId, Func<Job> creator)
        {
            lock (jobs)
            {
                var job = FindJob(jobId);
                if (job != null)
                {
                    if (job.State.IsOneOf(JobState.Completed, JobState.Failed))
                    {
                        RemoveJob(job);
                        job = null;
                    }
                }
                if (job == null)
                {
                    job = CreateAndStartJob(creator);
                    Debug.Assert(job.Id.Equals(jobId));
                    return true;
                }
                else return false;
            }
        }

        //  --------------
        //  FindJob method
        //  --------------

        private Job FindJob(Guid jobId)
        {
            if (jobs.TryGetValue(jobId, out Job job))
            {
                return job;
            }
            return null;
        }

        //  ------------------------
        //  CreateAndStartJob method
        //  ------------------------

        private Job CreateAndStartJob(Func<Job> creator)
        {
            return CreateJob(creator).Start();
        }

        //  ----------------
        //  CreateJob method
        //  ----------------

        private Job CreateJob(Func<Job> creator)
        {
            Job job = null;
            Job tmp = null;
            try
            {
                tmp = creator.Invoke();
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

                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture,
                    "JobEngine: {0} job{1} active.", jobs.Count, jobs.Count != 1 ? "s" : string.Empty));
            }
        }

        //  ---------------
        //  Complete method
        //  ---------------

        private void CompleteJob(Job job)
        {
            job.Complete();
            if (job.RemoveWhenCompleted) RemoveJob(job);
        }

        //  --------------------------
        //  RemoveJobIfFinished method
        //  --------------------------

        private void RemoveJobIfFinished(Job job)
        {
            if (job.State.IsOneOf(JobState.Completed, JobState.Failed))
            {
                RemoveJob(job);
            }
        }

        //  ----------------
        //  RemoveJob method
        //  ----------------

        private void RemoveJob(Job job)
        {
            jobs.Remove(job.Id);

            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture,
                "JobEngine: {0} job{1} active.", jobs.Count, jobs.Count != 1 ? "s" : string.Empty));

            job.Dispose();
        }

        #endregion private methods

        #region overrides

        //  ---------------
        //  OnDetach method
        //  ---------------

        /// <summary>
        /// Called when an extension is removed from the applications
        /// <see cref="IExtensibleObject{TObject}.Extensions"/>.
        /// </summary>

        protected override void OnDetach()
        {
            lock (jobs)
            {
                if (jobs.Count > 0)
                {
                    Trace.WriteLine("Requesting cancellation of all jobs ...");
                    foreach (var job in jobs.Values) job.Cancel();
                    Trace.WriteLine("Waiting for jobs to complete ...");
                    Task.WaitAll(jobs.Values.Select((job) => job.Task).ToArray());
                    Trace.WriteLine("... all jobs completed.");
                    Thread.Sleep(100);
                }
            }
        }

        #endregion overrides
    }

    #region JobState enumeration

    //  --------------------
    //  JobState enumeration
    //  --------------------

    /// <summary>
    /// Indicates the current state of a job.
    /// </summary>

    public enum JobState
    {
        /// <summary>
        /// The current job state is unknown.
        /// </summary>

        Unknown,

        /// <summary>
        /// The job was newly created but not yet started.
        /// </summary>

        New,

        /// <summary>
        /// The job is running.
        /// </summary>

        Running,

        /// <summary>
        /// The job was completed.
        /// </summary>

        Completed,

        /// <summary>
        /// The job failed because an exception occurred.
        /// </summary>

        Failed
    }
    
    #endregion JobState enumeration

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

    #region IJob interface

    //  --------------
    //  IJob interface
    //  --------------

    /// <summary>
    /// Provides access to members of a job that is performed asynchronous by a <see cref="JobEngine"/>.
    /// </summary>

    public interface IJob : IProgressUpdate
    {
        //  -----------
        //  Id property
        //  -----------

        /// <summary>
        /// Gets the job identifier.
        /// </summary>
        /// <value>
        /// The job identifier.
        /// </value>

        Guid Id { get; }

        //  --------------
        //  State property
        //  --------------

        /// <summary>
        /// Gets the state of the job.
        /// </summary>
        /// <value>
        /// The state of the job.
        /// </value>

        [Obsolete("This property will be removed.")]
        JobState State { get; }

        //  ---------------------
        //  CompleteWith property
        //  ---------------------

        /// <summary>
        /// Gets or sets an action that is invoke after the job completed.
        /// </summary>
        /// <value>
        /// The action to invoke after the job completed.
        /// </value>

        Action<IJob> CompleteWith { get; set; }

        //  ----------------------------
        //  RemoveWhenCompleted property
        //  ----------------------------

        /// <summary>
        /// Gets or sets a value indicating whether to remove the job when it completed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the job should be removed when it completed; otherwise, <c>false</c>.
        /// </value>

        bool RemoveWhenCompleted { get; set; }

        //  --------------
        //  Token property
        //  --------------

        /// <summary>
        /// Gets the token which propagates notification that the job should be canceled.
        /// </summary>
        /// <value>
        /// The token which propagates notification that the job should be canceled.
        /// </value>

        CancellationToken Token { get; }
    }

    #endregion IJob interface

    #region Job classes

    //  ---------
    //  Job class
    //  ---------

    /// <summary>
    /// Represents a job that is performed asynchronous by a <see cref="JobEngine"/>.
    /// </summary>

    internal class Job : IJob, IDisposable
    {
        #region fields

        private readonly object update = new object();
        private CancellationTokenSource cts = new CancellationTokenSource();

        #endregion fields

        #region properties

        //  -----------
        //  Id property
        //  -----------

        public Guid Id { get; }

        //  --------------
        //  State property
        //  --------------

        public JobState State { get; internal protected set; }

        //  ---------------------
        //  CompleteWith property
        //  ---------------------

        public Action<IJob> CompleteWith { get; set; }

        //  -----------------
        //  Progress property
        //  -----------------

        internal Progress Progress { get; }

        //  -------------
        //  Task property
        //  -------------

        internal Task Task { get; private set; }

        //  ----------------------------
        //  RemoveWhenCompleted property
        //  ----------------------------

        public bool RemoveWhenCompleted { get; set; }

        public CancellationToken Token { get; internal protected set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal protected Job(Func<Job, CancellationToken, Task> taskCreator, Action<Job> completion, Guid? jobId)
        {
            if (taskCreator == null) throw new ArgumentNullException(nameof(taskCreator));

            Id = jobId ?? Guid.NewGuid();
            State = JobState.New;
            Progress = new Progress();

            Task = taskCreator.Invoke(this, cts.Token);
            Task.ContinueWith(t => completion(this));
        }

        internal Job(IApplication application, Action action, Action<Job> completion, Guid? jobId) : this(CreateTask(application, (job) => action.Invoke()), completion, jobId) { }

        internal Job(IApplication application, Action<IJob> action, Action<Job> completion, Guid? jobId) : this(CreateTask(application, action), completion, jobId) { }

        #endregion construction

        #region methods

        //  -----------------
        //  CreateTask method
        //  -----------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        internal protected static Func<Job, CancellationToken, Task> CreateTask(IApplication application, Action<IJob> action)
        {
            return (job, token) => new Task(() =>
            {
                try
                {
                    job.Token = token;
                    action.Invoke(job);
                    job.State = JobState.Completed;
                }
                catch (Exception exception)
                {
                    job.State = JobState.Failed;
                    application?.ReportException(exception);
                }
            }, token);
        }

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
            if (State == JobState.Running) State = Task.Status == TaskStatus.Faulted ? JobState.Failed : JobState.Completed;
            CompleteWith?.Invoke(this);
        }

        //  -------------
        //  Cancel method
        //  -------------

        internal void Cancel() { cts.Cancel(); }

        //  ---------------------
        //  CreateProgress method
        //  ---------------------

        internal JobProgress CreateProgress()
        {
            lock (update)
            {
                return new JobProgress(Progress, State);
            }
        }

        #endregion methods

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false; // To detect redundant calls

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Task.Dispose();
                    cts.Dispose();
                }
                disposed = true;
            }
        }

        //  ---------
        //  finalizer
        //  ---------

        /// <summary>
        /// Finalizes an instance of the <see cref="Job"/> class.
        /// </summary>

        ~Job() { Dispose(false); }

        #endregion IDisposable implementation

        #region IProgressUpdate implementation

        //  -----------------
        //  MinValue property
        //  -----------------

        long IProgressUpdate.MinValue => Progress.MinValue;

        //  -----------------
        //  MaxValue property
        //  -----------------

        long IProgressUpdate.MaxValue => Progress.MaxValue;

        //  --------------
        //  SetStep method
        //  --------------

        void IProgressUpdate.SetStep(int current, int total, string description)
        {
            lock (update)
            {
                Progress.SetStep(current, total, description);
            }
        }

        //  ----------------------
        //  SetProgressStep method
        //  ----------------------

        void IProgressUpdate.SetProgressStep(int current, int total, string description)
        {
            lock (update)
            {
                Progress.SetStep(current, total, description);
            }
        }

        //  -------------
        //  Update method
        //  -------------

        void IProgressUpdate.Update(long minValue, long maxValue, long value, string message)
        {
            lock (update)
            {
                Progress.Update(minValue, maxValue, value, message);
            }
        }

        //  ---------------------
        //  UpdateProgress method
        //  ---------------------

        void IProgressUpdate.UpdateProgress(long minValue, long maxValue, long value, string message)
        {
            lock (update)
            {
                Progress.Update(minValue, maxValue, value, message);
                Debug.WriteLine("Job {0} progress: {1}", Id, Progress);
            }
        }

        #endregion IProgressUpdate implementation
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

        internal Job(IApplication application, Func<TResult> function, Action<Job> completion, Guid? jobId) : base(CreateTask(application, job => function()), completion, jobId) { }

        #endregion construction

        #region properties

        //  ---------------
        //  Result property
        //  ---------------

        internal TResult Result => (Task as Task<TResult>)?.Result;

        #endregion properties

        #region methods

        //  -----------------
        //  CreateTask method
        //  -----------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        internal protected static Func<Job, CancellationToken, Task> CreateTask(IApplication application, Func<IJob, TResult> function)
        {
            return (job, token) => new Task<TResult>(() =>
            {
                TResult result = null;
                try
                {
                    job.Token = token;
                    result = function.Invoke(job);
                    job.State = JobState.Completed;
                }
                catch (Exception exception)
                {
                    job.State = JobState.Failed;
                    application?.ReportException(exception);
                }
                return result;

            }, token);
        }

        #endregion methods
    }

    #endregion Job classes

    #region JobProgress class

    //  -----------------
    //  JobProgress class
    //  -----------------

    /// <summary>
    /// Provides progress and state informations about an asynchronous job.
    /// </summary>

    [DataContract]
    public class JobProgress : ProgressInfo
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="JobProgress"/> class
        /// with the specified progress information and job state.
        /// </summary>
        /// <param name="progressInfo">The progress information.</param>
        /// <param name="state">The current state of the job.</param>

        public JobProgress(ProgressInfo progressInfo, JobState state) : base(progressInfo)
        {
            State = state;
        }

        #endregion construction

        #region properties

        //  --------------
        //  State property
        //  --------------

        /// <summary>
        /// Gets the current state of the job.
        /// </summary>
        /// <value>
        /// The current state of the job.
        /// </value>

        [DataMember]
        public JobState State { get; internal set; }

        #endregion properties

        #region overrides

        //  ---------------
        //  ToString method
        //  ---------------

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} - {1}", State, base.ToString());
        }

        #endregion overrides
    }

    #endregion JobProgress class

    #region JobEngineExtensions class

    //  -------------------------
    //  JobEngineExtensions class
    //  -------------------------

    /// <summary>
    /// Provides extensions methods to perform jobs asynchronous.
    /// </summary>

    public static class JobEngineExtensions
    {
        //  ---------------
        //  RunAsJob method
        //  ---------------

        /// <summary>
        /// Runs the specified action as an asynchronous job.
        /// </summary>
        /// <param name="service">An object that implements <see cref="IContextInjectable{IApplication}" />.</param>
        /// <param name="action">The action to be performed asynchronously.</param>
        /// <returns>An unique identifier for the job.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="service"/> is a <c>null</c> reference.</exception>

        public static Guid RunAsJob(this IContextInjectable<IApplication> service, Action action)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            return service.Context.With<JobEngine>(true).Run(action);
        }

        /// <summary>
        /// Runs the specified action as an asynchronous job.
        /// </summary>
        /// <param name="service">An object that implements <see cref="IContextInjectable{IApplication}" />.</param>
        /// <param name="action">The action to be performed asynchronously.</param>
        /// <returns>An unique identifier for the job.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="service" /> is a <c>null</c> reference.</exception>

        public static Guid RunAsJob(this IContextInjectable<IApplication> service, Action<IJob> action)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            return service.Context.With<JobEngine>(true).Run(action);
        }

        /// <summary>
        /// Runs the specified action as an asynchronous job.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="action">The action to be performed asynchronously.</param>
        /// <returns>An unique identifier for the job.</returns>

        public static Guid RunAsJob(this IExtensibleObject<IApplication> application, Action<IJob> action)
        {
            return application.With<JobEngine>(true).Run(action);
        }

        /// <summary>
        /// Runs the specified action as an asynchronous job.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="action">The action to be performed asynchronously.</param>
        /// <returns>An unique identifier for the job.</returns>

        public static Guid RunAsJob(this IExtensibleObject<IApplication> application, Action action)
        {
            return application.With<JobEngine>(true).Run(action);
        }
    }

    #endregion JobEngineExtensions class
}

// eof "JobEngine.cs"
