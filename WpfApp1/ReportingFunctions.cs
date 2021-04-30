using Microsoft.Office.Interop.Word;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Xps.Packaging;

//1. Save techniques to JSON (separated by tactic)
//2. When generating report preview, add table based on JSON data
namespace WINTRE
{

    public class ReportingFunctions
    {
        public static List<ReportItem>[] techniqueGroups = new List<ReportItem>[9];
        public void UpdateReport(string tactic, string techniqueName)
        {
            if (WpfApp1.Page4.reporting) //is enabled
            {

                //Get MITREID for report
                string fileContents = File.ReadAllText(Directory.GetCurrentDirectory() + "\\TTPs\\" + tactic + "\\" + techniqueName + ".json");
                dynamic jsonContents = JsonConvert.DeserializeObject(fileContents);

                ReportItem item = new ReportItem
                {
                    techniqueID = WpfApp1.Page4.techniqueID++,
                    time = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"), //Set time
                    technique = techniqueName,
                    techniqueMITREID = jsonContents.ID,
                };
                //update WpfApp1.Page4.techniqueID
                WpfApp1.Page4.techniqueID = WpfApp1.Page4.techniqueID++;

                //Get corresponding tactic/category to match each potential list
                var dirs = Directory.GetDirectories(Directory.GetCurrentDirectory() + "\\TTPs\\");
                string[] tacticList = dirs.ToArray();

                //Get index so correct list is updated (techniques are sorted into matching tactic/category of technique)
                int techniqueGroupIndex = 0;
                for (int i = 0; i < 9; i++)
                {
                    if ((Directory.GetCurrentDirectory() + "\\TTPs\\" + tactic).Equals(tacticList[i]))
                    {
                        techniqueGroupIndex = i;
                        //Update list
                        //Initialize if first entry
                        if (techniqueGroups[techniqueGroupIndex] == null)
                        {
                            techniqueGroups[techniqueGroupIndex] = new List<ReportItem>
                            {
                                item
                            };
                        }
                        else
                        {
                            techniqueGroups[techniqueGroupIndex].Add(item);
                        }

                        i = 9; //break loop
                    }
                }

                //Serialize to tactic.json
                string JSONOutput = JsonConvert.SerializeObject(techniqueGroups[techniqueGroupIndex], Formatting.Indented); //Serialize updated list with newly added item
                File.WriteAllText(Directory.GetCurrentDirectory() + "\\Reports\\" + tactic + ".json", JSONOutput);

            } //else do not report
        }

        //Preview is the same thing as exporting the report, but it needs to be loaded into the DocPreview
        public void GenerateReport(bool preview)
        {
            if (WpfApp1.Page4.reporting)
            {
                //Increment count
                int count = Int32.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + "\\Reports\\count"));
                count++;
                File.WriteAllText(Directory.GetCurrentDirectory() + "\\Reports\\count", count.ToString());

                //Get tactics
                var files = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Reports\\").
                    Where(name => !name.Contains("count")).
                    Where(name => !name.Contains("currentReportProperties.json")).
                    Where(name => !name.Contains(".docx")).
                    Where(name => !name.Contains(".xps"));

                //Deserialize so that each tactic/technique collection is a new list https://www.newtonsoft.com/json/help/html/SerializingCollections.htm#DeserializingDictionaries
                List<ReportItem>[] techniqueGroups = new List<ReportItem>[files.Count()]; //Should equal number of tactics, i.e. number of tables in the report

                //Has to be an array so we can iterate on initizilation of the deserialized objects
                string[] newFiles = files.ToArray();

                //Deserialize into the List array
                for (int i = 0; i < files.Count(); i++)
                {
                    string jsonContents = File.ReadAllText(newFiles[i]);
                    try
                    {
                        techniqueGroups[i] = JsonConvert.DeserializeObject<List<ReportItem>>(jsonContents);
                    } catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }

                //WORD DOC STUFF
                //Create a new document  
                Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();
                //Create a missing variable for missing value  
                object missing = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Word.Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);

