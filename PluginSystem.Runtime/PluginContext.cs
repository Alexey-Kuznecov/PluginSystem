
using PluginSystem.Abstractions.Plugin;
using PluginSystem.Abstractions.Plugin.PluginSystem.Core;
using PluginSystem.Abstractions.Services;

namespace PluginSystem.Runtime
{
    using NLog;
    using PluginSystem.Abstractions.Commands;
    using PluginSystem.Core;
    using PluginSystem.Core.PluginSystem.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Контекст плагина, предоставляющий доступ к сервисам, командам, настройкам и механизмам управления жизненным циклом.
    /// Используется внутри плагинов для регистрации зависимостей и взаимодействия с хост-приложением.
    /// </summary>
    public class PluginContext : IPluginContext
    {
        private readonly Dictionary<Type, List<object>> _services = new();
        private readonly List<IDisposable> _disposables = new();
        private readonly List<object> _settings = new();
        private readonly CommandManager _commandManager;
        private readonly IPluginSettingsService _settingsService;
        private readonly ILogger _logger;
        private readonly IConsoleCommandRegistry _commandRegistry;
        private readonly string _pluginId;
        private readonly string _pluginDirectory;

        /// <summary>
        /// Коллекция зарегистрированных команд плагина.
        /// </summary>
        public List<IPluginCommand> Commands { get; } = new();

        /// <summary>
        /// Настройки плагина в формате "ключ-значение".
        /// </summary>
        public Dictionary<string, string> PluginSettings { get; } = new();

        /// <summary>
        /// Сервис для сохранения и загрузки пользовательских настроек плагина.
        /// </summary>
        public IPluginSettingsService SettingsService { get; } = new JsonPluginSettingsService();

        // ILogger
        public ILogger Logger => _logger;

        // PluginId
        public string PluginId => _pluginId;

        // IConsoleCommandRegistry
        IConsoleCommandRegistry IPluginContext.Commands => _commandRegistry;

        // Settings (интерфейс для работы с настройками плагина)
        public IPluginSettingsService Settings => _settingsService;

        // PluginDirectory
        public string PluginDirectory => _pluginDirectory;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="PluginContext"/> с указанным путём к файлу настроек.
        /// </summary>
        /// <param name="path">Путь к файлу, в котором будут храниться настройки плагина.</param>
        public PluginContext(string path)
        {
            SettingsService = new JsonPluginSettingsService(path);
            _commandManager = new CommandManager();
        }

        public PluginContext(
            IPluginSettingsService settingsService,
            ILogger logger,
            IConsoleCommandRegistry commandRegistry,
            string pluginId,
            string pluginDirectory)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _commandRegistry = commandRegistry ?? throw new ArgumentNullException(nameof(commandRegistry));
            _pluginId = pluginId ?? throw new ArgumentNullException(nameof(pluginId));
            _pluginDirectory = pluginDirectory ?? throw new ArgumentNullException(nameof(pluginDirectory));
        }

        /// <summary>
        /// Регистрирует реализацию интерфейса <typeparamref name="TService"/> через создание нового экземпляра <typeparamref name="TImplementation"/>.
        /// </summary>
        public void Register<TService, TImplementation>()
            where TImplementation : TService, new()
            where TService : class
        {
            var implementation = new TImplementation();
            Register<TService>(implementation);
        }

        /// <summary>
        /// Регистрирует экземпляр сервиса или команды.
        /// </summary>
        /// <typeparam name="T">Тип сервиса.</typeparam>
        /// <param name="instance">Экземпляр для регистрации.</param>
        /// <exception cref="ArgumentNullException">Если instance равен null.</exception>
        public void Register<T>(T instance) where T : class
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            var type = typeof(T);
            if (!_services.ContainsKey(type))
                _services[type] = new List<object>();

            _services[type].Add(instance);

