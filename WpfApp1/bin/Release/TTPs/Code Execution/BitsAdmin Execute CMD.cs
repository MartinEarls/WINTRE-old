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
            BitsAdminExecuteCMD();
        }
        static int BitsAdminExecuteCMD()
        {
			string currentDir = Directory.GetCurrentDirectory();
			
            Process process = new Process();
            process.StartInfo.FileName = "CMD";
            process.StartInfo.Arguments = "/c bitsadmin /create 1 & bitsadmin /addfile 1 C:\\Windows\\System32\\cmd.exe \"" + currentDir + "Outputs\\Code Execution\\" +
										  "cmd.exe\" & bitsadmin /SetNotifyCmdLine 1 \"" + currentDir + "Outputs\\Code Execution\\1.txt:cmd.exe\"" + " NULL & " +
										  "bitsadmin /RESUME 1 & bitsadmin /complete 1";
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