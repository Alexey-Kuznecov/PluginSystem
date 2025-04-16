
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
            //_context.Register<IPluginCommand>(new ResetToDefaultFormatCommand(_pluginSettings));
        }

        public void Shutdown()
        {
            _pluginSettings?.Save();
            _pluginSettings = null;
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

        public void OnUnload()
        {
            foreach (var h in _registeredHandlers)
            {
                // отписка вручную или через утилиту
            }
            _registeredHandlers.Clear();
            // Очищаем настройки и все связанные ресурсы
            _pluginSettings = null;
            _pluginSettings?.Delete();
            // Удаляем все зарегистрированные команды
            //_context?.GetRegisteredCommands().Clear();
        }
        private void UnregisterHandler(object? sender, EventArgs e)
        {
            // Handler logic here
        }

        private void RegisterHandler(object? sender, EventArgs e)
        {
            // Handler logic here
        }
    }
}
 
