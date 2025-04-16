using PluginSystem.Core;
using PluginSystem.Core.PluginSystem.Core;

namespace EchoPlugin
{
    public class EchoPlugin : IPlugin, IPluginUnloadable
    {
        private IPluginContext _context;
        public string Name => "EchoPlugin";
        public string Version => "0.5";
        public void Initialize(IPluginContext context)
        {
            _context = context;
            // Register commands or perform initialization tasks here
        }

        public void OnUnload()
        {
            //throw new NotImplementedException();
        }

        public void Shutdown()
        {
            // Perform cleanup tasks here
        }
    }
}
