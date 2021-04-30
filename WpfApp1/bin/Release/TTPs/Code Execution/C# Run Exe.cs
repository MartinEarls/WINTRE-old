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
            CSharpRunExe();
        }
        static int CSharpRunExe()
        {
            Process Process = new Process();
            Process.StartInfo.FileName = "\".\\Inputs\\Code Execution\\Example.exe\"";		
            Process.StartInfo.UseShellExecute = false;
			Process.StartInfo.RedirectStandardInput = true;
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