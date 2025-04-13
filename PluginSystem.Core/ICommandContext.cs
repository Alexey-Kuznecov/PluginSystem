
namespace PluginSystem.Core
{
    public interface ICommandContext
    {
        IPlugin Plugin { get; }
        PluginInfo Info { get; }
        IPluginContext RuntimeContext { get; }
        object? Target { get; }
        ICommandParameters Parameters { get; }
    }
}
