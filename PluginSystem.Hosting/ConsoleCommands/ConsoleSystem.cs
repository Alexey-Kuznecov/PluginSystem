
using PluginSystem.Hosting.ConsoleCommands.Commands;
using PluginSystem.Abstractions.Commands;

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
            var command = new HelpCommand(_dispatcher);
            // Здесь можно добавить встроенные команды, например: help, clear, exit и т.д.
            _registry.Register(command.Name, command); // пример
        }

        public void Dispatch(string input, ConsoleCommandContext context)
        {
            _registry.RegisterLambda("hello", "Печатает приветствие", (ctx, output) =>
            {
                output.WriteLine("Привет из лямбда-команды!");
            });
        }
    }
}
