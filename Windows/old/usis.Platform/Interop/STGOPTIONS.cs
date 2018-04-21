//
//  @(#) STGOPTIONS.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2008-2016 usis GmbH. All rights reserved.

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace usis.Platform.Interop
{
    //  ----------------
    //  STGOPTIONS class
    //  ----------------

    [StructLayout(LayoutKind.Sequential)]
    internal class STGOPTIONS
    {
        public ushort usVersion;
        public ushort reserved;
        public uint ulSectorSize;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pwcsTemplateFile;

        public STGOPTIONS(ushort version)
        {
            Debug.Assert(version == 1 || version == 2);

            usVersion = version;
            ulSectorSize = 512;
        }
    }
}

// eof "STGOPTIONS.cs"
