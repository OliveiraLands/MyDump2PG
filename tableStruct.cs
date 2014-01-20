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
using MySql.Data.MySqlClient;
using System.Data;

namespace MyDump2PG
{
    public class tableStruct
    {
        public string tableName;
        public string tableCreateSQL;
        public string tableSelectAll;
        public string tableInsertSQL;
        public string fieldDelimiter = "\"";
        public string DBEngine = "";
        public string DBCollate = "";

        private bool m_keepFieldCase = true;

        public bool keepFieldCase {
            get { return m_keepFieldCase; }
            set {
                fieldDelimiter = (value ? "\"" : "\"");
                m_keepFieldCase = value;
            }
        }

        public long rowCount = 0;
        public List<string> fieldNames = new List<string>();
        public Dictionary<string,string> fieldTypes = new Dictionary<string,string>();
        public Dictionary<string, int> fieldSizes = new Dictionary<string, int>();
        public Dictionary<string, string> fieldCharSet = new Dictionary<string, string>();

        private string m_connectionString = "";

        public tableStruct(string databaseName, string name, string connectionString, bool keepCase, string tbEngine, string tbCollate)
        {
            tableName = name;
            m_connectionString = connectionString;
            keepFieldCase = keepCase;
            DBEngine = tbEngine;
            DBCollate = tbCollate;

            tableSelectAll = String.Format("Select * from `{0}`;", tableName);

            tableCreateSQL = GetScriptCreateTablePG(databaseName, tableName);

            rowCount = GetTotalRows();
        }

        public string fname(string fieldName)
        {
            if (m_keepFieldCase)
                return fieldDelimiter + fieldName + fieldDelimiter;
            else
                return fieldDelimiter + fieldName.ToLower() + fieldDelimiter;
        }

        public long GetTotalRows()
        {
            using (MySqlConnection oConn = new MySqlConnection(m_connectionString))
            using (MySqlCommand oComm = new MySqlCommand())
            {
                oComm.Connection = oConn;
                oComm.Connection.Open();
                oComm.CommandTimeout = Schema2Script.sqlTimeOut;

                oComm.CommandText = String.Format("Select count(*) from `{0}`;", tableName);
                rowCount = (long)oComm.ExecuteScalar();
            }

            return rowCount;
        }

