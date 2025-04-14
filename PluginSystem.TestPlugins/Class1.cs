
using PluginSystem.Core;
using PluginSystem.Runtime;

namespace PluginSystem.TestPlugins
{
    // Плагин с ошибкой в обработке команд
    public class FaultyPlugin : IPlugin
    {
        public PluginInfo Info { get; } = new PluginInfo
        {
            Name = "FaultyPlugin",
            Version = "1.0.0",
            Author = "Test Author",
            SystemID = PluginHelper.GeneratePluginId(new[] { "CommandFaultyPlugin", "1.0.0" })
        };

        public string Name => throw new NotImplementedException();

        public string Version => throw new NotImplementedException();

        public void Initialize(IPluginContext context)
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            // Логика загрузки плагина, но с искусственной ошибкой
            Console.WriteLine("FaultyPlugin: Load started");

            // Эмуляция ошибки при загрузке
            throw new InvalidOperationException("Ошибка при загрузке плагина.");
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        public void Unload()
        {
            // Логика выгрузки плагина
            Console.WriteLine("FaultyPlugin: Unloaded");
        }
    }

    // Плагин с ошибкой в обработке команды
    public class CommandFaultyPlugin : IPlugin
    {
        public PluginInfo Info { get; } = new PluginInfo
        {
            Name = "CommandFaultyPlugin",
            Version = "1.0.0",
            Author = "Test Author",
            SystemID = PluginHelper.GeneratePluginId(new[] { "CommandFaultyPlugin", "1.0.0" })
        };

        public string Name => throw new NotImplementedException();

        public string Version => throw new NotImplementedException();

        public void Initialize(IPluginContext context)
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            Console.WriteLine("CommandFaultyPlugin: Load started");

            // Регистрация команды с ошибкой
            CommandManager.RegisterCommand("faulty_command", args =>
            {
                throw new ArgumentException("Ошибка при выполнении команды.");
            });
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        public void Unload()
        {
            Console.WriteLine("CommandFaultyPlugin: Unloaded");
        }
    }
}
