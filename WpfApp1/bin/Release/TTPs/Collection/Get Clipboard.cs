using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TechniqueDebugging
{
    class Program
    {
        public static void GetClipboard()
        {
            Stopwatch Timer = new Stopwatch();
            Timer.Start();

			Console.WriteLine("Getting clipboard contents...);
            while (Timer.Elapsed < TimeSpan.FromSeconds(10))
            {
                Clipboard.GetText();
            }

			Console.WriteLine("Clipboard contents: " + Clipboard.GetText());
            Timer.Stop();
            
        }

        [System.STAThread]
        public static void Main(string[] args)
        {
            GetClipboard();
        }
    }
}
