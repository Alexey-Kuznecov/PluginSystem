using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.UI
{
    using Prism.Ioc;
    using Prism.Modularity;
    using global::PluginSystem.UI.View;
    using global::PluginSystem.UI.ViewModels;

    public class PluginSystemUIModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider) { }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterForNavigation<PluginListView, PluginListViewModel>();
            //containerRegistry.RegisterForNavigation<PluginDetailsView, PluginDetailsViewModel>();
        }
    }
}
