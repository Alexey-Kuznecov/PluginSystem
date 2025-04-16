using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Loader;

    public static class PluginUnloadInspector
    {
        /// <summary>
        /// Проверяет выгрузку контекста и возвращает активные сборки, связанные с плагином.
        /// </summary>
        /// <param name="contextWeakRef">Слабая ссылка на PluginLoadContext.</param>
        /// <param name="pluginAssemblyNamePart">Часть имени плагин-сборки (например, "ClockPlugin").</param>
        /// <returns>Список названий сборок, которые ещё не выгрузились.</returns>
        public static List<string> Inspect(WeakReference contextWeakRef, string pluginAssemblyNamePart)
        {
            ForceGC(); // Принудительная сборка мусора

            var result = new List<string>();

            if (contextWeakRef.IsAlive)
            {
                Console.WriteLine("⚠️ Контекст плагина всё ещё жив. Поиск загруженных сборок...");

                var loaded = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic && a.FullName?.Contains(pluginAssemblyNamePart, StringComparison.OrdinalIgnoreCase) == true)
                    .Select(a => a.FullName!)
                    .ToList();

                if (loaded.Count == 0)
                {
                    Console.WriteLine("✅ Сборки не найдены — возможно, проблема в другом.");
                }
                else
                {
                    Console.WriteLine("🧷 Найдены живые сборки:");
                    foreach (var asm in loaded)
                    {
                        Console.WriteLine(" └─ " + asm);
                    }
                    result.AddRange(loaded);
                }
            }
            else
            {
                Console.WriteLine("✅ Контекст плагина успешно выгружен.");
            }

            return result;
        }

        /// <summary>
        /// Принудительный GC + финализация для попытки выгрузки.
        /// </summary>
        private static void ForceGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }

}
