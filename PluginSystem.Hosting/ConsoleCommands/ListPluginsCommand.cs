using PluginSystem.Core.PluginSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Hosting.ConsoleCommands
{
    public class ListPluginsCommand : IConsoleCommand
    {
        private readonly IPluginManager _pluginManager;

        public ListPluginsCommand(IPluginManager pluginManager)
        {
            _pluginManager = pluginManager;
        }

        public string Name => "plugins";

        public string Description => "Загружает список плагинов из папки плагинов.";

        public void Execute(CommandContext context)
        {
            foreach (var plugin in _pluginManager.GetAllPlugins())
                context.Output.WriteLine($"- {plugin.PluginInfo.Name}");
        }
    }
}
