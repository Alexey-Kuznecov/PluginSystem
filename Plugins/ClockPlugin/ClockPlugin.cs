
using PluginSystem.Core;
using PluginSystem.Core.PluginSystem.Core;

namespace ClockPlugin
{
    public class ClockPlugin : IPlugin, ICommandProvider, IPluginUnloadable
    {
        public string Name => "ClockPlugin";
        public string Version => "3.0";

        private IPluginContext _context; 

        private PluginSettings<ClockPluginSettingsPropertyChanged>? _pluginSettings; // Change the type to match the expected type
        private readonly List<Delegate> _registeredHandlers = new ();
        // Assuming `someEvent` and `handler` are placeholders for actual event and handler references,
        // you need to define them properly. Below is an example fix:

        // Define the event and handler
        private event EventHandler? someEvent;

        public void Initialize(IPluginContext context)
        {
            _context = context;

            // Update the instantiation of PluginSettings to use ClockPluginSettings instead of ClockPluginSettings2
            _pluginSettings = new PluginSettings<ClockPluginSettingsPropertyChanged>("ClockPlugin", context.SettingsService);

            // Register commands with the corrected _pluginSettings type
            _context.Register<IPluginCommand>(new GetCurrentTimeCommand(_pluginSettings));
            //_context.Register<IPluginCommand>(new SetTimeFormatCommand(_pluginSettings));
            //_context.Register<IPluginCommand>(new ToggleDateCommand(_pluginSettings));
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
            ////new SetTimeFormatCommand(_pluginSettings),
            //new ToggleDateCommand(_pluginSettings),
            new GetTimeInSecondsCommand(),
            //new ResetToDefaultFormatCommand(_pluginSettings)
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

