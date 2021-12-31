using Moneyguard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public class Eventi
    {
        private string attributo = "";
        private string metodo = "";
        private string tipo = "";

        private int[] data = null;
        private int[] data_modifica = null;
        private long datacode = 0;
        private long datacode_modifica = 0;
        private double valore = 0;
        private List<string> attributi = new List<string>();

        public int index;
        public List<int> enum_attr = new List<int>();
        public bool validation = false;

        const bool verbose = false;

        public Eventi()
        {
        }

        public string Info()
        {
            string stringa = "";
            try
            {
                stringa += attributo + " " + tipo + ", " + metodo + " -> " + valore + "$    " + Date.ShowDate(data) + " - " + Date.ShowDate(data_modifica) + " : ";
                foreach (string stringo in attributi) stringa += " " + stringo + ",";
            }
            catch (Exception) { }
            return stringa;
        }

        public void SetAttributi(List<string> attributi)
        {
            bool match = true;
            if (attributi.Count != this.attributi.Count) match = false;
            if (match) for (int i = 0; i < attributi.Count; i++) { if (Funzioni_utili.Scremato(attributi[i]) != Funzioni_utili.Scremato(this.attributi[i])) match = false; }
            if (match) return;

            this.attributi.Clear();
            for (int i = 0; i < attributi.Count; i++) this.attributi.Add(attributi[i]);
            validation = false;
            if (verbose) Console.WriteLine(Info() + " - to validate: Attributi" );
        }
        public void SetAttributo(int i, string attributo)
        {
            if (this.attributi[i] == attributo) return;
            this.attributi[i] = attributo;
            validation = false;
            if (verbose) Console.WriteLine(Info() + " - to validate: Attributi[i]");
        }
        public List<string> GetAttributi()
        {
            return this.attributi;
        }
        public string GetAttributo(int i)
        {
            return this.attributi[i];
        }
        public void AddAttributo(string attributo)
        {
            this.attributi.Add(attributo);
            validation = false;
            if (verbose) Console.WriteLine(Info() + " - to validate: +Attributi[i]");
        }

        public void SetData(int[] data)
        {
            if(this.data != null)
            {
                if (data.Length != this.data.Length) return;
                bool match = true;
                if (match) for (int i = 0; i < data.Length; i++) if (data[i] != this.data[i]) match = false;
                if (match) return;
            }

            this.data = new int[data.Length];
            for(int i=0; i<data.Length; i++) this.data[i] = data[i];
            validation = false;
            if (verbose) Console.WriteLine(Info() + " - to validate: Data");
        }
        public int[] GetData()
        {
            return this.data;
        }
        public void SetData_modifica(int[] data)
        {
            if (this.data_modifica != null)
            {
                if (data.Length != this.data_modifica.Length) return;
                bool match = true;
                if (match) for (int i = 0; i < data.Length; i++) if (data[i] != this.data_modifica[i]) match = false;
                if (match) return;
            }

            this.data_modifica = new int[data.Length];
            for (int i = 0; i < data.Length; i++) this.data_modifica[i] = data[i];
            validation = false;
            if (verbose) Console.WriteLine(Info() + " - to validate: Data_modifica");
        }
        public int[] GetData_modifica()
        {
            return this.data_modifica;
        }
        public void SetDatacode(long datacode)
        {
            if (this.datacode == datacode) return;
            this.datacode = datacode;
            validation = false;
            if (verbose) Console.WriteLine(Info() + " - to validate: Datacode");
        }
        public long GetDatacode()
        {
            return this.datacode;
        }
        public void SetDatacode_modifica(long datacode)
        {
            if (this.datacode_modifica == datacode) return;
            this.datacode_modifica = datacode;
            validation = false;
            if (verbose) Console.WriteLine(Info() + " - to validate: Datacode_modifica");
        }
        public long GetDatacode_modifica()
        {
            return this.datacode_modifica;
        }

        public string Get_Attributo()
        {
            return attributo;
        }
        public void Set_Attributo(string attributo)
        {
            if (this.attributo == attributo) return;
            this.attributo = attributo;
            validation = false;
            if (verbose) Console.WriteLine(Info() + " - to validate: Attributo");
        }
        public double GetValore()
        {
            return valore;
        }
        public void SetValore(double valore)
        {
            if (this.valore == valore) return;
            this.valore = valore;
            validation = false;
            if (verbose) Console.WriteLine(Info() + " - to validate:Valore");
        }
        public string GetTipo()
        {
            return tipo;
        }
        public void SetTipo(string tipo)
        {
            if (this.tipo == tipo) return;
            this.tipo = tipo;
            validation = false;
            if (verbose) Console.WriteLine(Info() + " - to validate: Tipo");
        }
        public string GetMetodo()
        {
            return metodo;
        }
        public void SetMetodo(string metodo)
        {
            if (this.metodo == metodo) return;
            this.metodo = metodo;
            validation = false;
            if (verbose) Console.WriteLine(Info() + " - to validate: Metodo");
        }

        public void Get_Enum_attr()
        {
            if (validation) return;
            enum_attr.Clear();
            foreach (string attributo in attributi)
            {
                for (int i = 0; i < Input.all_attributi.Count; i++)
                {
                    if (Funzioni_utili.Scremato(attributo) == Input.all_attributi[i]) enum_attr.Add(i);
                }
            }
        }
        public void Get_Attributi()
        {
            attributi.Clear();
            for (int i = 0; i < Input.all_attributi.Count; i++)
            {
                foreach (int val in enum_attr)
                {
                    if (val == i) attributi.Add(Input.all_attributi[i]);
                }
            }
            validation = true;
        }

        public void Load()
        {
            datacode = Date.Codifica(data);
            datacode_modifica = Date.Codifica(data_modifica);
        }


        private static int[] ShowDate(string data_txt)
        {
            int[] data = new int[6];
            string stringa = "";
            int indice = 0;
            
            for (int i = indice; ; i++) { if (data_txt.Substring(i, 1) == "/") { indice = i + 1; data[3] = Convert.ToInt32(stringa); stringa = ""; break; } else stringa += data_txt.Substring(i, 1); }
            for (int i = indice; ; i++) { if (data_txt.Substring(i, 1) == "/") { indice = i + 1; data[4] = Convert.ToInt32(stringa); stringa = ""; break; } else stringa += data_txt.Substring(i, 1); }
            for (int i = indice; ; i++) { if (data_txt.Substring(i, 1) == " ") { indice = i + 1; data[5] = Convert.ToInt32(stringa); stringa = ""; break; } else stringa += data_txt.Substring(i, 1); }
            for (int i = indice; ; i++) { if (data_txt.Substring(i, 1) == ":") { indice = i + 1; data[2] = Convert.ToInt32(stringa); stringa = ""; break; } else stringa += data_txt.Substring(i, 1); }
            for (int i = indice; ; i++) { if (data_txt.Substring(i, 1) == ":") { indice = i + 1; data[1] = Convert.ToInt32(stringa); stringa = ""; break; } else stringa += data_txt.Substring(i, 1); }
            for (int i = indice; i < data_txt.Length; i++) { stringa += data_txt.Substring(i, 1); }
            data[0] = Convert.ToInt32(stringa);
            return data;
        }



        public static List<Eventi> Order_datacode_datacode_modifica(List<Eventi> eventi)
        {
            foreach (Eventi evento in eventi) { evento.datacode = Date.Codifica(evento.data); evento.datacode_modifica = Date.Codifica(evento.data_modifica); }
            eventi = eventi.OrderBy(o => o.datacode).ToList();

            List<List<Eventi>> superlista = new List<List<Eventi>>();
            int j = -1;
            if (eventi.Count > 0)
            {
                superlista.Add(new List<Eventi>());
                superlista[0].Add(eventi[0]);
                j++;
                for (int i = 1; i < eventi.Count; i++)
                {
                    if (eventi[i].datacode != eventi[i - 1].datacode) { superlista.Add(new List<Eventi>()); j++; }
                    superlista[j].Add(eventi[i]);
                }
            }
            for (int i = 0; i < superlista.Count; i++) { superlista[i] = superlista[i].OrderBy(o => o.datacode_modifica).ToList(); }
            //superlista.Reverse();

            List<Eventi> eventi_out = new List<Eventi>();
            for (int m = 0; m < j + 1; m++)
            {
                for (int i = 0; i < superlista[m].Count; i++)
                {
                    eventi_out.Add(superlista[m][i]);
                }
            }
            //eventi_out.Reverse();
            return eventi_out;
        }
        public static List<Eventi> Order_datacode_valore(List<Eventi> eventi)
        {
            foreach (Eventi evento in eventi) { evento.datacode = Date.Codifica(evento.data); evento.datacode_modifica = Date.Codifica(evento.data_modifica); }
            eventi = eventi.OrderBy(o => o.datacode).ToList();

            List<List<Eventi>> superlista = new List<List<Eventi>>();
            int j = -1;
            if (eventi.Count > 0)
            {
                superlista.Add(new List<Eventi>());
                superlista[0].Add(eventi[0]);
                j++;
                for (int i = 1; i < eventi.Count; i++)
                {
                    if (eventi[i].datacode != eventi[i - 1].datacode) { superlista.Add(new List<Eventi>()); j++; }
                    superlista[j].Add(eventi[i]);
                }
            }
            for (int i = 0; i < superlista.Count; i++) { superlista[i] = superlista[i].OrderBy(o => o.valore).ToList(); superlista[i].Reverse(); }
            //superlista.Reverse();

            List<Eventi> eventi_out = new List<Eventi>();
            for (int m = 0; m < j + 1; m++)
            {
                for (int i = 0; i < superlista[m].Count; i++)
                {
                    eventi_out.Add(superlista[m][i]);
                }
            }
            //eventi_out.Reverse();
            return eventi_out;
        }

        /////////////////////////////////////////////////////////////////////////  Caricamento Eventi (System, System.IO)
        public static List<Eventi> GetEventi(string[] readText)
        {
            Console.WriteLine("Loading Eventi from file in AppData");
            List<Eventi> eventi = new List<Eventi>();
            string stringa = "";
            bool segnale = false;
            bool lettura = false;
            int indice = 0, i = -1, endline = 0;
            long datacode_max = 0;
            
            foreach (var line in readText)
            {
                if (segnale == false && line.Contains("Eventi:")) { segnale = true; continue; }
                if (segnale == false) { continue; }
                if (line.Length == 0 || line.Length == 1) continue;
                if (line.Contains("Eventi: End")) { return eventi; }

                i++; endline = 0;
                eventi.Add(new Eventi());

                for (int j = indice; ; j++)
                {
                    if (line.Substring(j, 1) == " ")
                    {
                        eventi[i].attributo = Associazione.DecodificaAttributo(stringa);
                        eventi[i].index = i;
                        stringa = ""; indice = j + 1; break;
                    }
                    stringa += line.Substring(j, 1);
                }

                for (int j = indice; ; j++)
                {
                    if (eventi[i].attributo == "Note") { if (stringa == "") eventi[i].valore = 0; else eventi[i].valore = Convert.ToDouble(stringa); break; }
                    if (line.Substring(j, 1) == " ")
                    {
                        if (stringa == "") eventi[i].valore = 0;
                        else eventi[i].valore = Convert.ToDouble(stringa);
                        stringa = ""; indice = j + 1; break;
                    }
                    stringa += line.Substring(j, 1);
                }

                for (int j = indice; ; j++)
                {
                    if (eventi[i].attributo == "Note") { eventi[i].metodo = stringa; break; }
                    if (line.Substring(j, 1) == " ")
                    {
                        eventi[i].metodo = Associazione.DecodificaMetodo(stringa);
                        stringa = ""; indice = j + 1; break;
                    }
                    stringa += line.Substring(j, 1);
                }
                
                for (int j = indice; ; j++)
                {
                    if (eventi[i].attributo == "Note") { eventi[i].tipo = stringa; break; }
                    if (line.Substring(j, 1) == " ")
                    {
                        lettura = false;
                        if(eventi[i].attributo == "Introito" || eventi[i].attributo == "Spesa") eventi[i].tipo = Associazione.DecodificaTipo(stringa);
                        else if(eventi[i].attributo == "Trasferimento") eventi[i].tipo = Associazione.DecodificaMetodo(stringa);
                        stringa = ""; indice = j + 1; break;
                    }
                    stringa += line.Substring(j, 1);
                }
                
                for (int j = indice; ; j++)
                {
                    if (line.Substring(j, 1) == " ")
                    {
                        eventi[i].datacode = Convert.ToInt32(stringa);
                        eventi[i].data = Date.Decodifica((uint)eventi[i].datacode);
                        stringa = ""; indice = j + 1; break;
                    }
                        stringa += line.Substring(j, 1);
                }

                for (int j = indice; ; j++)
                {
                    if (line.Substring(j, 1) == " ")
                    {
                        eventi[i].datacode_modifica = Convert.ToInt32(stringa);
                        if (eventi[i].datacode_modifica <= datacode_max) eventi[i].datacode_modifica = datacode_max + 10;
                        datacode_max = eventi[i].datacode_modifica;
                        eventi[i].data_modifica = Date.Decodifica((uint)eventi[i].datacode_modifica);

                        stringa = ""; indice = j + 1; break;
                    }
                    stringa += line.Substring(j, 1);
                }
                
                for (int m = 0; ; m++)
                {
                    /*
                    for (int j = indice; ; j++)
                    {
                        if (line.Substring(j, 5) == "|^*^|") { endline = 1; break; }
                        lettura = true;
                        if (lettura == false && line.Substring(j, 1) == " ") { lettura = true; j += 1; continue; }
                        if (lettura == true && line.Substring(j, 1) == " ")
                        {
                            lettura = false;
                            eventi[i].enum_attr.Add(Convert.ToInt32(stringa));
                            stringa = ""; indice = j + 1; break;
                        }
                        if (lettura)
                        {
                            stringa += line.Substring(j, 1);
                        }
                    }
                    if (endline == 1) { indice = 0; eventi[i].Load(); eventi[i].Get_Attributi(); break; }
                    */
                    
                    for (int j = indice; ; j++)
                    {
                        if (line.Substring(j, 5) == "|^*^|") { endline = 1; break; }
                        if (lettura == false && line.Substring(j, 5) == "|*^*|") { lettura = true; j += 4; continue; }
                        if (lettura == true && line.Substring(j, 5) == "|*^*|")
                        {
                            lettura = false;
                            eventi[i].attributi.Add(stringa);
                            stringa = ""; indice = j + 1; break;
                        }
                        if (lettura)
                        {
                            stringa += line.Substring(j, 1);
                        }
                    }
                    if (endline == 1) { indice = 0; eventi[i].Load(); break; }
                 
                }
                eventi[i].validation = true;

            }
            //eventi.Reverse();
            return eventi;
        }
    }
}
