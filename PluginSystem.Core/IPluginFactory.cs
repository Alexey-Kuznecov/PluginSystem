
using PluginSystem.Core.PluginSystem.Core;

namespace PluginSystem.Core
{
    public interface IPluginFactory
    {
        IPlugin CreatePlugin(IPluginInitContext container);
        PluginInfo GetPluginInfo(PluginInfo info);
    }
}
