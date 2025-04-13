
namespace PluginSystem.Runtime
{
    using PluginSystem.Core;

    public class PluginFactory : IPluginFactory
    {
        private readonly Type _pluginType;

        public PluginFactory(Type pluginType)
        {
            _pluginType = pluginType;
        }

        public IPlugin CreatePlugin()
        {
            return (IPlugin)Activator.CreateInstance(_pluginType)!;
        }

        public PluginInfo GetPluginInfo(PluginInfo info)
        {
            return info;
        }
    }
}
