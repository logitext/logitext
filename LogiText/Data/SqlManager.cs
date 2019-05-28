using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

/*
.NET Standard 2.0
*/

namespace Data
{
    public class SqlManager
    {
        public Connector connection { get; set; }
        public SqlManager(Connector connVal) { connection = connVal; }
        void NonQuery(string sqlCommand)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connection.connStr))
                using (var cmd = new SqlDataAdapter())
                using (var insertCommand = new SqlCommand(sqlCommand))
                {
                    insertCommand.Connection = conn;
                    cmd.InsertCommand = insertCommand;
                    conn.Open();
                    cmd.InsertCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void CreateTable(string tableName, string columns)
        {
            string sqlCommand = String.Format("CREATE TABLE {0} ({1})", tableName, columns);

            NonQuery(sqlCommand);

        }

        public void DeleteTable(string tableName)
        {
            string sqlCommand = String.Format("DROP TABLE {0}", tableName);

            NonQuery(sqlCommand);
        }

        public void InsertData(string tableName, string columns, string data)
        {
            string sqlCommand = String.Format("INSERT INTO {0} ({1}) VALUES ({2})", tableName, columns, data);

            NonQuery(sqlCommand);
        }
    }
}
