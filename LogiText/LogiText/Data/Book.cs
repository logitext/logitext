using System;
using System.Collections.Generic;
using System.Text;

namespace LogiText.Data
{
    public class Book
    {
        public static string[] fieldNames = {
            "name",

            "author",
            "edition",
            "binding",
            "publisher",

            "ISBN10",
            "ISBN13",
            "ASIN",

            "list_price",
            "net_price",
            "wholesale_price",
            "book_status",

            "imgURL"
        };

        public Dictionary<string, string> data;

        public Book()
        {
            data = new Dictionary<string, string>();

            foreach (string field in fieldNames)
                data[field] = "";
        }

        public void setField(string fieldname, string value)
        {
            data[fieldname] = value;
        }

        public string name   { get => data["name"];   set => setField("name",   value); }
        public string ISBN10 { get => data["ISBN10"]; set => setField("ISBN10", value); }
        public string ISBN13 { get => data["ISBN13"]; set => setField("ISBN13", value); }
        public string ASIN   { get => data["ASIN"];   set => setField("ASIN",   value); }
        public string imgURL { get => data["imgURL"]; set => setField("imgURL", value); }

        public string list_price { get => data["list_price"]; set => setField("list_price", value); }
        public string net_price  { get => data["net_price"];  set => setField("net_price",  value); }

        public string wholesale_price { get => data["wholesale_price"]; set => setField("wholesale_price", value); }
    }
}
