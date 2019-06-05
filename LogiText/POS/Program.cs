using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;
using LogiText.Data;

/*
.NET Framework 4.7.2
*/

namespace POS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            string connString = Helper.ConnVal("AwsTestDB");

            /*
            bookTest.name = "test";
            bookTest.ISBN10 = "0000000000";
            bookTest.ISBN13 = "9870000000";
            bookTest.ASIN = "00000";
            bookTest.price = "9.99";
            bookTest.imgURL = "https://test.com";
            bookTest.used_prices = "10";
            bookTest.new_prices = "12";
            



            
            List<string> tables = sql.ListTables();
            foreach (string t in tables)
            {
                Console.WriteLine(t);
            }
            */

            // FIX :: '' AROUND DATA
            

            Data.MySql sql = new Data.MySql(connString);
            //sql.InsertRecord("Test3", "name, test", "'testie', 'testie1'");

            //Book bookTest = sql.GetBook("Test3", "test", "testie1");
            DataTable t = sql.ReadAll("Test1", 10);

        }
    }
}
