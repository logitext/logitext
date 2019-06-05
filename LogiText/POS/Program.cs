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

            /*
            IMPORTANT :: FIGURE OUT WHAT TO DO ABOUT INSERTING DUPLICATES 
                         AND INSERTING A BOOK CLASS WITH A DIFFERENT SCHEMA THAN THE TABLE
            */

            string connString = Helper.ConnVal("AwsTestDB");

            Book bookTest = new Book();
            bookTest.name = "test";
            bookTest.ISBN10 = "0000000000";
            bookTest.ISBN13 = "3333";
            bookTest.ASIN = "00000";
            bookTest.price = "9.99";
            bookTest.imgURL = "https://test.com";
            bookTest.used_prices = "10";
            bookTest.new_prices = "12";


            Book bookTest1 = new Book();
            bookTest1.name = "test1";
            bookTest1.ISBN10 = "6666666";
            bookTest1.ISBN13 = "5";
            bookTest1.ASIN = "11111";
            bookTest1.price = "9.99";
            bookTest1.imgURL = "https://test500.com";
            bookTest1.used_prices = "10";
            bookTest1.new_prices = "15";


            Data.MySql sql = new Data.MySql(connString);
            //sql.InsertRecord("Test3", "name, test", "'testie', 'testie1'");
            //sql.InsertBook(bookTest1, "Test2");

            List<Book> books = new List<Book>();
            books.Add(bookTest);
            books.Add(bookTest1);

            sql.InsertBookList(books, "Test2");
            //sql.InsertBook(bookTest1, "Test2");
            // DataTable t = sql.ReadAll("Test1", 10);

        }
    }
}
