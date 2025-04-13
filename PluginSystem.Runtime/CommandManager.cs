
using PluginSystem.Core;

namespace PluginSystem.Runtime
{
    public class CommandManager
    {
        private readonly Stack<(IPluginCommand, ICommandContext)> _undoStack = new();
        private readonly Stack<(IPluginCommand, ICommandContext)> _redoStack = new();
        private readonly List<IPluginCommand> _commands = new(); // Список всех доступных команд

        private readonly List<IPluginCommand> _allCommands = new();

        // Метод для регистрации команд
        public void RegisterCommand(IPluginCommand command)
        {
            if (!_commands.Contains(command))
            {
                _commands.Add(command);
            }
        }

        public void RegisterPluginCommands(IPlugin plugin)
        {
            if (plugin is ICommandProvider commandProvider)
            {
                var commands = commandProvider.GetCommands();
                _allCommands.AddRange(commands);
            }
        }

        public List<IPluginCommand> GetAllCommands()
        {
            return _allCommands;
        }

        public void ExecuteCommand(IPluginCommand command, ICommandContext context)
        {
            command.Execute(context);

            if (command.CanUndo)
            {
                _undoStack.Push((command, context));
                _redoStack.Clear();
            }
        }

        public void Undo()
        {
            if (_undoStack.TryPop(out var entry))
            {
                var (command, context) = entry;
                command.Undo(context);
                _redoStack.Push((command, context));
            }
        }

        public void Redo()
        {
            if (_redoStack.TryPop(out var entry))
            {
                var (command, context) = entry;
                command.Execute(context);
                _undoStack.Push((command, context));
            }
        }
    }
}
