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
            VisualBasicScript();
        }
        static int VisualBasicScript()
        {
            Process process = new Process();
            process.StartInfo.FileName = "CMD";
            process.StartInfo.Arguments = "/c " + Directory.GetCurrentDirectory() + "\\Inputs\\CODEEX~1\\Example.vbs";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            string err = process.StandardError.ReadToEnd();
            Console.WriteLine(err);
            process.WaitForExit();
            return 0;
        }
    }
}