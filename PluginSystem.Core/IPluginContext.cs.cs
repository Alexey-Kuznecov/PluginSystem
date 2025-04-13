
namespace PluginSystem.Core
{
    public interface IPluginContext
    {
        List<IPluginCommand> Commands { get; }
        IPluginSettingsService SettingsService { get; }

        void Register<T>(T instance) where T : class;
        T? GetService<T>() where T : class;
        IEnumerable<T> GetServices<T>() where T : class;

        // Другие члены интерфейса
        void ExecuteCommand(IPluginCommand command, ICommandContext context);
    }
}