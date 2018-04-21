//
//	@(#) MmcSnapIn.cs
//
//  Project:    usisSvr
//  System:     Microsoft Visual Studio 12
//	Author:		Udo Schäfer
//
//	Copyright (c) 2014 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole;
using System.Diagnostics.CodeAnalysis;

namespace usis.Server.Administration
{
	//	---------------
	//	MmcSnapIn class
	//	---------------

	[SnapInSettings("{A420EFE9-D88C-486F-9947-87579C71FEDF}",
		 DisplayName = "usis Server",
		 Description = "Allow you to configure and manage solutions hosted by usis Server.",
		 Vendor = "usis GmbH")]
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]

	internal class MmcSnapIn : SnapIn
	{
		public MmcSnapIn()
		{
			this.RootNode = new ScopeNode();
			this.RootNode.DisplayName = "usis Server (local)";
		}

	} // MmcSnapIn class

} // usis.Server.Administration namespace

// eof "MmcSnapIn.cs"
