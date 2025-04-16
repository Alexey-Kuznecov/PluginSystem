using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Core.Abstractions.Lifecycle;

public interface IPluginResourceTracker
{
    void RegisterDisposable(IDisposable disposable);
    void UnregisterAll();
    void Cleanup();
}
