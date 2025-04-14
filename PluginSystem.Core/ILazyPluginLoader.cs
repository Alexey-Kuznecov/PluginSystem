using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Core
{
    namespace PluginSystem.Core
    {
        /// <summary>
        /// Представляет загрузчик плагинов с поддержкой ленивой (отложенной) инициализации.
        /// Позволяет получать метаданные плагинов без полной загрузки и активировать их при необходимости.
        /// </summary>
        public interface ILazyPluginLoader
        {
            /// <summary>
            /// Получает список "заглушек" плагинов, содержащих только метаданные и ленивую ссылку на сам плагин.
            /// </summary>
            IEnumerable<PluginStub> LoadAllPluginStubs();

            /// <summary>
            /// Получает метаданные плагина без его загрузки.
            /// </summary>
            PluginMetadata GetMetadata(string pluginPath);

            /// <summary>
            /// Загружает плагин по имени, но откладывает инициализацию до момента обращения.
            /// </summary>
            Lazy<IPlugin> LoadPluginLazy(string name);
        }
    }
}
