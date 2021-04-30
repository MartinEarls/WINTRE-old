using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using WINTRE;

namespace WpfApp1
{
    public partial class Page5 : Page
    {
        public static bool reload = false;
        public Page5()
        {
            InitializeComponent();
            LoadTactics();
            LoadCampaigns(reload);
        }

        public void LoadCampaigns(bool reload)
        {
            if(!reload)
            {
                string[] sourceFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Campaigns\\", "*.json");
                dynamic jsonContents = new dynamic[sourceFiles.Length];

                for (int i = 0; i < sourceFiles.Length; i++)
                {
                    StreamReader reader = new StreamReader(sourceFiles[i]);
                    jsonContents[i] = JsonConvert.DeserializeObject(reader.ReadToEnd());
                }

                Campaigns.ItemsSource = jsonContents;
            } else
            {
                //Empty the data grid first
                Campaigns.ItemsSource = null;

                string[] sourceFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Campaigns\\", "*.json");
                dynamic jsonContents = new dynamic[sourceFiles.Length];

                for (int i = 0; i < sourceFiles.Length; i++)
                {
                    StreamReader reader = new StreamReader(sourceFiles[i]);
                    jsonContents[i] = JsonConvert.DeserializeObject(reader.ReadToEnd());
                }

                Campaigns.ItemsSource = jsonContents;
            }
            
        }

        private void LoadTactics()
        {
            string[] tactics = Directory.GetDirectories(Directory.GetCurrentDirectory() + "\\TTPs\\");

            //Remove full path to place in combobox
            for (int i = 0; i < tactics.Length; i++)
            {
                tactics[i] = tactics[i].Replace(Directory.GetCurrentDirectory() + "\\TTPs\\", "");
            }

            SelectedTactic.ItemsSource = tactics;
        }

        private void GetTechniques(string tactic)
        {

            string[] CSSourceFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\TTPs\\" + tactic, "*.cs", SearchOption.AllDirectories);
            string[] CPPsourceFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\TTPs\\" + tactic, "*.cpp", SearchOption.AllDirectories);

            //Load C# tests
            string[] CSTestNames = CSSourceFiles;
            for (int i = 0; i < CSTestNames.Length; i++)
            {
                CSTestNames[i] = System.IO.Path.GetFileName(CSTestNames[i]);
                CSTestNames[i] = CSTestNames[i].Remove(CSTestNames[i].LastIndexOf(".cs")); //Remove the .cs at the end
            }

            //Load C++ tests
            string[] CPPTestNames = CPPsourceFiles;
            for (int i = 0; i < CPPTestNames.Length; i++)
            {
                CPPTestNames[i] = System.IO.Path.GetFileName(CPPTestNames[i]);
                CPPTestNames[i] = CPPTestNames[i].Remove(CPPTestNames[i].LastIndexOf(".cpp")); //Remove the .cpp at the end
            }

            string[] TestNames = CSTestNames.Concat(CPPTestNames).ToArray();
            SelectedTechnique.ItemsSource = TestNames;
        }

        private void ClearCampaignFields_Click(object sender, RoutedEventArgs e)
        {
            CampaignTitle.Text = "";
            CampaignDesc.Text = "";
            TacticsSummary.Text = "";
            TechniquesSummary.Text = "";
        }

        //Save currently selected technique
        private void SelectTechniquesForCampaign_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedTactic.Text == "" || SelectedTechnique.Text == "")
            {
                MessageBox.Show("No tactic/technique selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } else
            {
                TechniquesSummary.Text += "\n" + SelectedTechnique.Text;
            }

            //Add tactic if not added already
            if(!TacticsSummary.Text.Contains(SelectedTactic.Text))
            {
                TacticsSummary.Text += "\n" + SelectedTactic.Text;
            }
        }

        private void LoadTechniques(object sender, EventArgs e)
        {
            if (SelectedTactic.Text == "")
            {
                MessageBox.Show("No tactic selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                GetTechniques(SelectedTactic.Text);
            }
        }

        private void SaveCampaign_Click(object sender, RoutedEventArgs e)
        {
            //Technique name validation, check if it matches the regex and if empty
            Regex alphanumeric = new Regex("^[a-zA-Z0-9\x20]");

            if(CampaignTitle.Text == "" || !alphanumeric.IsMatch(CampaignTitle.Text))
            {
                MessageBox.Show("Invalid campaign name. Please only use alphanumeric characters including the space character.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } else
            {
                Campaign campaign = new Campaign
                {
                    Title = CampaignTitle.Text,
                    Description = CampaignDesc.Text,
                    Tactics = TacticsSummary.Text.Split("\n".ToCharArray()).Skip(1).ToArray(), //Skip first element (new array)
                    Techniques = TechniquesSummary.Text.Split("\n".ToCharArray()).Skip(1).ToArray()
                };

                //Newton JSON, save campaign as JSON file
                string JSONOutput = JsonConvert.SerializeObject(campaign, Formatting.Indented);

                //Check to make sure it doesn't exist already
                if (!File.Exists(Directory.GetCurrentDirectory() + "\\Campaigns\\" + CampaignTitle.Text + ".json"))
                {
                    File.WriteAllText(Directory.GetCurrentDirectory() + "\\Campaigns\\" + CampaignTitle.Text + ".json", JSONOutput);
                    MessageBox.Show("Campaign successfully created. Click the View Campaigns tab to view it.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Campaign already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public class Campaign
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string[] Tactics { get; set; }
            public string[] Techniques { get; set; }
        }

        private void LoadCampaignIntoQueue(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //Filter the object.ToString() to get the actual technique names to be executed so their source code files can be referenced
            try
            {
                string techniques = Campaigns.SelectedItem.ToString().Substring(
                Campaigns.SelectedItem.ToString().IndexOf("\"Techniques\": ["))
                .Replace("\"", "")
                .Replace("Techniques", "")
                .Replace("[", "")
                .Replace("]", "")
                .Replace(",", "")
                .Replace(":", "")
                .Replace("}", "")
                .Replace("  ", "")
                .Trim();

                MessageBox.Show("Campaign techniques loaded into queue.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

                //Write to queue file to be loaded in MainWindow.xaml
                File.WriteAllText(Directory.GetCurrentDirectory() + "\\Campaigns\\Queue\\Queue.json", techniques);

                List<string> list = new List<string>();
                using (StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + "\\Campaigns\\Queue\\Queue.json"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                    ((MainWindow)Application.Current.MainWindow).TechniqueQueue.ItemsSource = list;
                }
            } catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void Reload(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            reload = true;
            LoadCampaigns(reload);
        }
    }
}
