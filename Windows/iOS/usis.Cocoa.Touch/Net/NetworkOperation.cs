//
//  @(#) NetworkOperation.cs
//
//  Project:    usis Cocoa
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System;

namespace usis.Framework.Portable.Net
{
    //  ----------------------
    //  NetworkOperation class
    //  ----------------------

    public abstract class NetworkOperation : INetworkOperation, IDisposable
    {
        #region properties

        //  ------------------------
        //  ReportException property
        //  ------------------------

        public bool ReportException { get; set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        protected NetworkOperation() { ReportException = true; }

        #endregion construction

        #region methods

        //  --------------
        //  Perform method
        //  --------------

        public abstract void Perform(IApplication application, NetworkOperationPool pool);

        //  -------------
        //  Finish method
        //  -------------

        protected void Finish(NetworkOperationPool pool, Exception exception)
        {
            pool?.Finished(this, exception);
        }

        #endregion methods

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        protected virtual void Dispose(bool disposing) { }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable implementation
    }
}

// eof "NetworkOperation.cs"
