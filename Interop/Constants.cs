//
//  @(#) Constants.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017,2018 usis GmbH. All rights reserved.

namespace usis.Net.Bits.Interop
{
    #region Constants class

    //  ---------------
    //  Constants class
    //  ---------------

    internal static class Constants
    {
        internal const uint BG_JOB_ENUM_ALL_USERS = 1;
    }

    #endregion Constants class

    #region HResult class

    //  -------------
    //  HResult class
    //  -------------

    internal static class HResult
    {
        /// <summary>
        /// Represents the HRESULT constant <b>S_OK</b> (0x00000000):<br/><i>Operation successful</i>
        /// </summary>

        internal const int Ok = 0;

        /// <summary>
        /// Represents the HRESULT constant <b>E_FAIL</b> (0x80004005):<br/><i>Unspecified failure</i>
        /// </summary>

        internal const uint Fail = 0x80004005;

        /// <summary>
        /// Represents the HRESULT constant <b>E_ACCESSDENIED</b> (0x80070005):<br/><i>General access denied error</i>
        /// </summary>

        internal const uint E_ACCESSDENIED = 0x80070005;

        /// <summary>
        /// Represents the HRESULT constant <b>BG_E_NOT_FOUND</b> (0x80200001):<br/>
        /// <i>The requested job was not found.</i>
        /// </summary>

        internal const uint BG_E_NOT_FOUND = 0x80200001;

        /// <summary>
        /// Represents the HRESULT constant <b>BG_E_ERROR_INFORMATION_UNAVAILABLE</b> (0x8020000f).
        /// </summary>

        internal const uint BG_E_ERROR_INFORMATION_UNAVAILABLE = 0x8020000f;

        /// <summary>
        /// Represents the HRESULT constant <b>RPC_E_DISCONNECTED</b> (0x80010108):<br/>
        /// <i>The object invoked has disconnected from its clients.</i>
        /// </summary>

        internal const uint RPC_E_DISCONNECTED = 0x80010108;

        //  ----------------
        //  Succeeded method
        //  ----------------

        internal static bool Succeeded(uint result) => (result & 0x80000000) == 0;
    }

    #endregion HResult class

    #region Win32Error class

    //  ----------------
    //  Win32Error class
    //  ----------------

    internal static class Win32Error
    {
        /// <summary>
        /// The RPC server is unavailable.
        /// </summary>

        internal const uint RPC_S_SERVER_UNAVAILABLE = 0x800706ba;

        /// <summary>
        /// The resource loader cache does not have a loaded MUI entry.
        /// </summary>

        internal const uint ERROR_MUI_FILE_NOT_LOADED = 0x80073b01;

        /// <summary>
        /// The resource loader failed to find the Multilingual User Interface (MUI) file.
        /// </summary>

        internal const uint ERROR_MUI_FILE_NOT_FOUND = 0x80073afc;
    }

    #endregion Win32Error class

    #region CLSID class

    //  -----------
    //  CLSID class
    //  -----------

    internal static class CLSID
    {
        internal const string BackgroundCopyManager = "4991d34b-80a1-4291-83b6-3328366b9097";
        internal const string BackgroundCopyManager1_5 = "f087771f-d74f-4c1a-bb8a-e16aca9124ea";
        internal const string BackgroundCopyManager2_0 = "6d18ad12-bde3-4393-b311-099c346e6df9";
        internal const string BackgroundCopyManager2_5 = "03ca98d6-ff5d-49b8-abc6-03dd84127020";
        internal const string BackgroundCopyManager3_0 = "659cdea7-489e-11d9-a9cd-000d56965251";
        internal const string BackgroundCopyManager4_0 = "bb6df56b-cace-11dc-9992-0019b93a3a84";
        internal const string BackgroundCopyManager5_0 = "1ecca34c-e88a-44e3-8d6a-8921bde9e452";
        internal const string BackgroundCopyManager10_0 = "4d233817-b456-4e75-83d2-b17dec544d12";
        internal const string BackgroundCopyManager10_1 = "4bd3e4e1-7bd4-4a2b-9964-496400de5193";
    }

    #endregion CLSID class

    #region IID class

    //  ---------
    //  IID class
    //  ---------

    internal static class IID
    {
        internal const string IBackgroundCopyCallback = "97ea99c7-0186-4ad4-8df9-c5b4e0ed6b22";
        internal const string IBackgroundCopyError = "19c613a0-fcb8-4f28-81ae-897c3d078f81";
        internal const string IBackgroundCopyFile = "01b7bd23-fb88-4a77-8490-5891d3e4653a";
        internal const string IBackgroundCopyJob = "37668d37-507e-4160-9316-26306d150b12";
        internal const string IBackgroundCopyJob2 = "54b50739-686f-45eb-9dff-d6a9a0faa9af";
        internal const string IBackgroundCopyJob3 = "443c8934-90ff-48ed-bcde-26f5c7450042";
        internal const string IBackgroundCopyManager = "5ce34c0d-0dc9-4c1f-897c-daa1b78cee7c";
        internal const string IEnumBackgroundCopyFiles = "ca51e165-c365-424c-8d41-24aaa4ff3c40";
        internal const string IEnumBackgroundCopyJobs = "1af4f612-3b71-466f-8f58-7b6f73ac57ad";
    }

    #endregion IID class
}

// eof "Constants.cs"
