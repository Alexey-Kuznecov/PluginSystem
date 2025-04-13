
namespace PluginSystem.Core
{
    public interface IPlugin
    {
        string Name { get; }
        string Version { get; }
        void Initialize(IPluginContext context);
        void Shutdown();
    }
}
 