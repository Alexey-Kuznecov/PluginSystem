
using System.Collections.ObjectModel;
using System.Windows.Input;
using PluginSystem.Core;
using PluginSystem.Core.PluginSystem.Core;
using PluginSystem.Runtime;

namespace PluginSystem.UI.ViewModels
{
    public class PluginListViewModel : BindableBase
    {
        private readonly IPluginManager _pluginManager;

        public ObservableCollection<PluginInfo> Plugins { get; } = new();

        private PluginInfo? _selectedPlugin;
        public PluginInfo? SelectedPlugin
        {
            get => _selectedPlugin;
            set => SetProperty(ref _selectedPlugin, value);
        }

        public ICommand EnableCommand { get; }
        public ICommand DisableCommand { get; }

        public PluginListViewModel(IPluginManager pluginManager)
        {
            _pluginManager = pluginManager;

            EnableCommand = new DelegateCommand(EnableSelectedPlugin, CanEnable)
                .ObservesProperty(() => SelectedPlugin);
            DisableCommand = new DelegateCommand(DisableSelectedPlugin, CanDisable)
                .ObservesProperty(() => SelectedPlugin);

            LoadPlugins();
        }

        private void LoadPlugins()
        {
            Plugins.Clear();
            foreach (var plugin in _pluginManager.LoadPlugin("dd"))
                Plugins.Add(plugin.Info);
        }

        private void EnableSelectedPlugin()
        {
            if (SelectedPlugin != null)
                _pluginManager.IsPluginLoaded(SelectedPlugin.SystemID);
        }

        private void DisableSelectedPlugin()
        {
            if (SelectedPlugin != null)
                _pluginManager.DisablePlugin(SelectedPlugin.SystemID);
        }

        private bool CanEnable() => SelectedPlugin != null;
        private bool CanDisable() => SelectedPlugin != null;
    }
}
