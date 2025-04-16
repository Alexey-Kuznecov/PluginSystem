
using NLog;
using PluginSystem.Core.Abstractions;

namespace PluginSystem.Core
{
    public interface IPluginContext :
        Abstractions.Services.IPluginServiceRegistry,
        Abstractions.Commands.IPluginCommandHost,
        Abstractions.Settings.IPluginSettingsHost,
        Abstractions.Events.IPluginEventHost,
        Abstractions.Lifecycle.IPluginResourceTracker
    {
        // Добавил это свойство, что можно было использовать id без каких-либо костылей
        string PluginId { get; }
        IConsoleCommandRegistry Commands { get; }
        IPluginSettingsService Settings { get; }
        // Добавил это свойство, что можно было использовать путь к плагину без каких-либо костылей
        string PluginDirectory { get; }
        ILogger Logger { get; }
    }
}