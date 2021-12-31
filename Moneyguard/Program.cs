using Moneyguard.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;
using Microsoft.Win32;
using System.Diagnostics;
using System.Text;
using Firebase.Storage;
using Firebase.Auth;
using Firebase.Database;
using FireSharp.Response;
using System.Reflection;

namespace Moneyguard
{
    static class Program
    {
        static public string Program_Version;
        static public string Id_user = "local";
        static public string email_user = "";
        static public string pass_user = "";
        static public string fileversion = "V1";
        static public string fileversion2 = "V2";
        static public string filename = "Data" + fileversion;
        static public string filename_encr = "Data" + fileversion2;
        static public int num_max_icons_tip = 25;
        static public int num_max_icons_met = 25;
        static public int value = 0;
        static public bool ready_toquit = false;
        static readonly System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        static public EventWaitHandle point1 = new AutoResetEvent(false), quit = new AutoResetEvent(false), quit1 = new AutoResetEvent(false);
        static public WidgetMoneyguard widget;
        static Thread thread;
        static public FinestraPrincipale FinestraPrincipale;
        static public bool allow_to_connect = false;
        static public bool closing_def = false;
        static public bool login = true;
        static public bool aut_login = true;
        static bool aut_result = false;
        static public bool ready_tosave_oncloud = false;
        static public Thread Caric_Thread;
        static public Caricamento caricamento;
        static public bool caricamento_show = true;
        static public bool its_the_end = false;
        static public int min_to_aut_sync = 30;
        static public bool fast_reboot = false;
        static public int iterations = 0;
        static public bool install_new_version = false;
        static public bool saved_correctly = true;
        static public bool saving_tentative = false;
        static public int timeout_data = 7000;
        static public int timeout_images = 20000;
        static public bool sync_by_user;

        static public int operations_create_provider = 0;
        static public int operations_create_token = 0;
        static public int operations_create_client = 0;
        static public int operations_change_pass = 0;
        static public int operations_create_user = 0;
        static public int operations_downloadurls = 0;
        static public int operations_download = 0;
        static public int operations_upload = 0;
        static public int operations_downmetadata_success = 0;
        static public int operations_downmetadata_notsuccess = 0;
        static public double total_upload = 0;
        static public double total_download = 0;


