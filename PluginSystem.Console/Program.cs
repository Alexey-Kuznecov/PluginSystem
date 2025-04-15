﻿
using Microsoft.Extensions.DependencyInjection;
using PluginSystem.Console;
using PluginSystem.Core;
using PluginSystem.Core.PluginSystem.Core;
using PluginSystem.Hosting.ConsoleCommands;
using PluginSystem.Hosting.ConsoleCommands.Commands;
using PluginSystem.Runtime;
using System;

class Program
{
    private static PluginConsoleMenu _pluginConsoleMenu;
    private static PluginManager _pluginManager;

    static void Main(string[] args)
    {
        var pluginInfoService = new JsonPluginInfoService();
        var testInfo = new PluginInfo
        {
            SystemID = "testplugin-1.0.0-aabbccdd",
            Name = "Test Plugin",
            Version = "1.0.0",
            Author = "John Developer",
            Description = new Dictionary<string, string>
            {
                { "en", "Sample plugin for testing JSON saving." },
                { "ru", "Примерный плагин для тестирования JSON-сохранения." }
            }
        };

        // Сохранение
        pluginInfoService.Save(testInfo);
        Console.WriteLine("PluginInfo сохранён.");

        // Загрузка
        var loadedInfo = pluginInfoService.Load("testplugin-1.0.0-aabbccdd");
        if (loadedInfo != null)
        {
            Console.WriteLine($"Загружено: {loadedInfo.Name} (v{loadedInfo.Version}) автор: {loadedInfo.Author}");
        }
        else
        {
            Console.WriteLine("Не удалось загрузить PluginInfo.");
        }

        //var logger = new NLogLoggerService();
        //_pluginManager = new PluginManager(logger);
        //var services = new ServiceCollection().AddSingleton<ILoggerService, NLogLoggerService>();
        //services.AddSingleton<IPluginManager, PluginManager>();
        //var provider = services.BuildServiceProvider();
        //var output = new ConsoleOutput();
        //var dispatcher = new ConsoleCommandDispatcher(provider, output);
        //var autoComplete = new CommandAutoCompleteProvider(dispatcher);
       
        //// Регистрируем команды
        //dispatcher.Register(new LoadPluginCommand(output));
        //dispatcher.Register(new ListPluginsCommand());
        //dispatcher.Register(new SuggestCommand(autoComplete));
        //dispatcher.Register(new HelpCommand(dispatcher));
        //dispatcher.Register(new ExitCommand());

        //while (true)
        //{
        //    Console.Write("> ");
        //    var line = Console.ReadLine();
        //    if (line?.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase) == true)
        //        break;

        //    dispatcher.ExecuteCommand(line ?? "");
        //}
    }
}

    //private static PluginConsoleMenu _pluginConsoleMenu;
    //private static PluginManager _pluginManager;

    //static void Main()
    //{
    //    var logger = new NLogLoggerService();
    //    _pluginManager = new PluginManager(loggerService: logger);

    //    if (_pluginManager.LoadPlugin("FileManagerPlugin"))
    //    {
    //        _pluginConsoleMenu = new PluginConsoleMenu(
    //            _pluginManager.LoadedPlugins.Values
    //                .OrderBy(p => p.PluginInfo.Name)
    //                    .ToList());

    //        _pluginConsoleMenu.Run();
    //    }
    //}