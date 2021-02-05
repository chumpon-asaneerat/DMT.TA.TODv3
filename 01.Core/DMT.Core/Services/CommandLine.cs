using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DMT.Services
{
    public class CommandLine
    {
        public string FileName { get; set;  }

        public bool UseShellExecute { get; set; }
        public bool RedirectStandardOutput { get; set; }
        public bool RedirectStandardError { get; set; }
        public bool CreateNoWindow { get; set; }

        public CommandLine()
        {
            FileName = "netsh.exe";
            UseShellExecute = false;
            RedirectStandardOutput = false;
            RedirectStandardError = false;
            CreateNoWindow = true;
        }

        public void Run(string arguments)
        {
            var psi = new ProcessStartInfo();
            psi.FileName = FileName;
            psi.Arguments = arguments;
            psi.UseShellExecute = UseShellExecute;
            psi.RedirectStandardOutput = RedirectStandardOutput;
            psi.RedirectStandardError = RedirectStandardError;
            psi.CreateNoWindow = CreateNoWindow;

            using (var process = Process.Start(psi))
            {
                if (RedirectStandardOutput)
                {
                    process.OutputDataReceived += (sender, eventArgs) => Console.WriteLine("OUTPUT: " + eventArgs.Data);
                    process.BeginOutputReadLine();
                }

                if (RedirectStandardError)
                {
                    process.ErrorDataReceived += (sender, eventArgs) => Console.WriteLine("ERROR: " + eventArgs.Data);
                    process.BeginErrorReadLine();
                }

                process.WaitForExit();
            }
        }
    }
}
