namespace PluginSystem.Core.PluginSystem.Core
{
    public interface IPluginContainer
    {
        public PluginInfo PluginInfo => new PluginInfo(); // Информация о плагине, включая имя, версию и т.д.
        public IPluginContext Context { get; } // Контекст плагина
        public IPlugin Plugin { get; } // Сам плагин

        public void Clear();

        public IPluginContext GetContext(IPlugin plugin);
        
        public IPlugin? GetPlugin(string name);
    }
}