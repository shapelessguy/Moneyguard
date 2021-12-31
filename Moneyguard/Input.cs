using Moneyguard.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;

namespace Moneyguard
{
    public class Input
    {
        static public string filename = Program.filename;
        static public string filename_encr = Program.filename_encr;
        static public string path_moneyguard = @"C:\ProgramData\Cyan\Moneyguard\";
        static public string path = @"C:\ProgramData\Cyan\Moneyguard\local\";
        static public string path_in = path + filename + ".txt";
        static public string path_in2 = path + filename_encr + ".txt";
        static public string path_out = path + filename + "_.txt";
        static public string path_data;
        static public List<string> Updates;

        static public List<Eventi> eventi;
        static public List<Eventi_Aut> eventi_aut;
        static public List<Eventi> eventi_scremati;
        static public List<int> tipi_sort;
        static public List<int> metodi_sort;
        static public List<string> tipi;
        static public List<string> tipi_scremati;
        static public List<string> tipi_icons;
        static public List<string> metodi;
        static public List<string> metodi_scremati;
        static public List<double> totali;
        static public List<double> totali_iniziali;
        static public List<string> metodi_icons;
        static public List<string> attributi;
        static public List<string> attributi_icons;
        static public List<string> all_attributi;
        static public List<int> all_attributi_count;
        static public int[] data_attuale;
        static public int[] data_utile;
        static public Size Schermo = new Size(3840, 2160);
        static public int prova = 0;
        static public bool end_input = false;
        static public List<string> resources;
        static ResourceManager MyResourceClass;
        static public ResourceSet resourceSet;
        static public IEnumerable<string> dir_tip;
        static public IEnumerable<string> dir_met;
        static public List<string>[] Liste_attributi_tipi;
        static public int[] attributi_tipi_count;
        static public List<string> Lista_trasferimento;
        static public bool save_all_framments = false;
        static public string globals;
        static public int number_events_before;

        public static void RefreshPaths()
        {
            path = path_moneyguard + Program.Id_user + @"\";
            path_in = path + filename + ".txt";
            path_in2 = path + filename_encr + ".txt";
            path_out = path + filename + "_.txt";
            path_data = path + "Data";
        }
        public Input()
        {
            StartInput("");
        }
        public Input(string filename)
        {
            StartInput(filename);
        }

