
using PluginSystem.Abstractions.Commands;

namespace PluginSystem.Abstractions.Plugin
{
    public interface IPluginCommand
    {
        string Name => "Unnamed Command";
        bool CanUndo => false;
        string Id => string.Empty;
        string Description => string.Empty;
        CommandCategory Category => CommandCategory.Other;
        IReadOnlyList<string> ExpectedParameters => Array.Empty<string>();
        //ICommandParameters GetDefaultParameters() => new CommandParameters();
        void Execute(IPluginCommandContext context);
        void Undo(IPluginCommandContext context) { }
    }
}
