
namespace PluginSystem.Core
{
    public interface IPluginFactory
    {
        IPlugin CreatePlugin();

        PluginInfo GetPluginInfo(PluginInfo info); // Новый метод для получения инфорации о плагине
    }
}
