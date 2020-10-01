using System.Diagnostics;

namespace PrototypeShell.Controllers
{
    class WinShellRun
    {
        public Process proc;

        public void Call(string wd, string exec, string args = "")
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                FileName = "powershell.exe",
                Arguments = $"/c {exec} {args}",
                WorkingDirectory = wd
            };

            proc = Process.Start(psi);

            Command = $"{exec}  {args}";
            StandardOut = proc.StandardOutput.ReadToEnd();
            StandardErr = proc.StandardError.ReadToEnd();
            proc.WaitForExit();
            ExitCode = proc.ExitCode;
        }

        public string Command { get; private set; }
        
        public int ExitCode { get; private set; }

        public string StandardOut { get; private set; }

        public string StandardErr { get; private set; }
    }
}
