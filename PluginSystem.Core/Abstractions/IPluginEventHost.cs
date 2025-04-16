using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Core.Abstractions.Events;

public interface IPluginEventHost
{
    void RegisterEventHandler(Delegate handler);
    void UnregisterEventHandler(Delegate handler);
}