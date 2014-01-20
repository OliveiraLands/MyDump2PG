namespace MyDump2PG
{
    partial class frmMyDump2PG
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
            this.btnDump = new System.Windows.Forms.Button();
            this.lblDatabaseName = new System.Windows.Forms.Label();
            this.lblTableName = new System.Windows.Forms.Label();
            this.tbtTotalRecords = new System.Windows.Forms.Label();
            this.lblRecords = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.saveSQLFile = new System.Windows.Forms.SaveFileDialog();
            this.ckbKeepFieldCase = new System.Windows.Forms.CheckBox();
            this.edtMyServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.edtMyUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.edtMyPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.grpMySQL = new System.Windows.Forms.GroupBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.cklmyDatabases = new System.Windows.Forms.CheckedListBox();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnNone = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ckbOnlyStructure = new System.Windows.Forms.CheckBox();
            this.grpMySQL.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDump
            // 
            this.btnDump.Enabled = false;
            this.btnDump.Location = new System.Drawing.Point(373, 277);
            this.btnDump.Name = "btnDump";
            this.btnDump.Size = new System.Drawing.Size(135, 32);
            this.btnDump.TabIndex = 0;
            this.btnDump.Text = "Dump SQL ";
            this.btnDump.UseVisualStyleBackColor = true;
            this.btnDump.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblDatabaseName
            // 
            this.lblDatabaseName.Location = new System.Drawing.Point(19, 129);
            this.lblDatabaseName.Name = "lblDatabaseName";
            this.lblDatabaseName.Size = new System.Drawing.Size(348, 13);
            this.lblDatabaseName.TabIndex = 1;
            this.lblDatabaseName.Text = "Current Database";
            // 
            // lblTableName
            // 
            this.lblTableName.Location = new System.Drawing.Point(19, 159);
            this.lblTableName.Name = "lblTableName";
            this.lblTableName.Size = new System.Drawing.Size(129, 13);
            this.lblTableName.TabIndex = 2;
            this.lblTableName.Text = "Current Table";
            // 
            // tbtTotalRecords
            // 
            this.tbtTotalRecords.AutoSize = true;
            this.tbtTotalRecords.Location = new System.Drawing.Point(190, 159);
            this.tbtTotalRecords.Name = "tbtTotalRecords";
            this.tbtTotalRecords.Size = new System.Drawing.Size(56, 13);
            this.tbtTotalRecords.TabIndex = 3;
            this.tbtTotalRecords.Text = "0 Records";
            // 
            // lblRecords
            // 
            this.lblRecords.AutoSize = true;
            this.lblRecords.Location = new System.Drawing.Point(311, 159);
            this.lblRecords.Name = "lblRecords";
            this.lblRecords.Size = new System.Drawing.Size(56, 13);
            this.lblRecords.TabIndex = 4;
            this.lblRecords.Text = "0 Records";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(22, 219);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(347, 23);
            this.progressBar1.TabIndex = 5;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(22, 248);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(345, 23);
            this.progressBar2.TabIndex = 6;
            // 
            // saveSQLFile
            // 
            this.saveSQLFile.Filter = "PostgreSQL SQL Dump files\"|*.sql";
            this.saveSQLFile.Title = "Save MySQL 2 Postgresql Dump to";
            // 
            // ckbKeepFieldCase
            // 
            this.ckbKeepFieldCase.AutoSize = true;
            this.ckbKeepFieldCase.Checked = true;
            this.ckbKeepFieldCase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbKeepFieldCase.Location = new System.Drawing.Point(181, 37);
            this.ckbKeepFieldCase.Name = "ckbKeepFieldCase";
            this.ckbKeepFieldCase.Size = new System.Drawing.Size(132, 17);
            this.ckbKeepFieldCase.TabIndex = 10;
            this.ckbKeepFieldCase.Text = "Keep Field name Case";
            this.ckbKeepFieldCase.UseVisualStyleBackColor = true;
            // 
            // edtMyServer
            // 
            this.edtMyServer.Location = new System.Drawing.Point(12, 37);
            this.edtMyServer.Name = "edtMyServer";
            this.edtMyServer.Size = new System.Drawing.Size(104, 20);
            this.edtMyServer.TabIndex = 13;
            this.edtMyServer.Text = "localhost";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Server Name";
            // 
            // edtMyUser
            // 
            this.edtMyUser.Location = new System.Drawing.Point(12, 76);
            this.edtMyUser.Name = "edtMyUser";
            this.edtMyUser.Size = new System.Drawing.Size(100, 20);
            this.edtMyUser.TabIndex = 15;
            this.edtMyUser.Text = "root";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Admin Login";
            // 
            // edtMyPassword
            // 
            this.edtMyPassword.Location = new System.Drawing.Point(118, 76);
            this.edtMyPassword.Name = "edtMyPassword";
            this.edtMyPassword.PasswordChar = '*';
            this.edtMyPassword.Size = new System.Drawing.Size(85, 20);
            this.edtMyPassword.TabIndex = 17;
            this.edtMyPassword.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(118, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Admin Password";
            // 
            // grpMySQL
            // 
            this.grpMySQL.Controls.Add(this.ckbOnlyStructure);
            this.grpMySQL.Controls.Add(this.btnConnect);
            this.grpMySQL.Controls.Add(this.ckbKeepFieldCase);
            this.grpMySQL.Controls.Add(this.edtMyServer);
            this.grpMySQL.Controls.Add(this.edtMyPassword);
            this.grpMySQL.Controls.Add(this.label3);
            this.grpMySQL.Controls.Add(this.label1);
            this.grpMySQL.Controls.Add(this.edtMyUser);
            this.grpMySQL.Controls.Add(this.label2);
            this.grpMySQL.Location = new System.Drawing.Point(12, 12);
            this.grpMySQL.Name = "grpMySQL";
            this.grpMySQL.Size = new System.Drawing.Size(355, 108);
            this.grpMySQL.TabIndex = 18;
            this.grpMySQL.TabStop = false;
            this.grpMySQL.Text = "MySQL Server Credentials";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(246, 74);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(103, 23);
            this.btnConnect.TabIndex = 18;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // cklmyDatabases
            // 
            this.cklmyDatabases.CheckOnClick = true;
            this.cklmyDatabases.FormattingEnabled = true;
            this.cklmyDatabases.Location = new System.Drawing.Point(373, 42);
            this.cklmyDatabases.Name = "cklmyDatabases";
            this.cklmyDatabases.Size = new System.Drawing.Size(135, 229);
            this.cklmyDatabases.TabIndex = 19;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(373, 11);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(62, 25);
            this.btnSelectAll.TabIndex = 20;
            this.btnSelectAll.Text = "All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnNone
            // 
            this.btnNone.Location = new System.Drawing.Point(444, 11);
            this.btnNone.Name = "btnNone";
            this.btnNone.Size = new System.Drawing.Size(62, 25);
            this.btnNone.TabIndex = 21;
            this.btnNone.Text = "None";
            this.btnNone.UseVisualStyleBackColor = true;
            this.btnNone.Click += new System.EventHandler(this.btnNone_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 316);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.Size = new System.Drawing.Size(519, 22);
            this.statusStrip1.TabIndex = 22;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // ckbOnlyStructure
            // 
            this.ckbOnlyStructure.AutoSize = true;
            this.ckbOnlyStructure.Location = new System.Drawing.Point(181, 14);
            this.ckbOnlyStructure.Name = "ckbOnlyStructure";
            this.ckbOnlyStructure.Size = new System.Drawing.Size(123, 17);
            this.ckbOnlyStructure.TabIndex = 19;
            this.ckbOnlyStructure.Text = "Copy only Structures";
            this.ckbOnlyStructure.UseVisualStyleBackColor = true;
            // 
            // frmMyDump2PG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 338);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnNone);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.cklmyDatabases);
            this.Controls.Add(this.grpMySQL);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblRecords);
            this.Controls.Add(this.tbtTotalRecords);
            this.Controls.Add(this.lblTableName);
            this.Controls.Add(this.lblDatabaseName);
            this.Controls.Add(this.btnDump);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmMyDump2PG";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MySQL Dump to PostgreSQL script backup";
            this.grpMySQL.ResumeLayout(false);
            this.grpMySQL.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDump;
        private System.Windows.Forms.Label lblDatabaseName;
        private System.Windows.Forms.Label lblTableName;
        private System.Windows.Forms.Label tbtTotalRecords;
        private System.Windows.Forms.Label lblRecords;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.SaveFileDialog saveSQLFile;
        private System.Windows.Forms.CheckBox ckbKeepFieldCase;
        private System.Windows.Forms.TextBox edtMyServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox edtMyUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox edtMyPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox grpMySQL;
        private System.Windows.Forms.CheckedListBox cklmyDatabases;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnNone;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.CheckBox ckbOnlyStructure;
    }
}

