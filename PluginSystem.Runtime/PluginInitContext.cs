using PluginSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSystem.Abstractions.Plugin;

namespace PluginSystem.Runtime
{
    public class PluginInitContext : IPluginInitContext
    {
        public string Path { get; }
        public PluginInfo Info { get; }
        public IPluginContext Services => _pluginContext;

        private readonly PluginContext _pluginContext;

        public PluginInitContext(string pluginPath, PluginInfo info)
        {
            Path = pluginPath;
            Info = info ?? throw new ArgumentNullException(nameof(info));
            _pluginContext = new PluginContext(pluginPath);
        }

        public void Register<T>(T instance) where T : class
        {
            _pluginContext.Register(instance);
        }

        public void Register<TService, TImplementation>()
            where TService : class
            where TImplementation : TService, new()
        {
            var instance = new TImplementation();
            _pluginContext.Register<TService>(instance);
        }

        public PluginContext BuildPluginContext()
        {
            return _pluginContext;
        }
    }
}
