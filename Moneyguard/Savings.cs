using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;

namespace Moneyguard
{
    class Savings
    {
        public static string spazio = "|rn|";
        public static bool to_save_oncloud = false;
        public static bool icons_changed = false;
        public static bool save_time = false;
        public static bool saving_on_cloud = false;
        public static bool block_saving = false;
        public static int num_backups = 30;
        public static bool file_occupied = false;
        public static int number_events_now=-1;
        public static List<DateTime> months_to_save;
        public static bool save_all = false;

        static public EventWaitHandle point1 = new AutoResetEvent(false);
        static public EventWaitHandle quit = new AutoResetEvent(false);
        static public Thread thread;
        /////////////////////////////////////////////////////////////////////////  Salvataggio Eventi (System, System.IO)
        public static void SaveEvents()
        {
            Console.WriteLine("Start saving on local.. ");
            Properties.Settings.Default.aut_sync = Impostazioni.aut_sync;
            Properties.Settings.Default.window_size = Impostazioni.size;
            Properties.Settings.Default.window_location = Impostazioni.location;
            Properties.Settings.Default.close_in_application_bar = Impostazioni.close_inApplicationsBar;
            Properties.Settings.Default.show_calendar_when_opened = Impostazioni.show_calendar_when_opened;
            Properties.Settings.Default.widget_visible = Impostazioni.widget_visible;
            Properties.Settings.Default.ora_minuto = Impostazioni.ora_minuto;
            Properties.Settings.Default.mute = Impostazioni.mute;
            Properties.Settings.Default.show_maximized = Impostazioni.show_maximized;
            Properties.Settings.Default.widget_size = Impostazioni.widgetZoom;
            Properties.Settings.Default.widget_controlli_adestra = Impostazioni.controllidx;
            Properties.Settings.Default.widget_contrasto = Impostazioni.widget_contrasto;
            Properties.Settings.Default.widget_location = WidgetMoneyguard.location;
            Properties.Settings.Default.Save();

            if (block_saving) return;
            
            foreach (var evento in Input.eventi)
            {
                if (!evento.validation || save_all)
                {
                    evento.validation = true;
                    evento.Load();
                    DateTime data = new DateTime(evento.GetData()[5], evento.GetData()[4], 1);
                    if (!months_to_save.Contains(data)) { months_to_save.Add(data); Program.ready_tosave_oncloud = true; }
                }
            }
            save_all = false;
            Input.eventi = Eventi.Order_datacode_datacode_modifica(Input.eventi);
            Framments framments = new Framments(months_to_save);
            framments.SaveFramments_toSave();
            months_to_save.Clear();

            #region null
            /*
            int i = 0;
            try
            {
                file_occupied = true;

                #region Write on file
                using (StreamWriter sw = File.CreateText(Input.path_out))
                {
                    sw.Write("Pass: " + Impostazioni.pass + ";"); sw.WriteLine(""); sw.WriteLine("");
                    sw.Write("Timeout: " + Impostazioni.timeout_pass + ";"); sw.WriteLine(""); sw.WriteLine("");
                    sw.Write("Question: " + Impostazioni.question + ";"); sw.WriteLine(""); sw.WriteLine("");
                    sw.Write("Answer: " + Impostazioni.answer + ";"); sw.WriteLine(""); sw.WriteLine("");

                    foreach (string stringa in Input.Updates)
                    {
                        sw.WriteLine(stringa);
                    }
                    sw.WriteLine("");

                    sw.Write("Attributi: ");
                    foreach (string attributo in Input.attributi)
                    {
                        sw.Write(attributo + ";");
                    }
                    sw.WriteLine(""); sw.WriteLine("");

                    sw.Write("Attributi_icons: ");
                    foreach (string attributo in Input.attributi)
                    {
                        sw.Write(Associazione.AiconaAssociata(attributo) + ";");
                    }
                    sw.WriteLine(""); sw.WriteLine("");

                    sw.Write("Tipi: ");
                    foreach (string tipo in Input.tipi)
                    {
                        sw.Write(tipo + ";");
                    }
                    sw.WriteLine(""); sw.WriteLine("");

                    sw.Write("Tipi_icons: ");
                    foreach (string tipo in Input.tipi)
                    {
                        sw.Write(Associazione.IconaAssociata(tipo) + ";");
                    }
                    sw.WriteLine(""); sw.WriteLine("");

                    sw.Write("Metodi: ");
                    foreach (string metodo in Input.metodi)
                    {
                        sw.Write(metodo + ";");
                    }
                    sw.WriteLine(""); sw.WriteLine("");

                    sw.Write("Metodi_icons: ");
                    foreach (string metodo in Input.metodi)
                    {
                        sw.Write(Associazione.MiconaAssociata(metodo) + ";");
                    }
                    sw.WriteLine(""); sw.WriteLine("");
                    sw.Write("Metodi_importi_iniziali: ");
                    foreach (double valore in Input.totali_iniziali)
                    {
                        sw.Write(Convert.ToString(valore) + ";");
                    }
                    sw.WriteLine(""); sw.WriteLine("");
                    

                    sw.WriteLine("Eventi:");
                    foreach (var evento in Input.eventi)
                    {
                        evento.Load();
                        sw.Write(Associazione.CodificaAttributo(Input.eventi[i].Get_Attributo()) + " ");
                        if (Input.eventi[i].Get_Attributo() != "Note")
                        {
                            sw.Write(Convert.ToString(Input.eventi[i].GetValore()) + " ");
                            sw.Write(Associazione.CodificaMetodo(Input.eventi[i].GetMetodo()) + " ");
                            if (evento.Get_Attributo() == "Introito" || evento.Get_Attributo() == "Spesa") sw.Write(Associazione.CodificaTipo(Input.eventi[i].GetTipo()) + " ");
                            else if (evento.Get_Attributo() == "Trasferimento") sw.Write(Associazione.CodificaMetodo(Input.eventi[i].GetTipo()) + " ");
                        }
                        sw.Write(Input.eventi[i].GetDatacode() + " ");
                        sw.Write(Input.eventi[i].GetDatacode_modifica() + " ");

                        for (int m = 0; m < Input.eventi[i].GetAttributi().Count; m++)
                        {
                            sw.Write("|*^*|" + Input.eventi[i].GetAttributo(m) + "|*^*|");
                        }
                        sw.WriteLine("|^*^|");
                        
                        i++;
                    }
                    sw.WriteLine("Eventi: End");
                }

#endregion

                WidgetMoneyguard.refreshpanel = true;
            }
            catch (Exception) { try { File.Delete(Input.path_out); file_occupied = false; return; } catch (Exception) { file_occupied = false; return; } }

            //Console.WriteLine(" on local..");

            string[] readText = File.ReadAllLines(Input.path_out);

            bool uguali = true, inizio = true;
            for (int k = 0; k < Math.Min(readText.Length, Impostazioni.readText.Length); k++)
            {
                if (readText[k] != Impostazioni.readText[k] && inizio) uguali = false;
            }

            if (readText.Length == Impostazioni.readText.Length && uguali) { File.Delete(Input.path_out); }
            else
            {
                to_save_oncloud = true;
                File.Delete(Input.path_in);
                File.Move(Input.path_out, Input.path_in);
                File.SetCreationTime(Input.path_in, DateTime.UtcNow);
                Impostazioni.readText = new string[readText.Length];
                for (int k = 0; k < readText.Length; k++) { string stringa = readText[k]; Impostazioni.readText[k] = stringa; };

                Funzioni_utili.Encrypt(Input.path_in, Input.path + Input.filename_encr + ".txt");
            }
            file_occupied = false;
            */
            #endregion
            Console.Write(" - Creating backup.. ");
            thread = new Thread(Start);
            void Start() { CreateBackup(); };
            thread.Start();
            Console.WriteLine("done");

            string globals = Framments.GetGlobals();
            if (globals != Input.globals) { Input.globals = globals; Program.ready_tosave_oncloud = true;}

            number_events_now = Input.eventi.Count;
            if (Input.number_events_before != number_events_now) { Input.number_events_before = number_events_now; Program.ready_tosave_oncloud = true; }
            /*
            if (Program.Id_user != "local" && (FinestraPrincipale.form_closing || Program.closing_def || save_time) && (to_save_oncloud || icons_changed))
            {
                Program.ready_tosave_oncloud = true;
            }
            */
        }

