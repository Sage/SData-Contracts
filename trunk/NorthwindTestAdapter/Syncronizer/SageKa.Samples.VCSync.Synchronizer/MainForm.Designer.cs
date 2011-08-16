namespace SageKa.Samples.VCSync.Synchronizer
{
    partial class MainForm
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
            this.txtApp2 = new System.Windows.Forms.TextBox();
            this.txtApp1Example = new System.Windows.Forms.TextBox();
            this.txtApp1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLoggingApp = new System.Windows.Forms.TextBox();
            this.lblLoggingUrl = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sourceAuthenticationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.targetAuthenticationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loggingAuthenticationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.proxySettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSynchronize = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSynchronizeBack = new System.Windows.Forms.ToolStripLabel();
            this.statusStripRunNameLabel = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusRunnameCaption = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripRunName = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpBoxApplications = new System.Windows.Forms.GroupBox();
            this.grpBoxResources = new System.Windows.Forms.GroupBox();
            this.grdResources = new System.Windows.Forms.DataGridView();
            this.ResourceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStripRunNameLabel.SuspendLayout();
            this.grpBoxApplications.SuspendLayout();
            this.grpBoxResources.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResources)).BeginInit();
            this.SuspendLayout();
            // 
            // txtApp2
            // 
            this.txtApp2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtApp2.Location = new System.Drawing.Point(110, 60);
            this.txtApp2.Name = "txtApp2";
            this.txtApp2.Size = new System.Drawing.Size(337, 20);
            this.txtApp2.TabIndex = 10;
            this.txtApp2.TextChanged += new System.EventHandler(this.txtApp2_TextChanged);
            // 
            // txtApp1Example
            // 
            this.txtApp1Example.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtApp1Example.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtApp1Example.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.txtApp1Example.Location = new System.Drawing.Point(110, 112);
            this.txtApp1Example.Multiline = true;
            this.txtApp1Example.Name = "txtApp1Example";
            this.txtApp1Example.ReadOnly = true;
            this.txtApp1Example.Size = new System.Drawing.Size(337, 16);
            this.txtApp1Example.TabIndex = 8;
            this.txtApp1Example.TabStop = false;
            this.txtApp1Example.Text = "Example: http://localhost:5495/sdata/SampleCase1/Default/Source";
            // 
            // txtApp1
            // 
            this.txtApp1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtApp1.Location = new System.Drawing.Point(110, 34);
            this.txtApp1.Name = "txtApp1";
            this.txtApp1.Size = new System.Drawing.Size(337, 20);
            this.txtApp1.TabIndex = 9;
            this.txtApp1.TextChanged += new System.EventHandler(this.txtApp1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Location = new System.Drawing.Point(12, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Target Root URL";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(12, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Source Root URL";
            // 
            // txtLoggingApp
            // 
            this.txtLoggingApp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLoggingApp.Location = new System.Drawing.Point(110, 86);
            this.txtLoggingApp.Name = "txtLoggingApp";
            this.txtLoggingApp.Size = new System.Drawing.Size(337, 20);
            this.txtLoggingApp.TabIndex = 30;
            this.txtLoggingApp.TextChanged += new System.EventHandler(this.txtLoggingApp_TextChanged);
            // 
            // lblLoggingUrl
            // 
            this.lblLoggingUrl.AutoSize = true;
            this.lblLoggingUrl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblLoggingUrl.Location = new System.Drawing.Point(12, 89);
            this.lblLoggingUrl.Name = "lblLoggingUrl";
            this.lblLoggingUrl.Size = new System.Drawing.Size(96, 13);
            this.lblLoggingUrl.TabIndex = 28;
            this.lblLoggingUrl.Text = "Logging Root URL";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(496, 24);
            this.menuStrip1.TabIndex = 34;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::SageKa.Samples.VCSync.Synchronizer.Properties.Resources.open_16;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::SageKa.Samples.VCSync.Synchronizer.Properties.Resources.save_16;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sourceAuthenticationToolStripMenuItem,
            this.targetAuthenticationToolStripMenuItem,
            this.loggingAuthenticationToolStripMenuItem,
            this.proxySettingsToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // sourceAuthenticationToolStripMenuItem
            // 
            this.sourceAuthenticationToolStripMenuItem.Name = "sourceAuthenticationToolStripMenuItem";
            this.sourceAuthenticationToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.sourceAuthenticationToolStripMenuItem.Text = "Source Authentication";
            this.sourceAuthenticationToolStripMenuItem.Click += new System.EventHandler(this.sourceAuthenticationToolStripMenuItem_Click);
            // 
            // targetAuthenticationToolStripMenuItem
            // 
            this.targetAuthenticationToolStripMenuItem.Name = "targetAuthenticationToolStripMenuItem";
            this.targetAuthenticationToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.targetAuthenticationToolStripMenuItem.Text = "Target Authentication";
            this.targetAuthenticationToolStripMenuItem.Click += new System.EventHandler(this.targetAuthenticationToolStripMenuItem_Click);
            // 
            // loggingAuthenticationToolStripMenuItem
            // 
            this.loggingAuthenticationToolStripMenuItem.Name = "loggingAuthenticationToolStripMenuItem";
            this.loggingAuthenticationToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.loggingAuthenticationToolStripMenuItem.Text = "Logging Authentication";
            this.loggingAuthenticationToolStripMenuItem.Click += new System.EventHandler(this.loggingAuthenticationToolStripMenuItem_Click);
            // 
            // proxySettingsToolStripMenuItem
            // 
            this.proxySettingsToolStripMenuItem.Name = "proxySettingsToolStripMenuItem";
            this.proxySettingsToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.proxySettingsToolStripMenuItem.Text = "Proxy Settings";
            this.proxySettingsToolStripMenuItem.Click += new System.EventHandler(this.proxySettingsToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSynchronize,
            this.toolStripSynchronizeBack});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(496, 25);
            this.toolStrip1.TabIndex = 35;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSynchronize
            // 
            this.toolStripSynchronize.Image = global::SageKa.Samples.VCSync.Synchronizer.Properties.Resources.play_16;
            this.toolStripSynchronize.Name = "toolStripSynchronize";
            this.toolStripSynchronize.Size = new System.Drawing.Size(87, 22);
            this.toolStripSynchronize.Text = "Synchronize";
            this.toolStripSynchronize.Click += new System.EventHandler(this.toolStripSynchronize_Click);
            // 
            // toolStripSynchronizeBack
            // 
            this.toolStripSynchronizeBack.Image = global::SageKa.Samples.VCSync.Synchronizer.Properties.Resources.play_back_16;
            this.toolStripSynchronizeBack.Name = "toolStripSynchronizeBack";
            this.toolStripSynchronizeBack.Size = new System.Drawing.Size(123, 22);
            this.toolStripSynchronizeBack.Text = "Syncronize Reverse";
            this.toolStripSynchronizeBack.Click += new System.EventHandler(this.toolStripSynchronizeBack_Click);
            // 
            // statusStripRunNameLabel
            // 
            this.statusStripRunNameLabel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusRunnameCaption,
            this.toolStripRunName});
            this.statusStripRunNameLabel.Location = new System.Drawing.Point(0, 383);
            this.statusStripRunNameLabel.Name = "statusStripRunNameLabel";
            this.statusStripRunNameLabel.Size = new System.Drawing.Size(496, 22);
            this.statusStripRunNameLabel.TabIndex = 36;
            this.statusStripRunNameLabel.Text = "Run Name: ";
            // 
            // toolStripStatusRunnameCaption
            // 
            this.toolStripStatusRunnameCaption.Name = "toolStripStatusRunnameCaption";
            this.toolStripStatusRunnameCaption.Size = new System.Drawing.Size(69, 17);
            this.toolStripStatusRunnameCaption.Text = "Run Name: ";
            // 
            // toolStripRunName
            // 
            this.toolStripRunName.Name = "toolStripRunName";
            this.toolStripRunName.Size = new System.Drawing.Size(66, 17);
            this.toolStripRunName.Text = "Not started";
            // 
            // grpBoxApplications
            // 
            this.grpBoxApplications.Controls.Add(this.txtApp1);
            this.grpBoxApplications.Controls.Add(this.label1);
            this.grpBoxApplications.Controls.Add(this.label2);
            this.grpBoxApplications.Controls.Add(this.txtLoggingApp);
            this.grpBoxApplications.Controls.Add(this.txtApp2);
            this.grpBoxApplications.Controls.Add(this.txtApp1Example);
            this.grpBoxApplications.Controls.Add(this.lblLoggingUrl);
            this.grpBoxApplications.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpBoxApplications.Location = new System.Drawing.Point(12, 52);
            this.grpBoxApplications.Name = "grpBoxApplications";
            this.grpBoxApplications.Size = new System.Drawing.Size(463, 145);
            this.grpBoxApplications.TabIndex = 37;
            this.grpBoxApplications.TabStop = false;
            this.grpBoxApplications.Text = "Aplications";
            // 
            // grpBoxResources
            // 
            this.grpBoxResources.Controls.Add(this.btnDown);
            this.grpBoxResources.Controls.Add(this.btnUp);
            this.grpBoxResources.Controls.Add(this.grdResources);
            this.grpBoxResources.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpBoxResources.Location = new System.Drawing.Point(12, 203);
            this.grpBoxResources.Name = "grpBoxResources";
            this.grpBoxResources.Size = new System.Drawing.Size(463, 160);
            this.grpBoxResources.TabIndex = 38;
            this.grpBoxResources.TabStop = false;
            this.grpBoxResources.Text = "Resources";
            // 
            // grdResources
            // 
            this.grdResources.AllowDrop = true;
            this.grdResources.AllowUserToOrderColumns = true;
            this.grdResources.AllowUserToResizeColumns = false;
            this.grdResources.AllowUserToResizeRows = false;
            this.grdResources.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grdResources.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grdResources.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdResources.ColumnHeadersVisible = false;
            this.grdResources.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ResourceName});
            this.grdResources.Location = new System.Drawing.Point(15, 19);
            this.grdResources.MultiSelect = false;
            this.grdResources.Name = "grdResources";
            this.grdResources.Size = new System.Drawing.Size(397, 135);
            this.grdResources.TabIndex = 0;
            this.grdResources.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdResources_CellEndEdit);
            this.grdResources.SelectionChanged += new System.EventHandler(this.grdResources_SelectionChanged);
            // 
            // ResourceName
            // 
            this.ResourceName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ResourceName.HeaderText = "Resource Name";
            this.ResourceName.Name = "ResourceName";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(412, 20);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(45, 23);
            this.btnUp.TabIndex = 1;
            this.btnUp.Text = "up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(412, 131);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(44, 23);
            this.btnDown.TabIndex = 2;
            this.btnDown.Text = "down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 405);
            this.Controls.Add(this.grpBoxResources);
            this.Controls.Add(this.grpBoxApplications);
            this.Controls.Add(this.statusStripRunNameLabel);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Synchronizer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStripRunNameLabel.ResumeLayout(false);
            this.statusStripRunNameLabel.PerformLayout();
            this.grpBoxApplications.ResumeLayout(false);
            this.grpBoxApplications.PerformLayout();
            this.grpBoxResources.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdResources)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtApp2;
        private System.Windows.Forms.TextBox txtApp1Example;
        private System.Windows.Forms.TextBox txtApp1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLoggingApp;
        private System.Windows.Forms.Label lblLoggingUrl;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripSynchronize;
        private System.Windows.Forms.ToolStripLabel toolStripSynchronizeBack;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sourceAuthenticationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem targetAuthenticationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loggingAuthenticationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem proxySettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStripRunNameLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripRunName;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusRunnameCaption;
        private System.Windows.Forms.GroupBox grpBoxApplications;
        private System.Windows.Forms.GroupBox grpBoxResources;
        private System.Windows.Forms.DataGridView grdResources;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResourceName;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
    }
}

