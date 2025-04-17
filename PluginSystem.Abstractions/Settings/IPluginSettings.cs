using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Abstractions.Settings
{
    public interface IPluginSettings<T> : IDisposable where T : class, INotifyPropertyChanged, new()
    {
        T Value { get; }
        void Save();
        void Update(Action<T> updateAction);
        void Delete();
    }
}
