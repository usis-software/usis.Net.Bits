//
//  @(#) SnapInInstaller.cs
//
//  Project:    usis Database Registry Editor
//  System:     Microsoft Visual Studio 14
//  Author:     Udo Schäfer
//
//	Copyright (c) 2015 usis GmbH. All rights reserved.

using System.ComponentModel;

namespace usis.Data.Registry.Editor
{
    //  ---------------------
    //  SnapInInstaller class
    //  ---------------------

    /// <summary>
    /// Allows the .NET framework to install the snap-in.
    /// </summary>

    [RunInstaller(true)]
    public sealed class SnapInInstaller : Microsoft.ManagementConsole.SnapInInstaller
    {
    }
}

// eof "SnapInInstaller.cs"
