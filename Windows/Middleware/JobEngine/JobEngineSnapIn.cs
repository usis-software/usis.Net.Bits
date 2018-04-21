//
//  @(#) JobEngineSnapIn.cs
//
//  Project:    usis Job Engine
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2018 usis GmbH. All rights reserved.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using usis.Framework;
using usis.Framework.ServiceModel;
using usis.Platform;

namespace usis.JobEngine
{
    //  ---------------------
    //  JobEngineSnapIn class
    //  ---------------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class JobEngineSnapIn : ServiceSnapIn
    {
        protected override void OnConnecting(CancelEventArgs e)
        {
            var configuration = Platform.Windows.RegistryValueStorage.OpenLocalMachine().CreateStorage(@"SOFTWARE\usis\JobEngine\Jobs", false);
            foreach (var item in configuration.Storages)
            {
                //var key = item.Name;
                var typeName = item.GetString("Type");
                var type = System.Type.GetType(typeName);
                System.Activator.CreateInstance(type);
            }

            Application.ConnectRequiredSnapIns(this, typeof(ServiceHostSnapIn<WcfServiceHostFactory<Repository>>));
            Application.ConnectRequiredSnapIns(this, typeof(TestJob));

            base.OnConnecting(e);
        }
    }
}

// eof "JobEngineSnapIn.cs"