            if (instance is IPluginCommand command && !Commands.Contains(command))
            {
                Commands.Add(command);
                _commandManager.RegisterCommand(command);
            }
        }

        /// <summary>
        /// Получает первый зарегистрированный сервис типа <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Тип сервиса.</typeparam>
        /// <returns>Экземпляр сервиса или null, если не найден.</returns>
        public T? GetService<T>() where T : class
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var list) && list.Count > 0)
                return list[0] as T;

            return null;
        }

        /// <summary>
        /// Получает все зарегистрированные сервисы типа <typeparamref name="T"/>.
        /// </summary>
        public IEnumerable<T> GetServices<T>() where T : class
        {
            var type = typeof(T);
            return _services.TryGetValue(type, out var list) ? list.OfType<T>() : Enumerable.Empty<T>();
        }

        /// <summary>
        /// Устанавливает строковое значение настройки по ключу.
        /// </summary>
        public void SetPluginSetting(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Ключ не может быть пустым или содержать только пробелы.", nameof(key));

            PluginSettings[key] = value;
        }

        /// <summary>
        /// Получает значение настройки по ключу.
        /// </summary>
        public string? GetPluginSetting(string key) =>
            PluginSettings.TryGetValue(key, out string value) ? value : null;

        /// <summary>
        /// Возвращает список всех зарегистрированных команд.
        /// </summary>
        public List<IPluginCommand> GetRegisteredCommands() => Commands;

        /// <summary>
        /// Выполняет команду с использованием переданного контекста.
        /// </summary>
        public void ExecuteCommand(IPluginCommand command, IPluginCommandContext context) =>
            _commandManager.ExecuteCommand(command, context);

        /// <summary>
        /// Отменяет последнюю выполненную команду (если поддерживается).
        /// </summary>
        public void Undo() => _commandManager.Undo();

        /// <summary>
        /// Повторяет отменённую команду (если поддерживается).
        /// </summary>
        public void Redo() => _commandManager.Redo();

        /// <summary>
        /// Регистрирует объект, реализующий <see cref="IDisposable"/>, для последующей очистки.
        /// </summary>
        public void RegisterDisposable(IDisposable disposable) => _disposables.Add(disposable);

        /// <summary>
        /// Регистрирует объект настройки, который может использоваться плагином.
        /// </summary>
        public void RegisterSetting(object setting) => _settings.Add(setting);

        /// <summary>
        /// Очищает все зарегистрированные ресурсы, команды и сервисы.
        /// Вызывается при выгрузке плагина.
        /// </summary>
        public void Cleanup()
        {
            foreach (var disposable in _disposables)
            {
                try
                {
                    disposable.Dispose();
                }
                catch (Exception ex)
                {
                    // логировать ошибку при очистке
                }
            }

            _disposables.Clear();
            _settings.Clear();
            Commands.Clear();
            _services.Clear();
        }

        public void UnregisterAll()
        {
            // 1. Попробовать вызвать Dispose/Unload на зарегистрированных сервисах
            foreach (var serviceList in _services.Values)
            {
                foreach (var service in serviceList)
                {
                    switch (service)
                    {
                        case IDisposable disposable:
                            disposable.Dispose();
                            break;
                        case IPluginUnloadable unloadable:
                            unloadable.OnUnload(); // если реализуют вручную
                            break;
                    }
                }
            }

            // 2. Очистка команд
            Commands.Clear();

            // 3. Очистка сервисов
            _services.Clear();
        }
        // Registering Event Handlers
        public void RegisterEventHandler(Delegate handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            // Реализуйте регистрацию обработчика событий, возможно, через события плагина или внешние сервисы.
            _logger.Debug($"Event handler {handler.Method.Name} registered.", handler);
        }

        // Unregistering Event Handlers
        public void UnregisterEventHandler(Delegate handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            // Реализуйте отмену регистрации обработчика событий.
            _logger.Debug($"Event handler {handler.Method.Name} unregistered.");
        }

        // Load settings
        public T Load<T>(string pluginId) where T : class, new()
        {
            return _settingsService.Load<T>(pluginId);
        }

        // Save settings
        public void Save<T>(string pluginId, T settings) where T : class
        {
            _settingsService.Save(pluginId, settings);
        }

        // Delete settings
        public void Delete(string pluginId)
        {
            _settingsService.Delete(pluginId);
        }
    }
}