                //Get reportID/count
                int reportCount = Int32.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + "\\Reports\\count")); //Only update after report is "finalised" by exporting

                //Set animation status for word application  
                winword.ShowAnimation = false;

                //Set status for word application is to be visible or not.  
                winword.Visible = false;

                //At the start of the report generation, add the title/desc/report ID
                //Get Title/Desc
                dynamic newReport = JsonConvert.DeserializeObject(File.ReadAllText(Directory.GetCurrentDirectory() + "\\Reports\\currentReportProperties.json"));

                foreach (Microsoft.Office.Interop.Word.Section section in document.Sections)
                {
                    //Get the header range and add the header details.  
                    Microsoft.Office.Interop.Word.Range headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    headerRange.Fields.Add(headerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
                    headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    headerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdBlack;
                    headerRange.Font.Size = 20;
                    headerRange.Text = newReport.reportTitle;
                }

                //Report text
                document.Content.SetRange(0, 0);
                document.Content.Text += "Description: " + newReport.reportDesc + Environment.NewLine
                    + "Report ID: " + count + Environment.NewLine;

                //Paragraph
                //Add paragraph with Heading 1 style  
                Microsoft.Office.Interop.Word.Paragraph para1 = document.Content.Paragraphs.Add(ref missing);
                object styleHeading1 = "Heading 1";
                para1.Range.set_Style(ref styleHeading1);
                para1.Range.Text = "Adversary Simulation Test";
                para1.Range.InsertParagraphAfter();

                //Make a table for each list/category of technique
                string tableTitle = "";

                Microsoft.Office.Interop.Word.Table[] tableArray = new Microsoft.Office.Interop.Word.Table[techniqueGroups.Length];

                for (int i = 0; i < techniqueGroups.Length; i++)
                {
                    tableArray[i] = document.Tables.Add(para1.Range, techniqueGroups[i].Count + 1, 5, ref missing, ref missing);

                    tableArray[i].Borders.Enable = 1;
                    foreach (Row row in tableArray[i].Rows)
                    {
                        foreach (Cell cell in row.Cells)
                        {
                            //HEADER ROW
                            if (cell.RowIndex == 1)
                            {
                                cell.Range.Font.Bold = 1;
                                cell.Range.Font.Name = "Times New Roman";
                                cell.Range.Font.Size = 12;
                                cell.Shading.BackgroundPatternColor = WdColor.wdColorBlueGray;
                                //Center alignment for the Header cells  
                                cell.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                                cell.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

                                if (cell.ColumnIndex == 1)
                                {
                                    cell.Range.Text = "#";
                                }
                                else if (cell.ColumnIndex == 2)
                                {
                                    cell.Range.Text = "Technique";
                                }
                                else if (cell.ColumnIndex == 3)
                                {
                                    cell.Range.Text = "Detected";
                                }
                                else if (cell.ColumnIndex == 4)
                                {
                                    cell.Range.Text = "T#";
                                }
                                else if (cell.ColumnIndex == 5)
                                {
                                    cell.Range.Text = "Date/Time";
                                }

                            }
                            //DATA ROWS, keep in the mind the "Detected" column is left empty (utilise columnIndexes 0,1,3,4)
                            else
                            {
                                if (cell.RowIndex - 2 < techniqueGroups[i].Count)
                                {
                                    if (cell.ColumnIndex == 1)
                                    {
                                        cell.Range.Text = techniqueGroups[i].ElementAt(cell.RowIndex - 2).techniqueID++.ToString();
                                    }
                                    else if (cell.ColumnIndex == 2)
                                    {
                                        cell.Range.Text = techniqueGroups[i].ElementAt(cell.RowIndex - 2).technique;
                                    }
                                    else if (cell.ColumnIndex == 4)
                                    {
                                        cell.Range.Text = techniqueGroups[i].ElementAt(cell.RowIndex - 2).techniqueMITREID;
                                    }
                                    else if (cell.ColumnIndex == 5)
                                    {
                                        cell.Range.Text = techniqueGroups[i].ElementAt(cell.RowIndex - 2).time;
                                    }
                                }
                            }
                        }
                    }
                    //Paragraph after table
                    Microsoft.Office.Interop.Word.Paragraph paraBlank = document.Content.Paragraphs.Add(ref missing);
                    tableTitle = newFiles[i].Substring(newFiles[i].LastIndexOf("\\") + 1).Replace(".json", ""); //Filter the file path string
                    paraBlank.Range.Text = tableTitle;
                    paraBlank.Range.InsertParagraphAfter();
                }

                //Save the document  
                if (preview == false)
                {
                    object filename = Directory.GetCurrentDirectory() + "\\Reports\\Complete\\" + count + newReport.reportTitle + ".docx";
                    document.SaveAs2(ref filename);
                    document.Close(ref missing, ref missing, ref missing);
                    document = null;
                    winword.Quit(ref missing, ref missing, ref missing);
                    winword = null;
                    MessageBox.Show("Report template created successfully!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

                    //Open the new word doc
                    Process technique = new Process();
                    technique.StartInfo.FileName = "explorer"; //specify folder here?
                    technique.StartInfo.UseShellExecute = false;
                    technique.StartInfo.CreateNoWindow = false;
                    technique.StartInfo.RedirectStandardOutput = true;
                    technique.StartInfo.RedirectStandardError = true;
                    technique = Process.Start(technique.StartInfo.FileName, filename.ToString());
                }
                else
                {

                    //Then save as normal
                    object filename = Directory.GetCurrentDirectory() + "\\Reports\\" + count + newReport.reportTitle + "_temp" + ".docx";
                    document.SaveAs2(ref filename);
                    document.Close(ref missing, ref missing, ref missing);
                    document = null;
                    winword.Quit(ref missing, ref missing, ref missing);
                    winword = null;
                }
                
            }
        }

        public XpsDocument ConvertWordDocToXPSDoc(string wordDocName, string xpsDocName)

        {
            //Create a WordApplication and load the existing document
            Microsoft.Office.Interop.Word.Application wordApplication = new Microsoft.Office.Interop.Word.Application();      
            wordApplication.Documents.Add(wordDocName);
            Document doc = wordApplication.ActiveDocument;

            try
            {
                doc.SaveAs(xpsDocName, WdSaveFormat.wdFormatXPS);
                wordApplication.Quit();
                XpsDocument xpsDoc = new XpsDocument(xpsDocName, System.IO.FileAccess.Read);
                return xpsDoc;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            return null;
        }
    }

    public class ReportItem
    {
        public int techniqueID;
        public string technique;
        public string techniqueMITREID;
        public string time;
    }
}