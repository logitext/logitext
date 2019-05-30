using System;
using System.Collections.Generic;
using System.Text;
using LogiText.Data;

namespace Scraper
{
    public interface IWebScraper
    {
        string getPage(string url);
        string scrapeTitle(string page);
        string scrapeImageLocation(string page);
        Book getBook(string ISBN, string page);
    }
}
