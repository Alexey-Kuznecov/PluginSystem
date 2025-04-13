namespace PluginSystem.Tests
{
    using Xunit;

    namespace PluginSystem.Tests
    {
        public class PluginLoadingTests
        {
            [Fact]
            public void Test_FaultyPlugin_Load_Fails()
            {
                var plugin = new FaultyPlugin();

                // Проверяем, что при попытке загрузки плагина выбрасывается ошибка
                Assert.Throws<InvalidOperationException>(() => plugin.Load());
            }

            [Fact]
            public void Test_CommandFaultyPlugin_Command_Fails()
            {
                var plugin = new CommandFaultyPlugin();

                // Загружаем плагин и проверяем, что команда вызывает ошибку
                plugin.Load();

                var ex = Assert.Throws<ArgumentException>(() => CommandManager.ExecuteCommand("faulty_command"));
                Assert.Equal("Ошибка при выполнении команды.", ex.Message);
            }
        }
    }
}