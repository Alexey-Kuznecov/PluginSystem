using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Core.Abstractions
{
    public interface IConsoleCommandRegistry
    {
        void Register<T>(string name, T instance) where T : class, IConsoleCommand;
        IEnumerable GetRegisteredCommands();
    }
}
