using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication1;

namespace Moneyguard
{
    public class MyMetadata
    {
        public string filename;
        public string path;
        public DateTime data;
        public MyMetadata(string path, DateTime data)
        {
            this.path = path;
            this.filename = Path.GetFileNameWithoutExtension(path).Substring(4);
            this.data = data;
        }
        public MyMetadata(string line)
        {
            this.filename = line.Substring(0,6);
            this.data = Date.GetStandardData(line.Substring(6));
        }

        public static void CreateInfo()
        {
            List<MyMetadata> list_metadata = new List<MyMetadata>();
            string[] get_files = Directory.GetFiles(Input.path + @"Data");
            foreach (string stringa in get_files)
            {
                if (Path.GetFileNameWithoutExtension(stringa) == "Info") continue;
                list_metadata.Add(new MyMetadata(stringa, File.GetCreationTimeUtc(stringa)));
            }

            File.Delete(Input.path + @"Data\Info.txt");
            using (StreamWriter sw = new StreamWriter(Input.path + @"Data\Info.txt"))
            {
                foreach (MyMetadata metadata in list_metadata)
                {
                    sw.WriteLine(metadata.filename + Date.ShowStandardData(metadata.data));
                }
            }
            File.SetCreationTimeUtc(Input.path + @"Data\Info.txt", DateTime.Now);
        }
        public static List<MyMetadata> ReadMetadata_byFile(string file_path)
        {
            List<MyMetadata> lista = new List<MyMetadata>();
            string text;
            string[] lines;
            try
            {
                using (StreamReader sr = new StreamReader(file_path))
                {
                    text = sr.ReadToEnd();
                }
                lines = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    if (line.Length > 1)
                    {
                        lista.Add(new MyMetadata(line));
                    }
                }
                return lista;
            }
            catch (Exception) { return lista; }
        }

        public static List<MyMetadata> MetadataToUpdate(string path_local, string path_temp, bool fromstoragetolocal)
        {
            List<MyMetadata> output = new List<MyMetadata>();
            try
            {
                List<MyMetadata> list_local = ReadMetadata_byFile(path_local);
                List<MyMetadata> list_storage = ReadMetadata_byFile(path_temp);
                if (fromstoragetolocal)
                {
                    for (int j = 0; j < list_storage.Count; j++) 
                    {
                        bool trovato = false;
                        for (int i = 0; i < list_local.Count; i++)
                        {
                            if (list_local[i].filename == list_storage[j].filename) { trovato = true; if (list_local[i].data < list_storage[j].data) output.Add(list_storage[j]); }
                        }
                        if (!trovato) output.Add(list_storage[j]);
                    }
                }
                else
                {
                    for (int i = 0; i < list_local.Count; i++)
                    {
                        bool trovato = false;
                        for (int j = 0; j < list_storage.Count; j++)
                        {
                            if (list_local[i].filename == list_storage[j].filename) { trovato = true; if (list_local[i].data > list_storage[j].data) output.Add(list_local[i]); }
                        }
                        if (!trovato) output.Add(list_local[i]);
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); return null; }

            return output;
        }
        public static List<MyMetadata> MetadataToRemove(string path_local, string path_temp, bool fromstoragetolocal)
        {
            List<MyMetadata> output = new List<MyMetadata>();
            try
            {
                List<MyMetadata> list_local = ReadMetadata_byFile(path_local);
                List<MyMetadata> list_storage = ReadMetadata_byFile(path_temp);
                if (fromstoragetolocal)
                {

                    for (int i = 0; i < list_local.Count; i++) 
                    {
                        bool trovato = false;
                        for (int j = 0; j < list_storage.Count; j++)
                        {
                            if (list_storage[j].filename == list_local[i].filename) trovato = true;
                        }
                        if (!trovato) output.Add(list_local[i]);
                    }
                }
                else
                {
                    for (int j = 0; j < list_storage.Count; j++)
                    {
                        bool trovato = false;
                        for (int i = 0; i < list_local.Count; i++)
                        {
                            if (list_storage[j].filename == list_local[i].filename) trovato = true;
                        }
                        if (!trovato) output.Add(list_storage[j]);
                    }
                }
            }catch(Exception e) { Console.WriteLine(e.Message); return null; }


            return output;
        }
    }
}
