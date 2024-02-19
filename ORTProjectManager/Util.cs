using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORTProjectManager
{
    public class Util
    {
        public static Dictionary<string, string> LoadFromFile(string path) 
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            try
            {
                StreamReader sr = new StreamReader(path);
                string line;
                do
                {
                    line = sr.ReadLine();
                }
                while (line != "PREFERENCES:" && !sr.EndOfStream);
                while (!sr.EndOfStream)
                {
                    res.Add(sr.ReadLine(), sr.ReadLine());
                }
                sr.Dispose();
            }
            catch 
            {
            
            }
            return res;
        }

        public static string Filename(string path) 
        {
            string fname = path.Substring(path.LastIndexOf('\\') + 1);
            fname = fname.Substring(0, fname.LastIndexOf('.'));
            return fname;
        }

        public static string RelativePath(string relativeto, string path) 
        {
            Uri uri1 = new Uri(path);
            Uri uri2 = new Uri(relativeto.Substring(0, relativeto.LastIndexOf('\\') + 1));
            return uri2.MakeRelativeUri(uri1).ToString();
        }

        public static string AbsolutePath(string relativeto, string path)
        {
            Uri uri1 = new Uri(relativeto.Substring(0, relativeto.LastIndexOf('\\') + 1));
            Uri uri2 = new Uri(uri1,path);
            return uri2.LocalPath;
        }

        public static void Log(string t) 
        {
            if (logBox != null) 
            {
                logBox.Items.Add(t);
            }
        }

        public static ListBox logBox=null;

        public static int nobits(int nr) 
        {
            int nobits = 1;
            int max = 2;
            while (max < nr) 
            {
                nobits++;
                max *= 2;
            }
            return nobits;
        }
    }
}
