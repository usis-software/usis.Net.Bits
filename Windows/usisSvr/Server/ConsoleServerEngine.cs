//
//	@(#) ConsoleServerEngine.cs
//
//  Project:    usisSvr
//  System:     Microsoft Visual Studio 12
//	Author:		Udo Schäfer
//
//	Copyright (c) 2014 usis GmbH. All rights reserved.

using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace usis.Server
{
	#region ConsoleServerEngine class

	//	-------------------------
	//	ConsoleServerEngine class
	//	-------------------------

	internal class ConsoleServerEngine : ServerEngine, IServerEngine
	{
		#region private fields

		private ManualResetEvent stopEvent = new ManualResetEvent(false);

		#endregion // private fields

		#region Run method

		//	----------
		//	Run method
		//	----------

		int IServerEngine.Run()
		{
			if (this.StartServices() > 0)
			{
				// handle Win32 console events
				ControlEventHandler handler = new ControlEventHandler(this.OnControlEvent);
				NativeMethods.SetConsoleCtrlHandler(handler, true);

				Console.Write(Strings.PressCtrlC);
				this.stopEvent.WaitOne();

				Terminate();

				ConsoleExtension.FlushInputBuffer();

				return 0;
			}
			else Console.WriteLine(Strings.NoServicesConfigured);

			return -1;

		} // Run method

		#endregion Run method

		#region private methods

		#region StartServices method

		//	--------------------
		//	StartServices method
		//	--------------------

		private int StartServices()
		{
			int i = 0;
			foreach (var service in this.Services)
			{
				service.OnStart();
				i++;
			}
			return i;
		
		} // StartServices method

		#endregion StartServices method

		#region StopServices method

		//	--------------------
		//	StopServices metthod
		//	--------------------

		private void StopServices()
		{
			foreach (var service in this.Services)
			{
				service.OnStop();
			}
		
		} // StopServices metthod

		#endregion StopServices method

		#region OnControlEvent method

		//	---------------------
		//	OnControlEvent method
		//	---------------------

		private bool OnControlEvent(ConsoleEvent consoleEvent)
		{
			if (consoleEvent == ConsoleEvent.CTRL_CLOSE ||
				consoleEvent == ConsoleEvent.CTRL_LOGOFF ||
				consoleEvent == ConsoleEvent.CTRL_SHUTDOWN)
			{
				this.Terminate();
			}
			else this.stopEvent.Set();

			return true; // do not call default handler

		} // OnControlEvent method

		#endregion OnControlEvent method

		#region Terminate method

		//	----------------
		//	Terminate method
		//	----------------

		private void Terminate()
		{
			this.StopServices();

			Console.WriteLine(Strings.ServerStopped);
			Console.Beep();

		} // Terminate method

		#endregion Terminate method

		#endregion // private methods

		#region IDisposable methods

		//	--------------
		//	Dispose method
		//	--------------

		void IDisposable.Dispose()
		{
			this.stopEvent.Dispose();

		} // Dispose method

		#endregion IDisposable methods

	} // ConsoleServerEngine class

	#endregion // ConsoleServerEngine class

	#region ConsoleExtension class

	//	----------------------
	//	ConsoleExtension class
	//	----------------------

	internal static class ConsoleExtension
	{
		//	-----------------------
		//	FlushInputBuffer method
		//	-----------------------

		internal static void FlushInputBuffer()
		{
			SafeFileHandle handle = NativeMethods.GetStdHandle(NativeMethods.STD_INPUT_HANDLE);
			if (!handle.IsInvalid)
			{
				NativeMethods.FlushConsoleInputBuffer(handle);
			}

		} // FlushInputBuffer method

	} // ConsoleExtension class

	#endregion // ConsoleExtension class

	#region Interop

	//	------------------------
	//	ConsoleEvent enumeration
	//	------------------------

	internal enum ConsoleEvent
	{
		CTRL_C = 0,
		CTRL_BREAK = 1,
		CTRL_CLOSE = 2,
		CTRL_LOGOFF = 5,
		CTRL_SHUTDOWN = 6

	} // ConsoleEvent enumeration

	//	----------------------------
	//	ControlEventHandler delegate
	//	----------------------------

	internal delegate bool ControlEventHandler(ConsoleEvent consoleEvent);

	//	-------------------
	//	NativeMethods class
	//	-------------------

	internal static class NativeMethods
	{
		internal const int STD_INPUT_HANDLE = -10;

		//	----------------------------
		//	SetConsoleCtrlHandler method
		//	----------------------------

		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetConsoleCtrlHandler(
			ControlEventHandler e,
			[MarshalAs(UnmanagedType.Bool)] bool add);

		//	------------------------------
		//	FlushConsoleInputBuffer method
		//	------------------------------

		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool FlushConsoleInputBuffer(SafeFileHandle hConsoleInput);

		//	-------------------
		//	GetStdHandle method
		//	-------------------

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern SafeFileHandle GetStdHandle(int nStdHandle);

	} // NativeMethods class

	#endregion // Interop

} // namespace usis.Service

// eof "ConsoleServerEngine.cs"
