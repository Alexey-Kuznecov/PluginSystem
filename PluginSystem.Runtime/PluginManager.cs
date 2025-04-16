#define DEBUG 

namespace PluginSystem.Runtime
{
    using NLog.Config;
    using PluginSystem.Core;
    using PluginSystem.Core.PluginSystem.Core;
    using PluginSystem.Core.Utilities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// Менеджер плагинов, отвечающий за загрузку, выгрузку и управление плагинами.
    /// </summary>
    public class PluginManager : IPluginManager
    {
        #region Поля

        private readonly Dictionary<string, IPluginContainer> _loadedPlugins = new(); // Контейнеры плагинов
        private readonly PluginLoader _loader = new(); // Загрузчик плагинов
        private readonly ILoggerService _logger = new NLogLoggerService(); // Логгер
        private readonly string _assemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
        private readonly PluginPersistenceService _persistenceService;

        #endregion

        #region Свойства и события

        /// <summary>
        /// Коллекция загруженных плагинов, индексированных по их уникальному идентификатору.
        /// </summary>
        public Dictionary<string, IPluginContainer> PluginContainers => _loadedPlugins;

        public event Action<IPlugin> OnPluginLoaded = delegate { };
        public event Action<IPlugin> OnPluginUnloaded = delegate { };

        #endregion

        #region Зависимости и инициализация

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PluginManager"/> с указанным сервисом логирования.
        /// </summary>
        /// <param name="loggerService">Сервис логирования для записи сообщений.</param>
        public PluginManager(PluginPersistenceService persistenceService, ILoggerService loggerService)
        {
            _persistenceService = persistenceService;
            _logger = loggerService;

            // Инициализация остальных зависимостей
        }

        #endregion

        /// <summary>
        /// Загружает плагин по его имени.
        /// </summary>
        /// <param name="name">Имя плагина (имя папки и DLL-файла без расширения).</param>
        /// <returns><c>true</c>, если плагин успешно загружен; в противном случае — <c>false</c>.</returns>
        public bool LoadPlugin(string name)
        {
            var pluginPath = GetPluginPath(name); // Получаем путь к плагину
            var container = LoadPluginFactory(pluginPath); // Загружаем фабрику плагина
            if (container == null)
            {
                _logger.Error($"Не удалось загрузить фабрику плагина из {pluginPath}");
                return false;
            }

            var info = container.PluginInfo;
            info.EnsureSystemId(); // Получаем информацию о плагине
            
            if (IsPluginAlreadyLoaded(info.SystemID))
            {
                _logger.Error($"Не удалось загрузить плагин [{info.Name}], плагин уже загружен. {pluginPath}");
                return false;
            }
            // Создаем экземпляр плагина и регистрируем его в контейнере
            // Создаем экземпляр плагина
            PluginContainers.Add(info.SystemID, container);
            OnPluginLoaded?.Invoke(container.Plugin); // вызов события
            _logger.Info($"Плагин [{info.Name}] загружен и зарегистрирован.");
            return true;
        }

        #region Вспомогательные методы

        /// <summary>
        /// Формирует путь к DLL-файлу плагина на основе его имени.
        /// </summary>
        /// <param name="name">Имя плагина.</param>
        /// <returns>Полный путь к DLL-файлу плагина.</returns>
        private string GetPluginPath(string path) 
        {
            if (path.TryGetFirstDllFileInfo(out var fileName, out _))
            {
                return Path.Combine(path, fileName);
            }

            return AppContext.BaseDirectory;
        }


        /// <summary>
        /// Загружает фабрику плагина из указанного пути.
        /// </summary>
        /// <param name="path">Путь к DLL-файлу плагина.</param>
        /// <returns>Экземпляр фабрики плагина или <c>null</c>, если загрузка не удалась.</returns>
        private IPluginContainer? LoadPluginFactory(string path) =>
            _loader.LoadPlugin(path);

        /// <summary>
        /// Проверяет, загружен ли уже плагин с указанным идентификатором.
        /// </summary>
        /// <param name="systemId">Уникальный идентификатор плагина.</param>
        /// <returns><c>true</c>, если плагин уже загружен; в противном случае — <c>false</c>.</returns>
        private bool IsPluginAlreadyLoaded(string systemId) =>
            PluginContainers.ContainsKey(systemId);

        #endregion

        /// <summary>
        /// Выгружает все загруженные плагины и очищает контейнер.
        /// </summary>
        public void UnloadAll()
        {
            foreach (var container in PluginContainers.Values)
            {
                OnPluginUnloaded?.Invoke(container.Plugin); // уведомляем о выгрузке
            }
            PluginContainers.Clear();
            _logger.Info("Все плагины выгружены.");
        }

        public bool UnloadPlugin(string systemId)
        {
            if (!_loadedPlugins.TryGetValue(systemId, out var container))
                return false;

            _loadedPlugins.Remove(systemId);

            // Вызываем Dispose, если нужно
            var disposable = container.Plugin as IDisposable;
            if (disposable != null)
                disposable.Dispose();
            
            container.Unload(); // выгружаем плагин
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

#if DEBUG
            if (container?.LoadContext != null) 
                PluginUnloadDebugger.MonitorUnload(container?.LoadContext, systemId);
            var types = container?.LoadedAssembly?.GetTypes();
#endif
            // Проверяем, что все типы выгружены
            container = null;
            return true;
        }

        // Загружает все плагины и возвращает коллекцию их контейнеров
        public IEnumerable<IPluginContainer> LoadAllPlugins()
        {
            _logger.Info($"Начало загрузки плагинов..");
            var newlyLoaded = new List<IPluginContainer>();

            foreach (var pluginDir in Directory.EnumerateDirectories(_assemblyPath))
            {
                // var name = Path.GetFileName(pluginDir);
                var previousCount = _loadedPlugins.Count;

                if (LoadPlugin(pluginDir) && _loadedPlugins.Count > previousCount)
                {
                    // Последний добавленный — это и есть наш новый плагин
                    var container = _loadedPlugins.Values.Last();
                    newlyLoaded.Add(container);
                }
            }

            return newlyLoaded;
        }


        public IPlugin? GetPlugin(string pluginId) =>
            _loadedPlugins.Values
                .FirstOrDefault(c => string.Equals(
                    c.PluginInfo.SystemID, pluginId, StringComparison.OrdinalIgnoreCase))?.Plugin;


        // Получение плагина по ID
        public IPluginContainer? GetContainerById(string pluginId) =>
        _loadedPlugins.TryGetValue(
            pluginId, out var container) ? container : null;


        public PluginInfo? GetPluginInfo(string pluginId) =>
       _loadedPlugins.Values
           .FirstOrDefault(c => string.Equals(
               c.PluginInfo.SystemID, pluginId, StringComparison.OrdinalIgnoreCase))?.PluginInfo;


        public IEnumerable<IPluginContainer> GetAllPlugins()
            => _loadedPlugins.Values;

        public void SavePluginInfo(PluginInfo info)
        {
            _persistenceService.Save(info);
        }

        public PluginInfo? LoadPluginInfo(string systemId)
        {
            return _persistenceService.Load(systemId);
        }

        public bool PluginInfoExists(string systemId)
        {
            return _persistenceService.Exists(systemId);
        }
    }
}
