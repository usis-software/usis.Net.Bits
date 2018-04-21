//
//  @(#) SyncServiceClient.cs
//
//  Project:    IZYTRON.IQ.SyncSvc.Client
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 audius GmbH. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;
using usis.Platform;
using usis.Platform.ServiceModel;

namespace IZYTRON.IQ
{
    //  -----------------------
    //  SyncServiceClient class
    //  -----------------------

    public sealed class SyncServiceClient : IDisposable
    {
        #region fields

        private EventWaitHandle stopping = new EventWaitHandle(false, EventResetMode.ManualReset);
        private EventWaitHandle stopped = new EventWaitHandle(true, EventResetMode.ManualReset);

        #endregion fields

        #region properties

        //  -------------------
        //  DatabaseId property
        //  -------------------

        private Guid DatabaseId { get; }

        //  -------------------
        //  ServiceUrl property
        //  -------------------

        private Uri ServiceUrl { get; }

        //  --------------
        //  State property
        //  --------------

        public SyncState State
        {
            get
            {
                try
                {
                    using (var client = CreateClient())
                    {
                        return client.Service.GetState(DatabaseId);
                    }
                }
                catch (EndpointNotFoundException)
                {
                    return SyncState.ConnectionFailed;
                }
            }
        }

        //  ----------------
        //  Monitor property
        //  ----------------

        public ProgressMonitor Monitor { get; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        private SyncServiceClient(Guid databaseId, Uri serverUrl)
        {
            DatabaseId = databaseId;
            ServiceUrl = new Uri(serverUrl, Constants.SyncServicePath);
            Monitor = new ProgressMonitor();
        }

        #endregion construction

        #region public methods

        //  ----------------------
        //  ConnectToServer method
        //  ----------------------

        /// <summary>
        /// Connects to a IZYTRON.IQ Sync Server with the specified URL.
        /// </summary>
        /// <param name="serverUrl">The server URL.</param>
        /// <param name="databaseId">The database identifier.</param>
        /// <returns>
        /// A newly created <see cref="SyncServiceClient" /> object.
        /// </returns>

        public static SyncServiceClient ConnectToServer(Uri serverUrl, Guid databaseId)
        {
            return new SyncServiceClient(databaseId, serverUrl);
        }

        //  ---------------------------
        //  StartSynchronization method
        //  ---------------------------

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private Thread worker;

        public void StartSynchronization()
        {
            if (State == SyncState.ReadyToUpload)
            {
                // set new state on server
                UsingClient(service => service.ReportState(DatabaseId, SyncState.PreparingToUpload));

                StartBackgroundWork();
            }
            else throw new InvalidOperationException();
        }

        //  -----------------
        //  ResumeWait method
        //  -----------------

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public void ResumeWait()
        {
            if (State.IsOneOf(SyncState.Unknown, SyncState.ConnectionFailed, SyncState.ReadyToUpload))
            {
                throw new InvalidOperationException();
            }
            else StartBackgroundWork();
        }

        #endregion public methods

        #region private methods

        //  --------------------------
        //  StartBackgroundWork method
        //  --------------------------

        private void StartBackgroundWork()
        {
            stopped.Reset();
            worker = new Thread(DoBackgroundWork);
            worker.Start();
        }

        #endregion private methods

        //  -----------------------
        //  DoBackgroundWork method
        //  -----------------------

        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
        private void DoBackgroundWork()
        {
            var timeout = 1000;
            var close = true;
            do
            {
                switch (State)
                {
                    case SyncState.PreparingToUpload:
                        close = !PrepareUpload();
                        break;
                    case SyncState.Uploading:
                        close = !Upload();
                        break;
                    case SyncState.Merging:
                        close = !WaitForMerge();
                        break;
                    case SyncState.ConflictsPending:
                        close = true;
                        break;
                    case SyncState.PreparingToDownload:
                        close = !WaitForDownload();
                        break;
                    case SyncState.Downloading:
                        close = !Download();
                        break;
                    case SyncState.ApplyingDownload:
                        close = !ApplyDownload();
                        break;
                    case SyncState.Unknown:
                    case SyncState.ConnectionFailed:
                    case SyncState.ReadyToUpload:
                    default:
                        close = true;
                        break;
                }
                if (!close) Console.WriteLine("waiting...");

            } while (!close && !stopping.WaitOne(timeout));
            stopped.Set();
        }

        private bool PrepareUpload()
        {
            Monitor.SetStep(1, 6, "Prepare Upload");

            //Monitor.
            // TODO: create ZIP file for upload
            Thread.Sleep(3000);

            return true;
        }

        private bool Upload()
        {
            Monitor.SetStep(2, 6, "Upload");
            return true;
        }

        private bool WaitForMerge()
        {
            Monitor.SetStep(3, 6, "Serverside Merge");
            return true;
        }

        private bool WaitForDownload()
        {
            Monitor.SetStep(4, 6, "Prepare Download");
            return true;
        }

        private bool Download()
        {
            Monitor.SetStep(5, 6, "Download");
            return true;
        }

        private bool ApplyDownload()
        {
            Monitor.SetStep(6, 6, "Apply Download");
            return true;
        }

        #region private methods

        private void UsingClient(Action<ISyncService> action)
        {
            using (var client = CreateClient())
            {
                action.Invoke(client.Service);
            }
        }

        //private TResult UsingClient<TResult>(Func<ISyncService, TResult> function)
        //{
        //    using (var client = CreateClient())
        //    {
        //        return function.Invoke(client.Service);
        //    }
        //}

        private ServiceClient<ISyncService> CreateClient()
        {
            return new ServiceClient<ISyncService>(CreateEndpoint(typeof(ISyncService), ServiceUrl));
        }

        private static ServiceEndpoint CreateEndpoint(Type channelType, Uri url)
        {
            return new ServiceEndpoint(ContractDescription.GetContract(channelType), new BasicHttpBinding(), new EndpointAddress(url));
        }

        #endregion private methods

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>

        public void Dispose()
        {
            if (stopping != null && stopped != null)
            {
                if (stopping.Set()) stopped.WaitOne();
            }
            if (stopping != null) { stopping.Dispose(); stopping = null; }
            if (stopped != null) { stopped.Dispose(); stopped = null; }
            GC.SuppressFinalize(this);
        }

        //  ---------
        //  finalizer
        //  ---------

        ~SyncServiceClient() { Dispose(); }

        #endregion IDisposable implementation
    }
}
