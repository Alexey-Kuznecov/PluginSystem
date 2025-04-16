

namespace PluginSystem.Core
{
    /// <summary>
    /// Описание метаданных плагина, используемое для регистрации и отображения информации.
    /// </summary>
    public class PluginInfo
    {
        /// <summary>
        /// Имя плагина.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Версия плагина.
        /// </summary>
        public string? Version { get; set; } = "1.0.0";

        /// <summary>
        /// Автор или разработчик.
        /// </summary>
        public string? Author { get; set; }

        /// <summary>
        /// Внутренний уникальный системный идентификатор плагина.
        /// Может быть сгенерирован при загрузке плагина.
        /// </summary>
        public string SystemID { get; set; }

        /// <summary>
        /// Человекочитаемый идентификатор плагина, задается вручную.
        /// </summary>
        public string DeveloperID { get; set; } = string.Empty;

        /// <summary>
        /// Путь к файлу документации плагина (например, Markdown).
        /// </summary>
        public string? DocumentationPath { get; set; }

        /// <summary>
        /// Мультиязычное описание плагина, ключ — код языка.
        /// </summary>
        public Dictionary<string, string>? Description { get; set; }

        /// <summary>
        /// URL или путь к файлу изменений (changelog).
        /// </summary>
        public object? ChangelogUrl { get; set; }

        /// <summary>
        /// Возвращает локализованное описание плагина.
        /// </summary>
        /// <param name="lang">Код языка, например "en", "ru".</param>
        /// <returns>Строка описания на нужном языке или fallback-значение.</returns>
        public string GetDescription(string lang = "en")
        {
            if (Description == null || Description.Count == 0)
                return string.Empty;

            return Description.TryGetValue(lang, out var localized)
                ? localized
                : Description.Values.FirstOrDefault() ?? string.Empty;
        }

        /// <summary>
        /// Заглушка для получения инструкции по использованию (можно заменить на загрузку из файла).
        /// </summary>
        public string GetInstructions(string langCode) =>
            "Инструкция пока не реализована.";
    }
} 
