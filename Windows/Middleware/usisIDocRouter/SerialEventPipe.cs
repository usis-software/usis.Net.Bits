//
//  @(#) SerialEventPipe.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace usis.Platform
{
    //  ---------------------
    //  SerialEventPipe class
    //  ---------------------

    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    internal sealed class SerialEventPipe : IDisposable
    {
        #region fields

        private ConcurrentQueue<EventArgs> queue = new ConcurrentQueue<EventArgs>();

        private Task dequeueTask;

        private EventWaitHandle stop = new EventWaitHandle(false, EventResetMode.ManualReset);
        private EventWaitHandle dequeue = new EventWaitHandle(false, EventResetMode.AutoReset);

        #endregion fields

        #region public methods

        //  ------------
        //  Start method
        //  ------------

        public void Start()
        {
            dequeueTask = Task.Factory.StartNew(Dequeue, TaskCreationOptions.LongRunning);
        }

        //  --------------
        //  Enqueue method
        //  --------------

        public void Enqueue(EventArgs e)
        {
            queue.Enqueue(e);
            dequeue.Set();
        }

        //  -----------
        //  Stop method
        //  -----------

        public void Stop()
        {
            stop.Set();
            dequeueTask?.Wait();
        }

        #endregion public methods

        #region public events

        //  --------------
        //  Dequeued event
        //  --------------

        public event EventHandler Process;

        //  ------------
        //  Failed event
        //  ------------

        public event EventHandler<ExceptionEventArgs> Failed;

        #endregion public events

        #region private methods

        //  --------------
        //  Dequeue method
        //  --------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void Dequeue()
        {
            Trace.WriteLine("Event pipe thread started ...");

            var handles = new WaitHandle[] { stop, dequeue };
            while (WaitHandle.WaitAny(handles) != 0)
            {
                Trace.WriteLine("Checking event pipe ...");
                EventArgs e = null;
                while (queue.TryDequeue(out e))
                {
                    Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Event '{0}' dequeued.", e));
                    try
                    {
                        Process?.Invoke(this, e);
                    }
                    catch (Exception exception) { Failed?.Invoke(this, new ExceptionEventArgs(exception)); }
                }
            }

            Trace.WriteLine("... event pipe thread stopped.");
        }

        #endregion private methods

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        private bool disposed = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (stop != null) stop.Dispose();
                    if (dequeue != null) dequeue.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~EventQueue() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        #endregion IDisposable implementation
    }
}

// eof "SerialEventPipe.cs"
