
using PluginSystem.Abstractions.Plugin;

namespace PluginSystem.Runtime
{
    using PluginSystem.Core;
    using System;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Утилита для извлечения метаданных плагина на основе атрибута <see cref="PluginMetadataAttribute"/>.
    /// Позволяет получить информацию о плагине без его непосредственного создания.
    /// </summary>
    public static class PluginMetadataResolver
    {
        /// <summary>
        /// Пытается найти в сборке тип с атрибутом <see cref="PluginMetadataAttribute"/> и извлечь из него <see cref="PluginInfo"/>.
        /// </summary>
        /// <param name="assembly">Загруженная сборка плагина.</param>
        /// <returns>Объект <see cref="PluginInfo"/> с метаданными или <c>null</c>, если атрибут не найден.</returns>
        public static PluginInfo? Resolve(Assembly assembly)
        {
            var pluginType = assembly.GetTypes()
                .FirstOrDefault(t => typeof(IPlugin).IsAssignableFrom(t) &&
                                     t.GetCustomAttribute<PluginMetadataAttribute>() != null);

            if (pluginType == null)
                return null;

            var attr = pluginType.GetCustomAttribute<PluginMetadataAttribute>()!;
            return new PluginInfo
            {
                Name = attr.Name,
                Version = attr.Version,
                Author = attr.Author,
                DeveloperID = attr.DeveloperID ?? string.Empty,
                DocumentationPath = attr.DocumentationPath,
                Description = string.IsNullOrWhiteSpace(attr.Description)
                    ? null
                    : new() { ["en"] = attr.Description }
            };
        }
    }
}
