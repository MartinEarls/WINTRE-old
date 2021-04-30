using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace WINTRE
{
    class Program
    {
        static void Main(string[] args)
        {
            PowerShellRunExe();
        }
        static int PowerShellRunExe()
        {
            Process Process = new Process();
            Process.StartInfo.FileName = "powershell";
            //Process.StartInfo.Arguments = "& \".\\Inputs\\Code Execution\\Example.exe\"";			
            Process.StartInfo.UseShellExecute = false;
			Process.StartInfo.RedirectStandardInput = true;
            Process.StartInfo.RedirectStandardOutput = true;
            Process.StartInfo.RedirectStandardError = true;
            Process.Start();
			Process.StandardInput.WriteLine("& \".\\Inputs\\Code Execution\\Example.exe\"");
			Process.StandardInput.WriteLine("exit");
			
            string output = Process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            string err = Process.StandardError.ReadToEnd();
            Console.WriteLine(err);
            Process.WaitForExit();
            return 0;
        }
    }
}