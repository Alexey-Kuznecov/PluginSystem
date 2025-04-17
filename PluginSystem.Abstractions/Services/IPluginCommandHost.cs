using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSystem.Abstractions.Commands;
using PluginSystem.Abstractions.Plugin;


namespace PluginSystem.Abstractions.Services;

public interface IPluginCommandHost
{
    List<IPluginCommand> Commands { get; }
    void ExecuteCommand(IPluginCommand command, IPluginCommandContext context);
}
