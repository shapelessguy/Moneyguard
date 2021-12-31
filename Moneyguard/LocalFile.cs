using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyguard
{
    public class LocalFile
    {
        public string Name;
        public string path;
        public DateTime LastModifiedTime;
        public bool valid = true;
        public LocalFile(string path)
        {
            LastModifiedTime = File.GetLastWriteTime(path);
            Name = Path.GetFileName(path);
            this.path = path;
        }
    }
}
