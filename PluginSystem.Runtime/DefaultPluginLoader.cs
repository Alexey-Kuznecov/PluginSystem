using PluginSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Runtime
{
    public class DefaultPluginLoader // :  IPluginLoader
    {
        private readonly IEnumerable<IPluginFactory> _pluginFactories;

        public DefaultPluginLoader(IEnumerable<IPluginFactory> pluginFactories)
        {
            _pluginFactories = pluginFactories;
        }

        public IEnumerable<IPluginFactory> LoadAllPlugins(string directoryPath)
        {
            var factories = new List<IPluginFactory>();

            if (!Directory.Exists(directoryPath))
                return factories;

            foreach (var file in Directory.GetFiles(directoryPath, "*.dll"))
            {
                if (CanLoadPlugin(file))
                {
                    var factory = LoadFactoryFromAssembly(file);
                    if (factory != null)
                        factories.Add(factory);
                }
            }

            return factories;
        }

        public IPluginFactory? LoadPlugin(string path)
        {
            if (!CanLoadPlugin(path))
                return null;

            return LoadFactoryFromAssembly(path);
        }

        public bool CanLoadPlugin(string pluginPath)
        {
            try
            {
                var assembly = Assembly.LoadFrom(pluginPath);
                return PluginMetadataResolver.Resolve(assembly) != null;
            }
            catch
            {
                return false;
            }
        }

        public void UnloadPlugin(IPlugin plugin)
        {
            // В этой базовой реализации ничего не делаем.
            // В дальнейшем можно реализовать выгрузку через AssemblyLoadContext.
        }

        public PluginInfo? GetMetadata(string pluginPath)
        {
            try
            {
                var assembly = Assembly.LoadFrom(pluginPath);
                return PluginMetadataResolver.Resolve(assembly);
            }
            catch
            {
                return null;
            }
        }

        private IPluginFactory? LoadFactoryFromAssembly(string path)
        {
            try
            {
                var assembly = Assembly.LoadFrom(path);
                var pluginType = assembly.GetTypes().FirstOrDefault(t => typeof(IPluginFactory).IsAssignableFrom(t) && !t.IsAbstract);

                if (pluginType == null)
                    return null;

                var plugin = (IPluginFactory)Activator.CreateInstance(pluginType)!;

                return plugin;
            }
            catch
            {
                return null;
            }
        }
    }
}
