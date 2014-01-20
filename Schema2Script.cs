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
using System.Linq;
using System.Text;
using System.Globalization;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;

namespace MyDump2PG
{
    public class DumpProgressArg : EventArgs
    {
        public string tableName { get; set; }
        public string databaseName { get; set; }
        public long recordCount { get; set; }
        public long recordNum { get; set; }
        public bool cancel { get; set; }
        public long tableCount { get; set; }
        public long tableNum { get; set; }
    }

    class Schema2Script : IDisposable
    {
        const string VERSION = "vs 1.0";
        
        public string m_connectionString;

        NumberFormatInfo numFormat;
        DateTimeFormatInfo dateFormat;
        private bool m_keepFieldCase = true;

        public static int sqlTimeOut = 30 * 60;
        public bool createDatabase = true;
        public bool copyOnlyStructures = false;

        public bool keepFieldCase { 
            get { return m_keepFieldCase; }
            set { 
                m_keepFieldCase = value; 
            }
        }

        TextWriter twSQLFile;
        
        List<tableStruct> lstTables = new List<tableStruct>();

        DumpProgressArg dumpProgressArg = new DumpProgressArg();

        public delegate void dumpProgressChange(object sender, DumpProgressArg e);

        public event dumpProgressChange DumpProgressChanged;

        public Schema2Script(string connectionString)
        {
            numFormat = new NumberFormatInfo();
            numFormat.NumberDecimalSeparator = ".";
            numFormat.NumberGroupSeparator = string.Empty;

            dateFormat = new DateTimeFormatInfo();
            dateFormat.DateSeparator = "-";
            dateFormat.TimeSeparator = ":";

            m_connectionString = connectionString;
        }

        public void Dispose()
        {
            if (twSQLFile != null)
            {
                twSQLFile.Dispose();
            }
        }

        public void executeNonQuery(string sql)
        {
            using (MySqlConnection oConn = new MySqlConnection(m_connectionString))
            using (MySqlCommand oComm = new MySqlCommand())
            {
                oComm.Connection = oConn;
                oComm.Connection.Open();

                oComm.CommandText = sql;
                oComm.CommandTimeout = sqlTimeOut;
                oComm.ExecuteNonQuery();

                oConn.Close();
            }
        }

        public object executeScalar(string sql)
        {
            using (MySqlConnection oConn = new MySqlConnection(m_connectionString))
            using (MySqlCommand oComm = new MySqlCommand())
            {
                oComm.Connection = oConn;
                oComm.Connection.Open();

                oComm.CommandText = sql;
                oComm.CommandTimeout = sqlTimeOut;
                return oComm.ExecuteScalar();
            }
        }

        public DataTable executeQuery(string sql)
        {
            DataTable dtRet = new DataTable();

            using (MySqlConnection oConn = new MySqlConnection(m_connectionString))
            using (MySqlCommand oComm = new MySqlCommand())
            {
                oComm.Connection = oConn;
                oComm.Connection.Open();

                oComm.CommandText = sql;
                oComm.CommandTimeout = sqlTimeOut;

                MySqlDataAdapter daSrc = new MySqlDataAdapter(oComm);

                daSrc.Fill(dtRet);

                return dtRet;
            }
        }

        public string GetMyServerVersion()
        {
            string retVersion = "";

            DataTable odtVersion = executeQuery("SHOW variables WHERE Variable_name = 'version';");

            retVersion = odtVersion.Rows[0][1].ToString();

            odtVersion = executeQuery("SHOW variables WHERE Variable_name = 'version_comment';");

            retVersion += " " + odtVersion.Rows[0][1].ToString();

            return retVersion;
        }

        public DataTable GetDatabases()
        {
            return executeQuery("SELECT * FROM `information_schema`.`SCHEMATA`;");
        }
        
        public string GetDatabaseName()
        {
            using (MySqlConnection oConn = new MySqlConnection(m_connectionString))
            using (MySqlCommand oComm = new MySqlCommand())
            {
                oComm.Connection = oConn;
                oComm.Connection.Open();
                oComm.CommandText = "select Database()";

                return (string)oComm.ExecuteScalar();
            }
        }

