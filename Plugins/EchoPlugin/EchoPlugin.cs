using PluginSystem.Core;

namespace EchoPlugin
{
    public class EchoPlugin : IPlugin
    {
        public string Name => "EchoPlugin";

        public string Version => "1.0";

        public void Initialize(IPluginContext context)
        {
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }
    }
}
