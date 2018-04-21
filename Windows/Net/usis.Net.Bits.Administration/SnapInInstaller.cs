//
//  @(#) SnapInInstaller.cs
//
//  Project:    usis BITS Administration
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System.ComponentModel;

namespace usis.Net.Bits.Administration
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
