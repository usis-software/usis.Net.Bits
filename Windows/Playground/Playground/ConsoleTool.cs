using System;

namespace Playground
{
    internal static class ConsoleTool
    {
        internal static void PressAnyKey()
        {
            Console.WriteLine();
            Console.Write("Press any key to continue ... ");
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }
}
