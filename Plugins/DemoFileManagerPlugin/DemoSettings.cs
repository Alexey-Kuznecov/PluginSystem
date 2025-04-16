using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoFileManagerPlugin
{
    using System.ComponentModel;

    public class DemoSettings : INotifyPropertyChanged
    {
        private bool _showHiddenFiles;
        private string _sortMode = "Name";

        public bool ShowHiddenFiles
        {
            get => _showHiddenFiles;
            set
            {
                if (_showHiddenFiles != value)
                {
                    _showHiddenFiles = value;
                    OnPropertyChanged(nameof(ShowHiddenFiles));
                }
            }
        }

        public string SortMode
        {
            get => _sortMode;
            set
            {
                if (_sortMode != value)
                {
                    _sortMode = value;
                    OnPropertyChanged(nameof(SortMode));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
