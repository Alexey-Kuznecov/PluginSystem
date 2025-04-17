using PluginSystem.Abstractions.Commands;

namespace PluginSystem.Hosting.ConsoleCommands
{
    public interface IConsoleSystem
    {
        IConsoleCommandRegistry Registry { get; }
        IConsoleCommandDispatcher Dispatcher { get; }
        void RegisterBuiltInCommands();
    }
}
