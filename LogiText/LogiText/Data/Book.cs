using System;
using System.Collections.Generic;
using System.Text;

namespace LogiText.Data
{
    public class Book
    {
        public static string[] fieldNames = {
            "name",
            "ISBN",
            "price",
            "url",
            "imgURL"
        };

        public string name   { get; set; }
        public int    ISBN   { get; set; }
        public float  price  { get; set; }
        public string url    { get; set; }
        public string imgURL { get; set; }

        public Book() { }
    }
}
