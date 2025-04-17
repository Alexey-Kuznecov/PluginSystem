
using PluginSystem.Abstractions.Commands;
using PluginSystem.Abstractions.Plugin;

namespace PluginSystem.Console
{
    using PluginSystem.Commands;
    using PluginSystem.Core;
    using PluginSystem.Runtime;
    using System;
    using System.Collections.Generic;

    public class PluginConsoleMenu
    {
        private readonly List<PluginContainer> _plugins;

        public PluginConsoleMenu(List<PluginContainer> plugins)
        {
            _plugins = plugins;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Холст ==== Плагин Менеджер ====");
                Console.WriteLine("1. Загрузить плагины");
                Console.WriteLine("2. Показать загруженные плагины");
                Console.WriteLine("3. Выгрузить все плагины");
                Console.WriteLine("4. Выход");

                Console.Write("\nВыбор: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        LoadPlugins();
                        break;
                    case "2":
                        ShowLoadedPlugins();
                        break;
                    case "3":
                        UnloadPlugins();
                        break;
                    case "4":
                        return;
                    default:
                        break;
                }
            }
        }

        private void LoadPlugins()
        {
            Console.WriteLine("[Загрузка плагинов]");
            // Логика загрузки плагинов, возможно загрузка через менеджер плагинов
            Console.ReadKey();
        }

        private void UnloadPlugins()
        {
            Console.WriteLine("[Выгрузка всех плагинов]");
            // Логика выгрузки плагинов
            Console.ReadKey();
        }

        private void ShowLoadedPlugins()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("2. Показать загруженные плагины");

                for (int i = 0; i < _plugins.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {_plugins[i].PluginInfo.Name}");
                }
                Console.WriteLine("0. Назад");
                Console.Write("\nВыберите плагин: ");

                var input = Console.ReadLine();
                if (input == "0") break;

                if (int.TryParse(input, out int index) && index > 0 && index <= _plugins.Count)
                {
                    ShowPluginDetails(_plugins[index - 1]);
                }
            }
        }

        private void ShowPluginDetails(PluginContainer container)
        {
            while (true)
            {
                var info = container.PluginInfo;
                Console.Clear();
                Console.WriteLine($"{info.Name}");
                Console.WriteLine("1. Выполнить команду");
                Console.WriteLine("2. Краткое описание");
                Console.WriteLine("3. Документация");
                Console.WriteLine("4. История изменений");
                Console.WriteLine("5. Инструкция");
                Console.WriteLine("6. Назад");
                Console.Write("\nВыбор: ");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        ShowCommands(container);
                        break;
                    case "2":
                        Console.WriteLine($"Описание: {info.Description}");
                        break;
                    case "3":
                        Console.WriteLine($"Документация: {info.DocumentationPath}");
                        break;
                    case "4":
                        Console.WriteLine($"История изменений: {info.ChangelogUrl}");
                        break;
                    case "5":
                        Console.WriteLine($"Инструкция: {info.GetInstructions("ru")}");
                        break;
                    case "6":
                        return;
                }

                Console.WriteLine("\nНажмите любую клавишу...");
                Console.ReadKey();
            }
        }

        private void ShowCommands(PluginContainer container)
        {
            var commands = container.GetCommands();

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Команды плагина: {container.PluginInfo.Name}\n");

                for (int i = 0; i < commands.Count; i++)
                {
                    var cmd = commands[i];
                    Console.WriteLine($"{i + 1}. {cmd.Name} — {cmd.Description}");
                }

                Console.WriteLine("0. Назад");
                Console.Write("\nВыберите команду: ");

                var input = Console.ReadLine();
                if (input == "0") break;

                if (int.TryParse(input, out int index) && index > 0 && index <= commands.Count)
                {
                    if (container.Plugin is IPluginMenuProvider)
                    {
                        ShowMenus(null);
                    }
                    var command = commands[index - 1];
                    ExecuteCommand(container, command);
                }
            }
        }

        public void ShowMenus(IEnumerable<IPlugin> plugins)
        {
            foreach (var plugin in plugins)
            {
                if (plugin is IPluginMenuProvider menuProvider)
                {
                    var menu = menuProvider.GetMenu();
                    Console.WriteLine($"\n=== {menu.Title} ===");

                    for (int i = 0; i < menu.Items.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {menu.Items[i].Label}");
                    }

                    Console.Write("Выберите пункт меню: ");
                    if (int.TryParse(Console.ReadLine(), out int choice) &&
                        choice > 0 && choice <= menu.Items.Count)
                    {
                        Console.WriteLine();
                        menu.Items[choice - 1].Action();
                    }
                }
            }
        }

        private void ExecuteCommand(PluginContainer container, IPluginCommand command)
        {
            Console.Clear();
            Console.WriteLine($"▶ Выполнение команды: {command.Name}");

            // Если нужны параметры — спросим
            var context = new CommandContext
            {
                Plugin = container.Plugin,
                Info = container.PluginInfo,
                RuntimeContext = container.Context,
                Parameters = PromptParameters(command) // попросим пользователя ввести параметры
            };

            try
            {
                command.Execute(context);
                Console.WriteLine("✅ Команда выполнена.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private ICommandParameters PromptParameters(IPluginCommand command)
        {
            var parameters = new CommandParameters();

            // Если у команды есть ожидаемые параметры, мы запрашиваем их
            foreach (var param in command.ExpectedParameters)
            {
                Console.Write($"{param}: ");
                var value = Console.ReadLine();
                parameters.Set(param, value); // Устанавливаем введённые параметры
            }

            return parameters;
        }
    }
}
