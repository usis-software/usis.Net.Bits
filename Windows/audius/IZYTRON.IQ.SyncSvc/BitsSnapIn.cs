//
//  @(#) BitsSnapIn.cs
//
//  Project:    IZYTRON.IQ.SyncSvc
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 audius GmbH. All rights reserved.

using System.ComponentModel;
using usis.Framework.Windows;

namespace IZYTRON.IQ
{
    //  ----------------
    //  BitsSnapIn class
    //  ----------------

    internal class BitsSnapIn : HttpServerServiceSnapIn<BitsServer>
    {
        #region construction

        //  ------------
        //  construction
        //  ------------

        public BitsSnapIn()
        {
            Prefix = "http://*/bits/";
        }

        #endregion construction

        #region overrides

        //  -------------------
        //  CreateServer method
        //  -------------------

        protected override BitsServer CreateServer()
        {
            return new BitsServer(Application);
        }

        //  -------------------
        //  OnConnecting method
        //  -------------------

        protected override void OnConnecting(CancelEventArgs e)
        {
            base.OnConnecting(e);

            // create inbound directory, if it does not exist.
            Server.CheckInboundDirectory();
        }

        #endregion overrides
    }
}

// eof "BitsSnapIn.cs"
