using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Hosting.ConsoleCommands
{
    public interface IAutoCompleteProvider
    {
        IEnumerable<string> GetSuggestions(string input);
    }
}
