using Microsoft.CSharp;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void ClearPayloadsClick(object sender, RoutedEventArgs e)
        {
            string[] sourceFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Payloads\\", "*.*", SearchOption.AllDirectories);

            for (int i = 0; i < sourceFiles.Length; i++)
            {
                File.Delete(sourceFiles[i]);
            }
            LogOutput.Text = LogOutput.Text + Environment.NewLine + "~" + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt ") + "[INFO: DELETED " + sourceFiles.Length + " FILES]" + Environment.NewLine;
        }

        //Event for when tests are selected, load details from matching JSON file
        private void TestSelected(object sender, SelectionChangedEventArgs e)
        {
            //Get JSON file name
            string selectedTechnique = "";

            //Moving between tabs the selected test becomes NULL, need to handle this, so that UI update/JSON retrieval only starts if a valid test is selected.
            try
            {
                selectedTechnique = ComboBoxTechniques.SelectedItem.ToString() + ".json";
                //Continue with loading JSON elements onto UI
                string jsonFile;

                using (StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + "\\TTPs\\" + GetTabString() + "\\" + selectedTechnique)) //Read in JSON file of selected technique
                {
                    jsonFile = reader.ReadToEnd();
                    dynamic jsonContents = JsonConvert.DeserializeObject(jsonFile);
                    //IMPORTANT: All JSON files must follow a consistent set of values, only complex tests need additional value? Can this additional be ignored?

                    //Update UI accordingly
                    valueID.Text = jsonContents.ID;
                    valuePrivs.Text = jsonContents.elevated;
                    valueDesc.Text = jsonContents.desc;
                    arg1.Text = jsonContents.arg1;
                    arg2.Text = jsonContents.arg2;
                    arg3.Text = jsonContents.arg3;
                }
            }
            catch
            {
                //Expected exception for when switching between tabs (the value will always be initially null)
            }

            
        }

        public string GetTabString()
        {
            //Get the current tab
            TabItem tabValue = Tabs.SelectedItem as TabItem;
            _ = Tabs.ToString();
            //Cast tactic from Header object, containing the correct value
            string tactic = (string)tabValue.Header;
            return tactic;
        }

        //When we switch tabs
        private void LoadTechniques(object sender, SelectionChangedEventArgs args) //These functions can also handle UI elements for complex techniques (if value == complex1, then load complexUI e.g. crypter)
        {
            //Clear out previous technique details
            valueID.Text = "";
            valuePrivs.Text = "";
            valueDesc.Text = "";

            string[] CSSourceFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\TTPs\\" + GetTabString(), "*.cs" , SearchOption.AllDirectories);
            string[] CPPsourceFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\TTPs\\" + GetTabString(), "*.cpp", SearchOption.AllDirectories);

            //Load C# tests
            string[] CSTestNames = CSSourceFiles;
            for (int i = 0; i < CSTestNames.Length; i++)
            {
                CSTestNames[i] = Path.GetFileName(CSTestNames[i]);
                CSTestNames[i] = CSTestNames[i].Remove(CSTestNames[i].LastIndexOf(".cs")); //Remove the .cs at the end
            }

            //Load C++ tests
            string[] CPPTestNames = CPPsourceFiles;
            for (int i = 0; i < CPPTestNames.Length; i++)
            {
                CPPTestNames[i] = Path.GetFileName(CPPTestNames[i]);
                CPPTestNames[i] = CPPTestNames[i].Remove(CPPTestNames[i].LastIndexOf(".cpp")); //Remove the .cpp at the end
            }

            string[] TestNames = CSTestNames.Concat(CPPTestNames).ToArray();
            ComboBoxTechniques.ItemsSource = TestNames;
        }

        private void ButtonClickRunTest(object sender, RoutedEventArgs e)
        {
            string testName = ComboBoxTechniques.Text;
            if (testName == "")
            {
                MessageBox.Show("No technique selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } else
            {
                CSharpCodeProvider codeProvider = new CSharpCodeProvider();

                //Does this test have arguments?
                StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + "\\TTPs\\" + GetTabString() + "\\" + testName + ".json"); //Read in JSON file of selected technique

                string jsonFile = reader.ReadToEnd();
                dynamic jsonContents = JsonConvert.DeserializeObject(jsonFile);
                bool hasArgs = false;
                string arguments = "";

                //If there's arguments, process them
                if (jsonContents.hasArgs == "true")
                {
                    hasArgs = true;
                    arguments = ArgumentsTextBox.Text;
                }

                //Must choose TTP to run
                if (jsonContents.isCPP == "false") //C# technique
                {
                    WINTRE.Simulation simulation = new WINTRE.Simulation();
                    simulation.SimulateCS(testName, GetTabString(), codeProvider, arguments, hasArgs);
                }
                else if (jsonContents.isCPP == "true") //C or C++ technique
                {
                    //Simulate a technique that utilises C or C++ source code
                    WINTRE.Simulation simulation = new WINTRE.Simulation();
                    simulation.SimulateCPP(testName, GetTabString(), arguments, hasArgs);
                }

                LogOutput.Text = File.ReadAllText(Directory.GetCurrentDirectory() + "\\WINTRE-log.txt");

                //Assume a technique has been ran at this point
                WINTRE.ReportingFunctions report = new WINTRE.ReportingFunctions();
                report.UpdateReport(GetTabString(), testName);
            }
        }
    }
}
