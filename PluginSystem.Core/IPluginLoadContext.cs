using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Core
{
    public interface IPluginLoadContext
    {
        public void Unload();
    }
}
