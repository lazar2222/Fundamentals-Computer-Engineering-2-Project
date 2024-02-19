namespace ORTProjectManager
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOpenProject = new System.Windows.Forms.Button();
            this.btnSaveProject = new System.Windows.Forms.Button();
            this.lOpenProject = new System.Windows.Forms.Label();
            this.ofdOpen = new System.Windows.Forms.OpenFileDialog();
            this.btnNewProject = new System.Windows.Forms.Button();
            this.sfdsave = new System.Windows.Forms.SaveFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.lbMicrocode = new System.Windows.Forms.ListBox();
            this.btnAddMicrocode = new System.Windows.Forms.Button();
            this.ofdAddMicrocode = new System.Windows.Forms.OpenFileDialog();
            this.btnSchema = new System.Windows.Forms.Button();
            this.btnTranslate = new System.Windows.Forms.Button();
            this.btnSegment = new System.Windows.Forms.Button();
            this.btnAll = new System.Windows.Forms.Button();
            this.btnDiagram = new System.Windows.Forms.Button();
            this.logBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnOpenProject
            // 
            this.btnOpenProject.Location = new System.Drawing.Point(107, 12);
            this.btnOpenProject.Name = "btnOpenProject";
            this.btnOpenProject.Size = new System.Drawing.Size(89, 23);
            this.btnOpenProject.TabIndex = 0;
            this.btnOpenProject.Text = "Open Project";
            this.btnOpenProject.UseVisualStyleBackColor = true;
            this.btnOpenProject.Click += new System.EventHandler(this.btnOpenProject_Click);
            // 
            // btnSaveProject
            // 
            this.btnSaveProject.Location = new System.Drawing.Point(202, 12);
            this.btnSaveProject.Name = "btnSaveProject";
            this.btnSaveProject.Size = new System.Drawing.Size(89, 23);
            this.btnSaveProject.TabIndex = 1;
            this.btnSaveProject.Text = "SaveProject";
            this.btnSaveProject.UseVisualStyleBackColor = true;
            this.btnSaveProject.Click += new System.EventHandler(this.btnSaveProject_Click);
            // 
            // lOpenProject
            // 
            this.lOpenProject.AutoSize = true;
            this.lOpenProject.Location = new System.Drawing.Point(297, 17);
            this.lOpenProject.Name = "lOpenProject";
            this.lOpenProject.Size = new System.Drawing.Size(101, 13);
            this.lOpenProject.TabIndex = 2;
            this.lOpenProject.Text = "OpenProject:(None)";
            // 
            // ofdOpen
            // 
            this.ofdOpen.DefaultExt = "ortproj";
            this.ofdOpen.Filter = "ORTProjectFiles|*.ortproj";
            this.ofdOpen.InitialDirectory = "D:\\Faks\\_3. Semestar\\ORT2\\Projekat";
            this.ofdOpen.Title = "OpenProject";
            // 
            // btnNewProject
            // 
            this.btnNewProject.Location = new System.Drawing.Point(12, 12);
            this.btnNewProject.Name = "btnNewProject";
            this.btnNewProject.Size = new System.Drawing.Size(89, 23);
            this.btnNewProject.TabIndex = 3;
            this.btnNewProject.Text = "New Project";
            this.btnNewProject.UseVisualStyleBackColor = true;
            this.btnNewProject.Click += new System.EventHandler(this.btnNewProject_Click);
            // 
            // sfdsave
            // 
            this.sfdsave.DefaultExt = "ortproj";
            this.sfdsave.Filter = "ORTProjectFiles|*.ortproj";
            this.sfdsave.InitialDirectory = "D:\\Faks\\_3. Semestar\\ORT2\\Projekat";
            this.sfdsave.Title = "Save Project";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Microcode Files";
            // 
            // lbMicrocode
            // 
            this.lbMicrocode.FormattingEnabled = true;
            this.lbMicrocode.Location = new System.Drawing.Point(12, 64);
            this.lbMicrocode.Name = "lbMicrocode";
            this.lbMicrocode.Size = new System.Drawing.Size(120, 186);
            this.lbMicrocode.TabIndex = 5;
            // 
            // btnAddMicrocode
            // 
            this.btnAddMicrocode.Location = new System.Drawing.Point(12, 256);
            this.btnAddMicrocode.Name = "btnAddMicrocode";
            this.btnAddMicrocode.Size = new System.Drawing.Size(120, 23);
            this.btnAddMicrocode.TabIndex = 6;
            this.btnAddMicrocode.Text = "Add";
            this.btnAddMicrocode.UseVisualStyleBackColor = true;
            this.btnAddMicrocode.Click += new System.EventHandler(this.btnAddMicrocode_Click);
            // 
            // ofdAddMicrocode
            // 
            this.ofdAddMicrocode.DefaultExt = "mc";
            this.ofdAddMicrocode.Filter = "MicrocodeFiles|*.mc";
            this.ofdAddMicrocode.InitialDirectory = "D:\\Faks\\_3. Semestar\\ORT2\\Projekat";
            this.ofdAddMicrocode.Title = "Add Microcode File";
            // 
            // btnSchema
            // 
            this.btnSchema.Location = new System.Drawing.Point(419, 12);
            this.btnSchema.Name = "btnSchema";
            this.btnSchema.Size = new System.Drawing.Size(118, 23);
            this.btnSchema.TabIndex = 7;
            this.btnSchema.Text = "Generate Schemas";
            this.btnSchema.UseVisualStyleBackColor = true;
            this.btnSchema.Click += new System.EventHandler(this.btnSchema_Click);
            // 
            // btnTranslate
            // 
            this.btnTranslate.Location = new System.Drawing.Point(419, 42);
            this.btnTranslate.Name = "btnTranslate";
            this.btnTranslate.Size = new System.Drawing.Size(118, 23);
            this.btnTranslate.TabIndex = 8;
            this.btnTranslate.Text = "Translate";
            this.btnTranslate.UseVisualStyleBackColor = true;
            this.btnTranslate.Click += new System.EventHandler(this.btnTranslate_Click);
            // 
            // btnSegment
            // 
            this.btnSegment.Location = new System.Drawing.Point(419, 72);
            this.btnSegment.Name = "btnSegment";
            this.btnSegment.Size = new System.Drawing.Size(118, 23);
            this.btnSegment.TabIndex = 9;
            this.btnSegment.Text = "Segment";
            this.btnSegment.UseVisualStyleBackColor = true;
            this.btnSegment.Click += new System.EventHandler(this.btnSegment_Click);
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(419, 131);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(118, 23);
            this.btnAll.TabIndex = 10;
            this.btnAll.Text = "Run All";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // btnDiagram
            // 
            this.btnDiagram.Location = new System.Drawing.Point(419, 102);
            this.btnDiagram.Name = "btnDiagram";
            this.btnDiagram.Size = new System.Drawing.Size(118, 23);
            this.btnDiagram.TabIndex = 11;
            this.btnDiagram.Text = "Generate Diagram";
            this.btnDiagram.UseVisualStyleBackColor = true;
            this.btnDiagram.Click += new System.EventHandler(this.btnDiagram_Click);
            // 
            // logBox
            // 
            this.logBox.FormattingEnabled = true;
            this.logBox.Location = new System.Drawing.Point(12, 534);
            this.logBox.Name = "logBox";
            this.logBox.Size = new System.Drawing.Size(525, 173);
            this.logBox.TabIndex = 12;
            this.logBox.SelectedIndexChanged += new System.EventHandler(this.logBox_SelectedIndexChanged);
            this.logBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.logBox_MouseDoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 719);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.btnDiagram);
            this.Controls.Add(this.btnAll);
            this.Controls.Add(this.btnSegment);
            this.Controls.Add(this.btnTranslate);
            this.Controls.Add(this.btnSchema);
            this.Controls.Add(this.btnAddMicrocode);
            this.Controls.Add(this.lbMicrocode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnNewProject);
            this.Controls.Add(this.lOpenProject);
            this.Controls.Add(this.btnSaveProject);
            this.Controls.Add(this.btnOpenProject);
            this.Name = "Form1";
            this.Text = "ORTProjectManager";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenProject;
        private System.Windows.Forms.Button btnSaveProject;
        private System.Windows.Forms.Label lOpenProject;
        private System.Windows.Forms.OpenFileDialog ofdOpen;
        private System.Windows.Forms.Button btnNewProject;
        private System.Windows.Forms.SaveFileDialog sfdsave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbMicrocode;
        private System.Windows.Forms.Button btnAddMicrocode;
        private System.Windows.Forms.OpenFileDialog ofdAddMicrocode;
        private System.Windows.Forms.Button btnSchema;
        private System.Windows.Forms.Button btnTranslate;
        private System.Windows.Forms.Button btnSegment;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.Button btnDiagram;
        private System.Windows.Forms.ListBox logBox;
    }
}

