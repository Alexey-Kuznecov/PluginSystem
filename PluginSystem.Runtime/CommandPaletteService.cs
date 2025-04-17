
using PluginSystem.Abstractions.Plugin;
using PluginSystem.Core;

namespace PluginSystem.Runtime
{
    public class CommandPaletteService
    {
        private readonly Dictionary<string, IPluginCommand> _commands = new();

        public void Register(IPluginCommand command)
        {
            _commands[command.Id] = command;
        }

        public IEnumerable<IPluginCommand> GetAll() => _commands.Values;

        public IPluginCommand? FindById(string id)
        {
            return _commands.TryGetValue(id, out var cmd) ? cmd : null;
        }
    }
}
