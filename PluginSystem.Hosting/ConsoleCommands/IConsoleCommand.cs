using PluginSystem.Core;
using PluginSystem.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Hosting.ConsoleCommands
{
    public interface IConsoleCommand
    {
        IEnumerable<string> Aliases => Enumerable.Empty<string>();
        IEnumerable<string> GetSuggestions(string[] args); // <- поддержка автодополнения по аргументам
        string Name { get; }
        string Description { get; }
        /// <summary> Выполняет команду в заданном контексте. </summary>
        void Execute(CommandContext context);
    }
}
