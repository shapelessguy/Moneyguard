using Firebase.Auth;
using Firebase.Database;
using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moneyguard
{
    public class FirebaseClass
    {
        static public FirebaseAuthLink token = null;
        static public FirebaseAuthProvider auth;
        static public bool connesso = false;

        static public async Task<bool> CheckConnection()
        {
            //string externalObj;
            try
            {
                WebClient client = new WebClient();
                //client.OpenReadCompleted += (o, e) => externalObj = e.Result.ToString();
                Stream stream = await client.OpenReadTaskAsync(new Uri("http://google.com/generate_204", UriKind.Absolute));
            }
            catch (Exception)
            {
                connesso = false;
                return false;
            }
            connesso = true;
            return true;
        }
        static public void Check_Connection()
        {
            //string externalObj;
            try
            {
                WebClient client = new WebClient();
                //client.OpenReadCompleted += (o, e) => externalObj = e.Result.ToString();
                Stream stream = client.OpenRead(new Uri("http://google.com/generate_204", UriKind.Absolute));
            }
            catch (Exception)
            {
                connesso = false;
                return;
            }
            connesso = true;
        }

        static public async Task<string> GetProvider()
        {
            const string firebaseApiKey = "AIzaSyCkMWnAG5jv_kSIrKIy2o8ybi6dBtvluB0";

            if (!await CheckConnection()) { return "Errore di connessione"; }
            try
            {
                auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(firebaseApiKey));
                Program.operations_create_provider++;
            }
            catch (Exception) { return "Errore di connessione al server, riprova più tardi"; }
            return "";
        }
        static public async Task<string> CheckFirebaseCred(string firebaseUsername, string firebasePassword)
        {
            Console.WriteLine(await GetProvider());
            bool tryAgain = true;
            int iteration = 0;
            string error = "Errore";
            do
            {
                iteration++;
                if (iteration > 10) { return "Errore di autenticazione sconosciuto, riprova più tardi"; }
                Console.WriteLine("Trying to authenticate.. iteration: {0}", iteration);
                try
                {
                    token = await auth.SignInWithEmailAndPasswordAsync(firebaseUsername, firebasePassword);
                    Program.operations_create_token++;
                    tryAgain = false;
                }
                catch (FirebaseAuthException faException)
                {
                    Console.WriteLine(faException.Reason.ToString());
                    if (faException.Reason.ToString() == "InvalidEmailAddress") error = "E-mail non valida";
                    else if (faException.Reason.ToString() == "UnknownEmailAddress") error = "E-mail non registrata";
                    else if (faException.Reason.ToString() == "WrongPassword" || faException.Reason.ToString() == "Undefined") error = "Password errata";
                    else error = "Errore di autenticazione";

                    //MessageBox.Show(error);
                    tryAgain = false;
                    return error;
                    //token = await auth.CreateUserWithEmailAndPasswordAsync(firebaseUsername, firebasePassword, "Greg", false);
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            while (tryAgain);
            return "";
        }
        static public bool FireBaseLogIn()
        {
            const string firebaseUrl = "https://mobilemoneyguard.firebaseio.com/";
            try
            {
                var firebase = new FirebaseClient(firebaseUrl, new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(token.FirebaseToken) });
                Program.operations_create_client++;
            }
            catch (Exception) {return false; }
            Console.WriteLine("User Authenticated - " + token.User.Email);
            return true;
        }
        
        static public async Task FireBaseLogIn(string firebaseUsername, string firebasePassword)
        {
            await CheckFirebaseCred(firebaseUsername, firebasePassword);
            FireBaseLogIn();
            return;
        }
        public static async Task<bool> ChangePassword(string pass)
        {
            try
            {
                await auth.ChangeUserPassword(token.FirebaseToken, pass);
                Program.operations_change_pass++;
                Console.WriteLine("Password changed into: " + pass + " for the account: " + token.User.Email);
                return true;
            }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }
        public static async Task CreateUser(string firebaseUsername, string firebasePassword)
        {
            token = await auth.CreateUserWithEmailAndPasswordAsync(firebaseUsername, firebasePassword, "User", false);
            Program.operations_create_user++;
            return;
        }

        public static async Task UploadFile_onStorage(string file_local, string file_storage, bool changecreat_time)
        {
            if (token == null) { Console.WriteLine("LogIn richiesto.."); return; }
            //string file_local1 = file_local + "temp";
            //File.Copy(file_local, file_local1);
            //Console.WriteLine("Il problm è qui");
            try
            {
                using (var stream = System.IO.File.Open(file_local, FileMode.Open))
                {
                    FirebaseStorageOptions op = new FirebaseStorageOptions() { AuthTokenAsyncFactory = () => Task.FromResult(token.FirebaseToken) };
                    var task = new FirebaseStorage("mobilemoneyguard.appspot.com", op).Child(file_storage).PutAsync(stream);
                    await task;
                    Program.total_upload += new System.IO.FileInfo(file_local).Length;
                    Program.operations_upload++;
                };
                if (changecreat_time)
                {
                    Task<FirebaseMetaData> task_metadata = GetMetadata_fromStorage(file_storage);
                    await task_metadata;
                    File.SetCreationTime(file_local, task_metadata.Result.TimeCreated);
                }
                Console.WriteLine("The file " + file_local + " has been uploaded \n    ->  on the location " + file_storage + "\n" +
                    "    ->  creation time: " + File.GetCreationTimeUtc(file_local)
                    );
            }
            catch (Exception e) { Console.WriteLine("Error in uploading: "+e.Message);// try { File.Delete(file_local1); } catch (Exception) { };
                return; }
            //try { File.Delete(file_local1); } catch (Exception) { };
            return;
        }

       /*
        public static Thread Download_Thread(string file_local, string file_storage)
        {
            Thread thread = new Thread(Method);
            void Method()
            {
                if (token == null) { Console.WriteLine("LogIn richiesto.."); return; }
                FirebaseStorageOptions op = new FirebaseStorageOptions() { AuthTokenAsyncFactory = () => Task.FromResult(token.FirebaseToken) };
                string download_url = "";
                FirebaseMetaData metadata = GetMetadata_fromStorage(file_storage).Result;
                var task = new FirebaseStorage("mobilemoneyguard.appspot.com", op).Child(file_storage);
                var task1 = task.GetDownloadUrlAsync().ContinueWith((Task<string> uriTask) => { try { download_url = uriTask.Result.ToString(); } catch (Exception) { Console.WriteLine("File not found: " + file_storage); return; } });
                task1.Wait();

                using (var client = new WebClient())
                {
                    //if (File.Exists(file_local)) try { File.Move(file_local, file_local + "b"); } catch (Exception) { }
                    try
                    {
                        //client.DownloadProgressChanged += (o, e) => { if ((int)(e.ProgressPercentage / 50) * 50 == (int)e.ProgressPercentage) Console.WriteLine("Download progress: {0}%", e.ProgressPercentage); };
                        //client.DownloadFileCompleted += (o, e) => { Console.WriteLine("WTF???"); try { File.Delete(file_local + "b"); } catch (Exception ex) { Console.WriteLine(ex.Message); };  File.SetCreationTime(file_local, metadata.TimeCreated.ToUniversalTime()); File.SetLastWriteTime(file_local, metadata.TimeCreated.ToUniversalTime()); Console.WriteLine("The file " + file_storage + " has been downloaded on the location " + file_local);};

                        client.DownloadFile(new Uri(download_url), file_local);
                        client.DownloadFileCompleted += (o, e) =>
                        {
                            File.SetCreationTime(file_local, metadata.TimeCreated.ToUniversalTime());
                            File.SetLastWriteTime(file_local, metadata.TimeCreated.ToUniversalTime());
                            Console.WriteLine("The file " + file_storage + " has been downloaded\n    ->  on the location " + file_local + "\n    ->  creation time: " + File.GetCreationTimeUtc(file_local));

                        };


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in downloading the file " + file_storage);// try { File.Move(file_local + "b", file_local); } catch (Exception) { Console.WriteLine(ex.Message); };
                        return null;
                    }
                }
                return metadata;
            }
            return thread;
        }
        */

        public static async Task<Uri> GetUriFile_fromStorage(string file_storage)
        {
            #region CkeckConnection
            Task<bool> check_connection = CheckConnection();
            await check_connection;
            if (!check_connection.Result) return null;
            #endregion
            if (token == null) { Console.WriteLine("LogIn richiesto.."); return null; }
            FirebaseStorageOptions op = new FirebaseStorageOptions() { AuthTokenAsyncFactory = () => Task.FromResult(token.FirebaseToken) };
            var task = new FirebaseStorage("mobilemoneyguard.appspot.com", op).Child(file_storage);
            try
            {
                var task1 = task.GetDownloadUrlAsync();
                await task1;
                Program.operations_downloadurls++;
                return new Uri(task1.Result);
            }
            catch (Exception e) { Console.WriteLine(e.Message); return null; }
        }

        public static async Task<FirebaseMetaData> DownloadFile_fromStorage(string file_local, string file_storage)
        {
            if ((int)((DateTime.Now - FirebaseClass.token.Created).TotalSeconds) > 3500) { await FirebaseClass.CheckFirebaseCred(Program.email_user, Program.pass_user); FirebaseClass.FireBaseLogIn(); }
            if (token == null) { Console.WriteLine("LogIn richiesto.."); return null; }
            FirebaseStorageOptions op = new FirebaseStorageOptions() { AuthTokenAsyncFactory = () => Task.FromResult(token.FirebaseToken) };
            string download_url = "";
            FirebaseMetaData metadata = await GetMetadata_fromStorage(file_storage);
            var task = new FirebaseStorage("mobilemoneyguard.appspot.com", op).Child(file_storage);
            var task1 = task.GetDownloadUrlAsync().ContinueWith((Task<string> uriTask) => { try { download_url = uriTask.Result.ToString(); } catch (Exception) { Console.WriteLine("File not found: " + file_storage); return; } });
            await task1;
            Program.operations_downloadurls++;

            using (var client = new WebClient())
            {
                //if (File.Exists(file_local)) try { File.Move(file_local, file_local + "b"); } catch (Exception) { }
                try
                {
                    //client.DownloadProgressChanged += (o, e) => { if ((int)(e.ProgressPercentage / 50) * 50 == (int)e.ProgressPercentage) Console.WriteLine("Download progress: {0}%", e.ProgressPercentage); };
                    //client.DownloadFileCompleted += (o, e) => { Console.WriteLine("WTF???"); try { File.Delete(file_local + "b"); } catch (Exception ex) { Console.WriteLine(ex.Message); };  File.SetCreationTime(file_local, metadata.TimeCreated.ToUniversalTime()); File.SetLastWriteTime(file_local, metadata.TimeCreated.ToUniversalTime()); Console.WriteLine("The file " + file_storage + " has been downloaded on the location " + file_local);};

                    client.DownloadFile(new Uri(download_url), file_local);
                    Program.operations_download++;
                    Program.total_download += new System.IO.FileInfo(file_local).Length;
                    File.SetCreationTime(file_local, metadata.TimeCreated.ToUniversalTime());
                    File.SetLastWriteTime(file_local, metadata.TimeCreated.ToUniversalTime());
                    Console.WriteLine("The file " + file_storage + " has been downloaded\n    ->  on the location " + file_local + "\n    ->  creation time: " + File.GetCreationTimeUtc(file_local));
                    
                }
                catch (Exception ex) {
                    Console.WriteLine("Error in downloading the file " + file_storage);// try { File.Move(file_local + "b", file_local); } catch (Exception) { Console.WriteLine(ex.Message); };
                    return null; }
            }
            return metadata;
        }


        public static async Task<FirebaseMetaData> GetMetadata_fromStorage(string file_storage)
        {
            if ((int)((DateTime.Now - FirebaseClass.token.Created).TotalSeconds) > 3500) { await FirebaseClass.CheckFirebaseCred(Program.email_user, Program.pass_user); FirebaseClass.FireBaseLogIn(); }
            if (token == null) { Console.WriteLine("LogIn richiesto.."); return null; }
            FirebaseStorageOptions op = new FirebaseStorageOptions() { AuthTokenAsyncFactory = () => Task.FromResult(token.FirebaseToken) };
            FirebaseMetaData metadata = null;
            var task = new FirebaseStorage("mobilemoneyguard.appspot.com", op).Child(file_storage);
            var task2 = task.GetMetaDataAsync().ContinueWith((Task<FirebaseMetaData> uriTask) => { try { metadata = uriTask.Result; Program.operations_downmetadata_success++; } catch (Exception) { Program.operations_downmetadata_notsuccess++; return; } });
            await task2;
            

            return metadata;
        }

        public static async Task<bool> UpdateImages_FromStorageToLocal(bool forced, int num_icons_tip, int num_icons_met)
        {
            Console.WriteLine("Trying to update images from storage");
            #region Get metadata of all images on storage
            if (Program.Id_user == "local") return false;
            if ((int)((DateTime.Now - FirebaseClass.token.Created).TotalSeconds) > 3500) { await FirebaseClass.CheckFirebaseCred(Program.email_user, Program.pass_user); FirebaseClass.FireBaseLogIn(); }
            if (token == null) { Console.WriteLine("LogIn richiesto.."); return false; }
            #region CkeckConnection
            Task<bool> check_connection = CheckConnection();
            await check_connection;
            if (!check_connection.Result) return false;
            #endregion
            int i = 0;
            Task<FirebaseMetaData>[] metadata_tipo_ = new Task<FirebaseMetaData>[25];
            Task<FirebaseMetaData>[] metadata_metodo_ = new Task<FirebaseMetaData>[25];
            List<FirebaseMetaData> metadata_tipo_storage = new List<FirebaseMetaData>();
            List<FirebaseMetaData> metadata_metodo_storage = new List<FirebaseMetaData>();
            do
            {
                metadata_tipo_[i] = FirebaseClass.GetMetadata_fromStorage("user/" + token.User.LocalId + @"/Icons/Tipologie/img" + i + ".png");
                i++;
            } while (i < num_icons_tip);
            i = 0;
            do
            {
                metadata_metodo_[i] = FirebaseClass.GetMetadata_fromStorage("user/" + token.User.LocalId + @"/Icons/Metodi/img" + i + ".png");
                i++;
            } while (i < num_icons_met);
            foreach (Task task in metadata_tipo_) { await task; }
            foreach (Task task in metadata_metodo_) { await task; }
            for (i = 0; i < num_icons_tip; i++)
            {
                if (metadata_tipo_[i].Result != null) metadata_tipo_storage.Add(metadata_tipo_[i].Result);
            }
            for (i = 0; i < num_icons_met; i++)
            {
                if (metadata_metodo_[i].Result != null) metadata_metodo_storage.Add(metadata_metodo_[i].Result);
            }
            #endregion
            List<Task> tasks = new List<Task>();
            #region Update Tipologie images from storage to local
            foreach (FirebaseMetaData metadata in metadata_tipo_storage)
            {
                bool corrispondenza = false;
                foreach (string filename in Directory.GetFiles(Input.path_moneyguard + Program.Id_user + @"\Icons\Tipologie"))
                    if (Path.GetFileName(filename) == Path.GetFileName(metadata.Name)) { corrispondenza = true; break; }
                string path_local = Input.path_moneyguard + Program.Id_user + @"\Icons\Tipologie\" + Path.GetFileName(metadata.Name);
                string path_storage = "user/" + token.User.LocalId + @"/Icons/Tipologie/" + Path.GetFileName(metadata.Name);
                if (corrispondenza) //se il file c'è già in locale
                {
                    DateTime data_local = File.GetCreationTimeUtc(path_local);
                    DateTime data_storage = metadata.TimeCreated.ToUniversalTime();
                    if (data_storage > data_local || forced) { tasks.Add(DownloadFile_fromStorage(path_local, path_storage));
                    }
                }
                else  // se il file non c'è
                {
                    tasks.Add(DownloadFile_fromStorage(path_local, path_storage));

                }
            }
            #endregion
            #region Update Metodi images from storage to local
            foreach (FirebaseMetaData metadata in metadata_metodo_storage)
            {
                bool corrispondenza = false;
                foreach (string filename in Directory.GetFiles(Input.path_moneyguard + Program.Id_user + @"\Icons\Metodi"))
                    if (Path.GetFileName(filename) == Path.GetFileName(metadata.Name)) { corrispondenza = true; break; }
                string path_local = Input.path_moneyguard + Program.Id_user + @"\Icons\Metodi\" + Path.GetFileName(metadata.Name);
                string path_storage = "user/" + token.User.LocalId + @"/Icons/Metodi/" + Path.GetFileName(metadata.Name);
                if (corrispondenza) //se il file c'è già in locale
                {
                    DateTime data_local = File.GetCreationTimeUtc(path_local);
                    DateTime data_storage = metadata.TimeCreated.ToUniversalTime();
                    if (data_storage > data_local || forced) { tasks.Add(DownloadFile_fromStorage(path_local, path_storage)); Console.WriteLine(path_local + " Updating"); }
                }
                else  // se il file non c'è
                {
                    tasks.Add(DownloadFile_fromStorage(path_local, path_storage)); Console.WriteLine(path_local + " Creating");
                }
            }
            #endregion
            #region Deleting all files not needed
            /*
            foreach (string filename in Directory.GetFiles(Input.path_moneyguard + Program.Id_user + @"\Icons\Tipologie"))
            {
                bool exist = false;
                foreach (FirebaseMetaData metadata in metadata_tipo_storage)
                {
                    if (Path.GetFileName(metadata.Name) == Path.GetFileName(filename)) exist = true;
                }
                if (!exist) File.Delete(filename);
            }
            foreach (string filename in Directory.GetFiles(Input.path_moneyguard + Program.Id_user + @"\Icons\Metodi"))
            {
                bool exist = false;
                foreach (FirebaseMetaData metadata in metadata_metodo_storage)
                {
                    if (Path.GetFileName(metadata.Name) == Path.GetFileName(filename)) exist = true;
                }
                if (!exist) File.Delete(filename);
            }*/
            #endregion
            foreach (Task task in tasks) { await task; }
            return true;
        }
        public static async Task<bool> UpdateImages_FromLocalToStorage(bool forced, int num_icons_tip, int num_icons_met)
        {
            Console.WriteLine("Trying to update images on storage");
            #region Get metadata of all images on storage
            if (Program.Id_user == "local") return false;
            if ((int)((DateTime.Now - FirebaseClass.token.Created).TotalSeconds) > 3500) { await FirebaseClass.CheckFirebaseCred(Program.email_user, Program.pass_user); FirebaseClass.FireBaseLogIn(); }
            if (token == null) { Console.WriteLine("LogIn richiesto.."); return false; }
            #region CkeckConnection
            Task<bool> check_connection = CheckConnection();
            await check_connection;
            if (!check_connection.Result) return false;
            #endregion
            int i = 0;
            Task<FirebaseMetaData>[] metadata_tipo_ = new Task<FirebaseMetaData>[25];
            Task<FirebaseMetaData>[] metadata_metodo_ = new Task<FirebaseMetaData>[25];
            List<FirebaseMetaData> metadata_tipo_storage = new List<FirebaseMetaData>();
            List<FirebaseMetaData> metadata_metodo_storage = new List<FirebaseMetaData>();
            do
            {
                metadata_tipo_[i] = FirebaseClass.GetMetadata_fromStorage("user/" + token.User.LocalId + @"/Icons/Tipologie/img" + i + ".png");
                i++;
            } while (i < num_icons_tip);
            i = 0;
            do
            {
                metadata_metodo_[i] = FirebaseClass.GetMetadata_fromStorage("user/" + token.User.LocalId + @"/Icons/Metodi/img" + i + ".png");
                i++;
            } while (i < num_icons_met);
            foreach (Task task in metadata_tipo_) { await task; }
            foreach (Task task in metadata_metodo_) { await task; }
            for (i = 0; i < num_icons_tip; i++)
            {
                if (metadata_tipo_[i].Result != null) metadata_tipo_storage.Add(metadata_tipo_[i].Result);
            }
            for (i = 0; i < num_icons_met; i++)
            {
                if (metadata_metodo_[i].Result != null) metadata_metodo_storage.Add(metadata_metodo_[i].Result);
            }
            #endregion
            List<Task> tasks = new List<Task>();
            #region Update Tipologie images from local to storage
            foreach (string filename in Directory.GetFiles(Input.path_moneyguard + Program.Id_user + @"\Icons\Tipologie"))
            {
                bool corrispondenza = false;
                FirebaseMetaData Metadata = null;
                foreach (FirebaseMetaData metadata in metadata_tipo_storage)
                    if (Path.GetFileName(filename) == Path.GetFileName(metadata.Name)) { corrispondenza = true; Metadata = metadata; break; }
                string path_local = Input.path_moneyguard + Program.Id_user + @"\Icons\Tipologie\" + Path.GetFileName(filename); 
                string path_storage = "user/" + token.User.LocalId + @"/Icons/Tipologie/" + Path.GetFileName(filename);
                if (corrispondenza) //se il file c'è già in locale
                {
                    DateTime data_local = File.GetCreationTimeUtc(path_local);
                    DateTime data_storage = Metadata.TimeCreated.ToUniversalTime();
                    if (data_storage < data_local || forced) { tasks.Add(UploadFile_onStorage(path_local, path_storage, true)); }
                }
                else  // se il file non c'è
                {
                    tasks.Add(UploadFile_onStorage(path_local, path_storage, true));
                }
            }
            #endregion
            #region Update Metodi images from storage to local
            foreach (string filename in Directory.GetFiles(Input.path_moneyguard + Program.Id_user + @"\Icons\Metodi"))
            {
                bool corrispondenza = false;
                FirebaseMetaData Metadata = null;
                foreach (FirebaseMetaData metadata in metadata_metodo_storage)
                    if (Path.GetFileName(filename) == Path.GetFileName(metadata.Name)) { corrispondenza = true; Metadata = metadata; break; }
                string path_local = Input.path_moneyguard + Program.Id_user + @"\Icons\Metodi\" + Path.GetFileName(filename);
                string path_storage = "user/" + token.User.LocalId + @"/Icons/Metodi/" + Path.GetFileName(filename);
                if (corrispondenza) //se il file c'è già in locale
                {
                    DateTime data_local = File.GetCreationTimeUtc(path_local);
                    DateTime data_storage = Metadata.TimeCreated.ToUniversalTime();
                    if (data_storage < data_local || forced) { tasks.Add(UploadFile_onStorage(path_local, path_storage, true)); }
                }
                else  // se il file non c'è
                {
                    tasks.Add(UploadFile_onStorage(path_local, path_storage, true));
                }
            }
            #endregion
            #region Deleting all files not needed
            foreach (FirebaseMetaData metadata in metadata_tipo_storage)
            {
                bool exist = false;
                foreach (string filename in Directory.GetFiles(Input.path_moneyguard + Program.Id_user + @"\Icons\Tipologie"))
                {
                    if (Path.GetFileName(metadata.Name) == Path.GetFileName(filename)) exist = true;
                }
                if (!exist) tasks.Add(DeleteFile_onStorage("user/" + token.User.LocalId + @"/Icons/Tipologie/" + Path.GetFileName(metadata.Name)));
            }
            foreach (FirebaseMetaData metadata in metadata_metodo_storage)
            {
                bool exist = false;
                foreach (string filename in Directory.GetFiles(Input.path_moneyguard + Program.Id_user + @"\Icons\Metodi"))
                {
                    if (Path.GetFileName(metadata.Name) == Path.GetFileName(filename)) exist = true;
                }
                if (!exist) tasks.Add(DeleteFile_onStorage("user/" + token.User.LocalId + @"/Icons/Metodi/" + Path.GetFileName(metadata.Name)));
            }
            #endregion
            foreach (Task task in tasks) { await task; }
            return true;
        }
        public static async Task<bool> UpdateDataV1_FromStorageToLocal(bool forced)
        {
            Console.WriteLine("Trying to update data from storage");
            #region Get metadata of DataV1.txt on storage
            if (Program.Id_user == "local") return false;
            if (token == null) { Console.WriteLine("LogIn richiesto.."); return false; }
            
            Task<FirebaseMetaData> metadata_data_ = FirebaseClass.GetMetadata_fromStorage("user/" + token.User.LocalId + @"/DataV1.txt");
            await metadata_data_;
            FirebaseMetaData metadata_data = metadata_data_.Result;
            #endregion
            #region Update DataV1 from storage to local

            string path_local = Input.path_moneyguard + Program.Id_user + @"\DataV1.txt";
            string path_storage = "user/" + token.User.LocalId + @"/DataV1.txt";
            if (metadata_data == null) { Console.WriteLine(path_storage + " doesn't exist"); return false; }
            if (File.Exists(path_local)) //se il file c'è già in locale
            {
                DateTime data_local = File.GetCreationTimeUtc(path_local);
                DateTime data_storage = metadata_data.TimeCreated.ToUniversalTime();
                if (data_storage > data_local || forced) { await DownloadFile_fromStorage(path_local, path_storage); return true; }
                else return false;
            }
            else  // se il file non c'è
            {
                await DownloadFile_fromStorage(path_local, path_storage);
                return true;
            }
            #endregion
        }
        public static async Task<bool> UpdateDataV1_FromLocalToStorage(bool forced)
        {
            Console.WriteLine("Trying to update data on storage");
            #region Get metadata of DataV1.txt on storage
            if (token == null) { Console.WriteLine("LogIn richiesto.."); return false; }

            if (Program.Id_user == "local") return false;
            Task<FirebaseMetaData> metadata_data_ = FirebaseClass.GetMetadata_fromStorage("user/" + token.User.LocalId + @"/DataV1.txt");
            await metadata_data_;
            FirebaseMetaData metadata_data = metadata_data_.Result;
            #endregion
            #region Update DataV1 from local to storage

            string path_local = Input.path_moneyguard + Program.Id_user + @"\DataV1.txt";
            string path_storage = "user/" + token.User.LocalId + @"/DataV1.txt";
            if (!File.Exists(path_local)) { Console.WriteLine(path_local + " doesn't exist"); return false; }
            if (metadata_data != null) //se il file c'è già in storage
            {
                DateTime data_local = File.GetCreationTimeUtc(path_local);
                DateTime data_storage = metadata_data.TimeCreated.ToUniversalTime();
                Console.WriteLine("Local: " + data_local + "  Storage: " + data_storage);
                if (data_storage < data_local || forced) { await UploadFile_onStorage(path_local, path_storage, true); return true; }
                else return false;
            }
            else  // se il file non c'è
            {
                await UploadFile_onStorage(path_local, path_storage, true);
                return true;
            }
            #endregion
        }
        public static async Task<bool> UpdateData_FromLocalToStorage(bool forced, bool download_info)
        {
            Console.WriteLine("Trying to update data on storage");
            try
            {
                #region Get metadatas of framments on storage
                if (Program.Id_user == "local") return false;
                if ((int)((DateTime.Now - FirebaseClass.token.Created).TotalSeconds) > 3500) { await FirebaseClass.CheckFirebaseCred(Program.email_user, Program.pass_user); FirebaseClass.FireBaseLogIn(); }
                if (token == null) { Console.WriteLine("LogIn richiesto.."); return false; }
                #region CkeckConnection
                Task<bool> check_connection = CheckConnection();
                await check_connection;
                if (!check_connection.Result) return false;
                #endregion
                Task<FirebaseMetaData> info = GetInfo(download_info);
                await info;
                List<Task> tasks = new List<Task>();
                if (info.Result == null)
                {
                    Console.WriteLine("Info.txt not found on storage..");
                    foreach (string path in Directory.GetFiles(Input.path + @"Data"))
                    {
                        tasks.Add(UploadFile_onStorage(path, "user/" + token.User.LocalId + @"/Data/" + Path.GetFileName(path), true));
                    }
                    foreach (Task task in tasks) await task;
                    MyMetadata.CreateInfo();
                    await UploadFile_onStorage(Input.path_moneyguard + Program.Id_user + @"/Data/Info.txt", "user/" + token.User.LocalId + @"/Data/Info.txt", false);
                    return true;
                }
                else
                {
                    tasks.Clear();
                    MyMetadata.CreateInfo();
                    foreach (MyMetadata metadata in MyMetadata.MetadataToRemove(Input.path_moneyguard + Program.Id_user + @"/Data/Info.txt", Input.path_moneyguard + Program.Id_user + @"/Info_temp.txt", false))
                    {
                        if (forced) tasks.Add(DeleteFile_onStorage("user/" + token.User.LocalId + @"/Data/Data" + metadata.filename + ".txt"));
                    }
                    foreach (MyMetadata metadata in MyMetadata.MetadataToUpdate(Input.path_moneyguard + Program.Id_user + @"/Data/Info.txt", Input.path_moneyguard + Program.Id_user + @"/Info_temp.txt", false))
                    {
                        tasks.Add(UploadFile_onStorage(Input.path_moneyguard + Program.Id_user + @"/Data/Data" + metadata.filename + ".txt", "user/" + token.User.LocalId + @"/Data/Data" + metadata.filename + ".txt", true));
                    }
                    foreach (Task task in tasks) await task;
                    MyMetadata.CreateInfo();
                    if (tasks.Count > 0) await UploadFile_onStorage(Input.path_moneyguard + Program.Id_user + @"/Data/Info.txt", "user/" + token.User.LocalId + @"/Data/Info.txt", true);
                    return true;
                }
                #endregion
            }
            catch (Exception) { Console.WriteLine("Can't update data"); return false; }
        }
        public static async Task<bool> UpdateData_FromStorageToLocal(bool forced, bool download_info)
        {
            Console.WriteLine("Trying to update data on local");
            try
            {
                #region Get metadatas of framments on storage
                if (Program.Id_user == "local") return false;
                if ((int)((DateTime.Now - FirebaseClass.token.Created).TotalSeconds) > 3500) { await FirebaseClass.CheckFirebaseCred(Program.email_user, Program.pass_user); FirebaseClass.FireBaseLogIn(); }
                if (token == null) { Console.WriteLine("LogIn richiesto.."); return false; }
                #region CkeckConnection
                Task<bool> check_connection = CheckConnection();
                await check_connection;
                if (!check_connection.Result) return false;
                #endregion
                Task<FirebaseMetaData> info = GetInfo(download_info);
                await info;
                List<Task> tasks = new List<Task>();
                if (info.Result == null)
                {
                    Console.WriteLine("Info.txt not found on storage..");
                    return true;
                }
                else
                {
                    tasks.Clear();
                    List<Thread> list_thread = new List<Thread>();
                    MyMetadata.CreateInfo();
                    foreach (MyMetadata metadata in MyMetadata.MetadataToRemove(Input.path_moneyguard + Program.Id_user + @"/Data/Info.txt", Input.path_moneyguard + Program.Id_user + @"/Info_temp.txt", true))
                    {
                        if (forced) File.Delete(Input.path_moneyguard + Program.Id_user + @"/Data/Data" + metadata.filename + ".txt");
                    }
                    foreach (MyMetadata metadata in MyMetadata.MetadataToUpdate(Input.path_moneyguard + Program.Id_user + @"/Data/Info.txt", Input.path_moneyguard + Program.Id_user + @"/Info_temp.txt", true))
                    { 
                        tasks.Add(
                            //Download_Thread(Input.path_moneyguard + Program.Id_user + @"/Data/Data" + metadata.filename + ".txt", "user/" + token.User.LocalId + @"/Data/Data" + metadata.filename + ".txt");
                            DownloadFile_fromStorage(Input.path_moneyguard + Program.Id_user + @"/Data/Data" + metadata.filename + ".txt", "user/" + token.User.LocalId + @"/Data/Data" + metadata.filename + ".txt")
                            );
                    }
                    foreach (Task task in tasks) await task;
                    MyMetadata.CreateInfo();
                    if (tasks.Count > 0) await UploadFile_onStorage(Input.path_moneyguard + Program.Id_user + @"/Data/Info.txt", "user/" + token.User.LocalId + @"/Data/Info.txt", true);
                    return true;
                }
                #endregion
            }
            catch (Exception) { Console.WriteLine("Can't update data"); return false; }
        }

        public static async Task<FirebaseMetaData> GetInfo(bool download_info)
        {
            if (download_info)
            {
                File.Delete(Input.path_moneyguard + Program.Id_user + @"/Info_temp.txt");
                Task<FirebaseMetaData> info = DownloadFile_fromStorage(Input.path_moneyguard + Program.Id_user + @"/Info_temp.txt", "user/" + token.User.LocalId + @"/Data/Info.txt");
                await info;
                if (info.Result != null) File.SetCreationTimeUtc(Input.path_moneyguard + Program.Id_user + @"/Info_temp.txt", info.Result.TimeCreated);
                return info.Result;
            }
            else
            {
                FirebaseMetaData metadata = new FirebaseMetaData();
                metadata.Name = "Fake";
                return metadata;
            }
        }

        public static async Task<bool> DeleteFile_onStorage(string file_storage)
        {
            if (token == null) { Console.WriteLine("LogIn richiesto.."); return false; }
            #region CkeckConnection
            Task<bool> check_connection = CheckConnection();
            await check_connection;
            if (!check_connection.Result) return false;
            #endregion
            FirebaseStorageOptions op = new FirebaseStorageOptions() { AuthTokenAsyncFactory = () => Task.FromResult(token.FirebaseToken) };
            Task task = new FirebaseStorage("mobilemoneyguard.appspot.com", op).Child(file_storage).DeleteAsync();
            await task;
            Console.WriteLine("The file " + file_storage + " has been removed");
            return true;
        }

        static MailMessage mailDetails;
        static SmtpClient clientDetails;
        public static bool SendMail(string emailto, string subject, string body)
        {
            string email = "noreply.moneyguard@gmail.com";
            string pass = "Moneyguard180393acer";
            try
            {
                //Smpt Client Details
                //gmail >> smtp server : smtp.gmail.com, port : 587 , ssl required
                //yahoo >> smtp server : smtp.mail.yahoo.com, port : 587 , ssl required
                clientDetails = new SmtpClient();
                clientDetails.Port = 587;
                clientDetails.Host = "smtp.gmail.com";
                clientDetails.EnableSsl = true;
                clientDetails.Credentials = new System.Net.NetworkCredential(email, pass);
                
                mailDetails = new MailMessage();
                mailDetails.From = new MailAddress(email);
                mailDetails.To.Add(emailto);
                mailDetails.Subject = subject;
                mailDetails.IsBodyHtml = true;
                mailDetails.Body = body;

                
                //if (fileName.Length > 0)
                {
                    //   Attachment attachment = new Attachment(fileName);
                    //  mailDetails.Attachments.Add(attachment);
                }

                clientDetails.Send(mailDetails);
                return true;
                //fileName = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

        }
        
    }
}
