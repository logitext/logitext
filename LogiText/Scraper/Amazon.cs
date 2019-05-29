using System;
using System.Net;
using LogiText.Data;
using HtmlAgilityPack;

namespace Scraper
{
    public class Amazon
    {
        public static string getPage(string url)
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

        public static string scrapeTitle(string page)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(page);

            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//span/@id"))
            {
                string id = node.Id;

                if (id == "productTitle")
                {
                    return node.InnerText;
                }
            }

            return null;
        }

        public static string scrapeImageLocation(string page)
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

                    return img_url;
                }
            }

            return null;
        }

        public static Book scrapeISBN(string ISBN, string page)
        {
            Book r = new Book();

            r.imgURL = scrapeImageLocation(page);
            r.name = scrapeTitle(page);

            r.ISBN = ISBN;
            r.price = 22.99f;

            return r;
        }
    }
}
