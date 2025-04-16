using PluginSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClockPlugin
{
    public class BindableClockSettings
    {
        public BindableProperty<string> ClockFormat { get; }
        public BindableProperty<bool> ShowDate { get; }
        public BindableProperty<bool> Use24HourFormat { get; }
        public BindableProperty<string> TimeZone { get; }

        public BindableClockSettings(ClockPluginSettings settings, IPluginContext context)
        {
            ClockFormat = new BindableProperty<string>(settings.ClockFormat, context);
            ShowDate = new BindableProperty<bool>(settings.ShowDate, context);
            Use24HourFormat = new BindableProperty<bool>(settings.Use24HourFormat, context);
            TimeZone = new BindableProperty<string>(settings.TimeZone, context);
        }

        public ClockPluginSettings ToModel() => new()
        {
            ClockFormat = ClockFormat.Value,
            ShowDate = ShowDate.Value,
            Use24HourFormat = Use24HourFormat.Value,
            TimeZone = TimeZone.Value
        };
    }
}
