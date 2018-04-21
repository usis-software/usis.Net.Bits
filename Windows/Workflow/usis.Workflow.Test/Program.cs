using System;

namespace usis.Workflow.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var session = new Session())
            {
                foreach (var processDefinition in session.QueryProcessDefinitions(null))
                {
                    Console.WriteLine(processDefinition.Name);
                }
            }
            PressAnyKey();
        }

        internal static void PressAnyKey()
        {
            Console.WriteLine();
            Console.Write("Press any key to continue ... ");
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }
}
