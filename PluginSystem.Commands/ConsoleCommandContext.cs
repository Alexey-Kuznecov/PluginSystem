
using PluginSystem.Core;

namespace PluginSystem.Commands
{
    public class ConsoleCommandContext : ICommandContext
    {
        // Синглтон-инстанс
        public static ConsoleCommandContext Instance { get; } = new ConsoleCommandContext();

        // Параметры команды (можно подменять вручную для тестов)
        public ICommandParameters Parameters { get; set; } = new ConsoleCommandParameters();

        // Подключённый плагин (можно задать вручную при необходимости)
        public IPlugin Plugin { get; set; } = NullPlugin.Instance;

        // Информация о плагине
        public PluginInfo Info { get; set; } = new PluginInfo
        {
            DeveloperID = "Console",
            Name = "Console Host",
            Version = "1.0.0",
            Author = "System",
            Description = new Dictionary<string, string>()
        };

        // Контекст выполнения (например, для регистрации новых команд и т.д.)
        public IPluginContext RuntimeContext { get; set; } = NullPluginContext.Instance;

        // Целевой объект команды (опционально — в консоли можно оставить null)
        public object? Target { get; set; }

        private ConsoleCommandContext() { }

        public class NullPlugin : IPlugin
        {
            public static IPlugin Instance { get; } = new NullPlugin();

            public string Name => throw new NotImplementedException();

            public string Version => throw new NotImplementedException();

            public void Initialize(IPluginContext context) { }

            public void Shutdown()
            {
                throw new NotImplementedException();
            }
        }

        public class NullPluginContext : IPluginContext
        {
            public static IPluginContext Instance { get; } = new NullPluginContext();

            public List<IPluginCommand> Commands => throw new NotImplementedException();

            public IPluginSettingsService SettingsService => throw new NotImplementedException();

            public ILogger Logger => throw new NotImplementedException();

            public void Cleanup()
            {
                throw new NotImplementedException();
            }

            public void ExecuteCommand(IPluginCommand command, ICommandContext context)
            {
                throw new NotImplementedException();
            }

            public T? GetService<T>() where T : class
            {
                throw new NotImplementedException();
            }

            public IEnumerable<T> GetServices<T>() where T : class
            {
                throw new NotImplementedException();
            }

            public void Register<T>(T instance) where T : class
            {
                throw new NotImplementedException();
            }

            public void RegisterDisposable(IDisposable disposable)
            {
                throw new NotImplementedException();
            }

            public void RegisterEventHandler(Delegate handler)
            {
                throw new NotImplementedException();
            }

            public void RegisterSetting(object setting)
            {
                throw new NotImplementedException();
            }

            public void UnregisterAll()
            {
                throw new NotImplementedException();
            }

            public void UnregisterEventHandler(Delegate handler)
            {
                throw new NotImplementedException();
            }
        }
    }
}
