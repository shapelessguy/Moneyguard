using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication1;

namespace Moneyguard
{
    public class Framment
    {
        public List<Eventi> eventi = null;
        public int[] data = null;
        public DateTime data_datetime;
        public string filename;
        public string path;
        public string path_filename;
        public string text = "";
        public Framment(Eventi evento, string[] files)
        {
            path = Input.path;
            eventi = new List<Eventi>();
            Add(evento);
            string aus2 = "";
            if (data[1] < 10) aus2 += "0";
            filename = "Data" + data[2] + aus2 + data[1] + ".txt";
            path_filename = path + @"Data\" + filename;
        }
        public Framment(string text, string[] files)
        {
            path = Input.path;
            filename = "Data000000.txt";
            path_filename = path + @"Data\" + filename;
            this.text += text;
        }

        public void Add(Eventi evento)
        {
            eventi.Add(evento);
            data = new int[3] { 1, evento.GetData()[4], evento.GetData()[5] };
            data_datetime = new DateTime(evento.GetData()[5], evento.GetData()[4], 1);
            //Console.WriteLine(evento.Info());
        }

        public void Save()
        {
            string stringa_decr = "PRIMAFRASECHENON|^.^|";
            string stringa_encr = "";
            if (text != "")
            {
                stringa_decr += text;
                stringa_encr = StringCipher.Encrypt_Encode(stringa_decr);
                try
                {
                    if (File.Exists(path_filename))
                    {
                        using (StreamReader sr = new StreamReader(path_filename))
                        {
                            if (stringa_encr == sr.ReadToEnd()) return;
                        }
                    }

                    using (StreamWriter sw = File.CreateText(path_filename))
                    {
                        sw.Write(stringa_encr);
                    }
                    File.SetCreationTimeUtc(path_filename, DateTime.Now);
                    Console.WriteLine("File: " + path_filename + " is been updated");
                    return;
                }
                catch (Exception e) { Console.WriteLine("Error: " + e.Message);  return; }
            }





            int i = 0;
            foreach (Eventi evento in eventi)
            {
                if (i > 0) stringa_decr += "\n";
                evento.Load();
                stringa_decr += Associazione.CodificaAttributo(evento.Get_Attributo()) + " ";
                if (evento.Get_Attributo() != "Note")
                {
                    stringa_decr += Convert.ToString(evento.GetValore()) + " ";
                    stringa_decr += Associazione.CodificaMetodo(evento.GetMetodo()) + " ";
                    if (evento.Get_Attributo() == "Introito" || evento.Get_Attributo() == "Spesa") stringa_decr += Associazione.CodificaTipo(evento.GetTipo()) + " ";
                    else if (evento.Get_Attributo() == "Trasferimento") stringa_decr += Associazione.CodificaMetodo(evento.GetTipo()) + " ";
                }
                stringa_decr += evento.GetDatacode() + " ";
                stringa_decr += evento.GetDatacode_modifica() + " ";

                //Console.WriteLine(evento.Info());
                for (int m = 0; m < evento.GetAttributi().Count; m++)
                {
                    stringa_decr += "|*^*|" + evento.GetAttributo(m) + "|*^*|";
                }
                stringa_decr += "|^*^|";
                evento.validation = true;
                i++;
            }
            //Console.WriteLine(stringa_decr);

            stringa_encr = StringCipher.Encrypt_Encode(stringa_decr);
            try
            {
                if (File.Exists(path_filename))
                {
                    using (StreamReader sr = new StreamReader(path_filename))
                    {
                        if (stringa_encr == sr.ReadToEnd()) return;
                    }
                }
                File.Delete(path_filename);
                using (StreamWriter sw = File.CreateText(path_filename))
                {
                    sw.Write(stringa_encr);
                }
                File.SetCreationTimeUtc(path_filename, DateTime.Now);
                Console.WriteLine("File: " + path_filename + " is been updated");
            }
            catch (Exception e) { Console.WriteLine("Error: " + e.Message); }
        }

        public string[] Read()
        {
            string stringa;
            using(StreamReader sr = new StreamReader(path_filename))
            {
                stringa = StringCipher.Decode_Decrypt(sr.ReadToEnd());
            }
            string sep = "|^.^|";
            string[] read = stringa.Split(new string[] { sep }, 2, StringSplitOptions.RemoveEmptyEntries);
            string[] read_filtro = read[1].Split('\n');
            return read_filtro;
        }
    }
}
