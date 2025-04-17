using PluginSystem.Core.PluginSystem.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSystem.Abstractions.Plugin;

namespace PluginSystem.Core
{
    public class BindableProperty<T>
    {
        private T _value;
        private readonly List<EventHandler<PropertyChangedEventArgs>> _handlers = new();
        private readonly IPluginContext _context;

        public BindableProperty(T initialValue, IPluginContext context)
        {
            _value = initialValue;
            _context = context;
        }

        public T Value
        {
            get => _value;
            set
            {
                if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        public event EventHandler<PropertyChangedEventArgs>? PropertyChanged
        {
            add
            {
                if (value != null)
                {
                    _handlers.Add(value);
                    _context.RegisterEventHandler(value); // регистрируем в контексте
                }
            }
            remove
            {
                if (value != null)
                {
                    _handlers.Remove(value);
                    _context.UnregisterEventHandler(value);
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            foreach (var handler in _handlers.ToList())
                handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
