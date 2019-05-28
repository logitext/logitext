using System;
using LogiText.Data;

namespace Scraper
{
    public class Amazon
    {
        public static Book scrapeISBN(int ISBN)
        {
            Book r = new Book();

            r.name = "A Clash of Kings";
            r.ISBN = 0553108034;
            r.price = 22.99f;
            r.url = "https://www.amazon.com/Clash-Kings-Song-Fire-Book/dp/0553108034/ref=tmm_hrd_swatch_0?_encoding=UTF8&qid=1559015792&sr=8-2";
            r.imgURL = "https://images-na.ssl-images-amazon.com/images/I/51cRzw2Kj7L._SX325_BO1,204,203,200_.jpg";

            return r;
        }
    }
}
