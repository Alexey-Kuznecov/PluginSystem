
namespace PluginSystem.Runtime
{
    using PluginSystem.Core;

    /// <summary>
    /// Менеджер плагинов, отвечающий за загрузку, выгрузку и управление плагинами.
    /// </summary>
    public class PluginManager
    {
        #region Поля для экзмпляров контейнера, загрузчика и логгера

        private readonly PluginLoader _loader = new(); // Загрузчик плагинов
        private readonly ILoggerService _logger = new NLogLoggerService(); // Логгер

        private readonly string _assemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");

        #endregion

        #region Свойства контейнера плагинов

        public Dictionary<Guid, PluginContainer> LoadedPlugins = new(); // Словарь с загруженными плагинами

        #endregion

        public PluginManager(ILoggerService loggerService)
        {
            _logger = loggerService;
        }

        /// <summary>
        /// Загружает плагин по имени.
        /// </summary>
        public bool LoadPlugin(string name)
        {
            var factory = _loader.LoadPlugin(Path.Combine(_assemblyPath, name) + $"\\{name}.dll");
            if (factory == null) return false;

            var pluginInfo = factory.GetPluginInfo(new PluginInfo());
            var plugin = factory.CreatePlugin();

            if (pluginInfo == null || plugin == null) return false;

            // Проверка на дублирование
            if (LoadedPlugins.ContainsKey(pluginInfo.SystemID))
            {
                _logger.Error($"Плагин {plugin.Name} уже загружен.");
                return false;
            }

            var container = new PluginContainer(pluginInfo, plugin) { };
            LoadedPlugins.Add(pluginInfo.SystemID, container);

            _logger.Info($"Плагин {plugin.Name} успешно загружен.");
            return true;
        }

        /// <summary>
        /// Выгружает все плагины.
        /// </summary>
        public void UnloadAll()
        {
            foreach (var container in LoadedPlugins.Values)
            {
                container.Clear();
            }
            LoadedPlugins.Clear();
            _logger.Info("Все плагины выгружены.");
        }
    }
}
