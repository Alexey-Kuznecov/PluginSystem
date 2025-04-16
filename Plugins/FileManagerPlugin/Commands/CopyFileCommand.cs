
using PluginSystem.Core;
using System;
using System.Collections.Generic;
using System.IO;
using PluginSystem.Core.PluginSystem.Core;

namespace FileManagerPlugin.Commands;

public class CopyFileCommand : IPluginCommand, IPluginUnloadable
{
    public string Id => "CopyFile";
    public string Name => "Copy File";
    public string Description => "Copies a file from source to destination.";
    public CommandCategory Category => CommandCategory.File;
    public IReadOnlyList<string> ExpectedParameters => new[] { "source", "destination" };

    public bool CanUndo => false;
    public void Undo(ICommandContext context) { }

    public ICommandParameters GetDefaultParameters() => new CommandParameters();

    public void Execute(ICommandContext context)
    {
        var source = context.Parameters.Get<string>("source");
        var destination = context.Parameters.Get<string>("destination");

        if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(destination))
        {
            Console.WriteLine("Invalid source or destination path.");
            return;
        }

        File.Copy(source, destination, overwrite: true);
        Console.WriteLine($"File copied from {source} to {destination}");
    }

    public void OnUnload()
    {
        throw new NotImplementedException();
    }
}
