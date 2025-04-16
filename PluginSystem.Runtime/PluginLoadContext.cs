using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Runtime
{
    using PluginSystem.Core;
    using PluginSystem.Core.Utilities;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Loader;

    public class PluginLoadContext : AssemblyLoadContext, IPluginLoadContext
    {
        private readonly string _pluginDirectory;

        public PluginLoadContext(string pluginPath, bool isCollectible = true)
            : base(name: Path.GetFileName(pluginPath), isCollectible: isCollectible)
        {
            _pluginDirectory = Path.GetDirectoryName(pluginPath)!;
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            var dependencyPath = Path.Combine(_pluginDirectory, $"{assemblyName.Name}.dll");
            if (File.Exists(dependencyPath))
            {
                return LoadFromAssemblyPath(dependencyPath);
            }

            return null;
        }

        public void Unload()
        {
            var context = new PluginLoadContext(_pluginDirectory);
            var weakContext = new WeakReference(context);
            // Сбрасываем ссылки, чтобы выгрузить сборку из памяти
            base.Unload();
            PluginUnloadInspector.Inspect(weakContext, "ClockPlugin");
        }
    }

}