        static public async void FinishTask()
        {
            if (Program.Id_user == "local") return;
            if ((int)((DateTime.Now - FirebaseClass.token.Created).TotalSeconds) > 3500) { await FirebaseClass.CheckFirebaseCred(Program.email_user, Program.pass_user); FirebaseClass.FireBaseLogIn(); }
            //await FirebaseClass.UpdateDataV1_FromLocalToStorage(false);
            Program.saved_correctly = false;

            Task<bool> task1 = FirebaseClass.UpdateData_FromLocalToStorage(true, true);
            Task<bool> task2 = FirebaseClass.UpdateImages_FromLocalToStorage(false, Program.num_max_icons_tip, Program.num_max_icons_met);
            try
            {
                await Program.TimeoutAfter(task1, Program.timeout_data);
                await Program.TimeoutAfter(task2, Program.timeout_images);
            }
            catch (Exception) { quit.Set(); return; }
            Program.saved_correctly = true;
            quit.Set();
            return;
        }

        public static void CreateBackup()
        {
            try
            {

                string anno = DateTime.Now.Year.ToString();
                string mese = DateTime.Now.Month.ToString(); if (mese.Length == 1) mese = "0" + mese;
                string giorno = DateTime.Now.Day.ToString(); if (giorno.Length == 1) giorno = "0" + giorno;
                string Name = "Data" + anno + mese + giorno + ".txt";
                string[] allfiles = Directory.GetFiles(Input.path + @"\Backups\");
                for (int i = 0; i < allfiles.Length; i++) allfiles[i] = Path.GetFileName(allfiles[i]);
                List<string> files_in_backup = new List<string>();

                for (int i = 0; i < allfiles.Length; i++)
                {
                    if (Path.GetFileName(allfiles[i]) == Name) { try { File.Delete(Input.path + @"\Backups\" + allfiles[i]); } catch (Exception) { Console.WriteLine("Can't delete backups"); } }
                    else files_in_backup.Add(allfiles[i]);
                }

                int min = 99999999; int n; int indice = -1;
                if (files_in_backup.Count >= num_backups)
                {
                    for (int i = 0; i < files_in_backup.Count; i++)
                    {
                        if (int.TryParse(files_in_backup[i].Substring(4, files_in_backup[i].Length - 8), out n)) if (n < min) { min = n; indice = i; }

                    }
                    if (indice >= 0) try { Console.WriteLine("Deleting " + files_in_backup[indice]); File.Delete(Input.path + @"\Backups\" + files_in_backup[indice]); } catch (Exception) { Console.WriteLine("Can't delete last backup"); }
                }
                try
                {
                    int i = 0;
                    if (File.Exists(Input.path + @"\Backups\" + Name)) File.Delete(Input.path + @"\Backups\" + Name); 
                    using (StreamWriter sw = File.CreateText(Input.path + @"\Backups\" + Name))
                    {
                        sw.Write("Pass: " + Impostazioni.pass + ";"); sw.WriteLine(""); sw.WriteLine("");
                        sw.Write("Timeout: " + Impostazioni.timeout_pass + ";"); sw.WriteLine(""); sw.WriteLine("");
                        sw.Write("Question: " + Impostazioni.question + ";"); sw.WriteLine(""); sw.WriteLine("");
                        sw.Write("Answer: " + Impostazioni.answer + ";"); sw.WriteLine(""); sw.WriteLine("");

                        foreach (string stringa in Input.Updates)
                        {
                            sw.WriteLine(stringa);
                        }
                        sw.WriteLine("");

                        sw.Write("Attributi: ");
                        foreach (string attributo in Input.attributi)
                        {
                            sw.Write(attributo + ";");
                        }
                        sw.WriteLine(""); sw.WriteLine("");

                        sw.Write("Attributi_icons: ");
                        foreach (string attributo in Input.attributi)
                        {
                            sw.Write(Associazione.AiconaAssociata(attributo) + ";");
                        }
                        sw.WriteLine(""); sw.WriteLine("");

                        sw.Write("Tipi: ");
                        foreach (string tipo in Input.tipi)
                        {
                            sw.Write(tipo + ";");
                        }
                        sw.WriteLine(""); sw.WriteLine("");

                        sw.Write("Tipi_icons: ");
                        foreach (string tipo in Input.tipi)
                        {
                            sw.Write(Associazione.IconaAssociata(tipo) + ";");
                        }
                        sw.WriteLine(""); sw.WriteLine("");

                        sw.Write("Metodi: ");
                        foreach (string metodo in Input.metodi)
                        {
                            sw.Write(metodo + ";");
                        }
                        sw.WriteLine(""); sw.WriteLine("");

                        sw.Write("Metodi_icons: ");
                        foreach (string metodo in Input.metodi)
                        {
                            sw.Write(Associazione.MiconaAssociata(metodo) + ";");
                        }
                        sw.WriteLine(""); sw.WriteLine("");
                        sw.Write("Metodi_importi_iniziali: ");
                        foreach (double valore in Input.totali_iniziali)
                        {
                            sw.Write(Convert.ToString(valore) + ";");
                        }
                        sw.WriteLine(""); sw.WriteLine("");


                        sw.WriteLine("Eventi_Aut:");
                        foreach(Eventi_Aut evento in Input.eventi_aut)
                        {
                            sw.Write(evento.StringToSave());
                        }
                        sw.WriteLine("Eventi_Aut: End");

                        sw.WriteLine("Eventi:");
                        foreach (var evento in Input.eventi)
                        {
                            evento.Load();
                            sw.Write(Associazione.CodificaAttributo(Input.eventi[i].Get_Attributo()) + " ");
                            if (Input.eventi[i].Get_Attributo() != "Note")
                            {
                                sw.Write(Convert.ToString(Input.eventi[i].GetValore()) + " ");
                                sw.Write(Associazione.CodificaMetodo(Input.eventi[i].GetMetodo()) + " ");
                                if (evento.Get_Attributo() == "Introito" || evento.Get_Attributo() == "Spesa") sw.Write(Associazione.CodificaTipo(Input.eventi[i].GetTipo()) + " ");
                                else if (evento.Get_Attributo() == "Trasferimento") sw.Write(Associazione.CodificaMetodo(Input.eventi[i].GetTipo()) + " ");
                            }
                            sw.Write(Input.eventi[i].GetDatacode() + " ");
                            sw.Write(Input.eventi[i].GetDatacode_modifica() + " ");

                            for (int m = 0; m < Input.eventi[i].GetAttributi().Count; m++)
                            {
                                sw.Write("|*^*|" + Input.eventi[i].GetAttributo(m) + "|*^*|");
                            }
                            sw.WriteLine("|^*^|");

                            i++;
                        }
                        sw.WriteLine("Eventi: End");
                    }
                    Funzioni_utili.Encrypt(Input.path + @"\Backups\" + Name, Input.path + @"\Backups\temp" + Name);
                    File.SetCreationTimeUtc(Input.path + @"\Backups\" + Name, DateTime.Now);
                }
                catch (Exception ex) { Console.WriteLine("Can't create backup " + ex.Message); }
            }
            catch (Exception exc) { Console.WriteLine("Can't create backup " + exc.Message); }
        }

        public static void SaveGiorno()
        {
            int[] data_aus = new int[6];
            data_aus[0] = 0;
            data_aus[1] = 0;
            data_aus[2] = 0;
            data_aus[3] = FinestraPrincipale.BackPanel.StandardCalendar.giorno;
            data_aus[4] = FinestraPrincipale.BackPanel.StandardCalendar.mese;
            data_aus[5] = FinestraPrincipale.BackPanel.StandardCalendar.anno;
            if(FinestraPrincipale.BackPanel.Panel_Giorno != null)
                if (FinestraPrincipale.BackPanel.Panel_Giorno.Visible)
                {
                    foreach (Pulsante pulsante in FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale)
                    {
                        pulsante.SaveEvento();
                    }
                    if (FinestraPrincipale.BackPanel.Panel_Giorno.Note_something != -1)
                    {
                        int esito = 0;
                        foreach (Eventi evento in Input.eventi)
                        {
                            if (evento.Get_Attributo() == "Note" && evento.GetData()[3] == data_aus[3] && evento.GetData()[4] == data_aus[4] && evento.GetData()[5] == data_aus[5])
                            {
                                if (FinestraPrincipale.BackPanel.Panel_Giorno.Note.Text == "")
                                {
                                    Input.eventi.Remove(evento);
                                    evento.Load();
                                    DateTime data = new DateTime(evento.GetData()[5], evento.GetData()[4], 1);
                                    if (!months_to_save.Contains(data)) months_to_save.Add(data);
                                }
                                else
                                {
                                    string testo = FinestraPrincipale.BackPanel.Panel_Giorno.Note.Text;
                                    evento.SetAttributo(0, FinestraPrincipale.BackPanel.Panel_Giorno.Note.Text.Replace("\r\n", Savings.spazio));
                                }
                                esito = 1;
                                break;
                            }
                        }
                        if (esito == 0)
                        {
                            List<string> newattributi = new List<string>
                            { FinestraPrincipale.BackPanel.Panel_Giorno.Note.Text.Replace("\r\n", Savings.spazio) };
                            if (FinestraPrincipale.BackPanel.Panel_Giorno.Note.Text != "")
                            { Input.eventi.Add(new Eventi());
                                Input.eventi[Input.eventi.Count - 1].Set_Attributo("Note");
                                Input.eventi[Input.eventi.Count - 1].SetAttributi(newattributi);
                                Input.eventi[Input.eventi.Count - 1].SetData(data_aus);
                                Input.eventi[Input.eventi.Count - 1].SetData_modifica(Input.data_attuale);
                            }
                        }
                    }
                }
            
        }
}
}
