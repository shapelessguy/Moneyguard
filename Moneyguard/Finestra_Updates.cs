using SevenZip;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moneyguard
{
    public partial class Finestra_Updates : Form
    {
        static Finestra_Updates updates;
        static public string latest_version;
        public Finestra_Updates()
        {
            updates = this;
            InitializeComponent();
            label2.Text = "MoneyGuard V" + Program.Program_Version;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                await CheckNewVersion(false);
            }
            catch (Exception) { MessageBox.Show("Non posso connettermi al server"); }
        }

        public static async Task<bool> CheckNewVersion(bool show_mess)
        {
            await FirebaseClass.DownloadFile_fromStorage(Input.path + @"last_version.txt", "Versions/last_version.txt");
            string latest_version = File.ReadAllText(Input.path + @"last_version.txt");
            try { File.Delete(Input.path + @"\last_version.txt"); } catch (Exception) { Console.WriteLine("Impossible to delete last_version.txt"); }
            int version = Convert.ToInt32(latest_version.Replace(".", ""));
            if (Convert.ToInt32(Program.Program_Version.Replace(".", "")) >= version) { if(!show_mess) MessageBox.Show("La versione attuale è la più recente"); return false; }
            Finestra_Updates.latest_version = latest_version;

            if (Latest_Version.latest_version != null) if (Latest_Version.latest_version.Visible) { Latest_Version.latest_version.BringToFront(); Latest_Version.latest_version.HideRemember();  return false; }
            
            if (!show_mess)
            {
                Latest_Version.latest_version = new Latest_Version(show_mess);
                Latest_Version.latest_version.Show();
            }
            return true;
        }

        public static async Task DownloadLastVersion()
        {
            try
            {
                string filename1 = @"MoneyGuardV" + latest_version + ".exe";
                string filepath1 = Input.path_moneyguard + @"\MoneyGuardV" + latest_version.Replace(".", "_") + ".exe";
            
                if (File.Exists(filepath1))
                {
                    FinestraPrincipale.Finestra.Close();
                    WidgetMoneyguard.ready_toclose = true;
                    Program.install_new_version = true;
                    return;
                }

                Uri url1 = await FirebaseClass.GetUriFile_fromStorage("Versions/MoneyGuardV" + latest_version.Replace(".", "_") + ".exe");
                Uri url2 = await FirebaseClass.GetUriFile_fromStorage("Versions/MoneyGuardV" + latest_version.Replace(".", "_") + ".msi");
                if (url1 == null || url2 == null) { MessageBox.Show("Errore di connessione, riprova più tardi"); return; }
                string filename2 = @"MoneyGuard_Setup.msi";
                string filepath2 = Input.path_moneyguard + @"\MoneyGuard_Setup.msi";
                if (File.Exists(filepath2)) File.Delete(filepath2);
                using (var client = new WebClient())
                {
                    try
                    {
                        client.DownloadFileAsync(url1, filepath1);
                        client.DownloadFileCompleted += (o, e) =>
                        {
                            Console.WriteLine("The file " + filename1 + " has been downloaded\n    ->  on the location " + filepath1);
                            //ExtractArchive(filepath, Input.path);
                        };
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in downloading the file " + filename1);
                    }
                }
                using (var client = new WebClient())
                {
                    try
                    {
                        client.DownloadFileAsync(url2, filepath2);
                        client.DownloadFileCompleted += (o, e) =>
                        {
                            Console.WriteLine("The file " + filename2 + " has been downloaded\n    ->  on the location " + filepath2);
                            FinestraPrincipale.Finestra.Close();
                            WidgetMoneyguard.ready_toclose = true;
                            Program.install_new_version = true;

                        };
                        client.DownloadProgressChanged += (o, e) =>
                        {
                            Latest_Version.latest_version.progressBar1.Visible = true;
                            Latest_Version.latest_version.progressBar1.BringToFront();
                            Latest_Version.latest_version.progressBar1.Value = e.ProgressPercentage;
                        };
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in downloading the file " + filename1);
                    }
                }

            }
            catch (Exception) { MessageBox.Show("Errore sconosciuto, riprova più tardi"); }
        }

        private static ReadOnlyCollection<string> ExtractArchive(string varPathToFile, string varDestinationDirectory)
        {
            ReadOnlyCollection<string> readOnlyArchiveFilenames;
            ReadOnlyCollection<string> readOnlyVolumeFilenames;
            string dllPath = Environment.Is64BitProcess ?
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "7z64.dll")
                    : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "7z.dll");

            SevenZipExtractor.SetLibraryPath(dllPath);
            string fileName = varPathToFile;
            string directory = varDestinationDirectory;

            using (SevenZipExtractor extr = new SevenZipExtractor(fileName))
            {
                readOnlyArchiveFilenames = extr.ArchiveFileNames;
                readOnlyVolumeFilenames = extr.VolumeFileNames;
                try
                {
                    extr.Extracting += extr_Extracting;
                    extr.FileExtractionStarted += extr_FileExtractionStarted;
                    extr.ExtractionFinished += extr_ExtractionFinished;

                    extr.ExtractArchive(directory);
                }
                catch (FileNotFoundException error)
                {
                    MessageBox.Show(error.ToString(), "Error with extraction");
                }
            }
            return readOnlyVolumeFilenames;
        }
        
        private static void extr_FileExtractionStarted(object sender, FileInfoEventArgs e)
        {

        }
        private static void extr_Extracting(object sender, ProgressEventArgs e)
        {

        }
        private static void extr_ExtractionFinished(object sender, EventArgs e)
        {

        }






    }
}
