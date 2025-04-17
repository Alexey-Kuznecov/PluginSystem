using PluginSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSystem.Abstractions.Plugin;

namespace PluginSystem.Runtime
{
    public static class PluginInfoExtensions
    {
        public static void EnsureSystemId(this PluginInfo info)
        {
            if (!string.IsNullOrWhiteSpace(info.SystemID))
                return;

            var seed = !string.IsNullOrWhiteSpace(info.DeveloperID)
                ? info.DeveloperID
                : $"{info.Name}-{info.Version}-{info.Author}";

            info.SystemID = PluginHelper.GenerateReadablePluginId(seed).ToString();
        }
    }
}
