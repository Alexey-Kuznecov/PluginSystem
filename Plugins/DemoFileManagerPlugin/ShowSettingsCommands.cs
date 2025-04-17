using PluginSystem.Abstractions.Commands;
using PluginSystem.Abstractions.Settings;
using PluginSystem.Core;
using PluginSystem.Hosting.ConsoleCommands;

namespace DemoFileManagerPlugin;

public class ShowSettingsCommands : IConsoleCommand
{
    private IPluginSettings<DemoSettings> _settings;

    public string Name => throw new NotImplementedException();

    public string Description => throw new NotImplementedException();

    public ShowSettingsCommands(IPluginSettings<DemoSettings> settings)
    {
        _settings = settings;
    }

    public void Execute(IConsoleCommandContext context)
    {
        var s = _settings?.Value;
        if (s == null)
        {
            context?.Output.WriteLine("Settings not loaded.");
            return;
        }

        context?.Output.WriteLine($"ShowHiddenFiles: {s.ShowHiddenFiles}");
        context?.Output.WriteLine($"SortMode: {s.SortMode}");
    }

    public IEnumerable<string> GetSuggestions(string[] args)
    {
        throw new NotImplementedException();
    }
}