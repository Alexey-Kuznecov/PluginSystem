
using PluginSystem.Console;
using PluginSystem.Runtime;

class Program
{
    private static PluginConsoleMenu _pluginConsoleMenu;
    private static PluginManager _pluginManager;

    static void Main()
    {
        var logger = new NLogLoggerService();
        _pluginManager = new PluginManager(loggerService: logger);

        if (_pluginManager.LoadPlugin("FileManagerPlugin"))
        {
            _pluginConsoleMenu = new PluginConsoleMenu(
                _pluginManager.LoadedPlugins.Values
                    .OrderBy(p => p.PluginInfo.Name)
                        .ToList());

            _pluginConsoleMenu.Run();
        }
    }
}
