//
//  @(#) IDocRouterSnapIn.cs
//
//  Project:    usis IDoc Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using usis.Framework;
using usis.Framework.Configuration;
using usis.Framework.Windows;
using usis.Platform;
using usis.Platform.Windows;

namespace usis.Middleware.SAP
{
    //  ----------------------
    //  IDocRouterSnapIn class
    //  ----------------------

    internal sealed class IDocRouterSnapIn : SnapIn, IDisposable
    {
        #region constants

        internal const string ServerName = "IDocRouter";
        private const string DataDirectoryName = ServerName;
        private const string OutboundDirectoryName = "Outbound";
        private const string SentDirectoryName = "Sent";
        private const string IDocFileExtension = "idoc";

        #endregion constants

        #region fields

        private FileSystemWatcher watcher;

        //private ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
        //private EventWaitHandle stop = new EventWaitHandle(false, EventResetMode.ManualReset);
        //private EventWaitHandle dequeue = new EventWaitHandle(false, EventResetMode.AutoReset);
        //private Task dequeueTask;
        private SerialEventPipe pipe = new SerialEventPipe();
        private System.Timers.Timer checkQueueTimer;

        private int checkQueueInterval = 30;

        #endregion fields

        #region Main method

        //  -----------
        //  Main method
        //  -----------

        internal static void Main(string[] args)
        {
            DisplayHeader();

            // start applicaton server service with IDoc Router snap-in.
            ServicesHost.StartServicesOrConsole(args, new IDocRouterService());
        }

        #endregion Main method

        #region overrides

        //  -------------------
        //  OnConnecting method
        //  -------------------

        protected override void OnConnecting(CancelEventArgs e)
        {
            Application.SetEventLogSource(ServerName);
        
            // register IDoc receiver RFC service
            Application.With<RfcServiceApplicationExtension>(true).Service.RegisterHandler(ServerName, typeof(IDocReceiver));

            // create inbound/outboud directory if it does not exist.
            IDocReceiver.CheckInboundDirectory();
            CheckOutboundDirectory();

            // start outbound processing
            StartWatcher();
            CheckQueue();

            base.OnConnecting(e);
        }

        protected override void OnDisconnecting(CancelEventArgs e)
        {
            //stop.Set();
            //dequeueTask?.Wait();
            pipe.Stop();

            base.OnDisconnecting(e);
        }

        #endregion overrides

        #region private methods

        //  --------------------
        //  DisplayHeader method
        //  --------------------

        private static void DisplayHeader()
        {
            Console.WriteLine();
#if USIS
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            Console.WriteLine(assembly.GetTitle());
            Console.WriteLine(assembly.GetCopyright());
#else
            Console.WriteLine(Strings.RouterStarted);
#endif
            Console.WriteLine();
        }

        //  -------------------
        //  StartWatcher method
        //  -------------------

        private void StartWatcher()
        {
            //dequeueTask = Task.Factory.StartNew(Dequeue, TaskCreationOptions.LongRunning);
            pipe.Failed += (sender, e) => { Application.ReportException(e.Exception); };
            pipe.Process += (sender, e) =>
            {
                var path = (e as PathEventArgs)?.Path;
                if (File.Exists(path))
                {
                    IDocSender.SendFile(path, "IDocPartner");
                    var sent = Path.Combine(GetSentDirectoryPath(), Path.GetFileName(path));
                    if (File.Exists(sent)) File.Delete(sent);
                    File.Move(path, sent);
                    Debug.Print("File '{0}' moved to '{1}.", path, GetSentDirectoryPath());
                }
            };
            pipe.Start();

            watcher = new FileSystemWatcher();
            watcher.Changed += (sender, e) => { PushFile(e.FullPath); };
            watcher.Created += (sender, e) => { PushFile(e.FullPath); };
            watcher.Deleted += (sender, e) => { PushFile(e.FullPath); };
            watcher.Renamed += (sender, e) => { PushFile(e.FullPath); };

            watcher.Path = GetOutboudDirectoryPath();
            watcher.Filter = Path.ChangeExtension("*", IDocFileExtension);
            watcher.EnableRaisingEvents = true;

            // create "check queue" timer
            checkQueueTimer = new System.Timers.Timer(Convert.ToDouble(new TimeSpan(0, 0, checkQueueInterval).TotalMilliseconds));
            checkQueueTimer.Elapsed += (sender, e) => CheckQueue();
            checkQueueTimer.AutoReset = false;
        }

