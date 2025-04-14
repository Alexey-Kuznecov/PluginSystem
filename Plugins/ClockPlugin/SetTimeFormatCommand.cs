

using PluginSystem.Core;

namespace ClockPlugin
{
    public class SetTimeFormatCommand : IPluginCommand
    {
        public string Id => "SetTimeFormatCommand";
        public string Name => "Set Time Format";
        public string Description => "Устанавливает пользовательский формат отображения времени.";
        public CommandCategory Category => CommandCategory.Tools;

        public IReadOnlyList<string> ExpectedParameters => _expectedParameters;
        private readonly List<string> _expectedParameters = new() { "format" };

        private readonly PluginSettings<ClockPluginSettings>? _pluginSettings;

        // Для Undo
        private string? _previousFormat;

        public SetTimeFormatCommand(PluginSettings<ClockPluginSettings>? pluginSettings)
        {
            _pluginSettings = pluginSettings;
        }

        public void Execute(ICommandContext context)
        {
            var newFormat = context.Parameters.Get<string>("format");

            if (string.IsNullOrWhiteSpace(newFormat))
            {
                Console.WriteLine("Формат не задан. Используйте параметр 'format'.");
                return;
            }

            var settings = _pluginSettings?.Value;
            if (settings == null)
            {
                Console.WriteLine("Ошибка доступа к настройкам.");
                return;
            }

            _previousFormat = settings.ClockFormat;

            _pluginSettings?.Update(s => s.ClockFormat = newFormat);
            Console.WriteLine($"Формат времени обновлён: {newFormat}");
        }

        public bool CanUndo => true;

        public void Undo(ICommandContext context)
        {
            if (_previousFormat == null)
            {
                Console.WriteLine("Нет предыдущего значения для отката.");
                return;
            }

            _pluginSettings?.Update(s => s.ClockFormat = _previousFormat);
            Console.WriteLine($"Формат времени восстановлен: {_previousFormat}");
        }

        public ICommandParameters GetDefaultParameters()
        {
            var parameters = new CommandParameters();
            parameters.Set("format", "HH:mm:ss"); // Значение по умолчанию
            return parameters;
        }
    }
}
