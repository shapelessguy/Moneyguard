using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication1;

namespace Moneyguard
{
    public class Framments
    {
        List<Framment> catalogo;
        List<Framment> catalogo_to_save;
        string[] files;
        private List<DateTime> months_in_local;
        private List<DateTime> months_to_save;
        public Framments(List<DateTime> months_to_save)
        {
            this.months_to_save = months_to_save;
            if (!Directory.Exists(Input.path + @"Data")) { Console.WriteLine("Creating Data directory"); Directory.CreateDirectory(Input.path + @"Data"); }
            files = Directory.GetFiles(Input.path+@"Data");

            List<string> temp = new List<string>();
            foreach(string file in files)
            {
                if (Path.GetFileNameWithoutExtension(file) != "Info") temp.Add(file);
            }
            files = temp.ToArray();

            //GetDays_onLocal(files);
            GetMonths_onLocal(files);

            catalogo_to_save = new List<Framment>();
            catalogo_to_save.Add(new Framment(GetGlobals(), files));
            //days_to_save.OrderBy(o => o.Millisecond);
            months_to_save.OrderBy(o => o.Millisecond);


            /*
            if (days_to_save.Count != 0)
            {
                int j = -1;
                for (int i = 0; i < Input.eventi.Count; i++)
                {
                    DateTime data_evento = new DateTime(Input.eventi[i].GetData()[5], Input.eventi[i].GetData()[4], Input.eventi[i].GetData()[3]);
                    if (!days_to_save.Contains(data_evento)) continue;
                    j++;
                    if (j == 0)
                    {
                        catalogo_to_save.Add(new Framment(Input.eventi[i], files));
                        continue;
                    }

                    if (data_evento.Day == catalogo_to_save.Last().data[0] && data_evento.Month == catalogo_to_save.Last().data[1] && data_evento.Year == catalogo_to_save.Last().data[2]) { catalogo_to_save.Last().Add(Input.eventi[i]); }
                    else { Console.WriteLine(); Console.WriteLine(); catalogo_to_save.Add(new Framment(Input.eventi[i], files)); }
                }
            }*/

            if (months_to_save.Count != 0)
            {
                int j = -1;
                for (int i = 0; i < Input.eventi.Count; i++)
                {
                    DateTime data_evento = new DateTime(Input.eventi[i].GetData()[5], Input.eventi[i].GetData()[4], 1);
                    if (!months_to_save.Contains(data_evento)) continue;
                    j++;
                    if (j == 0)
                    {
                        catalogo_to_save.Add(new Framment(Input.eventi[i], files));
                        continue;
                    }

                    if (data_evento.Month == catalogo_to_save.Last().data[1] && data_evento.Year == catalogo_to_save.Last().data[2]) { catalogo_to_save.Last().Add(Input.eventi[i]); }
                    else { catalogo_to_save.Add(new Framment(Input.eventi[i], files)); }
                }
            }

            /*
            catalogo = new List<Framment>();
            catalogo.Add(new Framment(GetGlobals(), files));
            for (int i=0; i<Input.eventi.Count; i++)
            {
                int[] data_evento = new int[3] { Input.eventi[i].GetData()[3], Input.eventi[i].GetData()[4], Input.eventi[i].GetData()[5] };

                if (i == 0) {
                    catalogo.Add(new Framment(Input.eventi[i], files));
                    continue;
                }


               if (data_evento[0] == catalogo.Last().data[0] && data_evento[1] == catalogo.Last().data[1] && data_evento[2] == catalogo.Last().data[2])  { catalogo.Last().Add(Input.eventi[i]);}
                else catalogo.Add(new Framment(Input.eventi[i], files));
            }*/
        }


        public Framments()
        {
            if (!Directory.Exists(Input.path + @"Data")) { Console.WriteLine("Creating Data directory"); Directory.CreateDirectory(Input.path + @"Data"); }
            files = Directory.GetFiles(Input.path + @"Data"); Console.WriteLine("New Framments");
            //GetDays_onLocal(files);
            GetMonths_onLocal(files);
            catalogo = new List<Framment>();
            catalogo.Add(new Framment(GetGlobals(), files));
            for (int i = 0; i < Input.eventi.Count; i++)
            {
                int[] data_evento = new int[3] { 1, Input.eventi[i].GetData()[4], Input.eventi[i].GetData()[5] };

                if (i == 0)
                {
                    catalogo.Add(new Framment(Input.eventi[i], files));
                    continue;
                }


                if (data_evento[1] == catalogo.Last().data[1] && data_evento[2] == catalogo.Last().data[2]) { catalogo.Last().Add(Input.eventi[i]); }
                else catalogo.Add(new Framment(Input.eventi[i], files));
            }
        }

        public void SaveAllFramments()
        {
            foreach (Framment framment in catalogo)
            {
                framment.Save();
            }
            Console.WriteLine(" - All framments saved!");
        }
        public void SaveFramments_toSave()
        {
            DeleteUselessFiles();

            foreach (Framment framment in catalogo_to_save)
            {
                framment.Save();
            }
            MyMetadata.CreateInfo();
            Console.WriteLine(" - Monthly framments saved!");
        }

