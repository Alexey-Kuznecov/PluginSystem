
using PluginSystem.Abstractions.Commands;

namespace PluginSystem.Core
{
    public class SimpleCommandParameters : ICommandParameters
    {
        public Dictionary<string, object> Values { get; set; } = new();

        public bool Contains(string key)
        {
            throw new NotImplementedException();
        }

        public T? Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, object? value)
        {
            throw new NotImplementedException();
        }
    }
}
