//
//  @(#) Interfaces.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;

namespace usis.Framework.Net
{
    #region INetworkOperation interface

    //  ---------------------------
    //  INetworkOperation interface
    //  ---------------------------

    /// <summary>
    /// Represents a network operation that can be performed by a network operation pool.
    /// </summary>

    public interface INetworkOperation
    {
        //  --------------
        //  Perform method
        //  --------------

        /// <summary>
        /// Performs the network operation.
        /// </summary>
        /// <param name="application">The hosting application.</param>
        /// <param name="pool">The network operation pool that should perform the operation.</param>

        void Perform(IApplication application, NetworkOperationPool pool);

        //  ------------------------
        //  ReportException property
        //  ------------------------

        /// <summary>
        /// Gets a value indicating whether to report an exception that was raised during the network operation.
        /// </summary>
        /// <value>
        ///   <c>true</c> if an exception should by reported; otherwise, <c>false</c>.
        /// </value>

        bool ReportException { get; }
    }

    #endregion INetworkOperation interface

    #region IHttpResponse interface

    //  -----------------------
    //  IHttpResponse interface
    //  -----------------------

    /// <summary>
    /// Encapsulates HTTP-response information.
    /// </summary>

    public interface IHttpResponse
    {
        //  -------------------
        //  BodyToString method
        //  -------------------

        /// <summary>
        /// Gets the body as string.
        /// </summary>
        /// <returns>
        /// A string that represents the body of the HTTP response.
        /// </returns>

        string BodyToString();
    }

    #endregion IHttpResponse interface

    #region IHttpNetworkTask interface

    //  --------------------------
    //  IHttpNetworkTask interface
    //  --------------------------

    /// <summary>
    /// Represents a HTTP network task that performs a request and returns a response.
    /// </summary>
    /// <seealso cref="IDisposable" />

    public interface IHttpNetworkTask : IDisposable
    {
        //  --------------
        //  Perform method
        //  --------------

        /// <summary>
        /// Performs the specified HTTP request.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="handler">
        /// An handler that is executed to receive the HTTP response
        /// or an exception, when the request failed.
        /// </param>

        void Perform(HttpRequest httpRequest, Action<IHttpResponse, Exception> handler);
    }

    #endregion IHttpNetworkTask interface
}

// eof "Interfaces.cs"
