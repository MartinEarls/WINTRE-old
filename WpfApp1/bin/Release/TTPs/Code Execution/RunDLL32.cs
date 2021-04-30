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
            RunDLL32();
        }
        static int RunDLL32()
        {
            Process Process = new Process();
            Process.StartInfo.FileName = "cmd.exe";	
			Process.StartInfo.Arguments = "/c rundll32.exe \"" + Directory.GetCurrentDirectory() + "\\Inputs\\Code Execution\\Example.dll\",TestFunction";	
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