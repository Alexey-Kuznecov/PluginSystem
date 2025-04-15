
namespace PluginSystem.Core
{
    using System;
    using System.IO;
    using System.Text.Json;

    public class JsonPluginSettingsService : IPluginSettingsService
    {
        private readonly string _basePath;
        private readonly JsonSerializerOptions _options;

        public JsonPluginSettingsService(string? basePath = null)
        {
            _basePath = basePath ?? Path.Combine(AppContext.BaseDirectory, "Plugins", "");
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public T Load<T>(string pluginName) where T : new()
        {
            var path = GetSettingsPath(pluginName);

            try
            {
                if (File.Exists(path))
                {
                    var json = File.ReadAllText(path);
                    return JsonSerializer.Deserialize<T>(json, _options) ?? new T();
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"[Settings] Error loading '{pluginName}': {ex.Message}");
#endif
            }

            return new T(); // defaults
        }

        public void Save<T>(string pluginName, T settings)
        {
            var path = GetSettingsPath(pluginName);
            var dir = Path.GetDirectoryName(path);

            try
            {
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var json = JsonSerializer.Serialize(settings, _options);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"[Settings] Error saving '{pluginName}': {ex.Message}");
#endif
            }
        }

        private string GetSettingsPath(string pluginName)
        {
            var pluginDir = Path.Combine(AppContext.BaseDirectory, "Plugins", pluginName);
            return Path.Combine(pluginDir, "settings.json");
        }
    }
}
