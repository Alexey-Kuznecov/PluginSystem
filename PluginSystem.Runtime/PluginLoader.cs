
using PluginSystem.Abstractions.Plugin;

namespace PluginSystem.Runtime
{
    using PluginSystem.Core;
    using PluginSystem.Core.PluginSystem.Core;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Loader;

    /// <summary>
    /// Класс, отвечающий за загрузку плагинов из указанных DLL-файлов.
    /// </summary>
    public class PluginLoader : IPluginLoader
    {

        public IPluginContainer LoadPlugin(string path)
        {
            var loadContext = new PluginLoadContext(path);
            var assembly = loadContext.LoadFromAssemblyPath(path);

            // Поиск и создание фабрики
            var factoryType = assembly.GetTypes().First(t => typeof(IPluginFactory).IsAssignableFrom(t) && !t.IsAbstract);
            var factory = (IPluginFactory)Activator.CreateInstance(factoryType)!;

            // Создаем контексты
            var pluginInitContext = new PluginInitContext(path, factory.GetPluginInfo(new PluginInfo())); // временный init-контекст, куда фабрика регистрирует зависимости
            var plugin = factory.CreatePlugin(pluginInitContext);

            // Основной runtime-контекст, который передаётся в сам плагин
            var pluginContext = new PluginContext(path); // например, в PluginContext можно передать всё, что зарегистрировано в InitContext
            plugin.Initialize(pluginContext);

            // Получаем информацию о плагине
            var pluginInfo = factory.GetPluginInfo(new PluginInfo());
            var finalContext = pluginInitContext.BuildPluginContext();

            return new PluginContainer(
                path,
                assembly,
                loadContext,
                factory,
                plugin,
                pluginContext,
                pluginInfo
            );
        }

        /// <summary>
        /// Загружает все плагины из указанной директории, возвращая фабрики плагинов.
        /// </summary>
        /// <param name="path">Путь к директории, содержащей DLL-файлы плагинов.</param>
        /// <returns>
        /// Перечисление экземпляров <see cref="IPluginFactory"/>, созданных из найденных DLL-файлов.
        /// </returns>
        public IEnumerable<IPluginContainer> LoadAllPlugins(string path)
        {
            if (!Directory.Exists(path))
                yield break;

            foreach (var pathplugin in Directory.EnumerateFiles(path, "*.dll"))
            {
                IPluginContainer? container = LoadPlugin(path);
                if (container != null)
                    yield return container;
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

        public bool UnloadPlugin(string systemId)
        {
            return true;
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

        public bool UnloadPlugin(IPlugin plugin)
        {
            throw new NotImplementedException();
        }
    }
}
