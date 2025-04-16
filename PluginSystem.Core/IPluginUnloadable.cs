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
        /// Интерфейс для плагинов, которые хотят участвовать в процессе выгрузки.
        /// Позволяет освободить ресурсы, отписаться от событий и удалить живые ссылки.
        /// </summary>
        public interface IPluginUnloadable
        {
            /// <summary>
            /// Вызывается перед выгрузкой сборки плагина.
            /// Здесь необходимо освободить все ресурсы, чтобы не помешать выгрузке AssemblyLoadContext.
            /// </summary>
            void OnUnload();
        }
    }
}
