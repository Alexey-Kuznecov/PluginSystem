using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Core
{
    using System;
    using System.Collections.Generic;

    namespace PluginSystem.Core
    {
        /// <summary>
        /// Управляет жизненным циклом плагинов: загрузкой, выгрузкой, регистрацией и получением информации.
        /// </summary>
        public interface IPluginManager
        {
            /// <summary>
            /// Загружает плагин по имени.
            /// </summary>
            /// <param name="name">Имя или идентификатор плагина.</param>
            /// <returns>true, если плагин успешно загружен.</returns>
            bool LoadPlugin(string name);

            /// <summary>
            /// Загружает все доступные плагины.
            /// </summary>
            void LoadAllPlugins();

            /// <summary>
            /// Перезагружает плагин.
            /// </summary>
            /// <param name="name">Имя плагина.</param>
            /// <returns>true, если успешно.</returns>
            bool ReloadPlugin(string name);

            /// <summary>
            /// Выгружает плагин по имени.
            /// </summary>
            /// <param name="name">Имя плагина.</param>
            /// <returns>true, если успешно выгружен.</returns>
            bool UnloadPlugin(string name);

            /// <summary>
            /// Выгружает все загруженные плагины.
            /// </summary>
            void UnloadAll();

            /// <summary>
            /// Проверяет, загружен ли плагин.
            /// </summary>
            /// <param name="name">Имя плагина.</param>
            /// <returns>true, если загружен.</returns>
            bool IsPluginLoaded(string name);

            /// <summary>
            /// Получает экземпляр плагина.
            /// </summary>
            /// <param name="name">Имя плагина.</param>
            /// <returns>Экземпляр плагина или null.</returns>
            IPlugin? GetPlugin(string name);

            /// <summary>
            /// Получает метаинформацию о плагине.
            /// </summary>
            /// <param name="name">Имя плагина.</param>
            /// <returns>Информация о плагине или null.</returns>
            PluginInfo? GetPluginInfo(string name);

            /// <summary>
            /// Получает список всех загруженных плагинов.
            /// </summary>
            /// <returns>Список плагинов.</returns>
            IEnumerable<IPlugin> GetAllPlugins();

            /// <summary>
            /// Регистрирует плагин вручную (в обход автозагрузки).
            /// </summary>
            /// <param name="plugin">Плагин для регистрации.</param>
            void RegisterPlugin(IPlugin plugin);

            /// <summary>
            /// Событие, возникающее при успешной загрузке плагина.
            /// </summary>
            event Action<IPlugin> PluginLoaded;

            /// <summary>
            /// Событие, возникающее при выгрузке плагина.
            /// </summary>
            event Action<IPlugin> PluginUnloaded;
        }
    }
}
