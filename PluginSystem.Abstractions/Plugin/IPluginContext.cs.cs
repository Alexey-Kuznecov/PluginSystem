
using NLog;
using PluginSystem.Abstractions.Commands;
using PluginSystem.Abstractions.Services;

namespace PluginSystem.Abstractions.Plugin
{
    public interface IPluginContext :
        IPluginServiceRegistry,
        IPluginCommandHost,
        IPluginSettingsHost,
        IPluginEventHost,
        IPluginResourceTracker
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