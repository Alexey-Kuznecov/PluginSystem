using PluginSystem.Core.PluginSystem.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSystem.Abstractions.Plugin;
using PluginSystem.Abstractions.Plugin.PluginSystem.Core;

namespace PluginSystem.Core
{
    public abstract class BasePlugin<TSettings> : IPluginUnloadable
        where TSettings : class, INotifyPropertyChanged, new()
    {
        protected readonly IPluginContext Context;
        protected readonly PluginSettings<TSettings> PluginSettings;
        private readonly List<IDisposable> _disposables = new();

        protected BasePlugin(IPluginContext context, string pluginName)
        {
            Context = context;
            PluginSettings = new PluginSettings<TSettings>(pluginName, context.SettingsService);
        }

        /// <summary>
        /// Зарегистрировать команду и отслеживать её для автоматической выгрузки.
        /// </summary>
        protected void RegisterCommand(IPluginCommand command)
        {
            Context.Register(command);
        }

        /// <summary>
        /// Зарегистрировать сервис и отследить, если IDisposable.
        /// </summary>
        protected void RegisterService<T>(T service) where T : class
        {
            Context.Register(service);
            if (service is IDisposable d)
                _disposables.Add(d);
        }

        /// <summary>
        /// Получить настройки (инициализирует при первом доступе).
        /// </summary>
        protected TSettings Settings => PluginSettings.Value;

        /// <summary>
        /// Метод для ручной реализации логики загрузки в плагине.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Вызов при выгрузке плагина.
        /// </summary>
        public virtual void OnUnload()
        {
            PluginSettings.Save();

            foreach (var disposable in _disposables)
            {
                try { disposable.Dispose(); } catch { }
            }

            Context.UnregisterAll();
        }
    }
}
