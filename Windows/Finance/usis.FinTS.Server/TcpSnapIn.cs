//
//  @(#) TcpSnapIn.cs
//
//  Project:    usis.FinTS.Server
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using usis.Framework;
using usis.Platform.Windows;

namespace usis.FinTS.Server
{
    //  ---------------
    //  TcpSnapIn class
    //  ---------------

    internal class TcpSnapIn : ServiceSnapIn
    {
        #region fields

        private TcpServer server = new TcpServer(IPAddress.Loopback, 3000);

        #endregion fields

        #region properties

        //  -----------------------------
        //  ProgramDataDirectory property
        //  -----------------------------

        internal static string ProgramDataDirectory
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    Constants.CompanyDirectory,
                    Constants.ProductDirectory);
            }
        }

        #endregion properties

        #region overrides

        //  -------------------
        //  OnConnecting method
        //  -------------------

        protected override void OnConnecting(CancelEventArgs e)
        {
            // create and define the directory for storing the database
            if (!Directory.Exists(ProgramDataDirectory)) Directory.CreateDirectory(ProgramDataDirectory);
            AppDomain.CurrentDomain.SetData(Constants.DataDirectoryPropertyName, ProgramDataDirectory);

            // create model instance and add it as an extension
            Application.Extensions.Add(new Model(() => Start()));

            base.OnConnecting(e);
        }

        //  ---------------------
        //  OnDisconnected method
        //  ---------------------

        protected override void OnDisconnected(EventArgs e)
        {
            // start listening for TCP connections
            server.Stop();

            base.OnDisconnected(e);
        }

        #endregion overrides

        #region methods

        //  ------------
        //  Start method
        //  ------------

        private void Start()
        {
            // start listening for TCP connections; run connection as a job
            server.Start(client => Application.RunAsJob(job => Connection(Application, job, client)));
        }

        //  -----------------
        //  Connection method
        //  -----------------

        private static void Connection(IApplication application, IJob job, TcpClient client)
        {
            Trace.WriteLine("TCP connection opened.");
            job.RemoveWhenCompleted = true;

            using (var transport = TcpTransport.CreateBankTransport(client))
            {
                using (var dialog = BankDialog.Initialize(transport))
                {
                    do
                    {
                        dialog.Process((message) => application.With<Model>().ProcessMessage(dialog, message));
                    }
                    while (dialog.KeepConnectionActive);
                }
            }

            client.Close();
            Trace.WriteLine("TCP connection closed.");
        }

        #endregion methods
    }
}

// eof "TcpSnapIn.cs"
