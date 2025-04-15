using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Hosting.ConsoleCommands.Commands
{
    public class SuggestCommand : IConsoleCommand
    {
        private readonly IAutoCompleteProvider _provider;

        public SuggestCommand(IAutoCompleteProvider provider)
        {
            _provider = provider;
        }

        public string Name => "suggest";
        public string Description => "Предлагает команды, начинающиеся с указанного ввода.";

        public void Execute(CommandContext context)
        {
            if (context.Arguments.Length == 0)
            {
                context.Output.WriteError("Укажите часть команды для автодополнения.");
                return;
            }

            var suggestions = _provider.GetSuggestions(context.Arguments[0]);

            if (!suggestions.Any())
            {
                context.Output.WriteLine("Совпадений не найдено.");
            }
            else
            {
                foreach (var suggestion in suggestions)
                    context.Output.WriteLine(suggestion);
            }
        }

        public IEnumerable<string> GetSuggestions(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
