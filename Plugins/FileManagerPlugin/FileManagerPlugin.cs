
using FileManagerPlugin.Commands;
using PluginSystem.Commands;
using PluginSystem.Core;

namespace FileManagerPlugin
{
    public class FileManagerPlugin : IPlugin, IPluginMenuProvider
    {
        public string Id => "FileManagerPlugin";
        public string Name => "File Manager Plugin";

        public string Version => "1.0";

        public void Initialize(IPluginContext context)
        {
            context.Register(new CopyFileCommand());
            //context.RegisterCommand(new MoveFileCommand());
            //context.RegisterCommand(new DeleteFileCommand());
            //context.RegisterCommand(new CreateDirectoryCommand());
            //context.RegisterCommand(new RenameFileCommand());
        }

        public PluginMenu GetMenu()
        {
            return new PluginMenu()
            {
                Title = "File Manager",
                Items = new List<PluginMenuItem>
                {
                    new PluginMenuItem
                    {
                        Label = "📄 Copy File",
                        Action = () => new CopyFileCommand().Execute(ConsoleCommandContext.Instance)
                    },
                    new PluginMenuItem
                    {
                        Label = "🚚 Move File (todo)",
                        Action = () => Console.WriteLine("Move command coming soon!")
                    }
                }
            };
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }
    }
}
