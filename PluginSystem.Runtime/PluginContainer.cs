
using PluginSystem.Core;
using PluginSystem.Core.PluginSystem.Core;

namespace PluginSystem.Runtime
{
    /// <summary>
    /// Контейнер для хранения и управления плагином.
    /// </summary>
    public class PluginContainer : IPluginContainer
    {
        public PluginInfo PluginInfo { get; private set; } // Информация о плагине
        public string? Name { get; private set; } // Имя контейнера (категория)
        public Type PluginType { get; private set; } // Тип плагина
        public IPluginContext Context { get; private set; } // Контекст плагина
        public IPlugin Plugin { get; private set; } // Сам плагин
        private CommandManager CommandManager { get; set; } // Экземпляр CommandManager

        public PluginContainer(PluginInfo info, IPlugin plugin)
        {
            var context = new PluginContext(plugin.GetType().Assembly.Location);
            CommandManager = new CommandManager(); // Инициализация экземпляра CommandManager
            PluginInfo = info;
            Name = info.Name; // Используем имя из метаданных плагина
            PluginType = plugin.GetType();
            Context = context; // Инициализируем контекст (или получаем из внешнего источника)
            Plugin = plugin; // Сохраняем плагин
            plugin.Initialize(context); // Инициализация плагина
        }

        public List<IPluginCommand> GetCommands()
        {
            var commands = new List<IPluginCommand>();

            // Проверяем, реализует ли плагин интерфейс ICommandProvider
            if (Plugin is ICommandProvider provider)
            {
                commands = provider.GetCommands().ToList();

                // Регистрируем все команды в CommandManager
                foreach (var command in commands)
                {
                    // Преобразуем IPluginContext в ICommandContext, если это необходимо
                    if (Context is ICommandContext commandContext)
                    {
                        CommandManager.RegisterCommand(command);

                        // Вызываем ExecuteCommand в контексте плагина
                        Context.ExecuteCommand(command, commandContext);
                    }
                    else
                    {
                        // Логирование ошибки или обработка случая, если Context не является ICommandContext
                        Console.WriteLine("Context не является ICommandContext.");
                    }
                }
            }

            return commands;
        }

        public IPluginContext GetContext(IPlugin plugin) => Context; // Возвращаем контекст плагина

        public IPlugin? GetPlugin(string name) => Plugin; // Возвращаем текущий плагин

        /// <summary>
        /// Очищает контейнер, удаляя плагин.
        /// </summary>
        public void Clear()
        {
            Plugin?.Shutdown(); // Вызываем завершение работы плагина
            Plugin = null; // Убираем ссылку на плагин
        }

    }
}
