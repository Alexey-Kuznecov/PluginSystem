using PluginSystem.Core.PluginSystem.Core;
using Microsoft.Extensions.DependencyInjection;
using PluginSystem.Core;
using System.Text.Json;
using PluginSystem.Runtime;

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
                var infoService = context.Services.GetService<PluginPersistenceService>();
                if (infoService == null)
                {
                    context.Output.WriteError("Сервис JsonPluginInfoService не найден.");
                    return;
                }

                var pluginInfos = new List<PluginInfo>();

                foreach (var container in plugins)
                {
                    try
                    {
                        pluginInfos.Add(container.PluginInfo);
                    }
                    catch (Exception ex)
                    {
                        context.Output.WriteError($"Ошибка при получении информации о плагине '{container.Plugin.Name}': {ex.Message}");
                    }
                }

                var jsonText = JsonSerializer.Serialize(pluginInfos, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                context.Output.WriteLine(jsonText);
                return;
            }

            foreach (var container in plugins)
            {
                try
                {
                    var name = container.Plugin.Name;
                    var version = container.Plugin.Version;
                    var id = container.PluginInfo.SystemID;

                    if (verbose)
                        context.Output.WriteLine($"Плагин: {name} Версия: {version} ID: {id}");
                    else
                        context.Output.WriteLine($"{name} ({version})");
                }
                catch (Exception ex)
                {
                    context.Output.WriteError($"Ошибка при выводе информации о плагине: {ex.Message}");
                }
            }
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
