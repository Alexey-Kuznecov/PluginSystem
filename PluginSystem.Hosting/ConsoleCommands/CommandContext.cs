using PluginSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Hosting.ConsoleCommands
{
    public class CommandContext
    {
        public string[] Arguments { get; }
        public IServiceProvider Services { get; } // или IReadOnlyDictionary<string, object>
        public IConsoleOutput Output { get; }     // для вывода результата
        // public IPlugin? SourcePlugin { get; }     // команда может знать кто её вызвал (опционально)

        public CommandContext(IServiceProvider services, IConsoleOutput output, string[] args)//, IServiceProvider services, IConsoleOutput output, IPlugin? sourcePlugin = null)
        {
            Arguments = args;
            Services = services;
            Output = output;
            //SourcePlugin = sourcePlugin;
        }
    }
}
