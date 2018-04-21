//
//  @(#) TcpTransport.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.IO;
using System.Net.Sockets;
using usis.FinTS.Base;

namespace usis.FinTS
{
    //  ------------------
    //  TcpTransport class
    //  ------------------

    /// <summary>
    /// Provides methods to create FinTS transport providers that are using the TCP protocol.
    /// </summary>

    public static class TcpTransport
    {
        //  --------------------------
        //  CreateBankTransport method
        //  --------------------------

        /// <summary>
        /// Creates a transport provider for a bank system.
        /// </summary>
        /// <param name="client">The TCP connection.</param>
        /// <returns>A newly created transport provider object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="client"/> is a <c>null</c> reference.</exception>

        public static ITransport CreateBankTransport(TcpClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var stream = client.GetStream();
            var transport = new StreamTransport(stream, new CustomerSegmentReader(stream));
            return transport;
        }

        //  ------------------------------
        //  CreateCustomerTransport method
        //  ------------------------------

        /// <summary>
        /// Creates a transport provider for a customer system.
        /// </summary>
        /// <param name="client">The TCP connection.</param>
        /// <returns>A newly created transport provider object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="client"/> is a <c>null</c> reference.</exception>

        public static ITransport CreateCustomerTransport(TcpClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var stream = client.GetStream();
            return new StreamTransport(stream, new BankSegmentReader(stream));
        }
    }

    #region StreamTransport class

    //  ---------------------
    //  StreamTransport class
    //  ---------------------

    internal sealed class StreamTransport : ITransport
    {
        #region fields

        private Stream stream;
        private SegmentReader reader;

        #endregion fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal StreamTransport(Stream stream, SegmentReader reader)
        {
            this.stream = stream;
            this.reader = reader;
        }

        #endregion construction

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>

        public void Dispose()
        {
            if (stream != null) { stream.Close(); stream = null; }
            if (reader != null) { reader.Dispose(); reader = null; }
        }

        #endregion IDisposable implementation

        #region methods

        //  -----------
        //  Send method
        //  -----------

        public void Send(Message message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            message.Serialize(stream);
        }

        //  --------------
        //  Receive method
        //  --------------

        public Message Receive()
        {
            var parser = new MessageParser();
            Segment segment;
            while ((segment = reader.Read()) != null)
            {
                var message = parser.Next(segment);
                if (message != null) return message;
            }
            return null;
        }

        #endregion methods
    }

    #endregion StreamTransport class
}

// eof "TcpTransport.cs"