        private void StartInput(string filename)
        {
            Updates = new List<string>();
            eventi_scremati = new List<Eventi>();
            tipi_sort = new List<int>();
            metodi_sort = new List<int>();
            tipi = new List<string>();
            tipi_scremati = new List<string>();
            tipi_icons = new List<string>();
            metodi = new List<string>();
            metodi_scremati = new List<string>();
            totali = new List<double>();
            totali_iniziali = new List<double>();
            metodi_icons = new List<string>();
            attributi = new List<string>();
            attributi_icons = new List<string>();
            all_attributi = new List<string>();
            all_attributi_count = new List<int>();
            data_attuale = Date.GetActualDate();
            data_utile = new int[6];
            Schermo = new Size(3840, 2160);
            resources = new List<string>();
            MyResourceClass = new ResourceManager(typeof(Resources));
            resourceSet = MyResourceClass.GetResourceSet(System.Globalization.CultureInfo.CurrentUICulture, true, true);
            string[] readText_before = new string[0];
            if (filename == "") Impostazioni.readText = Framments.ReadAllFrammentsByLocal();
            else {
                //StringCipher.Decrypt(filename, "C://Users//Claudio//Desktop//bo.txt");
                //readText_before = Impostazioni.readText;
                //MessageBox.Show(filename);
                //StringCipher.DecryptFile(filename, filename + "t", StringCipher.GenerateKey());
                //MessageBox.Show("OK");
                Impostazioni.readText = StringCipher.DecodeAndDecryptFile(filename);
            }

            //Impostazioni.readText = Funzioni_utili.Decrypt(Input.path_in, Input.path_in2);
            //Impostazioni.readText = Framments.ReadAllFrammentsByLocal();
            //foreach (string stringa in Impostazioni.readText) Console.WriteLine(stringa);

            Impostazioni.pass = Funzioni_utili.GetString(Impostazioni.readText, "Pass: ");
            Impostazioni.timeout_pass = Funzioni_utili.GetInt(Impostazioni.readText, "Timeout: ");
            Impostazioni.question = Funzioni_utili.GetString(Impostazioni.readText, "Question: ");
            Impostazioni.answer = Funzioni_utili.GetString(Impostazioni.readText, "Answer: ");

            foreach (System.Collections.DictionaryEntry entry in resourceSet) resources.Add(entry.Key.ToString());
            Updates = Funzioni_utili.GetUpdates(Impostazioni.readText, "Update");
            tipi = Funzioni_utili.GetTipi(Impostazioni.readText, "Tipi: ");
            metodi = Funzioni_utili.GetTipi(Impostazioni.readText, "Metodi: "); totali.Clear(); foreach (string metodo in metodi) totali.Add(0);

            for (int i = 0; i < tipi.Count; i++) { tipi_sort.Add(i); }
            for (int i = 0; i < metodi.Count; i++) metodi_sort.Add(i);


            List<int> temp = Funzioni_utili.Get_Sort(Impostazioni.readText, "Ordine_tipi: ");
            if (temp.Count == tipi.Count) tipi_sort = temp;
            temp = Funzioni_utili.Get_Sort(Impostazioni.readText, "Ordine_metodi: ");
            if (temp.Count == metodi.Count) metodi_sort = temp;

            attributi = Funzioni_utili.GetTipi(Impostazioni.readText, "Attributi: ");
            tipi_icons = Funzioni_utili.GetTipi(Impostazioni.readText, "Tipi_icons: ");
            metodi_icons = Funzioni_utili.GetTipi(Impostazioni.readText, "Metodi_icons: ");
            totali_iniziali = Funzioni_utili.GetTotali_Iniziali(Impostazioni.readText, "Metodi_importi_iniziali: ");
            AdjustTotaliIniziali();
            attributi_icons = Funzioni_utili.GetTipi(Impostazioni.readText, "Attributi_icons: ");
            //all_attributi = Funzioni_utili.GetAttributi(Impostazioni.readText, "Attributes: ", "|^|");
            eventi = Eventi.GetEventi(Impostazioni.readText);
            eventi_aut = Eventi_Aut.GetEventi_Aut(Impostazioni.readText);
            RefreshData();
            RefreshImages();
            end_input = true;
            if(save_all_framments)
            {
                Program.caricamento_show = true;
                save_all_framments = false;
                Framments framments = new Framments();
                framments.SaveAllFramments();
                Program.caricamento_show = false;
            }
            globals = Framments.GetGlobals();
            number_events_before = eventi.Count;

        }
        static public void AdjustTotaliIniziali()
        {
            try
            {
                for (int i = totali_iniziali.Count - 1; i >= metodi.Count; i--) totali_iniziali.RemoveAt(i);
            }
            catch (Exception) { }
        }
        static public void Scrematura_eventi()
        {
            eventi_scremati.Clear();
            foreach (Eventi evento in eventi) {
                Eventi eventos = new Eventi();
                eventos.Set_Attributo(evento.Get_Attributo());
                eventos.SetTipo(evento.GetTipo());
                eventos.SetMetodo(evento.GetMetodo());
                eventos.SetValore(evento.GetValore());
                eventos.SetData(evento.GetData());
                eventos.SetData_modifica(evento.GetData_modifica());
                eventos.SetDatacode(evento.GetDatacode());
                eventos.SetDatacode_modifica(evento.GetDatacode_modifica());
                foreach (string stringa in evento.GetAttributi()) eventos.AddAttributo(stringa);
                eventi_scremati.Add(eventos);
            }

            foreach (Eventi evento in eventi_scremati)
            {
                evento.SetTipo(Funzioni_utili.Scremato(evento.GetTipo()));
                evento.SetMetodo(Funzioni_utili.Scremato(evento.GetMetodo()));
                for(int i=0; i<evento.GetAttributi().Count; i++) evento.SetAttributo(i, Funzioni_utili.Scremato(evento.GetAttributo(i)));
            }
        }

