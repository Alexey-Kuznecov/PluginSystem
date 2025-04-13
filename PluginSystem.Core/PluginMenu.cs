
namespace PluginSystem.Core
{
    // Класс описания меню плагина
    public class PluginMenu
    {
        public string Title { get; set; } = string.Empty;
        public List<PluginMenuItem> Items { get; set; } = new();
    }
}
