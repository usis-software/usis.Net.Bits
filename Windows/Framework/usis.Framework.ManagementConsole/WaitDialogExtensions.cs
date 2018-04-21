//
//  @(#) WaitDialogExtensions.cs
//
//  Project:    usis.ManagementConsole
//  System:     Microsoft Visual Studio 2017
//  Author:     Udo Schäfer
//
//  Copyright (c) 2014-2017 usis GmbH. All rights reserved.

using Microsoft.ManagementConsole.Advanced;
using System;
using System.Linq;
using System.Threading.Tasks;
using usis.ManagementConsole;

namespace usis.Framework.ManagementConsole
{
    //  --------------------------
    //  WaitDialogExtensions class
    //  --------------------------

    /// <summary>
    /// Provides extension methods to the <see cref="WaitDialog"/> class.
    /// </summary>

    public static class WaitDialogExtensions
    {
        //  -----------------
        //  WaitForJob method
        //  -----------------

        /// <summary>
        /// Waits for the specified job to complete.
        /// </summary>
        /// <param name="dialog">The wait dialog.</param>
        /// <param name="getJobState">State of the job to wait for.</param>
        /// <param name="statusText">A status text to display.</param>

        public static void WaitForJob(this WaitDialog dialog, Func<JobState> getJobState, string statusText)
        {
            if (dialog == null) throw new ArgumentNullException(nameof(dialog));

            dialog.UpdateProgress(0, 100, statusText);
            Task.Run(() =>
            {
                int t = 0;
                while (getJobState() == JobState.Running)
                {
                    System.Threading.Thread.Sleep(100);
                    t = t < 100 ? t + 10 : 0;
                    dialog.WorkProcessed = t;
                }
                dialog.CompleteDialog();
            });
            dialog.ShowDialog();
        }

        /// <summary>
        /// Waits for the specified job to complete.
        /// </summary>
        /// <param name="dialog">The wait dialog.</param>
        /// <param name="getJobProgress">State of the job to wait for.</param>
        /// <param name="statusText">A status text to display.</param>

        public static void WaitForJob(this WaitDialog dialog, Microsoft.ManagementConsole.SnapIn snapIn, Func<JobProgress> getJobProgress)
        {
            if (dialog == null) throw new ArgumentNullException(nameof(dialog));

            Task.Run(() =>
            {
                dialog.TotalWork = 100;
                var message = string.Empty;
                JobProgress progress = null;
                do
                {
                    progress = getJobProgress();
                    if (progress != null)
                    {
                        if (string.IsNullOrEmpty(message)) { dialog.StatusText = progress.Message; message = progress.Message; }
                        var worked = Convert.ToInt32(1.0 * dialog.TotalWork * progress.Value / (progress.MaxValue - progress.MinValue));
                        for (int i = dialog.WorkProcessed; i <= worked; i++)
                        {
                            dialog.WorkProcessed = i;
                            System.Threading.Thread.Sleep(50);
                        }
                        if (!message.Equals(progress.Message)) { message = progress.Message; dialog.StatusText = message; }
                    }
                }
                while (progress != null && progress.State == JobState.Running);

            }).ContinueWith(task =>
            {
                dialog.CompleteDialog();
                if (task.Status == TaskStatus.Faulted)
                {
                    snapIn.Invoke(new Action(() => { snapIn.Console.ShowDialog(task.Exception.InnerExceptions.First()); }));
                }
            });
            dialog.ShowDialog();
        }
    }
}

// eof "WaitDialogExtensions.cs"
