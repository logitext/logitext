using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;
using LogiText.Data;

namespace Data
{
    public interface IDataBase
    {
        void Version();

        DataTable ListTables();

        void CreateTable(string tableName, string columns);

        void DeleteTable(string tableName);

        void InsertRecord(string tableName, string columns, string data);

        DataTable ReadData(string tableName, string columns);

        DataTable ReadColumns(string tableName);

        /*
        Next Fuctionality:
        - void InsertBook(LogiText.Book record);
        - LogiText.Book RetrieveBook(identifier);
        - void InsertDataTable(DataTable data);
        */
    }


    public class MySql : IDataBase
    {
        MySqlConnection conn { get; set; }

        public bool connSuccessful { get; set; }

        public MySql(string connString)
        {
            try
            {
                conn = new MySqlConnection(connString);
                conn.Open();
                connSuccessful = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
                connSuccessful = false;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        
        public void Version()
        {
            if (connSuccessful)
            {
                conn.Open();
                Console.WriteLine("MySQL Version : {0}", conn.ServerVersion);
                conn.Close();
            }
            else
            {
                Console.WriteLine("Unsuccessful Connection");
            }
        }

        private DataTable Query(string queryString)
        {
            DataTable queryData = new DataTable();

            if (connSuccessful)
            {
                using (conn)
                {
                    try
                    {
                        MySqlCommand command = new MySqlCommand(queryString, conn);
                        command.Connection.Open();
                        MySqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            queryData.Load(reader);
                            reader.Close();
                            return queryData;
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                            reader.Close();
                            return queryData;
                        }
                    }
                    catch (Exception ex)
                    {
                        //log and/or rethrow or ignore
                        Console.WriteLine(ex);
                        return queryData;
                    }
                }
            }
            else
            {
                Console.WriteLine("No Successful Connection Established");
                return queryData;
            }
        }

        private void NonQuery(string queryString)
        {
            if (connSuccessful)
            {
                using (conn)
                {
                    try
                    {
                        MySqlCommand command = new MySqlCommand(queryString, conn);
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        //log and/or rethrow or ignore
                        Console.WriteLine(ex);
                    }
                }
            }
            else
            {
                Console.WriteLine("No Successful Connection Established");
            }
        }

        public DataTable ListTables()
        {
            DataTable tables = new DataTable();

            if (connSuccessful)
            {
                using (conn)
                {
                    try
                    {
                        conn.Open();
                        tables = conn.GetSchema("Tables");
                        return tables;
                    }
                    catch (Exception ex)
                    {
                        //log and/or rethrow or ignore
                        return tables;
                    }
                }
            }
            else
            {
                Console.WriteLine("No Successful Connection Found");
                return tables;
            }
        }

        public void CreateTable(string tableName, string columns)
        {
            string commandString = String.Format("CREATE TABLE IF NOT EXISTS {0} ({1});", tableName, columns);

            NonQuery(commandString);
        }

        public void DeleteTable(string tableName)
        {
            string commandString = String.Format("DROP TABLE IF EXISTS {0};", tableName);

            NonQuery(commandString);
        }

        public void InsertRecord(string tableName, string columns, string data)
        {
            string commandString = String.Format("INSERT INTO {0} ({1}) VALUES ({2});", tableName, columns, data);

            NonQuery(commandString);
        }

        public DataTable ReadData(string tableName, string columns)
        {
            string commandString = String.Format("SELECT {0} FROM {1};", columns, tableName);

            return Query(commandString);
        }

        public List<string> ReadColumns(string tableName)
        {
            string commandString = String.Format(@"SELECT COLUMN_NAME 
                                                    FROM INFORMATION_SCHEMA.COLUMNS
                                                    WHERE TABLE_NAME = '{0}';", tableName);

            DataTable cols = Query(commandString);

            List<string> colsList = new List<string>();

            foreach (DataRow row in cols.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    colsList.Add(item.ToString());
                }
            }

            return colsList;
        }

        public void InsertBook(Book book, string tableName)
        {
            List<string> columns = ReadColumns(tableName);

            string sqlStringCols;

            for (int i = 0; i < Book.fieldNames.Length; i++)
            {
                if (columns.Contains(Book.fieldNames[i])) ;
            }
        }
    }

    public class Sql : IDataBase
    {
        SqlConnection conn { get; set; }

        public bool connSuccessful { get; set; }

        public Sql(string connString)
        {
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();
                connSuccessful = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
                connSuccessful = false;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        public void Version()
        {
            if (connSuccessful)
            {
                conn.Open();
                Console.WriteLine("MySQL Version : {0}", conn.ServerVersion);
                conn.Close();
            }
            else
            {
                Console.WriteLine("Unsuccessful Connection");
            }
        }

        private DataTable Query(string queryString)
        {
            DataTable queryData = new DataTable();

            if (connSuccessful)
            {
                using (conn)
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(queryString, conn);
                        command.Connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            queryData.Load(reader);
                            reader.Close();
                            return queryData;
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                            reader.Close();
                            return queryData;
                        }
                    }
                    catch (Exception ex)
                    {
                        //log and/or rethrow or ignore
                        Console.WriteLine(ex);
                        return queryData;
                    }
                }
            }
            else
            {
                Console.WriteLine("No Successful Connection Established");
                return queryData;
            }
        }

        private void NonQuery(string queryString)
        {
            if (connSuccessful)
            {
                using (conn)
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(queryString, conn);
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        //log and/or rethrow or ignore
                        Console.WriteLine(ex);
                    }
                }
            }
            else
            {
                Console.WriteLine("No Successful Connection Established");
            }
        }

        public DataTable ListTables()
        {
            DataTable tables = new DataTable();

            if (connSuccessful)
            {
                using (conn)
                {
                    try
                    {
                        conn.Open();
                        tables = conn.GetSchema("Tables");
                        return tables;
                    }
                    catch (Exception ex)
                    {
                        //log and/or rethrow or ignore
                        return tables;
                    }
                }
            }
            else
            {
                Console.WriteLine("No Successful Connection Found");
                return tables;
            }
        }

        public void CreateTable(string tableName, string columns)
        {
            string commandString = String.Format("CREATE TABLE {0} ({1})", tableName, columns);

            NonQuery(commandString);
        }

        public void DeleteTable(string tableName)
        {
            string commandString = String.Format("DROP TABLE {0};", tableName);

            NonQuery(commandString);
        }

        public void InsertRecord(string tableName, string columns, string data)
        {
            string commandString = String.Format("INSERT INTO {0} ({1}) VALUES ({2});", tableName, columns, data);

            NonQuery(commandString);
        }

        public DataTable ReadData(string tableName, string columns)
        {
            string commandString = String.Format("SELECT {0} FROM {1};", columns, tableName);

            return Query(commandString);
        }

        public DataTable ReadColumns(string tableName)
        {
            string commandString = String.Format(@"SELECT COLUMN_NAME 
                                                    FROM INFORMATION_SCHEMA.COLUMNS
                                                    WHERE TABLE_NAME = '{0}';", tableName);

            return Query(commandString);
        }
    }
}
