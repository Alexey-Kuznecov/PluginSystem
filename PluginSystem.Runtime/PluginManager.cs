
namespace PluginSystem.Runtime
{
    using PluginSystem.Core;
    using PluginSystem.Core.PluginSystem.Core;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Менеджер плагинов, отвечающий за загрузку, выгрузку и управление плагинами.
    /// </summary>
    public class PluginManager : IPluginManager
    {
        #region Поля

        private readonly Dictionary<string, IPluginContainer> _pluginContainers = new(); // Контейнеры плагинов
        private readonly PluginLoader _loader = new(); // Загрузчик плагинов
        private readonly ILoggerService _logger = new NLogLoggerService(); // Логгер
        private readonly string _assemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
        private readonly PluginPersistenceService _persistenceService;

        #endregion

        #region Свойства и события

        /// <summary>
        /// Коллекция загруженных плагинов, индексированных по их уникальному идентификатору.
        /// </summary>
        public Dictionary<string, IPluginContainer> PluginContainers => _pluginContainers;

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
            var factory = LoadPluginFactory(pluginPath); // Загружаем фабрику плагина
            if (factory == null)
            {
                _logger.Error($"Не удалось загрузить фабрику плагина из {pluginPath}");
                return false;
            }

            var info = factory.GetPluginInfo(new PluginInfo());
            info.EnsureSystemId(); // Получаем информацию о плагине
            
            if (IsPluginAlreadyLoaded(info.SystemID))
            {
                _logger.Error($"Не удалось загрузить плагин [{info.Name}], плагин уже загружен. {pluginPath}");
                return false;
            }
            var plugin = factory.CreatePlugin(); // Создаем экземпляр плагина
            _logger.Info($"Плагин [{info.Name}] загружен и зарегистрирован.");
            return RegisterPlugin(info, plugin); // Регистрируем плагин в контейнере
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
        private IPluginFactory? LoadPluginFactory(string path) =>
            _loader.LoadPlugin(path);

        /// <summary>
        /// Проверяет, загружен ли уже плагин с указанным идентификатором.
        /// </summary>
        /// <param name="systemId">Уникальный идентификатор плагина.</param>
        /// <returns><c>true</c>, если плагин уже загружен; в противном случае — <c>false</c>.</returns>
        private bool IsPluginAlreadyLoaded(string systemId) =>
            PluginContainers.ContainsKey(systemId);

        /// <summary>
        /// Регистрирует плагин в контейнере загруженных плагинов.
        /// </summary>
        /// <param name="info">Информация о плагине.</param>
        /// <param name="plugin">Экземпляр плагина.</param>
        /// <returns><c>true</c>, если регистрация прошла успешно; в противном случае — <c>false</c>.</returns>
        private bool RegisterPlugin(PluginInfo info, IPlugin plugin)
        {
            var container = new PluginContainer(info, plugin);
            PluginContainers.Add(info.SystemID, container);
            OnPluginLoaded?.Invoke(plugin); // вызов события
            return true;
        }

        #endregion

        /// <summary>
        /// Выгружает все загруженные плагины и очищает контейнер.
        /// </summary>
        public void UnloadAll()
        {
            foreach (var container in PluginContainers.Values)
            {
                OnPluginUnloaded?.Invoke(container.Plugin); // уведомляем о выгрузке
                container.Clear();
            }
            PluginContainers.Clear();
            _logger.Info("Все плагины выгружены.");
        }

        // Загружает все плагины и возвращает коллекцию их контейнеров
        public IEnumerable<IPluginContainer> LoadAllPlugins()
        {
            _logger.Info($"Начало загрузки плагинов..");
            var newlyLoaded = new List<IPluginContainer>();

            foreach (var pluginDir in Directory.EnumerateDirectories(_assemblyPath))
            {
                // var name = Path.GetFileName(pluginDir);
                var previousCount = _pluginContainers.Count;

                if (LoadPlugin(pluginDir) && _pluginContainers.Count > previousCount)
                {
                    // Последний добавленный — это и есть наш новый плагин
                    var container = _pluginContainers.Values.Last();
                    newlyLoaded.Add(container);
                }
            }

            return newlyLoaded;
        }


        public IPlugin? GetPlugin(string pluginId) =>
            _pluginContainers.Values
                .FirstOrDefault(c => string.Equals(
                    c.PluginInfo.SystemID, pluginId, StringComparison.OrdinalIgnoreCase))?.Plugin;


        // Получение плагина по ID
        public IPluginContainer? GetContainerById(string pluginId) =>
        _pluginContainers.TryGetValue(
            pluginId, out var container) ? container : null;


        public PluginInfo? GetPluginInfo(string pluginId) =>
       _pluginContainers.Values
           .FirstOrDefault(c => string.Equals(
               c.PluginInfo.SystemID, pluginId, StringComparison.OrdinalIgnoreCase))?.PluginInfo;


        public IEnumerable<IPluginContainer> GetAllPlugins()
            => _pluginContainers.Values;

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
