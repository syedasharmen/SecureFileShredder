
using System;
using System.Diagnostics;
using System.IO;

namespace SecureFileShredder
{
    public static class SecureDelete
    {
        public static bool DeleteFile(string filePath)
        {
            string toolPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Tools",
                "sdelete64.exe"
            );

            if (!File.Exists(toolPath))
                throw new FileNotFoundException(
                    "SDelete was not found.", toolPath);

            if (!File.Exists(filePath))
                throw new FileNotFoundException(
                    "Selected file does not exist.", filePath);

            ProcessStartInfo startInfo =
                new ProcessStartInfo();

            startInfo.FileName = toolPath;
            startInfo.UseShellExecute = false;

            startInfo.Arguments = "-p 1 \"" + filePath + "\"";

            using (Process process =
                Process.Start(startInfo))
            {
                process.WaitForExit();

                if (process.ExitCode != 0)
                    throw new Exception(
                        "SDelete returned an error. Code: "
                        + process.ExitCode);

                return !File.Exists(filePath);
            }
        }
    }
}
