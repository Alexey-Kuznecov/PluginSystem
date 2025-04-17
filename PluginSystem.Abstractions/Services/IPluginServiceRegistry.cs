using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Abstractions.Services;

public interface IPluginServiceRegistry
{
    T Load<T>(string pluginId) where T : class, new();
    void Save<T>(string pluginId, T settings) where T : class;
    void Delete(string pluginId);
    void Register<T>(T instance) where T : class;
    T? GetService<T>() where T : class;
    IEnumerable<T> GetServices<T>() where T : class;
}
