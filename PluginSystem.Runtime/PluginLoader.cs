
namespace PluginSystem.Runtime
{
    using PluginSystem.Core;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Loader;

    /// <summary>
    /// Класс, отвечающий за загрузку плагинов из указанных DLL-файлов.
    /// </summary>
    public class PluginLoader : IPluginLoader
    {
        /// <summary>
        /// Загружает плагин из указанного пути к DLL-файлу.
        /// </summary>
        /// <param name="path">Полный путь к DLL-файлу плагина.</param>
        /// <returns>Экземпляр <see cref="IPluginFactory"/>, если загрузка успешна; в противном случае — <c>null</c>.</returns>
        public IPluginFactory? LoadPlugin(string path)
        {
            if (!File.Exists(path))
                return null;

            try
            {
                var assembly = LoadAssembly(path);

                // Сначала пробуем найти пользовательскую фабрику
                var factoryType = FindPluginFactoryType(assembly);
                if (factoryType != null)
                    return CreateInstance<IPluginFactory>(factoryType);

                // Если не нашли, ищем сам плагин
                var pluginType = FindPluginTypeImplementing<IPlugin>(assembly);
                if (pluginType != null)
                    return new PluginFactory(pluginType); // Универсальная фабрика

                return null;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Ошибка загрузки плагина из '{path}': {ex.Message}");
                return null;
            }
        }


        /// <summary>
        /// Загружает все плагины из указанной директории, возвращая фабрики плагинов.
        /// </summary>
        /// <param name="path">Путь к директории, содержащей DLL-файлы плагинов.</param>
        /// <returns>
        /// Перечисление экземпляров <see cref="IPluginFactory"/>, созданных из найденных DLL-файлов.
        /// </returns>
        public IEnumerable<IPluginFactory> LoadAllPlugins(string path)
        {
            if (!Directory.Exists(path))
                yield break;

            foreach (var plugin in Directory.EnumerateFiles(path, "*.dll"))
            {
                IPluginFactory? factory = LoadPlugin(plugin);
                if (factory != null)
                    yield return factory;
            }
        }

        private Type? FindPluginTypeImplementing<TInterface>(Assembly assembly)
        {
            return assembly
                .GetTypes()
                .FirstOrDefault(t => typeof(TInterface).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
        }

        /// <summary>
        /// Загружает сборку из указанного пути.
        /// </summary>
        /// <param name="path">Путь к DLL-файлу.</param>
        /// <returns>Загруженная сборка.</returns>
        private Assembly LoadAssembly(string path) =>
            Assembly.LoadFrom(path);

        /// <summary>
        /// Находит тип, реализующий интерфейс <see cref="IPluginFactory"/>, в указанной сборке.
        /// </summary>
        /// <param name="assembly">Сборка для поиска.</param>
        /// <returns>Тип, реализующий <see cref="IPluginFactory"/>, или <c>null</c>, если не найден.</returns>
        private Type? FindPluginFactoryType(Assembly assembly) =>
            assembly.GetTypes()
                    .FirstOrDefault(t => typeof(IPluginFactory).IsAssignableFrom(t) && !t.IsAbstract);

        /// <summary>
        /// Создает экземпляр указанного типа и приводит его к типу <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Тип, к которому необходимо привести созданный экземпляр. Должен быть ссылочным типом.</typeparam>
        /// <param name="factoryType">Тип, экземпляр которого необходимо создать.</param>
        /// <returns>Экземпляр типа <typeparamref name="T"/>, если создание и приведение успешны; в противном случае — <c>null</c>.</returns>
        private T? CreateInstance<T>(Type factoryType) where T : class
        {
            try
            {
                return Activator.CreateInstance(factoryType) as T;
            }
            catch (Exception ex)
            {
                // Здесь можно залогировать ошибку ex
                Console.Error.WriteLine($"Ошибка создания экземпляра типа '{factoryType.FullName}': {ex.Message}");
                return null;
            }
        }

        public bool CanLoadPlugin(string pluginPath)
        {
            throw new NotImplementedException();
        }

        public void UnloadPlugin(IPlugin plugin)
        {
            throw new NotImplementedException();
        }

        public PluginInfo? GetMetadata(string path)
        {
            try
            {
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
                var type = assembly.GetTypes()
                    .FirstOrDefault(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsAbstract && t.GetConstructor(Type.EmptyTypes) != null);

                if (type == null) return null;

                var plugin = (IPlugin)Activator.CreateInstance(type)!;

                return new PluginInfo
                {
                    Name = plugin.Name,
                    Version = plugin.Version,
                    DeveloperID = plugin.Name + "_" + plugin.Version,
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
