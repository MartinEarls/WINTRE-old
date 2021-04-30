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
            PowerShellIEX();
        }
        static int PowerShellIEX()
        {
            Process Process = new Process();
            Process.StartInfo.FileName = "powershell";
            Process.StartInfo.Arguments = "-command Invoke-Expression \".\\Inputs\\CODEEX~1\\Example.exe\"";
            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.RedirectStandardOutput = true;
            Process.StartInfo.RedirectStandardError = true;
            Process.Start();
            string output = Process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            string err = Process.StandardError.ReadToEnd();
            Console.WriteLine(err);
            Process.WaitForExit();
            return 0;
        }
    }
}