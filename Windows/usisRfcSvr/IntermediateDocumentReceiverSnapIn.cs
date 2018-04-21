//
//  @(#) IntermediateDocumentReceiverSnapIn.cs
//
//  Project:    usis RFC Server
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

#pragma warning disable 1591

using SAP.Middleware.Connector;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using usis.Framework;

namespace usis.Middleware.SAP
{
    //  ----------------------------------------
    //  IntermediateDocumentReceiverSnapIn class
    //  ----------------------------------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class IntermediateDocumentReceiverSnapIn : ServiceSnapIn
    {
        #region overrides

        //  -------------------
        //  OnConnecting method
        //  -------------------

        protected override void OnConnecting(CancelEventArgs e)
        {
            var extension = Application.With<RfcServerApplicationExtension>(true);
            extension.RegisterHandler("usis.RFC.RAC", typeof(IntermediateDocumentServer));

            base.OnConnecting(e);
        }

        #endregion overrides
    }

    //  --------------------------------
    //  IntermediateDocumentServer class
    //  --------------------------------

    internal static class IntermediateDocumentServer
    {
        //  --------------------------
        //  InboundAsynchronous method
        //  --------------------------

        [RfcServerFunction(Name = "IDOC_INBOUND_ASYNCHRONOUS")]
        public static void InboundAsynchronous(
            RfcServerContext context,
            IRfcFunction function)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (function == null) throw new ArgumentNullException(nameof(function));

            Debug.WriteLine("IDOC_INBOUND_ASYNCHRONOUS called...");
            System.Threading.Thread.Sleep(5000);
            Debug.WriteLine("IDOC_INBOUND_ASYNCHRONOUS returned.");
        }
    }
}

// eof "IntermediateDocumentReceiverSnapIn.cs"
