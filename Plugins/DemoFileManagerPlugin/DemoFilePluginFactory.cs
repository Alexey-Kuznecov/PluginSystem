using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSystem.Abstractions.Plugin;
using PluginSystem.Core;

namespace DemoFileManagerPlugin
{
    internal class DemoFilePluginFactory : IPluginFactory
    {
        public IPlugin CreatePlugin(IPluginInitContext container)
        {
            return new DemoFilePlugin();
        }

        public PluginInfo GetPluginInfo(PluginInfo info)
        {
            info.Name = "DemoFileManager";
            info.Version = "1.0.0";
            info.Author = "UnityCommander Team";
            info.DeveloperID = "demofilemanager-1.0.0";
            info.DocumentationPath = "/Plugins/local.json";
            info.Description = new Dictionary<string, string>
            {
                ["en"] = "Demonstration file manager plugin",
                ["ru"] = "Демонстрационный файловый менеджер",
                ["de"] = "Demonstrations-Dateimanager-Plugin",
            };
            return info;
        }
    }
}
