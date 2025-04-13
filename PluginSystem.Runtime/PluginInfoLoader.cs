
namespace PluginSystem.Runtime
{
    using PluginSystem.Core;
    using System.Text.Json;

    public static class PluginInfoLoader
    {
        public static PluginInfo? Load(string pluginFolderPath)
        {
            var path = Path.Combine(pluginFolderPath, "plugininfo.json");

            if (!File.Exists(path))
            {
                Console.WriteLine($"[WARN] plugininfo.json not found at: {path}");
                return null;
            }

            try
            {
                var json = File.ReadAllText(path);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var info = JsonSerializer.Deserialize<PluginInfo>(json, options);

                if (info == null || string.IsNullOrWhiteSpace(info.Name) || string.IsNullOrWhiteSpace(info.DeveloperID))
                {
                    Console.WriteLine($"[ERROR] PluginInfo is incomplete in: {path}");
                    return null;
                }

                return info;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to load plugin info: {ex.Message}");
                return null;
            }
        }
    }
}