        public void exportDump(string fileName)
        {
            executeNonQuery("Set global max_allowed_packet=1073741824"); // 1GB

            string databaseName = GetDatabaseName();
            string username = (string)executeScalar("select current_user");

            Dictionary<string, tableStruct> tables = new Dictionary<string, tableStruct>();

            //DataTable dtTables = executeQuery("Show full tables where Table_type like 'BASE TABLE';");
            DataTable dtTables = executeQuery("SELECT * FROM `information_schema`.`TABLES` where table_type='BASE TABLE' and table_schema='" + databaseName + "';");

            for (int i = 0; i < dtTables.Rows.Count; i++)
            {
                string tbName = dtTables.Rows[i]["TABLE_NAME"].ToString();
                string tbEngine = dtTables.Rows[i]["ENGINE"].ToString();
                string tbCollate = dtTables.Rows[i]["TABLE_COLLATION"].ToString();

                tables.Add(tbName, new tableStruct(databaseName, tbName, m_connectionString, m_keepFieldCase, tbEngine, tbCollate));
            }

            string fileBat = Path.GetDirectoryName(fileName) + "\\restore_" + databaseName + ".bat";


            //Encoding utf8woBom = new ASCIIEncoding();

            using (twSQLFile = new StreamWriter(fileBat, false, Encoding.Default))
            {
                twSQLFile.WriteLine("psql -d " + (createDatabase?"postgres": databaseName) + " -U postgres -q -f " + Path.GetFileName(fileName));
            }

            using (twSQLFile = new StreamWriter(fileName, false, Encoding.Default))
            {
                twSQLFile.WriteLine(String.Format("-- MySQL Dump for PostgreSQL scripts {0}", VERSION));
                twSQLFile.WriteLine(String.Format("--       . MySQL version {0}", GetMyServerVersion()) );
                twSQLFile.WriteLine("--");
                twSQLFile.WriteLine("");

                if (createDatabase)
                {
                    twSQLFile.WriteLine("Drop database if exists " + databaseName + ";");
                    twSQLFile.WriteLine("Create database " + databaseName + ";");
                    twSQLFile.WriteLine("\\c " + databaseName + ";");
                    twSQLFile.WriteLine("");
                }

                dumpProgressArg.databaseName = databaseName;
                dumpProgressArg.tableCount = tables.Count;

                int numtable = 0;
                foreach (KeyValuePair<string, tableStruct> table in tables)
                {
                    numtable++;

                    twSQLFile.WriteLine("-- ================================================================= -- ");
                    twSQLFile.WriteLine("-- Create table " + table.Value.tableName + " (" + table.Value.rowCount + " rows)");
                    twSQLFile.WriteLine("");
                    twSQLFile.WriteLine("Drop table if exists " + table.Value.fname(table.Value.tableName) + ";");
                    twSQLFile.WriteLine("");
                    twSQLFile.WriteLine(table.Value.tableCreateSQL);
                    twSQLFile.WriteLine("");

                    if (!copyOnlyStructures)
                    {
                        using (MySqlConnection oConn = new MySqlConnection(m_connectionString))
                        using (MySqlCommand oComm = new MySqlCommand())
                        {
                            oComm.CommandTimeout = (60 * 60);
                            oComm.Connection = oConn;
                            oComm.Connection.Open();
                            oComm.CommandText = table.Value.tableSelectAll;

                            MySqlDataReader rdData = oComm.ExecuteReader();

                            dumpProgressArg.tableName = table.Value.tableName;
                            dumpProgressArg.recordCount = table.Value.rowCount;
                            dumpProgressArg.recordNum = 0;

                            string sComma = "";
                            long recCount = 0;
                            while (rdData.Read())
                            {
                                if (sComma == "")
                                {
                                    twSQLFile.Write(table.Value.tableInsertSQL);
                                }

                                if (DumpProgressChanged != null)
                                {
                                    dumpProgressArg.tableNum = numtable;
                                    dumpProgressArg.recordNum++;
                                    DumpProgressChanged(this, dumpProgressArg);
                                }

                                if (recCount++ > 250)
                                {
                                    twSQLFile.WriteLine(";");
                                    twSQLFile.Write(table.Value.tableInsertSQL);
                                    sComma = "";
                                    recCount = 0;
                                }
                                twSQLFile.WriteLine(sComma);

                                string strValues = getValuesSQL(rdData, table.Value);
                                sComma = ",";

                                twSQLFile.Write("    " + strValues);
                            }
                            if (sComma != "")
                                twSQLFile.WriteLine(";");

                            twSQLFile.Flush();
                        }
                    }
                }
            }
        }

