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

                // ���������, ��� ��� ������� �������� ������� ������������� ������
                Assert.Throws<InvalidOperationException>(() => plugin.Load());
            }

            [Fact]
            public void Test_CommandFaultyPlugin_Command_Fails()
            {
                var plugin = new CommandFaultyPlugin();

                // ��������� ������ � ���������, ��� ������� �������� ������
                plugin.Load();

                var ex = Assert.Throws<ArgumentException>(() => CommandManager.ExecuteCommand("faulty_command"));
                Assert.Equal("������ ��� ���������� �������.", ex.Message);
            }
        }
    }
}