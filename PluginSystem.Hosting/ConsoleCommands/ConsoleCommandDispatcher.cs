using PluginSystem.Abstractions.Commands;
using PluginSystem.Core.PluginSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Hosting.ConsoleCommands
{
    public class ConsoleCommandDispatcher : IConsoleCommandDispatcher
    {
        private readonly Dictionary<string, IConsoleCommand> _commands = new(StringComparer.OrdinalIgnoreCase);
        private readonly IServiceProvider _services;
        private readonly IConsoleOutput _output;

        public ConsoleCommandDispatcher(IServiceProvider services, IConsoleOutput output)
        {
            _services = services;
            _output = output;
        }

        public void Register(IConsoleCommand command)
        {
            void TryAdd(string key)
            {
                if (_commands.ContainsKey(key))
                {
                    _output.WriteError($"Команда \"{key}\" уже зарегистрирована.");
                    return;
                }

                _commands[key] = command;
            }

            TryAdd(command.Name);
            foreach (var alias in command.Aliases ?? Enumerable.Empty<string>())
            {
                TryAdd(alias);
            }
        }

        public bool ExecuteCommand(string commandLine)
        {
            if (string.IsNullOrWhiteSpace(commandLine))
            {
                _output.WriteError("Команда не может быть пустой.");
                return false;
            }

            var parts = commandLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var commandName = parts[0];
            var args = parts.Skip(1).ToArray();

            if (_commands.TryGetValue(commandName, out var command))
            {
                try
                {
                    var context = new ConsoleCommandContext(_services, _output, args);
                    command.Execute(context);
                    return true;
                }
                catch (Exception ex)
                {
                    _output.WriteError($"Ошибка при выполнении команды: {ex.Message}");
                    return false;
                }
            }
            else
            {
                _output.WriteError($"Команда \"{commandName}\" не найдена.");
                return false;
            }
        }

        public IEnumerable<IConsoleCommand> GetAvailableCommands() => _commands.Values;
    }

}
