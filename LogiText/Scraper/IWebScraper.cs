using System;
using System.Collections.Generic;
using System.Text;
using LogiText.Data;

namespace Scraper
{
    public interface IWebScraper
    {
        string getPage(string url);

        void scrapeTitle(string page, ref Book book);
        void scrapeImageLocation(string page, ref Book book);
        void scrapeData(string page, ref Book book);
        void scrapePrices(string page, string refNumber, ref Book book);

        Book getBook(string ISBN, string page);
    }
}
