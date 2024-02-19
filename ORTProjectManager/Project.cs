using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORTProjectManager
{
    public class Project
    {
        public string savepath = null;
        public List<Microcode> Microcodes = new List<Microcode>();

        public static Project LoadFromFile(string path)
        {
            Project ret = new Project();
            ret.savepath = path;
            StreamReader sr = new StreamReader(path);

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (line.EndsWith(".mc"))
                {
                    line = Util.AbsolutePath(ret.savepath, line);
                    ret.Microcodes.Add(new Microcode(line));
                }
            }

            sr.Dispose();
            return ret;
        }

        public void SaveToFile()
        {
            StreamWriter sw = new StreamWriter(savepath);

            foreach (var item in Microcodes)
            {
                string rel = Util.RelativePath(savepath, item.savepath);
                sw.WriteLine(rel);
            }

            sw.Dispose();
        }

        public bool HasSchemas()
        {
            foreach (var item in Microcodes)
            {
                string check = Util.AbsolutePath(savepath, Util.Filename(item.savepath) + ".mcs");
                if (!(File.Exists(check)))
                {
                    return false;
                }
            }
            return true;
        }

        public bool HasMicroHex()
        {
            foreach (var item in Microcodes)
            {
                string check = Util.AbsolutePath(savepath, Util.Filename(item.savepath) + ".hex");
                if (!(File.Exists(check)))
                {
                    return false;
                }
            }
            return true;
        }

        public bool HasSegment()
        {
            if (savepath == null) { return false; }
            string check = Util.AbsolutePath(savepath, Util.Filename(savepath) + ".seg");
            return File.Exists(check);
        }

        public bool HasDiagram()
        {
            foreach (var item in Microcodes)
            {
                string check = Util.AbsolutePath(savepath, Util.Filename(item.savepath) + ".svg");
                if (!(File.Exists(check)))
                {
                    return false;
                }
            }
            return true;
        }

        public void GenerateSchemas()
        {
            foreach (var item in Microcodes)
            {
                item.GenerateSchema();
            }
        }

        public void Translate()
        {
            foreach (var item in Microcodes)
            {
                item.Translate();
            }
        }

        public void Segment()
        {
            Dictionary<string, int> signals = new Dictionary<string, int>();
            Dictionary<string, int> conditions = new Dictionary<string, int>();

            for (int i = 0; i < Microcodes.Count; i++)
            {
                Microcodes[i].LoadWithSchema();
                foreach (var item in Microcodes[i].schema.signals)
                {
                    if (signals.ContainsKey(item))
                    {
                        signals[item]++;
                    }
                    else 
                    {
                        signals.Add(item, 1);
                    }
                }
                foreach (var item in Microcodes[i].schema.conditions)
                {
                    if (conditions.ContainsKey(item))
                    {
                        conditions[item]++;
                    }
                    else
                    {
                        conditions.Add(item, 1);
                    }
                }
            }

            StreamWriter sw = new StreamWriter(Util.AbsolutePath(savepath, Util.Filename(savepath) + ".seg"));

            sw.WriteLine("//COMMON signals");
            foreach (var item in signals)
            {
                if (item.Value != 1)
                {
                    sw.WriteLine(item.Key);
                }
            }

            sw.WriteLine("//COMMON conditions");
            foreach (var item in conditions)
            {
                if (item.Value != 1)
                {
                    sw.WriteLine(item.Key);
                }
            }

            for (int i = 0; i < Microcodes.Count; i++)
            {
                sw.WriteLine("//" + Util.Filename(Microcodes[i].savepath) + " signals");
                foreach (var item in Microcodes[i].schema.signals)
                {
                    if (signals[item]==1)
                    {
                        sw.WriteLine(item);
                    }
                }
                sw.WriteLine("//" + Util.Filename(Microcodes[i].savepath) + " conditions");
                foreach (var item in Microcodes[i].schema.conditions)
                {
                    if (conditions[item] == 1)
                    {
                        sw.WriteLine(item);
                    }
                }
            }

            sw.Dispose();

            Util.Log("Segmented " + Util.Filename(savepath));
        }

        public void Diagram()
        {
            foreach (var item in Microcodes)
            {
                item.Diagram();
            }
        }

        public void Unload()
        {
            foreach (var item in Microcodes)
            {
                item.loaded = false;
            }
        }
    }
}
