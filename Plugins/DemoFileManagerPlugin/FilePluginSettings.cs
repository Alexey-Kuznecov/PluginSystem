using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoFileManagerPlugin;

public class FilePluginSettings : INotifyPropertyChanged
{
    public string DefaultCopyDirectory { get; set; } = "C:\\";
    public bool EnableLogging { get; set; } = true;
    public int MaxOperationThreads { get; set; } = 4;

    public event PropertyChangedEventHandler? PropertyChanged;
}

