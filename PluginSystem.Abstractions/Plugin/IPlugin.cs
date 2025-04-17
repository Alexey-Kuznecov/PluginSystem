
namespace PluginSystem.Abstractions.Plugin
{
    public interface IPlugin
    {
        string Name { get; }
        string Version { get; }
        void Initialize(IPluginContext context);
        void Shutdown();
    }
}
 