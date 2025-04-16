using Moq;
using PluginSystem.Core;
using PluginSystem.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Tests
{
    //public class PluginManagerTests
    //{
    //    private readonly Mock<IPluginFactory> _pluginFactoryMock;
    //    private readonly Mock<ILoggerService> _loggerMock;
    //    private readonly PluginPersistenceService _persistenceService;
    //    private readonly PluginManager _manager;

    //    public PluginManagerTests()
    //    {
    //        _pluginFactoryMock = new Mock<IPluginFactory>();
    //        _loggerMock = new Mock<ILoggerService>();
    //        _persistenceService = new PluginPersistenceService("TestPlugins");

    //        _manager = new PluginManager(_persistenceService, _loggerMock.Object, _pluginFactoryMock.Object);
    //    }

    //    [Fact]
    //    public void LoadPlugin_ShouldLoadSuccessfully()
    //    {
    //        // Arrange
    //        var path = "TestPlugins/SamplePlugin/SamplePlugin.dll";
    //        var container = new PluginContainer(new PluginInfo("SamplePlugin", "1.0"), null);

    //        _pluginFactoryMock.Setup(f => f.Create(path)).Returns(container);

    //        // Act
    //        var result = _manager.LoadPlugin(path);

    //        // Assert
    //        Assert.True(result); // Removed ".Success" as "result" is a boolean
    //        Assert.Contains(container, _manager.LoadAllPlugins);
    //    }
    //}

}
