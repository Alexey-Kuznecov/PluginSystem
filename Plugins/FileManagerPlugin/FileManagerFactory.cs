
using PluginSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerPlugin
{
    internal class FileManagerFactory : IPluginFactory
    {
        public IPlugin CreatePlugin()
        {
            return new FileManagerPlugin();
        }

        public PluginInfo GetPluginInfo(PluginInfo info)
        {
            info.Name = "FileManagerPlugin";
            info.Version = "1.0.0";
            info.Author = "UnityCommander Team";
            info.DeveloperID = "filemanagerplugin-1.0.0";
            info.DocumentationPath = "FileManagerPlugin";
            return info;
        }
    }
}
