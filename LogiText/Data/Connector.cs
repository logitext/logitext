using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

/*
.NET Standard 2.0
*/

namespace Data
{
    public class Connector
    {
        /* 
        Acts as as container for the connection so that special cases 
        and different databases can be handled
        */
        public string connStr { get; set; }
        public Connector(string connVal) { connStr = connVal; }
    }
}