        #region copyright
        static public string copyright = "Copyright © 2019\n" +
            "All images inside the software are conceded under the license:\n" +
            "https://creativecommons.org/licenses/by-sa/3.0/legalcode \n" +
            "\n" +
            "Credits:\n" +
            "Developer: Claudio Ciano\n" +
            "Images:\n";
        #endregion



        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        
        static void Main()
        {
            //Color color = Color.DarkTurquoise;
            //Console.WriteLine(color.R + "_" + color.G + "_" + color.B);

            string aus = "."; if (FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileMinorPart < 10) aus += "0";
            Program_Version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileMajorPart.ToString()
                + aus + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileMinorPart.ToString();

            int j = 0;
            foreach (Process clsProcess in Process.GetProcesses()) if (clsProcess.ProcessName == "Moneyguard" || clsProcess.ProcessName == "WidgetMoneyguard") j++;
            if (j > 1) try { if (!System.IO.File.Exists(Input.path_moneyguard + "Recall.txt")) System.IO.File.CreateText(Input.path_moneyguard + "Recall.txt"); return; } catch (Exception) { Console.WriteLine("Errore Recall"); return; }
            //if (System.IO.File.Exists(Input.path + "Recall.txt")) try { System.IO.File.Delete(Input.path + "Recall.txt"); } catch (Exception) { }
            if (File.Exists(Input.path_moneyguard + @"\MoneyGuardV" + Program_Version.Replace(".", "_") + ".exe"))
            {
                File.Delete(Input.path_moneyguard + @"\MoneyGuardV" + Program_Version.Replace(".", "_") + ".exe");
                if (File.Exists(Input.path_moneyguard + @"\MoneyGuard_Setup.msi")) File.Delete(Input.path_moneyguard + @"\MoneyGuard_Setup.msi");
            }
            if (!Directory.Exists(@"C:\ProgramData\Cyan")) Directory.CreateDirectory(@"C:\ProgramData\Cyan");
            if (!Directory.Exists(@"C:\ProgramData\Cyan\Moneyguard")) Directory.CreateDirectory(@"C:\ProgramData\Cyan\Moneyguard");
            if (!Directory.Exists(@"C:\ProgramData\Cyan\Moneyguard\local")) Directory.CreateDirectory(@"C:\ProgramData\Cyan\Moneyguard\local");

            Funzioni_utili.CreazioneCartella(@"C:\ProgramData\Cyan\Moneyguard\" + Program.Id_user);
            
            // try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Caric_Thread = new Thread(CaricamentO);
                Caric_Thread.Start();

                do
                {
                    saving_tentative = false;
                    caricamento_show = true;
                    if (!aut_login) Application.Run(new Login(aut_login));
                    else if (!Properties.Settings.Default.last_access_offline)
                    {
                        System.Threading.Thread Update = new System.Threading.Thread(FinishTask); Update.Start();
                        quit.WaitOne();
                        if (!aut_result) { Application.Run(new Login(aut_login)); }
                    }
                    caricamento_show = true;

                    login = false;
                    if (ready_toquit) { its_the_end = true; return; }

                    Input.RefreshPaths();


                    //StringCipher.DecryptFile(Input.path + @"\Giulietto.txt", Input.path + @"\Giulietto_decr.txt", "?	V??7u?");
                    //StringCipher.EncryptAndEncode(Input.path + @"\Giulietto_decr.txt", Input.path + @"\Giulietto_encr.txt");

                    PrimeImp();

                    try { new Input(); } catch (Exception e) { its_the_end = true; MessageBox.Show(@"Il file C:\ProgramData\Cyan\Moneyguard\DataV1.txt risulta corrotto. Ripristinalo oppure cancellalo\nEccezione: " + e); return; }
                    thread = new Thread(StartThread); thread.Start();



                    bool passedby = false;
                    for ( ; ; iterations++)
                    {
                        if (email_user == "shapelessguy@hotmail.it") FirebaseClass.SendMail("noreply.moneyguard@yahoo.com", "Ping", "Ciao");
                        FinestraPrincipale.active = true;
                        Application.Run(FinestraPrincipale = new FinestraPrincipale());
                        
                        FinestraPrincipale.Dispose();
                        FinestraPrincipale.active = false;
                        if (fast_reboot) { fast_reboot = false; continue; }
                        passedby = false;
                        if (ready_tosave_oncloud)
                        {
                            if (WidgetMoneyguard.ready_toclose) { passedby = true; caricamento_show = true; }
                            if (Id_user != "local")
                            {
                                Console.WriteLine("Saving in Cloud");
                                System.Threading.Thread SaveCloud = new System.Threading.Thread(Savings.FinishTask);
                                SaveCloud.Start();
                                Savings.quit.WaitOne();
                                // last_datatime = File.GetCreationTime(Input.path_in);
                                Console.WriteLine("End Saving in Cloud");
                                ready_tosave_oncloud = false;
                            }
                        }
                        if (WidgetMoneyguard.ready_toclose) { break; }
                        quit.Set();
                        point1.WaitOne();
                        if (ready_toquit)
                        {
                            break;
                        }
                        caricamento_show = true;
                        Thread Update1 = new System.Threading.Thread(RefreshData); Update1.Start();
                        quit1.WaitOne();
                        try { new Input(); } catch (Exception e) { its_the_end = true; MessageBox.Show(@"Il file C:\ProgramData\Cyan\Moneyguard\DataV1.txt risulta corrotto. Ripristinalo oppure cancellalo\nEccezione: " + e); return; }
                        WidgetMoneyguard.refreshpanel = true;
                    }
                    caricamento_show = true;
                    WidgetMoneyguard.notifyIcon1.Visible = false;
                    closing_def = true;

                    WidgetMoneyguard.ready_toclose = true;
                    for(; ;)
                    {
                        if (!thread.IsAlive) break;
                        System.Threading.Thread.Sleep(20);
                    }
                    if (!passedby || !saved_correctly)
                    {
                        if (!passedby) Savings.SaveEvents();
                        if (ready_tosave_oncloud || !saved_correctly)
                        {
                            if (Id_user != "local")
                            {
                                Console.WriteLine("Saving in Cloud");
                                System.Threading.Thread SaveCloud = new System.Threading.Thread(Savings.FinishTask);
                                SaveCloud.Start();
                                Savings.quit.WaitOne();

                                if (!saved_correctly) { caricamento_show = false; MessageBox.Show("Per sincronizzare il database è necessario avere una connessione ad internet. Prova a riaprire l'applicazione quando ne trovi una"); }
                                saved_correctly = false;
                                Console.WriteLine("End Saving in Cloud"); caricamento_show = true;
                                ready_tosave_oncloud = false;
                                saving_tentative = true;
                            }
                        }
                    }
                    GC.Collect();
                    
                }
                while (login);
                //Savings.SaveEvents();
                if ((ready_tosave_oncloud || !saved_correctly) && !saving_tentative)
                {
                    if (Id_user != "local")
                    {
                        Console.WriteLine("Saving in Cloud");
                        System.Threading.Thread SaveCloud = new System.Threading.Thread(Savings.FinishTask);
                        SaveCloud.Start();
                        Savings.quit.WaitOne();
                        if (!saved_correctly) { caricamento_show = false; MessageBox.Show("Per sincronizzare il database è necessario avere una connessione ad internet. Prova a riaprire l'applicazione quando ne trovi una"); }
                        saved_correctly = false;
                        Console.WriteLine("End Saving in Cloud");
                        ready_tosave_oncloud = false;
                    }
                }

                its_the_end = true;
                Console.WriteLine(GetStats());
                if (install_new_version) Process.Start(Input.path_moneyguard + @"\MoneyGuardV" + Finestra_Updates.latest_version.Replace(".", "_") + ".exe");
            }
           // catch (Exception e) { MessageBox.Show("Errore: " + e.Message); }
            
    
        }

