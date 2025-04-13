
namespace PluginSystem.Runtime
{
    public static class KeyboardBindingService
    {
        private static readonly Dictionary<string, string> _bindings = new();

        public static void Bind(string commandId, string keyCombo)
        {
            _bindings[commandId] = keyCombo;
        }

        public static string? GetBinding(string commandId)
        {
            return _bindings.TryGetValue(commandId, out var value) ? value : null;
        }

        public static bool Matches(string commandId, string pressedCombo)
        {
            return _bindings.TryGetValue(commandId, out var value) && value == pressedCombo;
        }
    }
}
