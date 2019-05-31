using System;
using System.Collections.Generic;
using System.Text;

namespace LogiText.Data
{
    public class Book
    {
        public static string[] fieldNames = {
            "name",
            "ISBN-10",
            "ISBN-13",
            "ASIN",
            "price",
            "url",
            "imgURL"
        };

        Dictionary<string, string> data;

        public Book()
        {
            data = new Dictionary<string, string>();

            foreach (string field in fieldNames)
            {
                data[field] = "";
            }
        }

        void setField(string fieldname, string value)
        {
            data[fieldname] = value;
        }

        // Added - JO
        //public string [] fieldName { get; set; }
        public string name   { get => data["name"];   set => setField("name",   value); }
        public string ISBN10 { get => data["ISBN10"]; set => setField("ISBN10", value); }
        public string ISBN13 { get => data["ISBN13"]; set => setField("ISBN13", value); }
        public string ASIN   { get => data["ASIN"];   set => setField("ASIN",   value); }
        public string price  { get => data["price"];  set => setField("price",  value); }
        public string imgURL { get => data["imgURL"]; set => setField("imgURL", value); }
    }
}
