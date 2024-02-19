using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORTProjectManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Project LoadedProject;
        public Dictionary<string, string> ProgramPreferences;

        private void Form1_Load(object sender, EventArgs e)
        {
            Util.logBox = logBox;
            ProgramPreferences = Util.LoadFromFile("preferences.pref");
            if (ProgramPreferences.ContainsKey("AUTOLOADPROJECT"))
            {
                try
                {
                    LoadedProject = Project.LoadFromFile(ProgramPreferences["AUTOLOADPROJECT"]);
                    UpdateLabel();
                    Util.Log(LoadedProject.savepath + " loaded");
                }
                catch
                {
                    btnNewProject_Click(sender, e);
                }
            }
            else
            {
                btnNewProject_Click(sender, e);
            }
        }

        private void btnNewProject_Click(object sender, EventArgs e)
        {
            LoadedProject = new Project();
            UpdateLabel();
            Util.Log("Created new Project");
        }

        private void btnOpenProject_Click(object sender, EventArgs e)
        {
            ofdOpen.ShowDialog();
            if (ofdOpen.FileName != "")
            {
                LoadedProject = Project.LoadFromFile(ofdOpen.FileName);
                UpdateLabel();
                Util.Log(LoadedProject.savepath + " loaded");
                ofdOpen.FileName = "";
            }
        }

        private void btnSaveProject_Click(object sender, EventArgs e)
        {
            if (LoadedProject.savepath == null)
            {
                sfdsave.ShowDialog();
                if (sfdsave.FileName != "")
                {
                    LoadedProject.savepath = sfdsave.FileName;
                    sfdsave.FileName = "";
                }
            }
            if (LoadedProject.savepath != null)
            {
                LoadedProject.SaveToFile();
                UpdateLabel();
                Util.Log(LoadedProject.savepath + " saved");
            }
        }

        private void UpdateLabel()
        {
            if (LoadedProject.savepath != null)
            {
                lOpenProject.Text = "OpenProject:" + Util.Filename(LoadedProject.savepath);
            }
            else
            {
                lOpenProject.Text = "OpenProject:Untitled";
            }

            lbMicrocode.Items.Clear();
            foreach (var item in LoadedProject.Microcodes)
            {
                lbMicrocode.Items.Add(Util.Filename(item.savepath));
            }

            btnSchema.BackColor = LoadedProject.HasSchemas() ? Color.Lime: Color.Red;
            btnTranslate.BackColor = LoadedProject.HasMicroHex() ? Color.Lime : Color.Red;
            btnSegment.BackColor = LoadedProject.HasSegment() ? Color.Lime : Color.Red;
            btnDiagram.BackColor = LoadedProject.HasDiagram() ? Color.Lime : Color.Red;
            btnAll.BackColor = LoadedProject.HasDiagram() && LoadedProject.HasSegment() && LoadedProject.HasMicroHex() && LoadedProject.HasSchemas() ? Color.Lime : Color.Red;
        }

        private void btnAddMicrocode_Click(object sender, EventArgs e)
        {
            ofdAddMicrocode.ShowDialog();
            if (ofdAddMicrocode.FileName != "")
            {
                if (!LoadedProject.Microcodes.Any(item => item.savepath == ofdAddMicrocode.FileName))
                {
                    LoadedProject.Microcodes.Add(new Microcode(ofdAddMicrocode.FileName));
                    Util.Log(ofdAddMicrocode.FileName + " added");
                }
                UpdateLabel();
                ofdAddMicrocode.FileName = "";
            }
        }

        private void btnSchema_Click(object sender, EventArgs e)
        {
            LoadedProject.Unload();
            LoadedProject.GenerateSchemas();
            UpdateLabel();
        }

        private void btnTranslate_Click(object sender, EventArgs e)
        {
            LoadedProject.Unload();
            LoadedProject.Translate();
            UpdateLabel();
        }

        private void btnSegment_Click(object sender, EventArgs e)
        {
            LoadedProject.Unload();
            LoadedProject.Segment();
            UpdateLabel();
        }

        private void btnDiagram_Click(object sender, EventArgs e)
        {
            LoadedProject.Unload();
            LoadedProject.Diagram();
            UpdateLabel();
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            LoadedProject.Unload();
            LoadedProject.GenerateSchemas();
            LoadedProject.Translate();
            LoadedProject.Segment();
            LoadedProject.Diagram();
            UpdateLabel();
        }

        private void logBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            logBox.Items.Clear();
        }

        private void logBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clipboard.SetText(logBox.SelectedItem.ToString());
        }
    }
}
