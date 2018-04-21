//
//  @(#) ConsoleServicesHost.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using System.Diagnostics;

namespace usis.Platform.Windows
{
    #region ConsoleServicesHost class

    //  -------------------------
    //  ConsoleServicesHost class
    //  -------------------------

    internal class ConsoleServicesHost : ServicesHostBase, IServicesHost
    {
        #region private fields

        private ManualResetEvent stopping = new ManualResetEvent(false);
        private ManualResetEvent stopped = new ManualResetEvent(false);

        #endregion private fields

        #region construction

        //  ------------
        //  construction
        //  ------------

        internal ConsoleServicesHost(params IService[] services) : base(services) { }

        #endregion construction

        #region Run method

        //  ----------
        //  Run method
        //  ----------

        int IServicesHost.Run()
        {
            NativeMethods.SetConsoleCtrlHandler(null, true);

            if (StartServices() > 0)
            {
                // handle Win32 console events
                NativeMethods.SetConsoleCtrlHandler(null, false);
                ControlEventHandler handler = new ControlEventHandler(OnControlEvent);
                NativeMethods.SetConsoleCtrlHandler(handler, true);

                // display CTRL+C message
                Console.WriteLine(Strings.PressCtrlC);
                Console.WriteLine();
                stopping.WaitOne();

                // shutdown services
                Terminate();
                stopped.Set();

                NativeMethods.SetConsoleCtrlHandler(handler, false);
                ConsoleExtension.FlushInputBuffer();

                return 0;
            }
            else Console.WriteLine(Strings.NoServicesConfigured);

            NativeMethods.SetConsoleCtrlHandler(null, false);

            return -1;
        }

        #endregion Run method

        #region private methods

        #region StartServices method

        //  --------------------
        //  StartServices method
        //  --------------------

        private int StartServices()
        {
            Parallel.ForEach(Services, s => s.Start());
            return Services.Count();
        }

        #endregion StartServices method

        #region StopServices method

        //  --------------------
        //  StopServices metthod
        //  --------------------

        private void StopServices()
        {
            Parallel.ForEach(Services, s => s.Stop());
        }

        #endregion StopServices method

        #region OnControlEvent method

        //  ---------------------
        //  OnControlEvent method
        //  ---------------------

        private bool OnControlEvent(ConsoleEvent consoleEvent)
        {
            Debug.Print("Console event: {0}", consoleEvent);

            stopping.Set();
            stopped.WaitOne();

            return true; // do not call default handler
        }

        #endregion OnControlEvent method

        #region Terminate method

        //  ----------------
        //  Terminate method
        //  ----------------

        private void Terminate()
        {
            StopServices();

            Console.WriteLine(Strings.ServerStopped);
            Console.Beep();
        }

        #endregion Terminate method

        #endregion private methods

        #region IDisposable implementation

        //  --------------
        //  Dispose method
        //  --------------

        void IDisposable.Dispose()
        {
            stopping.Dispose();
            stopped.Dispose();
        }

        #endregion IDisposable implementation
    }

    #endregion ConsoleServicesHost class

    #region ConsoleExtension class

    //  ----------------------
    //  ConsoleExtension class
    //  ----------------------

    internal static class ConsoleExtension
    {
        //  -----------------------
        //  FlushInputBuffer method
        //  -----------------------

        internal static void FlushInputBuffer()
        {
            SafeFileHandle handle = NativeMethods.GetStdHandle(NativeMethods.STD_INPUT_HANDLE);
            if (!handle.IsInvalid)
            {
                NativeMethods.FlushConsoleInputBuffer(handle);
            }
        }
    }

    #endregion ConsoleExtension class

    #region Interop

    #region ConsoleEvent enumeration

    //  ------------------------
    //  ConsoleEvent enumeration
    //  ------------------------

    internal enum ConsoleEvent
    {
        CTRL_C = 0,
        CTRL_BREAK = 1,
        CTRL_CLOSE = 2,
        CTRL_LOGOFF = 5,
        CTRL_SHUTDOWN = 6
    }

    #endregion ConsoleEvent enumeration

    #region ControlEventHandler delegate

    //  ----------------------------
    //  ControlEventHandler delegate
    //  ----------------------------

    internal delegate bool ControlEventHandler(ConsoleEvent consoleEvent);

    #endregion ControlEventHandler delegate

    #region NativeMethods class

    //  -------------------
    //  NativeMethods class
    //  -------------------

    internal static class NativeMethods
    {
        #region constants

        internal const int STD_INPUT_HANDLE = -10;

        #endregion constants

        //  ----------------------------
        //  SetConsoleCtrlHandler method
        //  ----------------------------

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetConsoleCtrlHandler(ControlEventHandler e, [MarshalAs(UnmanagedType.Bool)] bool add);

        //  ------------------------------
        //  FlushConsoleInputBuffer method
        //  ------------------------------

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool FlushConsoleInputBuffer(SafeFileHandle hConsoleInput);

        //  -------------------
        //  GetStdHandle method
        //  -------------------

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern SafeFileHandle GetStdHandle(int nStdHandle);
    }

    #endregion NativeMethods class

    #endregion Interop
}

// eof "ConsoleServicesHost.cs"
