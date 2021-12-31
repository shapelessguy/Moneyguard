using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Drawing;
using Moneyguard;
using System.Resources;
using Moneyguard.Properties;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{
    class Funzioni_utili
    {
        static string sSecretKey_string = "?	V??7u?";
        public static string[] Decrypt(string path1, string path2)
        {
            //Console.WriteLine("Decrypting..");
            string sSecretKey = StringCipher.GenerateKey();
            StringCipher.DecryptFile(path1, path2, sSecretKey);

            //string sSecretKey = sSecretKey_string;
            if (StringCipher.DecryptFile(path1, path2, sSecretKey) == 1)
            {
                Console.Write("Decrypting..   ");
                try { File.Delete(path2); } catch (Exception) { Console.WriteLine("Cannot delete path2"); }
                StringCipher.DecodeAndDecrypt(path1, path2);
                //Console.WriteLine("End Decryption2");
            }

            string[] readText = File.ReadAllLines(path2);
            try { File.Delete(path2); } catch (Exception) { }

            // Remove the Key from memory. 
            //StringCipher.ZeroMemory(gch.AddrOfPinnedObject(), sSecretKey.Length * 2);
            //gch.Free();
            Console.WriteLine("End Decryption");
            return readText;
        }
        public static void Encrypt(string path1, string path2)
        {
            Console.Write("Encrypting..   ");

            try
            {
                string sSecretKey;
                //sSecretKey = StringCipher.GenerateKey();
                sSecretKey = sSecretKey_string;

                // For additional security Pin the key.
                //GCHandle gch = GCHandle.Alloc(sSecretKey, GCHandleType.Pinned);

                // Encrypt the file.        
                //StringCipher.EncryptFile(path1, path2, sSecretKey);
                StringCipher.EncryptAndEncode(path1, path2);
                File.Delete(path1);
                File.Move(path2, path1);

                // Remove the Key from memory. 
                //StringCipher.ZeroMemory(gch.AddrOfPinnedObject(), sSecretKey.Length * 2);
                //gch.Free();
                Console.WriteLine("End Encryption");
            }
            catch (Exception) { }
        }
        public static string EstraiValore(string text)
        {
            string zero = "0,00\u20AC";
            if (text == "") return zero;
            string unità="", centesimi="";
            bool virgola = false;
            int i = 0;
            for(i=0; i<text.Length; i++)
            {
                if (text.Substring(i, 1) == "," || text.Substring(i, 1) == ".") { virgola = true; break; }
                else if (text.Substring(i, 1) != "'" || text.Substring(i, 1) != " " || text.Substring(i, 1) != "") unità += text.Substring(i, 1);
            }
            if (virgola)
            {
                for (i++ ; i < text.Length; i++)
                {
                    if (text.Substring(i, 1) == "," || text.Substring(i, 1) == ".") { return zero; }
                    else if (text.Substring(i, 1) != "'" || text.Substring(i, 1) != " " || text.Substring(i, 1) != "") centesimi += text.Substring(i, 1);
                }
            }
            
            if(centesimi != "") try { double valore = Convert.ToDouble(unità) + SetCentesimi(centesimi); return FormatoStandard(valore) + "\u20AC"; } catch (Exception) { return zero; }
            else try {  double valore = Convert.ToDouble(unità); if(valore>=1000) return valore.ToString() + "\u20AC"; else return valore.ToString() + ",00\u20AC"; } catch (Exception) { return zero; }

        }
        /////////////////////////////////////////////////////////////////////////
        public static string Scremato(string text)
        {
            if (text.Length == 0) return "";
            int i, j;
            for (i = 0; i < text.Length; i++) if (text.Substring(i, 1) == " ") { if (i == text.Length - 1) return ""; else continue; } else break;
            for (j = text.Length - 1; j >= 0; j--) if (text.Substring(j, 1) == " ") continue; else break;
            string stringa = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text.Substring(i, j - i + 1).ToLower());
            for (; ; ) if (stringa.Contains("  ")) stringa = stringa.Replace("  ", " "); else return stringa;
        }

        /////////////////////////////////////////////////////////////////////////
        public static string ParzialmenteScremato(string text)
        {
            if (text.Length == 0) return "";
            int i, j;
            for (i = 0; i < text.Length; i++) if (text.Substring(i, 1) == " ") { if (i == text.Length - 1) return ""; else continue; } else break;
            for (j = text.Length - 1; j >= 0; j--) if (text.Substring(j, 1) == " ") continue; else break;
            return text.Substring(i, j - i + 1);
        }
        /////////////////////////////////////////////////////////////////////////
        public static int GetInt(string[] readText, string segnoinizio)
        {
            int output = 0;
            string stringa = "";
            //string[] readText = File.ReadAllLines(path);
            //readText = Decrypt(readText);
            foreach (string line in readText)
            {
                if (line.Contains(segnoinizio)) stringa = line.Substring(segnoinizio.Length);
            }

            try { stringa = stringa.Substring(0, stringa.Length - 1); output = Convert.ToInt32(stringa); } catch (Exception) { return output; }
            return output;
        }
        /////////////////////////////////////////////////////////////////////////
        public static string GetString(string[] readText, string segnoinizio)
        {
            string stringa = "none";
            //string[] readText = File.ReadAllLines(path);
            //readText = Decrypt(readText);
            foreach (string line in readText)
            {
                if (line.Contains(segnoinizio)) stringa = line.Substring(segnoinizio.Length);
            }
            if (stringa == "none") return stringa;
            try { stringa = stringa.Substring(0, stringa.Length - 1); } catch (Exception) { return stringa; }
            return stringa;
        }
        /////////////////////////////////////////////////////////////////////////
        public static Point GetPoint(string[] readText, string segnoinizio, bool verify)
        {
            string stringa = "";
            int prosegui = 0, j = 0;
            Point location = new Point(0,0);
            //string[] readText = File.ReadAllLines(path);
            //readText = Decrypt(readText);
            foreach (string line in readText)
            {
                if (prosegui == 1) break;
                if (line.Contains(segnoinizio))
                {
                    for (int i = (segnoinizio.Length); i < line.Length; i++)
                    {
                        if (line.Substring(i, 1) == ";") { if (j == 0) { location.X = Convert.ToInt32(stringa); } if (j == 1) { location.Y = Convert.ToInt32(stringa); } j++; stringa = ""; }
                        else { stringa += line.Substring(i, 1); }
                    }
                    prosegui = 1;
                }
            }
            if (!verify) return location;

            return VerifyLocation(location);
        }
        ///////////////////////////////////////////////////////////////////////// 
        public static Point VerifyLocation(Point location)
        {
            List<Rectangle> lista_rett = new List<Rectangle>();
            foreach (Screen screen in Screen.AllScreens) lista_rett.Add(screen.Bounds);
            bool interno = false;
            foreach (Rectangle rett in lista_rett) if (rett.Contains(location)) interno = true;
            if (!interno) location = new Point(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y);
            return location;
        }
        ///////////////////////////////////////////////////////////////////////// 
        public static int GetWidgetZoom(string[] readText, string segnoinizio)
        {
            int zoom = 0;
            //string[] readText = File.ReadAllLines(path);
            //readText = Decrypt(readText);
            foreach (string line in readText)
            {
                if (line.Contains(segnoinizio))
                {
                    zoom = Convert.ToInt32(line.Substring(segnoinizio.Length, line.Length - segnoinizio.Length - 1));
                }
            }
            return zoom;
        }
        ///////////////////////////////////////////////////////////////////////// 
        public static string NewName(int num_max, int tipo_metodo)
        {
            List<string> nomi = new List<string>();
            if (tipo_metodo == 1) foreach (string resource in Input.dir_tip) nomi.Add(TakeFileName(resource));
            else foreach (string resource in Input.dir_met) nomi.Add(TakeFileName(resource));

            List<int> avaible_num = new List<int>();
            for (int i = 0; i < num_max; i++) avaible_num.Add(i);
            List<int> num = new List<int>();
            foreach (string nom in nomi) try { num.Add(Convert.ToInt16(nom.Substring(3))); } catch (Exception) { }
            for (int i = 0; i < num_max; i++)
            {
                if (num.Contains(i)) avaible_num.Remove(i);
            }
            if (avaible_num.Count == 0) return "error";
            return "img" + avaible_num[0].ToString();
        }
        ///////////////////////////////////////////////////////////////////////// 
        public static int CountImgs(int num_max, int tipo_metodo)
        {
            List<string> nomi = new List<string>();
            if (tipo_metodo == 1) foreach (string resource in Input.dir_tip) nomi.Add(TakeFileName(resource));
            else foreach (string resource in Input.dir_met) nomi.Add(TakeFileName(resource));

            List<int> avaible_num = new List<int>();
            for (int i = 0; i < num_max; i++) avaible_num.Add(i);
            List<int> num = new List<int>();
            foreach (string nom in nomi) try { num.Add(Convert.ToInt16(nom.Substring(3))); } catch (Exception) { }
            for (int i = 0; i < num_max; i++)
            {
                if (num.Contains(i)) avaible_num.Remove(i);
            }
            return num_max - avaible_num.Count;
        }
        ///////////////////////////////////////////////////////////////////////// 
        public static bool GetTrueFalse(string[] readText, string segnoinizio)
        {
            bool booleana = false;
            //string[] readText = File.ReadAllLines(path);
            //readText = Decrypt(readText);
            foreach (string line in readText)
            {
                if (line.Contains(segnoinizio))
                {
                    booleana = Convert.ToBoolean(line.Substring(segnoinizio.Length, line.Length - segnoinizio.Length - 1));
                }
            }
            return booleana;
        }
        /////////////////////////////////////////////////////////////////////////  Caricamento tipi (System, System.IO)
        public static List<string> GetTipi(string[] readText, string segnoinizio)
        {
            string stringa = "";
            int prosegui = 0, j = 0;
            List<string> tipi = new List<string>();
            //string[] readText = File.ReadAllLines(path);
            //readText = Decrypt(readText);
            foreach (string line in readText)
            {
                if (prosegui == 1) break;
                if (line.Contains(segnoinizio))
                {
                    for (int i = (segnoinizio.Length); i < line.Length; i++)
                    {
                        if (line.Substring(i, 1) == ";") { tipi.Add(stringa); j++; stringa = ""; }
                        else { stringa += line.Substring(i, 1); }
                    }
                    prosegui = 1;
                }
            }
            return tipi;
        }
        /////////////////////////////////////////////////////////////////////////  Caricamento tipi (System, System.IO)
        public static List<string> GetAttributi(string[] readText, string segnoinizio, string stacco)
        {
            string stringa = "";
            int prosegui = 0, j = 0;
            List<string> attributi = new List<string>();
            //string[] readText = File.ReadAllLines(path);
            //readText = Decrypt(readText);
            foreach (string line in readText)
            {
                if (prosegui == 1) break;
                if (line.Contains(segnoinizio))
                {
                    for (int i = (segnoinizio.Length); i < line.Length; i++)
                    {
                        if (line.Substring(i, stacco.Length) == stacco) { attributi.Add(stringa); j++; stringa = ""; i += stacco.Length - 1; }
                        else { stringa += line.Substring(i, 1); }
                    }
                    prosegui = 1;
                }
            }
            return attributi;
        }
        /////////////////////////////////////////////////////////////////////////  Caricamento tot iniziali (System, System.IO)
        public static List<double> GetTotali_Iniziali(string[] readText, string segnoinizio)
        {
            string stringa = "";
            int prosegui = 0, j = 0;
            List<double> tipi = new List<double>();
            //string[] readText = File.ReadAllLines(path);
            //readText = Decrypt(readText);
            foreach (string line in readText)
            {
                if (prosegui == 1) break;
                if (line.Contains(segnoinizio))
                {
                    for (int i = (segnoinizio.Length); i < line.Length; i++)
                    {
                        if (line.Substring(i, 1) == ";") { tipi.Add(Convert.ToDouble(stringa)); j++; stringa = ""; }
                        else { stringa += line.Substring(i, 1); }
                    }
                    prosegui = 1;
                }
            }
            return tipi;
        }
        ///////////////////////////////////////////////////////////////////////// 
        public static List<int> Get_Sort(string[] readText, string segnoinizio)
        {
            string stringa = "";
            int prosegui = 0, j = 0;
            List<int> tipi = new List<int>();
            //string[] readText = File.ReadAllLines(path);
            //readText = Decrypt(readText);
            foreach (string line in readText)
            {
                if (prosegui == 1) break;
                if (line.Contains(segnoinizio))
                {
                    for (int i = (segnoinizio.Length); i < line.Length; i++)
                    {
                        if (line.Substring(i, 1) == ";") { tipi.Add(Convert.ToInt32(stringa)); j++; stringa = ""; }
                        else { stringa += line.Substring(i, 1); }
                    }
                    prosegui = 1;
                }
            }
            return tipi;
        }
        /// ////////////////////////////////////////////////////////////////
        public static double SetCentesimi(string centesimi)
        {
            //Console.WriteLine(centesimi);
            centesimi = centesimi.Replace(" ", "");
            if (centesimi == "") return 0;
            int num_zeri_iniziali = 0;
            for (int i = 0; i < centesimi.Length; i++) if (centesimi.Substring(i, 1) == "0") num_zeri_iniziali++; else break;
            int cent_num = Convert.ToInt32(centesimi);
            if (cent_num == 0) return 0;
            double result = 0;
            if (num_zeri_iniziali == 0) result = (double)cent_num / Math.Pow(10,centesimi.Length);
            if (num_zeri_iniziali == 1) result = (double)cent_num / 100;
            if (result.ToString().Length > 4) return Convert.ToDouble(result.ToString().Substring(0, 4));
            else return result;
        }

        /// ////////////////////////////////////////////////////////////////
        public static string GetCentesimiString(double valore)
        {
            if (!valore.ToString().Contains(",")) return "0";
            string valore_txt = valore.ToString().Substring(valore.ToString().IndexOf(",") + 1).Replace(" ", "");
            if (valore_txt.Length == 1) valore_txt += "0";
            return valore_txt;
        }

        /// ////////////////////////////////////////////////////////////////
        public static int GetCentesimi(double valore)
        {
            int centesimi = 0, i = 0;
            string valore_txt = valore.ToString();
            if (valore_txt.Contains(","))
            {
                for (i = 0; i < valore_txt.Length; i++)
                {
                    if (valore_txt.Substring(i, 1) == ",") break;
                }
                for (int m = i + 1; m < valore_txt.Length; m++)
                {
                    if (m - i == 1) centesimi += 10 * Convert.ToInt32(valore_txt.Substring(m, 1));
                    if (m - i == 2) centesimi += Convert.ToInt32(valore_txt.Substring(m, 1));
                }
            }
            return centesimi;
        }

        /// ////////////////////////////////////////////////////////////////
        public static List<string> GetUpdates(string[] readText, string segnoinizio)
        {
            List<string> updates = new List<string>();

            foreach (string line in readText)
            {
                if (line.Length >= segnoinizio.Length)
                {
                    if (line.Substring(0, 6) == segnoinizio) updates.Add(line);
                }
            }
            return updates;
        }
        /// ////////////////////////////////////////////////////////////////
        public static string GetTipo(string tipometodo)
        {
            string stringa = "";
            if (tipometodo == "" || tipometodo == null) return "";
            if (tipometodo.Contains("\u2192"))
            {
                for (int i = 0; i < tipometodo.Length; i++)
                {
                    if (tipometodo.Substring(i, 1) == "\u2192") { break; }
                    else { stringa += tipometodo.Substring(i, 1); }
                }
            }
            else stringa = tipometodo;
            return stringa;
        }

        /// ////////////////////////////////////////////////////////////////
        public static string GetMetodo(string tipometodo)
        {
            string stringa = "";
            bool segnale = false;
            if (tipometodo == "" || tipometodo == null) return "";
            if (tipometodo.Contains("\u2192"))
            {
                for (int i = 0; i < tipometodo.Length; i++)
                {
                    if (tipometodo.Substring(i, 1) == "\u2192") { i++; segnale = true; }
                    if (segnale) stringa += tipometodo.Substring(i, 1);
                }
            }
            else stringa = tipometodo;
            return stringa;
        }

        /////////////////////////////////////////////////////////////////////////  Caricamento tipi (System, System.IO)
        public static int[] GetFestivi(string[] readText, string segnoinizio)
        {
            string stringa = "";
            int prosegui = 0, j = 0;
            //string[] tipi = new string[Convert.ToInt32(GetAttributo(path, "Colonne_tabella: ", ";"))];
            //string[] readText = File.ReadAllLines(path, Encoding.GetEncoding("iso-8859-1"));
            int[] giornifestivi = new int[7];
            foreach (string line in readText)
            {
                if (prosegui == 1) break;
                if (line.Contains(segnoinizio))
                {
                    for (int i = (segnoinizio.Length); ; i++)
                    {
                        try
                        {
                            if (line.Substring(i, 1) == ";") { giornifestivi[j] = Convert.ToInt32(stringa); j++; stringa = ""; }
                            else { stringa += line.Substring(i, 1); }
                        }
                        catch (Exception) { prosegui = 1; break; }
                    }
                }
            }
            return giornifestivi;
        }
        /////////////////////////////////////////////////////////////////////////  Caricamento proprietà matrice (System, System.IO)
        /*public static char[,] GetpropMatrice(string path, string segnoinizio)
        {
            int i = 0, m = 0, segnale = 0;
            int righe = Convert.ToInt32(GetAttributo(path, "Righe_tabella: ", ";")), colonne = Convert.ToInt32(GetAttributo(path, "Colonne_tabella: ", ";"));
            char[,] proprietà = new char[righe, colonne];
            for (i = 0; i < righe; i++)
            {
                for (int j = 0; j < colonne; j++)
                {
                    proprietà[i, j] = 'z';
                }
            }
            i = 0;
            foreach (var line in File.ReadAllLines(path))
            {
                if (segnale == 1)
                {
                    for (int j = 0; ; j++)
                    {
                        if (m == colonne) { break; }
                        if (line.Substring(j, 1) == ";") { m++; }
                        else if (Char.IsLetter(Convert.ToChar(line.Substring(j, 1)))) { proprietà[i, m] = Convert.ToChar(line.Substring(j, 1)); }
                    }

                    m = 0;
                }
                if (line.Contains(segnoinizio)) { segnale = 1; i = -1; }
                i++;
                if (i == righe) { break; }
            }
            return proprietà;
        }*/



        /////////////////////////////////////////////////////////////////////////  Caricamento matrice (System, System.IO)
        /*public static double[,] GetMatrice(string path, string segnoinizio)
        {
            int i = 0, m = 0, segnale = 0;
            string stringa = "";
            int righe = Convert.ToInt32(GetAttributo(path, "Righe_tabella: ", ";")), colonne = Convert.ToInt32(GetAttributo(path, "Colonne_tabella: ", ";"));
            double[,] valori = new double[righe, colonne];
            foreach (var line in File.ReadAllLines(path))
            {
                if (segnale == 1)
                {
                    for (int j = 0; ; j++)
                    {
                        if (m == colonne) { break; }
                        if (line.Substring(j, 1) == ";") { valori[i, m] = Convert.ToDouble(stringa); m++; stringa = ""; }
                        else if (Char.IsLetter(Convert.ToChar(line.Substring(j, 1)))) {; }
                        else { stringa += line.Substring(j, 1); }
                    }

                    m = 0;
                }
                if (line.Contains(segnoinizio)) { segnale = 1; i = -1; }
                i++;
                if (i == righe) { break; }
            }
            return valori;
        }*/
        /////////////////////////////////////////////////////////////////////////  Salvataggio (System, System.IO)
        public static void Save(string path, double[,] valori, char[,] proprietà, string[] tipi)
        {
            int righe = valori.GetUpperBound(0) + 1, colonne = valori.GetUpperBound(1) + 1;
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.Write("Tipi: ");
                for (int i = 0; i < colonne; i++)
                {
                    sw.Write(tipi[i]);
                    sw.Write(";");
                }
                sw.WriteLine(""); sw.WriteLine("");
                sw.Write("Righe_tabella: "); sw.Write(righe); sw.WriteLine(";");
                sw.Write("Colonne_tabella: "); sw.Write(colonne); sw.WriteLine(";\n");
                sw.WriteLine("");
                sw.WriteLine("Tabella:");
                for (int i = 0; i < righe; i++)
                {
                    for (int j = 0; j < colonne; j++)
                    {
                        sw.Write(valori[i, j]);
                        sw.Write(proprietà[i, j]);
                        sw.Write(";");
                    }
                    sw.WriteLine("");
                }
            }
        }
        /////////////////////////////////////////////////////////////////////////  Prelievo di attributo da una parola(System, System.IO)
        public static string GetAttributo(string[] readText, string parola, string segnofine)
        {
            int numline = 0, posizione = 0, a = 0;
            string attributo = "";
            //string[] readText = File.ReadAllLines(path);
            //readText = Decrypt(readText);
            foreach (var line in readText)
            {
                if (line.Contains(parola))
                {
                    posizione = line.IndexOf(parola) + parola.Length;
                    for (int i = 0; i < (line.Length - posizione); i++)
                    {
                        if (line.Substring(posizione + i, 1) == segnofine) break;
                        attributo += line.Substring(posizione + i, 1);
                    }
                    a++;
                }

                numline++;
            }
            if (a == 0) return "";
            if (a > 1) return "";
            return attributo;
        }
        /////////////////////////////////////////////////////////////////////////  Creazione Cartella (System, System.IO)
        public static void CreazioneCartella(string path)
        {
            if (!Directory.Exists(Input.path.Substring(0, Input.path.Length - 1))) Directory.CreateDirectory(Input.path.Substring(0, Input.path.Length - 1));
            //if(!File.Exists(Input.path+"Settings2.txt")) using (StreamWriter sw = File.CreateText(Input.path + "Settings2.txt")) sw.Write("Always_offline:False");
            try
            {
                if (Directory.Exists(path + @"\Backups") == false) Directory.CreateDirectory(path + "/Backups/");
                if (Directory.Exists(path + @"\Icons") == false) { Directory.CreateDirectory(path + "/Icons"); Directory.CreateDirectory(path + @"\Icons\Tipologie"); Directory.CreateDirectory(path + @"\Icons\Metodi"); }
                if (Directory.Exists(path + @"\Icons\Tipologie") == false) Directory.CreateDirectory(path + @"\Icons\Tipologie");
                if (Directory.Exists(path + @"\Icons\Metodi") == false) Directory.CreateDirectory(path + @"\Icons\Metodi");
                if (Directory.Exists(path + @"\Data") == false) Directory.CreateDirectory(path + @"\Data");
                if (File.Exists(path + @"\" + Input.filename + ".txt"))
                {
                    return;
                }
                else
                {
                    foreach (string file in Directory.EnumerateFiles(path, "*.txt"))
                    {
                        try
                        {
                            if (TakeFileName(file).Substring(0, 4) == "Data")
                            {
                                File.Move(file, path + @"\" + Input.filename + ".txt");
                                return;
                            }
                        }
                        catch (Exception) { Console.WriteLine("Errore Spostamento cartella"); }
                    }
                    string resource_data = Moneyguard.Properties.Resources.Data;

                    //System.IO.File.WriteAllBytes(Input.path + "credentials.json", Moneyguard.Properties.Resources.Data);

                    using (StreamWriter sw = File.CreateText(path + @"\" + Input.filename + ".txt"))
                    {
                        
                        sw.Write(resource_data);
                    }
                //Funzioni_utili.Encrypt(Input.path + "null.txt", Input.path_in);
                    System.IO.File.SetCreationTime(path + @"\" + Input.filename + ".txt", new DateTime(1970,1,1));
                    //System.Threading.Thread.Sleep(50);
                    File.Delete(Input.path + "null.txt");
                    return;
                }
            }
            catch (Exception)
            {
                //Console.WriteLine("Errore Spostamento cartella 2");
                string resource_data = Moneyguard.Properties.Resources.Data;
                //File.Copy(Moneyguard.Properties.Resources.Data, Input.path_in);
                using (StreamWriter sw = File.CreateText(path + @"\" + Input.filename + ".txt"))
                {
                    sw.Write(resource_data);
                }
                //Funzioni_utili.Encrypt(Input.path + "null.txt", Input.path_in);
                System.IO.File.SetCreationTime(path + @"\" + Input.filename + ".txt", new DateTime(1970, 1, 1));
                try
                {
                    Directory.CreateDirectory(path);
                    Directory.CreateDirectory(path + "/Icons/");
                    Directory.CreateDirectory(path + "/Backups/");
                    Directory.CreateDirectory(path + "/Icons/Tipologie/");
                    Directory.CreateDirectory(path + "/Icons/Metodi/");
                    Directory.CreateDirectory(path + @"\Data");
                    //using (StreamWriter sw = File.CreateText(path + @"\" + Input.filename + ".txt"))
                    {
                        //sw.Write(resource_data);
                    }
                }
                catch (Exception)
                {
                    File.Delete(Input.path + "null.txt");
                    return;
                }
                File.Delete(Input.path + "null.txt");

            }
        }
        ////////////////////////////////////////////////////////////////////////  Take Picture (System, System.IO)
        public static Image TakePicture2(string tipo, int tipo_metodo)
        {
            Image img;
            int index = 0;
            for (int i = 0; i < Input.tipi.Count; i++) if (Input.tipi[i] == tipo) { index = i; tipo_metodo = 1; break; }
            for (int i = 0; i < Input.metodi.Count; i++) if (Input.metodi[i] == tipo) { index = i; tipo_metodo = 2; break; }
            bool intelligentemente = true;

            if (tipo_metodo == 1)
            {

                foreach (string key in Input.resources)
                {
                    if (key == Associazione.IconaAssociata(tipo)) return new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject(Associazione.IconaAssociata(tipo))));
                }
                foreach (string filename in Input.dir_tip)
                {
                    if (TakeFileName(filename) == Associazione.IconaAssociata(tipo))
                    {
                        if (intelligentemente)
                        {
                            StreamReader streamReader = new StreamReader(Input.path + @"Icons\Tipologie\" + Associazione.IconaAssociata(tipo) + ".png");
                            Bitmap tmpBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
                            streamReader.Close();
                            return tmpBitmap;
                            using (var bmpTemp = Image.FromFile(Input.path + @"Icons\Tipologie\" + Associazione.IconaAssociata(tipo) + ".png")) { img = bmpTemp; }
                            Console.WriteLine(Input.path + @"Icons\Tipologie\" + Associazione.IconaAssociata(tipo) + ".png");
                            Console.WriteLine(img);
                            return img;
                        }
                        else return (Bitmap)Image.FromFile(Input.path + @"Icons\Tipologie\" + Associazione.IconaAssociata(tipo) + ".png");
                    }
                }
                return new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("no_icon")));
            }
            else
            {
                foreach (string key in Input.resources)
                {
                    if (key == Associazione.MiconaAssociata(tipo)) return new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject(Associazione.MiconaAssociata(tipo))));
                }
                foreach (string filename in Input.dir_met)
                {
                    if (TakeFileName(filename) == Associazione.MiconaAssociata(tipo))
                    {
                        if (intelligentemente)
                        {
                            StreamReader streamReader = new StreamReader(Input.path + @"Icons\Metodi\" + Associazione.MiconaAssociata(tipo) + ".png");
                            Bitmap tmpBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
                            streamReader.Close();
                            return tmpBitmap;
                            using (var bmpTemp = Image.FromFile(Input.path + @"Icons\Metodi\" + Associazione.MiconaAssociata(tipo) + ".png")) { img = bmpTemp; }
                            Console.WriteLine(Input.path + @"Icons\Metodi\" + Associazione.MiconaAssociata(tipo) + ".png");
                            return img;
                        }
                        else return (Bitmap)Image.FromFile(Input.path + @"Icons\Metodi\" + Associazione.MiconaAssociata(tipo) + ".png");
                    }
                }
                return new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("no_icon")));
            }
        }
        /// ////////////////////////////////////////////////////////////////////
        public static Bitmap TakePicture(string tipo, int tipo_metodo)
        {
            Bitmap img;
            int index = 0;
            for (int i = 0; i < Input.tipi.Count; i++) if (Input.tipi[i] == tipo) { index = i; tipo_metodo = 1; break; }
            for (int i = 0; i < Input.metodi.Count; i++) if (Input.metodi[i] == tipo) { index = i; tipo_metodo = 2; break; }
            bool intelligentemente = true;
            
            if (tipo_metodo == 1)
            {
                
                foreach (string key in Input.resources)
                {
                    if(key == Associazione.IconaAssociata(tipo)) return new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject(Associazione.IconaAssociata(tipo))));
                }
                foreach (string filename in Input.dir_tip)
                {
                    if (TakeFileName(filename) == Associazione.IconaAssociata(tipo))
                    {
                        if (intelligentemente)
                        {
                            StreamReader streamReader = new StreamReader(Input.path + @"Icons\Tipologie\" + Associazione.IconaAssociata(tipo) + ".png");
                            Bitmap tmpBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
                            streamReader.Close();
                            return tmpBitmap;
                            using (var bmpTemp = new Bitmap(Input.path + @"Icons\Tipologie\" + Associazione.IconaAssociata(tipo) + ".png")) { img = bmpTemp; }
                            Console.WriteLine(Input.path + @"Icons\Tipologie\" + Associazione.IconaAssociata(tipo) + ".png");
                            Console.WriteLine(img);
                            return img;
                        }
                        else return (Bitmap)Image.FromFile(Input.path + @"Icons\Tipologie\" + Associazione.IconaAssociata(tipo) + ".png");
                    }
                }
                return new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("no_icon")));
            }
            else
            {
                foreach (string key in Input.resources)
                {
                    if (key == Associazione.MiconaAssociata(tipo)) return new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject(Associazione.MiconaAssociata(tipo))));
                }
                foreach (string filename in Input.dir_met)
                {
                    if (TakeFileName(filename) == Associazione.MiconaAssociata(tipo))
                    {
                        if (intelligentemente)
                        {
                            StreamReader streamReader = new StreamReader(Input.path + @"Icons\Metodi\" + Associazione.MiconaAssociata(tipo) + ".png");
                            Bitmap tmpBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
                            streamReader.Close();
                            return tmpBitmap;
                            using (var bmpTemp = new Bitmap(Input.path + @"Icons\Metodi\" + Associazione.MiconaAssociata(tipo) + ".png")) { img = bmpTemp; }
                            Console.WriteLine(Input.path + @"Icons\Metodi\" + Associazione.MiconaAssociata(tipo) + ".png");
                            return img;
                        }
                        else return (Bitmap)Image.FromFile(Input.path + @"Icons\Metodi\" + Associazione.MiconaAssociata(tipo) + ".png");
                    }
                }
                return new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("no_icon")));
            }
        }

        ////////////////////////////////////////////////////////////////////////  Take File Name (System, System.IO)
        public static string TakeFileName(string filename)
        {
            string file="";
            for (int i = 0; ; i++)
            {
                if (filename.Substring(filename.Length - i - 1, 1) == @"\") { file = filename.Substring(filename.Length - i, i - 4); break; }
            }
            return file;
        }
        ////////////////////////////////////////////////////////////////////////  Distruzione Cartella (System, System.IO)
        public static void DistruzioneCartella(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                    return;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Errore Distruzione cartella");
                return;
            }
        }
        ////////////////////////////////////////////////////////////////////////  Lettura File text (System, System.IO)	pezzotto
        public void LetturaText(string path)
        {
            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    //Console.WriteLine(s);
                }
            }
        }
        /////////////////////////////////////////////////////////////////////////  Creazione File text (System, System.IO)
        public void CreazioneText(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine("Hello");
                sw.WriteLine("And");
                sw.WriteLine("Welcome");
            }
        }
        ////////////////////////////////////////////////////////////////////////  Visualizzazione Cifra euro
        public static string FormatoStandard(double numero)
        {
            bool minus = false; string numero_str = "";
            if (numero < 0) { numero = -numero; minus = true; }
            int num_cifre = ((int)numero).ToString().Length;
            if (numero >= 1000)
            {
                numero_str = Convert.ToString((int)numero);
                if (num_cifre > 3) numero_str = numero_str.Insert(num_cifre - 3, " ");
                if (num_cifre > 6) numero_str = numero_str.Insert(num_cifre - 6, " ");
                if (num_cifre > 9) numero_str = numero_str.Insert(num_cifre - 9, " ");
                if (minus) return "-" + numero_str;
                else  return numero_str;
            }
            if (numero < 0.01)  return "0,00"; 
            numero_str = Convert.ToString(numero);
            if (numero_str.IndexOf(',') == -1) if (minus)
                {
                    return "-" + numero_str;
                }
                else
                {
                    return numero_str;
                }
            else if (numero_str.Length - numero_str.IndexOf(',') == 2)
            {
                numero_str += "0";
            }

            else if (numero_str.Length - numero_str.IndexOf(',') > 3)
            {
                numero_str = numero_str.Substring(0, numero_str.IndexOf(',') + 3);
            }
            if (numero_str.Contains(",")) { } else numero_str += ",00";
            if (minus) return "-" + numero_str; else return numero_str;
        }
        ////////////////////////////////////////////////////////////////////////
        public static double DenaroTotale(double[,] valori, int numero_tipiguadagno, int numero_tipispeciali)
        {
            double denaro_totale = 0;
            for (int i = 0; i < valori.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < numero_tipiguadagno; j++)
                {
                    denaro_totale += valori[i, j];
                }
                for (int j = numero_tipiguadagno + numero_tipispeciali; j < valori.GetUpperBound(1) + 1; j++)
                {
                    denaro_totale -= valori[i, j];
                }
            }
            return denaro_totale;
        }

        ////////////////////////////////////////////////////////////////////////
        public static string CodData(string data)
        {
            string anno = data.Substring(19, 4);
            string mese = data.Substring(16, 2);
            string giorno = data.Substring(13, 2);
            data = "Data"+anno + mese + giorno;
            return data;
        }
        ////////////////////////////////////////////////////////////////////////
        public static string DecodData(string data)
        {
            string anno = data.Substring(4, 4);
            string mese = data.Substring(8, 2);
            string giorno = data.Substring(10, 2);
            data = "Database  \u2192  " + giorno + "/" + mese + "/" + anno;
            return data;
        }

    }
}
