namespace SimpleSyncStarter
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
            this.label1 = new System.Windows.Forms.Label();
            this.btn_2to1 = new System.Windows.Forms.Button();
            this.txtEndpoint1 = new System.Windows.Forms.TextBox();
            this.txtEndpoint2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_1to2 = new System.Windows.Forms.Button();
            this.lblFeedUrlExample = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Application 1";
            // 
            // btn_2to1
            // 
            this.btn_2to1.Location = new System.Drawing.Point(251, 112);
            this.btn_2to1.Name = "btn_2to1";
            this.btn_2to1.Size = new System.Drawing.Size(155, 23);
            this.btn_2to1.TabIndex = 3;
            this.btn_2to1.Text = "Synchronize App 2 -> App 1";
            this.btn_2to1.UseVisualStyleBackColor = true;
            this.btn_2to1.Click += new System.EventHandler(this.btn_2to1_Click);
            // 
            // txtEndpoint1
            // 
            this.txtEndpoint1.Location = new System.Drawing.Point(89, 17);
            this.txtEndpoint1.Name = "txtEndpoint1";
            this.txtEndpoint1.Size = new System.Drawing.Size(668, 20);
            this.txtEndpoint1.TabIndex = 0;
            // 
            // txtEndpoint2
            // 
            this.txtEndpoint2.Location = new System.Drawing.Point(89, 62);
            this.txtEndpoint2.Name = "txtEndpoint2";
            this.txtEndpoint2.Size = new System.Drawing.Size(668, 20);
            this.txtEndpoint2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Application 2";
            // 
            // btn_1to2
            // 
            this.btn_1to2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_1to2.Location = new System.Drawing.Point(90, 112);
            this.btn_1to2.Name = "btn_1to2";
            this.btn_1to2.Size = new System.Drawing.Size(155, 23);
            this.btn_1to2.TabIndex = 2;
            this.btn_1to2.Text = "Synchronize App 1 -> App 2";
            this.btn_1to2.UseVisualStyleBackColor = true;
            this.btn_1to2.Click += new System.EventHandler(this.btn_1to2_Click);
            // 
            // lblFeedUrlExample
            // 
            this.lblFeedUrlExample.AutoSize = true;
            this.lblFeedUrlExample.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblFeedUrlExample.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblFeedUrlExample.Location = new System.Drawing.Point(86, 37);
            this.lblFeedUrlExample.Name = "lblFeedUrlExample";
            this.lblFeedUrlExample.Size = new System.Drawing.Size(359, 13);
            this.lblFeedUrlExample.TabIndex = 6;
            this.lblFeedUrlExample.Text = "Example: http://localhost:5493/sdata/Northwind/gcrm/-/TradingAccounts";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.ForestGreen;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(86, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(359, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Example: http://localhost:5493/sdata/Northwind/gcrm/-/TradingAccounts";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panel1.ForeColor = System.Drawing.Color.LightGray;
            this.panel1.Location = new System.Drawing.Point(3, 159);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(763, 1);
            this.panel1.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(691, 172);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(769, 207);
            this.Controls.Add(this.txtEndpoint2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtEndpoint1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_1to2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblFeedUrlExample);
            this.Controls.Add(this.btn_2to1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "Simple Sync Starter";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_2to1;
        private System.Windows.Forms.TextBox txtEndpoint1;
        private System.Windows.Forms.TextBox txtEndpoint2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_1to2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblFeedUrlExample;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
    }
}

