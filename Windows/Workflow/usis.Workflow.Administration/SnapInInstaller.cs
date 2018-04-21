//
//  @(#) SnapInInstaller.cs
//
//  Project:    usis Workflow Management System
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2016 usis GmbH. All rights reserved.

using System.ComponentModel;

namespace usis.Workflow.Administration
{
    //  ---------------------
    //  SnapInInstaller class
    //  ---------------------

    /// <summary>
    /// Allows the .NET framework to install the snap-in.
    /// </summary>

    [RunInstaller(true)]
    public sealed class SnapInInstaller : Microsoft.ManagementConsole.SnapInInstaller { }
}

// eof "SnapInInstaller.cs"
