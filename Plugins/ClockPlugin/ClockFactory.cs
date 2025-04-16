
using PluginSystem.Core;
using PluginSystem.Core.PluginSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClockPlugin
{
    internal class ClockFactory : IPluginFactory
    {
        public IPlugin CreatePlugin(IPluginInitContext context)
        {
            return new ClockPlugin();
        }

        public PluginInfo GetPluginInfo(PluginInfo info)
        {
            info.Name = "ClockPlugin";
            info.Version = "1.0.0";
            info.Author = "UnityCommander Team";
            info.DeveloperID = "clockplugin-1.0.0";
            info.DocumentationPath = "ClockPlugin";
            return info;
        }
    }
}
