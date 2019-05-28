using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using System.Data.SqlClient;

namespace LogiText.Data
{
    public class DBManager
    {
        public string connStr { get; set; }

        public DBManager(string connVal) { connStr = connVal; }
        
        public void CreateTable(string tableName, string columns)
        {
            string sqlCommand = String.Format("CREATE TABLE {0} ({1})", tableName, columns);

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
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

        public void DeleteTable(string tableName)
        {
            string sqlCommand = String.Format("DROP TABLE {0}", tableName);

            try
            {
                using (SqlConnection con)
            }
        }
    }
}
