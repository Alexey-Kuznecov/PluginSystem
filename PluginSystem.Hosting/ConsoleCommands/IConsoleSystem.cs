using PluginSystem.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Hosting.ConsoleCommands
{
    public interface IConsoleSystem
    {
        IConsoleCommandRegistry Registry { get; }
        IConsoleCommandDispatcher Dispatcher { get; }
        void RegisterBuiltInCommands();
    }
}
