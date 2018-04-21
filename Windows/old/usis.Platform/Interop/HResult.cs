//
//  @(#) HResult.cs
//
//  Project:    usis Platform
//  System:     Microsoft Visual Studio 2015
//  Author:     Udo Schäfer
//
//  Copyright (c) 2008-2016 usis GmbH. All rights reserved.

namespace usis.Platform.Interop
{
    //  -------------
    //  HResult class
    //  -------------

    /// <summary>
    /// Contains HRESULT constants that are used in COM and Win32
    /// to describe an error or warning.
    /// </summary>

    internal static class HResult
    {
        #region public constants

        /// <summary>
        /// Represents the HRESULT constant <b>S_OK</b> (0x00000000):<br/>
        /// <i>The method succeeded.
        /// If a boolean return value is expected,
        /// the returned value is <b>true</b>.</i>
        /// </summary>

        public const int Ok = 0;

        /// <summary>
        /// Represents the HRESULT constant <b>S_FALSE</b> (0x00000001):<br/>
        /// <i>The method succeeded and if a boolean return value is expected,
        /// the returned value is <b>false</b>.</i>
        /// </summary>

        public const int False = 1;

        /// <summary>
        /// Represents the HRESULT representation of the
        /// system error code <b>ERROR_FILE_NOT_FOUND</b> (0x80070002):<br/>
        /// <i>The system cannot find the file specified.</i>
        /// </summary>

        public const int FileNotFound = unchecked((int)0x80070002);

        /// <summary>
        /// Represents the HRESULT constant <b>STG_E_FILENOTFOUND</b> (0x80030002):<br/>
        /// <i>The specified file does not exist.</i>
        /// </summary>

        public const int StorageFileNotFound = unchecked((int)0x80030002);

        /// <summary>
        /// Represents the HRESULT constant <b>STG_E_FILEALREADYEXISTS</b> (0x80030050):<br/>
        /// <i>The file exists but is not a storage object.</i>
        /// </summary>

        public const int StorageFileAlreadyExists = unchecked((int)0x80030050);

        /// <summary>
        /// Represents the HRESULT constant <b>STG_E_INVALIDNAME</b> (0x800300FC):<br/>
        /// <i>The name not valid.</i>
        /// </summary>

        public const int StorageInvalidName = unchecked((int)0x800300FC);

        /// <summary>
        /// Represents the HRESULT constant <b>REGDB_E_CLASSNOTREG</b> (0x80040154):<br/>
        /// <i>The class identifier for the ProgID cannot be found in the registry.</i>
        /// </summary>

        public const int RegistryClassNotRegistered = unchecked((int)0x80040154);

        /// <summary>
        /// Represents the HRESULT constant <b>E_NOINTERFACE</b> (0x80004002).
        /// <i>The <b>QueryInterface</b> method did not recognize the requested interface.
        /// The interface is not supported.</i>
        /// </summary>

        public const int NoInterface = unchecked((int)0x80004002);

        /// <summary>
        /// Represents the HRESULT constant <b>CO_E_SERVER_EXEC_FAILURE</b> (0x80080005).
        /// </summary>

        public const int ComServerExecutionFailed = unchecked((int)0x80080005);

        /// <summary>
        /// Represents the HRESULT constant <b>OLE_E_ADVISENOTSUPPORTED</b> (0x80040003).
        /// </summary>

        public const int OleAdviseNotSupported = unchecked((int)0x80040003);

        /// <summary>
        /// Represents the HRESULT constant <b>OLE_E_NOCONNECTION</b> (0x80040004).
        /// </summary>

        public const int OleNoConnection = unchecked((int)0x80040004);

        /// <summary>
        /// Represents the HRESULT constant <b>CONNECT_E_NOCONNECTION</b> (0x80040200):<br/>
        /// <i>There is no connection for this connection id.</i>
        /// </summary>

        public const int ConnectNoConnection = unchecked((int)0x80040200);

        /// <summary>
        /// Represents the HRESULT constant <b>E_INVALIDARG</b> (0x00070057):<br/>
        /// <i>One or more arguments are not valid.</i>
        /// </summary>

        public const int InvalidArgument = unchecked((int)0x80070057);

        #endregion public constants

        #region public methods

        //  ----------------
        //  Succeeded method
        //  ----------------

        /// <summary>
        /// Provides a generic test for success on any status value.
        /// Non-negative numbers indicate success.
        /// </summary>
        /// <param name="result">
        /// The status value to test.
        /// </param>
        /// <returns>
        /// <b>true</b> when the operation succeeded; otherwise <b>false</b>.
        /// </returns>

        public static bool Succeeded(int result)
        {
            return result >= 0;
        }

        #endregion public methods
    }
}

// eof "HResult.cs"
