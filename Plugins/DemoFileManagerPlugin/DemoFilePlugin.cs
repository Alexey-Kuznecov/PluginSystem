
using PluginSystem.Abstractions.Commands;
using PluginSystem.Core;
using PluginSystem.Core.PluginSystem.Core;
using PluginSystem.Hosting.ConsoleCommands;
using System.Runtime;
using PluginSystem.Abstractions.Plugin;
using PluginSystem.Abstractions.Plugin.PluginSystem.Core;

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

        // А с этим что делать?
        _settingsManager = new PluginSettings<DemoSettings>(
            context.PluginId,
            new JsonPluginSettingsService(context.PluginDirectory)
        );
        
        //context.Commands.Register<ShowSettingsCommands>("demo.settings.show", new ShowSettingsCommands()); // Это уже не нужно? 
        var module = new DemoCommandModule(context.Commands, _settingsManager); // Как получить и где взять?
        module.RegisterCommands();
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
}

