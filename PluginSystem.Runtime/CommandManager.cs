
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

        /// <summary>
        /// Регистрирует команду, основанную на делегате Action<object>.
        /// </summary>
        /// <param name="value">Делегат, представляющий действие команды.</param>
        public void RegisterCommand(Action<object> value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            var command = new DelegateCommand(value);
            RegisterCommand(command);
        }

        /// <summary>
        /// Внутренний класс, реализующий IPluginCommand на основе делегатов.
        /// </summary>
        private class DelegateCommand : IPluginCommand
        {
            private readonly Action<object> _execute;
            private readonly Action<object>? _undo;

            public DelegateCommand(Action<object> execute, Action<object>? undo = null)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _undo = undo;
            }

            public bool CanUndo => _undo != null;

            public void Execute(ICommandContext context)
            {
                _execute(context);
            }

            public void Undo(ICommandContext context)
            {
                if (_undo == null)
                    throw new InvalidOperationException("Undo operation is not supported for this command.");
                _undo(context);
            }
        }
    }
}
