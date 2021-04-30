using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1
{
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();
        }

        private void TextChangedDefenceNotes(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DescBox.Text == "e.g. Monitor for the usage of \"net user\", this can be achieved using Sysmon and filtering EventData based on CommandLine values")
            {
                DescBox.Text = "";
            }
        }

        private void ClickAddNewTechnique(object sender, System.Windows.RoutedEventArgs e)
        {
            Regex alphanumeric = new Regex("^[a-zA-Z0-9\x20]");

            //Required inputs
            if (Elevated.Text == "" || SelectedTactic.Text == "" || CommandInput.Text == "" || DescBox.Text == "" 
                || ID.Text == "" || TechniqueName.Text == "" || !(alphanumeric.IsMatch(TechniqueName.Text)))
            {
                MessageBox.Show("Invalid technique name. Please only use alphanumeric characters including the space character.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } else
            {
                Technique tech = new Technique
                {
                    name = TechniqueName.Text,
                    ID = ID.Text,
                    elevated = Elevated.Text,
                    tactic = SelectedTactic.Text,
                    template = SelectedTemplate.Text,
                    desc = DescBox.Text,
                    hasArgs = "false",
                    arg1 = "N/A",
                    arg2 = "N/A",
                    arg3 = "N/A",
                    isCPP = "false",

                    //Escape command
                    commands = EscapeCommand(CommandInput.Text)
                };
                tech.commands = tech.commands.TrimStart(' ', '"');
                tech.commands = tech.commands.TrimStart(' ', '"');
                tech.commands = tech.commands.Remove(tech.commands.Length - 1, 1);

                //Newton JSON, serialize the data for export
                string JSONOutput = JsonConvert.SerializeObject(tech, Formatting.Indented);
                //CustomLogOutput.Text = "[INFO: Serializing input to JSON]" + JSONOutput;

                //Output our JSON information so it can read (automatically) by the techniques page. Output using category + name.
                File.WriteAllText(Directory.GetCurrentDirectory() + "\\TTPs\\" + tech.tactic + "\\" + tech.name + ".json", JSONOutput); //Should be saved as .json file
                Console.WriteLine("[INFO: Saving new technique as JSON " + Directory.GetCurrentDirectory() + "\\TTPs\\" + tech.tactic + "\\" + tech.name + ".json]");

                //Copy template source file, to create new technique
                string templateSource;
                if (tech.template == "CMD")
                {
                    
                    using (StreamReader streamReader = new StreamReader(Directory.GetCurrentDirectory() + "\\TTPs\\Template.txt", Encoding.UTF8))
                    {
                        templateSource = streamReader.ReadToEnd();
                    }

                    //Replace UUIDs with custom technique properties
                    templateSource = templateSource.Replace("fd98b648-6624-4461-abcf-5d1ad11a2839e", tech.name.Replace(" ", "")); //Function and function call (name of the technique, remove spaces)
                    templateSource = templateSource.Replace("447912f6-0748-45b6-8b1e-4876333eeaf8", tech.template); //CMD or PowerShell
                    templateSource = templateSource.Replace("86c7c306-0c66-4919-9023-54cedcc0359f", tech.commands);
                } else //PowerShell
                {
                    using (StreamReader streamReader = new StreamReader(Directory.GetCurrentDirectory() + "\\TTPs\\TemplatePS.txt", Encoding.UTF8))
                    {
                        templateSource = streamReader.ReadToEnd();
                    }
                    templateSource = templateSource.Replace("fd98b648-6624-4461-abcf-5d1ad11a2839e", tech.name.Replace(" ", "")); //Function and function call (name of the technique, remove spaces)
                    templateSource = templateSource.Replace("447912f6-0748-45b6-8b1e-4876333eeaf8",  "powershell"); //CMD or PowerShell
                    templateSource = templateSource.Replace("86c7c306-0c66-4919-9023-54cedcc0359f", tech.commands);
                }


                //Save new source file, should be readable from Techniques page and compilable (assuming the command is valid, add checker for valid command?)
                File.WriteAllText(Directory.GetCurrentDirectory() + "\\TTPs\\" + tech.tactic + "\\" + tech.name + ".cs", templateSource); //Should be saved as .json file
                //CustomLogOutput.Text = "[INFO: Saving new technique as C# source code " + Directory.GetCurrentDirectory() + "\\TTPs\\" + tech.tactic + "\\" + tech.name + ".cs]";
                MessageBox.Show("Technique \"" + tech.name + "\" " + "added, view it on the techniques page.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            
        }
        public string EscapeCommand(string input)
        {
            using (var strWriter = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), strWriter, null);
                    return strWriter.ToString();
                }
            }
        }

        private void CommandInput_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(CommandInput.Text == "e.g. net user")
            {
                CommandInput.Text = "";
            }
        }
    }

    public class Technique
    {
        public string name;
        public string ID;
        public string elevated;
        public string tactic;
        public string template;
        public string commands;
        public string desc;
        public string hasArgs;
        public string arg1;
        public string arg2;
        public string arg3;
        public string isCPP;
    }
}