using System;
using System.Net;
using LogiText.Data;
using HtmlAgilityPack;

namespace Scraper
{    
    public class AmazonScraper : IWebScraper
    {
        public string getPage(string url)
        {
            WebClient client = new WebClient();

            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246");
            client.Headers.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");

            byte[] data;
            try
            { 
                data = client.DownloadData(url);
            }
            catch (Exception ex)
            {
                return null;
            }

            string r = System.Text.Encoding.Default.GetString(data);

            return r;
        }

        public void scrapeTitle(string page, ref Book book)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(page);

            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//span/@id"))
            {
                string id = node.Id;

                if (id == "productTitle")
                {
                    book.name =  node.InnerText;
                    return;
                }
            }
        }

        public void scrapeImageLocation(string page, ref Book book)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(page);

            // Look for image url
            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div/@id"))
            {
                string id = node.Id;

                if (id == "img-canvas")
                {
                    HtmlNode img = node.ChildNodes[1];
                    string img_url = img.Attributes["data-a-dynamic-image"].Value;
                    img_url = img_url.Remove(0, "&quot;".Length + 1);

                    for (int i = 0; i < img_url.Length - 6; i++)
                        if (img_url.Substring(i, 6) == "&quot;")
                        {
                            img_url = img_url.Substring(0, i);
                            break;
                        }

                    book.imgURL = img_url;
                    return;
                }
            }

        }

        public void scrapeData(string page, ref Book book)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(page);

            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div/@class"))
            {
                if (!node.HasClass("content")) continue;

                foreach (HtmlNode list in node.ChildNodes)
                {
                    if (list.Name != "ul") continue;

                    foreach (HtmlNode item in list.ChildNodes)
                    {
                        if (item.Name != "li") continue;

                        string[] words = item.InnerText.Split(':');
                        string word = words[1].Replace(" ", "").Replace("-", "");

                        switch (words[0].ToLower())
                        {
                            case "isbn-10":
                                if (word.Length > 10) break;
                                book.ISBN10 = word;   break;

                            case "isbn-13":
                                if (word.Length == 10) break;
                                book.ISBN13 = word;    break;

                            case "asin":
                                book.ASIN = word; break;

                            default:
                                break;
                        }
                    }
                }
            }

            /**/ if (book.ISBN10 == "" && book.ISBN13 != "")
                book.ISBN10 = IsbnConverter.ConvertTo10(book.ISBN13);
            else if (book.ISBN13 == "" && book.ISBN10 != "")
                book.ISBN13 = IsbnConverter.ConvertTo13(book.ISBN10);
        }

        public void scrapePrices(string page, string refNumber, ref Book book)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(page);

            bool found = false;
            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div/@class"))
            {
                if (!node.HasClass("inlineBlock-display")) continue;

                foreach (HtmlNode span in node.ChildNodes)
                {
                    if (!span.HasClass("offer-price")) continue;

                    book.list_price = span.InnerText;

                    found = true;
                    break;
                }

                if (found) break;
            }

            //string[] types_url = { "used", "new" };
            //string[] fields = { "used-prices", "new-prices" };

            //for (int i = 0; i < types_url.Length; i++)
            //{ 
                //string url = "https://www.amazon.com/gp/offer-listing/" + refNumber + "/ref=tmm_mmp_used_olp_sr?ie=UTF8&condition=" + types_url[i] + "&qid=&sr=";
                //doc.LoadHtml(getPage(url));

                //foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div/@aria-label"))
                //{
                    //foreach (HtmlNode price in node.ChildNodes)
                    //{
                        //if (!price.HasClass("olpOffer")) continue;


                    //}
                //}
            //}
        }

        public Book getBook(string ISBN, string page)
        {
            Book r = new Book();

            scrapeImageLocation(page, ref r);
            scrapeTitle(page, ref r);
            scrapeData(page, ref r);
            scrapePrices(page, ISBN, ref r);

            return r;
        }
    }
}
