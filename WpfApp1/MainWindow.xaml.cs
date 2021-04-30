using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace WINTRE
{
    public partial class MainWindow : Window
    {
        public string log = "";
        public bool logUpdate = false;
        public MainWindow()
        {
            //Setup stuff here
            InitializeComponent();
            ButtonTechniques.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); //Set startup page
            //Delete temp JSON files from a report
            IEnumerable<string> files = Directory.EnumerateFiles(Directory.GetCurrentDirectory() + "\\Reports\\");
            string[] fileArray = files.ToArray();

            if(files.Count() > 1)
            {
                for (int i = 0; i < files.Count() + 1; i++)
                {
                    if (!fileArray[i].Contains("count"))
                    {
                        File.Delete(fileArray[i]);
                    }
                }
            }

            //Delete log files on startup
            File.Delete(Directory.GetCurrentDirectory() + "\\WINTRE-log.txt");
        }

        private void Button_Click_Techniques(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new Uri("Techniques.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Button_Click_Campaigns(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new Uri("Campaigns.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Button_Click_Custom(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new Uri("Custom.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Button_Click_Reports(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new Uri("Reports.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Skip_Click(object sender, RoutedEventArgs e)
        {
            List<string> techniqueList = TechniqueQueue.Items.Cast<String>().ToList();
            techniqueList.Remove(techniqueList.First());
            //Overwrite queue
            TechniqueQueue.ItemsSource = techniqueList;
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            //Get techniques from listbox as list
            List<string> techniqueList = TechniqueQueue.Items.Cast<String>().ToList();

            //Get all source code files as string[]
            string[] CSSourceFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\TTPs\\", "*.cs", SearchOption.AllDirectories);
            string[] CPPsourceFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\TTPs\\", "*.cpp", SearchOption.AllDirectories);
            string[] SourceFiles = CSSourceFiles.Concat(CPPsourceFiles).ToArray();
            
            if(!techniqueList.Any())
            {
                MessageBox.Show("Queue is empty, goto the campaigns page and select a campaign.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } else
            {
                //Find source code that matches current technique name
                //MessageBox.Show(techniqueList.First());
                string techniquePath = "";

                for(int i = 0; i < SourceFiles.Length; i++)
                {
                    if (SourceFiles[i].Contains(techniqueList.First()))
                    {
                        techniquePath = SourceFiles[i];
                    }
                }

                //Check if technique requires arguments, if yes then error and inform user to run technique manually, else compile and run
                string jsonPath = techniquePath.Replace(".cs", ".json").Replace(".cpp",".json");

                StreamReader reader = new StreamReader(jsonPath);
                dynamic jsonContents = JsonConvert.DeserializeObject(reader.ReadToEnd());

                //Need to check if C# or C++
                if(jsonContents.hasArgs.ToString() == "true")
                {
                    //When instructing user to run the technique manually, need to also inform them of the category/tactic, extract this from the file path.
                    MessageBox.Show("This technique has arguments that must be entered manually on the techniques page. Execute it manually.\n\n"
                        + techniquePath.Substring(techniquePath.IndexOf("\\TTPs\\"))
                        .Replace("\\TTPs\\","")
                        .Replace("\\"," : ")
                        .Replace(".cs","")
                        .Replace(".cpp",""), 
                        "Info", MessageBoxButton.OK, MessageBoxImage.Information);

                    //Assume user will run manually, remove from list
                    techniqueList.Remove(techniqueList.First());
                    //Overwrite queue
                    TechniqueQueue.ItemsSource = techniqueList;

                } else //assuming for now C# test, need to add for CPP test
                {
                    //Get the tactic
                    string tactic = techniquePath.Substring(techniquePath.IndexOf("\\TTPs\\")) //Gives: "tactic\technique.cs"
                        .Replace("\\TTPs\\", "");

                    tactic = tactic.Substring(0, tactic.IndexOf("\\"));

                    //Assume a technique will be ran (due to the nature of the queue)
                    WINTRE.ReportingFunctions report = new WINTRE.ReportingFunctions();
                    report.UpdateReport(tactic, techniqueList.First());

                    //if C#
                    if (jsonContents.isCPP.ToString() == "false")
                    {
                        WINTRE.Simulation simulation = new WINTRE.Simulation();
                        simulation.SimulateCS(techniqueList.First(), tactic);
                        techniqueList.Remove(techniqueList.First());
                        //Overwrite queue
                        TechniqueQueue.ItemsSource = techniqueList;
                    } else //if C++
                    {
                        WINTRE.Simulation simulation = new WINTRE.Simulation();
                        simulation.SimulateCPP(techniqueList.First(), tactic);
                        techniqueList.Remove(techniqueList.First());
                        //Overwrite queue
                        TechniqueQueue.ItemsSource = techniqueList;
                    }
                }
            }
        }
    }
}
