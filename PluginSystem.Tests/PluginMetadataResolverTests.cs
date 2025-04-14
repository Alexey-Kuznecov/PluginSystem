using System.Reflection;
using PluginSystem.Core;
using PluginSystem.Runtime;
using Xunit;

namespace PluginSystem.Tests
{
    public class PluginMetadataResolverTests
    {   
        /// <summary>
        /// Проверяет, что если у плагина есть атрибут PluginMetadata,
        /// то PluginMetadataResolver корректно извлекает все его поля.
        /// </summary>
        [Fact]
        public void Resolve_ReturnsCorrectPluginInfo_WhenMetadataAttributePresent()
        {
            // Arrange
            Assembly assembly = typeof(SamplePlugin).Assembly;

            // Act
            var pluginInfo = PluginMetadataResolver.Resolve(assembly);

            // Assert
            Assert.NotNull(pluginInfo);
            Assert.Equal("SamplePlugin", pluginInfo!.Name);
            Assert.Equal("1.0.0", pluginInfo.Version);
            Assert.Equal("TestAuthor", pluginInfo.Author);
            Assert.Equal("com.test.sample", pluginInfo.DeveloperID);
            Assert.Equal("plugins/sample/docs.md", pluginInfo.DocumentationPath);
            Assert.True(pluginInfo.Description!.ContainsKey("en"));
            Assert.Equal("Test plugin description.", pluginInfo.Description["en"]);
        }

        /// <summary>
        /// Проверяет, что если в сборке нет классов с атрибутом PluginMetadata,
        /// то метод Resolve вернет null.
        /// </summary>
        [Fact]
        public void Resolve_ReturnsNull_WhenMetadataAttributeNotPresent()
        {
            // Arrange
            Assembly assembly = typeof(UnmarkedPlugin).Assembly;

            // Act
            var pluginInfo = PluginMetadataResolver.Resolve(assembly);

            // Assert
            Assert.Null(pluginInfo); // Вот что действительно нужно проверить
        }

        // Этот класс имитирует плагин с корректно заданным атрибутом PluginMetadata.
        [PluginMetadata(
            Name = "SamplePlugin",
            Version = "1.0.0",
            Author = "TestAuthor",
            DeveloperID = "com.test.sample",
            DocumentationPath = "plugins/sample/docs.md",
            Description = "Test plugin description.")]
        private class SamplePlugin : IPlugin
        {
            public string Name => "SamplePlugin";
            public string Version => "1.0.0";

            public void Initialize(IPluginContext context) { }
            public void Shutdown() { }
        }

        // Этот класс имитирует плагин без атрибута PluginMetadata.
        private class UnmarkedPlugin : IPlugin
        {
            public string Name => "Unmarked";
            public string Version => "1.0.0";

            public void Initialize(IPluginContext context) { }
            public void Shutdown() { }
        }
    }
}