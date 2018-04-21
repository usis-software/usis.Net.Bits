//
//	@(#) SnapIn.cs
//
//  Project:    usisSvr
//  System:     Microsoft Visual Studio 12
//	Author:		Udo Schäfer
//
//	Copyright (c) 2014 usis GmbH. All rights reserved.

using System;
using System.ComponentModel;

namespace usis.Framework
{
	//	------------
	//	SnapIn class
	//	------------

	internal class SnapIn : ISnapIn
	{
		#region Solution property

		//	-----------------
		//	Solution property
		//	-----------------

		protected ISolution Solution
		{
			get;
			private set;

		} // Solution property

		#endregion Solution property

		//	-------------------
		//	OnConnecting method
		//	-------------------

		protected virtual void OnConnecting(CancelEventArgs e)
		{
		} // OnConnecting method

		//	------------------
		//	OnConnected method
		//	------------------

		protected virtual void OnConnected(EventArgs e)
		{
		} // OnConnected method

		protected virtual void OnDisconnecting(CancelEventArgs e)
		{
		}

		protected virtual void OnDisconnected(EventArgs e)
		{
		}

		#region ISnapIn implementation

		//	-------------------
		//	OnConnection method
		//	-------------------

		public bool OnConnection(ISolution solution)
		{
			this.Solution = solution;

			CancelEventArgs e = new CancelEventArgs();
			this.OnConnecting(e);

			if (!e.Cancel) this.OnConnected(EventArgs.Empty);

			return !e.Cancel;
		
		} // OnConnection method

		public bool CanDisconnect()
		{
			CancelEventArgs e = new CancelEventArgs();
			this.OnDisconnecting(e);

			return !e.Cancel;
		
		} // CanDisconnect method

		public void OnDisconnection()
		{
			this.OnDisconnected(EventArgs.Empty);

			this.Solution = null;

		} // OnDisconnection method

		#endregion ISnapIn implementation

	} // SnapIn class

} // usis.Framework namespace

// eof "SnapIn.cs"
