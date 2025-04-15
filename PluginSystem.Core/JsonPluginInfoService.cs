using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace PluginSystem.Core
{
    public class JsonPluginInfoService
    {
        private readonly string _basePath;
        private readonly JsonSerializerOptions _options;

        public JsonPluginInfoService(string? basePath = null)
        {
            _basePath = basePath ?? Path.Combine(AppContext.BaseDirectory, "Plugins", "");
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
        }

        public PluginInfo? Load(string systemId)
        {
            var path = GetInfoPath(systemId);

            try
            {
                if (File.Exists(path))
                {
                    var json = File.ReadAllText(path);
                    return JsonSerializer.Deserialize<PluginInfo>(json, _options);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"[PluginInfo] Error loading '{systemId}': {ex.Message}");
#endif
            }

            return null;
        }

        public void Save(PluginInfo info)
        {
            var path = GetInfoPath(info.SystemID);
            var dir = Path.GetDirectoryName(path);

            try
            {
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var json = JsonSerializer.Serialize(info, _options);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"[PluginInfo] Error saving '{info.SystemID}': {ex.Message}");
#endif
            }
        }

        public bool Exists(string systemId)
        {
            var path = GetInfoPath(systemId);
            return File.Exists(path);
        }

        private string GetInfoPath(string systemId)
        {
            var safeId = MakeSafeFileName(systemId);
            var pluginDir = Path.Combine(_basePath, safeId);
            return Path.Combine(pluginDir, "plugin.info.json");
        }

        private static string MakeSafeFileName(string name)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name;
        }
    }
}