        //  ---------------
        //  PushFile method
        //  ---------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void PushFile(string path)
        {
            lock (/*queue*/this)
            {
                try
                {
                    if (File.Exists(path))
                    {
                        pipe.Enqueue(new PathEventArgs(path));
                        //queue.Enqueue(path);
                        Debug.WriteLine("File '{0}' enqueued.", path as object);
                        //dequeue.Set();
                    }
                }
                catch (Exception exception) { Application.ReportException(exception); }
            }
        }

        internal class PathEventArgs : EventArgs
        {
            public string Path { get; }

            public PathEventArgs(string path) { Path = path; }
        }

        //  -----------------
        //  CheckQueue method
        //  -----------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void CheckQueue()
        {
            Trace.WriteLine("Looking for unprocessed files...");
            try
            {
                foreach (var file in Directory.EnumerateFiles(GetOutboudDirectoryPath(), Path.ChangeExtension("*", IDocFileExtension)))
                {
                    PushFile(file);
                }
            }
            catch (Exception exception) { Application.ReportException(exception); }

            checkQueueTimer.Start();
        }

        //  --------------
        //  Dequeue method
        //  --------------

        //[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        //private void Dequeue()
        //{
        //    while (WaitHandle.WaitAny(new WaitHandle[] { stop, dequeue }) != 0)
        //    {
        //        Debug.WriteLine("Checking queue...");
        //        while (queue.TryDequeue(out string path))
        //        {
        //            Debug.Print("File '{0}' dequeued.", path);
        //            try
        //            {
        //                if (File.Exists(path))
        //                {
        //                    IDocSender.SendFile(path, "IDocPartner");
        //                    var sent = Path.Combine(GetSentDirectoryPath(), Path.GetFileName(path));
        //                    if (File.Exists(sent)) File.Delete(sent);
        //                    File.Move(path, sent);
        //                    Debug.Print("File '{0}' moved to '{1}.", path, GetSentDirectoryPath());
        //                }
        //            }
        //            catch (Exception exception) { Application.ReportException(exception); }
        //        }
        //    }
        //    Debug.WriteLine("... dequeue thread stopped.");
        //}

        //  -----------------------------
        //  CheckOutboundDirectory method
        //  -----------------------------

        internal static void CheckOutboundDirectory()
        {
            var path = GetOutboudDirectoryPath();
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            Debug.Print("Outbound directory: {0}", path);

            path = GetSentDirectoryPath();
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            Debug.Print("Sent directory: {0}", path);
        }

        //  ------------------------------
        //  GetOutboudDirectoryPath method
        //  ------------------------------

        private static string GetOutboudDirectoryPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), DataDirectoryName, OutboundDirectoryName);
        }

        //  ---------------------------
        //  GetSentDirectoryPath method
        //  ---------------------------

        private static string GetSentDirectoryPath()
        {
            return Path.Combine(GetOutboudDirectoryPath(), SentDirectoryName);
        }

        #endregion private methods

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        void IDisposable.Dispose()
        {
            if (watcher != null) { watcher.Dispose(); watcher = null; }
            if (pipe != null) { pipe.Dispose(); pipe = null; }
            //if (dequeue != null) { dequeue.Dispose(); dequeue = null; }
            //if (stop != null) { stop.Dispose(); stop = null; }
            if (checkQueueTimer != null) { checkQueueTimer.Dispose(); checkQueueTimer = null; }
        }

        #endregion IDisposable implementation
    }

    #region IDocRouterService class

    //  -----------------------
    //  IDocRouterService class
    //  -----------------------

    internal sealed class IDocRouterService : ApplicationServer.Service
    {
        //  ------------
        //  construction
        //  ------------

        internal IDocRouterService() : base(new ConfigurationRoot(typeof(IDocRouterSnapIn))) { }

        //  -------------
        //  Name property
        //  -------------

        public override string Name => IDocRouterSnapIn.ServerName;
    }

    #endregion IDocRouterService class
}

// eof "IDocRouterSnapIn.cs"