        private string getValuesSQL(MySqlDataReader drData, tableStruct table)
        {
            StringBuilder sbData = new StringBuilder();

            sbData.Append("(");

            for (int i = 0; i < drData.FieldCount; i++)
            {
                object objData;
                string fieldName = drData.GetName(i);

                try
                {
                    objData = drData[i];
                }
                catch
                {
                    objData = null;
                }

                /*
                if (fieldName == "data" && !(objData is System.String))
                {
                    string tipo = objData.GetType().ToString();

                    sbData.Append(" *" + tipo + "* ");
                }
                */

                if (objData == null || objData is System.DBNull)
                {
                    sbData.Append("NULL,");
                }

                else if (objData is System.String)
                {
                    if (table.fieldTypes[fieldName] == "longblob")
                    {
                        byte[] objByte = Encoding.Default.GetBytes((string)objData);

                        sbData.Append(GetBLOBfromData(objByte) + ",");
                    }
                    else
                    {
                        string data = objData.ToString();
                        string fieldComm = "";

                        if (table.fieldCharSet.ContainsKey(fieldName))
                        {
                            string charset = table.fieldCharSet[fieldName];

                            if (charset == "xlatin1")
                            {
                                string data2 = Encoding.UTF8.GetString(Encoding.GetEncoding("iso-8859-1").GetBytes(data));
                                //string data2 = Encoding.UTF8.GetString(Encoding.Default.GetBytes(data));

                                //byte[] utf8Bytes = Encoding.Default.GetBytes(data);
                                //byte[] isoBytes = Encoding.Convert(Encoding.GetEncoding("iso-8859-1"), Encoding.UTF8, utf8Bytes);

                                //string data2 = Encoding.UTF8.GetString(isoBytes);

                                EscapeString(ref data2);
                                fieldComm = String.Format("E'{0}'", data2);
                            }
                            else
                                if (charset == "xutf8")
                                {
                                    byte[] bytes = Encoding.Default.GetBytes(data);

                                    string data2 = Encoding.UTF8.GetString(bytes);

                                    EscapeString(ref data2);

                                    fieldComm = String.Format("E'{0}'", data2);
                                }
                                else
                                {
                                    EscapeString(ref data);
                                    fieldComm = String.Format("E'{0}'", data);
                                }
                        }
                        else
                        {
                            EscapeString(ref data);
                            fieldComm = String.Format("E'{0}'", data);
                        }


                        if (table.fieldSizes.ContainsKey(fieldName))
                        {
                            int fieldSize = table.fieldSizes[fieldName];

                            sbData.Append("cast(" + fieldComm + " as varchar(" + fieldSize + ")),");
                        }
                        else
                            sbData.Append(fieldComm + ",");
                    }
                }
                else if (objData is System.DateTime)
                {
                    try
                    {
                        sbData.Append(String.Format("'{0}',", ((DateTime)objData).ToString("yyyy-MM-dd HH:mm:ss", dateFormat)));
                    }
                    catch (Exception ex)
                    {
                        sbData.Append("NULL,");
                    }
                }
                else if (objData is System.Boolean)
                {
                    sbData.Append(objData.ToString() + ",");
                }
                else if (objData is System.Byte[])
                {
                    sbData.Append( GetBLOBfromData((byte[])objData) + ",");
                }
                else if (objData is short)
                {
                    sbData.Append(((short)objData).ToString(numFormat) + ",");
                }
                else if (objData is int)
                {
                    sbData.Append(((int)objData).ToString(numFormat) + ",");
                }
                else if (objData is long)
                {
                    sbData.Append(((long)objData).ToString(numFormat) + ",");
                }
                else if (objData is ushort)
                {
                    sbData.Append(((ushort)objData).ToString(numFormat) + ",");
                }
                else if (objData is uint)
                {
                    sbData.Append(((uint)objData).ToString(numFormat) + ",");
                }
                else if (objData is ulong)
                {
                    sbData.Append(((ulong)objData).ToString(numFormat) + ",");
                }
                else if (objData is double)
                {
                    sbData.Append(((double)objData).ToString(numFormat) + ",");
                }
                else if (objData is decimal)
                {
                    sbData.Append(((decimal)objData).ToString(numFormat) + ",");
                }
                else if (objData is float)
                {
                    sbData.Append(((float)objData).ToString(numFormat) + ",");
                }
                else if (objData is byte)
                {
                    sbData.Append(((byte)objData).ToString(numFormat) + ",");
                }
                else if (objData is sbyte)
                {
                    sbData.Append(((sbyte)objData).ToString(numFormat) + ",");
                }
                else if (objData is TimeSpan)
                {
                    sbData.Append("'" + ((TimeSpan)objData).Hours.ToString().PadLeft(2, '0') + ":" + ((TimeSpan)objData).Minutes.ToString().PadLeft(2, '0') + ":" + ((TimeSpan)objData).Seconds.ToString().PadLeft(2, '0') + "',");
                }
                else if (objData is MySql.Data.Types.MySqlDateTime)
                {
                    if (((MySql.Data.Types.MySqlDateTime)objData).IsNull)
                    {
                        sbData.Append("NULL,");
                    }
                    else
                    {
                        string dataType = table.fieldTypes[drData.GetName(i)];

                        if (((MySql.Data.Types.MySqlDateTime)objData).IsValidDateTime)
                        {
                            DateTime dtime = ((MySql.Data.Types.MySqlDateTime)objData).Value;

                            if (dataType == "datetime")
                                sbData.Append("'" + dtime.ToString("yyyy-MM-dd HH:mm:ss", dateFormat) + "',");
                            else 
                            if (dataType == "date")
                               sbData.Append("'" + dtime.ToString("yyyy-MM-dd", dateFormat) + "',");
                            else 
                            if (dataType == "time")
                                sbData.Append("'" + dtime.ToString("HH:mm:ss", dateFormat) + "',");
                        }
                        else
                        {
                            sbData.Append("null,");
                        }
                    }
                }
                else if (objData is System.Guid)
                {
                    string dataType = table.fieldTypes[drData.GetName(i)];

                    if (dataType == "binary(16)")
                    {
                        sbData.Append("'" + GetBLOBfromData(((Guid)objData).ToByteArray()) + "',");
                    }
                    else if (dataType == "char(36)")
                    {
                        sbData.Append("'" + objData + "',");
                    }
                }
                else
                {
                    throw new Exception("Unhandled data type. Current processing data type: " + objData.GetType().ToString() + ". Please report this bug with this message to the development team.");
                }
            }

            sbData.Remove(sbData.Length - 1, 1);
            sbData.Append(")");
            return sbData.ToString();
        }

