using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Core.Abstractions.Services;

public interface IPluginServiceRegistry
{
    void Register<T>(T instance) where T : class;
    T? GetService<T>() where T : class;
    IEnumerable<T> GetServices<T>() where T : class;
}
