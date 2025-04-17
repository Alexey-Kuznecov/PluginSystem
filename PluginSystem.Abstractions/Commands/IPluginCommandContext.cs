
using PluginSystem.Abstractions.Plugin;

namespace PluginSystem.Abstractions.Commands
{
    public interface IPluginCommandContext
    {
        IPlugin Plugin { get; }
        PluginInfo Info { get; }
        IPluginContext RuntimeContext { get; }
        object? Target { get; }
        ICommandParameters Parameters { get; }
    }
}
