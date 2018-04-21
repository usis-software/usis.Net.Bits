using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Net;

namespace PushSharp.Core
{
    public class ServiceBroker<TNotification> : IServiceBroker<TNotification>, IDisposable where TNotification : INotification
    {
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (notifications != null)
                {
                    notifications.Dispose();
                    notifications = null;
                }
                foreach (IDisposable item in workers)
                {
                    if (item != null) item.Dispose();
                }
            }
        }

        ~ServiceBroker()
        {
            Dispose(false);
        }

        static ServiceBroker ()
        {
            ServicePointManager.DefaultConnectionLimit = 100;
            ServicePointManager.Expect100Continue = false;
        }

        public ServiceBroker (IServiceConnectionFactory<TNotification> connectionFactory)
        {
            ServiceConnectionFactory = connectionFactory;

            lockWorkers = new object ();
            workers = new List<ServiceWorker<TNotification>> ();
            running = false;

            notifications = new BlockingCollection<TNotification> ();
            ScaleSize = 1;
            //AutoScale = true;
            //AutoScaleMaxSize = 20;
        }

        public event EventHandler<NotificationEventArgs<TNotification>> OnNotificationSucceeded;
        public event EventHandler<NotificationExceptionEventArgs<TNotification>> OnNotificationFailed;

        //public bool AutoScale { get; set; }
        //public int AutoScaleMaxSize { get; set; }
        public int ScaleSize { get; private set; }

        public IServiceConnectionFactory<TNotification> ServiceConnectionFactory { get; set; }

        BlockingCollection<TNotification> notifications;
        List<ServiceWorker<TNotification>> workers;
        object lockWorkers;
        bool running;

        public virtual void QueueNotification (TNotification notification)
        {
            notifications.Add (notification);
        }

        public IEnumerable<TNotification> TakeMany ()
        {
            return notifications.GetConsumingEnumerable ();
        }

        public bool IsCompleted {
            get { return notifications.IsCompleted; }
        }

        public void Start ()
        {
            if (running)
                return;

            running = true;
            ChangeScale (ScaleSize);
        }

        public void Stop()
        {
            Stop(false);
        }

        public void Stop (bool immediately)
        {
            if (!running)
                throw new OperationCanceledException ("Service Broker has already been signaled to Stop");

            running = false;

            notifications.CompleteAdding ();

            lock (lockWorkers) {
                // Kill all workers right away
                if (immediately)
                    workers.ForEach (sw => sw.Cancel ());
					
                var all = (from sw in workers
                                       select sw.WorkerTask).ToArray ();

                Log.Write(LogLevel.Info, "Waiting on Tasks");

                Task.WaitAll (all);

                Log.Write(LogLevel.Info, "Done Waiting on Tasks");

                workers.Clear ();
            }
        }

        public void ChangeScale (int newScaleSize)
        {
            if (newScaleSize <= 0)
                throw new ArgumentOutOfRangeException ("newScaleSize", "Must be Greater than Zero");

            if (!running)
                return;

            ScaleSize = newScaleSize;

            lock (lockWorkers) {

                // Scale down
                while (workers.Count > ScaleSize) {
                    workers [0].Cancel ();
                    workers.RemoveAt (0);
                }

                // Scale up
                while (workers.Count < ScaleSize) {
                    var worker = new ServiceWorker<TNotification> (this, ServiceConnectionFactory.Create ());
                    workers.Add (worker);
                    worker.Start ();
                }

                Log.Write(LogLevel.Info, "Scaled Changed to: " + workers.Count);
            }
        }

        public void NotificationSucceeded (TNotification notification)
        {
            var evt = OnNotificationSucceeded;
            if (evt != null)
                evt (this, new NotificationEventArgs<TNotification>(notification));
        }

        public void NotificationFailed (TNotification notification, AggregateException exception)
        {
            var evt = OnNotificationFailed;
            if (evt != null)
                evt (this, new NotificationExceptionEventArgs<TNotification>(notification, exception));
        }
    }

    internal sealed class ServiceWorker<TNotification> : IDisposable where TNotification : INotification
    {
        private CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

        public ServiceWorker (IServiceBroker<TNotification> broker, IServiceConnection<TNotification> connection)
        {
            Broker = broker;
            Connection = connection;
        }

        public IServiceBroker<TNotification> Broker { get; private set; }

        public IServiceConnection<TNotification> Connection { get; private set; }

        public CancellationTokenSource CancelTokenSource
        {
            get { return cancelTokenSource; }
        }

        public Task WorkerTask { get; private set; }

        public void Start ()
        {
            WorkerTask = Task.Factory.StartNew (async delegate {
                while (!CancelTokenSource.IsCancellationRequested && !Broker.IsCompleted) {

                    try {
                       
                        var toSend = new List<Task> ();
                        foreach (var n in Broker.TakeMany ()) {
                            var t = Connection.Send (n);
                            // Keep the continuation
                            var cont = t.ContinueWith (ct => {
                                var cn = n;
                                var ex = t.Exception;

                                if (ex == null)
                                    Broker.NotificationSucceeded (cn);
                                else
                                    Broker.NotificationFailed (cn, ex);                                
                            });

                            // Let's wait for the continuation not the task itself
                            toSend.Add (cont);
                        }

                        if (toSend.Count <= 0)
                            continue;
                       
                        try {
                            //Task.WaitAll (toSend.ToArray (), CancelTokenSource.Token);
                            Log.Write(LogLevel.Info, "Waiting on all tasks {0}", toSend.Count ());
                            await Task.WhenAll (toSend).ConfigureAwait (false);
                            Log.Write(LogLevel.Info, "All Tasks Finished");
                        } catch (Exception) {
                            Log.Write(LogLevel.Info, "When All Failed");

                        }
                        Log.Write(LogLevel.Info, "Passed WhenAll");

                    } catch (Exception ex) {
                        Log.Write(LogLevel.Info, "Broker.Take: {0}", ex);
                    }
                }

                if (CancelTokenSource.IsCancellationRequested)
                    Log.Write(LogLevel.Info, "CancellationRequested");
                if (Broker.IsCompleted)
                    Log.Write(LogLevel.Info, "IsCompleted");

                Log.Write(LogLevel.Info, "Task Ended");
            }, CancelTokenSource.Token, TaskCreationOptions.LongRunning | TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).Unwrap ();
				
            WorkerTask.ContinueWith (t => {
                var ex = t.Exception;
                if (ex != null)
                    Log.Write(LogLevel.Info, "ServiceWorker.WorkerTask: {0}", ex);
            }, TaskContinuationOptions.OnlyOnFaulted);              
        }

        public void Cancel ()
        {
            CancelTokenSource.Cancel ();
        }

        public void Dispose()
        {
            if (cancelTokenSource != null)
            {
                cancelTokenSource.Dispose();
                cancelTokenSource = null;
            }
        }
    }
}

