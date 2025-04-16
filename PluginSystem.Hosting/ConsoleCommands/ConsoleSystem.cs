using PluginSystem.Core.Abstractions;
using PluginSystem.Hosting.ConsoleCommands;
using PluginSystem.Hosting.ConsoleCommands.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Hosting.ConsoleCommands
{
    public class ConsoleSystem : IConsoleSystem
    {
        private readonly IConsoleCommandRegistry _registry;
        private readonly IConsoleCommandDispatcher _dispatcher;

        public IConsoleCommandRegistry Registry => _registry;
        public IConsoleCommandDispatcher Dispatcher => _dispatcher;

        public ConsoleSystem(IConsoleCommandRegistry registry, IConsoleCommandDispatcher dispatcher)
        {
            _registry = registry;
            _dispatcher = dispatcher;
        }

        public void Register(IConsoleCommand command) => _registry.Register("", command);

        public void RegisterBuiltInCommands()
        {
            // Здесь можно добавить встроенные команды, например: help, clear, exit и т.д.
            _registry.Register(new HelpCommand(_registry)); // пример
        }

        public void Dispatch(string input, CommandContext context)
            => _dispatcher.Execute(input, context);
    }
}
