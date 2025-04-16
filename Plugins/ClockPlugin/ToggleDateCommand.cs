

using PluginSystem.Core;
using PluginSystem.Core.PluginSystem.Core;

namespace ClockPlugin
{
    public class ToggleDateCommand : IPluginCommand, IPluginUnloadable
    {
        public string Id => "ToggleDateCommand";
        public string Name => "ToggleDateCommand";
        public string Description => "Включает или выключает отображение даты на часах.";
        public CommandCategory Category => CommandCategory.Tools;

        // Параметры, ожидаемые командой
        public IReadOnlyList<string> ExpectedParameters => _expectedParameters;
        private readonly List<string> _expectedParameters = new() { "forceState" }; // true / false / toggle

        private readonly PluginSettings<ClockPluginSettings>? _pluginSettings;

        public ToggleDateCommand(PluginSettings<ClockPluginSettings>? pluginSettings)
        {
            _pluginSettings = pluginSettings;
        }

        public void Execute(ICommandContext context)
        {
            var settings = _pluginSettings?.Value ?? new ClockPluginSettings();

            var forceState = context.Parameters.Get<string>("forceState");

            if (bool.TryParse(forceState, out var parsed))
            {
                settings.ShowDate = parsed;
            }
            else
            {
                // если параметр не указан или не валиден — просто переключаем
                settings.ShowDate = !settings.ShowDate;
            }

            _pluginSettings?.Update(_ => _.ShowDate = settings.ShowDate);
            Console.WriteLine($"Date display is now {(settings.ShowDate ? "enabled" : "disabled")}.");
        }

        public bool CanUndo => true;

        public void Undo(ICommandContext context)
        {
            // Пример: переключить обратно
            var settings = _pluginSettings?.Value ?? new ClockPluginSettings();
            settings.ShowDate = !settings.ShowDate;
            _pluginSettings?.Update(_ => _.ShowDate = settings.ShowDate);
            Console.WriteLine($"Undo: Date display is now {(settings.ShowDate ? "enabled" : "disabled")}.");
        }

        public ICommandParameters GetDefaultParameters()
        {
            var parameters = new CommandParameters();
            parameters.Set("forceState", "toggle"); // toggle = поведение по умолчанию (переключить)
            return parameters;
        }

        public void OnUnload()
        {
            // Например, очистка ссылок, если они тяжелые
            _pluginSettings?.Save(); // если нужно явно сохранить перед выгрузкой
            // Можно даже null-нуть:
            // _pluginSettings = null;
        }
    }
}
