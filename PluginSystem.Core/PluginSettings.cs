
namespace PluginSystem.Core
{
    using System;
    using System.ComponentModel;

    public class PluginSettings<T> where T : class, INotifyPropertyChanged, new()
    {
        private readonly string _pluginName;
        private readonly IPluginSettingsService _settingsService;
        private T? _settings;
        private bool _loaded;

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
                    _settings.PropertyChanged += (_, _) => Save();
                    _loaded = true;
                }
                return _settings!;
            }
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
            updateAction.Invoke(Value); // сохранение произойдёт автоматически
        }

        public void Delete()
        {
            _settingsService.Delete(_pluginName);
            _settings = null;
            _loaded = false;
        }
    }
}
