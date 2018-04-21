//
//  @(#) TcpServer.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace usis.Platform.Windows
{
    //  ---------------
    //  TcpServer class
    //  ---------------

    /// <summary>
    /// Provides a component that listens for connections from TCP network clients.
    /// </summary>
    /// <seealso cref="TcpListener" />

    public class TcpServer : TcpListener
    {
        #region fields

        private Thread listenerThread;

        #endregion fields

        #region properties

        //  ---------------
        //  Action property
        //  ---------------

        private Action<TcpClient> Action { get; set; }

        #endregion properties

        #region events

        //  -----------------------
        //  ExceptionOccurred event
        //  -----------------------

        /// <summary>
        /// Occurs when an exception occurred while listing for connections
        /// or while performing the connection action which was provided by the <see cref="Start(Action{TcpClient})"/> method.
        /// </summary>

        public event EventHandler<ExceptionEventArgs> ExceptionOccurred;

        #endregion events

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpServer"/> class
        /// with the specified local address and port.
        /// </summary>
        /// <param name="localAddress">The local address.</param>
        /// <param name="port">The port.</param>

        public TcpServer(IPAddress localAddress, int port) : base(localAddress, port) { Initialize(); }

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpServer"/> class
        /// with the specified local endpoint.
        /// </summary>
        /// <param name="localEP">
        /// An <see cref="IPEndPoint"/> that represents the local endpoint
        /// to which to bind the listener <see cref="Socket"/>.
        /// </param>

        public TcpServer(IPEndPoint localEP) : base(localEP) { Initialize(); }

        #endregion construction

        #region methods

        //  -----------------
        //  Initialize method
        //  -----------------

        private void Initialize() { listenerThread = new Thread(Listen); }

        //  ------------
        //  Start method
        //  ------------

        /// <summary>
        /// Starts the server with the specified action to handle an incoming connection.
        /// </summary>
        /// <param name="action">The action handle an incoming connection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is a <c>null</c> reference.</exception>

        public void Start(Action<TcpClient> action)
        {
            Action = action ?? throw new ArgumentNullException(nameof(action));
            Start();
            listenerThread.Start();
        }

        //  --------------------------
        //  OnExceptionOccurred method
        //  --------------------------

        /// <summary>
        /// Raises the <see cref="ExceptionOccurred"/> event.
        /// </summary>
        /// <param name="exception">The exception.</param>

        protected virtual void OnExceptionOccurred(Exception exception)
        {
            ExceptionOccurred?.Invoke(this, new ExceptionEventArgs(exception));
        }

        //  -------------
        //  Listen method
        //  -------------

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void Listen()
        {
            Trace.WriteLine("TcpServer started accepting connections.");
            do
            {
                try
                {
                    TcpClient client = AcceptTcpClient();
                    Action.Invoke(client);
                }
                catch (SocketException exception)
                {
                    if (exception.SocketErrorCode != SocketError.Interrupted)
                    {
                        Trace.WriteLine(exception);
                        OnExceptionOccurred(exception);
                    }
                }
                catch (Exception exception)
                {
                    OnExceptionOccurred(exception);
                }
            }
            while (Active);
            Trace.WriteLine("TcpServer stopped accepting connections.");
        }

        #endregion methods
    }
}

// eof "TcpServer.cs"
