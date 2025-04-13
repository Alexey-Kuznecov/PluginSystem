using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Core
{
    // Один пункт меню
    public class PluginMenuItem
    {
        public string Label { get; set; } = string.Empty;
        public Action Action { get; set; } = () => { };
    }
}
