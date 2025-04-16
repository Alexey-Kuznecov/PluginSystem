﻿
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
        private readonly CommandManager _commandManager;

        public PluginContext(string path)
        {
            SettingsService = new JsonPluginSettingsService(path);
            _commandManager = new CommandManager();
        }

        public void Register<TService, TImplementation>()
            where TImplementation : TService, new()
            where TService : class
        {
            var implementation = new TImplementation();
            Register<TService>(implementation);
        }

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

        public T? GetService<T>() where T : class
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var list) && list.Count > 0)
                return list[0] as T;

            return null;
        }

        public IEnumerable<T> GetServices<T>() where T : class
        {
            var type = typeof(T);
            return _services.TryGetValue(type, out var list) ? list.OfType<T>() : Enumerable.Empty<T>();
        }

        public void SetPluginSetting(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Ключ не может быть пустым или содержать только пробелы.", nameof(key));

            PluginSettings[key] = value;
        }

        public string? GetPluginSetting(string key) =>
            PluginSettings.TryGetValue(key, out string value) ? value : null;

        public List<IPluginCommand> GetRegisteredCommands() => Commands;

        public void ExecuteCommand(IPluginCommand command, ICommandContext context) =>
            _commandManager.ExecuteCommand(command, context);

        public void Undo() => _commandManager.Undo();
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

        public void RegisterEventHandler(Delegate handler)
        {
            throw new NotImplementedException();
        }

        public void UnregisterEventHandler(Delegate handler)
        {
            throw new NotImplementedException();
        }
    }
}