//
//  @(#) NetworkOperation.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System;

namespace usis.Framework.Net
{
    //  ----------------------
    //  NetworkOperation class
    //  ----------------------

    /// <summary>
    /// Provides a base class for a network operation that can be performed
    /// by a network operation pool.
    /// </summary>
    /// <seealso cref="INetworkOperation" />
    /// <seealso cref="IDisposable" />

    public abstract class NetworkOperation : INetworkOperation, IDisposable
    {
        #region properties

        //  ------------------------
        //  ReportException property
        //  ------------------------

        /// <summary>
        /// Gets a value indicating whether to report an exception that was raised during the network operation.
        /// </summary>
        /// <value>
        /// <c>true</c> if an exception should by reported; otherwise, <c>false</c>.
        /// </value>

        public bool ReportException { get; set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkOperation"/> class.
        /// </summary>

        protected NetworkOperation() { ReportException = true; }

        #endregion construction

        #region methods

        //  --------------
        //  Perform method
        //  --------------

        /// <summary>
        /// Performs the network operation.
        /// </summary>
        /// <param name="application">The hosting application.</param>
        /// <param name="pool">The network operation pool that should perform the operation.</param>

        public abstract void Perform(IApplication application, NetworkOperationPool pool);

        //  -------------
        //  Finish method
        //  -------------

        /// <summary>
        /// Calls the <see cref="NetworkOperationPool.Finished(INetworkOperation, Exception)"/> method on the specified
        /// <see cref="NetworkOperationPool"/>.
        /// </summary>
        /// <param name="pool">The network operation pool.</param>
        /// <param name="exception">An exception that occurred when performing the network operation.</param>

        protected void Finish(NetworkOperationPool pool, Exception exception)
        {
            pool?.Finished(this, exception);
        }

        #endregion methods

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>

        protected virtual void Dispose(bool disposing) { }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable implementation
    }
}

// eof "NetworkOperation.cs"
