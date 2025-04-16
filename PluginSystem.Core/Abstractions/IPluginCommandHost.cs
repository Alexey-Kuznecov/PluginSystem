using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSystem.Core;

namespace PluginSystem.Core.Abstractions.Commands;

public interface IPluginCommandHost
{
    List<IPluginCommand> Commands { get; }
    void ExecuteCommand(IPluginCommand command, ICommandContext context);
}
