using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;
using LogiText.Data;

namespace Data
{
    public abstract class DbManager
    {
        public Dictionary<string, string> queryStrings;

        public bool connSuccessful { get; set; }

        protected abstract void SetDict();

        protected abstract DataTable Query(string queryString);

        protected abstract bool NonQuery(string queryString);

        public bool DropTable(string tableName)
        {
            string commandString = String.Format(queryStrings["drop"], tableName);
            
            if (NonQuery(commandString))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public abstract List<string> ListTables();

        public bool CreateTable(string tableName, string columns)
        {
            string commandString = String.Format(queryStrings["create"], tableName, columns);

            if (NonQuery(commandString))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool InsertRecord(string tableName, string columns, string data)
        {
            string commandString = String.Format(queryStrings["insert"], tableName, columns, data);

            if (NonQuery(commandString))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataTable ReadAll(string tableName, int limit = 0, string columns = "*")
        {
            string commandString = String.Format(queryStrings["selectAll"], columns, tableName, limit.ToString());

            return Query(commandString);
        }

        public DataTable ReadWhere(string tableName, string valueColumn, string value, string selectColumn = "*")
        {
            string commandString = String.Format(queryStrings["selectWhere"], selectColumn, tableName, valueColumn, value);

            return Query(commandString);
        }

        public bool RecordInTable(string tableName, string valueColumn, string value)
        {
            DataTable data = ReadWhere(tableName, valueColumn, value);

            if (data.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<string> ColumnNames(string tableName)
        {
            string commandString = String.Format(queryStrings["columnNames"], tableName);

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

        public List<string> ColumnInfo(string tableName)
        {
            string commandString = String.Format(queryStrings["columnInfo"], tableName);

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

        public bool InsertBook(Book book, string tableName)
        {
            List<string> columns = ColumnInfo(tableName);
            
            /*
            CHECK IF EACH ITEM IN fieldNames IS IN columns LIST
            THEN ADD NAME TO sqlStringCols
            */

            if (BookInTable(book, tableName))
            {
                Console.WriteLine("{0} already exists in database", book.data["ISBN13"]);
                return true;
            }
            else
            {
                string sqlStringCols = "";
                string sqlStringVal = "";

                for (int i = 0; i < Book.fieldNames.Length; i++)
                {
                    string field = Book.fieldNames[i];
                    string value = book.data[field];

                    int nameIndex = columns.FindIndex(a => a.Contains(field));

                    // Returns false if the field name is not in column list
                    if (nameIndex == -1)
                    {
                        Console.WriteLine("{0} does not match column schema", book.ISBN13);
                        return false;
                    }
                    else
                    {
                        int typeIndex = nameIndex + 1;

                        string colString = String.Format("{0}", field);
                        string valString = String.Format("'{0}'", value);

                        if (i != Book.fieldNames.Length - 1)
                        {
                            colString += ", ";
                            valString += ", ";
                        }

                        sqlStringCols += colString;
                        sqlStringVal += valString;
                    }
                }

                try
                {
                    InsertRecord(tableName, sqlStringCols, sqlStringVal);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool BookInTable(Book book, string tableName)
        {
            string field = "ISBN13";
            string value = book.data[field];
            
            if (RecordInTable(tableName, field, value))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public Book GetBook(string tableName, string field, string identifier)
        {
            if (RecordInTable(tableName, field, identifier))
            {
                DataTable rowCheck = ReadWhere(tableName, field, identifier);

                if (rowCheck.Rows.Count > 1)
                {
                    Console.WriteLine("Note: There is more than one record with identifier {0}", identifier);
                }

                Book newBook = new Book();

                List<string> columns = ColumnNames(tableName);
                List<string> fieldNames = new List<string>(Book.fieldNames);

                foreach (string column in columns)
                {
                    if (fieldNames.Contains(column))
                    {
                        DataTable data = ReadWhere(tableName, field, identifier, column);
                        string value = data.Rows[0][0].ToString();
                        newBook.data[column] = value;
                    }
                }

                return newBook;
            }
            else
            {
                Console.WriteLine("{0} could not be found in {1}", identifier, tableName);
                return new Book();
            }
        }

        public int RowCount(string tableName)
        {
            string commandString = String.Format(queryStrings["count"], tableName);

            DataTable rowQuery = Query(commandString);

            return Convert.ToInt32(rowQuery.Rows[0][0]);
        }
    }

    public class MySql : DbManager
    {
        protected MySqlConnection conn { get; set; }

        public MySql(string connString)
        {
            SetDict();

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

        protected override void SetDict()
        {
            queryStrings = new Dictionary<string, string>
            {
                ["drop"] = "DROP TABLE IF EXISTS {0};",
                ["create"] = "CREATE TABLE IF NOT EXISTS {0} ({1});",
                ["insert"] = "INSERT INTO {0} ({1}) VALUES ({2});",
                ["selectAll"] = "SELECT {0} FROM {1} LIMIT 0, {2};",
                ["selectWhere"] = "SELECT {0} FROM {1} WHERE {2} = {3};",
                ["columnNames"] = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}';",
                ["columnInfo"] = "SELECT COLUMN_NAME, COLUMN_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}';",
                ["exists"] = "SELECT * FROM {1} WHERE {2} = {3};",
                ["count"] = "SELECT COUNT(*) FROM {0};"
            };
        }

        protected override DataTable Query(string queryString)
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
                            // Console.WriteLine("No rows found.");
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

        protected override bool NonQuery(string queryString)
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
                        return true;
                    }
                    catch (Exception ex)
                    {
                        //log and/or rethrow or ignore
                        Console.WriteLine(ex);
                        return false;
                    }
                }
            }
            else
            {
                Console.WriteLine("No Successful Connection Established");
                return false;
            }
        }

        public override List<string> ListTables()
        {
            //DataTable tables = new DataTable();

            List<string> tableList = new List<string>();

            if (connSuccessful)
            {
                using (conn)
                {
                    try
                    {
                        conn.Open();
                        DataTable tables = new DataTable();
                        tables = conn.GetSchema("Tables");

                        foreach (DataRow row in tables.Rows)
                        {
                            string tablename = (string)row[2];
                            tableList.Add(tablename);
                        }

                        return tableList;
                    }
                    catch (Exception ex)
                    {
                        //log and/or rethrow or ignore
                        return tableList;
                    }
                }
            }
            else
            {
                Console.WriteLine("No Successful Connection Found");
                return tableList;
            }
        }
    }    

    public class Sql : DbManager
    {
        protected SqlConnection conn { get; set; }

        public Sql(string connString)
        {
            SetDict();

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

        protected override void SetDict()
        {
            queryStrings = new Dictionary<string, string>
            {
                ["drop"] = "DROP TABLE IF EXISTS {0};",
                ["create"] = "CREATE TABLE IF NOT EXISTS {0} ({1});",
                ["insert"] = "INSERT INTO {0} ({1}) VALUES ({2});",
                ["selectAll"] = "SELECT {0} FROM {1};",
                ["selectWhere"] = "SELECT {0} FROM {1} WHERE {2} = {3};",
                ["columnNames"] = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}';",
                ["columnInfo"] = "SELECT COLUMN_NAME, COLUMN_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}';",
                ["exists"] = "SELECT * FROM {1} WHERE {2} = {3};",
                ["count"] = "SELECT COUNT(*) FROM {0};"
            };
        }

        protected override DataTable Query(string queryString)
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
                            // Console.WriteLine("No rows found.");
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

        protected override bool NonQuery(string queryString)
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
                        return true;
                    }
                    catch (Exception ex)
                    {
                        //log and/or rethrow or ignore
                        Console.WriteLine(ex);
                        return false;
                    }
                }
            }
            else
            {
                Console.WriteLine("No Successful Connection Established");
                return false;
            }
        }

        public override List<string> ListTables()
        {
            List<string> tableList = new List<string>();

            if (connSuccessful)
            {
                using (conn)
                {
                    try
                    {
                        conn.Open();
                        DataTable tables = new DataTable();
                        tables = conn.GetSchema("Tables");

                        foreach (DataRow row in tables.Rows)
                        {
                            string tablename = (string)row[2];
                            tableList.Add(tablename);
                        }

                        return tableList;
                    }
                    catch (Exception ex)
                    {
                        //log and/or rethrow or ignore
                        return tableList;
                    }
                }
            }
            else
            {
                Console.WriteLine("No Successful Connection Found");
                return tableList;
            }
        }
    }
}
