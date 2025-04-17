using PluginSystem.Abstractions.Commands;
using PluginSystem.Hosting.ConsoleCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Hosting
{
    public static class ConsoleCommandRegistryExtensions
    {
        public static void RegisterLambda(
            this IConsoleCommandRegistry registry,
            string name,
            string description,
            Action<ConsoleCommandContext, IConsoleOutput> handler,
            IEnumerable<string>? aliases = null)
        {
            var command = new DelegateConsoleCommand(name, description, handler, aliases);
            registry.Register(name, command);
        }
    }
}
