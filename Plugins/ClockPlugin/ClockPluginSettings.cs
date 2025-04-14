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

    public class ClockPluginSettings : INotifyPropertyChanged
    {
        private string _clockFormat = "HH:mm:ss";
        private bool _showDate = true;
        private bool _use24HourFormat = true;
        private string _timeZone = "Local";

        public string ClockFormat
        {
            get => _clockFormat;
            set { if (_clockFormat != value) { _clockFormat = value; OnPropertyChanged(nameof(ClockFormat)); } }
        }

        public bool ShowDate
        {
            get => _showDate;
            set { if (_showDate != value) { _showDate = value; OnPropertyChanged(nameof(ShowDate)); } }
        }

        public bool Use24HourFormat
        {
            get => _use24HourFormat;
            set { if (_use24HourFormat != value) { _use24HourFormat = value; OnPropertyChanged(nameof(Use24HourFormat)); } }
        }

        public string TimeZone
        {
            get => _timeZone;
            set { if (_timeZone != value) { _timeZone = value; OnPropertyChanged(nameof(TimeZone)); } }
        }

        public List<Reminder> Reminders { get; set; } = new();
        public HotkeySettings Hotkeys { get; set; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
