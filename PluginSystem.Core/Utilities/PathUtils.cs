using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Core
{
    public static class PathUtils
    {
        public static bool TryGetFirstDllFileInfo(this string folderPath, out string fileName, out string extension)
        {
            fileName = null;
            extension = null;

            if (!Directory.Exists(folderPath))
                return false;

            var firstDllFile = Directory.EnumerateFiles(folderPath, "*.dll").FirstOrDefault();
            if (firstDllFile == null)
                return false;

            fileName = Path.GetFileName(firstDllFile);
            extension = Path.GetExtension(firstDllFile);
            return true;
        }

        public static bool IsProbablyPath(string arg)
        {
            return Path.IsPathRooted(arg) || arg.Contains("\\") || arg.Contains("/") || Path.HasExtension(arg);
        }

        public static string GetSolutionDirectory()
        {
            var dir = AppContext.BaseDirectory;
            for (int i = 0; i < 5; i++) // поднимаемся на 5 уровней вверх
            {
                dir = Directory.GetParent(dir)?.FullName ?? throw new InvalidOperationException("Cannot locate solution directory.");
                if (Directory.GetFiles(dir, "*.sln").Any())
                    return dir;
            }

            throw new DirectoryNotFoundException("Solution file not found.");
        }
    }
}