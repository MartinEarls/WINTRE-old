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
            FTP();
        }
        static int FTP()
        {
            Process process = new Process();
            process.StartInfo.FileName = "CMD";
            process.StartInfo.Arguments = "/c echo !cmd /c \"" + Directory.GetCurrentDirectory() + "\\Inputs\\CODEEX~1\\Example.exe" + "\" > \".\\Outputs\\CODEEX~1\\ftpcommands.txt\" && ftp -s:\".\\Outputs\\CODEEX~1\\ftpcommands.txt\"";
            process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
			
			process.StandardInput.WriteLine("quit");
			
            string output = process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            string err = process.StandardError.ReadToEnd();
            Console.WriteLine(err);
            process.WaitForExit();
            return 0;
        }
    }
}