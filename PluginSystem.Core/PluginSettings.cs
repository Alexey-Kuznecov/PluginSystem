using PluginSystem.Core;
using System;
using System.ComponentModel;
using PluginSystem.Abstractions.Services;
using PluginSystem.Abstractions.Settings;

namespace PluginSystem.Core
{
    public class PluginSettings<T> : IPluginSettings<T> where T : class, INotifyPropertyChanged, new()
    {
        private readonly string _pluginName;
        private readonly IPluginSettingsService _settingsService;
        private T? _settings;
        private bool _loaded;
        private bool _disposed;

        public PluginSettings(string pluginName, IPluginSettingsService settingsService)
        {
            _pluginName = pluginName;
            _settingsService = settingsService;
        }

        public T Value
        {
            get
            {
                if (!_loaded)
                {
                    _settings = _settingsService.Load<T>(_pluginName);
                    if (_settings != null)
                        _settings.PropertyChanged += OnSettingsChanged;
                    _loaded = true;
                }

                return _settings!;
            }
        }

        private void OnSettingsChanged(object? sender, PropertyChangedEventArgs e)
        {
            Save(); // автосохранение
        }

        public void Save()
        {
            if (_loaded && _settings != null)
            {
                _settingsService.Save(_pluginName, _settings);
            }
        }

        public void Update(Action<T> updateAction)
        {
            updateAction.Invoke(Value); // триггерит Save через PropertyChanged
        }

        public void Delete()
        {
            if (_loaded && _settings != null)
            {
                _settings.PropertyChanged -= OnSettingsChanged;
            }

            _settingsService.Delete(_pluginName);
            _settings = null;
            _loaded = false;
        }

        public void Dispose()
        {
            if (_disposed) return;

            if (_loaded && _settings != null)
            {
                _settings.PropertyChanged -= OnSettingsChanged;
                _settings = null;
            }

            _disposed = true;
        }
    }
}