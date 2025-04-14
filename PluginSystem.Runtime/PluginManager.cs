
namespace PluginSystem.Runtime
{
    using PluginSystem.Core;

    /// <summary>
    /// Менеджер плагинов, отвечающий за загрузку, выгрузку и управление плагинами.
    /// </summary>
    public class PluginManager
    {
        #region Поля

        private readonly PluginLoader _loader = new(); // Загрузчик плагинов
        private readonly ILoggerService _logger = new NLogLoggerService(); // Логгер
        private readonly string _assemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");

        #endregion

        #region Свойства

        /// <summary>
        /// Коллекция загруженных плагинов, индексированных по их уникальному идентификатору.
        /// </summary>
        public Dictionary<string, PluginContainer> LoadedPlugins = new();

        #endregion

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PluginManager"/> с указанным сервисом логирования.
        /// </summary>
        /// <param name="loggerService">Сервис логирования для записи сообщений.</param>
        public PluginManager(ILoggerService loggerService)
        {
            _logger = loggerService;
        }

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

            var plugin = factory.CreatePlugin(); // Создаем экземпляр плагина
            var info = factory.GetPluginInfo(new PluginInfo()); // Получаем информацию о плагине

            if (IsPluginAlreadyLoaded(info.SystemID))
            {
                _logger.Error($"Не удалось загрузить плагин, плагин уже загружен. {pluginPath}");
                return false;
            }

            return RegisterPlugin(info, plugin); // Регистрируем плагин в контейнере
        }

        #region Вспомогательные методы

        /// <summary>
        /// Формирует путь к DLL-файлу плагина на основе его имени.
        /// </summary>
        /// <param name="name">Имя плагина.</param>
        /// <returns>Полный путь к DLL-файлу плагина.</returns>
        private string GetPluginPath(string name) =>
            Path.Combine(_assemblyPath, name, $"{name}.dll");

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
            LoadedPlugins.ContainsKey(systemId);

        /// <summary>
        /// Регистрирует плагин в контейнере загруженных плагинов.
        /// </summary>
        /// <param name="info">Информация о плагине.</param>
        /// <param name="plugin">Экземпляр плагина.</param>
        /// <returns><c>true</c>, если регистрация прошла успешно; в противном случае — <c>false</c>.</returns>
        private bool RegisterPlugin(PluginInfo info, IPlugin plugin)
        {
            var container = new PluginContainer(info, plugin);
            LoadedPlugins.Add(info.SystemID, container);
            return true;
        }

        #endregion

        /// <summary>
        /// Выгружает все загруженные плагины и очищает контейнер.
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
