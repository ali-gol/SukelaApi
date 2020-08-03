
using SukelaApi.Library;
using SukelaApi.Service.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SukelaApi.Service
{
    public class LinkService
    {
        private readonly string _cookieString;
        public LinkService(string cookieString)
        {
            _cookieString = cookieString;
        }
        public async Task<List<Entry>> GetDetail(string url)
        {
            var client = GetClient(url);
            client.SetCookieString(_cookieString);
            var html = await client.DownloadStringTaskAsync();
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            var entries = doc
                .GetElementbyId("entry-item-list")
                .SelectNodes("li")
                .Select(c => new Entry
                {
                    Id = long.Parse(c.Attributes["data-id"].Value),
                    Content = c.ChildNodes[1].InnerHtml,
                    EntryDate = c.SelectSingleNode("footer[1]/div[2]/a[1]").InnerText,
                    EntryUrl = $"https://eksisozluk.com/{c.SelectSingleNode("footer[1]/div[2]/a[1]").Attributes["href"].Value}",
                    Author = c.SelectSingleNode("footer[1]/div[2]/a[1]").InnerText,
                    AuthorUrl = $"https://eksisozluk.com/{c.SelectSingleNode("footer[1]/div[2]/a[2]").Attributes["href"].Value}",

                }).ToList();
            return entries;
        }

        private CustomWebClient GetClient(string url)
        {
            var client = new CustomWebClient(url);
            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36");
            client.Headers.Add("origin", "https://eksisozluk.com");
            client.Headers.Add("upgrade-insecure-requests", "1");
            client.Headers.Add("x-requested-with", "XMLHttpRequest");
            return client;
        }
    }
}
