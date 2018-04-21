using System;
using System.Diagnostics.CodeAnalysis;
using usis.Framework;

namespace usis.JobEngine
{
    internal class TestJob : JobSnapIn
    {
        public TestJob() : base("test") { }

        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "usis.Platform.IProgressUpdate.UpdateProgress(System.Int64,System.Int64,System.Int64,System.String)")]
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object)")]
        protected override void Run(IJob job)
        {
            if (job == null) throw new ArgumentNullException(nameof(job));
            job.RemoveWhenCompleted = true;

            for (int step = 1; step <= 10; step++)
            {
                job.SetProgressStep(step, 10, $"Step {step} of 10");
                for (int i = 1; i <= 100; i++)
                {
                    job.UpdateProgress(1, 100, i, $"{i}% complete");
                    System.Threading.Thread.Sleep(100);
                    if (job.Token.IsCancellationRequested) return;
                }
            }
        }
    }
}
