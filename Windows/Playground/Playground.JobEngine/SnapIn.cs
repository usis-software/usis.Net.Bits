using usis.Framework.ServiceModel;

namespace Playground.JobEngine
{
    internal class SnapIn : NamedPipeServiceHostSnapIn<TestJob, ITestJob> { }
}
