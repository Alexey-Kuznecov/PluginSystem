
using NLog.Config;
using PluginSystem.Core;
using PluginSystem.Core.PluginSystem.Core;
using PluginSystem.Core.Utilities;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Loader;
using System.Xml.Linq;
using PluginSystem.Abstractions.Commands;
using PluginSystem.Abstractions.Plugin;

namespace PluginSystem.Runtime
{
    public class PluginContainer : IPluginContainer, IDisposable
    {
        public string AssemblyPath { get; set; } // Путь к DLL-файлу плагина
        public IPluginLoadContext? LoadContext { get; set; } // Контекст загрузки плагина
        public Assembly? LoadedAssembly { get;  set; } // Загруженная сборка плагина
        public IPluginFactory Factory { get;  set; } // Фабрика плагина
        public PluginInfo PluginInfo { get;  set; } // Информация о плагине, включая имя, версию и т.д.
        public string? Name => PluginInfo.Name; // Имя плагина
        public Type PluginType { get;  set; } // Тип плагина
        public IPluginContext Context { get; set; } // Контекст плагина
        public IPlugin Plugin { get; set; } // Сам плагин

        public CommandManager CommandManager { get; } = new();
        public PluginContainer(
            string pluginPath,
            Assembly assembly,
            IPluginLoadContext loadContext,
            IPluginFactory factory,
            IPlugin plugin,
            IPluginContext context,
            PluginInfo pluginInfo)
        {
            AssemblyPath = pluginPath;
            LoadedAssembly = assembly;
            LoadContext = loadContext;
            Factory = factory;
            Plugin = plugin;
            Context = context;
            PluginInfo = pluginInfo;
            PluginType = Plugin.GetType();

            RegisterPluginCommands();
        }

        private void RegisterPluginCommands()
        {
            if (Plugin is not ICommandProvider provider)
                return;

            foreach (var command in provider.GetCommands())
            {
                CommandManager.RegisterCommand(command);
            }
        }

        public IPluginContext GetContext(IPlugin plugin) => Context;

        public IPlugin? GetPlugin(string name) =>
            PluginInfo.Name == name ? Plugin : null;

        public List<IPluginCommand> GetCommands()
        {
            if (Plugin is ICommandProvider provider)
                return provider.GetCommands().ToList();

            return new List<IPluginCommand>();
        }
        public void Clear()
        {
            Plugin?.Shutdown();
        }

        public void Unload()
        {
            //Context = null;
            //PluginType = null;
            //Factory = null;
            //PluginInfo = null;
            //LoadedAssembly = null;
            Plugin?.Shutdown();
            LoadContext?.Unload();
            //Plugin = null;
        }

        public void Dispose()
        {
            (LoadContext as IDisposable)?.Dispose();
            Plugin?.Shutdown();
        }
    }
}
