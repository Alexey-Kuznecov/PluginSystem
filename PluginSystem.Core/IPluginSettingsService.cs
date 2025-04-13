
namespace PluginSystem.Core
{
    public interface IPluginSettingsService
    {
        T Load<T>(string pluginName) where T : new();
        void Save<T>(string pluginName, T settings);
    }
}
