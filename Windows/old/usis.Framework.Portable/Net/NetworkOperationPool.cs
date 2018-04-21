//
//  @(#) NetworkOperationPool.cs
//
//  Project:    usis Framework
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;
using System.Collections.Generic;

namespace usis.Framework.Portable.Net
{
    //  --------------------------
    //  NetworkOperationPool class
    //  --------------------------

    /// <summary>
    /// Represents an application extension that can perform
    /// network operations.
    /// </summary>
    /// <seealso cref="ApplicationExtension" />

    [Obsolete("Use type from usis.Framework.Net namespace instead.")]
    public class NetworkOperationPool : ApplicationExtension
    {
        #region fields

        private HashSet<INetworkOperation> operations = new HashSet<INetworkOperation>();

        #endregion fields

        #region methods

        //  --------------
        //  Perform method
        //  --------------

        /// <summary>
        /// Performs the specified network operation.
        /// </summary>
        /// <param name="operation">The network operation to perform.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="operation"/> is a null reference.
        /// </exception>

        public void Perform(INetworkOperation operation)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            lock (operations)
            {
                operations.Add(operation);
                if (operations.Count == 1)
                {
                    OnNetworkActivityStarted();
                }
            }
            operation.Perform(Owner, this);
        }

        //  ---------------
        //  Finished method
        //  ---------------

        /// <summary>
        /// Removes the specified network operation from the pool when it is finished.
        /// </summary>
        /// <param name="operation">The network operation that is finished.</param>
        /// <param name="exception">An exception that occurred when performing the network operation.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="operation"/> is a null reference.
        /// </exception>

        public void Finished(INetworkOperation operation, Exception exception)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            lock (operations)
            {
                operations.Remove(operation);
                (operation as IDisposable)?.Dispose();
                if (operations.Count == 0)
                {
                    OnNetworkActivityStopped();
                }
            }
            if (exception != null && operation.ReportException)
            {
                Owner.ReportException(exception);
            }
        }

        //  -------------------------------
        //  OnNetworkActivityStarted method
        //  -------------------------------

        /// <summary>
        /// Called when any network activity is started.
        /// </summary>

        protected virtual void OnNetworkActivityStarted() { }

        /// <summary>
        /// Called when all network activities are stopped.
        /// </summary>

        protected virtual void OnNetworkActivityStopped() { }

        #endregion methods
    }
}

// eof "NetworkOperationPool.cs"
