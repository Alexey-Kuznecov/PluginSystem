using PluginSystem.Abstractions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSystem.Hosting;
using PluginSystem.Core;
using PluginSystem.Abstractions.Settings;

namespace DemoFileManagerPlugin
{
    public class DemoCommandModule
    {
        private readonly IConsoleCommandRegistry _registry;
        private readonly IPluginSettings<DemoSettings> _settings;

        public DemoCommandModule(IConsoleCommandRegistry registry, IPluginSettings<DemoSettings> settings)
        {
            _registry = registry;
            _settings = settings;
        }

        public void RegisterCommands()
        {
            _registry.RegisterLambda("hello", "Печатает приветствие", (ctx, output) =>
            {
                output.WriteLine("Привет!");
            });

            _registry.Register("demo.settings.show", new ShowSettingsCommands(_settings));
        }
    }
}
