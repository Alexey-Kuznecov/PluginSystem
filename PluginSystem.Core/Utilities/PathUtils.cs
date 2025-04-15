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
    }
}