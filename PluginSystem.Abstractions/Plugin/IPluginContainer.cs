using System.Reflection;

namespace PluginSystem.Abstractions.Plugin
{
    public interface IPluginContainer
    {
        #region Выгрузка плагина

        public string AssemblyPath { get; }
        public Assembly? LoadedAssembly { get; set; }
        public IPluginLoadContext? LoadContext { get; set; }

        #endregion

        public PluginInfo PluginInfo => new PluginInfo(); // Информация о плагине, включая имя, версию и т.д.
        public IPluginContext Context { get; } // Контекст плагина
        public IPlugin Plugin { get; } // Сам плагин

        void Unload();
    }
}