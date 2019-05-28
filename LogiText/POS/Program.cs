using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            LogiText.Data.DBManager dB = new LogiText.Data.DBManager(connStr);
            dB.CreateTable("TableNew3", "col1 int");
        }
    }
}
