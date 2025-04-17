using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Abstractions.Commands
{
    public interface IConsoleCommandContext
    {
        string[] Arguments { get; }
        IConsoleOutput Output { get; }
        IServiceProvider Services { get; }
    }
}
