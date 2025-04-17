
using PluginSystem.Abstractions.Plugin;


namespace PluginSystem.Abstractions.Plugin
{
    public interface IPluginFactory
    {
        IPlugin CreatePlugin(IPluginInitContext container);
        PluginInfo GetPluginInfo(PluginInfo info);
    }
}