        public string GetScriptCreateTablePG(string databaseName, string tableName)
        {
            StringBuilder sbCreateTable = new StringBuilder();

            tableSelectAll = "Select ";

            using (MySqlConnection oConn = new MySqlConnection(m_connectionString))
            using (MySqlCommand oComm = new MySqlCommand())
            {
                oConn.Open();
                oComm.Connection = oConn;
                oComm.CommandText = @"select * 
                                        from information_schema.columns
                                       where table_schema = '" + databaseName + @"'
                                         and table_name = '" + tableName + @"'
                                ";

                MySqlDataAdapter da = new MySqlDataAdapter(oComm);
                DataTable dt = new DataTable();
                da.Fill(dt);
                da = null;

                Dictionary<string, string> my2pgf = new Dictionary<string, string>()
                {
                    { "bigint", "bigint"},
                    { "blob", "bytea" },
                    { "char", "char" },
                    { "datetime", "timestamp" },
                    { "decimal", "numeric" },
                    { "double", "double precision" },
                    { "enum",   "varchar" },
                    { "float", "real" },
                    { "int", "integer" },
                    { "longblob", "bytea" },
                    { "longtext", "text" },
                    { "mediumblob", "bytea" },
                    { "mediumtext", "text" },
                    { "set", "varchar" },
                    { "smallint", "smallint" },
                    { "text", "text" },
                    { "time", "time" },
                    { "timestamp", "timestamp" },
                    { "tinyint", "smallint" },
                    { "varchar", "varchar"}
                };

                sbCreateTable.AppendLine(String.Format("Create table if not exists {0} ", fname(tableName) ));
                sbCreateTable.AppendLine(" (");
                int recCount = 0;
                string primKey = "";

                tableInsertSQL = "insert into " + fname(tableName) + " ( ";

                string sComma = "", sCommaSel = "", sCommaPK = "";
                foreach (DataRow dr in dt.Rows)
                {
                    string fieldName = dr["COLUMN_NAME"].ToString();
                    string fieldDef = dr["COLUMN_DEFAULT"].ToString();
                    bool fieldIsNull = dr["IS_NULLABLE"].ToString() == "YES";
                    string fieldType = dr["DATA_TYPE"].ToString();
                    string fieldLen = (dr["CHARACTER_MAXIMUM_LENGTH"] is DBNull ? "" : dr["CHARACTER_MAXIMUM_LENGTH"].ToString());
                    string numPrecision = (dr["NUMERIC_PRECISION"] is DBNull ? "" : dr["NUMERIC_PRECISION"].ToString());
                    string numScale = (dr["NUMERIC_SCALE"] is DBNull ? "" : dr["NUMERIC_SCALE"].ToString());
                    string keyType = dr["COLUMN_KEY"].ToString();
                    string charset = dr["CHARACTER_SET_NAME"].ToString();
                    string extra = dr["EXTRA"].ToString();

                    string pgtype = my2pgf[fieldType];

                    if (fieldType=="int" && extra == "auto_increment") {
                        pgtype = "serial";
                    }

                    if ((string)dr["COLUMN_TYPE"] == "tinyint(1)")
                    {
                        pgtype = "boolean";
                        fieldDef = (fieldDef == "1" ? "true" : "false");
                    }

                    if ("char,varchar,".Contains(fieldType + ","))
                    {
                        fieldLen = Convert.ToInt32(fieldLen).ToString();
                        fieldSizes.Add(fieldName, Convert.ToInt32(fieldLen));
                        fieldCharSet.Add(fieldName, charset);

                        if (charset == "latin1")
                        {
                            tableSelectAll += sCommaSel + String.Format("CONVERT(`{0}` USING utf8) as `{0}`", fieldName);
                        }
                        else
                            tableSelectAll += sCommaSel + String.Format("`{0}`", fieldName);
                    }
                    else
                    if ("longtext,text,".Contains(fieldType + ","))
                    {
                        fieldCharSet.Add(fieldName, charset);

                        if (charset == "latin1")
                        {
                            tableSelectAll += sCommaSel + String.Format("CONVERT(`{0}` USING latin1) as `{0}`", fieldName);
                        }
                        else
                            tableSelectAll += sCommaSel + String.Format("Convert(`{0}` using utf8) as `{0}`", fieldName);
                    }
                    else
                    {
                        tableSelectAll += sCommaSel + "`" + fieldName + "`";
                    }

                    if (keyType == "PRI") {
                        primKey += sCommaPK + fname(fieldName);
                        sCommaPK = ",";
                    }

                    fieldNames.Add(fieldName);
                    fieldTypes.Add(fieldName, fieldType);

                    tableInsertSQL += sComma + fname(fieldName) + " ";
                    sComma = ",";
                    sCommaSel = ",";

                    sbCreateTable.AppendLine("   " + fname(fieldName) + " " + pgtype +
                        ("char,varchar,decimal,".Contains(fieldType + ",") ? "(" + (fieldType == "decimal" ? numPrecision : fieldLen) +
                        (fieldType == "decimal" ? "," + numScale : "") + ") " : "") +
                        (fieldIsNull ? "" : " not null ") +
                        (fieldDef == "" ? "" : " default " +
                        ("char,varchar,".Contains(fieldType + ",") ? String.Format("'{0}'", fieldDef) :
                         (fieldType == "timestamp" || fieldType == "datetime" ? "now()" : fieldDef)
                        )) +
                        (++recCount == dt.Rows.Count ? "" : ",")
                        );

                }
                tableInsertSQL += ") values ";

                if (primKey != "")
                {
                    sbCreateTable.AppendLine(String.Format("   , Primary Key ({0}) ", primKey));
                }
                sbCreateTable.AppendLine(" );");
            }

            tableSelectAll += " from `" + tableName + "`;";

            return sbCreateTable.ToString();
        }

    }
}
