using Newtonsoft.Json;
using System.IO;
using System.Windows;
using Page = System.Windows.Controls.Page;
using MessageBox = System.Windows.MessageBox;
using System.Text.RegularExpressions;

namespace WpfApp1
{
    public partial class Page4 : Page
    {
        public static bool reporting = false;
        public static bool preview = false;
        public static int techniqueID = 0;
        public static bool refresh = false;

        public Page4()
        {
            InitializeComponent();
        }

        private void EnableReporting(object sender, RoutedEventArgs e)
        {
            //Need to validate filenames
            Regex alphanumeric = new Regex("^[a-zA-Z0-9\x20]");

            if (ReportTitle.Text == "" || ReportDesc.Text == "")
            {
                MessageBox.Show("Title or description values are empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } else if(!alphanumeric.IsMatch(ReportTitle.Text) || !alphanumeric.IsMatch(ReportDesc.Text))
            {
                MessageBox.Show("Invalid title! Please only use alphanumeric characters including the space character.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } else
            {
                reporting = true;

                //Report properties to be added to the report
                Report report = new Report
                {
                    reportTitle = ReportTitle.Text,
                    reportDesc = ReportDesc.Text
                };

                //Serialize and prepare for write to disk
                string JSONOutput = JsonConvert.SerializeObject(report, Formatting.Indented);
                File.WriteAllText(Directory.GetCurrentDirectory() + "\\Reports\\currentReportProperties.json", JSONOutput);
                MessageBox.Show("Reporting is now enabled!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ExportReportClick(object sender, RoutedEventArgs e)
        {
            //If less than 2 .json files => no techniques have been ran don't run this
            WINTRE.ReportingFunctions report = new WINTRE.ReportingFunctions();
            report.GenerateReport(preview);
        }

        private void GeneratePreview_Click(object sender, RoutedEventArgs e)
        {
            if (!reporting)
            {
                System.Windows.MessageBox.Show("Reporting is not enabled.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {

                preview = true;
                //Basically the same as saving the report but it's a temp docx file. Need to convert to XPS for DocPreview https://www.c-sharpcorner.com/UploadFile/mahesh/viewing-word-documents-in-wpf/
                
                string[] jsonFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Reports\\", "*.json");

                //If less than 2 .json files => no techniques have been ran don't run this
                if (!(jsonFiles.Length < 2) && refresh == false) 
                {
                    LoadReportIntoView(refresh);
                    refresh = true;
                } else if (refresh == true)
                { //Refreshing https://stackoverflow.com/questions/283027/wpf-documentviewer-doesnt-release-the-xps-file

                    
                    string[] xpsFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Reports\\", "*.xps");
                    System.Uri uri = new System.Uri(xpsFiles[0]);
                    //Get the XpsPackage itself
                    var theXpsPackage = System.IO.Packaging.PackageStore.GetPackage(uri);
                    //Close to remove file lock
                    theXpsPackage.Close();
                    System.IO.Packaging.PackageStore.RemovePackage(uri);

                    if (File.Exists(xpsFiles[0])) //delete and re-generate new one
                    {
                        File.Delete(xpsFiles[0]);
                    }

                    LoadReportIntoView(refresh);
                }
                else
                { 
                    EmptyReport();
                }
                
            }
        }

        private void LoadReportIntoView(bool refresh)
        {
            WINTRE.ReportingFunctions report = new WINTRE.ReportingFunctions();
            report.GenerateReport(preview);
            string[] docFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Reports\\", "*.docx");

            //Should be one file
            DocPreview.Document = report.ConvertWordDocToXPSDoc(docFiles[docFiles.Length-1], docFiles[docFiles.Length-1].Replace(".docx", ".xps")).GetFixedDocumentSequence();
            if(refresh)
            {
                MessageBox.Show("Report updated in document viewer.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EmptyReport()
        {
            //Do not process an empty report
            MessageBox.Show("Error, no techniques have been ran yet?", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

        
}

public class Report
{
    public string reportTitle;
    public string reportDesc;
}

public class ReportItem
{
    public int techniqueID;
    public string technique;
    public string techniqueMITREID;
    public string time;
}
