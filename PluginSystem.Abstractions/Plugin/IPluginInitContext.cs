using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Abstractions.Plugin
{
    public interface IPluginInitContext
    {
        string Path { get; }
        PluginInfo Info { get; }
        IPluginContext Services { get; }

        void Register<T>(T instance) where T : class;

        void Register<TService, TImplementation>()
            where TService : class
            where TImplementation : TService, new();
    }
}
