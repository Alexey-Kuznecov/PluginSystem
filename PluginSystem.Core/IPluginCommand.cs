
namespace PluginSystem.Core
{
    public interface IPluginCommand
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        CommandCategory Category { get; }
        void Execute(ICommandContext context);
        void Undo(ICommandContext context);
        bool CanUndo { get; }
        ICommandParameters GetDefaultParameters();
        IReadOnlyList<string> ExpectedParameters { get; }
    }
}
