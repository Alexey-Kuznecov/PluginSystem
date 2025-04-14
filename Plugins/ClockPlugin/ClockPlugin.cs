
using PluginSystem.Core;

namespace ClockPlugin
{
    public class ClockPlugin : IPlugin, ICommandProvider
    {
        public string Name => "ClockPlugin";
        public string Version => "3.0";

        private IPluginContext _context;

        private PluginSettings<ClockPluginSettings>? _pluginSettings;

        public void Initialize(IPluginContext context)
        {
            _context = context;
            _context.Register<IPluginCommand>(new ClockCommand());
            _context.Register<IPluginCommand>(new GetCurrentTimeCommand(_pluginSettings));
            _context.Register<IPluginCommand>(new SetTimeFormatCommand(_pluginSettings));
            _context.Register<IPluginCommand>(new ToggleDateCommand(_pluginSettings));
            _context.Register<IPluginCommand>(new GetTimeInSecondsCommand());
            _context.Register<IPluginCommand>(new ResetToDefaultFormatCommand(_pluginSettings));

            // Настройки плагина
            var settingsService = context.SettingsService;
            _pluginSettings = new PluginSettings<ClockPluginSettings>("ClockPlugin", settingsService);

            // Пример использования:
            var timeFormat = _pluginSettings.Value.ClockFormat;

            // Обновление значений
            _pluginSettings.Update(settings =>
            {
                settings.ClockFormat = "HH:mm";
                settings.ShowDate = false;
            });
        }

        public void Shutdown()
        {
            _pluginSettings?.Save();
        }

        public IEnumerable<IPluginCommand> GetCommands()
        {
            return new List<IPluginCommand>
            {
                new GetCurrentTimeCommand(_pluginSettings),
                new SetTimeFormatCommand(_pluginSettings),
                new ToggleDateCommand(_pluginSettings),
                new GetTimeInSecondsCommand(),
                new ResetToDefaultFormatCommand(_pluginSettings)
            };
        }
    }
}
 
