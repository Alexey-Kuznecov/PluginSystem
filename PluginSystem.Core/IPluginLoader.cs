using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Core
{
    public interface IPluginLoader
    {
        // Загружает все плагины из заданной директории
        IEnumerable<IPluginFactory> LoadAllPlugins(string directoryPath);

        // Загружает один плагин по имени (или ID, как угодно)
        IPluginFactory? LoadPlugin(string name);

        // Проверяет, можно ли загрузить плагин по пути
        bool CanLoadPlugin(string pluginPath);

        // Освобождает ресурсы / выгружает плагины, если требуется
        void UnloadPlugin(IPlugin plugin);

        // Возвращает метаданные плагинов без полной загрузки
        PluginInfo? GetMetadata(string pluginPath);
    }
}
