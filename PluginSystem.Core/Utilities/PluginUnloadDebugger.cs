using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Core.Utilities
{
    public static class PluginUnloadDebugger
    {
        public static void MonitorUnload(IPluginLoadContext loadContext, string pluginId)
        {
            var weakRef = new WeakReference(loadContext);

            // Принудительный сбор мусора
            for (int i = 0; i < 10 && weakRef.IsAlive; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            if (weakRef.IsAlive)
            {
                Console.WriteLine($"⚠️ Плагин [{pluginId}] не выгружен полностью. Возможно, остались живые ссылки.");
            }
            else
            {
                Console.WriteLine($"✅ Плагин [{pluginId}] успешно выгружен.");
            }
        }
    }
}
