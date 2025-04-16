using PluginSystem.Core;
using PluginSystem.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace PluginSystem.Tests
{
    public class PluginLoadContextTests
    {
        private const string PluginsPath = "Plugins/TestPlugin"; // Примерная папка для сборок плагинов

        [Fact]
        public void Plugin_ShouldLoadInIsolatedContext()
        {
            // Arrange
            var pluginPath = Path.GetFullPath(Path.Combine(PathUtils.GetSolutionDirectory(), PluginsPath, "TestPlugin.dll"));
            var context = new PluginLoadContext(Path.GetDirectoryName(pluginPath)!);

            // Act
            var assembly = context.LoadFromAssemblyPath(pluginPath);
            var type = assembly.GetTypes().FirstOrDefault(t => typeof(IPlugin).IsAssignableFrom(t));

            // Assert
            type.Should().NotBeNull("Тип, реализующий IPlugin, должен быть найден");
            var instance = Activator.CreateInstance(type!);
            instance.Should().BeAssignableTo<IPlugin>();
        }

        [Fact]
        public void Plugin_ContextShouldBeCollectableAfterUnload()
        {
            // Arrange
            WeakReference? contextRef = null;

            void LoadAndUnload()
            {
                var pluginPath = Path.GetFullPath(Path.Combine(PathUtils.GetSolutionDirectory(), PluginsPath, "TestPlugin.dll"));
                var context = new PluginLoadContext(Path.GetDirectoryName(pluginPath)!);
                contextRef = new WeakReference(context);

                // Загружаем плагин
                var assembly = context.LoadFromAssemblyPath(pluginPath);

                // Отменяем все ссылки
                context.Unload();
            }

            // Act
            LoadAndUnload();

            // Форсируем GC
            for (int i = 0; i < 5 && contextRef!.IsAlive; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(100);
            }

            // Assert
            contextRef!.IsAlive.Should().BeFalse("Контекст должен быть выгружен и собран GC");
        }

        [Fact]
        public void Plugin_LoadingInvalidPath_ShouldNotThrow()
        {
            // Arrange
            var invalidPath = Path.Combine(PluginsPath, "NonExistent.dll");
            var context = new PluginLoadContext(PluginsPath);

            // Act
            Action act = () => context.LoadFromAssemblyPath(invalidPath);

            // Assert
            act.Should().Throw<FileNotFoundException>();
        }
    }
}
