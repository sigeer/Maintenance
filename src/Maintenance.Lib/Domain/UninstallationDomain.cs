﻿using Serilog;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Maintenance.Lib.Domain
{
    public class UninstallationDomain
    {
        public static void Core()
        {
            var dirInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            var currentExeFullName = Process.GetCurrentProcess().MainModule?.FileName;

            var files = dirInfo.GetFiles().Where(x => x.FullName != currentExeFullName);

            foreach (var file in files)
            {
                Log.Logger.Information($"删除文件:{file.FullName}");
                File.Delete(file.FullName);
            }
            var dirs = dirInfo.GetDirectories();
            foreach (var dir in dirs)
            {
                Log.Logger.Information($"删除目录:{dir.FullName}");
                Directory.Delete(dir.FullName, true);
            }
            Log.Logger.Information($"当前系统：{RuntimeInformation.OSDescription}");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string batPath = string.Empty;
                string command = string.Empty;
                if (dirInfo.Parent != null)
                {
                    batPath = Path.Combine(dirInfo.Parent.FullName, "tmp.bat");
                    command = $"timeout /t 1 & del \"{currentExeFullName}\" & rmdir \"{AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\')}\" & del \"{batPath}\"";
                }
                else
                {
                    batPath = Path.Combine(dirInfo.FullName, "tmp.bat");
                    command = $"timeout /t 1 & del \"{currentExeFullName}\" & del \"{batPath}\"";
                }
                File.WriteAllText(batPath, command, encoding: Encoding.UTF8);
                Process.Start(batPath);
                Environment.Exit(0);
            }
            Log.Logger.Information("清理完成");
        }
    }
}
