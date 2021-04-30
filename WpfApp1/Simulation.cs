using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace WINTRE
{
    public class Simulation
    {
        public void SimulateCS(string testName, string tactic)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            string readContents;
            string currentDirectory = Directory.GetCurrentDirectory();

            using (StreamReader streamReader = new StreamReader(currentDirectory + "\\TTPs\\" + tactic + "\\" + testName + ".cs", Encoding.UTF8))
            {
                readContents = streamReader.ReadToEnd();
            }

            //Compile technique
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll"); //all techniques
            parameters.ReferencedAssemblies.Add("System.Core.dll"); //all techniques
            parameters.ReferencedAssemblies.Add("System.Drawing.dll"); //screenshot
            parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll"); //screenshot, clipboard
            // True - memory generation, false - external file generation
            parameters.GenerateInMemory = false;
            // True - exe file generation, false - dll file generation
            parameters.GenerateExecutable = true;
            //Name
            parameters.OutputAssembly = currentDirectory + "\\Payloads\\" + tactic + "\\" + testName + ".exe";
            parameters.TreatWarningsAsErrors = false;
            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, readContents);

            foreach (CompilerError CompErr in results.Errors)
            {
                Console.WriteLine("Line number " + CompErr.Line + ", Error Number: " + CompErr.ErrorNumber + ", '" + CompErr.ErrorText + ";");
            }

            //Run technique
            Process technique = new Process();
            technique.StartInfo.FileName = currentDirectory + "\\Payloads\\" + tactic + "\\" + testName + ".exe"; //specify folder here?
            technique.StartInfo.UseShellExecute = false;
            technique.StartInfo.CreateNoWindow = false; //Have to leave this off for some GUI based ones (PS Cred Prompt) Add additional argument to JSON "hasGUI" like "hasArgs"
            technique.StartInfo.RedirectStandardOutput = true;
            technique.StartInfo.RedirectStandardError = true;

            technique.Start();

            //Read the output (or the error)
            string standardOutput = "";
            string standardError = "";
            try
            {
                standardOutput = technique.StandardOutput.ReadToEnd();
                standardError = technique.StandardError.ReadToEnd();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            Debug.WriteLine(standardOutput);
            Debug.WriteLine(standardError);
            technique.WaitForExit();

            WINTRE.Logging log = new WINTRE.Logging();
            log.Log(testName, tactic, standardOutput, standardError);
        }

        public void SimulateCPP(string testName, string tactic)
        {
            string readContents;
            string currentDirectory = Directory.GetCurrentDirectory();

            using (StreamReader streamReader = new StreamReader(currentDirectory + "\\TTPs\\" + tactic + "\\" + testName + ".cpp", Encoding.UTF8))
            {
                readContents = streamReader.ReadToEnd();
            }

            //Compile technique
            Process Compile = new Process();
            Compile.StartInfo.FileName = "cmd.exe";
            Compile.StartInfo.UseShellExecute = false;
            Compile.StartInfo.CreateNoWindow = false;
            Compile.StartInfo.RedirectStandardOutput = true;
            Compile.StartInfo.RedirectStandardError = true;
            Compile.StartInfo.RedirectStandardInput = true;
            Compile.Start();

            //Set environment variables from bat file, may have to make this dynamic?
            //Try get vcvarsall.bat for VS 2014, 2017 and 2019
            //Need to check for community, professional, enterprise versions
            string vccarsallPath = "";
            if (Directory.Exists(@"C:\Program Files (x86)\Microsoft Visual Studio 14.0\")) { //Check for 2015 (14.0)
                vccarsallPath = @"C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\vcvarsall.bat";
            }
            else if(Directory.Exists("C:\\Program Files(x86)\\Microsoft Visual Studio\\2017\\")) //Check for 2017
            {
                if(File.Exists("C:\\Program Files(x86)\\Microsoft Visual Studio\\2017\\BuildTools\\VC\\Auxiliary\\Build\\vcvarsall.bat"))
                {
                    vccarsallPath = "C:\\Program Files(x86)\\Microsoft Visual Studio\\2017\\BuildTools\\VC\\Auxiliary\\Build\\vcvarsall.bat";
                }
            } else if(Directory.Exists(@"C:\Program Files (x86)\Microsoft Visual Studio\2019")) //Check for 2019 versions
            {
                if(File.Exists(@"C:\Program Files(x86)\Microsoft Visual Studio\2019\Community\VC\Auxiliary\Build\vcvarsall.bat"))
                {
                    vccarsallPath = @"C:\Program Files(x86)\Microsoft Visual Studio\2019\Community\VC\Auxiliary\Build\vcvarsall.bat";
                } else if(File.Exists(@"C:\Program Files(x86)\Microsoft Visual Studio\2019\Professional\VC\Auxiliary\Build\vcvarsall.bat")) {
                    vccarsallPath = @"C:\Program Files(x86)\Microsoft Visual Studio\2019\Professional\VC\Auxiliary\Build\vcvarsall.bat";
                }
                else if (File.Exists(@"C:\Program Files(x86)\Microsoft Visual Studio\2019\Enterprise\VC\Auxiliary\Build\vcvarsall.bat")) {
                    vccarsallPath = @"C:\Program Files(x86)\Microsoft Visual Studio\2019\Enterprise\VC\Auxiliary\Build\vcvarsall.bat";
                }
            }

            if(vccarsallPath == "")
            {
                //Implies no appropriate VS installation found
                MessageBox.Show("C++ support for Visual Studio does not appear to be installed, preventing C++ technique compiliation. " +
                    "Please install C++ support using your Visual Studio Installer." + "\n" + "https://docs.microsoft.com/en-us/cpp/build/vscpp-step-0-installation",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } else
            {
                Compile.StandardInput.WriteLine("\"" + vccarsallPath + "\" x64");
                //CL.exe and compiler options
                Compile.StandardInput.WriteLine("cl " + "\"" + currentDirectory + "\\TTPs\\" + tactic + "\\" + testName + ".cpp\" " +
                    "/permissive- /GS /GL /analyze- /W3 /Gy /Zc:wchar_t /Zi /Gm- /O2 /sdl /Zc:inline /fp:precise /D \"WIN32\" /D \"NDEBUG\" " +
                    "/D \"_CONSOLE\" /D \"_UNICODE\" /D \"UNICODE\" /errorReport:prompt /WX- /Zc:forScope /Gd /Oy- /Oi /MD /FC /EHsc /nologo " +
                    "/diagnostics:column /Fo\"" + currentDirectory + "\\Payloads\\" + tactic + "\\" + testName + "\"" + " /Fe\"" + currentDirectory + "\\Payloads\\" + tactic + "\\" + testName + "\"" +
                    " /Fd\"" + currentDirectory + "\\Payloads\\" + tactic + "\\" + testName + "\"");

                //Exit
                Compile.StandardInput.WriteLine("exit");

                //Read the output (or the error)
                string standardOutput = "";
                string standardError = "";

                try
                {
                    standardOutput = Compile.StandardOutput.ReadToEnd();
                    standardError = Compile.StandardError.ReadToEnd();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }

                Debug.WriteLine(standardOutput);
                Debug.WriteLine(standardError);
                Compile.WaitForExit();

                //Run technique
                Process technique = new Process(); //CHANGE TTPS TO PAYLOADS WHEN COMPILER OPTIONS FIGURED OUT
                technique.StartInfo.FileName = currentDirectory + "\\PAYLOADS\\" + tactic + "\\" + testName + ".exe"; //specify folder here?
                technique.StartInfo.UseShellExecute = false;
                technique.StartInfo.CreateNoWindow = false; //Have to leave this off for some GUI based ones (Cred Prompt) Add additional argument to JSON "hasGUI" like "hasArgs"
                technique.StartInfo.RedirectStandardOutput = true;
                technique.StartInfo.RedirectStandardError = true;
                technique.Start();

                try
                {
                    standardOutput = technique.StandardOutput.ReadToEnd();
                    standardError = technique.StandardError.ReadToEnd();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }

                Debug.WriteLine(standardOutput);
                Debug.WriteLine(standardError);
                technique.WaitForExit();

                WINTRE.Logging log = new WINTRE.Logging();
                log.Log(testName, tactic, standardOutput, standardError);
            }
        }

        public void SimulateCPP(string testName, string tactic, string arguments, bool hasArgs)
        {
            string readContents;
            string currentDirectory = Directory.GetCurrentDirectory();

            using (StreamReader streamReader = new StreamReader(currentDirectory + "\\TTPs\\" + tactic + "\\" + testName + ".cpp", Encoding.UTF8))
            {
                readContents = streamReader.ReadToEnd();
            }

            //Compile technique
            Process Compile = new Process();
            Compile.StartInfo.FileName = "cmd.exe";
            Compile.StartInfo.UseShellExecute = false;
            Compile.StartInfo.CreateNoWindow = false;
            Compile.StartInfo.RedirectStandardOutput = true;
            Compile.StartInfo.RedirectStandardError = true;
            Compile.StartInfo.RedirectStandardInput = true;
            Compile.Start();

            //Set environment variables from bat file, may have to make this dynamic?
            Compile.StandardInput.WriteLine("\"C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\BuildTools\\VC\\Auxiliary\\Build\\vcvarsall.bat\" x64");
            //CL.exe and compiler options
            Compile.StandardInput.WriteLine("cl " + "\"" + currentDirectory + "\\TTPs\\" + tactic + "\\" + testName + ".cpp\" " +
                "/permissive- /GS /GL /analyze- /W3 /Gy /Zc:wchar_t /Zi /Gm- /O2 /sdl /Zc:inline /fp:precise /D \"WIN32\" /D \"NDEBUG\" " +
                "/D \"_CONSOLE\" /D \"_UNICODE\" /D \"UNICODE\" /errorReport:prompt /WX- /Zc:forScope /Gd /Oy- /Oi /MD /FC /EHsc /nologo " +
                "/diagnostics:column /Fo\"" + currentDirectory + "\\Payloads\\" + tactic + "\\" + testName + "\"" + " /Fe\"" + currentDirectory + "\\Payloads\\" + tactic + "\\" + testName + "\"" +
                " /Fd\"" + currentDirectory + "\\Payloads\\" + tactic + "\\" + testName + "\"");

            //Exit
            Compile.StandardInput.WriteLine("exit");

            //Read the output (or the error)
            string standardOutput = "";
            string standardError = "";

            try
            {
                standardOutput = Compile.StandardOutput.ReadToEnd();
                standardError = Compile.StandardError.ReadToEnd();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            Debug.WriteLine(standardOutput);
            Debug.WriteLine(standardError);
            Compile.WaitForExit();

            //Run technique
            Process technique = new Process(); //CHANGE TTPS TO PAYLOADS WHEN COMPILER OPTIONS FIGURED OUT
            technique.StartInfo.FileName = currentDirectory + "\\PAYLOADS\\" + tactic + "\\" + testName + ".exe"; //specify folder here?
            technique.StartInfo.UseShellExecute = false;
            technique.StartInfo.CreateNoWindow = false;
            technique.StartInfo.RedirectStandardOutput = true;
            technique.StartInfo.RedirectStandardError = true;

            if (!hasArgs)
            {
                technique.Start();
            }
            else
            {
                technique = Process.Start(technique.StartInfo.FileName, arguments); //try catch for bad args?
            }

            try
            {
                standardOutput = technique.StandardOutput.ReadToEnd();
                standardError = technique.StandardError.ReadToEnd();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            Debug.WriteLine(standardOutput);
            Debug.WriteLine(standardError);
            technique.WaitForExit();

            WINTRE.Logging log = new WINTRE.Logging();
            log.Log(testName, tactic, standardOutput, standardError);
        }

        public void SimulateCS(string testName, string tactic, CSharpCodeProvider codeProvider, string arguments, bool hasArgs) //Call this function with a loop, iterating through an array of stored tests to be executed
        {
            string readContents;
            string currentDirectory = Directory.GetCurrentDirectory();

            using (StreamReader streamReader = new StreamReader(currentDirectory + "\\TTPs\\" + tactic + "\\" + testName + ".cs", Encoding.UTF8))
            {
                readContents = streamReader.ReadToEnd();
            }

            //Compile technique
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll"); //all techniques
            parameters.ReferencedAssemblies.Add("System.Core.dll"); //all techniques
            parameters.ReferencedAssemblies.Add("System.Drawing.dll"); //screenshot
            parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll"); //screenshot, clipboard
            // True - memory generation, false - external file generation
            parameters.GenerateInMemory = false;
            // True - exe file generation, false - dll file generation
            parameters.GenerateExecutable = true;
            //Name
            parameters.OutputAssembly = currentDirectory + "\\Payloads\\" + tactic + "\\" + testName + ".exe";
            parameters.TreatWarningsAsErrors = false;
            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, readContents);

            foreach (CompilerError CompErr in results.Errors)
            {
                Console.WriteLine("Line number " + CompErr.Line + ", Error Number: " + CompErr.ErrorNumber + ", '" + CompErr.ErrorText + ";");
            }

            //Run technique
            Process technique = new Process();
            technique.StartInfo.FileName = currentDirectory + "\\Payloads\\" + tactic + "\\" + testName + ".exe"; //specify folder here?
            technique.StartInfo.UseShellExecute = false;
            technique.StartInfo.CreateNoWindow = false; //Have to leave this off for some GUI based ones (PS Cred Prompt) Add additional argument to JSON "hasGUI" like "hasArgs"
            technique.StartInfo.RedirectStandardOutput = true;
            technique.StartInfo.RedirectStandardError = true;
            if (!hasArgs)
            {
                technique.Start();
            }
            else
            {
                technique = Process.Start(technique.StartInfo.FileName, arguments); //try catch for bad args?
            }

            //Read the output (or the error)
            string standardOutput = "";
            string standardError = "";
            try
            {
                standardOutput = technique.StandardOutput.ReadToEnd();
                standardError = technique.StandardError.ReadToEnd();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            Debug.WriteLine(standardOutput);
            Debug.WriteLine(standardError);
            technique.WaitForExit();

            WINTRE.Logging log = new WINTRE.Logging();
            log.Log(testName, tactic, standardOutput, standardError);
        }
    }
}
