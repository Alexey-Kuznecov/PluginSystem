using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Hosting.ConsoleCommands
{
    public interface IConsoleCommandDispatcher
    {
        void Register(IConsoleCommand command);
        bool ExecuteCommand(string commandLine);
        IEnumerable<IConsoleCommand> GetAvailableCommands();
    }
}
