/* MIT LICENCE:

Copyright (C) 2013 - Fernando Francisco de Oliveira

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT 
SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN 
ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE 
OR OTHER DEALINGS IN THE SOFTWARE.
 
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyDump2PG
{
    public partial class frmMyDump2PG : Form
    {
        DateTime inicio = DateTime.Now;
        DateTime termino = DateTime.Now;
        TimeSpan tempo;
        string currentDB = "mysql";

        public string mySQLConn
        {
            get
            {
                return String.Format("Data Source={0};User ID={1};Password={2};Old Guids=true;Database={3};",
                            edtMyServer.Text, edtMyUser.Text, edtMyPassword.Text, currentDB);
            }
        }
        public frmMyDump2PG()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            btnDump.Enabled = false;
            cklmyDatabases.Items.Clear();

            if (btnConnect.Text == "Connect")
            {
                btnConnect.Text = "Disconnect";

                currentDB = "mysql";

                using (Schema2Script s2s = new Schema2Script(mySQLConn))
                {
                    try
                    {
                        //database = s2s.GetDatabaseName();

                        DataTable dbs = s2s.GetDatabases();

                        foreach (DataRow row in dbs.Rows)
                        {
                            if (row["SCHEMA_NAME"].ToString() != "mysql" &&
                                row["SCHEMA_NAME"].ToString() != "test" &&
                                row["SCHEMA_NAME"].ToString() != "information_schema" &&
                                row["SCHEMA_NAME"].ToString() != "performance_schema")
                            {
                                cklmyDatabases.Items.Add(row["SCHEMA_NAME"].ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error connecting to database", ex.Message);
                        return;
                    }
                }
                btnDump.Enabled = true;
            }
            else
            {
                btnConnect.Text = "Connect";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cklmyDatabases.CheckedItems.Count == 0)
            {
                MessageBox.Show("You must select at least one database");
                return;
            }

            for(int iDb = 0; iDb< cklmyDatabases.CheckedItems.Count; iDb++) {

                currentDB = cklmyDatabases.CheckedItems[iDb].ToString();

                processExport();

                int i = cklmyDatabases.Items.IndexOf(currentDB);

                if (i >= 0) cklmyDatabases.SetItemChecked(i, false);
            }
        }

        void processExport()
        {
            using (Schema2Script s2s = new Schema2Script(mySQLConn))
            {
                string database = "";

                try
                {
                    database = s2s.GetDatabaseName();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error connecting to database", ex.Message);
                    return;
                }

                saveSQLFile.FileName = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + database + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".sql";

                lblDatabaseName.Text = "db:" + database;

                //if (saveSQLFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //{
                s2s.createDatabase = true;
                s2s.keepFieldCase = ckbKeepFieldCase.Checked;
                s2s.copyOnlyStructures = ckbOnlyStructure.Checked;

                s2s.DumpProgressChanged += s2s_DumpProgressChanged;
                inicio = DateTime.Now;

                s2s.exportDump(saveSQLFile.FileName);

                termino = DateTime.Now;
                tempo = termino - inicio;

                statusStrip1.Items[0].Text = "Start " + inicio.ToString() + " Finished " + termino.ToString() + " Duration " +
                     tempo.Minutes + " m " + tempo.Seconds + " s " + tempo.Milliseconds + " ms";
                //}
            }
        }

        void s2s_DumpProgressChanged(object sender, DumpProgressArg e)
        {
            lblDatabaseName.Text = e.databaseName;
            lblTableName.Text = e.tableName;
            lblTableName.Refresh();
            tbtTotalRecords.Text = e.recordCount.ToString();
            tbtTotalRecords.Refresh();
            lblRecords.Text = e.recordNum.ToString();
            lblRecords.Refresh();

            progressBar1.Maximum = (int)e.recordCount;
            progressBar1.Value = (int)e.recordNum;
            progressBar1.Refresh();

            progressBar2.Maximum = (int)e.tableCount;
            progressBar2.Value = (int)e.tableNum;
            progressBar2.Refresh();

        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            markDatabases(sender);
        }

        private void markDatabases(object sender)
        {
            for (int i = 0; i < cklmyDatabases.Items.Count; i++)
            {
                cklmyDatabases.SetItemChecked(i, (sender == btnSelectAll));
            }
        }

        private void btnNone_Click(object sender, EventArgs e)
        {
            markDatabases(sender);
        }
        
    }
}
