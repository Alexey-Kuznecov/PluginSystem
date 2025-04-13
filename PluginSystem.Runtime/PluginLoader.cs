
namespace PluginSystem.Runtime
{
    using PluginSystem.Core;
    using System.Reflection;

    internal class PluginLoader
    {
        public IPluginFactory? LoadPlugin(string path)
        {
            if (!File.Exists(path))
            {
                // Логирование или уведомление о том, что файл не найден
                return null;
            }

            try
            {
                var assembly = Assembly.LoadFrom(path);
                var factoryType = assembly.GetTypes()
                    .FirstOrDefault(t => typeof(IPluginFactory).IsAssignableFrom(t) && !t.IsAbstract);

                if (factoryType == null)
                {
                    // Логирование предупреждения о том, что подходящий тип не найден
                    return null;
                }

                return (IPluginFactory?)Activator.CreateInstance(factoryType);
            }
            catch (Exception ex)
            {
                // Здесь можно залогировать ошибку ex
                Console.Error.WriteLine($"Ошибка загрузки плагина из '{path}': {ex.Message}");
                return null;
            }
        }
    }
}
