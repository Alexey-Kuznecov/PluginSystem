
using PluginSystem.Abstractions.Plugin;

namespace PluginSystem.Abstractions.Commands
{
    public interface ICommandProvider
    {
        /// <summary>
        /// Получает все доступные команды для выполнения в контексте плагина.
        /// </summary>
        /// <returns>Список команд, предоставленных плагином.</returns>
        IEnumerable<IPluginCommand> GetCommands();
    }
}
