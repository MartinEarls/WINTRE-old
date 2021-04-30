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
            BitsAdmin();
        }
        static int BitsAdmin()
        {
			string currentDir = Directory.GetCurrentDirectory();
			
            Process process = new Process();
            process.StartInfo.FileName = "bitsadmin";
            process.StartInfo.Arguments = "/transfer safedownload /download /priority normal https://live.sysinternals.com/eula.txt \"" + currentDir +
			"\\Outputs\\Command and Control\\BitsAdmin.txt\"";
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