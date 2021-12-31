using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using File = Google.Apis.Drive.v3.Data.File;
using Google.Apis.Drive.v3.Data;
using Microsoft.Win32;
using System.IO;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using Google.Apis.Download;
using System.Xml;
using System.Net;

namespace Moneyguard
{
    static public class GoogleDrive
    {
        static public int limite_secondi = 15;
        static public int num_backups = 20;
        static string[] Scopes = { DriveService.Scope.Drive};
        static string ApplicationName = "MoneyGuard";
        public static UserCredential credential;
        public static DriveService service;
        public static FilesResource.ListRequest listRequest;
        public static bool upload = false;
        public static bool download = true;
        public static bool exit = false;
        public static bool connesso = true;
        public static IList<Google.Apis.Drive.v3.Data.File> files;
        public static List<File> allfiles;
        public static string StandardFields = "nextPageToken, files(id, name, trashed, explicitlyTrashed, modifiedTime, parents, size)";
        public static DateTime last_datetime;
        public static string cyan_id = "";
        public static string moneyguard_id = "";
        public static string icons_id = "";
        public static string backups_id = "";
        public static string tipo_id = "";
        public static string metodo_id = "";
        public const long riga_byte = 130;
        static public bool wait = false;
        static public bool cartelle_sistemate = false;
        static public bool dontworry = false;

        static public void CheckConnection()
        {
            System.Threading.Thread CheckThread = new Thread(Check);
            CheckThread.Start();
        }

