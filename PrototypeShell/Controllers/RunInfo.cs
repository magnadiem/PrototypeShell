using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeShell.Controllers
{
    public static class RunInfo
    {
        public static string Username { get; }
            = Environment.UserName;
        public static string Hostname { get; }
            = Environment.MachineName;
        private static string _currentPath = PathManager.CurrentPath;

        public static string CurrentPath
        {
            get
            {
                return _currentPath;
            }

            set
            {
                if (PathManager.ProcessPath(value) == 0)
                {
                    _currentPath = PathManager.CurrentPath;
                }
            }
        }
    }

    static class PathManager
    {
        public static string CurrentPath { get; private set; }
            = Environment.CurrentDirectory;

        public static int ProcessPath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                if (Directory.Exists(path))
                {
                    CurrentPath = path;
                    return 0;
                }
                else
                {
                    return -1;
                }

            }
            else
            {
                string tempPath = Path.GetFullPath(Path.Combine(CurrentPath, path));
                if (Directory.Exists(tempPath))
                {
                    CurrentPath = tempPath;
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
        }
    }
}
