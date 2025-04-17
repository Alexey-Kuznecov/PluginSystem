
using PluginSystem.Abstractions.Plugin;

namespace PluginSystem.Core
{
    public interface IIdentifiablePlugin : IPlugin
    {
        public Guid SystemID { get; }
    }
}