        static public void Check()
        {
            Console.WriteLine("Checking connection");
            //try { IList<Google.Apis.Drive.v3.Data.File> files = service.Files.List().Execute().Files; }
            //catch (NullReferenceException) { GetService(); Console.WriteLine("Starting Service"); return; }
            //catch (Exception) { connesso = false; Console.WriteLine("Not Connect"); return; }
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("http://google.com/generate_204")) { }
                }
            }
            catch (Exception)
            {
                connesso = false; Console.WriteLine("Not Connect");
                return;
            }
            connesso = true; Console.WriteLine("Connect");
        }

        static public void GetService()
        {
            System.IO.File.WriteAllBytes(Input.path + "credentials.json", Moneyguard.Properties.Resources.credentials);

            using (var stream = new FileStream(Input.path + "credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = Input.path + "token.json";

                Console.WriteLine("Trying to take credentials");
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, Scopes, "user", CancellationToken.None, new FileDataStore(credPath, true)).Result;
                System.Threading.Thread.Sleep(10);

                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            //service.HttpClient.Timeout = TimeSpan.FromMinutes(100);


        }

        static public void StartDriveThread()
        {
            GoogleDrive.wait = true;
            Console.WriteLine("Start Drive Thread");
            try { GetService(); Console.WriteLine("Servizio Attivo"); } catch (Exception) { wait = false; Console.WriteLine("Non è stato possibile recuperare il servizio"); Console.WriteLine("End Drive Thread"); return;  }
            // List files.
            files = null;

            listRequest = service.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = StandardFields;

            bool connesso = false;
            AttesaDrive.uscita_alarm = false;
            AttesaDrive.ciclo_alarm = -1;

            for (int i=0; i<limite_secondi; i++)
            {
                Console.WriteLine("Checking connection " + i);
                if (!exit)
                {
                    try { files = listRequest.Execute().Files; connesso = true; break; }
                    catch (Exception) { System.Threading.Thread.Sleep(1000); }
                }
            }
            if (!connesso)
            {
                AttesaDrive.uscita_alarm = true;
                wait = false;
                Console.WriteLine("End Drive Thread");
                return;
            }

            AttesaDrive.uscita_alarm = false;
            AttesaDrive.ciclo_alarm = -1;


            try
            {
                try { GetAllDriveFiles(); } catch (Exception e) { if(!AttesaDrive.uscita && !dontworry) MessageBox.Show("Errore nel download da Drive - " + e.Message); }
                
                if (allfiles != null && allfiles.Count > 0)
                {
                    foreach (var file in allfiles)
                    {
                        if (file.Name == "MoneyGuard" && upload) { UploadDataToDrive(service, Input.path_in, file.Id, file.ModifiedTime); upload = false; UpLoadImages(tipo_id, metodo_id); }
                        if (file.Name == "DataV1.txt" && download) { DownloadFileToDrive(service, file.Id, Input.path_in2, file.ModifiedTime, tipo_id, metodo_id); download = false; }
                    }
                }
                AttesaDrive.uscita = true;

                Program.allow_to_connect = true;
                Savings.saving_on_cloud = false;
                Savings.to_save_oncloud = false;
                Savings.save_time = false;
                Savings.icons_changed = false;

                Console.WriteLine("End Thread Google Drive");
            }
            catch (Exception) { Console.WriteLine("Errore di Connessione"); }

            wait = false; Console.WriteLine("End Drive Thread");
            dontworry = false;
        }





        private static string UploadFileToDrive(DriveService service, string filePath, string FolderId, DateTime? created_time)
        {
            try
            {
                var fileMetadata = new File();
                fileMetadata.Name = Path.GetFileName(filePath);
                fileMetadata.MimeType = "text/txt";
                fileMetadata.Parents = new List<string> { FolderId };

                FilesResource.CreateMediaUpload req;
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    req = service.Files.Create(fileMetadata, stream, fileMetadata.MimeType);
                    req.Fields = "id, modifiedTime";
                    req.Upload();
                }
                var file = req.ResponseBody;
                try
                {
                    last_datetime = (DateTime)file.ModifiedTime;
                    System.IO.File.SetLastWriteTime(filePath, last_datetime);
                }
                catch (Exception) { }
                return file.Id;
            }
            catch (Exception e) { if (!dontworry) MessageBox.Show("Errore nell'upload dei file - " + e.Message); dontworry = false; return ""; }
        }

        private static DialogResult dr;
        private static bool dractive = false;
        private static string UploadDataToDrive(DriveService service, string filePath, string FolderId, DateTime? created_time)
        {
            for (int i = 0; i < 10; i++)
            {
                if (Savings.file_occupied) System.Threading.Thread.Sleep(500);
                else break;
            }
            try
            {
                long size_online = 0; long size_local = 0;
                size_local = new System.IO.FileInfo(filePath).Length;
                foreach (var fil in allfiles) if (fil.Name == "DataV1.txt") size_online = (long)fil.Size;
                Console.WriteLine("Dimensione online: " + size_online + ",  Dimensione offline: " + size_local);
                if (size_online != 0 && size_local != 0 && Math.Abs(size_local - size_online) > riga_byte * 10)
                {
                    //if (!FinestraPrincipale.form_closing) { Console.WriteLine("Not saved on Drive"); return ""; }
                    if (dractive) return "";
                    string dim = "";
                    if (size_local - size_online > 0) dim = "grande"; else dim = "piccolo";
                    dr = MessageBox.Show("Si sta cercando di caricare su Drive un file molto più " + dim + " di quello condiviso. Sei sicuro di continuare?", "Verifica", MessageBoxButtons.YesNo);
                    dractive = true;
                    switch (dr)
                    {
                        case DialogResult.Yes: break;
                        case DialogResult.No: { dractive = false; return ""; }
                    }
                    dractive = false;
                }
                foreach (var fil in allfiles) if (fil.Name == "DataV1.txt" && upload) DeleteFile(fil);
                var fileMetadata = new File();
                fileMetadata.Name = Path.GetFileName(filePath);
                fileMetadata.MimeType = "text/txt";
                fileMetadata.Parents = new List<string> { FolderId };

                FilesResource.CreateMediaUpload req;
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    req = service.Files.Create(fileMetadata, stream, fileMetadata.MimeType);
                    req.Fields = "id, modifiedTime";
                    req.Upload();
                }
                var file = req.ResponseBody;
                try
                {
                    last_datetime = (DateTime)file.ModifiedTime;
                    System.IO.File.SetLastWriteTime(filePath, last_datetime);
                }
                catch (Exception) { }

                List<File> files_in_backup = new List<File>();
                var fileMetadata_backup = new File();
                string anno = DateTime.Now.Year.ToString();
                string mese = DateTime.Now.Month.ToString(); if (mese.Length == 1) mese = "0" + mese;
                string giorno = DateTime.Now.Day.ToString(); if (giorno.Length == 1) giorno = "0" + giorno;
                fileMetadata_backup.Name = Path.GetFileName(filePath).Substring(0, Path.GetFileName(filePath).Length - 6) + anno + mese + giorno + Path.GetFileName(filePath).Substring(Path.GetFileName(filePath).Length - 4);
                fileMetadata_backup.MimeType = "text/txt";
                fileMetadata_backup.Parents = new List<string> { backups_id };
                for (int i = 0; i < allfiles.Count; i++)
                    if (allfiles[i].Parents != null)
                    {
                        if ((allfiles[i].Name == fileMetadata_backup.Name || (bool)allfiles[i].Trashed || (bool)allfiles[i].ExplicitlyTrashed)) { DeleteFile(allfiles[i]); Console.WriteLine("Deleting " + allfiles[i].Name); }
                        else if (allfiles[i].Parents[0] == backups_id) files_in_backup.Add(allfiles[i]);
                    }
                FilesResource.CreateMediaUpload req_backup;

                int min = 99999999; int n; int indice = -1;
                if (files_in_backup.Count >= num_backups)
                {
                    for (int i = 0; i < files_in_backup.Count; i++)
                    {
                        if (int.TryParse(files_in_backup[i].Name.Substring(4, files_in_backup[i].Name.Length - 8), out n)) if (n < min) { min = n; indice = i; }
                    }
                    if (indice >= 0) DeleteFile(files_in_backup[indice]);
                }
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    req_backup = service.Files.Create(fileMetadata_backup, stream, fileMetadata_backup.MimeType);
                    req_backup.Fields = "id, modifiedTime";
                    req_backup.Upload();
                }
                var file_backup = req_backup.ResponseBody;
                
                return file.Id;
            }
            catch (Exception e) { if(!dontworry) MessageBox.Show("Errore nell'upload dei dati - " + e.Message); return ""; }
        }

        private static void DeleteFile(File file)
        {
            try
            {
                service.Files.Delete(file.Id).Execute();
            }
            catch (Exception e) { if (!dontworry) MessageBox.Show("Errore di eliminazione su Drive - " + e.Message); }
        }
        public static void DownloadFileToDrive(DriveService service, string fileId, string filePath, DateTime? created_time, string tipo_id, string metodo_id)
        {
            try
            {
                Console.WriteLine("Downloading..");
                FilesResource.GetRequest request = service.Files.Get(fileId);
                
                string fileName = request.Execute().Name;
                
                using (var memoryStream = new MemoryStream())
                {
                    request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
                    {
                        switch (progress.Status)
                        {
                            case DownloadStatus.Downloading:
                                {
                                    Console.WriteLine("Download processing");
                                    break;
                                }
                            case DownloadStatus.Completed:
                                {
                                    //SaveStream(stream1, filePath);
                                    Console.WriteLine("Download complete");
                                    break;
                                }
                            case DownloadStatus.Failed:
                                {
                                    Program.allow_to_connect = false;
                                    Console.WriteLine("Download error");
                                    break;
                                }
                        }
                    };
                    request.Download(memoryStream);
                    SaveStream(memoryStream, filePath, created_time, tipo_id, metodo_id);
                }
                
            }
            catch (Exception) { Program.allow_to_connect = false;
                //MessageBox.Show("Errore nel download dei dati"); 
            }
        }

        private static void SaveStream(MemoryStream stream, string filePath, DateTime? created_time, string tipo_id, string metodo_id)
        {
            string file_online, file_local;
            using (FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.Write)) { stream.WriteTo(file); }
            if (tipo_id == "" && metodo_id == "")
            {
                System.IO.File.SetLastWriteTime(filePath, (DateTime)created_time);
                return;
            }
            file_online = System.IO.File.ReadAllText(filePath);
            file_local = System.IO.File.ReadAllText(Input.path_in);
            DownLoadImages(tipo_id, metodo_id);
            if (file_online != file_local)
            {
                DateTime data_online = new DateTime(created_time.Value.Year, created_time.Value.Month, created_time.Value.Day, created_time.Value.Hour, created_time.Value.Minute, created_time.Value.Second);
                DateTime data_local = System.IO.File.GetLastWriteTime(Input.path_in);

                if (data_online > data_local)
                {
                    System.IO.File.Delete(Input.path_in);
                    System.IO.File.Move(Input.path_in2, Input.path_in);
                    System.IO.File.SetLastWriteTime(Input.path_in, (DateTime)created_time);
                    return;
                }
                else
                {
                    System.IO.File.Delete(Input.path_in2);
                    return;
                }
            }
            System.IO.File.Delete(Input.path_in);
            System.IO.File.Move(Input.path_in2, Input.path_in);
            AttesaDrive.visible = false;
        }

        public static string CreateDir(string title, string FolderId)
        {
            // Creare cartella Moneyguard su Drive
            File fileMetadata = new File();
            fileMetadata.Name = title;
            if(FolderId != "") fileMetadata.Parents = new List<string> { FolderId };
            fileMetadata.MimeType = "application/vnd.google-apps.folder";

            FilesResource.CreateRequest folder = service.Files.Create(fileMetadata);
            folder.Fields = "id";
            var file = folder.Execute();
            return file.Id;
        }

        public static void DownLoadImages(string tipologie, string metodi)
        {
            Console.WriteLine("Start Downloading Images");
            List<Google.Apis.Drive.v3.Data.File> Lista_images_tipologie = new List<Google.Apis.Drive.v3.Data.File>();
            List<Google.Apis.Drive.v3.Data.File> Lista_images_metodi = new List<Google.Apis.Drive.v3.Data.File>();
            List<LocalFile> Lista_images_tipologie_local = new List<LocalFile>();
            List<LocalFile> Lista_images_metodi_local = new List<LocalFile>();
            List<File> Lista_images_tipologie_to_download = new List<File>();
            List<File> Lista_images_metodi__to_download = new List<File>();
            foreach (var file in allfiles)
            {
                if (file.Parents != null)
                {
                    if (file.Parents[0] == tipologie) { Lista_images_tipologie.Add(file); }
                    if (file.Parents[0] == metodi) Lista_images_metodi.Add(file);
                }
            }
            string[] icons_tipologie = System.IO.Directory.GetFiles(Input.path + @"Icons\Tipologie");
            string[] icons_metodi = System.IO.Directory.GetFiles(Input.path + @"Icons\Metodi");
            foreach (string stringa in icons_tipologie) { Lista_images_tipologie_local.Add(new LocalFile(stringa));}
            foreach (string stringa in icons_metodi) Lista_images_metodi_local.Add(new LocalFile(stringa));

            for (int i = 0; i < Lista_images_tipologie_local.Count; i++)
            {
                bool elimina = true;
                for(int k=0; k<Lista_images_tipologie.Count; k++)
                {
                    if (Lista_images_tipologie_local[i].Name == Lista_images_tipologie[k].Name && Lista_images_tipologie_local[i].LastModifiedTime == Lista_images_tipologie[k].ModifiedTime) elimina = false;
                }
                if (elimina)
                {
                    System.IO.File.Delete(Lista_images_tipologie_local[i].path);
                    Lista_images_tipologie_local[i].valid = false;
                }
            }
            for (int k = 0; k < Lista_images_tipologie.Count; k++)
            {
                bool to_download = true;
                for (int i = 0; i < Lista_images_tipologie_local.Count; i++)
                {
                    if(Lista_images_tipologie_local[i].valid) if (Lista_images_tipologie_local[i].Name == Lista_images_tipologie[k].Name && Lista_images_tipologie_local[i].LastModifiedTime == Lista_images_tipologie[k].ModifiedTime) to_download = false;
                }
                if (to_download)
                {
                    AttesaDrive.download_images = true;
                    DownloadFileToDrive(service, Lista_images_tipologie[k].Id, Input.path + @"Icons\Tipologie\" + Lista_images_tipologie[k].Name, Lista_images_tipologie[k].ModifiedTime, "", "");
                }
            }

            for (int i = 0; i < Lista_images_metodi_local.Count; i++)
            {
                bool elimina = true;
                for (int k = 0; k < Lista_images_metodi.Count; k++)
                {
                    if (Lista_images_metodi_local[i].Name == Lista_images_metodi[k].Name && Lista_images_metodi_local[i].LastModifiedTime == Lista_images_metodi[k].ModifiedTime) elimina = false;
                }
                if (elimina)
                {
                    System.IO.File.Delete(Lista_images_metodi_local[i].path);
                    Lista_images_metodi_local[i].valid = false;
                }
            }
            for (int k = 0; k < Lista_images_metodi.Count; k++)
            {
                bool to_download = true;
                for (int i = 0; i < Lista_images_metodi_local.Count; i++)
                {
                    if (Lista_images_metodi_local[i].valid) if (Lista_images_metodi_local[i].Name == Lista_images_metodi[k].Name && Lista_images_metodi_local[i].LastModifiedTime == Lista_images_metodi[k].ModifiedTime) to_download = false;
                }
                if (to_download)
                {
                    AttesaDrive.download_images = true;
                    DownloadFileToDrive(service, Lista_images_metodi[k].Id, Input.path + @"Icons\Metodi\" + Lista_images_metodi[k].Name, Lista_images_metodi[k].ModifiedTime, "", "");
                }
            }

            Console.WriteLine("Finish Downloading Images");
        }
        public static void UpLoadImages(string tipologie, string metodi)
        {
            //UploadFileToDrive(DriveService service, string filePath, string FolderId, DateTime? created_time );
            Console.WriteLine("Start Uploading Images");
            List<Google.Apis.Drive.v3.Data.File> Lista_images_tipologie = new List<Google.Apis.Drive.v3.Data.File>();
            List<Google.Apis.Drive.v3.Data.File> Lista_images_metodi = new List<Google.Apis.Drive.v3.Data.File>();
            List<LocalFile> Lista_images_tipologie_local = new List<LocalFile>();
            List<LocalFile> Lista_images_metodi_local = new List<LocalFile>();
            List<File> Lista_images_tipologie_to_download = new List<File>();
            List<File> Lista_images_metodi__to_download = new List<File>();
            foreach (var file in allfiles)
            {
                if (file.Parents != null)
                {
                    if (file.Parents[0] == tipologie) { Lista_images_tipologie.Add(file); }
                    if (file.Parents[0] == metodi) Lista_images_metodi.Add(file);
                }
            }
            string[] icons_tipologie = System.IO.Directory.GetFiles(Input.path + @"Icons\Tipologie");
            string[] icons_metodi = System.IO.Directory.GetFiles(Input.path + @"Icons\Metodi");
            foreach (string stringa in icons_tipologie) { Lista_images_tipologie_local.Add(new LocalFile(stringa));}
            foreach (string stringa in icons_metodi) Lista_images_metodi_local.Add(new LocalFile(stringa));
            

            if (Lista_images_tipologie.Count > 0)
            {
                for (int k = 0; k < Lista_images_tipologie.Count; k++)
                {
                    bool elimina = true;
                    for (int i = 0; i < Lista_images_tipologie_local.Count; i++)
                    {
                        if (Lista_images_tipologie_local[i].Name == Lista_images_tipologie[k].Name && Lista_images_tipologie_local[i].LastModifiedTime == Lista_images_tipologie[k].ModifiedTime) { elimina = false; }
                    }
                    if (elimina)
                    {
                        AttesaDrive.upload_images = true;
                        Console.WriteLine("Eliminazione: " + Lista_images_tipologie[k].Name + "___________Tipologie");
                        DeleteFile(Lista_images_tipologie[k]);
                    }
                }
            }
            if(Lista_images_metodi.Count > 0)
            {
                for (int k = 0; k < Lista_images_metodi.Count; k++)
                {
                    bool elimina = true;
                    for (int i = 0; i < Lista_images_metodi_local.Count; i++)
                    {
                        if (Lista_images_metodi_local[i].Name == Lista_images_metodi[k].Name && Lista_images_metodi_local[i].LastModifiedTime == Lista_images_metodi[k].ModifiedTime) elimina = false;
                    }
                    if (elimina)
                    {
                        AttesaDrive.upload_images = true;
                        Console.WriteLine("Eliminazione: " + Lista_images_metodi[k].Name + "___________Metodi");
                        DeleteFile(Lista_images_metodi[k]);
                    }
                }
            }


            try { GetAllDriveFiles(); } catch (Exception e) { if (!dontworry) MessageBox.Show("Errore nel download da Drive - "+ e.Message); }
            Lista_images_tipologie.Clear();
            Lista_images_metodi.Clear();
            foreach (var file in allfiles)
            {
                if (file.Parents != null)
                {
                    if (file.Parents[0] == tipologie) { Lista_images_tipologie.Add(file); }
                    if (file.Parents[0] == metodi) Lista_images_metodi.Add(file);
                }
            }

            if (Lista_images_tipologie_local.Count > 0)
            {
                for (int i = 0; i < Lista_images_tipologie_local.Count; i++)
                {
                    bool to_upload = true;
                    for (int k = 0; k < Lista_images_tipologie.Count; k++)
                    {
                        if (Lista_images_tipologie_local[i].Name == Lista_images_tipologie[k].Name && Lista_images_tipologie_local[i].LastModifiedTime == Lista_images_tipologie[k].ModifiedTime) to_upload = false;
                    }
                    if (to_upload && Lista_images_tipologie_local.Count > 0)
                    {
                        AttesaDrive.upload_images = true;
                        UploadFileToDrive(service, Input.path + @"Icons\Tipologie\" + Lista_images_tipologie_local[i].Name, tipologie, Lista_images_tipologie_local[i].LastModifiedTime);
                        Console.WriteLine("Uploading: " + Lista_images_tipologie_local[i].Name + "___________Tipologie");
                    }
                }
            }
            if (Lista_images_metodi_local.Count > 0)
            {
                for (int i = 0; i < Lista_images_metodi_local.Count; i++)
                {
                    bool to_upload = true;
                    for (int k = 0; k < Lista_images_metodi.Count; k++)
                    {
                        if (Lista_images_metodi_local[i].Name == Lista_images_metodi[k].Name && Lista_images_metodi_local[i].LastModifiedTime == Lista_images_metodi[k].ModifiedTime) to_upload = false;
                    }
                    if (to_upload)
                    {
                        AttesaDrive.upload_images = true;
                        UploadFileToDrive(service, Input.path + @"Icons\Metodi\" + Lista_images_metodi_local[i].Name, metodi, Lista_images_metodi_local[i].LastModifiedTime);
                        Console.WriteLine("Uploading: " + Lista_images_metodi_local[i].Name + "___________Metodi");
                    }
                }
            }


            Console.WriteLine("Finish Uploading Images");
        }

        public static void GetAllDriveFiles()
        {
            GetService();
            allfiles = new List<File>();
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 1000;
            listRequest.Fields = StandardFields;

            var result = listRequest.Execute();
            IList<Google.Apis.Drive.v3.Data.File> files = result.Files;

            bool inizio = true; bool esci = false; string id_initial = "";
            while (files != null && files.Count > 0)
            {
                inizio = true;
                foreach (var file in files)
                {
                    if (inizio) { if (file.Id == id_initial) { esci = true; break; } id_initial = file.Id; inizio = false; allfiles.Add(file); }
                    else { if (file.Id == id_initial) { esci = true; break; } allfiles.Add(file); }
                }
                if (esci) break;
                if (!string.IsNullOrWhiteSpace(result.NextPageToken))
                {
                    listRequest = service.Files.List();
                    listRequest.PageToken = result.NextPageToken;
                    listRequest.PageSize = 100;
                    listRequest.Fields = StandardFields;
                    result = listRequest.Execute();
                    files = result.Files;
                }

            }
            SistemazioneCartelle();
        }

        public static string GetNameByID(string id)
        {
            foreach (var file in allfiles)
            {
                if (file.Id == id)
                {
                    return file.Name;
                }
            }
            return "_none_";
        }
        public static void GetInfoByFile(File file)
        {
            Console.WriteLine("INFO OF " + file.Name);
            foreach (string stringa in file.Parents) Console.WriteLine("   - " + GetNameByID(stringa) + " .. of " + file.Parents.Count);
            Console.WriteLine(" " + "TRASHED: " + ((bool)file.Trashed || (bool)file.ExplicitlyTrashed));
            Console.WriteLine("END INFO");
        }
        public static bool IsFileInRoot(File file)
        {
            foreach (string stringa in file.Parents) if (GetNameByID(stringa) == "_none_") return true; else return false;
            return true;
        }

        public static void SistemazioneCartelle()
        {
            if (cartelle_sistemate) return;
            if (allfiles != null && allfiles.Count > 0)
            {
                bool exists = false;
                foreach (var file in allfiles)
                {
                    if (file.Name == "Cyan")
                    {
                        exists = true;
                        Console.WriteLine("Folder already existing");
                        GetInfoByFile(file);
                        if ((bool)file.Trashed || (bool)file.ExplicitlyTrashed || !IsFileInRoot(file)) { DeleteFile(file); exists = false; }
                        else cyan_id = file.Id;
                    }
                }
                if (!exists)
                {
                    Console.WriteLine("Creazione di Cyan, MoneyGuard, Backups, Icons, Tipologie e Metodi");
                    cyan_id = CreateDir("Cyan", "");
                    moneyguard_id = CreateDir("MoneyGuard", cyan_id);
                    icons_id = CreateDir("Icons", moneyguard_id);
                    backups_id = CreateDir("Backups", moneyguard_id);
                    tipo_id = CreateDir("Tipologie", icons_id);
                    metodo_id = CreateDir("Metodi", icons_id);
                }
                else
                {
                    foreach (var file in allfiles)
                    {
                        if (file.Name == "MoneyGuard") { GetInfoByFile(file); exists = true; Console.WriteLine("Folder already existing"); if ((bool)file.Trashed || (bool)file.ExplicitlyTrashed) { DeleteFile(file); exists = false; } else moneyguard_id = file.Id; }
                    }
                    foreach (var file in allfiles)
                    {
                        if (file.Name == "DataV1.txt" && ((bool)file.Trashed || (bool)file.ExplicitlyTrashed || file.Parents[0] != moneyguard_id)) { DeleteFile(file); }
                    }
                    if (!exists)
                    {
                        Console.WriteLine("Creazione di MoneyGuard, Backups, Icons, Tipologie e Metodi");
                        moneyguard_id = CreateDir("MoneyGuard", "");
                        icons_id = CreateDir("Icons", moneyguard_id);
                        backups_id = CreateDir("Backups", moneyguard_id);
                        tipo_id = CreateDir("Tipologie", icons_id);
                        metodo_id = CreateDir("Metodi", icons_id);
                    }
                    else
                    {
                        exists = false; bool back_exists = false;
                        foreach (var file in allfiles)
                        {
                            if (file.Parents != null) if (file.Name == "Icons" && file.Parents[0] == moneyguard_id) { GetInfoByFile(file); exists = true; if ((bool)file.Trashed || (bool)file.ExplicitlyTrashed) { DeleteFile(file); exists = false; } else icons_id = file.Id; }
                            if (file.Parents != null) if (file.Name == "Backups" && file.Parents[0] == moneyguard_id) { GetInfoByFile(file); back_exists = true; if ((bool)file.Trashed || (bool)file.ExplicitlyTrashed) { DeleteFile(file); back_exists = false; } else backups_id = file.Id; }
                        }
                        if (!back_exists)
                        {
                            Console.WriteLine("Creazione di Backups");
                            backups_id = CreateDir("Backups", moneyguard_id);
                        }
                        if (!exists)
                        {
                            Console.WriteLine("Creazione di Icons, Tipologie e Metodi");
                            icons_id = CreateDir("Icons", moneyguard_id);
                            tipo_id = CreateDir("Tipologie", icons_id);
                            metodo_id = CreateDir("Metodi", icons_id);
                        }
                        else
                        {
                            bool exists1 = false; bool exists2 = false;
                            foreach (var file in allfiles)
                            {
                                if (file.Parents != null) if (file.Name == "Tipologie" && file.Parents[0] == icons_id) { GetInfoByFile(file); exists1 = true; if ((bool)file.Trashed || (bool)file.ExplicitlyTrashed) { DeleteFile(file); exists1 = false; } else tipo_id = file.Id; }
                                if (file.Parents != null) if (file.Name == "Metodi" && file.Parents[0] == icons_id) { GetInfoByFile(file); exists2 = true; if ((bool)file.Trashed || (bool)file.ExplicitlyTrashed) { DeleteFile(file); exists2 = false; } else metodo_id = file.Id; }
                            }
                            if (!exists1)
                            {
                                Console.WriteLine("Creazione di Tipologie");
                                tipo_id = CreateDir("Tipologie", icons_id);
                            }
                            if (!exists2)
                            {
                                Console.WriteLine("Creazione di Metodi");
                                metodo_id = CreateDir("Metodi", icons_id);
                            }
                        }
                    }
                }
            }

            else
            {
                Console.WriteLine("Creazione di Cyan, MoneyGuard, Backups, Icons, Tipologie e Metodi");
                cyan_id = CreateDir("Cyan", "");
                moneyguard_id = CreateDir("MoneyGuard", cyan_id);
                icons_id = CreateDir("Backups", moneyguard_id);
                icons_id = CreateDir("Icons", moneyguard_id);
                tipo_id = CreateDir("Tipologie", icons_id);
                metodo_id = CreateDir("Metodi", icons_id);
            }
            cartelle_sistemate = true;
        }
    }
}
