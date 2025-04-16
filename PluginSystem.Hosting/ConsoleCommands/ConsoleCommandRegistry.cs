using PluginSystem.Core.Abstractions;
using System.Collections;

namespace PluginSystem.Hosting.ConsoleCommands
{
    internal class ConsoleCommandRegistry : IConsoleCommandRegistry
    {
        //public void Register<T>(string command, T instance) where T : class
        //{
        //    throw new NotImplementedException();
        //}

        public IEnumerable GetRegisteredCommands()
        {
            throw new NotImplementedException();
        }

        public void Register<T>(string command, T instance)
        {
            throw new NotImplementedException();
        }
    }
}