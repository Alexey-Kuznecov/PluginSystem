
using PluginSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSystem.Core.PluginSystem.Core;

namespace ClockPlugin
{
    public class GetCurrentTimeCommand : IPluginCommand, IPluginUnloadable
    {
        public string Id => "GCTCommand";
        public string Name => "Get Current Time";
        public string Description => "Отображает текущее время и, при необходимости, дату.";
        public CommandCategory Category => CommandCategory.Tools;

        public IReadOnlyList<string> ExpectedParameters => _expectedParameters;
        private readonly List<string> _expectedParameters = new() { "format", "showDate" };

        private readonly PluginSettings<ClockPluginSettings>? _pluginSettings;

        public GetCurrentTimeCommand(PluginSettings<ClockPluginSettings>? pluginSettings)
        {
            _pluginSettings = pluginSettings;
        }

        public void Execute(ICommandContext context)
        {
            var settings = _pluginSettings?.Value ?? new ClockPluginSettings();

            // Получаем параметры из контекста или используем из настроек
            var format = context.Parameters.Get<string>("format") ?? settings.ClockFormat ?? "HH:mm";
            var showDate = context.Parameters.Get<bool?>("showDate") ?? settings.ShowDate;

            string currentTime = DateTime.Now.ToString(format);

            if (showDate)
            {
                currentTime += " " + DateTime.Now.ToString("yyyy-MM-dd");
            }

            Console.WriteLine($"Current Time: {currentTime}");
        }

        public bool CanUndo => false;

        public void Undo(ICommandContext context)
        {
            // Ничего не нужно откатывать
        }

        public ICommandParameters GetDefaultParameters()
        {
            var parameters = new CommandParameters();
            parameters.Set("format", "HH:mm");
            parameters.Set("showDate", false);
            return parameters;
        }

        public void OnUnload()
        {
            // Например, очистка ссылок, если они тяжелые
            _pluginSettings?.Save(); // если нужно явно сохранить перед выгрузкой
            // Можно даже null-нуть:
            // _pluginSettings = null;
        }
    }
}
