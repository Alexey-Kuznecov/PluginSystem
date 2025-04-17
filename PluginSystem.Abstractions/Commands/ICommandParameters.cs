
namespace PluginSystem.Abstractions.Commands
{
    public interface ICommandParameters
    {
        void Set(string key, object? value);
        T? Get<T>(string key);
        bool Contains(string key);
    }
}
