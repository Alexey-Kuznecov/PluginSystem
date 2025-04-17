using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSystem.Abstractions.Commands;

namespace PluginSystem.Hosting.ConsoleCommands.Commands
{
    public class HelpCommand : IConsoleCommand
    {
        private readonly IConsoleCommandDispatcher _dispatcher;

        public HelpCommand(IConsoleCommandDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public string Name => "help";
        public string Description => "Показывает список доступных команд или справку по конкретной команде.";
        public IEnumerable<string> Aliases => new[] { "?", "h" };

        public void Execute(ConsoleCommandContext context)
        {
            var args = context.Arguments;

            if (args.Length == 0)
            {
                context.Output.WriteLine("Доступные команды:");
                foreach (var command in _dispatcher.GetAvailableCommands().Distinct())
                {
                    context.Output.WriteLine($" - {command.Name,-15} {command.Description}");
                }
            }
            else
            {
                var name = args[0];
                var command = _dispatcher.GetAvailableCommands()
                    .FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                                      || c.Aliases.Contains(name, StringComparer.OrdinalIgnoreCase));

                if (command != null)
                {
                    context.Output.WriteLine($"{command.Name} — {command.Description}");
                    if (command.Aliases.Any())
                    {
                        context.Output.WriteLine($"Алиасы: {string.Join(", ", command.Aliases)}");
                    }
                }
                else
                {
                    context.Output.WriteError($"Команда \"{name}\" не найдена.");
                }
            }
        }

        public void Execute(IConsoleCommandContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetSuggestions(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
