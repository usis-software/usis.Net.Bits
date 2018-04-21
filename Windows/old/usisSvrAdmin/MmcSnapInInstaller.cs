//
//	@(#) MmcSnapInInstaller.cs
//
//  Project:    usisSvr
//  System:     Microsoft Visual Studio 12
//	Author:		Udo Schäfer
//
//	Copyright (c) 2014 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using System.ComponentModel;

namespace usis.Server.Administration
{
	//	------------------------
	//	MmcSnapInInstaller class
	//	------------------------
	/// <summary>
	/// Provides the metadata to populate or remove register entries for the snap-in.
	/// </summary>
	
	[RunInstaller(true)]
	public class MmcSnapInInstaller : SnapInInstaller
	{
	} // MmcSnapInInstaller class

} // usis.Server.Administration namespace

// eof "MmcSnapInInstaller.cs"
