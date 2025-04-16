using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Core.Utilities
{
    public static class PluginUnloadDebugger
    {
        private static readonly Dictionary<string, WeakReference> _pluginReferences = new();

        /// <summary>
        /// Создает слабую ссылку на PluginLoadContext и следит за его выгрузкой.
        /// </summary>
        public static void MonitorUnload(IPluginLoadContext loadContext, string pluginId)
        {
            if (loadContext == null || string.IsNullOrEmpty(pluginId))
                return;

            var weakRef = new WeakReference(loadContext, trackResurrection: true);
            _pluginReferences[pluginId] = weakRef;

            Console.WriteLine($"[UnloadDebug] Начат мониторинг плагина: {pluginId}");
        }

        /// <summary>
        /// Проверяет, выгрузился ли плагин.
        /// </summary>
        public static void CheckUnloaded(string pluginId)
        {
            if (_pluginReferences.TryGetValue(pluginId, out var weakRef))
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                if (weakRef.IsAlive)
                {
                    Console.WriteLine($"⚠️ [UnloadDebug] Плагин [{pluginId}] все еще жив в памяти.");
                }
                else
                {
                    Console.WriteLine($"✅ [UnloadDebug] Плагин [{pluginId}] успешно выгружен.");
                    _pluginReferences.Remove(pluginId);
                }
            }
            else
            {
                Console.WriteLine($"[UnloadDebug] Нет информации по плагину: {pluginId}");
            }
        }

        /// <summary>
        /// Проверяет все отслеживаемые плагины.
        /// </summary>
        public static void CheckAll()
        {
            foreach (var kvp in _pluginReferences.Keys.ToList())
            {
                CheckUnloaded(kvp);
            }
        }
    }
}
