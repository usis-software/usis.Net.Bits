//
//  @(#) ITransport.cs
//
//  Project:    usis.FinTS
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

namespace usis.FinTS
{
    //  --------------------
    //  ITransport interface
    //  --------------------

    /// <summary>
    /// Defines methods a FinTS transport provider must implement.
    /// </summary>

    public interface ITransport : System.IDisposable
    {
        //  -----------
        //  Send method
        //  -----------

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message">The message to send.</param>

        void Send(Message message);

        //  --------------
        //  Receive method
        //  --------------

        /// <summary>
        /// Receives a message.
        /// </summary>
        /// <returns>The message received.</returns>

        Message Receive();
    }
}

// eof "ITransport.cs"
