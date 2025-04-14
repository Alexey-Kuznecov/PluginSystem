using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Hosting.ConsoleCommands
{
    public class ExitCommand : IConsoleCommand
    {
        public string Name => "exit";
        public string Description => "Завершает приложение.";

        public void Execute(CommandContext context)
        {
            Console.WriteLine("Выход...");
            Environment.Exit(0);
        }
    }
}
