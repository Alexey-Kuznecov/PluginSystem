using Microsoft.Extensions.DependencyInjection;
using PluginSystem.Abstractions.Commands;
using PluginSystem.Core;
using PluginSystem.Core.PluginSystem.Core;
using PluginSystem.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PluginSystem.Hosting.ConsoleCommands.Commands
{
    public class LoadPluginCommand : IConsoleCommand
    {
        private IPluginManager _pluginManager;
        private readonly IConsoleOutput _output;

        public string Name => "load";
        public string Description => "Загружает плагин из указанного пути или загружает все плагины с ключом --all.";

        public LoadPluginCommand(IConsoleOutput output)
        {
            _output = output;
        }

        public void Execute(IConsoleCommandContext context)
        {
            _pluginManager = context.Services.GetService<IPluginManager>();
            var args = context.Arguments;
            if (args.Contains("--all", StringComparer.OrdinalIgnoreCase))
            {
                var loaded = _pluginManager.LoadAllPlugins();
                context.Output.WriteLine($"Загружено плагинов: {loaded.Count()}");
                return;
            }

            var pathArg = args.FirstOrDefault(arg => arg.EndsWith(".dll", StringComparison.OrdinalIgnoreCase));
            if (pathArg == null)
            {
                context.Output.WriteError("Укажите путь к .dll файлу плагина.");
                return;
            }

            var isRelative = args.Contains("--p");
            var path = isRelative ? Path.Combine(AppContext.BaseDirectory, pathArg) : pathArg;

            if (!File.Exists(path))
            {
                context.Output.WriteError($"Файл \"{path}\" не найден.");
                return;
            }

            var result = _pluginManager.LoadPlugin(path);
            if (result)
                context.Output.WriteLine($"Плагин успешно загружен: {Path.GetFileName(path)}");
            else
                context.Output.WriteError("Не удалось загрузить плагин.");
        }

        public IEnumerable<string> GetSuggestions(string[] args)
        {
            var suggestions = new List<string> { "--all", "--p" };
            return suggestions.Where(s => args.Length == 0 || s.StartsWith(args.Last(), StringComparison.OrdinalIgnoreCase));
        }
    }
}

