using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WINTRE
{
    class Logging
    {
        public string Log(string testName, string tactic, string standardOutput, string standardError)
        {
            string log = "";
            if (standardError.Length == 0)
            {
                //Change to print relevant logging messages from JSON
                log += "~" + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt ") + "[EXECUTED: " + "(" + tactic + ") " + testName + " RAN SUCCESSFULLY]";
                log += Environment.NewLine + standardOutput.Trim() + Environment.NewLine;
                //Send to log file
                File.AppendAllText("WINTRE-log.txt", log);
                return log;
            }
            else
            {
                log += "~" + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt ") + "[FAILED: " + "(" + tactic + ") " + testName + " COULD NOT EXECUTE]";
                log += Environment.NewLine + standardError.Trim() + Environment.NewLine;
                //Send to log file
                File.AppendAllText("WINTRE-log.txt", log);
                return log;
            }
        }
    }
}
