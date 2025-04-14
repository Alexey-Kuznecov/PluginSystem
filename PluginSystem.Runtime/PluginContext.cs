
namespace PluginSystem.Runtime
{
    using PluginSystem.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PluginContext : IPluginContext
    {
        private readonly Dictionary<Type, List<object>> _services = new();
        public List<IPluginCommand> Commands { get; } = new();
        public Dictionary<string, string> PluginSettings { get; } = new();
        public IPluginSettingsService SettingsService { get; }
        private readonly CommandManager _commandManager; // Менеджер команд для undo/redo

        public PluginContext(string path)
        {
            SettingsService = new JsonPluginSettingsService(path);
            _commandManager = new CommandManager(); // Инициализация менеджера команд
        }

        public void Register<T>(T instance) where T : class
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            var type = typeof(T);

            if (!_services.ContainsKey(type))
                _services[type] = new List<object>();

            _services[type].Add(instance);

            // Регистрация команд
            if (instance is IPluginCommand command && !Commands.Contains(command))
            {
                Commands.Add(command);
                _commandManager.RegisterCommand(command); // Регистрация в CommandManager
            }
        }
        /// <summary>
        /// Возвращает первый зарегистрированный экземпляр сервиса заданного типа.
        /// Если сервис не зарегистрирован, возвращается значение <c>null</c>.
        /// </summary>
        /// <typeparam name="T">Тип сервиса, который требуется получить.</typeparam>
        /// <returns>Первый зарегистрированный сервис или <c>null</c>, если сервис не найден.</returns>
        public T? GetService<T>() where T : class
        {
            var type = typeof(T);

            if (_services.TryGetValue(type, out var list) && list.Count > 0)
            {
                return list[0] as T;
            }

            return null;
        }

        /// <summary>
        /// Возвращает все зарегистрированные экземпляры сервиса заданного типа.
        /// </summary>
        /// <typeparam name="T">Тип сервиса.</typeparam>
        /// <returns>Перечисление зарегистрированных сервисов данного типа.</returns>
        public IEnumerable<T> GetServices<T>() where T : class
        {
            var type = typeof(T);

            if (_services.TryGetValue(type, out var list))
            {
                return list.OfType<T>();
            }

            return Enumerable.Empty<T>();
        }

        public void SetPluginSetting(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Ключ не может быть пустым или содержать только пробелы.", nameof(key));

            PluginSettings[key] = value;
        }

        public string? GetPluginSetting(string key)
        {
            if (PluginSettings.TryGetValue(key, out string value))
            {
                return value;
            }
            return null;
        }

        public List<IPluginCommand> GetRegisteredCommands()
        {
            return Commands; // Возвращает все зарегистрированные команды
        }

        // Исполнение команды с учётом undo/redo
        public void ExecuteCommand(IPluginCommand command, ICommandContext context)
        {
            _commandManager.ExecuteCommand(command, context); // Выполняем команду через CommandManager
        }

        // Функции для Undo и Redo
        public void Undo()
        {
            _commandManager.Undo();
        }

        public void Redo()
        {
            _commandManager.Redo();
        }
    }
}