        public void EscapeString(ref string strg)
        {
            var builder = new StringBuilder();

            Dictionary<char, string> lstEscape = new Dictionary<char, string>()
            {
                {'\\', "\\\\"},
                {'\r', "\\r"},
                {'\n', "\\n"},
                {'\a', ""},
                {'\b', ""},
                {'\f', ""},
                {'\t', "\\t"},
                {'\v', ""},
                {'\'', "\\'"}
            };
            strg = strg.Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\t", "\t");
            foreach (var ch in strg)
            {
                if (ch != 0x00)
                {
                    if (lstEscape.ContainsKey(ch))
                        builder.Append(lstEscape[ch]);
                    else
                        builder.Append(ch);
                }
            }
            strg = builder.ToString();
        }

        public string GetBLOBfromData(byte[] byteData)
        {
            char[] arrC = new char[byteData.Length * 2];
            byte bytB;
            for (int i1 = 0, i2=0; i2 < byteData.Length; i1++,i2++)
            {
                bytB = ((byte)(byteData[i2] >> 4));
                arrC[i1] = (char)(bytB > 9 ? bytB + 0x37 : bytB + 0x30);
                i1++;
                bytB = ((byte)(byteData[i2] & 0xF));
                arrC[i1] = (char)(bytB > 9 ? bytB + 0x37 : bytB + 0x30);
            }
            return "decode('" + (new string(arrC)) + "', 'hex')" ;
        }


    }
}
