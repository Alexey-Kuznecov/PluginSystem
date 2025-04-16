using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSystem.Core;

namespace EchoPlugin
{
    public class EchoFactory : IPluginFactory
    {
        public IPlugin CreatePlugin(IPluginInitContext container)
        {
            return new EchoPlugin();
        }

        public PluginInfo GetPluginInfo(PluginInfo info)
        {
            info.Name = "EchoPlugin";
            info.Version = "1.0.0";
            info.Author = "UnityCommander Team";
            info.DeveloperID = "echoplugin-1.0.0";
            info.DocumentationPath = "EchoPlugin";
            return info;
        }
    }
}
