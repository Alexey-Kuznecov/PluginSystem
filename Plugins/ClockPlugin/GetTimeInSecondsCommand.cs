
using PluginSystem.Core;

namespace ClockPlugin
{
    public class GetTimeInSecondsCommand : IPluginCommand
    {
        private readonly List<string> _expectedParameters = new() { "format", "timezone" };

        public string Id => "GetTimeInSecondsCommand";
        public string Name => "Get Time In Seconds";
        public string Description => "Показывает текущее время в секундах с начала эпохи Unix.";
        public CommandCategory Category => CommandCategory.Tools;

        public IReadOnlyList<string> ExpectedParameters => _expectedParameters;

        public void Execute(ICommandContext context)
        {
            var format = context.Parameters.Get<string>("format") ?? "HH:mm";
            var timezone = context.Parameters.Get<string>("timezone") ?? "local";

            var now = timezone.ToLowerInvariant() switch
            {
                "utc" => DateTime.UtcNow,
                _ => DateTime.Now
            };

            var timeInSeconds = (long)(now - new DateTime(1970, 1, 1)).TotalSeconds;

            Console.WriteLine($"Current time in seconds since Unix epoch: {timeInSeconds}");
        }

        public bool CanUndo => false;

        public void Undo(ICommandContext context)
        {
            // Нечего откатывать
        }

        public ICommandParameters GetDefaultParameters()
        {
            var parameters = new CommandParameters();
            parameters.Set("format", "HH:mm");
            parameters.Set("timezone", "local");
            return parameters;
        }
    }
}
