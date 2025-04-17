using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Abstractions.Services;

public interface IPluginEventHost
{
    void RegisterEventHandler(Delegate handler);
    void UnregisterEventHandler(Delegate handler);
}