using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            string connStr = Helper.ConnVal("TestDB");
            Data.Connector connection = new Data.Connector(Helper.ConnVal("TestDB"));
            Data.SqlManager dB = new Data.SqlManager(connection);
            dB.DeleteTable("Test");

            /*
            | ----- CREATE BOOK CLASS FOR QUERYING AND INSERTING ----- |
            */
        }
    }
}
