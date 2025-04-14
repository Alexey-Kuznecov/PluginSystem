
using Microsoft.Extensions.DependencyInjection;
using PluginSystem.Console;
using PluginSystem.Core;
using PluginSystem.Core.PluginSystem.Core;
using PluginSystem.Hosting.ConsoleCommands;
using PluginSystem.Runtime;
using System;

class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection().AddSingleton<ILoggerService, NLogLoggerService>().BuildServiceProvider();

        var output = new ConsoleOutput();
        var dispatcher = new ConsoleCommandDispatcher(services, output);

        // Регистрируем команды
        dispatcher.Register(new HelpCommand(dispatcher));
        dispatcher.Register(new ExitCommand());

        while (true)
        {
            Console.Write("> ");
            var line = Console.ReadLine();
            if (line?.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase) == true)
                break;

            dispatcher.ExecuteCommand(line ?? "");
        }
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