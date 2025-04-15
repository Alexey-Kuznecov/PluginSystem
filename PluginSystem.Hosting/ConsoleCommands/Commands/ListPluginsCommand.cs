using PluginSystem.Core.PluginSystem.Core;
using Microsoft.Extensions.DependencyInjection;

namespace PluginSystem.Hosting.ConsoleCommands.Commands
{
    public class ListPluginsCommand : IConsoleCommand //, IAutoCompleteProvider
    {
        public string Name => "list";
        public string Description => "Показать список плагинов.";

        public void Execute(CommandContext context)
        {
            bool verbose = context.Arguments.Contains("--verbose");
            bool json = context.Arguments.Contains("--json");

            var manager = context.Services.GetService<IPluginManager>();
            if (manager == null)
            {
                context.Output.WriteError("Не удалось получить менеджер плагинов.");
                return;
            }

            var plugins = manager.GetAllPlugins();
            if (!plugins.Any())
            {
                context.Output.WriteLine("Список плагинов пуст.");
                return;
            }

            if (json)
            {
                var jsonText = System.Text.Json.JsonSerializer.Serialize(plugins.Select(p => new
                {
                    p.Plugin.Name,
                    p.Plugin.Version
                }), new System.Text.Json.JsonSerializerOptions { WriteIndented = true });

                context.Output.WriteLine(jsonText);
                return;
            }

            foreach (var container in plugins)
            {
                if (verbose)
                {
                    context.Output.WriteLine($"Плагин: {container.Plugin.Name} Версия: {container.Plugin.Version} ID: {container.PluginInfo.SystemID}");
                }
                else
                {
                    context.Output.WriteLine($"{container.Plugin.Name} ({container.Plugin.Version})");
                }
            }

            foreach (var container in plugins)
                context.Output.WriteLine($"{container.Plugin.Name} ({container.Plugin.Version})");
        }

        public IEnumerable<string> GetSuggestions(string[] args)
        {
            var argumentLevels = new List<string[]>
                {
                    new[] { "plugins", "commands" }, // 1-й уровень
                    new[] { "--verbose", "--json" }  // 2-й уровень
                };
            return AutoCompleteHelper.Suggest(argumentLevels, args);
        }
    }
}