        static void CaricamentO()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Color color1 = Color.LightCyan;
            Color color2 = Color.DarkTurquoise;
            //bool asp = false;
            System.Windows.Forms.Timer timers = new System.Windows.Forms.Timer()
            {
                Enabled = true,
                Interval = 10,
            };
            timers.Tick += (o, e) => {
                if (its_the_end) { timers.Dispose(); caricamento.Close(); return; }
                if (!caricamento_show && caricamento.WindowState == FormWindowState.Minimized) return;
                caricamento.circularProgressBar1.Value++;
                if (caricamento.circularProgressBar1.Value == 100)
                {
                    //asp = false;
                    caricamento.circularProgressBar1.Value = 0;
                    if (caricamento.circularProgressBar1.ProgressColor == color1) { caricamento.circularProgressBar1.ProgressColor = color2; caricamento.circularProgressBar1.OuterColor = color1; }
                    else { caricamento.circularProgressBar1.ProgressColor = color1; caricamento.circularProgressBar1.OuterColor = color2; }
                }
                //if (caricamento.circularProgressBar1.Value == 100) asp = true;
                if (caricamento_show) { caricamento.Visible = true; caricamento.BringToFront(); caricamento.Update(); caricamento.WindowState = FormWindowState.Normal; }
                else { caricamento.Visible = false; caricamento.WindowState = FormWindowState.Minimized; } };
            Application.Run(caricamento = new Caricamento());
            caricamento.circularProgressBar1.Value = 99;
            { caricamento.circularProgressBar1.ProgressColor = color2; caricamento.circularProgressBar1.OuterColor = color1; }
        }

        static void StartThread()
        {
            Application.Run(widget = new WidgetMoneyguard());
            widget.KeyPreview = true;
        }

        static void PrimeImp()
        {
            Impostazioni.aut_sync = Properties.Settings.Default.aut_sync;
            Impostazioni.size = Properties.Settings.Default.window_size;
            Impostazioni.location = Properties.Settings.Default.window_location;
            Impostazioni.close_inApplicationsBar = Properties.Settings.Default.close_in_application_bar;
            Impostazioni.show_calendar_when_opened = Properties.Settings.Default.show_calendar_when_opened;
            Impostazioni.widget_visible = Properties.Settings.Default.widget_visible;
            Impostazioni.ora_minuto = Properties.Settings.Default.ora_minuto;
            Impostazioni.mute = Properties.Settings.Default.mute;
            Impostazioni.show_maximized = Properties.Settings.Default.show_maximized;
            Impostazioni.widgetZoom = Properties.Settings.Default.widget_size;
            Impostazioni.controllidx = Properties.Settings.Default.widget_controlli_adestra;
            Impostazioni.widget_contrasto = Properties.Settings.Default.widget_contrasto;
            WidgetMoneyguard.location = Properties.Settings.Default.widget_location;
            WidgetMoneyguard.size = new Size(Impostazioni.widgetZoom * 5 + 200, (int)((Impostazioni.widgetZoom * 5 + 200) / 3.5));
        }
        private static async void FinishTask()
        {
            if (Properties.Settings.Default.entra_aut)
            {
                string error = await FirebaseClass.CheckFirebaseCred(Properties.Settings.Default.email, Properties.Settings.Default.pass);
                if (error != "") { FirebaseClass.token = null; aut_result = false; quit.Set(); return; }
                if (!FirebaseClass.FireBaseLogIn()) { aut_result = false; quit.Set(); return; }
                
                ready_toquit = false;
                Program.Id_user = FirebaseClass.token.User.LocalId;
                Program.email_user = Properties.Settings.Default.email;
                Program.pass_user = Properties.Settings.Default.pass;
                Input.RefreshPaths();
                //notifyIcon0.ShowBalloonTip(30000, "Moneyguard", "Connessione in corso..", ToolTipIcon.Info);
                Funzioni_utili.CreazioneCartella(@"C:\ProgramData\Cyan\Moneyguard\" + Program.Id_user);
                if ((int)((DateTime.Now - FirebaseClass.token.Created).TotalSeconds) > 3500) { await FirebaseClass.CheckFirebaseCred(Program.email_user, Program.pass_user); FirebaseClass.FireBaseLogIn(); }
                
                Task<bool> task1 = FirebaseClass.UpdateData_FromStorageToLocal(true, true); 
                Task<bool> task2 = FirebaseClass.UpdateImages_FromStorageToLocal(false, Program.num_max_icons_tip, Program.num_max_icons_met);
                try {
                    await TimeoutAfter(task1, timeout_data); 
                    await TimeoutAfter(task2, timeout_images);
                }
                catch (Exception) { aut_result = false; quit.Set(); return;}
                aut_result = true;
                Console.WriteLine("Automatic Login");
            }
            else aut_result = false;

            quit.Set();
        }
        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, int timeout)
        {
            using (var timeoutCancellationTokenSource = new CancellationTokenSource())
            {
                var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
                if (completedTask == task)
                {
                    timeoutCancellationTokenSource.Cancel();
                    return await task;  // Very important in order to propagate exceptions
                }
                else
                {
                    throw new TimeoutException("The operation has timed out.");
                }
            }
        }

