
using System.Collections;
using PluginSystem.Abstractions.Commands;

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

        void IConsoleCommandRegistry.Register<T>(string name, T instance)
        {
            throw new NotImplementedException();
        }
    }
}