        static public void LoadAttributi()
        {
            all_attributi.Clear();
            all_attributi_count.Clear();
            tipi_scremati.Clear();
            metodi_scremati.Clear();
            tipi_scremati = new List<string>();
            Liste_attributi_tipi = new List<string>[tipi.Count];
            attributi_tipi_count = new int[tipi.Count];

            /*
            Liste_attributi_tipi_ = new List<Classe_ordine>[tipi.Count];

            for (int i = 0; i < tipi.Count; i++) Liste_attributi_tipi[i] = new List<string>();
            Lista_trasferimento = new List<string>();
            int indexx = -1;
            foreach (Eventi evento in eventi)
            {
                if (evento.attributo == "Introito" || evento.attributo == "Spesa")
                {
                    for (int j = 0; j < tipi.Count; j++) if (evento.tipo == tipi[j]) indexx = j;
                    for (int i = 0; i < evento.attributi.Count; i++)
                    {
                        foreach (Classe_ordine classe in Liste_attributi_tipi_[indexx]) { if(evento.attributi[i] == classe.attributo)   }
                    }
                }
                if (evento.attributo == "Trasferimento")
                {
                    for (int i = 0; i < evento.attributi.Count; i++) { if (Lista_trasferimento.Contains(evento.attributi[i])) continue; else Lista_trasferimento.Add(evento.attributi[i]); }
                }
            }
            */



            for (int i = 0; i < tipi.Count; i++) { Liste_attributi_tipi[i] = new List<string>(); attributi_tipi_count[i] = 0; }
            Lista_trasferimento = new List<string>();
            int index=-1;
            //eventi.Reverse();
            for(int m=eventi.Count-1; m>=0; m--)
            {
                if (eventi[m].Get_Attributo() == "Note") continue;
                if (eventi[m].Get_Attributo() == "Introito" || eventi[m].Get_Attributo() == "Spesa")
                {
                    for (int j = tipi.Count-1; j >= 0; j--) if (eventi[m].GetTipo() == tipi[j]) index = j;
                    for (int i = eventi[m].GetAttributi().Count-1; i >= 0 ; i--) {
                        if (Liste_attributi_tipi[index].Contains(eventi[m].GetAttributo(i))) continue;
                        else Liste_attributi_tipi[index].Add(eventi[m].GetAttributo(i));
                        //attributi_tipi_count[index]++;
                    }

                }
                if (eventi[m].Get_Attributo() == "Trasferimento")
                {
                    for (int i = eventi[m].GetAttributi().Count-1; i >=0; i--) {
                        if (Lista_trasferimento.Contains(eventi[m].GetAttributo(i))) continue;
                        else Lista_trasferimento.Add(eventi[m].GetAttributo(i));
                      //  attributi_tipi_count[index]++;
                    }
                }
            }
            for(int j=eventi.Count-1; j>=0;j--)
            {
                if (eventi[j].Get_Attributo() == "Note") continue;
                for (int i = 0; i < eventi[j].GetAttributi().Count; i++) {
                    if (all_attributi.Contains(eventi[j].GetAttributo(i))) continue;
                    else all_attributi.Add(eventi[j].GetAttributo(i));
                    //all_attributi_count++;
                }
            }

            for (int i = 0; i < tipi.Count; i++) tipi_scremati.Add(Funzioni_utili.Scremato(tipi[i]));
            for (int i = 0; i < metodi.Count; i++) metodi_scremati.Add(Funzioni_utili.Scremato(metodi[i]));
            for (int i = 0; i < all_attributi.Count; i++) all_attributi[i] = Funzioni_utili.Scremato(all_attributi[i]);
            for (int i = 0; i < Lista_trasferimento.Count; i++) Lista_trasferimento[i] = Funzioni_utili.Scremato(Lista_trasferimento[i]);
            for (int i = 0; i< tipi.Count; i++ ) for(int j=0; j<Liste_attributi_tipi[i].Count; j++) Liste_attributi_tipi[i][j] = Funzioni_utili.Scremato(Liste_attributi_tipi[i][j]);

            Sort();
            //eventi.Reverse();
        }

        static public void Sort()
        {
            /*foreach (List<string> lista in Liste_attributi_tipi) lista.Reverse();
            all_attributi.Reverse();
            tipi_scremati.Reverse();
            metodi_scremati.Reverse();*/
        }

        static public void RefreshImages()
        {
            dir_tip = Directory.EnumerateFiles(path + @"\Icons\Tipologie");
            dir_met = Directory.EnumerateFiles(path + @"\Icons\Metodi");
        }
        static public void RefreshData()
        {
            data_attuale = Date.GetActualDate();
            RefreshDataUtile();
        }
        static public void RefreshDataUtile()
        {
            if (data_attuale[2] < Impostazioni.ora_minuto.X) { data_utile = Date.DataIncrement(data_attuale, -60 * 60 * 24); data_utile[0] = 0; data_utile[1] = 59; data_utile[2] = 23; }
            else if (data_attuale[2] == Impostazioni.ora_minuto.X && data_attuale[1] < Impostazioni.ora_minuto.Y) { data_utile = Date.DataIncrement(data_attuale, -60 * 60 * 24); data_utile[0] = 0; data_utile[1] = 59; data_utile[2] = 23; }
            else data_utile = data_attuale;
        }

        static public void RefreshTotale()
        {
            Totale();
        }
        static public double[] Totale()
        {
            double[] totale = new double[3];
            for (int i = 0; i < Input.totali.Count; i++) Input.totali[i] = Input.totali_iniziali[i];
            foreach (Eventi evento in Input.eventi)
            {
                if (evento.Get_Attributo() == "Introito")
                {
                    totale[0] += evento.GetValore();
                    for (int i = 0; i < Input.totali.Count; i++)
                    {
                        if (evento.GetMetodo() == Input.metodi[i]) Input.totali[i] += evento.GetValore();
                    }
                }
                if (evento.Get_Attributo() == "Spesa")
                {
                    for (int i = 0; i < Input.totali.Count; i++)
                    {
                        if (evento.GetMetodo() == Input.metodi[i]) { Input.totali[i] -= evento.GetValore();  }
                    }
                    totale[1] += evento.GetValore();
                }
                if (evento.Get_Attributo() == "Trasferimento")
                {
                    totale[2] += evento.GetValore();
                    for (int i = 0; i < Input.totali.Count; i++)
                    {
                        if (evento.GetTipo() == Input.metodi[i]) Input.totali[i] -= evento.GetValore();
                        if (evento.GetMetodo() == Input.metodi[i]) Input.totali[i] += evento.GetValore();
                    }
                }
            }
            return totale;
        }
    }
}