        public static async void RefreshData()
        {
            Funzioni_utili.CreazioneCartella(@"C:\ProgramData\Cyan\Moneyguard\" + Program.Id_user);

            Task<bool> task1 = FirebaseClass.UpdateData_FromStorageToLocal(true, true);
            Task<bool> task2 = FirebaseClass.UpdateImages_FromStorageToLocal(false, Program.num_max_icons_tip, Program.num_max_icons_met);
            try { 
            await TimeoutAfter(task1, timeout_data);
            await TimeoutAfter(task2, timeout_images);

            }
            catch (Exception) { }

            quit1.Set();
        }
        

        public static async void SyncData()
        {
            if (Id_user != "local")
            {
                Funzioni_utili.CreazioneCartella(@"C:\ProgramData\Cyan\Moneyguard\" + Program.Id_user);
                if ((int)((DateTime.Now - FirebaseClass.token.Created).TotalSeconds) > 3500) { await FirebaseClass.CheckFirebaseCred(Program.email_user, Program.pass_user); FirebaseClass.FireBaseLogIn(); }

                saved_correctly = false;
                Task<bool> task1 = FirebaseClass.UpdateData_FromStorageToLocal(false, true);
                try
                {
                    await TimeoutAfter(task1, timeout_data);
                }
                catch (Exception) { Program.caricamento_show = false; if (sync_by_user) MessageBox.Show("Non è possibile connettersi al server, riprova più tardi."); Program.caricamento_show = true; quit1.Set(); return; }

                Task<bool> task2 = FirebaseClass.UpdateData_FromLocalToStorage(true, false);
                Task<bool> task3 = FirebaseClass.UpdateImages_FromLocalToStorage(false, Program.num_max_icons_tip, Program.num_max_icons_met);
                Task<bool> task4 = FirebaseClass.UpdateImages_FromStorageToLocal(false, Program.num_max_icons_tip, Program.num_max_icons_met);

                try
                {
                    await TimeoutAfter(task2, timeout_data);
                    await TimeoutAfter(task3, timeout_images);
                    await TimeoutAfter(task4, timeout_images);
                }
                catch (Exception) { if (sync_by_user) MessageBox.Show("Non è possibile connettersi al server, riprova più tardi."); quit1.Set(); return; }

                if (task2.Result) try { new Input(); } catch (Exception ex) { Program.its_the_end = true; MessageBox.Show(@"Il file C:\ProgramData\Cyan\Moneyguard\DataV1.txt risulta corrotto. Ripristinalo oppure cancellalo\nEccezione: " + ex.Message); return; }
                WidgetMoneyguard.refreshpanel = true;

                if (task2.Result && task3.Result) saved_correctly = true;
            }
            quit1.Set();
        }

        public static string GetStats()
        {
            string output = "\n\nSTATISTICS";
            output += "\nProvider created: ";
            output += operations_create_provider;
            output += "\nTokens created: ";
            output += operations_create_token;
            output += "\nClients created: ";
            output += operations_create_client;
            output += "\nPasswords changed: ";
            output += operations_change_pass;
            output += "\nUsers created: ";
            output += operations_create_user;
            output += "\nNumber of uploads: ";
            output += operations_upload;
            output += "\nNumber of downloads: ";
            output += operations_download;
            output += "\nNumber of URL downloads: ";
            output += operations_downloadurls;
            output += "\nNumber of successfull Metadata taken: ";
            output += operations_downmetadata_success;
            output += "\nNumber of not successfull Metadata taken: ";
            output += operations_downmetadata_notsuccess;
            output += "\nTotal upload: ";
            output += total_upload + " Bytes";
            output += "\nTotal download: ";
            output += total_download + " Bytes";
            output += "\n\n";
            return output;
        }


    }
}
