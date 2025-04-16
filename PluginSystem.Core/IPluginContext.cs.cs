
using NLog;

namespace PluginSystem.Core
{
    public interface IPluginContext :
        Abstractions.Services.IPluginServiceRegistry,
        Abstractions.Commands.IPluginCommandHost,
        Abstractions.Settings.IPluginSettingsHost,
        Abstractions.Events.IPluginEventHost,
        Abstractions.Lifecycle.IPluginResourceTracker
    {
        ILogger Logger { get; }
    }
}