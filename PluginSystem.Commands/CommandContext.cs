
using PluginSystem.Core;

namespace PluginSystem.Commands
{
    public class CommandContext : ICommandContext
    {
        public IPlugin Plugin { get; set; } = null!;
        public PluginInfo Info { get; set; } = null!;
        public IPluginContext RuntimeContext { get; set; } = null!;
        public object? Target { get; set; }
        public ICommandParameters Parameters { get; set; } = new CommandParameters();
    }
}
