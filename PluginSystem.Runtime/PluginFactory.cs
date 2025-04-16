﻿
namespace PluginSystem.Runtime
{
    using PluginSystem.Core;
    using PluginSystem.Core.PluginSystem.Core;
    using System.Xml.Linq;

    public class PluginFactory : IPluginFactory
    {
        private readonly Type _pluginType;

        public PluginFactory(Type pluginType)
        {
            _pluginType = pluginType;
        }

        public IPlugin CreatePlugin(IPluginInitContext context)
        {
            var plugin = (IPlugin)Activator.CreateInstance(_pluginType)!;
            plugin.Initialize(context.Services);
            return plugin;
        }

        public PluginInfo GetPluginInfo(PluginInfo info)
        {
            info.Name ??= _pluginType.Name;
            info.Version ??= "1.0.0";
            info.Author ??= "Unknown";
            info.DeveloperID ??= _pluginType.FullName?.ToLowerInvariant();
            info.DocumentationPath ??= _pluginType.Assembly.FullName ?? "Unknown";
            return info;
        }
    }
}