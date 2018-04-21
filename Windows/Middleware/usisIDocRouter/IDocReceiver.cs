//
//  @(#) IDocReceiver.cs
//
//  Project:    usis IDoc Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using SAP.Middleware.Connector;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace usis.Middleware.SAP
{
    //  ------------------
    //  IDocReceiver class
    //  ------------------

    internal static class IDocReceiver
    {
        #region RFC server functions

        //  --------------------------
        //  InboundAsynchronous method
        //  --------------------------

        [RfcServerFunction(Name = "IDOC_INBOUND_ASYNCHRONOUS")]
        public static void InboundAsynchronous(RfcServerContext context, IRfcFunction function)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (function == null) throw new ArgumentNullException(nameof(function));

            Debug.WriteLine("RFC 'IDOC_INBOUND_ASYNCHRONOUS' called...");
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                // read all IDocs from RFC function tables
                foreach (var document in IDoc.ReadDocuments(new IDocRfcFunctionReader(function)))
                {
                    // write IDoc to a temporary file
                    var tmp = Path.GetTempFileName();
                    using (var writer = new StreamWriter(tmp, false, Encoding.UTF8))
                    {
                        document.Write(writer);
                    }

                    // copy temporary file to inbound directory
                    var path = Path.Combine(GetInboudDirectoryPath(document.ControlRecord.SenderPartnerNumber), document.GetFileName());
                    if (File.Exists(path)) File.Delete(path);
                    File.Move(tmp, path);

                    Console.WriteLine(Strings.IDocReceived, path);
                    Debug.WriteLine(Strings.IDocReceived, path as object);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            sw.Stop();
            Debug.WriteLine("RFC 'IDOC_INBOUND_ASYNCHRONOUS' completed in {0} ms.", sw.ElapsedMilliseconds);
        }

        #endregion RFC server functions

        #region internal and private methods

        //  ----------------------------
        //  CheckInboundDirectory method
        //  ----------------------------

        internal static void CheckInboundDirectory()
        {
            var path = GetInboudDirectoryPath();
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            Debug.Print("Inboud directory: {0}", path);
        }

        //  -----------------------------
        //  GetInboudDirectoryPath method
        //  -----------------------------

        private static string GetInboudDirectoryPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "IDocRouter", "Inbound");
        }

        private static string GetInboudDirectoryPath(string sender)
        {
            var path = GetInboudDirectoryPath();
            if (!string.IsNullOrWhiteSpace(sender))
            {
                path = Path.Combine(path, sender);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            }
            return path;
        }

        #endregion internal and private methods
    }
}

// eof "IDocReceiver.cs"
