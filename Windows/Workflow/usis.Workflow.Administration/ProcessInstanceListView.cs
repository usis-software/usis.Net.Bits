using Microsoft.ManagementConsole;

namespace usis.Workflow.Administration
{
    internal sealed class ProcessInstanceListNode : ScopeNode
    {
        internal ProcessInstanceListNode() : base(true)
        {
            DisplayName = Strings.ProcessInstances;
            ViewDescriptions.Add(ProcessInstanceListView.Description);
        }
    }

    internal static class ProcessInstanceListView
    {
        internal static ViewDescription Description
        {
            get
            {
                return new MmcListViewDescription()
                {
                    DisplayName = Strings.ProcessInstances
                };
            }
        }
    }
}
