
using PluginSystem.Core;

namespace ClockPlugin
{
    public class ResetToDefaultFormatCommand : IPluginCommand
    {
        public string Id => "ResetToDefaultFormatCommand";
        public string Name => "Reset Clock Format";
        public string Description => "Сбрасывает формат часов и отображение даты на значения по умолчанию.";
        public CommandCategory Category => CommandCategory.Tools;

        public IReadOnlyList<string> ExpectedParameters => _expectedParameters;
        private readonly List<string> _expectedParameters = new() { "confirm" }; // можно ввести "yes", чтобы сбросить

        private readonly PluginSettings<ClockPluginSettings>? _pluginSettings;

        // Для Undo
        private string? _previousFormat;
        private bool? _previousShowDate;

        public ResetToDefaultFormatCommand(PluginSettings<ClockPluginSettings>? pluginSettings)
        {
            _pluginSettings = pluginSettings;
        }

        public void Execute(ICommandContext context)
        {
            var confirm = context.Parameters.Get<string>("confirm");
            if (!string.Equals(confirm, "yes", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Сброс отменён. Укажите параметр confirm = yes для подтверждения.");
                return;
            }

            var settings = _pluginSettings?.Value ?? new ClockPluginSettings();

            // Сохраняем старые значения для Undo
            _previousFormat = settings.ClockFormat;
            _previousShowDate = settings.ShowDate;

            // Сброс на значения по умолчанию
            _pluginSettings?.Update(s =>
            {
                s.ClockFormat = "HH:mm";
                s.ShowDate = false;
            });

            Console.WriteLine("Настройки часов сброшены к значениям по умолчанию.");
        }

        public bool CanUndo => true;

        public void Undo(ICommandContext context)
        {
            if (_previousFormat == null || _previousShowDate == null)
            {
                Console.WriteLine("Undo невозможно — нет сохранённых предыдущих значений.");
                return;
            }

            _pluginSettings?.Update(s =>
            {
                s.ClockFormat = _previousFormat!;
                s.ShowDate = _previousShowDate.Value;
            });

            Console.WriteLine("Настройки часов восстановлены к предыдущим значениям.");
        }

        public ICommandParameters GetDefaultParameters()
        {
            var parameters = new CommandParameters();
            parameters.Set("confirm", "no"); // по умолчанию сброс не происходит
            return parameters;
        }
    }
}
