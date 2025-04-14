using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Core
{
    using System;

    /// <summary>
    /// Атрибут, который позволяет указать метаданные плагина без необходимости его активации.
    /// Используется загрузчиком для получения информации до инициализации плагина.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class PluginMetadataAttribute : Attribute
    {
        /// <summary>Название плагина.</summary>
        public string? Name { get; set; }

        /// <summary>Версия плагина.</summary>
        public string? Version { get; set; }

        /// <summary>Имя автора.</summary>
        public string? Author { get; set; }

        /// <summary>Описание плагина.</summary>
        public string? Description { get; set; }

        /// <summary>Внешний человекочитаемый идентификатор.</summary>
        public string? DeveloperID { get; set; }

        /// <summary>Путь к файлу с документацией (Markdown).</summary>
        public string? DocumentationPath { get; set; }
    }
}
