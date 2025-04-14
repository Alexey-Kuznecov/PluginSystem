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
        /// Заглушка плагина, содержащая метаданные и отложенную ссылку на загрузку плагина.
        /// </summary>
        public class PluginStub
        {
            /// <summary>
            /// Метаданные плагина (имя, автор, версия и т.п.)
            /// </summary>
            public PluginMetadata Metadata { get; }

            /// <summary>
            /// Отложенная загрузка экземпляра плагина.
            /// </summary>
            public Lazy<IPlugin> Plugin { get; }

            public PluginStub(PluginMetadata metadata, Func<IPlugin> pluginFactory)
            {
                Metadata = metadata;
                Plugin = new Lazy<IPlugin>(pluginFactory);
            }
        }
    }
}
