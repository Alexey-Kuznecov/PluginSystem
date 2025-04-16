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

    public class ClockPluginSettingsPropertyChanged : INotifyPropertyChanged
    {
        private string _clockFormat = "HH:mm";
        private bool _showDate = true;
        private bool _use24HourFormat = true;
        private string _timeZone = "UTC";
        private List<Reminder> _reminders = new();
        private HotkeySettings _hotkeys = new();

        public string ClockFormat
        {
            get => _clockFormat;
            set
            {
                if (_clockFormat != value)
                {
                    _clockFormat = value;
                    OnPropertyChanged(nameof(ClockFormat));
                }
            }
        }

        public bool ShowDate
        {
            get => _showDate;
            set
            {
                if (_showDate != value)
                {
                    _showDate = value;
                    OnPropertyChanged(nameof(ShowDate));
                }
            }
        }

        public bool Use24HourFormat
        {
            get => _use24HourFormat;
            set
            {
                if (_use24HourFormat != value)
                {
                    _use24HourFormat = value;
                    OnPropertyChanged(nameof(Use24HourFormat));
                }
            }
        }

        public string TimeZone
        {
            get => _timeZone;
            set
            {
                if (_timeZone != value)
                {
                    _timeZone = value;
                    OnPropertyChanged(nameof(TimeZone));
                }
            }
        }

        public List<Reminder> Reminders
        {
            get => _reminders;
            set
            {
                if (_reminders != value)
                {
                    _reminders = value;
                    OnPropertyChanged(nameof(Reminders));
                }
            }
        }

        public HotkeySettings Hotkeys
        {
            get => _hotkeys;
            set
            {
                if (_hotkeys != value)
                {
                    _hotkeys = value;
                    OnPropertyChanged(nameof(Hotkeys));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
