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
            ScheduledTask();
        }
        static int ScheduledTask()
        {
			DateTime currentTime = DateTime.Now;
			string taskTime = "";
			
			if(currentTime.Hour.ToString().Length == 1) {
				taskTime = "0" + currentTime.Hour.ToString() + ":" + (Int32.Parse(currentTime.Minute.ToString())+1);
			} else {
				taskTime = currentTime.Hour.ToString() + ":" + (Int32.Parse(currentTime.Minute.ToString())+1);
			}
			
			Console.WriteLine("Scheduling task for: " + taskTime);
			
			//Delete existing task
			Process deleteTask = new Process();
            deleteTask.StartInfo.FileName = "CMD";
            deleteTask.StartInfo.Arguments = "/c schtasks /delete /tn WINTRE /f";
            deleteTask.StartInfo.UseShellExecute = false;
			deleteTask.StartInfo.RedirectStandardOutput = true;
            deleteTask.StartInfo.RedirectStandardError = true;
            deleteTask.Start();
			deleteTask.WaitForExit();
			
			string output = deleteTask.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            string err = deleteTask.StandardError.ReadToEnd();
			
			//Run schtasks
            Process process = new Process();
            process.StartInfo.FileName = "CMD";
            process.StartInfo.Arguments = "/c schtasks /Create /SC once /TN WINTRE /TR \"" + Directory.GetCurrentDirectory() + "\\Inputs\\Code Execution\\Example.exe\" /ST " + taskTime;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            output = process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            err = process.StandardError.ReadToEnd();
            Console.WriteLine(err);
            process.WaitForExit();
            return 0;
        }
    }
}