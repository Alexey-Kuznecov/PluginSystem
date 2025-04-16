using PluginSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Runtime
{
    public class LoadedPlugin
    {
        public PluginInfo Info { get; }
        public PluginLoadContext Context { get; }
        public Assembly Assembly { get; }
        public IPlugin Instance { get; }

        public LoadedPlugin(PluginInfo info, PluginLoadContext context, Assembly assembly, IPlugin instance)
        {
            Info = info;
            Context = context;
            Assembly = assembly;
            Instance = instance;
        }
    }
}
