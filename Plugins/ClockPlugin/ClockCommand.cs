
using PluginSystem.Core;

namespace ClockPlugin
{
    public class ClockCommand : IPluginCommand
    {
        public string Id => "clock";
        public string Name => "Показать текущее время";
        public string Description => "Показывает текущее локальное время";
        public bool CanUndo => false;

        public CommandCategory Category => CommandCategory.Tools;

        public IReadOnlyList<string> ExpectedParameters => throw new NotImplementedException();

        public void Execute(ICommandContext context)
        {
            Console.WriteLine($"🕒 Текущее время: {DateTime.Now:T}");
        }

        public ICommandParameters GetDefaultParameters()
        {
            throw new NotImplementedException();
        }

        public void Undo(ICommandContext context)
        {
            // Нечего откатывать
        }
    }
}
