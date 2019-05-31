using System;
using System.Collections.Generic;
using System.Text;

namespace LogiText.Data
{
    public class Book
    {
        public static string[] fieldNames = {
            "name",
            "ISBN10",
            "ISBN13",
            "ASIN",
            "price",
            "new-prices",
            "used-prices",
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
        public string price  { get => data["price"];  set => setField("price",  value); }
        public string imgURL { get => data["imgURL"]; set => setField("imgURL", value); }
        public string used_prices { get => data["used-prices"]; set => setField("used-prices", value); }
        public string new_prices  { get => data["new-prices"];  set => setField("new-prices",  value); }
    }
}
