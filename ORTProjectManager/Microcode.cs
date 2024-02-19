using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORTProjectManager
{
    public class Microcode
    {
        public Microcode(string s)
        {
            savepath = s;
        }

        public void GenerateSchema()
        {
            if (!loaded)
            {
                Load();
                loaded = true;
            }
            schema = Schema.Generate(microinstructions);
            schema.SaveToFile(Util.AbsolutePath(savepath, Util.Filename(savepath) + ".mcs"));
            Util.Log("Generated schema for " + Util.Filename(savepath));
        }

        public void Translate()
        {
            try
            {
                LoadWithSchema();

                StreamWriter sw = new StreamWriter(Util.AbsolutePath(savepath, Util.Filename(savepath) + ".hex"));

                sw.WriteLine("v2.0 raw");

                for (int i = 0; i < microinstructions.Count; i++)
                {
                    sw.WriteLine(TranslateMI(microinstructions[i]));
                }

                sw.Dispose();
                Util.Log("Translated " + Util.Filename(savepath));
            }
            catch (BadShemaException)
            {
                Util.Log("Greska u shemi, molim regenerisite sheme");
            }
        }

        public void Diagram()
        {
            LoadWithSchema();

            SvgDocument svgDoc = new SvgDocument
            {
                Width = 1200,
                Height = 100 * schema.maxstep,
                ViewBox = new SvgViewBox(0, 0, 1200, 100 * schema.maxstep),
            };

            var group = new SvgGroup();
            svgDoc.Children.Add(group);

            group.Children.Add(new SvgLine()
            {
                StartX = 400,
                EndX = 400,
                StartY = 0,
                EndY = 100 * schema.maxstep,
                //StrokeDashArray = new SvgUnitCollection{ 4 },
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 1
            });
            group.Children.Add(new SvgLine()
            {
                StartX = 800,
                EndX = 800,
                StartY = 0,
                EndY = 100 * schema.maxstep,
                //StrokeDashArray = new SvgUnitCollection{ 4 },
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 1
            });

            int ystart = 50;

            for (int i = 0; i < microinstructions.Count; i++)
            {
                DiagramStep(microinstructions[i], group, ref ystart);
            }

            FileStream sw = new FileStream(Util.AbsolutePath(savepath, Util.Filename(savepath) + ".svg"), FileMode.Create);
            svgDoc.Write(sw);
            sw.Dispose();
            Util.Log("Generated Diagram " + Util.Filename(savepath));
        }

        private void DiagramStep(Microinstruction mi, SvgGroup group, ref int sy)
        {
            //TODO DRAW CASE
            if (mi.label != null)
            {
                drawRect(group, 270, sy, 50, 25);
                drawText(group, 295, sy + 16.84f, mi.label);
                drawRect(group, 670, sy, 50, 25);
                drawText(group, 695, sy + 16.84f, mi.label);
                drawArrowLeft(group, 270, sy + 12.5f, 250);
                drawArrowLeft(group, 670, sy + 12.5f, 650);
            }
            drawRect(group, 50, sy, 200, 35);
            drawText(group, 150, sy + 21.84f, mi.getDisplayText());
            drawRect(group, 450, sy, 200, 35);
            drawText(group, 550, sy + 21.84f, mi.getSignals());
            drawArrowDown(group, 150, sy + 35, sy + 50);
            drawArrowDown(group, 550, sy + 35, sy + 50);
            drawTextnc(group, 810, sy + 20, mi.ToText());
            sy += 50;
            if (mi.condition != null)
            {
                drawBranch(group, 50, sy);
                drawText(group, 150, sy + 19.34f, mi.condition);
                drawBranch(group, 450, sy);
                drawText(group, 550, sy + 19.34f, mi.condition);
                drawArrowDown(group, 150, sy + 30, sy + 45);
                drawArrowDown(group, 550, sy + 30, sy + 45);
                drawText(group, 245, sy + 12, "1");
                drawText(group, 142, sy + 42, "0");
                drawText(group, 645, sy + 12, "1");
                drawText(group, 542, sy + 42, "0");
                if (mi.destination != "this")
                {
                    drawArrowRight(group, 242, sy + 15, 270);
                    drawArrowRight(group, 642, sy + 15, 670);
                    drawRect(group, 270, sy + 2.5f, 50, 25);
                    drawText(group, 295, sy + 19.34f, mi.destination);
                    drawRect(group, 670, sy + 2.5f, 50, 25);
                    drawText(group, 695, sy + 19.34f, mi.destination);
                }
                else
                {
                    drawLine(group, 242, sy + 15, 270, sy + 15);
                    drawLine(group, 642, sy + 15, 670, sy + 15);
                    drawLine(group, 270, sy + 15, 270, sy - 20);
                    drawLine(group, 670, sy + 15, 670, sy - 20);
                    drawArrowLeft(group, 270, sy - 20, 250);
                    drawArrowLeft(group, 670, sy - 20, 650);
                }
                sy += 45;
            }
            else if (mi.destination != null)
            {
                drawRect(group, 125, sy, 50, 25);
                drawText(group, 150, sy + 16.84f, mi.destination);
                drawRect(group, 525, sy, 50, 25);
                drawText(group, 550, sy + 16.84f, mi.destination);
                sy += 40;
            }
            else if (mi.casei) 
            {
                drawLongBranch(group, 50, sy);
                drawText(group, 150, sy + 19.34f, "case " + mi.getCaseText());
                drawText(group, 150, sy + 39.34f, "then " + mi.getThenText());
                drawLongBranch(group, 450, sy);
                drawText(group, 550, sy + 19.34f,"case "+mi.getCaseText());
                drawText(group, 550, sy + 39.34f,"then "+mi.getThenText());
                sy += 65;
            }
            group.Children.Add(new SvgLine()
            {
                StartX = 0,
                EndX = 1200,
                StartY = sy-2,
                EndY = sy-2,
                StrokeDashArray = new SvgUnitCollection{ 4 },
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 1
            });
        }

        public void drawRect(SvgGroup doc, float sx, float sy, float w, float h)
        {
            doc.Children.Add(new SvgRectangle()
            {
                X = sx,
                Y = sy,
                Width = w,
                Height = h,
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 1,
                FillOpacity = 0
            });
        }

        public void drawBranch(SvgGroup doc, int sx, int sy)
        {
            doc.Children.Add(new SvgLine()
            {
                StartX = sx + 15,
                EndX = sx + 15 + 170,
                StartY = sy,
                EndY = sy,
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 1
            });
            doc.Children.Add(new SvgLine()
            {
                StartX = sx + 15,
                EndX = sx + 15 + 170,
                StartY = sy + 30,
                EndY = sy + 30,
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 1
            });
            doc.Children.Add(new SvgLine()
            {
                StartX = sx + 15,
                EndX = sx + 7,
                StartY = sy,
                EndY = sy + 15,
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 1
            });
            doc.Children.Add(new SvgLine()
            {
                StartX = sx + 15 + 170,
                EndX = sx + 7 + 15 + 170,
                StartY = sy,
                EndY = sy + 15,
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 1
            });
            doc.Children.Add(new SvgLine()
            {
                StartX = sx + 7,
                EndX = sx + 15,
                StartY = sy + 15,
                EndY = sy + 30,
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 1
            });
            doc.Children.Add(new SvgLine()
            {
                StartX = sx + 7 + 15 + 170,
                EndX = sx + 15 + 170,
                StartY = sy + 15,
                EndY = sy + 30,
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 1
            });
        }

        public void drawLongBranch(SvgGroup doc, int sx, int sy)
        {
            doc.Children.Add(new SvgLine()
            {
                StartX = sx,
                EndX = sx+30 + 170,
                StartY = sy,
                EndY = sy,
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 1
            });
            doc.Children.Add(new SvgLine()
            {
                StartX = sx,
                EndX = sx +30 + 170,
                StartY = sy + 50,
                EndY = sy + 50,
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 1
            });
            doc.Children.Add(new SvgLine()
            {
                StartX = sx,
                EndX = sx - 8,
                StartY = sy,
                EndY = sy + 25,
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 1
            });
            doc.Children.Add(new SvgLine()
            {
                StartX = sx + 30 + 170,
                EndX = sx + 8 + 30 + 170,
                StartY = sy,
                EndY = sy + 25,
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 1
            });
            doc.Children.Add(new SvgLine()
            {
                StartX = sx -8,
                EndX = sx,
                StartY = sy + 25,
                EndY = sy + 50,
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 1
            });
            doc.Children.Add(new SvgLine()
            {
                StartX = sx + 8 + 30 + 170,
                EndX = sx + 30 + 170,
                StartY = sy + 25,
                EndY = sy + 50,
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 1
            });
        }

        public void drawText(SvgGroup doc, float sx, float sy, string text)
        {
            doc.Children.Add(new SvgText()
            {
                Nodes = { new SvgContentNode { Content = text } },
                TextAnchor = SvgTextAnchor.Middle,
                X = { sx },
                Y = { sy },
            });
        }

        public void drawTextnc(SvgGroup doc, float sx, float sy, string text)
        {
            doc.Children.Add(new SvgText()
            {
                Nodes = { new SvgContentNode { Content = text } },
                X = { sx },
                Y = { sy },
            });
        }

        public void drawLine(SvgGroup doc, float sx, float sy, float ex, float ey)
        {
            doc.Children.Add(new SvgLine()
            {
                StartX = sx,
                EndX = ex,
                StartY = sy,
                EndY = ey,
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 1
            });
        }

        public void drawArrowDown(SvgGroup doc, float sx, float sy, float ey)
        {
            drawLine(doc, sx, sy, sx, ey - 5);
            doc.Children.Add(new SvgPolygon()
            {
                Points = new SvgPointCollection() { new SvgUnit((float)(sx - 2.5)), new SvgUnit((float)(ey - 5)), new SvgUnit((float)(sx + 2.5)), new SvgUnit((float)(ey - 5)), new SvgUnit((float)(sx)), new SvgUnit((float)(ey)) },
                Fill = new SvgColourServer(Color.Black)
            });

        }

        public void drawArrowup(SvgGroup doc, float sx, float sy, float ey)
        {
            drawLine(doc, sx, sy, sx, ey + 5);
            doc.Children.Add(new SvgPolygon()
            {
                Points = new SvgPointCollection() { new SvgUnit((float)(sx - 2.5)), new SvgUnit((float)(ey + 5)), new SvgUnit((float)(sx + 2.5)), new SvgUnit((float)(ey + 5)), new SvgUnit((float)(sx)), new SvgUnit((float)(ey)) },
                Fill = new SvgColourServer(Color.Black)
            });

        }

        public void drawArrowRight(SvgGroup doc, float sx, float sy, float ex)
        {
            drawLine(doc, sx, sy, ex - 5, sy);
            doc.Children.Add(new SvgPolygon()
            {
                Points = new SvgPointCollection() { new SvgUnit((float)(ex - 5)), new SvgUnit((float)(sy - 2.5)), new SvgUnit((float)(ex - 5)), new SvgUnit((float)(sy + 2.5)), new SvgUnit((float)(ex)), new SvgUnit((float)(sy)) },
                Fill = new SvgColourServer(Color.Black)
            });
        }

        public void drawArrowLeft(SvgGroup doc, float sx, float sy, float ex)
        {
            drawLine(doc, sx, sy, ex + 5, sy);
            doc.Children.Add(new SvgPolygon()
            {
                Points = new SvgPointCollection() { new SvgUnit((float)(ex + 5)), new SvgUnit((float)(sy - 2.5)), new SvgUnit((float)(ex + 5)), new SvgUnit((float)(sy + 2.5)), new SvgUnit((float)(ex)), new SvgUnit((float)(sy)) },
                Fill = new SvgColourServer(Color.Black)
            });
        }

        public void LoadWithSchema()
        {
            if (!loaded)
            {
                Load();
                if (File.Exists(Util.AbsolutePath(savepath, Util.Filename(savepath) + ".mcs")))
                {
                    schema = Schema.LoadFromFile(Util.AbsolutePath(savepath, Util.Filename(savepath) + ".mcs"));
                    schema.SaveToFile(Util.AbsolutePath(savepath, Util.Filename(savepath) + ".mcs"));
                }
                else
                {
                    schema = Schema.Generate(microinstructions);
                    schema.SaveToFile(Util.AbsolutePath(savepath, Util.Filename(savepath) + ".mcs"));
                    Util.Log("Generated schema for " + Util.Filename(savepath));
                }
                loaded = true;
            }
        }

        public void Load()
        {
            microinstructions.Clear();
            StreamReader sr = new StreamReader(savepath);
            int step = 0;

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (line != "" && !line.StartsWith("//"))
                {
                    ParseLine(line, step);
                    step++;
                }
            }

            sr.Dispose();

            LinkLabels();

            string res = "";
            foreach (var item in microinstructions)
            {
                res += item.ToString() + "\n";
            }
            Util.Log(res);
        }

        public void ParseLine(string line, int step)
        {
            Microinstruction mi = new Microinstruction();
            mi.step = step;

            if (line.Contains("//"))
            {
                line = line.Substring(0, line.IndexOf("//"));
            }

            if (line.Contains('"'))
            {
                mi.docstring = line.Substring(line.IndexOf('"') + 1).Trim();
                line = line.Substring(0, line.IndexOf('"'));
            }

            line = line.Replace(" ", "");

            if (line.Contains(':'))
            {
                mi.label = line.Substring(0, line.IndexOf(':'));
                line = line.Substring(line.IndexOf(':') + 1);
            }

            if (line.Contains("brif"))
            {
                string branch = line.Substring(line.IndexOf("brif") + 4);
                line = line.Substring(0, line.IndexOf("brif"));
                mi.condition = branch.Substring(0, branch.IndexOf("then"));
                mi.destination = branch.Substring(branch.IndexOf("then") + 4);
            }
            if (line.Contains("brcase"))
            {
                string branch = line.Substring(line.IndexOf("brcase") + 6);
                line = line.Substring(0, line.IndexOf("brcase"));
                string condition = branch.Substring(0, branch.IndexOf("then"));
                string destination = branch.Substring(branch.IndexOf("then") + 4);
                mi.caseconditions.AddRange(condition.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                mi.casedestinations.AddRange(destination.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                mi.casei = true;
                if (mi.caseconditions.Count != mi.casedestinations.Count)
                {
                    Util.Log("Case has incompatible number of parameters");
                }
            }
            else if (line.Contains("br"))
            {
                mi.destination = line.Substring(line.IndexOf("br") + 2);
                line = line.Substring(0, line.IndexOf("br"));
            }

            mi.signals.AddRange(line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            microinstructions.Add(mi);
        }

        public void LinkLabels()
        {
            for (int i = 0; i < microinstructions.Count; i++)
            {
                if (microinstructions[i].destination != null)
                {
                    if (microinstructions[i].destination == "this") { microinstructions[i].destinationstep = microinstructions[i].step; }
                    else if (microinstructions[i].destination == "next") { microinstructions[i].destinationstep = microinstructions[i].step + 1; }
                    else
                    {
                        bool done = false;
                        foreach (var item in microinstructions)
                        {
                            if (item.label == microinstructions[i].destination)
                            {
                                microinstructions[i].destinationstep = item.step;
                                done = true;
                                break;
                            }
                        }
                        if (!done)
                        {
                            Util.Log("Label not found: " + microinstructions[i].destination);
                        }
                    }
                }
                if (microinstructions[i].casei)
                {
                    foreach (var mic in microinstructions[i].casedestinations)
                    {
                        bool done = false;
                        foreach (var item in microinstructions)
                        {
                            if (item.label == mic)
                            {
                                microinstructions[i].casedestinationstep.Add(item.step);
                                done = true;
                                break;
                            }
                        }
                        if (!done)
                        {
                            Util.Log("Label not found: " + mic);
                        }
                    }

                }
            }
        }

        public string TranslateMI(Microinstruction mi)
        {
            int hexd = (schema.instrbits() + 3) / 4;
            long inst = 0;
            for (int i = 0; i < mi.signals.Count; i++)
            {
                int ind = schema.signals.IndexOf(mi.signals[i]);
                if (ind == -1) { throw new BadShemaException(); }
                inst += ((long)1 << ind);
            }
            if (mi.destination != null)
            {
                inst += ((long)mi.destinationstep << (schema.signals.Count + schema.ccbits()));
                if (mi.condition != null)
                {
                    int ind = schema.conditions.IndexOf(mi.condition);
                    if (ind == -1) { throw new BadShemaException(); }
                    inst += ((long)(ind + 2) << schema.signals.Count);
                }
                else
                {
                    inst += ((long)1 << schema.signals.Count);
                }
            }
            if (mi.casei) 
            {
                int ind = schema.conditions.IndexOf(mi.getCaseText());
                if (ind == -1) { throw new BadShemaException(); }
                inst += ((long)(ind + 2) << schema.signals.Count);
            }


            return inst.ToString("X" + hexd);
        }

        public string savepath = null;
        public bool loaded = false;
        public List<Microinstruction> microinstructions = new List<Microinstruction>();
        public Schema schema;
    }

    public class Microinstruction
    {
        public int step;
        public string docstring;
        public string destination;
        public string condition;
        public string label;
        public int destinationstep;
        public bool casei = false;
        public List<string> caseconditions = new List<string>();
        public List<string> casedestinations = new List<string>();
        public List<int> casedestinationstep = new List<int>();
        public List<string> signals = new List<string>();

        public string getDisplayText()
        {
            if (docstring != null) { return docstring; }
            return getSignals();
        }

        public string getSignals()
        {
            string res = "";
            bool first = true;
            foreach (var item in signals)
            {
                if (!first) { res += ","; }
                first = false;
                res += item;
            }
            return res;
        }

        public string getCaseText() 
        {
            string res = "";
            bool first = true;
            foreach (var item in caseconditions)
            {
                if (!first) { res += ","; }
                first = false;
                res += item;
            }
            return res;
        }
        public string getThenText()
        {
            string res = "";
            bool first = true;
            foreach (var item in casedestinations)
            {
                if (!first) { res += ","; }
                first = false;
                res += item;
            }
            return res;
        }

        public override string ToString()
        {
            string res = step.ToString("X2");
            if (label != null)
            {
                res += " " + label + ":";
            }
            bool first = true;
            foreach (var item in signals)
            {
                if (!first) { res += ","; }
                else { res += " "; }
                first = false;
                res += item;
            }
            if (destination != null)
            {
                res += " br";
                if (condition != null)
                {
                    res += "if " + condition + " then";
                }
                res += " " + destination + " (" + destinationstep.ToString("X2") + ")";
            }
            if (casei)
            {
                res += " brcase";
                bool firstcc = true;
                foreach (var item in caseconditions)
                {
                    res += firstcc ? " " : ",";
                    res += item;
                    firstcc = false;
                }
                res += " then";
                for (int i = 0; i < casedestinations.Count; i++)
                {
                    res += i == 0 ? " " : ",";
                    try
                    {
                        res += casedestinations[i] + " (" + casedestinationstep[i].ToString("X2") + ")";
                    }
                    catch { }
                }
            }
            if (docstring != null)
            {
                res += " \" " + docstring;
            }
            return res;
        }

        public string ToText()
        {
            string res = step.ToString("X2");
            if (label != null)
            {
                res += " " + label + ":";
            }
            bool first = true;
            foreach (var item in signals)
            {
                if (!first) { res += ","; }
                else { res += " "; }
                first = false;
                res += item;
            }
            if (destination != null)
            {
                res += " br";
                if (condition != null)
                {
                    res += "if " + condition + " then";
                }
                res += " " + destination + " (" + destinationstep.ToString("X2") + ")";
            }
            if (casei)
            {
                res += " brcase";
                bool firstcc = true;
                foreach (var item in caseconditions)
                {
                    res += firstcc ? " " : ",";
                    res += item;
                    firstcc = false;
                }
                res += " then";
                for (int i = 0; i < casedestinations.Count; i++)
                {
                    res += i == 0 ? " " : ",";
                    try
                    {
                        res += casedestinations[i] + " (" + casedestinationstep[i].ToString("X2") + ")";
                    }
                    catch { }
                }
            }
            return res;
        }
    }

    public class Schema
    {
        public List<string> signals = new List<string>();
        public List<string> conditions = new List<string>();
        public int maxstep = 0;

        public static Schema Generate(List<Microinstruction> mi)
        {
            Schema ret = new Schema();

            foreach (var item in mi)
            {
                foreach (var sig in item.signals)
                {
                    if (!ret.signals.Contains(sig))
                    {
                        ret.signals.Add(sig);
                    }
                }
                if (item.condition != null && !ret.conditions.Contains(item.condition))
                {
                    ret.conditions.Add(item.condition);
                }
                if (item.casei)
                {
                    ret.conditions.Add(item.getCaseText());
                }
                if (item.step > ret.maxstep) { ret.maxstep = item.step; }
            }
            ret.maxstep++;
            return ret;
        }

        public int ccbits()
        {
            return Util.nobits(conditions.Count + 2);
        }

        public int instrbits()
        {
            return signals.Count + ccbits() + Util.nobits(maxstep);
        }

        public static Schema LoadFromFile(string path)
        {
            Schema ret = new Schema();
            StreamReader sr = new StreamReader(path);

            string line = sr.ReadLine();
            while (!(line = sr.ReadLine()).StartsWith("//Conditions"))
            {
                ret.signals.Add(line);
            }
            while (!(line = sr.ReadLine()).StartsWith("//Steps"))
            {
                ret.conditions.Add(line);
            }
            line = line.Replace("//Steps ", "");
            ret.maxstep = Convert.ToInt32(line);

            sr.Dispose();
            return ret;
        }

        public void SaveToFile(string path)
        {
            StreamWriter sw = new StreamWriter(path);

            sw.WriteLine("//Signals " + signals.Count + ":");
            for (int i = 0; i < signals.Count; i++)
            {
                sw.WriteLine(signals[i]);
            }

            sw.WriteLine("//Conditions " + conditions.Count + ":");
            for (int i = 0; i < conditions.Count; i++)
            {
                sw.WriteLine(conditions[i]);
            }

            sw.WriteLine("//Steps " + maxstep);

            sw.WriteLine("//Mikroinstrukcija " + instrbits() + " bitova");
            int bitcnt = 0;
            for (int i = 0; i < signals.Count; i++)
            {
                sw.WriteLine(bitcnt.ToString("D2") + ":" + signals[i]);
                bitcnt++;
            }
            for (int i = 0; i < ccbits(); i++)
            {
                sw.WriteLine(bitcnt.ToString("D2") + ":cc" + i);
                bitcnt++;
            }
            for (int i = 0; i < Util.nobits(maxstep); i++)
            {
                sw.WriteLine(bitcnt.ToString("D2") + ":ba" + i);
                bitcnt++;
            }

            sw.WriteLine("//cc kodovi");
            sw.WriteLine("00:noBranch");
            sw.WriteLine("01:uncondJump");
            for (int i = 0; i < conditions.Count; i++)
            {
                sw.WriteLine((i + 2).ToString("D2") + ":" + conditions[i]);
            }

            sw.Dispose();
        }
    }

    [Serializable]
    public class BadShemaException : Exception
    {
        public BadShemaException() { }
        public BadShemaException(string message) : base(message) { }
        public BadShemaException(string message, Exception inner) : base(message, inner) { }
        protected BadShemaException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
