//
//  @(#) BackgroundCopyJobExtensions.cs
//
//  Project:    usis.Net.Bits
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2017 usis GmbH. All rights reserved.

using System;

namespace usis.Net.Bits
{
    //  ---------------------------------
    //  BackgroundCopyJobExtensions class
    //  ---------------------------------

    /// <summary>
    /// Provides extension methods to the <see cref="BackgroundCopyJob"/> class.
    /// </summary>

    public static class BackgroundCopyJobExtensions
    {
        //  ---------------------------------
        //  GetPercentBytesTransferred method
        //  ---------------------------------

        /// <summary>
        /// Gets the percentage of bytes transferred.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns>
        /// The percentage of bytes transferred.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="job" /> is a <c>null</c> reference.</exception>

        public static float GetPercentBytesTransferred(this BackgroundCopyJob job)
        {
            if (job == null) throw new ArgumentNullException(nameof(job));

            var progress = job.RetrieveProgress();
            return progress.BytesTotal == 0 ? 0.0f : 100.0f * progress.BytesTransferred / progress.BytesTotal;
        }

        //  -------------------
        //  GetErrorInfo method
        //  -------------------

        /// <summary>
        /// Gets error informations after an error occurs.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns>
        /// Informations about the last error that occured.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="job"/> is a <c>null</c> reference.</exception>

        public static BackgroundCopyErrorInfo GetErrorInfo(this BackgroundCopyJob job)
        {
            if (job == null) throw new ArgumentNullException(nameof(job));

            using (var error = job.RetrieveError(false))
            {
                return error == null ? null : new BackgroundCopyErrorInfo(error);
            }
        }
    }
}

// eof "BackgroundCopyJobExtensions.cs"
