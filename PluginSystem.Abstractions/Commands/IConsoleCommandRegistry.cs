using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Abstractions.Commands
{
    public interface IConsoleCommandRegistry
    {
        void Register<T>(string name, T instance) where T : class, IConsoleCommand;
        IEnumerable GetRegisteredCommands();
    }
}
