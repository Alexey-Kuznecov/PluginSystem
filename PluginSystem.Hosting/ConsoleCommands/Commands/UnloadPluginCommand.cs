using Microsoft.Extensions.DependencyInjection;
using PluginSystem.Core.PluginSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSystem.Abstractions.Commands;

namespace PluginSystem.Hosting.ConsoleCommands.Commands
{
    public class UnloadPluginCommand : IConsoleCommand
    {
        private IPluginManager _pluginManager;

        public UnloadPluginCommand()
        {
        }

        public string Name => "unload";
        public string Description => "Выгружает плагин по его ID";

        public void Execute(IConsoleCommandContext context)
        {
            _pluginManager = context.Services.GetService<IPluginManager>();
            var pluginId = context.Arguments.ElementAtOrDefault(0);
            if (string.IsNullOrWhiteSpace(pluginId))
            {
                context.Output.WriteLine("Укажите ID плагина для выгрузки.");
                return;
            }

            if (_pluginManager.UnloadPlugin(pluginId))
            {
                context.Output.WriteLine($"Плагин {pluginId} выгружен.");
            }
            else
                context.Output.WriteLine($"Плагин {pluginId} не найден или не может быть выгружен.");
        }

        public IEnumerable<string> GetSuggestions(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
