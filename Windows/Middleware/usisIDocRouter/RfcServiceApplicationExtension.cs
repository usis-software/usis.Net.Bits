//
//  @(#) RfcServiceApplicationExtension.cs
//
//  Project:    usis IDoc Router
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016,2017 usis GmbH. All rights reserved.

using System.Diagnostics.CodeAnalysis;
using usis.Framework;

namespace usis.Middleware.SAP
{
    //  ------------------------------------
    //  RfcServiceApplicationExtension class
    //  ------------------------------------

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class RfcServiceApplicationExtension : ApplicationExtension
    {
        #region properties

        //  ---------------
        //  Server property
        //  ---------------

        internal RfcService Service { get; private set; }

        #endregion properties

        #region overrides

        //  ---------------
        //  OnAttach method
        //  ---------------

        protected override void OnAttach()
        {
            Service = new RfcService();

            base.OnAttach();
        }

        //  --------------
        //  OnStart method
        //  --------------

        protected override void OnStart()
        {
            Service.Start();

            base.OnStart();
        }

        //  ---------------
        //  OnDetach method
        //  ---------------

        protected override void OnDetach()
        {
            Service.Shutdown();

            base.OnDetach();
        }

        #endregion overrides
    }
}

// eof "RfcServiceApplicationExtension.cs"