        public void DeleteUselessFiles()
        {
            foreach (DateTime datetime in months_to_save)
            {
                bool trovato = false;
                bool nullo = true;
                foreach (DateTime datetime_local in months_in_local)
                {
                    if (datetime == datetime_local) trovato = true;
                }
                foreach (Framment framment in catalogo_to_save)
                {
                    if (months_to_save.Contains(framment.data_datetime)) nullo = false;
                }

                if (trovato && nullo)
                {
                    string aus2 = "";
                    if (datetime.Month < 10) aus2 += "0";
                    try
                    {
                        string path = Input.path + @"Data\" + "Data" + datetime.Year + aus2 + datetime.Month + ".txt";
                        File.Delete(path);
                        //using (StreamWriter sw = new StreamWriter(path)) { sw.Write(StringCipher.Encrypt_Encode("PRIMAFRASECHENON|^.^|")); }
                        Console.WriteLine("File: " + path + " is been deleted");
                    }
                    catch (Exception) { }
                }
            }
        }

        public List<string> ReadAllFramments()
        {
            List<string> lista = new List<string>();

            int i = 0;
            foreach (Framment framment in catalogo)
            {
                if(i==1) lista.Add("Eventi:");
                string[] stringhe = framment.Read();
                foreach (string stringa in stringhe) lista.Add(stringa);
                i++;
            }
            lista.Add("Eventi: End");
            Console.WriteLine("All framments readed");
            return lista;
        }
        public static string[] ReadAllFrammentsByLocal()
        {
            List<string> lista = new List<string>();
            Console.WriteLine("Reading all framments");

            int i = 0;
            string[] get_files = Directory.GetFiles(Input.path + @"Data");
            if (get_files.Length == 0)
            {
                string[] readText = Funzioni_utili.Decrypt(Input.path_in, Input.path_in2);
                Input.save_all_framments = true;
                return readText;
            }
            foreach (string stringa in get_files)
            {
                if (Path.GetFileNameWithoutExtension(stringa) == "Info") continue;
                if (i == 1) lista.Add("Eventi:");

                string asp;
                using (StreamReader sr = new StreamReader(stringa))
                {
                    asp = StringCipher.Decode_Decrypt(sr.ReadToEnd());
                }
                string[] read = asp.Split(new string[] { "|^.^|" }, 2, StringSplitOptions.RemoveEmptyEntries);
                if (read.Length < 2) continue;
                string[] read_filtro = read[1].Split('\n');

                
                for (int j=0; j<read_filtro.Length; j++) lista.Add(read_filtro[j]);
                i++;
            }
            lista.Add("Eventi: End");
            Console.WriteLine("All framments readed");
            return lista.ToArray();
        }

        public static string GetGlobals()
        {
            string text = "";
            text += "Pass: " + Impostazioni.pass + ";\n";
            text += "Timeout: " + Impostazioni.timeout_pass + ";\n";
            text += "Question: " + Impostazioni.question + ";\n";
            text += "Answer: " + Impostazioni.answer + ";\n";

            foreach (string stringa in Input.Updates)
            {
                text += stringa +"\n";
            }

            text += "Attributi: ";
            foreach (string attributo in Input.attributi)
            {
                text += attributo + ";";
            }
            text += "\n";

            text += "Attributi_icons: ";
            foreach (string attributo in Input.attributi)
            {
                text += Associazione.AiconaAssociata(attributo) + ";";
            }
            text += "\n";

            text += "Ordine_tipi: ";
            foreach (int intero in Input.tipi_sort)
            {
                text += intero + ";";
            }
            text += "\n";

            text += "Ordine_metodi: ";
            foreach (int intero in Input.metodi_sort)
            {
                text += intero + ";";
            }
            text += "\n";

            text += "Tipi: ";
            foreach (string tipo in Input.tipi)
            {
                text += tipo + ";";
            }
            text += "\n";

            text += "Tipi_icons: ";
            foreach (string tipo in Input.tipi)
            {
                text += Associazione.IconaAssociata(tipo) + ";";
            }
            text += "\n";

            text += "Metodi: ";
            foreach (string metodo in Input.metodi)
            {
                text += metodo + ";";
            }
            text += "\n";

            text += "Metodi_icons: ";
            foreach (string metodo in Input.metodi)
            {
                text += Associazione.MiconaAssociata(metodo) + ";";
            }
            text += "\n";

            text += "Metodi_importi_iniziali: ";
            foreach (double valore in Input.totali_iniziali)
            {
                text += Convert.ToString(valore) + ";";
            }
            text += "\n";

            text += "Eventi_Aut:\n";
            foreach (Eventi_Aut evento in Input.eventi_aut)
            {
                text += evento.StringToSave();
            }
            text += "Eventi_Aut: End";

            return text;
        }
        
        public void GetMonths_onLocal(string[] files)
        {
            months_in_local = new List<DateTime>();
            foreach (string file in files)
            {
                string filename = Path.GetFileNameWithoutExtension(file).Substring(4);
                int year = Convert.ToInt32(filename.Substring(0, 4));
                int month = Convert.ToInt32(filename.Substring(4, 2));
                if (year != 0) months_in_local.Add(new DateTime(year, month, 1));
            }
        }

    }
}
