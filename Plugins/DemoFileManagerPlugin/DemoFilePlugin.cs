
using PluginSystem.Core;
using PluginSystem.Core.PluginSystem.Core;
using PluginSystem.Hosting.ConsoleCommands;
using System.Runtime;

namespace DemoFileManagerPlugin;

public sealed class DemoFilePlugin : IPlugin, IPluginUnloadable
{
    private IPluginContext? _context;
    public string Name => "DemoFileManager";
    public string Version => "1.0.0";

    private PluginSettings<DemoSettings>? _settingsManager;

    public void Initialize(IPluginContext context)
    {
        _context = context;
        _settingsManager = new PluginSettings<DemoSettings>(
            context.PluginId,
            new JsonPluginSettingsService(context.PluginDirectory)
        );
        context.Commands.Register("demo.settings.show", ShowSettings);
        context.Commands.Register("demo.settings.update", UpdateSetting);
        // Пример изменения
        _settingsManager.Update(s => s.ShowHiddenFiles = true);
        _context.Logger.Info($"[{Name}] Настройки загружены. Копировать в: {_settingsManager}");

        // Дальше регистрируешь команды, интерфейсы и т.д.
    }

    public void OnUnload()
    {
        if (_context is not null)
        {
            _context.Logger.Info($"[{Name}] Сохраняем настройки перед выгрузкой...");
            _settingsManager?.Save();

            _context.Logger.Info($"[{Name}] Плагин выгружается.");
            _context.Cleanup();
        }
        _settingsManager?.Save();        // Явно сохраняем
        _settingsManager?.Dispose();     // Отписываемся
        _settingsManager = null;

        _context = null;
    }

    public void Shutdown()
    {
        // Пока не используется, можно добавить при необходимости
    }

    private void ShowSettings(CommandContext ctx, IConsoleOutput output)
    {
        var s = _settingsManager?.Value;
        if (s == null)
        {
            output.WriteLine("Settings not loaded.");
            return;
        }

        output.WriteLine($"ShowHiddenFiles: {s.ShowHiddenFiles}");
        output.WriteLine($"SortMode: {s.SortMode}");
    }

    private void UpdateSetting(CommandContext ctx, IConsoleOutput output)
    {
        if (ctx.Arguments.Length < 2)
        {
            output.WriteLine("Usage: demo.settings.update <property> <value>");
            return;
        }

        var prop = ctx.Arguments[0].ToLowerInvariant();
        var val = ctx.Arguments[1];

        _settingsManager?.Update(s =>
        {
            switch (prop)
            {
                case "showhiddenfiles":
                    if (bool.TryParse(val, out var b))
                        s.ShowHiddenFiles = b;
                    else
                        output.WriteLine("Invalid boolean value.");
                    break;

                case "sortmode":
                    s.SortMode = val;
                    break;

                default:
                    output.WriteLine($"Unknown setting: {prop}");
                    break;
            }
        });

        output.WriteLine("Setting updated.");
    }
}

