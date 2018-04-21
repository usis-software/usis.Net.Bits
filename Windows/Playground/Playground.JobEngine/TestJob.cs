using System;
using usis.Framework;
using usis.Platform;

namespace Playground.JobEngine
{
    internal sealed class TestJob : ContextInjectable<IApplication>, ITestJob
    {
        public Guid Start()
        {
            return Context.RunAsJob(job =>
            {
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
            });
        }
    }
}
