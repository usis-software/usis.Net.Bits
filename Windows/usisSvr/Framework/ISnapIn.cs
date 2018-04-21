//
//	@(#) ISnapIn.cs
//
//  Project:    usisSvr
//  System:     Microsoft Visual Studio 12
//	Author:		Udo Schäfer
//
//	Copyright (c) 2014 usis GmbH. All rights reserved.

using System;
using System.ServiceModel;

namespace usis.Framework
{
	#region ISolution interface

	//	-------------------
	//	ISolution interface
	//	-------------------

	internal interface ISolution : IExtensibleObject<ISolution>
	{
	} // ISolution interface

	#endregion ISolution interface

	#region ISnapIn interface

	//	-----------------
	//	ISnapIn interface
	//	-----------------

	internal interface ISnapIn
	{
		//	-------------------
		//	OnConnection method
		//	-------------------

		/// <summary>
		/// A <see cref="ISolution"/> calls this method during startup
		/// after an instance of the implementing class has been created.
		/// </summary>
		/// <param name="theSolution">
		/// The theSolution that connects the snap-in.
		/// </param>
		/// <returns>
		/// <b>true</b> if the snap-in is connected; otherwise, <b>false</b>.
		/// </returns>

		bool OnConnection(ISolution solution);

		//	--------------------
		//	CanDisconnect method
		//	--------------------

		/// <summary>
		/// Called to determine whether the implementing snap-in
		/// is ready to be disconnnected.
		/// </summary>
		/// <returns>
		/// <b>true</b> when the snap-in is ready to be disconnected;
		/// otherwise, <b>false</b>.
		/// </returns>

		bool CanDisconnect();

		//	----------------------
		//	OnDisconnection method
		//	----------------------

		/// <summary>
		/// Called to disconnect the implementing snap-in.
		/// </summary>

		void OnDisconnection();
	
	} // ISnapIn interface

	#endregion ISnapIn interface

} // usis.Framework namespace

// eof "ISnapIn.cs"
