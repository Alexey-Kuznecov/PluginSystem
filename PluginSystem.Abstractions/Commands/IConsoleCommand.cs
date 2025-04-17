
namespace PluginSystem.Abstractions.Commands
{
    public interface IConsoleCommand
    {
        IEnumerable<string> Aliases => Enumerable.Empty<string>();
        IEnumerable<string> GetSuggestions(string[] args); // <- поддержка автодополнения по аргументам
        string Name { get; }
        string Description { get; }
        /// <summary> Выполняет команду в заданном контексте. </summary>
        void Execute(IConsoleCommandContext context);
    }
}
