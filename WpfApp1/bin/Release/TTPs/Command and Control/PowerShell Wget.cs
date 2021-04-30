using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace WINTRE
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                PowerShellWget("");
            }
            else
            {
                PowerShellWget(args[0]);
            }
            
        }
        static int PowerShellWget(string targetURL)
        {
            Process Process = new Process();
            Process.StartInfo.FileName = "powershell";

            if(targetURL.Length == 0 ) //No argument, download default file from sys internals repo
            {
                Process.StartInfo.Arguments = "-command wget https://live.sysinternals.com/Eula.txt -O '.\\Outputs\\Command and Control\\wget.txt'";
            } else
            {
                Process.StartInfo.Arguments = "-command wget " + targetURL + "'.\\Outputs\\Command and Control\\wget.txt'";
            }

            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.RedirectStandardOutput = true;
            Process.StartInfo.RedirectStandardError = true;
            Process.Start();
            string output = Process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            Debug.WriteLine(output);
            string err = Process.StandardError.ReadToEnd();
            Console.WriteLine(err);
            Process.WaitForExit();
            return 0;
        }
    }
}