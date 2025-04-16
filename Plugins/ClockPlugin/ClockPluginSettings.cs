using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClockPlugin
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public class ClockPluginSettings
    {
        public string ClockFormat { get; set; } = "HH:mm:ss";
        public bool ShowDate { get; set; } = true;
        public bool Use24HourFormat { get; set; } = true;
        public string TimeZone { get; set; } = "Local";

        public List<Reminder> Reminders { get; set; } = new();
        public HotkeySettings Hotkeys { get; set; } = new();
    }

    public class Reminder
    {
        public string Time { get; set; } = "15:00";
        public string Message { get; set; } = "Сделать перерыв";
    }

    public class HotkeySettings
    {
        public string OpenSettings { get; set; } = "Ctrl+Alt+C";
        public string ShowTimePopup { get; set; } = "Ctrl+Shift+T";
    }
}
