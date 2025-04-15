
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

        var logger = new NLogLoggerService();
        _pluginManager = new PluginManager(logger);
        var services = new ServiceCollection().AddSingleton<ILoggerService, NLogLoggerService>();
        services.AddSingleton<IPluginManager, PluginManager>();
        services.AddSingleton<JsonPluginInfoService>();
        var provider = services.BuildServiceProvider();
        var output = new ConsoleOutput();
        var dispatcher = new ConsoleCommandDispatcher(provider, output);
        var autoComplete = new CommandAutoCompleteProvider(dispatcher);

        // Регистрируем команды
        dispatcher.Register(new LoadPluginCommand(output));
        dispatcher.Register(new ListPluginsCommand());
        dispatcher.Register(new SuggestCommand(autoComplete));
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