using PluginSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using PluginSystem.Abstractions.Plugin;

namespace PluginSystem.Runtime
{
    public class PluginPersistenceService
    {
        private readonly string _basePath;
        private readonly JsonSerializerOptions _options;

        public PluginPersistenceService(string? basePath = null)
        {
            _basePath = basePath ?? Path.Combine(AppContext.BaseDirectory, "Plugins");
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
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
                Console.WriteLine($"[PluginPersistence] Error saving '{info.SystemID}': {ex.Message}");
#endif
            }
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
                Console.WriteLine($"[PluginPersistence] Error loading '{systemId}': {ex.Message}");
#endif
            }

            return null;
        }

        public bool Exists(string systemId)
        {
            return File.Exists(GetInfoPath(systemId));
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
