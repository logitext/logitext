using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;

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

            Data.MySql sql = new Data.MySql(connString);
            //sql.CreateTable("Test1", "name char(50), ISBN10 char(50), ISBN13 char(50), ASIN char(50), price char(50), imgURL char(255)");
            //sql.DeleteTable("Test1");
            DataTable data = sql.ReadColumns("Test1");
            
            
            foreach (DataRow row in data.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    Console.Write(item);
                }
                Console.Write("\n");
            }
           
        }
    }
}
